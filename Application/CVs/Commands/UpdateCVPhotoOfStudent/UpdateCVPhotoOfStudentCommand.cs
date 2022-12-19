using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.UpdateCVPhotoOfStudent;

public record UpdateCVPhotoOfStudentCommand
    : IRequest<string?>
{
    public Guid CVId { get; init; }
    public IFormFile? Photo { get; init; }

    public Guid StudentId { get; init; }
}

public class UpdateCVPhotoOfStudentCommandHandler
    : IRequestHandler<UpdateCVPhotoOfStudentCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public UpdateCVPhotoOfStudentCommandHandler(
        IApplicationDbContext context,
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(
        UpdateCVPhotoOfStudentCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

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