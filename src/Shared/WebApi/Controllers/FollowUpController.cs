using EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;
using EduCrm.Modules.People.Application.UseCases.CreateFollowUp;
using EduCrm.Modules.People.Application.UseCases.GetFollowUpById;
using EduCrm.Modules.People.Application.UseCases.ListFollowUps;
using EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;
using EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;
using EduCrm.Modules.People.Application.UseCases.UpdateFollowUp;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.WebApi.Contracts.Person;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/follow-up")]
public class FollowUpController : ControllerBase
{
    private readonly IChangeFollowUpStatusService _changeStatus;
    private readonly ICreateFollowUpService _create;
    private readonly IGetFollowUpByIdService _getById;
    private readonly IListFollowUpsService _list;
    private readonly IRescheduleDueDateService _reschedule;
    private readonly ISnoozeFollowUpService _snooze;
    private readonly IUpdateFollowUpService _update;
    private readonly IRequestValidator _validator;

    public FollowUpController(
        IChangeFollowUpStatusService changeStatus,
        ICreateFollowUpService create,
        IGetFollowUpByIdService getById,
        IListFollowUpsService list,
        IRescheduleDueDateService reschedule,
        ISnoozeFollowUpService snooze,
        IUpdateFollowUpService update,
        IRequestValidator validator)
    {
        _changeStatus = changeStatus;
        _create = create;
        _getById = getById;
        _list = list;
        _reschedule = reschedule;
        _snooze = snooze;
        _update = update;
        _validator = validator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateFollowUpRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateFollowUpInput(
            req.PersonId,
            req.Type,
            req.Title,
            req.DueAtUtc,
            req.ProgramId,
            req.Note);

        var result = await _create.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateFollowUpResponse(r.FollowUpId)));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getById.GetAsync(id, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new GetFollowUpByIdResponse(
                r.Id,
                r.OrganizationId,
                r.Type,
                r.Status,
                r.Title,
                r.Note,
                r.DueAtUtc,
                r.SnoozedUntilUtc,
                r.CompletedAtUtc,
                r.CancelledAtUtc,
                new FollowUpPersonResponse(r.Person.Id, r.Person.FullName, r.Person.Email, r.Person.Phone),
                r.Program is null ? null : new FollowUpProgramResponse(r.Program.Id, r.Program.Name))));
    }

    [HttpPost("{id:guid}/update")]
    [Authorize]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateFollowUpRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdateFollowUpInput(
            id,
            req.PersonId,
            req.Type,
            req.Title,
            req.DueAtUtc,
            req.ProgramId,
            req.Note);

        var result = await _update.UpdateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UpdateFollowUpResponse(r.FollowUpId)));
    }

    [HttpPost("{id:guid}/complete")]
    [Authorize]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        var input = new ChangeFollowUpStatusInput(id, FollowUpStatus.Completed);
        var result = await _changeStatus.ChangeAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ChangeFollowUpStatusResponse(r.FollowUpId, r.Status, r.CompletedAtUtc, r.CancelledAtUtc)));
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var input = new ChangeFollowUpStatusInput(id, FollowUpStatus.Cancelled);
        var result = await _changeStatus.ChangeAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ChangeFollowUpStatusResponse(r.FollowUpId, r.Status, r.CompletedAtUtc, r.CancelledAtUtc)));
    }

    [HttpPost("{id:guid}/snooze")]
    [Authorize]
    public async Task<IActionResult> Snooze(
        Guid id,
        [FromBody] SnoozeFollowUpRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var result = await _snooze.SnoozeAsync(new SnoozeFollowUpInput(id, req.SnoozeUntilUtc), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new SnoozeFollowUpResponse(r.FollowUpId, r.SnoozedUntilUtc)));
    }

    [HttpPost("{id:guid}/reschedule")]
    [Authorize]
    public async Task<IActionResult> Reschedule(
        Guid id,
        [FromBody] RescheduleDueDateRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var result = await _reschedule.RescheduleAsync(new RescheduleDueDateInput(id, req.DueAtUtc), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new RescheduleDueDateResponse(r.FollowUpId, r.DueAtUtc, r.Status, r.SnoozedUntilUtc)));
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? preFilter = null,
        [FromQuery] string? status = null,
        [FromQuery] Guid? personId = null,
        [FromQuery] Guid? programId = null,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 100) pageSize = 10;

        IReadOnlyList<FollowUpType>? typeFilter = null;
        if (!string.IsNullOrWhiteSpace(preFilter))
        {
            var types = preFilter
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(v => Enum.TryParse<FollowUpType>(v, ignoreCase: true, out var t) ? t : (FollowUpType?)null)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .ToList();

            if (types.Count > 0) typeFilter = types;
        }

        IReadOnlyList<FollowUpStatus>? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            var statuses = status
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(v => Enum.TryParse<FollowUpStatus>(v, ignoreCase: true, out var s) ? s : (FollowUpStatus?)null)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .ToList();

            if (statuses.Count > 0) statusFilter = statuses;
        }

        var input = new ListFollowUpsInput(page, pageSize, typeFilter, statusFilter, personId, programId);
        var result = await _list.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new FollowUpListPagedResponse(
                r.Items.Select(x => new FollowUpListItemResponse(
                    x.Id,
                    x.PersonName,
                    x.ProgramName,
                    x.Type,
                    x.Status,
                    x.Title,
                    x.DueAtUtc,
                    x.SnoozedUntilUtc)).ToList(),
                page,
                pageSize,
                r.TotalCount)));
    }
}