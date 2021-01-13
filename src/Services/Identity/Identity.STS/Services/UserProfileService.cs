using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Windgram.ApplicationCore.Domain.Entities;

namespace Windgram.Identity.Web.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ILogger _logger;
        public UserProfileService(
            ILogger<UserProfileService> logger,
            UserManager<UserIdentity> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
                throw new ArgumentException("Invalid subject identifier");
            var claims = await GetClaimsFromUser(user, context.RequestedClaimTypes);
            context.IssuedClaims = claims.ToList();
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No subject Id claim present");

            await IsActiveAsync(context, sub);
        }

        private async Task<IEnumerable<Claim>> GetClaimsFromUser(UserIdentity user, IEnumerable<string> requestedClaimTypes)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.Name,user.UserName),
            };
            if (requestedClaimTypes.Contains(IdentityServerConstants.StandardScopes.Profile))
            {
                //claims.Add(new Claim(JwtClaimTypes.NickName, user.n.IsNullOrEmpty() ? string.Empty : user.NickName));
                //claims.Add(new Claim(JwtClaimTypes.Picture, _settings.BuildFileUrl(user.AvatarFileId)));

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
                }
            }
            if (requestedClaimTypes.Contains(IdentityServerConstants.StandardScopes.Profile) && user.EmailConfirmed)
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            }
            return claims;
        }

        private async Task IsActiveAsync(IsActiveContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await IsActiveAsync(context, user);
            }
            else
            {
                context.IsActive = false;
            }
        }
        private async Task IsActiveAsync(IsActiveContext context, UserIdentity user)
        {
            context.IsActive = await IsUserActiveAsync(user);
        }

        private async Task<bool> IsUserActiveAsync(UserIdentity user)
        {
            var isActive = user.Email.IsNullOrEmpty() ? true : user.EmailConfirmed;
            return await Task.FromResult(isActive);
        }

        private async Task<UserIdentity> FindUserAsync(string subjectId)
        {
            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                _logger.LogWarning("No user found matching subject Id: {subjectId}", subjectId);
            }

            return user;
        }
    }
}
