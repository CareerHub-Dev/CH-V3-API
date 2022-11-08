namespace Application.Common.Exceptions;

public class BanException : Exception
{
    public string Reasone { get; set; }
    public DateTime Expires { get; set; }

    public BanException(string reasone, DateTime expires)
        : base("Dear user, your account has been banned.")
    {
        Reasone = reasone;
        Expires = expires;
    }
}
