﻿namespace WebUI.Common.Models.Company;

public record UpdateOwnCompanyAccountView
{
    public string Name { get; init; } = string.Empty;
    public string Motto { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}