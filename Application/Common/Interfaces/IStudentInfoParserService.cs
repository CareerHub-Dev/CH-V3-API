using Application.Services.StudentInfoParser.Models;

namespace Application.Common.Interfaces
{
    public interface IStudentInfoParserService
    {
        public IEnumerable<StudentRow> Parse(StreamReader reader);
    }
}
