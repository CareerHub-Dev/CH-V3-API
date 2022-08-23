namespace Application.Common.Interfaces;

public interface IProcedureService
{
    Task<string> GenerateAccountVerificationTokenAsync(CancellationToken cancellationToken = default);
    Task<string> GenerateAccountResetTokenAsync(CancellationToken cancellationToken = default);
}
