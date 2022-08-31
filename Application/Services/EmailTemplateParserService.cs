using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailTemplateParserService : IEmailTemplateParserService
{
    private readonly ClientSettings _clientSettings;

    public EmailTemplateParserService(IOptions<ClientSettings> clientSettings)
    {
        _clientSettings = clientSettings.Value;
    }

    public string PrepareInviteAdminEmail(Admin admin, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleRoute/token={admin.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareInviteCompanyEmail(Company company, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleRoute/token={company.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PreparePasswordResetEmail(Account account, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleRoute/token={account.ResetToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }

    public string PrepareVerifyStudentEmail(Student student, string stringTemplate)
    {
        var url = $"{_clientSettings.Url}/sampleRoute/token={student.VerificationToken}";

        stringTemplate = stringTemplate.Replace("{{{url}}}", url);

        return stringTemplate;
    }
}
