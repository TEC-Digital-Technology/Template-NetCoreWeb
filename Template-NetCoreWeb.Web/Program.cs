var builder = WebApplication.CreateBuilder(args);

#region TEC
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services.AddRequestResponseLoggingOption(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureMvcLogging(options));
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(TEC.Core.Web.Mvc.Logging.Filters.LogRequestFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Mvc.Logging.Filters.LogResponseFilter));
    options.Filters.Add(typeof(TEC.Core.Web.Mvc.Logging.Filters.LogExceptionFilter));
    options.EnableEndpointRouting = false;
});
#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
#region TEC
app.ConfigureHttpContext();
app.ConfigureLogging(options => Template_NetCoreWeb.WebMvc.StartupConfig.LoggingConfig.ConfigureBasicLogging(options));
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
