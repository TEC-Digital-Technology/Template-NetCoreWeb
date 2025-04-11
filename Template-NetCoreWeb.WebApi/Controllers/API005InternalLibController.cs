using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Web;
using Template_NetCoreWeb.Core.Logging.SoapManagers;
using Template_NetCoreWeb.Core.UIData;
using Template_NetCoreWeb.Utils.Enums;
using Template_NetCoreWeb.WebApi.Exceptions;
using Template_NetCoreWeb.WebApi.Models.Request.API005InternalLib;
using Template_NetCoreWeb.WebApi.Models.Response.API005InternalLib;

namespace Template_NetCoreWeb.WebApi.Controllers;

/// <summary>
/// API-005 Internal Libraries 控制器
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[Authorize(Const.frontendAudiencePolicyName)]
public class API005InternalLibController : ControllerBase
{
    /// <summary>
    /// 初始化 API-005 Internal Libraries 控制器
    /// </summary>
    public API005InternalLibController(TEC.Internal.Web.AccountService.S001AccountApiHandler accountApiHandler)
    {
        this.AccountApiHandler = accountApiHandler ?? throw new ArgumentNullException(nameof(accountApiHandler));
    }
    /// <summary>
    /// 透過 Email 取得指定帳號的資訊
    /// </summary>
    /// <param name="getAccountInfoByEmailRequest">透過 Email 取得指定帳號資訊的請求</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GetAccountInfoByEmailResponse> GetAccountInfoByEmail([FromBody] GetAccountInfoByEmailRequest getAccountInfoByEmailRequest)
    {
        return new GetAccountInfoByEmailResponse()
        {
            Result = await this.AccountApiHandler.GetAccountInfoByEmailAsync(HttpContextProvider.CurrentActivityId!.Value,
                new TEC.Internal.Web.AccountService.Request.S001Account.GetAccountInfoByEmailRequest(getAccountInfoByEmailRequest.Email))
        };
    }

    /// <summary>
    /// 取得帳號處理物件
    /// </summary>
    private TEC.Internal.Web.AccountService.S001AccountApiHandler AccountApiHandler { get; }
}
