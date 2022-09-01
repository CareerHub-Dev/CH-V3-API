using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Image;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudentPhoto;

public record UpdateStudentPhotoCommand : IRequest<Guid?>
{
    public Guid StudentId { get; init; }
    public CreateImage? Photo { get; init; }
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
        var entity = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if(entity.PhotoId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.PhotoId);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        var newImage = request.Photo?.ToImageWithGeneratedId;

        if(newImage != null)
        {
            await _context.Images.AddAsync(newImage);
        }

        entity.PhotoId = newImage?.Id;

        await _context.SaveChangesAsync();

        return entity.PhotoId;
    }
}