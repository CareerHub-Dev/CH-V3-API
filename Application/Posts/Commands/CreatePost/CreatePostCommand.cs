using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.CreatePost;

public record CreatePostCommand 
    : IRequest<Guid>
{
    public Guid AccountId { get; init; }
    public string Text { get; init; } = string.Empty;
    public List<IFormFile> Images { get; init; } = new List<IFormFile>();
}

public class CreatePostCommandHandler 
    : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public CreatePostCommandHandler(
        IApplicationDbContext context, 
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<Guid> Handle(
        CreatePostCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var post = new Post
        {
            AccoundId = request.AccountId,
            Text = request.Text,
            CreatedDate = DateTime.UtcNow,
        };

        foreach (var image in request.Images)
        {
            post.Images.Add(await _imagesService.SaveImageAsync(image));
        }

        await _context.Posts.AddAsync(post);

        await _context.SaveChangesAsync();

        return post.Id;
    }
}