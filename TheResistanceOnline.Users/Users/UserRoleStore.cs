using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Users.Users;

public class UserRoleStore: IUserRoleStore<User>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private bool _disposed;

    private readonly IdentityErrorDescriber _errorDescriber = new();
    private readonly IRoleStore<Role> _roleStore;

    #endregion

    #region Construction

    public UserRoleStore(IDataContext dataContext, IRoleStore<Role> roleStore)
    {
        _dataContext = dataContext;
        _roleStore = roleStore;
    }

    #endregion

    #region Private Methods

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    #endregion

    #region Public Methods

    public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(roleName);
        var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        ArgumentNullException.ThrowIfNull(role);
        user.AddRole(role.Id);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        await _dataContext.AddAsync(user, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        _dataContext.Remove(user);

        try
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        catch(DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public void Dispose()
    {
        _disposed = true;
    }

    public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var strongUserId = new UserId(Guid.Parse(userId));
        return await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(strongUserId)
                                 .ExecuteAsync(cancellationToken);
    }

    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return await _dataContext.Query<IUserByNameDbQuery>()
                                 .WithParams(normalizedUserName)
                                 .ExecuteAsync(cancellationToken);
    }

    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);


        user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(user.Id)
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .ExecuteAsync(cancellationToken);
        if (user?.UserRole?.Role?.Name == null)
        {
            return new List<string>();
        }

        return new List<string>
               {
                   user.UserRole.Role.Name
               };
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        return await _dataContext.Query<IUsersByRoleNamesDbQuery>()
                                 .WithParams(roleName)
                                 .ExecuteAsync(cancellationToken);
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(roleName);


        user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(user.Id)
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .ExecuteAsync(cancellationToken);
        if (user.UserRole == null)
        {
            return false;
        }

        return user.UserRole.Role.Name == roleName;
    }

    public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        user.UserRole = null;
        return _dataContext.SaveChangesAsync(cancellationToken);
    }

    public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        _dataContext.Attach(user);
        user.ConcurrencyStamp = Guid.NewGuid();
        _dataContext.Update(user);

        try
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        catch(DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    #endregion
}
