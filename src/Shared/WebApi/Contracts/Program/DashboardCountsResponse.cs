namespace EduCrm.WebApi.Contracts.Program;

public sealed record DashboardCountsResponse(
    int NewApplicationsCount,
    int ProgramsStartingInNext7DaysCount,
    int OpenFollowUpsCount,
    int OverdueFollowUpsCount);
