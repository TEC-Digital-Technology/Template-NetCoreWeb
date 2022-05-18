using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Web.WebApi.Response;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Models.Response.API001Test
{
    /// <summary>
    /// 取得名字的回應資料
    /// </summary>
    public class GetMyInformationResponse : ResponseBase<ResultCodeSettingEnum>
    {
        /// <summary>
        /// 設定或取得名字
        /// </summary>
        public string? YourName { get; set; }
        /// <summary>
        /// 設定或取得支援的銀行
        /// </summary>
        public string? Banks { set; get; }
    }
}
