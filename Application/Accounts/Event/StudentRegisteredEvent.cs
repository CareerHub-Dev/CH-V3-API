using Application.Emails.Commands.SendVerifyStudentEmail;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Event;

public class StudentRegisteredEvent : INotification
{
    public StudentRegisteredEvent(Student student)
    {
        Student = student;
    }

    public Student Student { get; }
}

public class StudentRegisteredEventHandler : INotificationHandler<StudentRegisteredEvent>
{
    private readonly IMediator _mediator;

    public StudentRegisteredEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(StudentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendVerifyStudentEmailCommand(notification.Student.Id));
    }
}
