using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistics.Query.GetReviewsAnalitics
{
    public class ReviewsAnaluticsResult
    {
        public List<ReviewAnalitic> AllReviews { get; set; } = new List<ReviewAnalitic>();
        public List<ReviewAnalitic> AppvoredReviews { get; set; } = new List<ReviewAnalitic>();
        public List<ReviewAnalitic> RejectedReviews { get; set; } = new List<ReviewAnalitic>();
        public List<ReviewAnalitic> PendingReviews { get; set; } = new List<ReviewAnalitic>();
    }
}
