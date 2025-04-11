using Microsoft.AspNetCore.Authentication.JwtBearer;
using TEC.Core.Security.Cryptography.Ciphers;
using TEC.Internal.Utils.NetCore.Authorization;
using TEC.Internal.Web.Core.Enums.Settings;
using TEC.Internal.Web.Core.Security.Settings;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.StartupConfig;

/// <summary>
/// 處理認證資料的類別
/// </summary>
internal static class AuthConfig
{
    /// <summary>
    /// 設定應用程式認證邏輯
    /// </summary>
    internal static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = TEC.Internal.Web.Core.Security.AuthHelper.ParseTokenValidationParameters(new TokenAuthSettingCollection()
             {
                { TokenAuthSettingEnum.Issuer, builder.Configuration["TEC:Adfs:Issuer"] },
                { TokenAuthSettingEnum.AllowedAudience, builder.Configuration.GetSection("TEC:Adfs:AllowedAudience").Get<string[]>() },
                { TokenAuthSettingEnum.CertificationPath, builder.Configuration.GetSection("TEC:Adfs:SigningCertPath").Get<string[]>()}
             });
            options.Events = TEC.Internal.Utils.NetCore.Authentication.JwtBearerEventsHelper.GenerateJwtBearerEvents(ResultCodeSettingEnum.AuthorizationFailed,
                ResultCodeSettingEnum.AuthenticationRequired, ResultCodeSettingEnum.AuthenticationFailed, ExceptionConfig.ResultFormat);
        });
        builder.Services.AddAuthorizationBuilder()
           .AddPolicy(Const.frontendAudiencePolicyName, policy => policy.Requirements.Add(new AudienceAuthorizationRequirement(
               builder.Configuration["TEC:Adfs:FrontendAudience"])));
        builder.Services.AddAudienceRequiredAuthorizationHanlder();
    }
}