using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Abstractions;

namespace Application.BackgroundServices;

public class RemoveOldImagesBackgroundService
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan interval = TimeSpan.FromDays(30);

    public RemoveOldImagesBackgroundService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var _scope = _scopeFactory.CreateScope();

            var context = _scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var fileSystem = _scope.ServiceProvider.GetRequiredService<IFileSystem>();

            var imageFileNamesInDB = await context.Companies.Select(x => x.Logo)
                .Union(context.Companies.Select(x => x.Banner))
                .Union(context.Students.Select(x => x.Photo))
                .Union(context.JobOffers.Select(x => x.Image))
                .Union(context.CVs.Select(x => x.Photo))
                .Where(x => x != null)
                .Select(x => fileSystem.Path.GetFileName(x))
                .ToListAsync();

            var imageService = _scope.ServiceProvider.GetRequiredService<IImagesService>();

            var imageFileNamesInStorage = imageService.GetImageFileNames();

            var exceptimageFileNames = imageFileNamesInStorage.Except(imageFileNamesInDB);

            imageService.DeleteImagesIfExist(exceptimageFileNames);

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(interval, stoppingToken);
        }
    }
}
