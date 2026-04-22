namespace EduCrm.Modules.Program.Application.UseCases.GetDashboardCounts;

public sealed record GetDashboardCountsResult(
    int NewApplicationsCount,
    int ProgramsStartingInNext7DaysCount,
    int OpenFollowUpsCount,
    int OverdueFollowUpsCount);
