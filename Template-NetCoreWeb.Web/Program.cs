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
