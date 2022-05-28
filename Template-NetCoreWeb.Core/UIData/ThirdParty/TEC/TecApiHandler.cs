using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.TEC
{
    /// <summary>
    /// TEC API 介接物件，此類別無法被繼承
    /// </summary>
    public sealed class TecApiHandler
    {
        private readonly HttpClient _httpClient;
        public TecApiHandler(HttpClient? httpClient)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        /// <summary>
        /// 取得三瑪郵遞區號清單
        /// </summary>
        /// <returns></returns>
        public async Task<JArray> GetZipCode3Async()
        {
            var response = await this._httpClient.GetAsync("https://api.tecyt.com/api/API0101Address/GetZipCode3");
            return JArray.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}
