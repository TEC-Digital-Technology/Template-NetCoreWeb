using Microsoft.AspNetCore.Authentication.Cookies;
using Template_NetCoreWeb.WebMvc.Settings;
using Template_NetCoreWeb.WebMvc.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

#region TEC
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRequestResponseLoggingOption(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureLogging(options));
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogRequestFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogResponseFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogExceptionFilter));
});
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Template_NetCoreWeb.Session";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
#region Logging(Log4net)
builder.Services.AddLog4NetLoggingConfiguration<
    Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope,
    Template_NetCoreWeb.Utils.Enums.Logging.LoggingSystemScope,
    Template_NetCoreWeb.Utils.Enums.Logging.LoggingMessageType,
    Template_NetCoreWeb.Utils.Logging.LogState>(Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope.FrontEnd, options =>
    {
        options.LoggerRepositoryCreated = (repository) =>
        {
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net_api.config"));
        };
    });
#endregion
#region HTTP Handlers
//HttpClientHandler 生命週期會由 AddHttpClient 控制，在此的 AddScope 可以支援從 ServiceProvider 取得 SettingCollection 的功能
builder.Services.AddScoped(typeof(Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler), serviceProvider =>
{
    Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler handler = new(new HttpClientHandler(), true, serviceProvider.GetRequiredService<ILoggerFactory>());
    handler.LogHttpError += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpError;
    handler.LogHttpRequest += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpRequest;
    handler.LogHttpResponse += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpResponse;
    return handler;
});
builder.Services.AddScoped(typeof(Template_NetCoreWeb.Core.Logging.HttpHandlers.NetCoreDemoLoggingHttpClientHandler), serviceProvider =>
{
    Template_NetCoreWeb.Core.Logging.HttpHandlers.NetCoreDemoLoggingHttpClientHandler handler = new(new HttpClientHandler(), true, serviceProvider.GetRequiredService<ILoggerFactory>());
    handler.LogHttpError += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpError;
    handler.LogHttpRequest += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpRequest;
    handler.LogHttpResponse += Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpResponse;
    return handler;
});
builder.Services.AddHttpClient<Template_NetCoreWeb.Core.UIData.ThirdParty.TECApi.TecApiHandler>()
    .ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
#endregion
#region Internal Libraries
builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new TEC.Internal.Web.Core.ApiProxy.Settings.ApiClientSettingCollection()
    {
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiClientSettingEnum.ClientName, configuration["TEC:InternalWeb:ApiClientSetting:ClientName"] },
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiClientSettingEnum.CryptionIv, configuration["TEC:InternalWeb:ApiClientSetting:CryptionIv"] },
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiClientSettingEnum.CryptionKey, configuration["TEC:InternalWeb:ApiClientSetting:CryptionKey"] },
    };
});
builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new TEC.Internal.Web.Core.ApiProxy.Settings.ApiResultSettingCollection()
    {
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiResultSettingEnum.DefaultExpectedResultCode, configuration["TEC:InternalWeb:ApiResultSetting:DefaultExpectedResultCode"] },
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiResultSettingEnum.ResultCodeKey, configuration["TEC:InternalWeb:ApiResultSetting:ResultCodeKey"] },
        { TEC.Internal.Web.Core.ApiProxy.Settings.ApiResultSettingEnum.ResultMessageKey, configuration["TEC:InternalWeb:ApiResultSetting:ResultMessageKey"] },
    };
});
builder.Services.ConfigureNetCoreDemo();
#endregion
#region ADFS
builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new TEC.Internal.Web.Core.Security.Settings.AuthSettingCollection()
    {
        { TEC.Internal.Web.Core.Security.Settings.AuthSettingEnum.Issuer, configuration["TEC:Adfs:Issuer"] },
        { TEC.Internal.Web.Core.Security.Settings.AuthSettingEnum.AllowedAudience, configuration.GetSection("TEC:Adfs:AllowedAudience").Get<string[]>() },
        { TEC.Internal.Web.Core.Security.Settings.AuthSettingEnum.CertificationPath, configuration.GetSection("TEC:Adfs:SigningCertPath").Get<string[]>() },
    };
});
builder.Services.AddScoped(serviceProvider =>
{
    var provider = new TEC.Core.Settings.Providers.ConfigurationSettingProvider<ClientApplicationSettingCollection, Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum, string>(serviceProvider.GetRequiredService<IConfiguration>());
    return (ClientApplicationSettingCollection)provider.Load();
});
builder.Services.AddScoped<Microsoft.Identity.Client.IConfidentialClientApplication>(serviceProvider =>
{
    var clientApplicationSettingCollection = serviceProvider.GetRequiredService<ClientApplicationSettingCollection>();
    Uri redirectUri = new Uri((Uri)clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.FrontendBaseUrl], "/Auth/OAuth");
    return Microsoft.Identity.Client.ConfidentialClientApplicationBuilder.Create(clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.ClientId].ToString())
                  .WithAdfsAuthority(clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.Authority].ToString(), true)
                  .WithRedirectUri(redirectUri.AbsoluteUri)
                  //這個地方只是為了要組授權 Url ，不需要 Secret；但如果不給會有例外，所以隨便產。
                  .WithClientSecret(Guid.Empty.ToString())
                  .Build();
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.EventsType = typeof(Template_NetCoreWeb.WebMvc.Security.TecCookieAuthenticationEvents);
        options.Cookie.Name = "Template-NetCoreWeb.Identity";
        options.LoginPath = new PathString("/Auth/OAuthSignIn");
    });
builder.Services.AddScoped<Template_NetCoreWeb.WebMvc.Security.TecCookieAuthenticationEvents>();
#endregion
#region Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<PersonalDataSettingCollection>(serviceProvider =>
{
    PersonalDataSettingCollection result;
    var session = serviceProvider.GetRequiredService<ISession>();
    if (!session.TryGetValue(nameof(PersonalDataSettingCollection), out byte[]? sessionValue))
    {
        result = new PersonalDataSettingCollection();
        session.Set(nameof(PersonalDataSettingCollection), System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(result)));
    }
    else
    {
        result = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonalDataSettingCollection>(System.Text.Encoding.UTF8.GetString(sessionValue!))!;
    }
    return result;
});
#endregion
#endregion
var app = builder.Build();
// Configure the HTTP request pipeline.
#region TEC
app.ConfigureHttpContext();
app.ConfigureLogging(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureBasicLogging(options));
app.ConfigureLogging(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureLogging(options));
app.UseHttpsRedirection();
#region Session
app.UseSession();
#endregion
#endregion
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
