namespace IdentityWorkshop.Api.Contracts;

public sealed record WorkshopApiResponse<T>(
    T Data,
    string Message,
    string CorrelationId);

