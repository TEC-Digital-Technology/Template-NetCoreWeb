using TEC.Core.Web.WebApi.Response;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Models.Response.API005InternalLib;

/// <summary>
/// 透過 Email 取得指定帳號的回應資料
/// </summary>
public class GetAccountInfoByEmailResponse : ResponseBase<ResultCodeSettingEnum>
{
    /// <summary>
    /// 設定或取得查詢結果
    /// </summary>
    public TEC.Internal.Web.AccountService.Response.S001Account.AccountInfoResponse? Result { set; get; }
}
