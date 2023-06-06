using Application.Common.Interfaces;
using Application.Statistics.Query.GetReviewsAnalitics;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistics.Query.GetTagsAnalitics
{
    public record GetTagsAnaliticsQuery : IRequest<TagsAnaliticsResult>
    {
        public List<Guid>? Ids { get; set; } = new List<Guid>();
    }

    public class GetTagsAnaliticsQueryHandler
    : IRequestHandler<GetTagsAnaliticsQuery, TagsAnaliticsResult>
    {
        private readonly IApplicationDbContext _context;

        public GetTagsAnaliticsQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TagsAnaliticsResult> Handle(
            GetTagsAnaliticsQuery request,
            CancellationToken cancellationToken)
        {
            var tags = await _context.Tags
                .Where(x => request.Ids != null && request.Ids.Any() ? request.Ids.Contains(x.Id) : true)
                .Select(x => new TagAnaliticResult
                {
                    Tag = new Common.DTO.Tags.BriefTagDTO { Id = x.Id, Name = x.Name },
                    SavedJobOffers = x.JobOffers.Count(),
                    SentCV = x.JobOffers.Sum(x => x.CVJobOffers.Count()),
                }).ToListAsync();

            return new TagsAnaliticsResult { TagsAnalitics = tags };
        }
    }
}
