using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EduCrm.Modules.Account.Application.Email;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Email;

public sealed class ResendEmailSender : IEmailSender
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _http;
    private readonly EmailOptions _options;

    public ResendEmailSender(HttpClient http, IOptions<EmailOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_options.Resend.ApiKey))
            throw new InvalidOperationException("Email:Resend:ApiKey is not configured.");
        if (string.IsNullOrWhiteSpace(_options.FromEmail))
            throw new InvalidOperationException("Email:FromEmail is not configured.");

        var from = string.IsNullOrWhiteSpace(_options.FromName)
            ? _options.FromEmail
            : $"{_options.FromName} <{_options.FromEmail}>";

        var payload = new ResendSendRequest(
            From: from,
            To: new[] { message.To },
            Subject: message.Subject,
            Html: message.HtmlBody,
            Text: message.TextBody);

        using var request = new HttpRequestMessage(HttpMethod.Post, "emails")
        {
            Content = JsonContent.Create(payload, options: SerializerOptions)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.Resend.ApiKey);

        using var response = await _http.SendAsync(request, ct);
        if (response.IsSuccessStatusCode) return;

        var body = await response.Content.ReadAsStringAsync(ct);
        throw new InvalidOperationException(
            $"Resend send failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
    }

    private sealed record ResendSendRequest(
        string From,
        IReadOnlyCollection<string> To,
        string Subject,
        string Html,
        string? Text);
}