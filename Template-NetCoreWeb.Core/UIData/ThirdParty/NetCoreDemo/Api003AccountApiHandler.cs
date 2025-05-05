using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TEC.Internal.Web.Core.ApiProxy;
using TEC.Internal.Web.Core.ApiProxy.Settings;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo;

/// <summary>
/// 介接 API003Account API 的類別
/// </summary>
public class Api003AccountApiHandler : ApiHandlerBase
{
    /// <summary>
    /// 初始化介接 API003Account API 的物件
    /// </summary>
    /// <param name="baseUri">基底 URI</param>
    /// <param name="httpClientFunc">產生用於通訊的<see cref="HttpClient"/>的方法封裝</param>
    /// <param name="apiResultSettingCollection">API 回傳結果的設定檔集合</param>
    /// <param name="tokenHandlingBehaviors">Token 處理的相關行為</param>
    public Api003AccountApiHandler(Uri baseUri, Func<HttpClient> httpClientFunc,
        ApiResultSettingCollection apiResultSettingCollection, TokenHandlingBehaviors? tokenHandlingBehaviors = null)
        : base(baseUri, httpClientFunc, apiResultSettingCollection, tokenHandlingBehaviors)
    {
    }

    /// <summary>
    /// 初始化介接 API003Account API 的物件
    /// </summary>
    /// <param name="httpClient">用於通訊的<see cref="HttpClient"/>物件</param>
    /// <param name="hostEnvironment">代表目前執行的環境</param>
    /// <param name="apiResultSettingCollection">API 回傳結果的設定檔集合</param>
    /// <param name="tokenHandlingBehaviors">Token 處理的相關行為</param>
    public Api003AccountApiHandler(HttpClient httpClient, IHostEnvironment hostEnvironment,
        ApiResultSettingCollection apiResultSettingCollection, TokenHandlingBehaviors? tokenHandlingBehaviors = null)
        : base(httpClient, hostEnvironment, apiResultSettingCollection, tokenHandlingBehaviors)
    {
    }

    /// <summary>
    /// 取得指定的使用者特定設定檔資料
    /// </summary>
    /// <param name="activityId">代表本次活動的 ID</param>
    /// <param name="code">用於取得 Token 的 Code</param>
    /// <param name="authentication">驗證資訊</param>
    /// <returns></returns>
    public async Task<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenByAuthorizationCodeResponse> AcquireTokenByAuthorizationCodeAsync(Guid activityId, string code, AuthenticationHeaderValue authentication)
    {
        JToken returnedJToken = await base.PostAsync(activityId, "api/API003Account/AcquireTokenByAuthorizationCode", new SortedDictionary<string, object>()
        {
            {"Code",code }
        }, authentication);
        return returnedJToken.ToObject<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenByAuthorizationCodeResponse>()!;
    }
    /// <summary>
    /// 取得指定的使用者特定設定檔資料
    /// </summary>
    /// <param name="activityId">代表本次活動的 ID</param>
    /// <param name="homeAccountId">使用者 HomeAccountId</param>
    /// <param name="authentication">驗證資訊</param>
    /// <returns></returns>
    public async Task<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenSilentResponse> AcquireTokenSilentAsync(Guid activityId, string homeAccountId, AuthenticationHeaderValue authentication)
    {
        JToken returnedJToken = await base.PostAsync(activityId, "api/API003Account/AcquireTokenSilent", new SortedDictionary<string, object>()
        {
            {"HomeAccountId",homeAccountId }
        }, authentication);
        return returnedJToken.ToObject<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenSilentResponse>()!;
    }
    /// <summary>
    /// 登出指定的帳號
    /// </summary>
    /// <param name="activityId">代表本次活動的 ID</param>
    /// <param name="homeAccountId">使用者 HomeAccountId</param>
    /// <returns></returns>
    public async Task<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.SignOutResponse> SignOutAsync(Guid activityId, string homeAccountId)
    {
        JToken returnedJToken = await base.PostAsync(activityId, "api/API003Account/SignOut", new SortedDictionary<string, object>()
        {
            {"HomeAccountId",homeAccountId }
        }, null);
        return returnedJToken.ToObject<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.SignOutResponse>()!;
    }
    /// <inheritdoc/>
    protected override EnvironmentSettingCollection GetEnvironmentSettingCollection(IHostEnvironment hostingEnvironment) =>
        EnvironmentSettingCollectionHelperInernal.GetFrontendEnvironmentSettingCollection(hostingEnvironment);
}
