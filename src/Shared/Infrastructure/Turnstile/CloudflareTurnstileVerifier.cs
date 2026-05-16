using System.Net.Http.Json;
using System.Text.Json.Serialization;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EduCrm.Infrastructure.Turnstile;

public sealed class CloudflareTurnstileVerifier : ITurnstileVerifier
{
    private readonly HttpClient _http;
    private readonly TurnstileOptions _options;
    private readonly IHostEnvironment _env;
    private readonly ILogger<CloudflareTurnstileVerifier> _logger;

    public CloudflareTurnstileVerifier(
        HttpClient http,
        IOptions<TurnstileOptions> options,
        IHostEnvironment env,
        ILogger<CloudflareTurnstileVerifier> logger)
    {
        _http = http;
        _options = options.Value;
        _env = env;
        _logger = logger;
    }

    public async Task<Result> VerifyAsync(string? token, string? remoteIp, CancellationToken ct)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogDebug("Turnstile verification skipped (Development environment).");
            return Result.Success();
        }

        if (string.IsNullOrWhiteSpace(_options.SecretKey))
        {
            _logger.LogError("Turnstile:SecretKey is not configured.");
            return Result.Fail(CommonErrors.TurnstileFailed());
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail(CommonErrors.TurnstileFailed(new[] { "missing-input-response" }));
        }

        var form = new List<KeyValuePair<string, string>>
        {
            new("secret", _options.SecretKey),
            new("response", token)
        };
        if (!string.IsNullOrWhiteSpace(remoteIp))
            form.Add(new KeyValuePair<string, string>("remoteip", remoteIp));

        try
        {
            using var content = new FormUrlEncodedContent(form);
            using var response = await _http.PostAsync("turnstile/v0/siteverify", content, ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Turnstile siteverify returned non-success status {Status}.", (int)response.StatusCode);
                return Result.Fail(CommonErrors.TurnstileFailed());
            }

            var body = await response.Content.ReadFromJsonAsync<SiteverifyResponse>(cancellationToken: ct);
            if (body is null)
            {
                _logger.LogWarning("Turnstile siteverify returned empty body.");
                return Result.Fail(CommonErrors.TurnstileFailed());
            }

            if (body.Success) return Result.Success();

            _logger.LogInformation(
                "Turnstile verification rejected. Cloudflare error codes: {Errors}",
                body.ErrorCodes is null ? "(none)" : string.Join(",", body.ErrorCodes));
            return Result.Fail(CommonErrors.TurnstileFailed(body.ErrorCodes));
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Turnstile siteverify call failed.");
            return Result.Fail(CommonErrors.TurnstileFailed());
        }
    }

    private sealed record SiteverifyResponse(
        [property: JsonPropertyName("success")] bool Success,
        [property: JsonPropertyName("error-codes")] IReadOnlyList<string>? ErrorCodes);
}
