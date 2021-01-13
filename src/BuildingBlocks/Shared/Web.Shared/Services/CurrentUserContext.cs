using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Windgram.Web.Shared.Services
{
    public class CurrentUserContext : IUserContext
    {
        private string _userId;
        private string _userName;
        private string _email;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;
                }
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                return _userId;
            }
        }
        public string IpAddress
        {
            get
            {
                var ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return ip;
            }
        }

        public string UserName
        {

            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    _userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                }
                return _userName;
            }
        }

        public string EmailAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_email))
                {
                    _email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                }
                return _email;
            }
        }
    }
}
