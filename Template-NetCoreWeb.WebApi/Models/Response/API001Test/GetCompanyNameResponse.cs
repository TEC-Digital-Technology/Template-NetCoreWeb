using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Web.WebApi.Response;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Models.Response.API001Test;

/// <summary>
/// 取得公司名稱的回應資料
/// </summary>
public class GetCompanyNameResponse : ResponseBase<ResultCodeSettingEnum>
{
    /// <summary>
    /// 設定或取得公司名稱
    /// </summary>
    public string? ComapnyName { get; set; }
}
