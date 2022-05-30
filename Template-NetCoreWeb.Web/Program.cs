var builder = WebApplication.CreateBuilder(args);

#region TEC
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services.AddRequestResponseLoggingOption(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureLogging(options));
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogRequestFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogResponseFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Logging.Filters.LogExceptionFilter));
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
builder.Services.AddHttpClient<Template_NetCoreWeb.Core.UIData.ThirdParty.TEC.TecApiHandler>()
    .ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
#endregion
#endregion
var app = builder.Build();
// Configure the HTTP request pipeline.
#region TEC
app.ConfigureHttpContext();
app.ConfigureLogging(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureBasicLogging(options));
app.ConfigureLogging(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureLogging(options));
#endregion
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
