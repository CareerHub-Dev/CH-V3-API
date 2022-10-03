﻿using Domain.Enums;

namespace Application.Common.DTO.Students;

public class StudentDTO : DetailedStudentDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public ActivationStatus ActivationStatus { get; set; }
}
