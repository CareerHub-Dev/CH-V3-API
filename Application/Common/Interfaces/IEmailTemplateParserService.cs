using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEmailTemplateParserService
{
    string PrepareInviteAdminEmailAsync(Admin admin, string stringTemplate);
    string PrepareInviteCompanyEmailAsync(Company company, string stringTemplate);
    string PreparePasswordResetEmailAsync(Account account, string stringTemplate);
    string PrepareVerifyStudentEmailAsync(Student student, string stringTemplate);
}
