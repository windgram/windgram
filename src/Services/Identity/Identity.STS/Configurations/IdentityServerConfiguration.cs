namespace Windgram.Identity.STS.Configurations
{
    public class IdentityServerConfiguration
    {
        public string IdentityUrl { get; set; }
        public string CertificatePfxFilePath { get; set; }
        public string CertificatePfxPassword { get; set; }
        public ExternalLoginProvider GitHubLogin { get; set; }
        public class ExternalLoginProvider
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Scopes { get; set; }
        }
    }
}
