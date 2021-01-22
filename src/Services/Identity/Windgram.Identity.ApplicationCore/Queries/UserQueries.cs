using Dapper;
using MySqlConnector;
using System.Threading.Tasks;
using Windgram.Caching;
using Windgram.Identity.ApplicationCore.Constants;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Identity.ApplicationCore.ViewModels.Identity;

namespace Windgram.Identity.ApplicationCore.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly string _connectionStrings;
        private readonly ICacheManager _cacheManager;
        public UserQueries(
            string connectionStrings,
            ICacheManager cacheManager)
        {
            _connectionStrings = connectionStrings;
            _cacheManager = cacheManager;
        }

        public async Task<UserProfileViewModel> GetUserProfileById(string id)
        {
            return await _cacheManager.GetOrCreateAsync(UserIdentity.GetProfileById(id), () => this.LoadUserProfileById(id));
        }

        private async Task<UserProfileViewModel> LoadUserProfileById(string id)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();

                var rawSql = $@"SELECT * FROM {IdentityTableDefaults.Schema}_{IdentityTableDefaults.User} WHERE Id = @id";
                var user = await connection.QueryFirstOrDefaultAsync<UserIdentity>(rawSql, new { id });
                if (user == null)
                    return null;
                var userClaims = await connection.QueryAsync<UserIdentityUserClaim>(
                    $"SELECT * FROM {IdentityTableDefaults.Schema}_{IdentityTableDefaults.UserClaim} WHERE UserId = @userId",
                    new { userId = id });
                return new UserProfileViewModel(userClaims)
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName,
                    CreatedDateTime = user.CreatedDateTime
                };
            }
        }
    }
}
