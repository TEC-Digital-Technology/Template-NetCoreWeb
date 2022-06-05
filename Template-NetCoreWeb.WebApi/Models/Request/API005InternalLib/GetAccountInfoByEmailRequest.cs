using System.ComponentModel.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API005InternalLib
{
    /// <summary>
    /// 透過 Email 取得指定帳號資訊的請求
    /// </summary>
    public class GetAccountInfoByEmailRequest
    {
        /// <summary>
        /// 設定或取得 Email
        /// </summary>
        [Required(ErrorMessage = "1000")]
        public string? Email{ set; get; }
    }
}
