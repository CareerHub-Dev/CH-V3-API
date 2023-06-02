using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DP = DocumentFormat.OpenXml.Drawing.Pictures;
using Domain.Entities;
using Application.Common.Interfaces;
using DocumentFormat.OpenXml.Validation;
using System.IO.Abstractions;

namespace Infrastructure.Services
{
    public class CVWordGenerator : ICVWordGenerator
    {
        private readonly IPathService _pathService;
        private readonly IFileSystem _fileSystem;

        public CVWordGenerator(IPathService pathService, IFileSystem fileSystem)
        {
            _pathService = pathService;
            _fileSystem = fileSystem;
        }

        public MemoryStream GenerateDocument(CV cv)
        {
            if (cv.Photo is not null)
            {
                cv.Photo = _fileSystem.Path.Combine(_pathService.GetWebRootPath, cv.Photo);
            }

            var builder = new CvDocumentBuilder(cv);
            var document = builder.BuildDocument();
            var styles = builder.BuildStyles();

            var stream = new MemoryStream();

            using (var wordProcessingDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
            {
                var mainPart = wordProcessingDocument.AddMainDocumentPart();
                mainPart.Document = document;

                if (cv.Photo is not null)
                {
                    var imagePart = mainPart.AddImagePart(ImagePartType.Jpeg, CvDocumentBuilder.PhotoImagePartId);
                    using var photoFileStream = new FileStream(cv.Photo, FileMode.Open, FileAccess.Read, FileShare.Read);
                    imagePart.FeedData(photoFileStream);
                }

                var stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();
                stylePart.Styles = styles;

                var bulletListNumberingPart = mainPart.AddNewPart<NumberingDefinitionsPart>("BulletList");
                bulletListNumberingPart.Numbering = builder.BuildBulletListNumbering();

#if DEBUG
                var validator = new OpenXmlValidator();
                var errors = validator.Validate(wordProcessingDocument);

                foreach (var e in errors)
                {
                    Console.WriteLine($"{e.Part} {e.Path?.PartUri}");
                    Console.WriteLine($"\t{e.Description}");
                }
#endif

            }
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }

    public class CvDocumentBuilder
    {
        private readonly CV _data;
        private readonly CvDocumentLocalizedStrings _strings;
        private static readonly string GraphicDataUri = "http://schemas.openxmlformats.org/drawingml/2006/picture";
        public static readonly string PhotoImagePartId = "PhotoImagePart";

        public CvDocumentBuilder(CV data)
        {
            _data = data;
            _strings = CvDocumentLocalizedStrings.FromTemplateLanguage(data.TemplateLanguage);
        }

        public Document BuildDocument()
        {
            Document document = new()
            {
                Body = new()
            };
            var fullNameParagraph = GetTitleParagraph(_data.FirstName + " " + _data.LastName);
            var goalsParagraph = GetPlainTextParagraph(_data.Goals ?? "");

            var foreignLanguagesTitle = GetSecondaryTitleParagraph(_strings.ForeignLanguages);
            var foreignLanguagesElements = _data.ForeignLanguages
                .Select(element => element.ToString())
                .ToList();
            var workingExperiencesTitle = GetSecondaryTitleParagraph(_strings.Experience);
            var projectsTitle = GetSecondaryTitleParagraph(_strings.ProjectLinks);
            var educationTitle = GetSecondaryTitleParagraph(_strings.Education);

            if (_data.Photo is not null)
            {
                var imageElement = GetDrawingParagraphFromImage(
                    PhotoImagePartId,
                    _data.Photo,
                    _strings.MyPhoto,
                    128,
                    128
                );
                document.Body.AppendChild(imageElement);
            }
            document.Body.AppendChild(fullNameParagraph);
            document.Body.AppendChild(goalsParagraph);
            document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
            if (_data.HardSkills.Count > 0)
            {
                var hardSkillsTitle = GetSecondaryTitleParagraph(_strings.HardSkills);
                var hardSkills = string.Join("    ", _data.HardSkills);
                var hardSkillsParagraph = GetPlainTextParagraph(hardSkills);
                document.Body.AppendChild(hardSkillsTitle);
                document.Body.AppendChild(hardSkillsParagraph);
                document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
            }

            if (_data.SoftSkills.Count > 0)
            {
                var softSkillsTitle = GetSecondaryTitleParagraph(_strings.SoftSkills);
                var softSkills = string.Join("    ", _data.SoftSkills);
                var softSkillsParagraph = GetPlainTextParagraph(softSkills);
                document.Body.AppendChild(softSkillsTitle);
                document.Body.AppendChild(softSkillsParagraph);
                document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
            }

            if (foreignLanguagesElements.Count > 0)
            {
                document.Body.AppendChild(foreignLanguagesTitle);

                var elementParagraphs = GetList(foreignLanguagesElements);
                foreach (var paragraph in elementParagraphs)
                {
                    document.Body.AppendChild(paragraph);
                }
                document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
            }

            if (_data.Experiences.Count > 0)
            {
                document.Body.AppendChild(workingExperiencesTitle);
                document.Body.AppendChild(GetPlainTextParagraph(string.Empty));

                foreach (var workExperience in _data.Experiences)
                {
                    var workExperienceParagraphs = GetWorkExperienceParagraphs(workExperience);
                    foreach (var paragraph in workExperienceParagraphs)
                    {
                        document.Body.AppendChild(paragraph);
                    }
                    document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
                }
                document.Body.AppendChild(GetPlainTextParagraph(string.Empty));
            }

            if (_data.ProjectLinks.Count > 0)
            {
                document.Body.AppendChild(projectsTitle);
                Paragraph paragraph = new(
                    new ParagraphProperties(
                        new SpacingBetweenLines { After = "0" }
                    )
                );

                foreach (var link in _data.ProjectLinks)
                {
                    var hyperlink = GetHyperlinkFrom(link);

                    paragraph.AppendChild(hyperlink);
                    paragraph.AppendChild(
                        new Run(
                            new RunProperties(
                                new RunFonts { Ascii = "Arial" },
                                new FontSize { Val = "22" }
                            ),
                            new Text("    ") { Space = SpaceProcessingModeValues.Preserve }
                        ));
                }
                document.Body.AppendChild(paragraph);
                document.Body.AppendChild(new Paragraph(new Run()));
            }

            if (_data.Educations.Count > 0)
            {
                document.Body.AppendChild(educationTitle);
                foreach (var education in _data.Educations)
                {
                    var educationParagraphs = GetEducationParagraphs(education);
                    foreach (var paragraph in educationParagraphs)
                    {
                        document.Body.AppendChild(paragraph);
                    }
                    document.Body.AppendChild(new Paragraph(new Run()));
                }
            }

            return document;
        }

        public Numbering BuildBulletListNumbering()
        {
            Numbering element = new(
                    new AbstractNum(
                        new Level(
                            new NumberingFormat { Val = NumberFormatValues.Bullet },
                            new LevelText { Val = "-" },
                            new LevelJustification { Val = LevelJustificationValues.Left },
                            new ParagraphProperties(
                                new Indentation { Left = "720", Hanging = "360" }
                            )
                        )
                        { LevelIndex = 0 }
                    )
                    { AbstractNumberId = 1 },
                    new NumberingInstance(
                        new AbstractNumId { Val = 1 }
                    )
                    { NumberID = 1 }
                );
            return element;
        }

        public Styles BuildStyles()
        {
            var hyperLinkStyle = new Style(
               new StyleName { Val = "Hyperlink" },
               new RunProperties(
                   new Color { Val = "000080" },
                   new Underline { Val = UnderlineValues.Single }
               )
            )
            {
                Type = StyleValues.Character,
                StyleId = "InternetLink",
                CustomStyle = true,
            };
            return new Styles(hyperLinkStyle);
        }

        private static Hyperlink GetHyperlinkFrom(CVProjectLink link)
        {
            Hyperlink hyperlink = new(
                new Run(
                    new RunProperties(
                        new RunStyle { Val = "InternetLink" },
                        new RunFonts { Ascii = "Arial" },
                        new FontSize { Val = "22" }
                    ),
                    new Text(link.Title)
                )
            )
            {
                Anchor = link.Url
            };
            return hyperlink;
        }

        private static Paragraph GetTitleParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new Bold(),
                        new BoldComplexScript(),
                        new FontSize { Val = "28" },
                        new FontSizeComplexScript { Val = "28" }
                    ),
                    new Text(text)
                )
            );
        }

        private static Paragraph GetSecondaryTitleParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new Bold(),
                        new BoldComplexScript(),
                        new FontSize { Val = "24" },
                        new FontSizeComplexScript { Val = "24" }
                    ),
                    new Text(text)
                )
            );
        }

        private static Paragraph GetPlainTextParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new FontSize { Val = "22" },
                        new FontSizeComplexScript { Val = "22" }
                    ),
                    new Text(text)
                )
            );
        }

        private static List<Paragraph> GetList(List<string> listElements)
        {
            List<Paragraph> paragraphs = new();

            foreach (var listElement in listElements)
            {
                Paragraph paragraph = new();

                Run textRun = new(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new FontSize { Val = "22" }
                    ),
                    new Text(listElement)
                );
                paragraph.PrependChild(textRun);

                ParagraphProperties paragraphProperties = new(
                    new SpacingBetweenLines { After = "0" },
                    new ParagraphStyleId { Val = "Normal" },
                    new NumberingProperties(
                        new NumberingLevelReference { Val = 0 },
                        new NumberingId { Val = 1 }
                    ),
                    new BiDi { Val = false },
                    new Justification { Val = JustificationValues.Left }
                );
                paragraph.PrependChild(paragraphProperties);

                paragraphs.Add(paragraph);
            }
            return paragraphs;
        }

        private List<Paragraph> GetEducationParagraphs(Education education)
        {
            var endDateString = education.EndDate.HasValue ? education.EndDate.Value.Year.ToString() : _strings.Present;
            Paragraph degreeAndSpecialityWithPeriod = new(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new Bold(),
                        new BoldComplexScript()
                    ),
                    new Text(education.Degree + ": " + education.Specialty)
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" }
                    ),
                    new Text(", " + education.StartDate.Year.ToString() + " \u2013 " + endDateString)
                )
            );

            Paragraph university = new(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" }
                    ),
                    new Text(string.Join(", ", education.University, education.City, education.Country))
                )
            );
            return new List<Paragraph> { degreeAndSpecialityWithPeriod, university };
        }

        private List<Paragraph> GetWorkExperienceParagraphs(CVExperience workExperience)
        {
            var endDateString = workExperience.EndDate.HasValue ? workExperience.EndDate.Value.ToString("MM/yyyy") : _strings.Present;
            Paragraph titleAndTimePeriod = new(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new Bold(),
                        new FontSize { Val = "22" }
                    ),
                    new Text(workExperience.Title)
                ),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Arial" },
                        new FontSize { Val = "22" }
                    ),
                    new Text(" \u2013 " + workExperience.StartDate.ToString("MM/yyyy") + " - " +
                        endDateString)
                    { Space = SpaceProcessingModeValues.Preserve }
                )
            );

            var companyParagraph = GetPlainTextParagraph(_strings.At + " " + workExperience.CompanyName);
            // var descriptionParagraph = GetPlainTextParagraph(workExperience.Description);

            return new List<Paragraph> { titleAndTimePeriod, companyParagraph };
        }

        private static Paragraph GetDrawingParagraphFromImage(
            string imagePartId,
            string fileName,
            string pictureName,
            double width,
            double height
        )
        {
            double englishMetricUnitsPerInch = 914400;
            double pixelsPerInch = 96;

            //calculate size in emu
            double emuWidth = width * englishMetricUnitsPerInch / pixelsPerInch;
            double emuHeight = height * englishMetricUnitsPerInch / pixelsPerInch;
            var picture = new DP.Picture(
                new DP.NonVisualPictureProperties(
                    new DP.NonVisualDrawingProperties { Id = (UInt32Value)0U, Name = fileName },
                    new DP.NonVisualPictureDrawingProperties()
                ),
                new DP.BlipFill(
                    new A.Blip(
                    new A.BlipExtensionList(new A.BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" }))
                    {
                        Embed = imagePartId,
                        CompressionState = A.BlipCompressionValues.Print
                    },
                    new A.Stretch(new A.FillRectangle())
                ),
                new DP.ShapeProperties(
                    new A.Transform2D(
                        new A.Offset { X = 0L, Y = 0L },
                        new A.Extents { Cx = (Int64Value)emuWidth, Cy = (Int64Value)emuHeight }
                    ),
                    new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }
                )
            );

            var drawing = new Drawing(
                new DW.Anchor(
                    new DW.SimplePosition { X = 0, Y = 0 },
                    new DW.HorizontalPosition
                    {
                        RelativeFrom = DW.HorizontalRelativePositionValues.Column,
                        HorizontalAlignment = new("right"),
                    },
                    new DW.VerticalPosition
                    {
                        RelativeFrom = DW.VerticalRelativePositionValues.Paragraph,
                        PositionOffset = new("635"),
                    },
                    new DW.Extent { Cx = (Int64Value)emuWidth, Cy = (Int64Value)emuHeight },
                    new DW.EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                    new DW.WrapSquare { WrapText = DW.WrapTextValues.Left },
                    new DW.DocProperties { Id = (UInt32Value)1U, Name = pictureName },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                    new A.GraphicFrameLocks { NoChangeAspect = true }),
                    new A.Graphic(new A.GraphicData(picture) { Uri = GraphicDataUri })
                )
                {
                    BehindDoc = false,
                    DistanceFromTop = (UInt32Value)0U,
                    DistanceFromBottom = (UInt32Value)0U,
                    DistanceFromLeft = (UInt32Value)109855U,
                    DistanceFromRight = (UInt32Value)0U,
                    SimplePos = false,
                    Locked = false,
                    LayoutInCell = false,
                    AllowOverlap = true,
                    RelativeHeight = (UInt32Value)2,
                    // EditId = "50D07946",
                    // ^ I have no idea what is it, it was put so in official Docs
                });

            // var pp =  new ParagraphProperties(
            /*new ParagraphStyleId { Val = "Normal" },
                    new BiDi { Val = false },
                    new Justification { Val = JustificationValues.Left }
                ),*/

            return new Paragraph(
                new Run(drawing)
            );
        }
    }
}
