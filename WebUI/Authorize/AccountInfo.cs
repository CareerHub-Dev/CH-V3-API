﻿using Domain.Enums;

namespace WebUI.Authorize;

public class AccountInfo
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public ActivationStatus ActivationStatus { get; set; }
}
