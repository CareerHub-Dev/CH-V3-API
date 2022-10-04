using Application.Common.DTO.Companies;

namespace Application.Common.DTO.JobOffers;

public class DetailedJobOfferDTO
{
    public Guid Id { get; set; }
    public string Title { set; get; } = string.Empty;
    public Guid? ImageId { set; get; }
    public DateTime EndDate { get; set; }

    public BriefCompanyDTO Company { get; set; } = new BriefCompanyDTO();
}
