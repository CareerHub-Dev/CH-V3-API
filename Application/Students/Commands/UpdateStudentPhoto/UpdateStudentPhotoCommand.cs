using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudentPhoto;

public record UpdateStudentPhotoCommand : IRequest<string?>
{
    public Guid StudentId { get; init; }
    public IFormFile? Photo { get; init; }
}

public class UpdateStudentPhotoCommandHandler : IRequestHandler<UpdateStudentPhotoCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;
    public UpdateStudentPhotoCommandHandler(IApplicationDbContext context, IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(UpdateStudentPhotoCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if (!string.IsNullOrWhiteSpace(student.Photo))
        {
            _imagesService.DeleteImageIfExists(Path.GetFileName(student.Photo));

            student.Photo = null;
        }

        if (request.Photo != null)
        {
            student.Photo = await _imagesService.SaveImageAsync(request.Photo);
        }

        await _context.SaveChangesAsync();

        return student.Photo;
    }
}