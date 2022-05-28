using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API003Account
{
    /// <summary>
    /// 新增帳號的請求
    /// </summary>
    public class AddAccountRequest
    {
        /// <summary>
        /// 設定或取得帳號
        /// </summary>
        [Required(ErrorMessage = "1000")]
        public string? Username { set; get; }
        /// <summary>
        /// 設定或取得適用銀行名稱
        /// </summary>
        [Required(ErrorMessage = "1000")]
        public string? Password { set; get; }
    }
}
