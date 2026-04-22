using System.Text;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.PublishProgram;

public sealed class PublishProgramService(
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IPublishProgramService
{
    public async Task<Result<PublishProgramResult>> PublishAsync(PublishProgramInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<PublishProgramResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<PublishProgramResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var program = await programRepo.GetTrackedByIdAsync(input.ProgramId, orgContext.OrganizationId.Value, ct);
        if (program is null)
            return Result<PublishProgramResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));

        if (program.IsPublic)
            return Result<PublishProgramResult>.Fail(ProgramErrors.ProgramAlreadyPublic(input.ProgramId));

        if (program.IsArchived)
            return Result<PublishProgramResult>.Fail(ProgramErrors.ProgramAlreadyArchived(input.ProgramId));

        var slug = program.PublicSlug ?? GenerateSlug(program.Name);
        var now = clock.UtcNow.UtcDateTime;

        program.Publish(slug, now);

        await uow.SaveChangesAsync(ct);

        return Result<PublishProgramResult>.Success(
            new PublishProgramResult(program.Id, program.PublicSlug!, program.PublicPublishedAtUtc!.Value));
    }

    private static string GenerateSlug(string name)
    {
        var turkishMap = new Dictionary<char, char>
        {
            ['ç'] = 'c', ['Ç'] = 'c',
            ['ş'] = 's', ['Ş'] = 's',
            ['ğ'] = 'g', ['Ğ'] = 'g',
            ['ü'] = 'u', ['Ü'] = 'u',
            ['ö'] = 'o', ['Ö'] = 'o',
            ['ı'] = 'i', ['İ'] = 'i'
        };

        var sb = new StringBuilder();
        foreach (var c in name)
        {
            if (turkishMap.TryGetValue(c, out var mapped))
            {
                sb.Append(mapped);
            }
            else
            {
                var lower = char.ToLowerInvariant(c);
                if (lower == ' ')
                    sb.Append('_');
                else if ((lower >= 'a' && lower <= 'z') || (lower >= '0' && lower <= '9') || lower == '_')
                    sb.Append(lower);
                else if (lower > 127)
                    sb.Append('_');
                // skip punctuation and other ASCII special chars
            }
        }

        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = new string(Enumerable.Range(0, 5)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray());

        // random part = "_" + 5 chars = 6 chars reserved
        const int maxNamePartLength = 194;
        var namePart = sb.ToString().TrimEnd('_');
        if (namePart.Length > maxNamePartLength)
            namePart = namePart[..maxNamePartLength].TrimEnd('_');

        return $"{namePart}_{random}";
    }
}

