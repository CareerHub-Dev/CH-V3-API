﻿namespace Domain.Entities;

public class Company : Account
{
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? Banner { get; set; }
    public string Motto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<CompanyLink> Links { get; set; } = new List<CompanyLink>();
    public List<Student> SubscribedStudents { get; set; } = new List<Student>();
    public List<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
}
