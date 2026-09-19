using IdentityWorkshop.Application.Users;
using IdentityWorkshop.Domain.Users;
using Xunit;

namespace IdentityWorkshop.UnitTests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task SearchAsync_UsesDefaultLimit_WhenLimitIsNotPositive()
    {
        var repository = new FakeUserRepository();
        var service = new UserService(repository);

        await service.SearchAsync(new UserSearchQuery("alice", 0), CancellationToken.None);

        Assert.Equal(20, repository.LastQuery?.Limit);
        Assert.Equal("alice", repository.LastQuery?.Term);
    }

    [Fact]
    public async Task GetByIdAsync_MapsUserToDetailDto()
    {
        var userId = Guid.NewGuid();
        var repository = new FakeUserRepository
        {
            User = new WorkshopUser
            {
                UserId = userId,
                Username = "alice",
                DisplayName = "Alice Workshop",
                EmailAddress = "alice@example.test",
                DepartmentCode = "IT",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };
        var service = new UserService(repository);

        var result = await service.GetByIdAsync(userId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("alice@example.test", result.EmailAddress);
    }

    [Fact]
    public async Task GetByIdAsync_RejectsEmptyGuid()
    {
        var service = new UserService(new FakeUserRepository());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.GetByIdAsync(Guid.Empty, CancellationToken.None));
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public UserSearchQuery? LastQuery { get; private set; }

        public WorkshopUser? User { get; init; }

        public Task<IReadOnlyList<WorkshopUser>> SearchAsync(
            UserSearchQuery query,
            CancellationToken cancellationToken)
        {
            LastQuery = query;
            return Task.FromResult<IReadOnlyList<WorkshopUser>>([]);
        }

        public Task<WorkshopUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
            Task.FromResult(User);
    }
}
