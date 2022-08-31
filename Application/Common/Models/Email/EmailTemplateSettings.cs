namespace Application.Common.Models.Email;

public class EmailTemplateSettings
{
    public EmailTemplate InviteAdmin { get; set; } = new EmailTemplate();

    public EmailTemplate InviteCompany { get; set; } = new EmailTemplate();

    public EmailTemplate PasswordReset { get; set; } = new EmailTemplate();

    public EmailTemplate VerifyStudent { get; set; } = new EmailTemplate();
}
