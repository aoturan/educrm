using EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;
using EduCrm.Modules.Program.Application.UseCases.Create;
using EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;
using EduCrm.Modules.Program.Application.UseCases.ChangeStatus;
using EduCrm.Modules.Program.Application.UseCases.DeleteEnrollment;
using EduCrm.Modules.Program.Application.UseCases.GetById;
using EduCrm.Modules.Program.Application.UseCases.GetDashboardCounts;
using EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;
using EduCrm.Modules.Program.Application.UseCases.GetPublicProgramBySlug;
using EduCrm.Modules.Program.Application.UseCases.List;
using EduCrm.Modules.Program.Application.UseCases.ListActive;
using EduCrm.Modules.Program.Application.UseCases.PublishProgram;
using EduCrm.Modules.Program.Application.UseCases.Search;
using EduCrm.Modules.Program.Application.UseCases.UnpublishProgram;
using EduCrm.Modules.Program.Application.UseCases.UpdateProgram;
using EduCrm.WebApi.Contracts.Program;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/program")]
public class ProgramController : ControllerBase
{
    private readonly IArchiveProgramService _archive;
    private readonly IChangeProgramStatusService _changeStatus;
    private readonly ICreateService _create;
    private readonly ICreateEnrollmentService _createEnrollment;
    private readonly IDeleteEnrollmentService _deleteEnrollment;
    private readonly IGetDashboardCountsService _getDashboardCounts;
    private readonly IGetEnrollmentCandidatesService _getEnrollmentCandidates;
    private readonly IGetProgramByIdService _getById;
    private readonly IGetPublicProgramBySlugService _getPublicBySlug;
    private readonly IListActiveProgramsService _listActive;
    private readonly IListProgramsService _list;
    private readonly IPublishProgramService _publish;
    private readonly ISearchProgramsService _search;
    private readonly IUnpublishProgramService _unpublish;
    private readonly IUpdateProgramService _update;
    private readonly IRequestValidator _validator;

    public ProgramController(
        IArchiveProgramService archive,
        IChangeProgramStatusService changeStatus,
        ICreateService create,
        ICreateEnrollmentService createEnrollment,
        IDeleteEnrollmentService deleteEnrollment,
        IGetDashboardCountsService getDashboardCounts,
        IGetEnrollmentCandidatesService getEnrollmentCandidates,
        IGetProgramByIdService getById,
        IGetPublicProgramBySlugService getPublicBySlug,
        IListActiveProgramsService listActive,
        IListProgramsService list,
        IPublishProgramService publish,
        ISearchProgramsService search,
        IUnpublishProgramService unpublish,
        IUpdateProgramService update,
        IRequestValidator validator)
    {
        _archive = archive;
        _changeStatus = changeStatus;
        _create = create;
        _createEnrollment = createEnrollment;
        _deleteEnrollment = deleteEnrollment;
        _getDashboardCounts = getDashboardCounts;
        _getEnrollmentCandidates = getEnrollmentCandidates;
        _getById = getById;
        _getPublicBySlug = getPublicBySlug;
        _listActive = listActive;
        _list = list;
        _publish = publish;
        _search = search;
        _unpublish = unpublish;
        _update = update;
        _validator = validator;
    }

    [HttpGet("dashboard/counts")]
    [Authorize]
    public async Task<IActionResult> GetDashboardCounts(CancellationToken ct)
    {
        var result = await _getDashboardCounts.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new DashboardCountsResponse(
                r.NewApplicationsCount,
                r.ProgramsStartingInNext7DaysCount,
                r.OpenFollowUpsCount,
                r.OverdueFollowUpsCount)));
    }

    [HttpPost("{id:guid}/status")]
    [Authorize]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeProgramStatusRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new ChangeProgramStatusInput(id, req.Status);
        var result = await _changeStatus.ChangeAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ChangeProgramStatusResponse(r.ProgramId, r.Status, r.CompletedAtUtc)));
    }

    [HttpPost("enrollment/create")]
    [Authorize]
    public async Task<IActionResult> CreateEnrollment(
        [FromBody] CreateEnrollmentRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateEnrollmentInput(req.ProgramId, req.PersonId);
        var result = await _createEnrollment.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateEnrollmentResponse(r.EnrollmentId)));
    }

    [HttpGet("enrollments/candidates")]
    [Authorize]
    public async Task<IActionResult> GetEnrollmentCandidates(
        [FromQuery] Guid programId,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if (programId == Guid.Empty)
            return BadRequest("programId is required.");

        if (page < 1) page = 1;
        if (pageSize is < 1 or > 100) pageSize = 20;

        var input = new GetEnrollmentCandidatesInput(programId, search, page, pageSize);
        var result = await _getEnrollmentCandidates.GetAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new EnrollmentCandidatesPagedResponse(
                r.Items.Select(x => new EnrollmentCandidateResponse(x.Id, x.FullName, x.Phone, x.Email)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpDelete("enrollment/{enrollmentId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteEnrollment(Guid enrollmentId, CancellationToken ct)
    {
        var result = await _deleteEnrollment.DeleteAsync(enrollmentId, ct);
        return result.ToActionResult(HttpContext, this);
    }

    [HttpGet("public/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicBySlug(string slug, CancellationToken ct)
    {
        var result = await _getPublicBySlug.GetAsync(new GetPublicProgramBySlugInput(slug), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new PublicProgramResponse(
                r.Name,
                r.StartDate,
                r.EndDate,
                r.Capacity,
                r.PublicShortDescription,
                r.PublicDetailedDescription,
                r.PublicModality,
                r.PublicScheduleText,
                r.PublicInstructorName,
                r.PublicEnrollmentDeadline,
                r.LocationDetails,
                r.OnlineParticipationInfo,
                r.PriceAmount,
                r.PriceCurrency,
                r.PriceNote,
                r.PriceType,
                r.IsPublic,
                r.OrganizationName)));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)    {
        var result = await _getById.GetAsync(id, ct);

        return result.ToActionResult(HttpContext, this, p =>
            Ok(new GetProgramByIdResponse(
                p.Id,
                p.OrganizationId,
                p.CreatedByUserId,
                p.Name,
                p.StartDate,
                p.EndDate,
                p.PublicShortDescription,
                p.InternalNotes,
                p.PublicDetailedDescription,
                p.PublicModality,
                p.LocationDetails,
                p.OnlineParticipationInfo,
                p.Capacity,
                p.PublicScheduleText,
                p.PublicInstructorName,
                p.PublicEnrollmentDeadline,
                p.IsPublic,
                p.Status,
                p.CompletedAtUtc,
                p.CreatedAtUtc,
                p.UpdatedAtUtc,
                p.IsArchived,
                p.ArchivedAtUtc,
                p.PriceAmount,
                p.PriceCurrency,
                p.PriceNote,
                p.PriceType,
                p.PublicSlug,
                p.PublicPublishedAtUtc,
                p.Enrollments
                    .Select(e => new ProgramEnrollmentResponse(
                        e.EnrollmentId,
                        e.PersonId,
                        e.EnrolledAtUtc,
                        e.FullName,
                        e.Email,
                        e.Phone))
                    .ToList())));
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List(
        [FromQuery] ListProgramsQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var input = new ListProgramsInput(page, pageSize, query.Q, query.PreFilter, query.ShowArchived, query.PersonId, query.Face);
        var result = await _list.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ProgramListPagedResponse(
                r.Items.Select(x => new ProgramListItemResponse(
                    x.Id, x.Name, x.PublicShortDescription,
                    x.Status, x.StartDate, x.EndDate, x.EnrollmentCount, x.IsArchived)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string q, CancellationToken ct)
    {
        var result = await _search.SearchAsync(q, ct);

        return result.ToActionResult(HttpContext, this, items =>
            Ok(items.Select(x => new ProgramListItemResponse(
                x.Id, x.Name, x.PublicShortDescription,
                x.Status, x.StartDate, x.EndDate, x.EnrollmentCount, x.IsArchived))));
    }

    [HttpGet("active")]
    [Authorize]
    public async Task<IActionResult> ListActive(CancellationToken ct)
    {
        var result = await _listActive.ListAsync(ct);

        return result.ToActionResult(HttpContext, this, items =>
            Ok(items.Select(x => new ActiveProgramResponse(x.Id, x.Name))));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateProgramRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateInput(
            req.Name, req.StartDate, req.EndDate,
            req.PublicShortDescription, req.PublicModality, req.PublicScheduleText,
            req.PriceType,
            req.InternalNotes, req.PublicDetailedDescription,
            req.LocationDetails, req.OnlineParticipationInfo, req.Capacity,
            req.PublicInstructorName, req.PublicEnrollmentDeadline,
            req.PriceAmount, req.PriceCurrency, req.PriceNote);

        var result = await _create.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new CreateProgramResponse(r.ProgramId)));
    }

    [HttpPost("{id:guid}/update")]
    [Authorize]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProgramRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdateProgramInput(
            id,
            req.Name,
            req.StartDate,
            req.EndDate,
            req.PublicShortDescription,
            req.PublicModality,
            req.PublicScheduleText,
            req.PriceType,
            req.InternalNotes,
            req.PublicDetailedDescription,
            req.LocationDetails,
            req.OnlineParticipationInfo,
            req.Capacity,
            req.PublicInstructorName,
            req.PublicEnrollmentDeadline,
            req.PriceAmount,
            req.PriceCurrency,
            req.PriceNote);

        var result = await _update.UpdateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UpdateProgramResponse(r.ProgramId)));
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        var result = await _publish.PublishAsync(new PublishProgramInput(id), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new PublishProgramResponse(r.ProgramId, r.PublicSlug, r.PublicPublishedAtUtc)));
    }

    [HttpPost("{id:guid}/unpublish")]
    [Authorize]
    public async Task<IActionResult> Unpublish(Guid id, CancellationToken ct)
    {
        var result = await _unpublish.UnpublishAsync(new UnpublishProgramInput(id), ct);

        return result.ToActionResult(HttpContext, this, r => Ok(new { programId = r.ProgramId }));
    }

    [HttpPost("{id:guid}/archive")]
    [Authorize]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)    {
        var result = await _archive.ArchiveAsync(new ArchiveProgramInput(id, ShouldArchive: true), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ArchiveProgramResponse(r.ProgramId, r.IsArchived, r.ArchivedAtUtc, r.Status)));
    }

    [HttpPost("{id:guid}/unarchive")]
    [Authorize]
    public async Task<IActionResult> Unarchive(Guid id, CancellationToken ct)
    {
        var result = await _archive.ArchiveAsync(new ArchiveProgramInput(id, ShouldArchive: false), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ArchiveProgramResponse(r.ProgramId, r.IsArchived, r.ArchivedAtUtc, r.Status)));
    }
}