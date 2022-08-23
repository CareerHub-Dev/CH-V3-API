﻿using Domain.Entities;

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

    public static IQueryable<StudentLog> Search(this IQueryable<StudentLog> studentLogs, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return studentLogs;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return studentLogs
            .Where(x => 
                x.FirstName.Trim().ToLower().Contains(lowerCaseTerm) ||
                x.LastName.Trim().ToLower().Contains(lowerCaseTerm) ||
                x.NormalizedEmail.Contains(lowerCaseTerm)
            );
    }

    public static IQueryable<Admin> Search(this IQueryable<Admin> admins, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return admins;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return admins
            .Where(x =>
                x.NormalizedEmail.Trim().ToLower().Contains(lowerCaseTerm)
            );
    }
}
