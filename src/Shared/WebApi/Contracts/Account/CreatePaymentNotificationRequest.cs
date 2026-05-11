using Microsoft.AspNetCore.Http;

namespace EduCrm.WebApi.Contracts.Account;

public sealed class CreatePaymentNotificationRequest
{
    public string SenderName { get; init; } = string.Empty;
    public DateOnly PaymentDate { get; init; }
    public decimal Amount { get; init; }
    public string? Note { get; init; }
    public IFormFile? Receipt { get; init; }
}