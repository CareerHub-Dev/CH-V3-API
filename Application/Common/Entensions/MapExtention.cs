using Application.Common.DTO.StudentGroups;
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
}
