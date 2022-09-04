using Domain.Entities;

namespace Application.Common.Entensions;

public static class FiltrationExtentions
{
    public static IQueryable<Admin> Filter(this IQueryable<Admin> admins, Guid? WithoutAdminId = null, bool? IsVerified = null)
    {
        if (WithoutAdminId.HasValue)
        {
            admins = admins.Where(x => x.Id != WithoutAdminId);
        }

        if (IsVerified.HasValue && IsVerified == true)
        {
            admins = admins.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (IsVerified.HasValue && IsVerified == false)
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

    public static IQueryable<Tag> Filter(this IQueryable<Tag> tags, bool? IsAccepted = null)
    {
        if (IsAccepted.HasValue)
        {
            tags = tags.Where(x => x.IsAccepted == IsAccepted);
        }

        return tags;
    }

    public static IQueryable<JobOffer> Filter(this IQueryable<JobOffer> jobOffers, bool? IsActive = null)
    {
        if (IsActive.HasValue && IsActive == true)
        {
            jobOffers = jobOffers.Where(x => x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow);
        }
        else if (IsActive.HasValue && IsActive == false)
        {
            jobOffers = jobOffers.Where(x => x.StartDate > DateTime.UtcNow);
        }

        return jobOffers;
    }

    public static IEnumerable<JobOffer> Filter(this IEnumerable<JobOffer> jobOffers, bool? IsActive = null)
    {
        if (IsActive.HasValue && IsActive == true)
        {
            jobOffers = jobOffers.Where(x => x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow);
        }
        else if (IsActive.HasValue && IsActive == false)
        {
            jobOffers = jobOffers.Where(x => x.StartDate > DateTime.UtcNow);
        }

        return jobOffers;
    }

    public static IQueryable<Student> Filter(this IQueryable<Student> students, Guid? WithoutStudentId = null, bool? IsVerified = null)
    {
        if (WithoutStudentId.HasValue)
        {
            students = students.Where(x => x.Id != WithoutStudentId);
        }

        if (IsVerified.HasValue && IsVerified == true)
        {
            students = students.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (IsVerified.HasValue && IsVerified == false)
        {
            students = students.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return students;
    }

    public static IEnumerable<Student> Filter(this IEnumerable<Student> students, Guid? WithoutStudentId = null, bool? IsVerified = null)
    {
        if (WithoutStudentId.HasValue)
        {
            students = students.Where(x => x.Id != WithoutStudentId);
        }

        if (IsVerified.HasValue && IsVerified == true)
        {
            students = students.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (IsVerified.HasValue && IsVerified == false)
        {
            students = students.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return students;
    }

    public static IQueryable<Company> Filter(this IQueryable<Company> companies, Guid? WithoutCompanyId = null, bool? IsVerified = null)
    {
        if (WithoutCompanyId.HasValue)
        {
            companies = companies.Where(x => x.Id != WithoutCompanyId);
        }

        if (IsVerified.HasValue && IsVerified == true)
        {
            companies = companies.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (IsVerified.HasValue && IsVerified == false)
        {
            companies = companies.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return companies;
    }
}
