namespace IdentityWorkshop.Domain.Users;

public sealed class WorkshopUser
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string? DepartmentCode { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}

