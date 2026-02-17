using EduCrm.SharedKernel.Errors;

namespace EduCrm.SharedKernel.Results;

public class Result
{
    private readonly List<Error> _errors = new();

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors => _errors;

    protected Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        IsSuccess = isSuccess;

        if (errors is null) return;

        var list = errors.Where(e => e is not null).ToList();
        if (list.Count == 0) return;

        _errors.AddRange(list);
    }

    public static Result Success() => new(true, Array.Empty<Error>());

    public static Result Fail(Error error) =>
        new(false, new[] { error });

    public static Result Fail(IEnumerable<Error> errors) =>
        new(false, errors);

    public static Result Fail(params Error[] errors) =>
        new(false, errors);
}