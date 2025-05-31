namespace Invx.SharedKernel.Api.Configurations;
public class ApiSettings
{
    public string ServiceName { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public bool EnableSwagger { get; set; } = true;
    public bool EnableDetailedErrors { get; set; } = false;
    public CorsSettings Cors { get; set; } = new();
}