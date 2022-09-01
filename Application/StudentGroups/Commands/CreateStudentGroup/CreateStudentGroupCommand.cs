using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.StudentGroups.Commands.CreateStudentGroup;

public record CreateStudentGroupCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
}

public class CreateStudentGroupCommandHandler : IRequestHandler<CreateStudentGroupCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStudentGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new StudentGroup
        {
            Name = request.Name,
        };

        await _context.StudentGroups.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity.Id;
    }
}
