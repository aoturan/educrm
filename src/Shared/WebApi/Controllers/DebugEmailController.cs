using EduCrm.Modules.Account.Application.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

// TEMPORARY: remove once email integration is verified end-to-end.
[ApiController]
[Route("api/_debug/email")]
public sealed class DebugEmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public DebugEmailController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpPost("send")]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromQuery] string? to, CancellationToken ct)
    {
        var recipient = string.IsNullOrWhiteSpace(to) ? "akin.turan@yahoo.com" : to;

        await _emailSender.SendAsync(
            new EmailMessage(
                To: recipient,
                Subject: "EduCRM email test",
                HtmlBody: "<p>This is a <strong>test</strong> from EduCRM via Resend.</p>",
                TextBody: "This is a test from EduCRM via Resend."),
            ct);

        return Ok(new { ok = true, to = recipient });
    }
}