﻿namespace Application.Companies.Query.Models;

public class CompanyBriefWithAmountStatisticWithVerificationInfoDTO : CompanyBriefWithAmountStatisticDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
