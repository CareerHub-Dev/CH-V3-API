﻿namespace WebUI.Common.Models.Account;

public class ChangePasswordRequest : IValidatableMarker
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}