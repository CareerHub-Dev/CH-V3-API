namespace Application.Common.Interfaces;

public interface IProcedureService
{
    Task<string> GenerateAccountVerificationTokenAsync();
    Task<string> GenerateAccountResetTokenAsync();
}
