﻿namespace WebUI.Common.Models.Account;

public class AuthenticateRequest
{
    public string Email { set; get; } = string.Empty;
    public string Password { set; get; } = string.Empty;
}
