using Application.Common.Models.Filtration.Admin;
using Domain.Entities;

namespace Application.Common.Entensions;

public static class FiltrationExtentions
{
    public static IQueryable<Admin> Filter(this IQueryable<Admin> admins, AdminListFilterParameters parameters)
    {
        if (parameters.WithoutAdminId.HasValue)
        {
            admins = admins.Where(x => x.Id != parameters.WithoutAdminId);
        }

        if (parameters.IsVerified.HasValue && parameters.IsVerified == true)
        {
            admins = admins.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (parameters.IsVerified.HasValue && parameters.IsVerified == false)
        {
            admins = admins.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return admins;
    }
    public static IQueryable<Admin> Filter(this IQueryable<Admin> admins, AdminFilterParameters parameters)
    {
        if (parameters.IsVerified.HasValue && parameters.IsVerified == true)
        {
            admins = admins.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (parameters.IsVerified.HasValue && parameters.IsVerified == false)
        {
            admins = admins.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return admins;
    }

    public static IQueryable<StudentLog> Filter(this IQueryable<StudentLog> studentLogs, Guid? StudentGroupId = null)
    {
        if (StudentGroupId.HasValue)
        {
            studentLogs = studentLogs.Where(x => x.StudentGroupId == StudentGroupId);
        }

        return studentLogs;
    }
}
