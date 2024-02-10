using TEC.Core.Web.WebApi.Response;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Models.Response.API004Soap;

/// <summary>
/// 兩數相加的回應資料
/// </summary>
public class AddIntegerResponse : ResponseBase<ResultCodeSettingEnum>
{
    /// <summary>
    /// 設定或取得相加的結果
    /// </summary>
    public long Result { set; get; }
}
