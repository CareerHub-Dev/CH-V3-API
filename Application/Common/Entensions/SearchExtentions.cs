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

    public static IQueryable<JobPosition> Search(this IQueryable<JobPosition> jobPositions, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return jobPositions;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return jobPositions.Where(x => x.Name.Trim().ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<JobDirection> Search(this IQueryable<JobDirection> jobDirections, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return jobDirections;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return jobDirections.Where(x => x.Name.Trim().ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<Tag> Search(this IQueryable<Tag> tags, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return tags;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return tags.Where(x => x.Name.Trim().ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<StudentLog> Search(this IQueryable<StudentLog> studentLogs, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return studentLogs;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return studentLogs
            .Where(x =>
                x.FirstName.Trim().ToLower().Contains(lowerCaseTerm) ||
                x.LastName.Trim().ToLower().Contains(lowerCaseTerm)
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

    public static IQueryable<Student> Search(this IQueryable<Student> students, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return students;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return students.Where(x =>
            x.FirstName.ToLower().ToLower().Contains(lowerCaseTerm) ||
            x.LastName.ToLower().ToLower().Contains(lowerCaseTerm)
        );
    }

    public static IQueryable<Company> Search(this IQueryable<Company> companies, string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
            return companies;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return companies.Where(x => x.Name.Trim().ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<JobOffer> Search(this IQueryable<JobOffer> jobOffers, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return jobOffers;

        var lowerCaseTerm = searchTerm.NormalizeName();

        return jobOffers.Where(x => x.Title.ToLower().ToLower().Contains(lowerCaseTerm));
    }
}
