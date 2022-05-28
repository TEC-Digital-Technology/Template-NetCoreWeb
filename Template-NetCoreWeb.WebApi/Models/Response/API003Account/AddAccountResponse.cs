using TEC.Core.Web.WebApi.Response;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Models.Response.API003Account
{
    /// <summary>
    /// 新增帳號的回應資料
    /// </summary>
    public class AddAccountResponse : ResponseBase<ResultCodeSettingEnum>
    {
        /// <summary>
        /// 設定或取得已新增的帳號 ID
        /// </summary>
        public Guid AccountId { set; get; }
    }
}
