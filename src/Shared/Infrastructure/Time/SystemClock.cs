using EduCrm.SharedKernel.Abstractions;

namespace EduCrm.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}