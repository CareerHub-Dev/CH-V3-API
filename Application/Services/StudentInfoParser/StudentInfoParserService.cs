using Application.Common.Interfaces;
using Application.Services.StudentInfoParser.Models;
using CsvHelper;
using System.Globalization;

namespace Application.Services.StudentInfoParser
{
    public class StudentInfoParserService : IStudentInfoParserService
    {
        public IEnumerable<StudentRow> Parse(StreamReader reader)
        {
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.Configuration.MissingFieldFound = null;
            csv.Context.RegisterClassMap<StudentRow>();
            csv.Context.Configuration.BadDataFound = null;
            csv.Context.Configuration.Delimiter = ",";
            return csv.GetRecords<StudentRow>().ToArray();
        }
    }
}
