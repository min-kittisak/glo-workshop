using IdentityWorkshop.Application.Users;
using IdentityWorkshop.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkshop.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly Persistence.WorkshopDbContext _db;

    public UserRepository(Persistence.WorkshopDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WorkshopUser>> SearchAsync(
        UserSearchQuery query,
        CancellationToken cancellationToken)
    {
        var users = _db.Users.AsNoTracking().Where(user => user.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Term))
        {
            // LAB 11/12: วัดผลค้นหาแบบ contains ก่อน แล้วปรับเป็น query ที่ใช้ Index ได้
            var pattern = $"%{query.Term.Trim()}%";
            users = users.Where(user =>
                EF.Functions.ILike(user.Username, pattern) ||
                EF.Functions.ILike(user.DisplayName, pattern));
        }

        return await users
            .OrderBy(user => user.Username)
            .Take(query.Limit)
            .ToListAsync(cancellationToken);
    }

    public Task<WorkshopUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
        _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.UserId == userId, cancellationToken);
}

