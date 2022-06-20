using System.ComponentModel.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API003Account
{
    /// <summary>
    /// 要由 Authorization Code 取得認證結果的請求
    /// </summary>
    public class AcquireTokenByAuthorizationCodeRequest
    {
        /// <summary>
        /// 設定或取得 Code
        /// </summary>
        [Required(ErrorMessage = "1000")]
        public string? Code { set; get; }
    }
}
