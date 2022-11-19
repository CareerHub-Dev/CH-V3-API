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
    public string Title { init; get; } = string.Empty;
    public string Overview { init; get; } = string.Empty;
    public string Requirements { init; get; } = string.Empty;
    public string Responsibilities { init; get; } = string.Empty;
    public string Preferences { init; get; } = string.Empty;
    public IFormFile? Image { get; init; }

    public JobType JobType { get; init; }
    public WorkFormat WorkFormat { get; init; }
    public ExperienceLevel ExperienceLevel { get; init; }

    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Guid CompanyId { get; init; }
    public Guid JobPositionId { get; init; }

    public List<Guid> TagIds { get; init; } = new List<Guid>();
}

public class CreateJobOfferCommandHandler 
    : IRequestHandler<CreateJobOfferCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public CreateJobOfferCommandHandler(
        IApplicationDbContext context, 
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
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

        if (request.Image != null)
        {
            jobOffer.Image = await _imagesService.SaveImageAsync(request.Image);
        }

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

            if (tag == null)
            {
                throw new NotFoundException(nameof(Tag), id);
            }

            tags.Add(tag);
        }

        return tags;
    }
}
