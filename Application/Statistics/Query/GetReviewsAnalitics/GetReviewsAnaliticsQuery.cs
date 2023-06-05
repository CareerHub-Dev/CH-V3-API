using Application.Common.DTO.Posts;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Posts.Queries.GetAdmininstrationPostsWithPaging;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistics.Query.GetReviewsAnalitics;

public class GetReviewsAnaliticsQuery
    : IRequest<ReviewsAnaluticsResult>
{
    public ReviewsAnaliticRange Range { get; set; }
}

public class GetReviewsAnaliticsQueryHandler
    : IRequestHandler<GetReviewsAnaliticsQuery, ReviewsAnaluticsResult>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsAnaliticsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewsAnaluticsResult> Handle(
        GetReviewsAnaliticsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new ReviewsAnaluticsResult();

        switch (request.Range)
        {
            case ReviewsAnaliticRange.Week:
                {
                    var nowDate = DateTime.UtcNow;
                    var startDate = nowDate.AddDays(-7);

                    var range = GenerateDatesRange(startDate, nowDate);

                    var reviews = await _context.CVJobOffers.Where(x => x.Created > startDate).ToListAsync();

                    result.AllReviews = 
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date) }).ToList();
                    result.AppvoredReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Success) }).ToList();
                    result.RejectedReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Rejected) }).ToList();
                    result.PendingReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.In_progress) }).ToList();
                    break;
                }

            case ReviewsAnaliticRange.Month:
                {
                    var nowDate = DateTime.UtcNow;
                    var startDate = nowDate.AddDays(-31);

                    var range = GenerateDatesRange(startDate, nowDate);

                    var reviews = await _context.CVJobOffers.Where(x => x.Created > startDate).ToListAsync();

                    result.AllReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date) }).ToList();
                    result.AppvoredReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Success) }).ToList();
                    result.RejectedReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Rejected) }).ToList();
                    result.PendingReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.In_progress) }).ToList();
                    break;
                }

            case ReviewsAnaliticRange.Year:
                {
                    var nowDate = DateTime.UtcNow;
                    var startDate = nowDate.AddDays(-31);

                    var range = GenerateMonthsRange(startDate, nowDate);

                    var reviews = await _context.CVJobOffers.Where(x => x.Created > startDate).ToListAsync();

                    result.AllReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date) }).ToList();
                    result.AppvoredReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Success) }).ToList();
                    result.RejectedReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.Rejected) }).ToList();
                    result.PendingReviews =
                        range.Select(x => new ReviewAnalitic { DateTime = x, Amount = reviews.Count(y => y.Created.Date == x.Date && y.Status == Review.In_progress) }).ToList();
                    break;
                }

            case ReviewsAnaliticRange.AllTime:
                {
                    var statistic = await _context.CVJobOffers
                        .GroupBy(x => new { x.Created.Date.Month, x.Created.Date.Year })
                        .Select(x => new 
                        {
                            x.Key,
                            AllReviews = x.Count(),
                            AppvoredReviews = x.Count(y => y.Status == Review.Success),
                            RejectedReviews = x.Count(y => y.Status == Review.Rejected),
                            PendingReviews = x.Count(y => y.Status == Review.In_progress),
                        }).ToListAsync();

                    result.AllReviews =
                        statistic.Select(x => new ReviewAnalitic { DateTime = new DateTime(x.Key.Year, x.Key.Month, 1), Amount = x.AllReviews }).ToList();
                    result.AppvoredReviews =
                        statistic.Select(x => new ReviewAnalitic { DateTime = new DateTime(x.Key.Year, x.Key.Month, 1), Amount = x.AppvoredReviews }).ToList();
                    result.RejectedReviews =
                        statistic.Select(x => new ReviewAnalitic { DateTime = new DateTime(x.Key.Year, x.Key.Month, 1), Amount = x.RejectedReviews }).ToList();
                    result.PendingReviews =
                        statistic.Select(x => new ReviewAnalitic { DateTime = new DateTime(x.Key.Year, x.Key.Month, 1), Amount = x.PendingReviews }).ToList();
                    break;
                }
        }

        return result;
    }

    List<DateTime> GenerateDatesRange(DateTime startDate, DateTime endDate)
    {
        List<DateTime> dates = new List<DateTime>();

        DateTime currentDate = startDate;

        while (currentDate <= endDate)
        {
            dates.Add(currentDate);
            currentDate = currentDate.AddDays(1);
        }

        return dates;
    }

    List<DateTime> GenerateMonthsRange(DateTime startDate, DateTime endDate)
    {
        List<DateTime> months = new List<DateTime>();

        DateTime currentMonth = new DateTime(startDate.Year, startDate.Month, 1);

        while (currentMonth <= endDate)
        {
            months.Add(currentMonth);
            currentMonth = currentMonth.AddMonths(1);
        }

        return months;
    }
}