﻿namespace WebUI.Common.Models.Account;

public class ForgotPasswordRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}