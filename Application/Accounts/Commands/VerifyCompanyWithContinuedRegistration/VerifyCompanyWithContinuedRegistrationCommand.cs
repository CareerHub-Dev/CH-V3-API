﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;

public record VerifyCompanyWithContinuedRegistrationCommand : IRequest
{
    public string Token { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;
    public string CompanyMotto { get; init; } = string.Empty;
    public string CompanyDescription { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class VerifyCompanyWithContinuedRegistrationCommandHandler : IRequestHandler<VerifyCompanyWithContinuedRegistrationCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyCompanyWithContinuedRegistrationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyCompanyWithContinuedRegistrationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.Token);
        }

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;
        entity.CompanyName = request.CompanyName;
        entity.CompanyMotto = request.CompanyMotto;
        entity.CompanyDescription = request.CompanyDescription;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}