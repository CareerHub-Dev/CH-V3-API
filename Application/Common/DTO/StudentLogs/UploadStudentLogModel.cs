namespace Application.Common.DTO.StudentLogs;

public class UploadStudentLogModel
{
    private string _fullname = string.Empty;
    public string FullName
    {
        get
        {
            return _fullname;
        }
        set
        {
            _fullname = value;
            var initials = value.Split(' ');

            LastName = initials.ElementAtOrDefault(0) ?? string.Empty;
            FirstName = initials.ElementAtOrDefault(1) ?? string.Empty;
            MiddleName = initials.ElementAtOrDefault(2) ?? string.Empty;
        }
    }

    public string MiddleName { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public string Group { set; get; } = string.Empty;
}
