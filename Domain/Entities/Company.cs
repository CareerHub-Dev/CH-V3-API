using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Companies")]
public class Company : Account
{
    [MaxLength(40)]
    public string CompanyName { get; set; } = string.Empty;
    public Guid? CompanyLogoId { get; set; }
    public Guid? CompanyBannerId { get; set; }
    [MaxLength(120)]
    public string CompanyMotto { get; set; } = string.Empty;
    [MaxLength(256)]
    public string CompanyDescription { get; set; } = string.Empty;
}
