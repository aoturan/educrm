namespace EduCrm.Modules.Account.Application.UseCases.GetNotifications;

public sealed record GetNotificationsResult(IReadOnlyList<NotificationItem> Items);
