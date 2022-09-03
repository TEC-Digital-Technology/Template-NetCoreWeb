using Microsoft.Extensions.DependencyInjection;

namespace Template_NetCoreWeb.WebApi.StartupConfig;

/// <summary>
/// TEC 內部服務介接設定
/// </summary>
public static class InternalLibrariesConfig
{
    /// <summary>
    /// 初始化 AccountService 介接物件
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void ConfigureAccountService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<TEC.Internal.Web.AccountService.S001AccountApiHandler>((serviceProvider, httpClient) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:AccountService"]);
        }).ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
        serviceCollection.AddHttpClient<TEC.Internal.Web.AccountService.S002AuthorizeApiHandler>((serviceProvider, httpClient) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:AccountService"]);
        }).ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
        serviceCollection.AddHttpClient<TEC.Internal.Web.AccountService.F001AccountApiHandler>((serviceProvider, httpClient) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:AccountService"]);
        }).ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
        serviceCollection.AddHttpClient<TEC.Internal.Web.AccountService.F002PasscodeApiHandler>((serviceProvider, httpClient) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:AccountService"]);
        }).ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
    }
}
