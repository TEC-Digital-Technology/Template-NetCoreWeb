using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API004Soap;

/// <summary>
/// 兩個整數相加的請求
/// </summary>
public class AddIntegerRequest
{
    /// <summary>
    /// 設定或取得第一個數字
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public int? Number1{ set; get; }
    /// <summary>
    /// 設定或取得第二個數字
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public int? Number2 { set; get; }
}
