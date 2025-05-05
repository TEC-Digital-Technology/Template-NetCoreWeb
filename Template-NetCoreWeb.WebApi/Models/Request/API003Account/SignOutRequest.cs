using System.ComponentModel.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API003Account;

/// <summary>
/// 登出帳號的請求
/// </summary>
public class SignOutRequest
{
    /// <summary>
    /// 設定或取得 HomeAccountId
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? HomeAccountId { set; get; }
}
