using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Extensions;

public static class FluentValidationExtensions
{
    public static IActionResult ToValidationProblem(
        this ValidationResult result,
        ControllerBase controller)
    {
        if (result.IsValid)
            throw new InvalidOperationException("Cannot create validation problem from valid result.");
        
        var errors = result.Errors
            .Where(e => e is not null)
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
            );

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };
    
        return controller.ValidationProblem(problem);
    }
}