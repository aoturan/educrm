namespace EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;

public static class SupportContactMessageListPreFilter
{
    public const string New = "new";
    public const string Handled = "handled";
}

public sealed record ListSupportContactMessagesInput(
    int Page = 1,
    int PageSize = 10,
    string? PreFilter = null);
