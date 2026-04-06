using EduCrm.Modules.People.Application.UseCases.ArchivePerson;
using EduCrm.Modules.People.Application.UseCases.Create;
using EduCrm.Modules.People.Application.UseCases.GetById;
using EduCrm.Modules.People.Application.UseCases.ListPersons;
using EduCrm.Modules.People.Application.UseCases.UpdatePerson;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.WebApi.Contracts.Person;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController : ControllerBase
{
    private readonly IArchivePersonService _archive;
    private readonly ICreatePersonService _create;
    private readonly IGetPersonByIdService _getById;
    private readonly IListPersonsService _list;
    private readonly IUpdatePersonService _update;
    private readonly IRequestValidator _validator;

    public PersonController(
        IArchivePersonService archive,
        ICreatePersonService create,
        IGetPersonByIdService getById,
        IListPersonsService list,
        IUpdatePersonService update,
        IRequestValidator validator)
    {
        _archive = archive;
        _create = create;
        _getById = getById;
        _list = list;
        _update = update;
        _validator = validator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreatePersonRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateInput(
            req.FullName,
            SourceType.Manual,
            req.Phone,
            req.Email,
            req.Notes,
            req.ProgramId);

        var result = await _create.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new CreatePersonResponse(r.PersonId)));
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] string? q = null,
        [FromQuery] string? preFilter = null,
        [FromQuery] bool showArchived = false,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 5;

        var input = new ListPersonsInput(page, pageSize, q, preFilter, showArchived);
        var result = await _list.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new PersonListResponse(
                r.Items.Select(x => new PersonListItemResponse(
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Phone,
                    x.EnrolledProgramCount,
                    x.HasActiveEnrollment,
                    x.IsArchived)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount,
                r.EnrolledCount,
                r.NotEnrolledCount)));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getById.GetAsync(id, ct);

        return result.ToActionResult(HttpContext, this, p =>
            Ok(new GetPersonByIdResponse(
                p.Id,
                p.FullName,
                p.Email,
                p.Phone,
                p.JoinedDate,
                p.Notes,
                p.EnrolledPrograms.Select(x => new PersonEnrolledProgramResponse(
                    x.Id,
                    x.Name,
                    x.StartDate,
                    x.EndDate,
                    x.Status)).ToList(),
                p.FollowUps.Select(x => new PersonFollowUpResponse(
                    x.Title,
                    x.Status,
                    x.Type,
                    x.DueAtUtc,
                    x.SnoozedUntilUtc,
                    x.ProgramName)).ToList(),
                p.IsArchived,
                p.ArchivedAtUtc)));
    }

    [HttpPost("{id:guid}/archive")]
    [Authorize]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        var result = await _archive.ArchiveAsync(new ArchivePersonInput(id, ShouldArchive: true), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ArchivePersonResponse(r.PersonId, r.IsArchived, r.ArchivedAtUtc)));
    }

    [HttpPost("{id:guid}/unarchive")]
    [Authorize]
    public async Task<IActionResult> Unarchive(Guid id, CancellationToken ct)
    {
        var result = await _archive.ArchiveAsync(new ArchivePersonInput(id, ShouldArchive: false), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ArchivePersonResponse(r.PersonId, r.IsArchived, r.ArchivedAtUtc)));
    }

    [HttpPost("{id:guid}/update")]
    [Authorize]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePersonRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdatePersonInput(id, req.FullName, req.Phone, req.Email, req.Notes);
        var result = await _update.UpdateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UpdatePersonResponse(r.PersonId)));
    }
}