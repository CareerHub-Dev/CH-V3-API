using Application.Common.DTO.Tags;

namespace Application.Statistics.Query.GetTagsAnalitics
{
    public class TagAnaliticResult
    {
        public BriefTagDTO Tag { get; set; } = new BriefTagDTO();
        public int SavedJobOffers { get; set; }
        public int SentCV { get; set; }
    }
}
