﻿namespace Application.Students.Queries.Models;

public class StudentBriefDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; }
}