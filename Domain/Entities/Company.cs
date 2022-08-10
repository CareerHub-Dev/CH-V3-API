namespace Domain.Entities;

public class Company : Account
{
    public string CompanyName { get; set; } = string.Empty;
    public Guid? CompanyLogoId { get; set; }
    public Guid? CompanyBannerId { get; set; }
    public string CompanyMotto { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
}
