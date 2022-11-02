using Application.Common.DTO.JobPositions;
using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.StudentLogs;
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
}
