using Template_NetCoreWeb.WebApi.Settings;
using Template_NetCoreWeb.WebApi.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

#region TEC
Microsoft.Extensions.Caching.Memory.MemoryCache memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()
{
});
builder.Services.AddRequestResponseLoggingOption(options => Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.ConfigureLogging(options));
builder.Services.AddSingleton(typeof(Microsoft.Extensions.Caching.Memory.MemoryCache), memoryCache);
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogRequestFilter));
        options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogResponseFilter));
        options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogExceptionFilter));
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddNewtonsoftJson(options =>
    {
        options.UseMemberCasing();
    })
    .AddResultCodeConfiguration<Template_NetCoreWeb.Utils.Enums.ResultCodeSettingEnum>(new Template_NetCoreWeb.StartupConfig.ResultDefinition(memoryCache));
#region Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "TEC Digital Technology Inc.",
        Description = "Template Web API Site by Antony"
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
#endregion
#region HTTP Handlers
//HttpClientHandler �ͩR�g���|�� AddHttpClient ����A�b���� AddScope �i�H�䴩�q ServiceProvider ���o SettingCollection ���\��
builder.Services.AddScoped(typeof(Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler), serviceProvider =>
{
    Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler handler = new(new HttpClientHandler(), true, serviceProvider.GetRequiredService<ILoggerFactory>());
    handler.LogHttpError += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpError;
    handler.LogHttpRequest += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpRequest;
    handler.LogHttpResponse += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.LoggingHttpClientHandler_LogHttpResponse;
    return handler;
});
builder.Services.AddHttpClient<Template_NetCoreWeb.Core.UIData.ThirdParty.TECApi.TecApiHandler>()
    .ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
#endregion
#region SOAP
builder.Services.AddSingleton(typeof(Template_NetCoreWeb.Core.Logging.SoapManagers.SoapDemoLoggingManager), serviceProvider =>
{
    Template_NetCoreWeb.Core.Logging.SoapManagers.SoapDemoLoggingManager manager = new(serviceProvider.GetRequiredService<ILoggerFactory>());
    manager.LogSoapError += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.SoapLoggingManager_LogSoapError!;
    manager.LogSoapRequest += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.SoapLoggingManager_LogSoapRequest!;
    manager.LogSoapResponse += Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.SoapLoggingManager_LogSoapResponse!;
    return manager;
});
#endregion
#region Logging(UIData)
builder.Services.AddSingleton(serviceProvider =>
{
    TEC.Core.Logging.UIData.LoggableUIDataOptions loggableUIDataOptions = new TEC.Core.Logging.UIData.LoggableUIDataOptions(
        Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.UIData_LoggingAction,
        () => TEC.Core.Web.HttpContextProvider.CurrentActivityId!.Value,
        true, "***");
    return loggableUIDataOptions;
});
#endregion
#region Logging(Log4net)
Enum.GetValues<Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope>()
    .ToList()
    .ForEach(scope =>
    {
        //�Y��L���ε{��/���x�O�z�L�� API ���x�Ӭ������ܡA�h�ݭn���Ҧ� LoggingScope �إ߬�������C
        //�Ϥ��A�Y��L���ε{��/���x�O�ۤv�����ӫD�z�L�� API ���x�ӥN�B�z�A�h�o�̩I�s AddLog4NetLoggingConfiguration �� Scope �u�ݭn��J API
        builder.Services.AddLog4NetLoggingConfiguration<
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope,
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingSystemScope,
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingMessageType,
        Template_NetCoreWeb.Utils.Logging.LogState>(scope, options =>
        {
            options.LoggerRepositoryCreated = (repository) =>
            {
#warning ���F DEMO �� log4net ��Ʈw�L�k�s���ɭP���� Log �g�J�o�� Timeout �����D�A�p�G Log �M�θ�Ʈw�i�H���Q�s���A�h�������ѤU�@��
                //log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net_adonet.config"));
            };
        });
    });
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
builder.Services.ConfigureAccountService();
#endregion
#region ADFS
builder.Services.AddScoped(serviceProvider =>
{
    var provider = new TEC.Core.Settings.Providers.ConfigurationSettingProvider<ClientApplicationSettingCollection, Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum, string>(serviceProvider.GetRequiredService<IConfiguration>());
    return (ClientApplicationSettingCollection)provider.Load();
});
builder.Services.AddScoped<Microsoft.Identity.Client.IConfidentialClientApplication>(serviceProvider =>
{
    var clientApplicationSettingCollection = serviceProvider.GetRequiredService<ClientApplicationSettingCollection>();
    Uri redirectUri = new Uri((Uri)clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.FrontendBaseUrl], "/Auth/OAuth");
    Microsoft.Identity.Client.IConfidentialClientApplication result = Microsoft.Identity.Client.ConfidentialClientApplicationBuilder.Create(clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.ClientId].ToString())
                  .WithAdfsAuthority(clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.Authority].ToString(), true)
                  .WithRedirectUri(redirectUri.AbsoluteUri)
                  .WithClientSecret(clientApplicationSettingCollection[Template_NetCoreWeb.Utils.Enums.Settings.ClientApplicationSettingEnum.ClientSecret].ToString())
                  //��@�Υ~�� Token Cache �ɡA�ݭn�h���U�@��
                  .WithCacheOptions(new Microsoft.Identity.Client.CacheOptions(true))
                  .Build();
    //�� API ���h�ӯ��x�ɡA�ݭn�@�ΦP�@�� Token Cache(�i�H�ϥ� Redis �� SQL Server)�A�������ѥH�U��k�ù�@��
    //result.UserTokenCache.SetAfterAccessAsync();
    //result.UserTokenCache.SetBeforeAccessAsync();
    return result;
});
#endregion
#region UIData
builder.Services.AddScoped<Template_NetCoreWeb.Core.UIData.AccountUIData>();
#endregion
#endregion
#region Data Protection
//builder.Services.AddDataProtection()
//        .SetApplicationName("netcoredemo.tecyt.com.applocal")
//        .SetDefaultKeyLifetime(TimeSpan.FromDays(90))//the same as default value
//        .PersistKeysToAzureBlobStorage(new Uri("https://xxx.blob.core.windows.net/demo-dev/keys.xml?sp=racwl&st=2022-06-25T09:46:00Z&se=2022-07-25T17:46:00Z&sip=125.227.223.156&spr=https&sv=2021-06-08&sr=c&sig=xxxxxxxxxxxxx"))
//        .ProtectKeysWithCertificate("the thumbnail of the certificate");//Must install the certificate with private key into Key Store#endregion
#endregion
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
#region TEC
app.MapControllers();
app.ConfigureHttpContext();
app.UseRouting();
app.UseTecApiMechanism<Template_NetCoreWeb.Utils.Enums.ResultCodeSettingEnum>("api/{controller}/{action}", app.Environment.IsDevelopment());
#region Logging
app.ConfigureLogging(options => Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.ConfigureBasicLogging(options));
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder => appBuilder.ConfigureLogging(options => Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.ConfigureLogging(options)));
#endregion
#region Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DefaultModelsExpandDepth(-1);
});
app.MapGet("", (context) =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.FromResult(0);
});
#endregion
#endregion

app.Run();
