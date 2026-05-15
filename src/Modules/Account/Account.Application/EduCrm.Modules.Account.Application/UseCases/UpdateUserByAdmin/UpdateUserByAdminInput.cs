namespace EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;

public sealed record UpdateUserByAdminInput(
    Guid TargetUserId,
    string FullName);
