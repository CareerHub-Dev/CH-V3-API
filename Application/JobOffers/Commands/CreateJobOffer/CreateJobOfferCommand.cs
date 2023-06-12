using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.CreateJobOffer;

public record CreateJobOfferCommand
    : IRequest<Guid>
{
    public required string Title { init; get; }
    public required string Overview { init; get; } 
    public required string Requirements { init; get; }
    public required string Responsibilities { init; get; }
    public required string Preferences { init; get; }
    public required JobType JobType { get; init; }
    public required WorkFormat WorkFormat { get; init; }
    public required ExperienceLevel ExperienceLevel { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required Guid CompanyId { get; init; }
    public required Guid JobPositionId { get; init; }
    public required List<Guid> TagIds { get; init; }
}

public class CreateJobOfferCommandHandler
    : IRequestHandler<CreateJobOfferCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateJobOfferCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateJobOfferCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (!await _context.JobPositions
            .AnyAsync(x => x.Id == request.JobPositionId))
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        var tags = await GetTagsAsync(request.TagIds);

        var jobOffer = new JobOffer
        {
            Title = request.Title,
            Overview = request.Overview,
            Requirements = request.Requirements,
            Responsibilities = request.Responsibilities,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CompanyId = request.CompanyId,
            JobPositionId = request.JobPositionId,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            Preferences = request.Preferences,
            Tags = tags,
        };

        await _context.JobOffers.AddAsync(jobOffer);
        await _context.SaveChangesAsync();

        return jobOffer.Id;
    }

    private async Task<List<Tag>> GetTagsAsync(List<Guid> ids)
    {
        var tags = new List<Tag>();

        foreach (var id in ids)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag is null)
            {
                throw new NotFoundException(nameof(Tag), id);
            }

            tags.Add(tag);
        }

        return tags;
    }
}
