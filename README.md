# CareerHub API

## Introduction

This API project is a closed system for [KhNURE](https://nure.ua/) students and companies. The main goal of the project is to help students to find a job.
Omit the general functionality and talk about the main features:
* Student:
  * Create a CV file and apply it to a job offer.
* Company: 
  * Create job offer
* Admin:
  * Managing users and common subjects

The project is still under development so it will get new functions like statistics, notifications, and chat.

## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)
* [Bcrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net)
* [PostgreSQL](https://www.postgresql.org/)

## Database Configuration

To interact with the database, you need to update **API/appsettings.Development.json** or **API/appsettings.Production.json** (depending on the project configuration) as follows:

```json
  "ConnectionStrings": {
    "DefaultConnection": "Host=*;Database=*;Username=*;Password=*"
  }
```

Verify that the **DefaultConnection** connection string within **appsettings.Development.json** points to a valid PostgreSQL instance. 

When you run the application the database will be automatically created (if necessary and in Development mode) and the latest migrations will be applied.

## Database Migrations

To use `dotnet-ef` for your migrations.
Then, add the following flags to your command (values assume you are executing from repository root)

* `--project Infrastructure` (optional if in this folder)
* `--startup-project API`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project Infrastructure --startup-project API --output-dir Persistence\Migrations`
