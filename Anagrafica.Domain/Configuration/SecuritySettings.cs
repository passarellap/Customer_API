namespace AnagraficaAPI.Domain.Configuration
{
    public class SecuritySettings
    {
        public const string SecuritySettingsName = "SecuritySettings";

        public bool EnableIdentityServer { get; set; }
        public string AuthorityUrl { get; set; }
        public string RequiredScope { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
    }
}
