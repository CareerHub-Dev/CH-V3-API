using Domain.Entities;

namespace Application.Common.Entensions;

public static class SearchExtentions
{
    public static IQueryable<StudentGroup> Search(this IQueryable<StudentGroup> studentGroups, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return studentGroups;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return studentGroups.Where(x => x.Name.Trim().ToLower().Contains(lowerCaseTerm));
    }
}
