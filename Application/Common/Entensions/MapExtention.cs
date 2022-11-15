using Application.Common.DTO.Admins;
using Application.Common.DTO.Bans;
using Application.Common.DTO.Companies;
using Application.Common.DTO.CompanyLinks;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.StudentLogs;
using Application.Common.DTO.Students;
using Application.Common.DTO.Tags;
using Domain.Entities;

namespace Application.Common.Entensions;

public static class MapExtention
{
    #region Tag

    public static IQueryable<BriefTagDTO> MapToBriefTagDTO(this IQueryable<Tag> tags)
    {
        return tags.Select(x => new BriefTagDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public static IQueryable<TagDTO> MapToTagDTO(this IQueryable<Tag> tags)
    {
        return tags.Select(x => new TagDTO
        {
            Id = x.Id,
            Name = x.Name,
            IsAccepted = x.IsAccepted,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
        });
    }

    #endregion

    #region StudentGroup

    public static IQueryable<BriefStudentGroupDTO> MapToBriefStudentGroupDTO(this IQueryable<StudentGroup> studentGroups)
    {
        return studentGroups.Select(x => new BriefStudentGroupDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public static IQueryable<StudentGroupDTO> MapToStudentGroupDTO(this IQueryable<StudentGroup> studentGroups)
    {
        return studentGroups.Select(x => new StudentGroupDTO
        {
            Id = x.Id,
            Name = x.Name,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
        });
    }

    #endregion

    #region JobPosition

    public static IQueryable<BriefJobPositionDTO> MapToBriefJobPositionDTO(this IQueryable<JobPosition> jobPositions)
    {
        return jobPositions.Select(x => new BriefJobPositionDTO
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public static IQueryable<JobPositionDTO> MapToJobPositionDTO(this IQueryable<JobPosition> jobPositions)
    {
        return jobPositions.Select(x => new JobPositionDTO
        {
            Id = x.Id,
            Name = x.Name,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
        });
    }

    #endregion

    #region StudentLog

    public static IQueryable<StudentLogDTO> MapToStudentLogDTO(this IQueryable<StudentLog> studentLogs)
    {
        return studentLogs.Select(x => new StudentLogDTO
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Created = x.Created,
            LastModified = x.LastModified,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name }
        });
    }

    #endregion

    #region Ban

    public static IQueryable<BanDTO> MapToBanDTO(this IQueryable<Ban> bans)
    {
        return bans.Select(x => new BanDTO
        {
            Id = x.Id,
            Reason = x.Reason,
            Expires = x.Expires,
        });
    }

    #endregion

    #region Admin

    public static IQueryable<AdminDTO> MapToAdminDTO(this IQueryable<Admin> admins)
    {
        return admins.Select(x => new AdminDTO
        {
            Id = x.Id,
            Email = x.Email,
            Verified = x.Verified,
            PasswordReset = x.PasswordReset,
            IsSuperAdmin = x.IsSuperAdmin
        });
    }

    #endregion

    #region Student

    public static IQueryable<ShortStudentDTO> MapToShortStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new ShortStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
        });
    }

    public static IQueryable<FollowedShortStudentDTO> MapToFollowedShortStudentDTO(this IQueryable<Student> students, Guid followerStudentId)
    {
        return students.Select(x => new FollowedShortStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
            IsFollowed = x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == followerStudentId),
        });
    }

    public static IQueryable<DetailedStudentDTO> MapToDetailedStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new DetailedStudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            Phone = x.Phone,
            BirthDate = x.BirthDate,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
        });
    }

    public static IQueryable<StudentDTO> MapToStudentDTO(this IQueryable<Student> students)
    {
        return students.Select(x => new StudentDTO
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Photo = x.Photo,
            Phone = x.Phone,
            BirthDate = x.BirthDate,
            StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
            Verified = x.Verified,
            PasswordReset = x.PasswordReset
        });
    }

    #endregion

    #region Company 

    public static IQueryable<BriefCompanyDTO> MapToBriefCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new BriefCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner
        });
    }

    public static IQueryable<BriefCompanyWithStatsDTO> MapToBriefCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new BriefCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            )
        });
    }

    public static IQueryable<ShortCompanyDTO> MapToShortCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new ShortCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
        });
    }

    public static IQueryable<ShortCompanyWithStatsDTO> MapToShortCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new ShortCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            )
        });
    }

    public static IQueryable<FollowedShortCompanyWithStatsDTO> MapToFollowedShortCompanyWithStatsDTO(
        this IQueryable<Company> companies,
        Guid followerStudentId,
        bool? isJobOfferMustBeActive = null,
        bool? isSubscriberMustBeVerified = null)
    {
        return companies.Select(x => new FollowedShortCompanyWithStatsDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            AmountJobOffers = x.JobOffers.Count(x => !isJobOfferMustBeActive.HasValue || (isJobOfferMustBeActive.Value ?
                x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow : x.StartDate > DateTime.UtcNow)
            ),
            AmountSubscribers = x.SubscribedStudents.Count(x => !isSubscriberMustBeVerified.HasValue || (isSubscriberMustBeVerified.Value ?
                x.Verified != null || x.PasswordReset != null : x.Verified == null && x.PasswordReset == null)
            ),
            IsFollowed = x.SubscribedStudents.Any(x => x.Id == followerStudentId),
        });
    }

    public static IQueryable<DetailedCompanyDTO> MapToDetailedCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new DetailedCompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            Description = x.Description,
            Links = x.Links.MapToCompanyLinkDTO().ToList(),
        });
    }

    public static IQueryable<CompanyDTO> MapToCompanyDTO(this IQueryable<Company> companies)
    {
        return companies.Select(x => new CompanyDTO
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            Logo = x.Logo,
            Banner = x.Banner,
            Motto = x.Motto,
            Description = x.Description,
            Links = x.Links.MapToCompanyLinkDTO().ToList(),
            Verified = x.Verified,
            PasswordReset = x.PasswordReset,
        });
    }

    #endregion

    #region CompanyLink

    public static IEnumerable<CompanyLinkDTO> MapToCompanyLinkDTO(this IEnumerable<CompanyLink> links)
    {
        return links.Select(x => new CompanyLinkDTO
        {
            Title = x.Title,
            Uri = x.Uri
        });
    }

    #endregion

    #region Experience

    public static IQueryable<ExperienceDTO> MapToExperienceDTO(this IQueryable<Experience> experiences)
    {
        return experiences.Select(x => new ExperienceDTO
        {
            Id = x.Id,
            Title = x.Title,
            CompanyName = x.CompanyName,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobLocation = x.JobLocation,
            StartDate = x.StartDate,
            EndDate = x.EndDate
        });
    }

    #endregion
}
