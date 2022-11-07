using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.BackgroundServices;

public class RemoveOldImagesBackgroundService : BackgroundService
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
            using (var _scope = _scopeFactory.CreateScope())
            {
                var _context = _scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var imageFileNames = await _context.Companies.Select(x => x.Logo)
                    .Union(_context.Companies.Select(x => x.Banner))
                    .Union(_context.Students.Select(x => x.Photo))
                    .Union(_context.JobOffers.Select(x => x.Image))
                    .Union(_context.CVs.Select(x => x.Photo))
                    .Where(x => x != null)
                    .Select(x => Path.GetFileName(x))
                    .ToListAsync();



                await _context.SaveChangesAsync(stoppingToken);
            }
            await Task.Delay(interval, stoppingToken);
        }
    }
}
