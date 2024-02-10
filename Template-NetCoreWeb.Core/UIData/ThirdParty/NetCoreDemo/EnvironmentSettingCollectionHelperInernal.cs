using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Internal.Web.Core.ApiProxy.Settings;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo;

/// <summary>
/// 處理產生 <see cref="EnvironmentSettingCollection"/> 的靜態輔助類別
/// </summary>
internal static class EnvironmentSettingCollectionHelperInernal
{
    /// <summary>
    /// 以輸入的環境資訊，產生對應前台的環境設定
    /// </summary>
    /// <param name="hostEnvironment">環境資訊</param>
    /// <returns></returns>
    internal static EnvironmentSettingCollection GetFrontendEnvironmentSettingCollection(IHostEnvironment hostEnvironment)
    {
        //因為 Web.Core API 需要有驗證資訊，介接時需要提供 OBO 的 Client ID。
        //這段在本專案沒有任何作用，可以隨意填寫
        //若需要正確用法，可以參考 Internal Utils 套件的 API 介接程式碼
        EnvironmentSettingCollection result = new EnvironmentSettingCollection(hostEnvironment)
        {
            {EnvironmentSettingEnum.OboClientId, "http://demo.com/fe-prd" },
            {EnvironmentSettingEnum.OboScopes , new List<string>() { "openid" } }
        };
        if (hostEnvironment.IsDevelopment())
        {
            result[EnvironmentSettingEnum.OboClientId] = "http://demo.com/fe-dev";
        }
        return result;
    }
}
