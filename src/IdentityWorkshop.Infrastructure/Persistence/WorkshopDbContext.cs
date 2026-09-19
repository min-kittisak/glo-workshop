using IdentityWorkshop.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkshop.Infrastructure.Persistence;

public sealed class WorkshopDbContext : DbContext
{
    public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkshopUser> Users => Set<WorkshopUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 5.8.2.3 / LAB 8: ตรวจ Mapping ตาราง workshop.users ก่อนรัน SQL และวัด Query
        modelBuilder.Entity<WorkshopUser>(entity =>
        {
            entity.ToTable("users", "workshop");
            entity.HasKey(user => user.UserId);

            entity.Property(user => user.UserId).HasColumnName("user_id");
            entity.Property(user => user.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
            entity.Property(user => user.DisplayName).HasColumnName("display_name").HasMaxLength(200).IsRequired();
            entity.Property(user => user.EmailAddress).HasColumnName("email_address").HasMaxLength(255).IsRequired();
            entity.Property(user => user.DepartmentCode).HasColumnName("department_code").HasMaxLength(30);
            entity.Property(user => user.IsActive).HasColumnName("is_active").IsRequired();
            entity.Property(user => user.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        });
    }
}
