using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.TECApi;

/// <summary>
/// TEC API 介接物件，此類別無法被繼承
/// </summary>
public sealed class TecApiHandler
{
    /// <summary>
    /// 初始化 TEC API 介接物件
    /// </summary>
    /// <param name="httpClient">此物件使用的 <see cref="System.Net.HttpClient"/></param>
    /// <exception cref="ArgumentNullException">當<paramref name="httpClient"/>為<c>null</c>參考時擲出</exception>
    public TecApiHandler(HttpClient? httpClient)
    {
        this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    /// <summary>
    /// 取得三瑪郵遞區號清單
    /// </summary>
    /// <returns></returns>
    public async Task<JArray> GetZipCode3Async()
    {
        var response = await this.HttpClient.GetAsync("https://api.tecyt.com/api/API0101Address/GetZipCode3");
        return JArray.Parse(await response.Content.ReadAsStringAsync());
    }
    /// <summary>
    /// 取得物件內使用的 <see cref="System.Net.HttpClient"/> 執行個體
    /// </summary>
    private HttpClient HttpClient { get; }
}
