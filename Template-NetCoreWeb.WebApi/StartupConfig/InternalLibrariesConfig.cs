using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template_NetCoreWeb.Core.Logging.HttpHandlers;

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
        //AccountService
        Assembly.GetAssembly(typeof(TEC.Internal.Web.AccountService.F001AccountApiHandler))!.GetTypes()
            .Where(t => t.BaseType == typeof(TEC.Internal.Web.Core.ApiProxy.ApiHandlerBase))
            .ToList()
            .ForEach(t =>
            {
                Action<IServiceProvider, HttpClient> action = new Action<IServiceProvider, HttpClient>((serviceProvider, httpClient) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    httpClient.BaseAddress = new Uri(configuration["TEC:InternalWeb:ServiceLocation:AccountService"]!);
                });
                IHttpClientBuilder httpClientBuilder = (IHttpClientBuilder)InternalLibrariesConfig.AddHttpClientMethodInfo.MakeGenericMethod(t).Invoke(null, new object[] { serviceCollection, t.FullName!, action })!;
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler<Template_NetCoreWeb.Core.Logging.HttpHandlers.TECLoggingHttpClientHandler>();
            });
    }
    /// <summary>
    /// 取得用於設定 HTTP Client 的靜態擴充方法
    /// </summary>
    /// <returns></returns>
    private static MethodInfo AddHttpClientMethodInfo =>
        typeof(HttpClientFactoryServiceCollectionExtensions).GetMethods()
                   .Where(methodInfo => methodInfo.Name == nameof(HttpClientFactoryServiceCollectionExtensions.AddHttpClient))
                   .Where(methodInfo =>
                   {
                       if (!methodInfo.IsGenericMethod)
                       {
                           return false;
                       }
                       Type[] genericArgs = methodInfo.GetGenericArguments();
                       if (genericArgs.Length != 1)
                       {
                           return false;
                       }
                       ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                       if (parameterInfos.Length != 3)
                       {
                           return false;
                       }
                       return parameterInfos[2].ParameterType.FullName!.StartsWith("System.Action`2");
                   })
                   .Single();
}
