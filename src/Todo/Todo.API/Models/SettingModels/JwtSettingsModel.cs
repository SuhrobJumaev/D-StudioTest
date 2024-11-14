namespace Todo.API.Models.SettingModels
{
    public class JwtSettingsModel
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationHours { get; set; }
        public int MaxRefreshTokenLifetimeDays { get; set; }
    }
}
