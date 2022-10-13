using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Entensions;

public static class FiltrationExtentions
{
    public static IQueryable<Account> Filter(
        this IQueryable<Account> accounts, 
        bool? isVerified = null)
    {
        if (isVerified.HasValue && isVerified == true)
        {
            accounts = accounts.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (isVerified.HasValue && isVerified == false)
        {
            accounts = accounts.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        return accounts;
    }
    public static IQueryable<Admin> Filter(
        this IQueryable<Admin> admins, 
        Guid? withoutAdminId = null, 
        bool? isVerified = null, 
        bool? isSuperAdmin = null,
        ActivationStatus? activationStatus = null)
    {
        if (withoutAdminId.HasValue)
        {
            admins = admins.Where(x => x.Id != withoutAdminId);
        }

        if (isVerified.HasValue && isVerified == true)
        {
            admins = admins.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (isVerified.HasValue && isVerified == false)
        {
            admins = admins.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        if (isSuperAdmin.HasValue)
        {
            admins = admins.Where(x => x.IsSuperAdmin == isSuperAdmin);
        }

        if (activationStatus.HasValue)
        {
            admins = admins.Where(x => x.ActivationStatus == activationStatus);
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
        ActivationStatus? companyActivationStatus = null,
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

        if (isCompanyVerified.HasValue && isCompanyVerified == true)
        {
            jobOffers = jobOffers.Where(x => x.Company!.Verified != null || x.Company!.PasswordReset != null);
        }
        else if (isCompanyVerified.HasValue && isCompanyVerified == false)
        {
            jobOffers = jobOffers.Where(x => x.Company!.Verified == null && x.Company!.PasswordReset == null);
        }

        if (companyActivationStatus.HasValue)
        {
            jobOffers = jobOffers.Where(x => x.Company!.ActivationStatus == companyActivationStatus);
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
            jobOffers = jobOffers.Where(x => tagIds.All(y => x.Tags.Any(z => z.Id == y)));
        }

        return jobOffers;
    }

    public static IQueryable<Student> Filter(
        this IQueryable<Student> students, 
        Guid? withoutStudentId = null, 
        bool? isVerified = null, 
        List<Guid>? studentGroupIds = null,
        ActivationStatus? activationStatus = null)
    {
        if (withoutStudentId.HasValue)
        {
            students = students.Where(x => x.Id != withoutStudentId);
        }

        if (studentGroupIds != null && studentGroupIds.Count != 0)
        {
            students = students.Where(x => studentGroupIds.Contains(x.StudentGroupId));
        }

        if (isVerified.HasValue && isVerified == true)
        {
            students = students.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (isVerified.HasValue && isVerified == false)
        {
            students = students.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        if (activationStatus.HasValue)
        {
            students = students.Where(x => x.ActivationStatus == activationStatus);
        }

        return students;
    }

    public static IQueryable<Company> Filter(
        this IQueryable<Company> companies, 
        Guid? withoutCompanyId = null, 
        bool? isVerified = null,
        ActivationStatus? activationStatus = null)
    {
        if (withoutCompanyId.HasValue)
        {
            companies = companies.Where(x => x.Id != withoutCompanyId);
        }

        if (isVerified.HasValue && isVerified == true)
        {
            companies = companies.Where(x => x.Verified != null || x.PasswordReset != null);
        }
        else if (isVerified.HasValue && isVerified == false)
        {
            companies = companies.Where(x => x.Verified == null && x.PasswordReset == null);
        }

        if (activationStatus.HasValue)
        {
            companies = companies.Where(x => x.ActivationStatus == activationStatus);
        }

        return companies;
    }

    public static IQueryable<CompanyLink> Filter(
        this IQueryable<CompanyLink> companyLinks, 
        bool? isCompanyVerified = null,
        ActivationStatus? companyActivationStatus = null)
    {

        if (isCompanyVerified.HasValue && isCompanyVerified == true)
        {
            companyLinks = companyLinks.Where(x => x.Company!.Verified != null || x.Company!.PasswordReset != null);
        }
        else if (isCompanyVerified.HasValue && isCompanyVerified == false)
        {
            companyLinks = companyLinks.Where(x => x.Company!.Verified == null && x.Company!.PasswordReset == null);
        }

        if (companyActivationStatus.HasValue)
        {
            companyLinks = companyLinks.Where(x => x.Company!.ActivationStatus == companyActivationStatus);
        }

        return companyLinks;
    }

    public static IQueryable<Experience> Filter(
        this IQueryable<Experience> experiences,
        bool? isStudentVerified = null,
        ActivationStatus? studentActivationStatus = null)
    {

        if (isStudentVerified.HasValue && isStudentVerified == true)
        {
            experiences = experiences.Where(x => x.Student!.Verified != null || x.Student!.PasswordReset != null);
        }
        else if (isStudentVerified.HasValue && isStudentVerified == false)
        {
            experiences = experiences.Where(x => x.Student!.Verified == null && x.Student!.PasswordReset == null);
        }

        if (studentActivationStatus.HasValue)
        {
            experiences = experiences.Where(x => x.Student!.ActivationStatus == studentActivationStatus);
        }

        return experiences;
    }

    public static IQueryable<CV> Filter(
        this IQueryable<CV> cvs,
        bool? isStudentVerified = null,
        ActivationStatus? studentActivationStatus = null)
    {

        if (isStudentVerified.HasValue && isStudentVerified == true)
        {
            cvs = cvs.Where(x => x.Student!.Verified != null || x.Student!.PasswordReset != null);
        }
        else if (isStudentVerified.HasValue && isStudentVerified == false)
        {
            cvs = cvs.Where(x => x.Student!.Verified == null && x.Student!.PasswordReset == null);
        }

        if (studentActivationStatus.HasValue)
        {
            cvs = cvs.Where(x => x.Student!.ActivationStatus == studentActivationStatus);
        }

        return cvs;
    }
}
