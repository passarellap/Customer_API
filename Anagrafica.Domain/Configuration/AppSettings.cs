using System.Collections.Generic;

namespace AnagraficaAPI.Domain.Configuration
{
    public class AppSettings
    {
        public const string AppSettingsName = "AppSettings";

        public bool? EnabledSqlEnableRetryOnFailure { get; set; }
        public IEnumerable<string> CorsEnabledDomains { get; set; }
        public string OpenApiServer { get; set; }
        public string DocsGeneratorUrl { get; set; }
    }
}
