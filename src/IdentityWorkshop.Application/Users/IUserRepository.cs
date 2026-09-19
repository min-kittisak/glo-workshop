using IdentityWorkshop.Domain.Users;

namespace IdentityWorkshop.Application.Users;

public interface IUserRepository
{
    Task<IReadOnlyList<WorkshopUser>> SearchAsync(UserSearchQuery query, CancellationToken cancellationToken);

    Task<WorkshopUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}

