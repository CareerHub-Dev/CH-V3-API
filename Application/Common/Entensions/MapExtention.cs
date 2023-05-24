using Application.Common.DTO.Admins;
using Application.Common.DTO.Bans;
using Application.Common.DTO.Companies;
using Application.Common.DTO.CompanyLinks;
using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.CVs;
using Application.Common.DTO.Educations;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.DTO.JobDirection;
using Application.Common.DTO.JobOfferReviews;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Notifications;
using Application.Common.DTO.Posts;
using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.StudentLogs;
using Application.Common.DTO.Students;
using Application.Common.DTO.Tags;
using Application.Common.Enums;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Entensions;

public static class MapExtention
{
    #region Tag

    public static IQueryable<BriefTagDTO> MapToBriefTagDTO(this IQueryable<Tag> tags)
    {
        return tags.Select(x => new BriefTagDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public static IQueryable<TagDTO> MapToTagDTO(this IQueryable<Tag> tags)
    {
        return tags.Select(x => new TagDTO
        {
            Id = x.Id,
            Name = x.Name,
            IsAccepted = x.IsAccepted,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
        });
    }

    #endregion

    #region StudentGroup

    public static IQueryable<BriefStudentGroupDTO> MapToBriefStudentGroupDTO(this IQueryable<StudentGroup> studentGroups)
    {
        return studentGroups.Select(x => new BriefStudentGroupDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public static IQueryable<StudentGroupDTO> MapToStudentGroupDTO(this IQueryable<StudentGroup> studentGroups)
    {
        return studentGroups.Select(x => new StudentGroupDTO
        {
            Id = x.Id,
            Name = x.Name,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
        });
    }

    #endregion

    #region JobPosition

    public static IQueryable<JobPositionDTO> MapToJobPositionDTO(this IQueryable<JobPosition> jobPositions)
    {
        return jobPositions.Select(x => new JobPositionDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    #endregion

    #region JobDirection

    public static IQueryable<JobDirectionDTO> MapToJobDirectionDTO(this IQueryable<JobDirection> jobDirections)
    {
        return jobDirections.Select(x => new JobDirectionDTO
        {
            Id = x.Id,
            Name = x.Name,
            RecomendedTemplateLanguage = x.RecomendedTemplateLanguage
        });
    }

    #endregion

    #region StudentLog

    public static IQueryable<StudentLogDTO> MapToStudentLogDTO(this IQueryable<StudentLog> studentLogs)
    {
        return studentLogs.Select(x => new StudentLogDTO
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name }
        });
    }

    #endregion

    #region Ban

    public static IQueryable<BanDTO> MapToBanDTO(this IQueryable<Ban> bans)
    {
        return bans.Select(x => new BanDTO
        {
            Id = x.Id,
            Reason = x.Reason,
            Expires = x.Expires
        });
    }

    #endregion

    #region Admin

    public static IQueryable<AdminDTO> MapToAdminDTO(this IQueryable<Admin> admins)
    {
        return admins.Select(x => new AdminDTO
        {
            Id = x.Id,
            Email = x.Email,
            Verified = x.Verified,
            PasswordReset = x.PasswordReset,
            IsSuperAdmin = x.IsSuperAdmin
        });
    }

    #endregion

    #region Student

    public static IQueryable<ShortStudentDTO> MapToShortStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new ShortStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
        });
    }

    public static IQueryable<FollowedShortStudentDTO> MapToFollowedShortStudentDTO(this IQueryable<Student> students, Guid followerStudentId)
    {
        return students.Select(x => new FollowedShortStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
            IsFollowed = x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == followerStudentId),
        });
    }

    public static IQueryable<DetailedStudentDTO> MapToDetailedStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new DetailedStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            Phone = x.Phone,
            BirthDate = x.BirthDate,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
        });
    }

    public static IQueryable<StudentDTO> MapToStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new StudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            Phone = x.Phone,
            BirthDate = x.BirthDate,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
            Verified = x.Verified,
            PasswordReset = x.PasswordReset
        });
    }

    #endregion

    #region Company 

    public static IQueryable<BriefCompanyDTO> MapToBriefCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new BriefCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner
        });
    }

    public static IQueryable<BriefCompanyWithStatsDTO> MapToBriefCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new BriefCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            )
        });
    }

    public static IQueryable<ShortCompanyDTO> MapToShortCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new ShortCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
        });
    }

    public static IQueryable<ShortCompanyWithStatsDTO> MapToShortCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new ShortCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            )
        });
    }

    public static IQueryable<FollowedShortCompanyWithStatsDTO> MapToFollowedShortCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        Guid followerStudentId,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new FollowedShortCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            ),
            IsFollowed = x.SubscribedStudents.Any(x => x.Id == followerStudentId),
        });
    }

    public static IQueryable<DetailedCompanyDTO> MapToDetailedCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new DetailedCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            Description = x.Description,
            Links = x.Links.MapToCompanyLinkDTO().ToList(),
        });
    }

    public static IQueryable<CompanyDTO> MapToCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new CompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            Description = x.Description,
            Links = x.Links.MapToCompanyLinkDTO().ToList(),
            Verified = x.Verified,
            PasswordReset = x.PasswordReset,
        });
    }

    #endregion

    #region CompanyLink

    public static IEnumerable<CompanyLinkDTO> MapToCompanyLinkDTO(this IEnumerable<CompanyLink> links)
    {
        return links.Select(x => new CompanyLinkDTO
        {
            Title = x.Title,
            Uri = x.Uri
        });
    }

    #endregion

    #region Experience

    public static IQueryable<ExperienceDTO> MapToExperienceDTO(this IQueryable<Experience> experiences)
    {
        return experiences.Select(x => new ExperienceDTO
        {
            Id = x.Id,
            Title = x.Title,
            CompanyName = x.CompanyName,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobLocation = x.JobLocation,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            StudentId = x.StudentId
        });
    }

    public static IEnumerable<CVExperienceDTO> MapToExperienceDTO(this IEnumerable<CVExperience> experiences)
    {
        return experiences.Select(x => new CVExperienceDTO
        {
            Title = x.Title,
            CompanyName = x.CompanyName,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobLocation = x.JobLocation,
            StartDate = x.StartDate,
            EndDate = x.EndDate
        });
    }

    #endregion

    #region JobOffer 

    public static IQueryable<DetiledJobOfferDTO> MapToDetiledJobOfferDTO(this IQueryable<JobOffer> jobOffers)
    {
        return jobOffers.Select(x => new DetiledJobOfferDTO
        {
            Id = x.Id,
            Title = x.Title,
            Image = x.Image,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobPosition = new JobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
            JobDirection = new JobDirectionDTO { Id = x.JobPosition!.JobDirection!.Id, Name = x.JobPosition.JobDirection.Name },
            Company = new BriefCompanyOfJobOfferDTO { Id = x.Company!.Id, Name = x.Company.Name },
            Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
        });
    }

    public static IQueryable<DetiledJobOfferWithStatsDTO> MapToDetiledJobOfferWithStatsDTO(
        this IQueryable<JobOffer> jobOffers,
        bool? isSubscriberMustBeVerified = null,
        bool? isStudentOfAppliedCVMustBeVerified = null)
    {
        return jobOffers.Select(x => new DetiledJobOfferWithStatsDTO
        {
            Id = x.Id,
            Title = x.Title,
            Image = x.Image,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobPosition = new JobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
            JobDirection = new JobDirectionDTO { Id = x.JobPosition!.JobDirection!.Id, Name = x.JobPosition.JobDirection.Name },
            Company = new BriefCompanyOfJobOfferDTO { Id = x.Company!.Id, Name = x.Company.Name },
            Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
            AmountSubscribers = x.SubscribedStudents.Count(x =>
                !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                    x.Verified != null || x.PasswordReset != null :
                    x.Verified == null && x.PasswordReset == null)
            ),
            AmountAppliedCVs = x.CVJobOffers.Count(x =>
                !isStudentOfAppliedCVMustBeVerified.HasValue || (isStudentOfAppliedCVMustBeVerified.Value ?
                    x.CV!.Student!.Verified != null || x.CV!.Student.PasswordReset != null :
                    x.CV!.Student!.Verified == null && x.CV!.Student.PasswordReset == null)
            )
        });
    }

    public static IQueryable<FollowedDetiledJobOfferWithStatsDTO> MapToFollowedDetiledJobOfferWithStatsDTO(
        this IQueryable<JobOffer> jobOffers,
        Guid followerStudentId,
        bool? isSubscriberMustBeVerified = null,
        bool? isStudentOfAppliedCVMustBeVerified = null)
    {
        return jobOffers.Select(x => new FollowedDetiledJobOfferWithStatsDTO
        {
            Id = x.Id,
            Title = x.Title,
            Image = x.Image,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobPosition = new JobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
            JobDirection = new JobDirectionDTO { Id = x.JobPosition!.JobDirection!.Id, Name = x.JobPosition.JobDirection.Name },
            Company = new BriefCompanyOfJobOfferDTO { Id = x.Company!.Id, Name = x.Company.Name },
            Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
            AmountSubscribers = x.SubscribedStudents.Count(x =>
                !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                    x.Verified != null || x.PasswordReset != null :
                    x.Verified == null && x.PasswordReset == null)
            ),
            AmountAppliedCVs = x.CVJobOffers.Count(x =>
                !isStudentOfAppliedCVMustBeVerified.HasValue || (isStudentOfAppliedCVMustBeVerified.Value ?
                    x.CV!.Student!.Verified != null || x.CV!.Student.PasswordReset != null :
                    x.CV!.Student!.Verified == null && x.CV!.Student.PasswordReset == null)
            ),
            IsFollowed = x.SubscribedStudents.Any(x => x.Id == followerStudentId),
        });
    }

    public static IQueryable<JobOfferDTO> MapToJobOfferDTO(this IQueryable<JobOffer> jobOffers)
    {
        return jobOffers.Select(x => new JobOfferDTO
        {
            Id = x.Id,
            Title = x.Title,
            Image = x.Image,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobPosition = new JobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
            JobDirection = new JobDirectionDTO { Id = x.JobPosition!.JobDirection!.Id, Name = x.JobPosition.JobDirection.Name },
            Company = new BriefCompanyOfJobOfferDTO { Id = x.Company!.Id, Name = x.Company.Name },
            Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
            Overview = x.Overview,
            Requirements = x.Requirements,
            Responsibilities = x.Responsibilities,
            Preferences = x.Preferences
        });
    }

    #endregion

    #region CV 

    public static IQueryable<BriefCVDTO> MapToBriefCVDTO(this IQueryable<CV> cvs)
    {
        return cvs.Select(x => new BriefCVDTO
        {
            Id = x.Id,
            Title = x.Title,
            ExperienceLevel = x.ExperienceLevel,
            Created = x.Created,
            Modified = x.Modified
        });
    }

    public static IQueryable<CVDTO> MapToCVDTO(this IQueryable<CV> cvs)
    {
        return cvs.Select(x => new CVDTO
        {
            Id = x.Id,
            Title = x.Title,
            ExperienceLevel = x.ExperienceLevel,
            Created = x.Created,
            Modified = x.Modified,
            JobPosition = new JobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
            JobDirection = new JobDirectionDTO { Id = x.JobPosition.JobDirection!.Id, Name = x.JobPosition.JobDirection.Name },
            TemplateLanguage = x.TemplateLanguage,
            LastName = x.LastName,
            FirstName = x.FirstName,
            Photo = x.Photo,
            Goals = x.Goals,
            StudentId = x.StudentId,
            HardSkills = x.HardSkills,
            SoftSkills = x.SoftSkills,
            ForeignLanguages = x.ForeignLanguages.MapToForeignLanguageDTO().ToList(),
            ProjectLinks = x.ProjectLinks.MapToCVProjectLinkDTO().ToList(),
            Educations = x.Educations.MapToEducationDTO().ToList(),
        });
    }

    #endregion

    #region CVJobOffer

    public static IQueryable<BriefCVWithStatusDTO> MapToBriefCVWithStatusDTO(this IQueryable<CVJobOffer> cvJobOffers)
    {
        return cvJobOffers.Select(x => new BriefCVWithStatusDTO
        {
            Status = x.Status,
            Id = x.CV!.Id,
            Title = x.CV.Title,
            Created = x.CV.Created,
            Modified = x.CV.Modified
        });
    }

    #endregion

    #region ForeignLanguage

    public static IEnumerable<ForeignLanguageDTO> MapToForeignLanguageDTO(this IEnumerable<ForeignLanguage> languages)
    {
        return languages.Select(x => new ForeignLanguageDTO
        {
            Name = x.Name,
            LanguageLevel = x.LanguageLevel,
        });
    }

    #endregion

    #region CVProjectLink

    public static IEnumerable<CVProjectLinkDTO> MapToCVProjectLinkDTO(this IEnumerable<CVProjectLink> links)
    {
        return links.Select(x => new CVProjectLinkDTO
        {
            Title = x.Title,
            Url = x.Url,
        });
    }

    #endregion

    #region Education

    public static IEnumerable<EducationDTO> MapToEducationDTO(this IEnumerable<Education> educations)
    {
        return educations.Select(x => new EducationDTO
        {
            University = x.University,
            City = x.City,
            Country = x.Country,
            Specialty = x.Specialty,
            Degree = x.Degree,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
        });
    }

    #endregion

    #region AccountToAccountOfPost

    public static AccountOfPostDTO MapToAccountOfPostDTO(this Account account)
    {
        return account switch
        {
            Company company => new AccountOfPostDTO
            {
                Id = company.Id,
                Name = company.Name,
                Image = company.Logo ?? company.Banner,
                Role = Role.Company
            },
            Student student => new AccountOfPostDTO
            {
                Id = student.Id,
                Name = $"{student.LastName} {student.FirstName}",
                Image = student.Photo,
                Role = Role.Student
            },
            Admin admin => new AccountOfPostDTO
            {
                Id = admin.Id,
                Name = "Administration",
                Image = null,
                Role = Role.Admin
            },
            _ => new AccountOfPostDTO(),
        };
    }

    #endregion

    #region Post

    public static IQueryable<PostDTO> MapToPostDTO(this IQueryable<Post> posts)
    {
        return posts.Select(x => new PostDTO
        {
            Id = x.Id,
            Text = x.Text,
            Images = x.Images,
            CreatedDate = x.CreatedDate,
            Likes = x.StudentsLiked.Count(),
            Account = x.Account!.MapToAccountOfPostDTO()
        });
    }

    public static IQueryable<LikedPostDTO> MapToLikedPostDTO(this IQueryable<Post> posts, Guid likerStudentId)
    {
        return posts.Select(x => new LikedPostDTO
        {
            Id = x.Id,
            Text = x.Text,
            Images = x.Images,
            CreatedDate = x.CreatedDate,
            Likes = x.StudentsLiked.Count(),
            Account = x.Account!.MapToAccountOfPostDTO(),
            IsLiked = x.StudentsLiked.Any(x => x.Id == likerStudentId)
        });
    }

    #endregion

    #region JobOfferReview

    public static IQueryable<JobOfferReviewDTO> MapToJobOfferReviewDTO(this IQueryable<CVJobOffer> cvJobOffers)
    {
        return cvJobOffers.Select(x => new JobOfferReviewDTO
        {
            Id = x.Id,
            Status = x.Status,
            Message = x.Message,
            Created = x.Created,
            CV = new ShortCVofReviewDTO
            {
                Id = x.CV!.Id,
                Title = x.CV.Title,
                Created = x.CV.Created,
                Modified = x.CV.Modified,
            },
            JobOffer = new ShortJobOfferOfReviewDTO
            {
                Id = x.JobOffer!.Id,
                Title = x.JobOffer.Title,
                Image = x.JobOffer.Image
            }
        });
    }

    #endregion

    #region Notification

    public static IQueryable<NotificationDTO> MapToNotificationDTO(this IQueryable<Notification> notification)
    {
        return notification.Select(x => new NotificationDTO
        {
            Id = x.Id,
            ReferenceId = x.ReferenceId,
            UkMessage = x.UkMessage,
            EnMessage = x.EnMessage,
            Image = x.Image,
            IsViewed = x.IsViewed,
            Created = x.Created,
            Type = x.Type
        });
    }

    #endregion
}
