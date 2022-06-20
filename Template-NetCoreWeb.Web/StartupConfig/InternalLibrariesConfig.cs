using Microsoft.Extensions.DependencyInjection;

namespace Template_NetCoreWeb.WebMvc.StartupConfig
{
    /// <summary>
    /// TEC 內部服務介接設定
    /// </summary>
    public static class InternalLibrariesConfig
    {
        /// <summary>
        /// 初始化 AccountService 介接物件
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void ConfigureNetCoreDemo(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Api003AccountApiHandler>((serviceProvider, httpClient) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:TemplateNetCoreWeb.WebApi"]);
            }).ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.NetCoreDemoLoggingHttpClientHandler>();
        }
    }
}
