
var builder = WebApplication.CreateBuilder(args);

#region TEC
Microsoft.Extensions.Caching.Memory.MemoryCache memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()
{
});
builder.Services.AddSingleton(typeof(Microsoft.Extensions.Caching.Memory.MemoryCache), memoryCache);
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services
    .AddControllers(option =>
    {
        option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
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
#endregion

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapControllers();
#region TEC
string apiRouteTemplate = "api/{controller}/{action}";
app.ConfigureHttpContext();
app.UseRouting();
app.UseTecApiMechanism<Template_NetCoreWeb.Utils.Enums.ResultCodeSettingEnum>(apiRouteTemplate, app.Environment.IsDevelopment());
#region Logging
app.ConfigureLogging(options => Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.ConfigureBasicLogging(options));
app.UseWhen(context =>context.Request.Path.StartsWithSegments("/api"), appBuilder => appBuilder.ConfigureLogging(options => Template_NetCoreWeb.WebApi.StartupConfig.LoggingConfig.ConfigureApiLogging(options)));
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
