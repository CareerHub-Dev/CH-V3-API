using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.JobOffers.Commands.UpdateJobOfferImage;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.UpdateCVPhoto;

public record UpdateCVPhotoCommand
    : IRequest<string?>
{
    public Guid CVId { get; init; }
    public IFormFile? Photo { get; init; }
}

public class UpdateCVPhotoCommandHandler
    : IRequestHandler<UpdateCVPhotoCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public UpdateCVPhotoCommandHandler(
        IApplicationDbContext context,
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(
        UpdateCVPhotoCommand request,
        CancellationToken cancellationToken)
    {
        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        if (!string.IsNullOrWhiteSpace(cv.Photo))
        {
            _imagesService.DeleteImageIfExists(Path.GetFileName(cv.Photo));
            cv.Photo = null;
        }

        if (request.Photo != null)
        {
            cv.Photo = await _imagesService.SaveImageAsync(request.Photo);
        }

        await _context.SaveChangesAsync();

        return cv.Photo;
    }
}