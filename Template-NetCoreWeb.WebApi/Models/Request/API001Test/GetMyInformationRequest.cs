using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.DataAnnotations;

namespace Template_NetCoreWeb.WebApi.Models.Request.API001Test
{
    /// <summary>
    /// 取得名字的請求
    /// </summary>
    public class GetMyInformationRequest
    {
        /// <summary>
        /// 設定或取得名字
        /// </summary>
        [Required(ErrorMessage = "1000")]
        public string? Name { set; get; }
        /// <summary>
        /// 設定或取得適用銀行名稱
        /// </summary>
        [ItemRequired(ErrorMessage = "1002")]
        public List<string>? BankNames { set; get; }
    }
}
