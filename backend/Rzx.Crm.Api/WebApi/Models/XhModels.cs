namespace Rzx.Crm.Api.WebApi.Models
{
    public class XhDefault
    {
        public const string AppName = "CRM";
        public const string AppVersion = "0.0.1";
        public const string AppBuild = "123";
        public const string AppInstance = "3adf4667";
        public const string AppEnv = "Development";
    }

    public class XhGetPrefsResponse
    {
        public object xhTheme => new { type = "string", value = "dark", defaultValue = "dark" };
        public object xhIdleDetectionDisabled => new { type = "bool", value = false, defaultValue = false };
    }

    public class XhGetConfigResponse
    {
        public int xhAppVersionCheckSecs = -1;
        public string xhEmailSupport = "none";
        public string xhAppTimeZone = "GMT";
        public bool xhAppVersionCheckEnabled = false;
        public object xhAlertBannerConfig = new { enabled = false };
    }

    public class XhEnvironmentResponse
    {
        public string AppCode => XhDefault.AppName;
        public string AppName => XhDefault.AppName;
        public string AppVersion => XhDefault.AppVersion;
        public string AppBuild => XhDefault.AppBuild;
        public string AppEnvironment => XhDefault.AppEnv;
        public string GrailsVersion => "6.2.0";
        public string JavaVersion => "17.0.8.1";
        public string ServerTimeZone => "Etc/UTC";
        public int ServerTimeZoneOffset => 0;
        public string AppTimeZone => "GMT";
        public int AppTimeZoneOffset => 0;
        public bool WebSocketsEnabled => false;
        public string InstanceName => XhDefault.AppInstance;
        public string HoistCoreVersion => "30.0.0";
        public object AlertBanner => new { active = false };
        public object PollConfig => new { interval = -1, onVersionChange = "promptReload" };
    }

    public class XhEnvironmentPollResponse
    {
        public string AppCode => XhDefault.AppName;
        public string AppVersion => XhDefault.AppVersion;
        public string AppBuild => XhDefault.AppBuild;
        public string InstanceName => XhDefault.AppInstance;
        public object AlertBanner => new { active = false };
        public object PollConfig => new { interval = -1, onVersionChange = "promptReload" };
    }
}
