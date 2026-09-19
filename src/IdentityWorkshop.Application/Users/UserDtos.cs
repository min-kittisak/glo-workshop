namespace IdentityWorkshop.Application.Users;

public sealed record UserListItemDto(
    Guid Id,
    string Username,
    string DisplayName,
    string? DepartmentCode,
    bool IsActive);

public sealed record UserDetailDto(
    Guid Id,
    string Username,
    string DisplayName,
    string EmailAddress,
    string? DepartmentCode,
    bool IsActive,
    DateTime CreatedAt);

public sealed record UserSearchQuery(string? Term, int Limit = 20);

