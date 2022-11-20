using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly IEmailTemplateParserService _emailTemplateParserService;
    private readonly IEmailTemplatesService _emailTemplatesService;
    private readonly IMailKitService _mailKitService;
    private readonly EmailTemplateSettings _emailTemplateSettings;

    public EmailService(
        IEmailTemplateParserService emailTemplateParserService,
        IEmailTemplatesService emailTemplatesService,
        IMailKitService mailKitService,
        IOptions<EmailTemplateSettings> emailTemplateOptions)
    {
        _emailTemplateParserService = emailTemplateParserService;
        _emailTemplatesService = emailTemplatesService;
        _mailKitService = mailKitService;
        _emailTemplateSettings = emailTemplateOptions.Value;
    }

    public async Task SendInviteAdminEmailAsync(Admin admin)
    {
        var template = await _emailTemplatesService
            .ReadTemplateAsync(_emailTemplateSettings.InviteAdmin.TemplateName);

        template = _emailTemplateParserService.PrepareInviteAdminEmail(admin, template);

        await _mailKitService.SendAsync(
            admin.NormalizedEmail,
            _emailTemplateSettings.InviteAdmin.Subject,
            template);
    }

    public async Task SendInviteCompanyEmailAsync(Company company)
    {
        var template = await _emailTemplatesService
            .ReadTemplateAsync(_emailTemplateSettings.InviteCompany.TemplateName);

        template = _emailTemplateParserService.PrepareInviteCompanyEmail(company, template);

        await _mailKitService.SendAsync(
            company.NormalizedEmail,
            _emailTemplateSettings.InviteCompany.Subject,
            template);
    }

    public async Task SendPasswordResetEmailAsync(Account account)
    {
        var template = await _emailTemplatesService
            .ReadTemplateAsync(_emailTemplateSettings.PasswordReset.TemplateName);

        template = _emailTemplateParserService.PreparePasswordResetEmail(account, template);

        await _mailKitService.SendAsync(
            account.NormalizedEmail,
            _emailTemplateSettings.PasswordReset.Subject,
            template);
    }

    public async Task SendVerifyStudentEmailAsync(Student student)
    {
        var template = await _emailTemplatesService
            .ReadTemplateAsync(_emailTemplateSettings.VerifyStudent.TemplateName);

        template = _emailTemplateParserService.PrepareVerifyStudentEmail(student, template);

        await _mailKitService.SendAsync(
            student.NormalizedEmail,
            _emailTemplateSettings.VerifyStudent.Subject,
            template);
    }
}
