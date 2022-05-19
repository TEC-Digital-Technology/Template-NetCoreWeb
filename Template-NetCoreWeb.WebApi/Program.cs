
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
#region Logging(Log4net)
Enum.GetValues<Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope>()
    .ToList()
    .ForEach(scope =>
    {
        //若其他應用程式/站台是透過本 API 站台來紀錄的話，則需要為所有 LoggingScope 建立紀錄機制。
        //反之，若其他應用程式/站台是自己紀錄而非透過本 API 站台來代處理，則這裡呼叫 AddLog4NetLoggingConfiguration 的 Scope 只需要輸入 API
        builder.Services.AddLog4NetLoggingConfiguration<
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingScope,
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingSystemScope,
        Template_NetCoreWeb.Utils.Enums.Logging.LoggingMessageType,
        Template_NetCoreWeb.Utils.Logging.LogState>(scope, options =>
        {
            options.LoggerRepositoryCreated = (repository) =>
            {
                log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net_adonet.config"));
            };
        });
    });
#endregion
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
