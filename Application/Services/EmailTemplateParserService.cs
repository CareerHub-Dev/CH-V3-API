using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailTemplateParserService 
    : IEmailTemplateParserService
{
    private readonly ClientSettings _clientSettings;

    public EmailTemplateParserService(
        IOptions<ClientSettings> clientSettings)
    {
        _clientSettings = clientSettings.Value;
    }

    public string PrepareInviteAdminEmail(Admin admin, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleInviteAdminRoute/token={admin.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareInviteCompanyEmail(Company company, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleInviteCompanyRoute/token={company.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PreparePasswordResetEmail(Account account, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/samplePasswordResetRoute/token={account.ResetToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareVerifyStudentEmail(Student student, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleVerifyStudentRoute/token={student.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }
}
