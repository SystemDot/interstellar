namespace Interstellar.TestApp;

public class UserService
{
    public Task<User> GetCurrentUserAsync()
    {
        return Task.FromResult(new User(Guid.NewGuid()));
    }
}