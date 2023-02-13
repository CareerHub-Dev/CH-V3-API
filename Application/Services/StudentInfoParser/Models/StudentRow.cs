using CsvHelper.Configuration;

namespace Application.Services.StudentInfoParser.Models
{
    public sealed class StudentRow : ClassMap<StudentRow>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { set; get; } = string.Empty;
        public string Group { set; get; } = string.Empty;

        public StudentRow()
        {
            Map(s => s.FullName).Index(2);
            Map(s => s.Email).Index(5);
            Map(s => s.Group).Index(6);
        }
    }
}
