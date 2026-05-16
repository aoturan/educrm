using EduCrm.Infrastructure.Turnstile;
using EduCrm.Modules.Support.Application.UseCases.CreateSupportContactMessage;
using EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;
using EduCrm.WebApi.Contracts.Support;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Helpers;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/support")]
public class SupportController : ControllerBase
{
    private readonly ICreateSupportRequestService _create;
    private readonly ICreateSupportContactMessageService _createContactMessage;
    private readonly IRequestValidator _validator;
    private readonly ITurnstileVerifier _turnstile;

    public SupportController(
        ICreateSupportRequestService create,
        ICreateSupportContactMessageService createContactMessage,
        IRequestValidator validator,
        ITurnstileVerifier turnstile)
    {
        _create = create;
        _createContactMessage = createContactMessage;
        _validator = validator;
        _turnstile = turnstile;
    }

    [HttpPost]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> Create(
        [FromBody] CreateSupportRequestRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateSupportRequestInput(req.Subject, req.Message, req.PageUrl);
        var result = await _create.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateSupportRequestResponse(r.SupportRequestId)));
    }

    [HttpPost("contact-messages")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateContactMessage(
        [FromBody] CreateSupportContactMessageRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("public.contact.ip", RateLimitKey.Ip(HttpContext)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var turnstile = await _turnstile.VerifyAsync(req.TurnstileToken, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        if (turnstile.IsFailure) return turnstile.ToActionResult(HttpContext, this);

        var input = new CreateSupportContactMessageInput(req.FullName, req.Email, req.Subject, req.Message);
        var result = await _createContactMessage.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateSupportContactMessageResponse(r.SupportContactMessageId)));
    }
}
