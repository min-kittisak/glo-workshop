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
        // 5.8.2.3 / LAB 8-9: จุดวัด Query Plan ก่อน/หลังปรับ Query และ Index
        // 5.8.2.2 / LAB 3 / MINI BUG 3: ผลค้นหาควรแสดงเฉพาะผู้ใช้งานที่ยัง Active
        var users = _db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Term))
        {
            // 5.8.2.3 / LAB 8: เริ่มจาก Contains/Leading Wildcard เพื่อเก็บ Baseline
            // 5.8.2.3 / LAB 9: ปรับ Query ให้สัมพันธ์กับ Index แล้วเปรียบเทียบ Execution Plan
            var pattern = $"%{query.Term.Trim()}%";
            // 5.8.2.2 / LAB 3 / MINI BUG 4: ควรค้นหาทั้ง Username และ DisplayName
            users = users.Where(user =>
                EF.Functions.ILike(user.Username, pattern));
        }

        return await users
            // 5.8.2.2 / LAB 3 / MINI BUG 5: ผลลัพธ์ควรเรียงตาม Username เพื่ออ่านง่าย
            .OrderByDescending(user => user.Username)
            .Take(query.Limit)
            .ToListAsync(cancellationToken);
    }

    public Task<WorkshopUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
        _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.UserId == userId, cancellationToken);
}
