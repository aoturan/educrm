namespace EduCrm.Modules.Program.Application.UseCases.Create;

public class CreateResult
{
    public Guid ProgramId { get; init; }

    public CreateResult(Guid programId)
    {
        ProgramId = programId;
    }
}

