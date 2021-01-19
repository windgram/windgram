namespace Windgram.Shared.Web.Services
{
    public interface IUserContext
    {
        string EmailAddress { get; }
        string IpAddress { get; }
        string UserId { get; }
        string UserName { get; }
    }
}