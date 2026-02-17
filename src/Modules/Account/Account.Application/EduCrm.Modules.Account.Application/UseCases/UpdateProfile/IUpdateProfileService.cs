using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateProfile;

public interface IUpdateProfileService
{
    Task<Result<UpdateProfileResult>> UpdateProfileAsync(UpdateProfileInput input, CancellationToken ct);
}

