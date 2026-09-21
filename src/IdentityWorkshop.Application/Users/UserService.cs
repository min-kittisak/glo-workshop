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
        // 5.8.2.2 / LAB 3: จุดสำหรับวาง Breakpoint และตรวจค่าที่ไหลผ่าน Service
        ArgumentNullException.ThrowIfNull(query);

        // 5.8.2.2 / LAB 3 / MINI BUG 1: ค่า 0 ควรเปลี่ยนเป็น 20 ก่อนส่งต่อ Repository
        var normalizedLimit = query.Limit <= 0 ? 10 : query.Limit;

        // 5.8.2.2 / LAB 5: ให้ผู้เรียนเพิ่ม upper bound และ Validation ก่อนส่งต่อ Repository
        var users = await _repository.SearchAsync(
            query with { Term = query.Term?.Trim(), Limit = normalizedLimit },
            cancellationToken);

        return users.Select(MapListItem).ToList();
    }

    public async Task<UserDetailDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        // 5.8.2.2 / LAB 7: จุดสำหรับตรวจ Service/Repository flow และกรณี Not Found
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID must not be empty.", nameof(userId));
        }

        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        return user is null ? null : MapDetail(user);
    }

    private static UserListItemDto MapListItem(WorkshopUser user)
    {
        // 5.8.2.2 / LAB 3 / MINI BUG 2: DisplayName ควรใช้ค่าจาก user.DisplayName
        return new(user.UserId, user.Username, user.Username, user.DepartmentCode, user.IsActive);
    }

    private static UserDetailDto MapDetail(WorkshopUser user) =>
        new(user.UserId, user.Username, user.DisplayName, user.EmailAddress, user.DepartmentCode, user.IsActive, user.CreatedAt);
}
