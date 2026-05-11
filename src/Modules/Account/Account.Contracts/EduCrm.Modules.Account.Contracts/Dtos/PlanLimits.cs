namespace EduCrm.Modules.Account.Contracts.Dtos;

public sealed class PlanLimits
{
    public int  Users           { get; init; }
    public int  ActivePersons   { get; init; }
    public int? ActivePrograms  { get; init; }
    public int? OpenFollowUps   { get; init; }
    public bool ExportEnabled   { get; init; }
}

public sealed class PlanLimitsOptions
{
    public PlanLimits Free { get; init; } = new();
    public PlanLimits Plus { get; init; } = new();
    public PlanLimits Pro  { get; init; } = new();
}