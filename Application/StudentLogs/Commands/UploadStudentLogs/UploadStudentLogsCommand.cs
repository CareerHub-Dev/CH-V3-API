using Application.Common.DTO.StudentLogs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.UploadStudentLogs;

public record UploadStudentLogsCommand(IFormFile File)
    : IRequest<UploadStudentLogsResult>;

public class UploadStudentLogsCommandHandler
    : IRequestHandler<UploadStudentLogsCommand, UploadStudentLogsResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IStudentInfoParserService _studentInfoParserService;
    private readonly IValidator<UploadStudentLogModel> _uploadValidator;

    public UploadStudentLogsCommandHandler(
        IApplicationDbContext context,
        IStudentInfoParserService studentInfoParserService,
        IValidator<UploadStudentLogModel> uploadValidator)
    {
        _context = context;
        _studentInfoParserService = studentInfoParserService;
        _uploadValidator = uploadValidator;
    }

    public async Task<UploadStudentLogsResult> Handle(
        UploadStudentLogsCommand request,
        CancellationToken cancellationToken)
    {
        using var streamReader = new StreamReader(request.File.OpenReadStream());
        var uploadStudentLogs = _studentInfoParserService.Parse(streamReader).Select(x => new UploadStudentLogModel()
        {
            Email = x.Email,
            FullName = x.FullName,
            Group = x.Group,
        }).ToList();

        var result = new UploadStudentLogsResult() { Total = uploadStudentLogs.Count };

        if (uploadStudentLogs.Any())
        {
            var studentGroups = await _context.StudentGroups.AsNoTracking().ToDictionaryAsync(x => x.Name.Trim().ToLower());
            var students = await _context.Students.ToDictionaryAsync(x => x.NormalizedEmail);

            foreach (var uploadStudentLog in uploadStudentLogs)
            {
                var validate = await _uploadValidator.ValidateAsync(uploadStudentLog);

                var normalizedGroup = uploadStudentLog.Group.Trim().ToLower();
                var isStudentGroupExist = studentGroups.ContainsKey(normalizedGroup);

                if (!validate.IsValid || !isStudentGroupExist)
                {
                    result.InvalidAmount++;

                    var errors = validate.Errors.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                        .SelectMany(x => x).ToList();

                    if (!isStudentGroupExist)
                    {
                        errors.Add("Student group does not exist");
                    }

                    result.NoPassedUploadStudentLogs.Add(new NoPassedUploadStudentLog
                    {
                        UploadStudentLog = uploadStudentLog,
                        Errors = errors
                    });
                    continue;
                }

                var normalizedEmail = uploadStudentLog.Email.Trim().ToLower();

                var student = students.GetValueOrDefault(normalizedEmail);

                if (student != null)
                {
                    student.FirstName = uploadStudentLog.FirstName;
                    student.LastName = uploadStudentLog.LastName;
                    student.StudentGroupId = studentGroups[normalizedGroup].Id;

                    result.UpdatedAmount++;
                    continue;
                }

                var studentLogs = await _context.StudentLogs.ToDictionaryAsync(x => x.NormalizedEmail);
                var studentLog = studentLogs.GetValueOrDefault(normalizedEmail);

                if (studentLog != null)
                {
                    studentLog.FirstName = uploadStudentLog.FirstName;
                    studentLog.LastName = uploadStudentLog.LastName;
                    studentLog.StudentGroupId = studentGroups[normalizedGroup].Id;

                    result.UpdatedAmount++;
                }
                else
                {
                    var newStudentLog = new StudentLog
                    {
                        FirstName = uploadStudentLog.FirstName,
                        LastName = uploadStudentLog.LastName,
                        StudentGroupId = studentGroups[normalizedGroup].Id,
                        Email = uploadStudentLog.Email,
                        NormalizedEmail = normalizedEmail,
                    };

                    await _context.StudentLogs.AddAsync(newStudentLog);

                    result.AddedAmount++;
                }
            }

            await _context.SaveChangesAsync();
        }

        return result;
    }
}