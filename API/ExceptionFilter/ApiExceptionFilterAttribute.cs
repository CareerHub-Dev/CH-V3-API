using API.ExceptionFilter.Models;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Authentication;

namespace API.ExceptionFilter;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(AuthenticationException), HandleAuthenticationException },
                { typeof(ArgumentException), HandleArgumentException },
                { typeof(ArgumentOutOfRangeException), HandleArgumentOutOfRangeException },
                { typeof(BanException), HandleBanException },
                { typeof(FileNotFoundException), HandleFileNotFoundException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleAnyException(context);
    }

    private void HandleAnyException(ExceptionContext context)
    {
        var exception = context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
            Title = "Internal Server Error.",
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleAuthenticationException(ExceptionContext context)
    {
        var exception = (AuthenticationException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Authorization failed.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleBanException(ExceptionContext context)
    {
        var exception = (BanException)context.Exception;

        var details = new BanProblemDetails()
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3",
            Title = "Forbidden.",
            Detail = exception.Message,
            Status = StatusCodes.Status403Forbidden,
            Expires = exception.Expires,
            Reasone = exception.Reasone,
        };

        context.Result = new ObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleArgumentException(ExceptionContext context)
    {
        var exception = (ArgumentException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid argument.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleArgumentOutOfRangeException(ExceptionContext context)
    {
        var exception = (ArgumentOutOfRangeException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid argument.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleFileNotFoundException(ExceptionContext context)
    {
        var exception = (FileNotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

}
