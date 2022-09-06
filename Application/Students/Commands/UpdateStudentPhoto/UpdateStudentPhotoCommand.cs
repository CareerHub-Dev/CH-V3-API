using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudentPhoto;

public record UpdateStudentPhotoCommand : IRequest<Guid?>
{
    public Guid StudentId { get; init; }
    public IFormFile? Photo { get; init; }
}

public class UpdateStudentPhotoCommandHandler : IRequestHandler<UpdateStudentPhotoCommand, Guid?>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentPhotoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> Handle(UpdateStudentPhotoCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if(student.PhotoId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == student.PhotoId);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        if (request.Photo != null)
        {
            var imagePhoto = await request.Photo.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(imagePhoto);
            student.PhotoId = imagePhoto.Id;
        }
        else
        {
            student.PhotoId = null;
        }

        await _context.SaveChangesAsync();

        return student.PhotoId;
    }
}