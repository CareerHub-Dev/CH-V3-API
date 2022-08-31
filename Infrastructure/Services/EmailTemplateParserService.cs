using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class EmailTemplateParserService : IEmailTemplateParserService
{
    private readonly AppSettings _appSettings;

    public EmailTemplateParserService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public string PrepareInviteAdminEmailAsync(Admin admin, string stringTemplate)
    {
        var url = $"{_appSettings.ClientUrl}/sampleRoute/token={admin.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareInviteCompanyEmailAsync(Company company, string stringTemplate)
    {
        var url = $"{_appSettings.ClientUrl}/sampleRoute/token={company.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PreparePasswordResetEmailAsync(Account account, string stringTemplate)
    {
        var url = $"{_appSettings.ClientUrl}/sampleRoute/token={account.ResetToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareVerifyStudentEmailAsync(Student student, string stringTemplate)
    {
        var url = $"{_appSettings.ClientUrl}/sampleRoute/token={student.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }
}
