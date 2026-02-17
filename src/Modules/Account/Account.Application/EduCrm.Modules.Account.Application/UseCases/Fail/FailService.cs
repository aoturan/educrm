using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Fail;

public sealed class FailService : IFailService
{
    public Result Fail()
        => Result.Fail(new Error(ErrorCodes.DemoFail, "This is an intentional failure."));
}