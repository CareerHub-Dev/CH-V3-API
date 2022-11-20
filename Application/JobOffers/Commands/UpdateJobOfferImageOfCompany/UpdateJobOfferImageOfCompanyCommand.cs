using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;

public record UpdateJobOfferImageOfCompanyCommand
    : IRequest<string?>
{
    public Guid JobofferId { get; init; }
    public IFormFile? Image { get; init; }

    public Guid CompanyId { get; init; }
}

public class UpdateJobOfferImageOfCompanyCommandHandler
    : IRequestHandler<UpdateJobOfferImageOfCompanyCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public UpdateJobOfferImageOfCompanyCommandHandler(
        IApplicationDbContext context,
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(
        UpdateJobOfferImageOfCompanyCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var jobOffer = await _context.JobOffers
            .FirstOrDefaultAsync(x => x.Id == request.JobofferId && x.CompanyId == request.CompanyId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobofferId);
        }

        if (!string.IsNullOrWhiteSpace(jobOffer.Image))
        {
            _imagesService.DeleteImageIfExists(Path.GetFileName(jobOffer.Image));
            jobOffer.Image = null;
        }

        if (request.Image != null)
        {
            jobOffer.Image = await _imagesService.SaveImageAsync(request.Image);
        }

        await _context.SaveChangesAsync();

        return jobOffer.Image;
    }
}