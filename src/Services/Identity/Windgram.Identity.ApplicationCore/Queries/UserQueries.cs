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
        public async Task<UserViewModel> GetUserById(string id)
        {
            return await _cacheManager.GetOrCreateAsync(UserIdentity.GetById(id), () => this.LoadUserById(id));
        }
        public async Task<UserClaimsViewModel> GetUserClaimsById(string id)
        {
            return await _cacheManager.GetOrCreateAsync(UserIdentity.GetClaimsById(id), () => this.LoadUserClaimsById(id));
        }
        private async Task<UserViewModel> LoadUserById(string id)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();

                var rawSql = $@"SELECT Id,UserName,Email,PhoneNumber,CreatedDateTime FROM {IdentityTableDefaults.Schema}_{IdentityTableDefaults.User} WHERE Id = @id";
                return await connection.QueryFirstOrDefaultAsync<UserViewModel>(rawSql, new { id });
            }
        }
        private async Task<UserClaimsViewModel> LoadUserClaimsById(string id)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();
                var userClaims = await connection.QueryAsync<UserIdentityUserClaim>(
                    $"SELECT * FROM {IdentityTableDefaults.Schema}_{IdentityTableDefaults.UserClaim} WHERE UserId = @userId",
                    new { userId = id });
                return new UserClaimsViewModel(userClaims);
            }
        }
    }
}
