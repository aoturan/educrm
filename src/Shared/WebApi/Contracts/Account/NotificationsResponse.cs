namespace EduCrm.WebApi.Contracts.Account;

public sealed record NotificationItemResponse(string Message, string Link);

public sealed record NotificationsResponse(IReadOnlyList<NotificationItemResponse> Items);
