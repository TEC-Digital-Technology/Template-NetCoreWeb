using System.ComponentModel.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API003Account;

/// <summary>
/// 不透過讓使用者輸入認證的方式，背景取得 Token的請求
/// </summary>
public class AcquireTokenSilentRequest
{
    /// <summary>
    /// 設定或取得 HomeAccountId
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? HomeAccountId { set; get; }
}
