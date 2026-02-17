using EduCrm.SharedKernel.Errors;

namespace EduCrm.SharedKernel.Results;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value when result is failure.");

    private Result(bool isSuccess, T? value, IEnumerable<Error>? errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public static Result<T> Success(T value) =>
        new(true, value, Array.Empty<Error>());

    public new static Result<T> Fail(Error error) =>
        new(false, default, new[] { error });

    public new static Result<T> Fail(IEnumerable<Error> errors) =>
        new(false, default, errors);

    public new static Result<T> Fail(params Error[] errors) =>
        new(false, default, errors);
}