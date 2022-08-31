using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEmailTemplateParserService
{
    string PrepareInviteAdminEmail(Admin admin, string stringTemplate);
    string PrepareInviteCompanyEmail(Company company, string stringTemplate);
    string PreparePasswordResetEmail(Account account, string stringTemplate);
    string PrepareVerifyStudentEmail(Student student, string stringTemplate);
}
