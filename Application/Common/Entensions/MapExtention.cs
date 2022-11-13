using Application.Common.DTO.Bans;
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
}
