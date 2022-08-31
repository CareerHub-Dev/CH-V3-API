using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendInviteAdminEmailAsync(Admin admin);
    Task SendInviteCompanyEmailAsync(Company company);
    Task SendPasswordResetEmailAsync(Account account);
    Task SendVerifyStudentEmailAsync(Student student);
}
