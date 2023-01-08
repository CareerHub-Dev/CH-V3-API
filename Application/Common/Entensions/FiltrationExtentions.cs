using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Entensions;

public static class FiltrationExtentions
{
    public static IQueryable<Account> Filter(
        this IQueryable<Account> accounts,
        bool? isVerified = null,
        bool? isBanned = null)
    {
        switch (isVerified)
        {
            case true:
                accounts = accounts.Where(x => x.Verified != null || x.PasswordReset != null);
                break;
            case false:
                accounts = accounts.Where(x => x.Verified == null && x.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isBanned)
        {
            case true:
                accounts = accounts.Where(x => x.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                accounts = accounts.Where(x => x.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        return accounts;
    }

    public static IQueryable<Admin> Filter(
        this IQueryable<Admin> admins,
        Guid? withoutAdminId = null,
        bool? isVerified = null,
        bool? isBanned = null,
        bool? isSuperAdmin = null)
    {
        if (withoutAdminId.HasValue)
        {
            admins = admins.Where(x => x.Id != withoutAdminId);
        }

        switch (isVerified)
        {
            case true:
                admins = admins.Where(x => x.Verified != null || x.PasswordReset != null);
                break;
            case false:
                admins = admins.Where(x => x.Verified == null && x.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isBanned)
        {
            case true:
                admins = admins.Where(x => x.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                admins = admins.Where(x => x.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        if (isSuperAdmin.HasValue)
        {
            admins = admins.Where(x => x.IsSuperAdmin == isSuperAdmin);
        }

        return admins;
    }

    public static IQueryable<StudentLog> Filter(
        this IQueryable<StudentLog> studentLogs,
        List<Guid>? studentGroupIds = null)
    {
        if (studentGroupIds != null && studentGroupIds.Count != 0)
        {
            studentLogs = studentLogs.Where(x => studentGroupIds.Contains(x.StudentGroupId));
        }

        return studentLogs;
    }

    public static IQueryable<Tag> Filter(
        this IQueryable<Tag> tags,
        bool? isAccepted = null)
    {
        if (isAccepted.HasValue)
        {
            tags = tags.Where(x => x.IsAccepted == isAccepted);
        }

        return tags;
    }

    public static IQueryable<JobOffer> Filter(
        this IQueryable<JobOffer> jobOffers,
        bool? isActive = null,
        bool? isCompanyVerified = null,
        bool? isCompanyBanned = null,
        JobType? jobType = null,
        WorkFormat? workFormat = null,
        ExperienceLevel? experienceLevel = null,
        Guid? jobPositionId = null,
        List<Guid>? tagIds = null)
    {
        if (isActive.HasValue && isActive == true)
        {
            jobOffers = jobOffers.Where(x => x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow);
        }
        else if (isActive.HasValue && isActive == false)
        {
            jobOffers = jobOffers.Where(x => x.StartDate > DateTime.UtcNow);
        }

        switch (isCompanyVerified)
        {
            case true:
                jobOffers = jobOffers.Where(x => x.Company!.Verified != null || x.Company!.PasswordReset != null);
                break;
            case false:
                jobOffers = jobOffers.Where(x => x.Company!.Verified == null && x.Company!.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isCompanyBanned)
        {
            case true:
                jobOffers = jobOffers.Where(x => x.Company!.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                jobOffers = jobOffers.Where(x => x.Company!.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        if (jobType.HasValue)
        {
            jobOffers = jobOffers.Where(x => x.JobType == jobType);
        }

        if (workFormat.HasValue)
        {
            jobOffers = jobOffers.Where(x => x.WorkFormat == workFormat);
        }

        if (experienceLevel.HasValue)
        {
            jobOffers = jobOffers.Where(x => x.ExperienceLevel == experienceLevel);
        }

        if (jobPositionId.HasValue)
        {
            jobOffers = jobOffers.Where(x => x.JobPositionId == jobPositionId);
        }

        if (tagIds != null && tagIds.Count > 0)
        {
            jobOffers = jobOffers.Where(x => x.Tags.Count(x => tagIds.Contains(x.Id)) == tagIds.Count);
        }

        return jobOffers;
    }

    public static IQueryable<Student> Filter(
        this IQueryable<Student> students,
        Guid? withoutStudentId = null,
        bool? isVerified = null,
        bool? isBanned = null,
        List<Guid>? studentGroupIds = null)
    {
        if (withoutStudentId.HasValue)
        {
            students = students.Where(x => x.Id != withoutStudentId);
        }

        if (studentGroupIds != null && studentGroupIds.Count != 0)
        {
            students = students.Where(x => studentGroupIds.Contains(x.StudentGroupId));
        }

        switch (isVerified)
        {
            case true:
                students = students.Where(x => x.Verified != null || x.PasswordReset != null);
                break;
            case false:
                students = students.Where(x => x.Verified == null && x.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isBanned)
        {
            case true:
                students = students.Where(x => x.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                students = students.Where(x => x.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        return students;
    }

    public static IQueryable<Company> Filter(
        this IQueryable<Company> companies,
        Guid? withoutCompanyId = null,
        bool? isBanned = null,
        bool? isVerified = null)
    {
        if (withoutCompanyId.HasValue)
        {
            companies = companies.Where(x => x.Id != withoutCompanyId);
        }

        switch (isVerified)
        {
            case true:
                companies = companies.Where(x => x.Verified != null || x.PasswordReset != null);
                break;
            case false:
                companies = companies.Where(x => x.Verified == null && x.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isBanned)
        {
            case true:
                companies = companies.Where(x => x.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                companies = companies.Where(x => x.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        return companies;
    }

    public static IQueryable<Experience> Filter(
        this IQueryable<Experience> experiences,
        bool? isStudentVerified = null,
        bool? isStudentBanned = null)
    {

        switch (isStudentVerified)
        {
            case true:
                experiences = experiences.Where(x => x.Student!.Verified != null || x.Student!.PasswordReset != null);
                break;
            case false:
                experiences = experiences.Where(x => x.Student!.Verified == null && x.Student!.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isStudentBanned)
        {
            case true:
                experiences = experiences.Where(x => x.Student!.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                experiences = experiences.Where(x => x.Student!.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        return experiences;
    }

    public static IQueryable<CV> Filter(
        this IQueryable<CV> cvs,
        bool? isStudentVerified = null,
        bool? isStudentBanned = null)
    {

        switch (isStudentVerified)
        {
            case true:
                cvs = cvs.Where(x => x.Student!.Verified != null || x.Student!.PasswordReset != null);
                break;
            case false:
                cvs = cvs.Where(x => x.Student!.Verified == null && x.Student!.PasswordReset == null);
                break;
            default:
                break;
        }

        switch (isStudentBanned)
        {
            case true:
                cvs = cvs.Where(x => x.Student!.Bans.Any(x => x.Expires >= DateTime.UtcNow));
                break;
            case false:
                cvs = cvs.Where(x => x.Student!.Bans.All(x => x.Expires < DateTime.UtcNow));
                break;
            default:
                break;
        }

        return cvs;
    }

    public static IQueryable<Post> Filter(
        this IQueryable<Post> posts,
        bool? isAccountVerified = null,
        Guid? accountId = null)
    {

        switch (isAccountVerified)
        {
            case true:
                posts = posts.Where(x => x.Account!.Verified != null || x.Account!.PasswordReset != null);
                break;
            case false:
                posts = posts.Where(x => x.Account!.Verified == null && x.Account!.PasswordReset == null);
                break;
            default:
                break;
        }

        if(accountId != null)
        {
            posts = posts.Where(x => x.AccountId == accountId);
        }

        return posts;
    }
}
