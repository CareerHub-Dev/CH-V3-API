﻿namespace Application.Common.Models;

public class JwtToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
