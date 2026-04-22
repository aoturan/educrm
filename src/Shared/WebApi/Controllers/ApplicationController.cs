using EduCrm.Modules.Program.Application.UseCases.AssignPersonToApplication;
using EduCrm.Modules.Program.Application.UseCases.CloseApplication;
using EduCrm.Modules.Program.Application.UseCases.ContactApplication;
using EduCrm.Modules.Program.Application.UseCases.ConvertApplication;
using EduCrm.Modules.Program.Application.UseCases.CreateApplication;
using EduCrm.Modules.Program.Application.UseCases.FindPersonsForApplication;
using EduCrm.Modules.Program.Application.UseCases.GetApplicationById;
using EduCrm.Modules.Program.Application.UseCases.ListApplications;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.WebApi.Contracts.Application;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/application")]
public class ApplicationController : ControllerBase
{
    private readonly IAssignPersonToApplicationService _assignPerson;
    private readonly ICloseApplicationService _close;
    private readonly IContactApplicationService _contact;
    private readonly IConvertApplicationService _convert;
    private readonly ICreateApplicationService _createApplication;
    private readonly IFindPersonsForApplicationService _findPersons;
    private readonly IGetApplicationByIdService _getById;
    private readonly IListApplicationsService _list;
    private readonly IRequestValidator _validator;

    public ApplicationController(
        IAssignPersonToApplicationService assignPerson,
        ICloseApplicationService close,
        IContactApplicationService contact,
        IConvertApplicationService convert,
        ICreateApplicationService createApplication,
        IFindPersonsForApplicationService findPersons,
        IGetApplicationByIdService getById,
        IListApplicationsService list,
        IRequestValidator validator)
    {
        _assignPerson = assignPerson;
        _close = close;
        _contact = contact;
        _convert = convert;
        _createApplication = createApplication;
        _findPersons = findPersons;
        _getById = getById;
        _list = list;
        _validator = validator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
        [FromBody] CreateApplicationRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateApplicationInput(
            req.ProgramSlug,
            req.SubmittedFullName,
            req.SubmittedPhone,
            req.SubmittedEmail,
            req.SubmittedMessage);

        var result = await _createApplication.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateApplicationResponse(r.ApplicationId)));
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? preFilter = null,
        [FromQuery] Guid? programId = null,
        [FromQuery] bool isBrief = false,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;

        var statuses = preFilter?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => Enum.TryParse<ApplicationStatus>(s, ignoreCase: true, out var status) ? status : (ApplicationStatus?)null)
            .Where(s => s.HasValue)
            .Select(s => s!.Value)
            .ToList();

        var input = new ListApplicationsInput(page, pageSize, statuses?.Count > 0 ? statuses : null, programId, isBrief);
        var result = await _list.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ApplicationListResponse(
                r.Items.Select(x => new ApplicationListItemResponse(
                    x.Id,
                    x.ProgramId,
                    x.SubmittedFullName,
                    x.SubmittedPhone,
                    x.SubmittedEmail,
                    x.ProgramName,
                    x.Status,
                    x.LastSubmittedAtUtc,
                    x.SubmissionCount)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpGet("{applicationId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid applicationId, CancellationToken ct)
    {
        var result = await _getById.GetAsync(applicationId, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new GetApplicationByIdResponse(
                r.Id,
                r.Status,
                r.SubmittedFullName,
                r.SubmittedPhone,
                r.SubmittedMessage,
                r.FirstSubmittedAtUtc,
                r.LastSubmittedAtUtc,
                r.SubmissionCount,
                r.ConvertedAtUtc,
                r.ClosedAtUtc,
                r.ClosedNote,
                r.Person is null ? null : new ApplicationPersonInfoResponse(r.Person.PersonId, r.Person.FullName),
                r.Program is null ? null : new ApplicationProgramInfoResponse(r.Program.ProgramId, r.Program.Name))));
    }

    [HttpPost("{applicationId:guid}/assign-person")]
    [Authorize]
    public async Task<IActionResult> AssignPerson(Guid applicationId, CancellationToken ct)
    {
        var result = await _assignPerson.AssignAsync(new AssignPersonInput(applicationId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new AssignPersonResponse(
                r.IsAmbiguous,
                r.Candidates.Select(c => new PersonCandidateResponse(
                    c.PersonId,
                    c.FullName,
                    c.Email,
                    c.Phone)).ToList())));
    }

    [HttpGet("{applicationId:guid}/persons")]
    [Authorize]
    public async Task<IActionResult> FindPersons(Guid applicationId, CancellationToken ct)
    {
        var result = await _findPersons.ResolveAsync(new FindPersonsForApplicationInput(applicationId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(r.Persons.Select(p => new PersonCandidateResponse(
                p.PersonId,
                p.FullName,
                p.Email,
                p.Phone)).ToList()));
    }   

    [HttpPost("{applicationId:guid}/contact")]
    [Authorize]
    public async Task<IActionResult> Contact(
        Guid applicationId,
        [FromBody] ContactApplicationRequest req,
        CancellationToken ct)
    {
        var result = await _contact.ContactAsync(
            new ContactApplicationInput(applicationId, req.PersonId), ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("{applicationId:guid}/close")]
    [Authorize]
    public async Task<IActionResult> Close(
        Guid applicationId,
        [FromBody] CloseApplicationRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var result = await _close.CloseAsync(
            new CloseApplicationInput(applicationId, req.ClosedNote), ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("{applicationId:guid}/convert")]
    [Authorize]
    public async Task<IActionResult> Convert(
        Guid applicationId,
        [FromBody] ConvertApplicationRequest req,
        CancellationToken ct)
    {
        var result = await _convert.ConvertAsync(
            new ConvertApplicationInput(applicationId, req.PersonId), ct);

        return result.ToActionResult(HttpContext, this);
    }
}