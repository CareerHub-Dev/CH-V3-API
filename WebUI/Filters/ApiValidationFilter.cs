using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebUI.DTO;

namespace WebUI.Filters;

public class ApiValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context.ActionArguments.SingleOrDefault(x => x.Value is IValidatableMarker);

        if (param.Value != null)
        {
            var genericValidatorType = typeof(IValidator<>);
            var validatorOfSpecificType = genericValidatorType.MakeGenericType(param.Value.GetType());

            var validator = context.HttpContext.RequestServices.GetService(validatorOfSpecificType);

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(param.Value);

                var validationResult = ((IValidator)validator).Validate(validationContext);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(validationResult.ToDictionary())
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    });
                    return;
                }
            }
        }

        await next();
    }
}
