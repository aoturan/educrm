using EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;
using EduCrm.WebApi.Contracts.Support;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/support")]
public class SupportController : ControllerBase
{
    private readonly ICreateSupportRequestService _create;
    private readonly IRequestValidator _validator;

    public SupportController(
        ICreateSupportRequestService create,
        IRequestValidator validator)
    {
        _create = create;
        _validator = validator;
    }

    [HttpPost]
    [Authorize]
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
}