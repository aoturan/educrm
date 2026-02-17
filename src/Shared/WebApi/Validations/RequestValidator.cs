using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EduCrm.WebApi.Validations;

public interface IRequestValidator
{
    Task<ValidationResult> ValidateAsync<T>(T request, CancellationToken ct);
}

public sealed class RequestValidator : IRequestValidator
{
    private readonly IServiceProvider _sp;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<RequestValidator> _logger;

    public RequestValidator(IServiceProvider sp, IWebHostEnvironment env, ILogger<RequestValidator> logger)
    {
        _sp = sp;
        _env = env;
        _logger = logger;
    }

    public async Task<ValidationResult> ValidateAsync<T>(T request, CancellationToken ct)
    {
        var validator = _sp.GetService<IValidator<T>>();
        if (validator is null)
            return new ValidationResult(); // validator yoksa valid say

        var result = await validator.ValidateAsync(request, ct);
        if (!result.IsValid && _env.IsDevelopment())
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogInformation("Request validation failed for {RequestType}: {Errors}", typeof(T).Name, errors);
        }

        return result;
    }
}