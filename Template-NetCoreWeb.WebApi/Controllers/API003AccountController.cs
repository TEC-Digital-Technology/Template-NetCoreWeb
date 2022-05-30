using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template_NetCoreWeb.Core.UIData;
using Template_NetCoreWeb.Utils.Enums;
using Template_NetCoreWeb.WebApi.Exceptions;
using Template_NetCoreWeb.WebApi.Models.Request.API003Account;
using Template_NetCoreWeb.WebApi.Models.Response.API003Account;

namespace Template_NetCoreWeb.WebApi.Controllers
{
    /// <summary>
    /// API-003 帳號控制器
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class API003AccountController : ControllerBase
    {
        public API003AccountController(AccountUIData accountUIData)
        {
            this.AccountUIData = accountUIData ?? throw new ArgumentNullException(nameof(accountUIData));
        }
        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="addAccountRequest">取得公司名稱的請求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddAccountResponse> AddAccount([FromBody] AddAccountRequest addAccountRequest)
        {
            return await Task.FromResult(new AddAccountResponse()
            {
                AccountId = this.AccountUIData.AddAccount(addAccountRequest.Username!, addAccountRequest.Password!)
            });
        }

        /// <summary>
        /// 取得帳號處理物件
        /// </summary>
        private AccountUIData AccountUIData { get; }
    }
}
