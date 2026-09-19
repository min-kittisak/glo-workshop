using IdentityWorkshop.Domain.Users;

namespace IdentityWorkshop.Application.Users;

public sealed class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<UserListItemDto>> SearchAsync(
        UserSearchQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var normalizedLimit = query.Limit <= 0 ? 20 : query.Limit;

        // LAB 8: ให้ผู้เรียนเพิ่มกฎ upper bound และ Validation ที่เหมาะสมก่อนส่งต่อ Repository
        var users = await _repository.SearchAsync(
            query with { Term = query.Term?.Trim(), Limit = normalizedLimit },
            cancellationToken);

        return users.Select(MapListItem).ToList();
    }

    public async Task<UserDetailDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID must not be empty.", nameof(userId));
        }

        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        return user is null ? null : MapDetail(user);
    }

    private static UserListItemDto MapListItem(WorkshopUser user) =>
        new(user.UserId, user.Username, user.DisplayName, user.DepartmentCode, user.IsActive);

    private static UserDetailDto MapDetail(WorkshopUser user) =>
        new(user.UserId, user.Username, user.DisplayName, user.EmailAddress, user.DepartmentCode, user.IsActive, user.CreatedAt);
}

