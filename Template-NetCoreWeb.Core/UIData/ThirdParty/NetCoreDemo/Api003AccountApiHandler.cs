using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TEC.Internal.Web.Core.ApiProxy.Settings;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo
{
    /// <summary>
    /// 介接 API003Account API 的類別
    /// </summary>
    public class Api003AccountApiHandler : TEC.Internal.Web.Core.ApiProxy.ApiHandlerBase
    {
        public Api003AccountApiHandler(HttpClient httpClient, ApiResultSettingCollection apiResultSettingCollection, ApiClientSettingCollection apiClientSettingCollection)
            : base(httpClient, apiResultSettingCollection, apiClientSettingCollection)
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
        /// <param name="id">使用者 ID (UPN)</param>
        /// <param name="authentication">驗證資訊</param>
        /// <returns></returns>
        public async Task<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenSilentResponse> AcquireTokenSilentAsync(Guid activityId, string id, AuthenticationHeaderValue authentication)
        {
            JToken returnedJToken = await base.PostAsync(activityId, "api/API003Account/AcquireTokenSilent", new SortedDictionary<string, object>()
            {
                {"Id",id }
            }, authentication);
            return returnedJToken.ToObject<Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response.AcquireTokenSilentResponse>()!;
        }
    }
}
