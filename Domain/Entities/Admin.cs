namespace Domain.Entities;

public class Admin : Account
{
    public bool IsSuperAdmin { get; set; }
}
