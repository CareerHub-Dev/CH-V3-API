﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.UpdateJobOfferDetail;

public record UpdateJobOfferDetailCommand
    : IRequest
{
    public Guid JobOfferId { get; init; }

    public string Title { init; get; } = string.Empty;
    public string Overview { init; get; } = string.Empty;
    public string Requirements { init; get; } = string.Empty;
    public string Responsibilities { init; get; } = string.Empty;
    public string Preferences { init; get; } = string.Empty;

    public JobType JobType { get; init; }
    public WorkFormat WorkFormat { get; init; }
    public ExperienceLevel ExperienceLevel { get; init; }

    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Guid JobPositionId { get; init; }

    public List<Guid> TagIds { get; init; } = new List<Guid>();
}

public class UpdateJobOfferDetailCommandHandler
    : IRequestHandler<UpdateJobOfferDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobOfferDetailCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateJobOfferDetailCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobPositions
            .AnyAsync(x => x.Id == request.JobPositionId))
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        var tags = await GetTagsAsync(request.TagIds);

        var jobOffer = await _context.JobOffers
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        jobOffer.Title = request.Title;
        jobOffer.Overview = request.Overview;
        jobOffer.Requirements = request.Requirements;
        jobOffer.StartDate = request.StartDate;
        jobOffer.EndDate = request.EndDate;
        jobOffer.JobPositionId = request.JobPositionId;
        jobOffer.JobType = request.JobType;
        jobOffer.WorkFormat = request.WorkFormat;
        jobOffer.ExperienceLevel = request.ExperienceLevel;
        jobOffer.Preferences = request.Preferences;
        jobOffer.Tags = tags;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }

    private async Task<List<Tag>> GetTagsAsync(List<Guid> ids)
    {
        var tags = new List<Tag>();

        foreach (var id in ids)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null)
            {
                throw new NotFoundException(nameof(Tag), id);
            }

            tags.Add(tag);
        }

        return tags;
    }
}