using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Program.Contracts.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Program.Infrastructure.Queries;

public class ProgramReader : IProgramReader
{
    private readonly AppDbContext _db;

    public ProgramReader(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProgramSummary?> GetProgramByIdAsync(Guid programId, Guid organizationId, CancellationToken ct)
    {
        return await _db.Programs
            .AsNoTracking()
            .Where(p => p.Id == programId && p.OrganizationId == organizationId)
            .Select(p => new ProgramSummary(p.Id, p.OrganizationId, p.Name))
            .FirstOrDefaultAsync(ct);
    }
}