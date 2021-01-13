namespace Windgram.Web.Shared.Services
{
    public interface IUserContext
    {
        string EmailAddress { get; }
        string IpAddress { get; }
        string UserId { get; }
        string UserName { get; }
    }
}