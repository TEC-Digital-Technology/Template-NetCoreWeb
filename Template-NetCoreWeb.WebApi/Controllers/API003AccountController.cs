using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Web;
using Template_NetCoreWeb.Core.UIData;
using Template_NetCoreWeb.Utils.Enums;
using Template_NetCoreWeb.Utils.Enums.Settings;
using Template_NetCoreWeb.WebApi.Exceptions;
using Template_NetCoreWeb.WebApi.Models.Request.API003Account;
using Template_NetCoreWeb.WebApi.Models.Response.API003Account;
using Template_NetCoreWeb.WebApi.Settings;

namespace Template_NetCoreWeb.WebApi.Controllers;

/// <summary>
/// API-003 帳號控制器
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public class API003AccountController : ControllerBase
{
    public API003AccountController(AccountUIData accountUIData, ClientApplicationSettingCollection clientApplicationSettingCollection,
        IConfidentialClientApplication confidentialClientApplication)
    {
        this.AccountUIData = accountUIData ?? throw new ArgumentNullException(nameof(accountUIData));
        this.ClientApplicationSettingCollection = clientApplicationSettingCollection ?? throw new ArgumentNullException(nameof(clientApplicationSettingCollection));
        this.ConfidentialClientApplication = confidentialClientApplication ?? throw new ArgumentNullException(nameof(confidentialClientApplication));
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
    /// 由 Authorization Code 取得認證結果
    /// </summary>
    /// <param name="acquireTokenByAuthorizationCodeRequest">要由 Authorization Code 取得認證結果的請求</param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<AcquireTokenByAuthorizationCodeResponse> AcquireTokenByAuthorizationCode(AcquireTokenByAuthorizationCodeRequest acquireTokenByAuthorizationCodeRequest)
    {
        AuthenticationResult authenticationResult = await this.ConfidentialClientApplication.AcquireTokenByAuthorizationCode(new[] { "profile", "openid", "email" }, acquireTokenByAuthorizationCodeRequest.Code!)
            .WithCorrelationId(HttpContextProvider.CurrentActivityId!.Value)
            .WithExtraQueryParameters(new Dictionary<string, string>()
                {
                    {"resource",this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.GraphResourceId].ToString()! }
                }).ExecuteAsync();
        AcquireTokenByAuthorizationCodeResponse acquireTokenByAuthorizationCodeResponse = new AcquireTokenByAuthorizationCodeResponse();
        acquireTokenByAuthorizationCodeResponse.HomeAccountId = authenticationResult.Account.HomeAccountId.Identifier;
        acquireTokenByAuthorizationCodeResponse.AccessToken = authenticationResult.AccessToken;
        acquireTokenByAuthorizationCodeResponse.AccessTokenType = authenticationResult.TokenType;
        acquireTokenByAuthorizationCodeResponse.Authority = this.ConfidentialClientApplication.Authority;
        acquireTokenByAuthorizationCodeResponse.ExpiresOn = authenticationResult.ExpiresOn;
        acquireTokenByAuthorizationCodeResponse.ExtendedLifeTimeToken = authenticationResult.IsExtendedLifeTimeToken;
        acquireTokenByAuthorizationCodeResponse.IdToken = authenticationResult.IdToken;
        acquireTokenByAuthorizationCodeResponse.TenantId = authenticationResult.TenantId;
        return acquireTokenByAuthorizationCodeResponse;
    }

    /// <summary>
    /// 不透過讓使用者輸入認證的方式，背景取得 Token。
    /// </summary>
    /// <param name="acquireTokenSilentRequest">不透過讓使用者輸入認證的方式，背景取得 Token 的請求</param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<AcquireTokenSilentResponse> AcquireTokenSilent(AcquireTokenSilentRequest acquireTokenSilentRequest)
    {
        AuthenticationResult authenticationResult;
        try
        {
            IAccount account = await this.ConfidentialClientApplication.GetAccountAsync(acquireTokenSilentRequest.HomeAccountId!);
            authenticationResult = await this.ConfidentialClientApplication.AcquireTokenSilent(new[] { "profile", "openid", "email" }, account)
                .WithCorrelationId(HttpContextProvider.CurrentActivityId!.Value)
                .WithForceRefresh(true)
                .WithExtraQueryParameters(new Dictionary<string, string>()
                {
                    {"resource",this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.GraphResourceId].ToString()! }
                }).ExecuteAsync();
        }
        catch (Exception ex)
        {
            throw new OperationFailedException(ResultCodeSettingEnum.SilentTokenAcquisitionFailed, ex);
        }
        AcquireTokenSilentResponse acquireTokenSilentResponse = new AcquireTokenSilentResponse();
        acquireTokenSilentResponse.HomeAccountId = authenticationResult.Account.HomeAccountId.Identifier;
        acquireTokenSilentResponse.AccessToken = authenticationResult.AccessToken;
        acquireTokenSilentResponse.AccessTokenType = authenticationResult.TokenType;
        acquireTokenSilentResponse.Authority = this.ConfidentialClientApplication.Authority;
        acquireTokenSilentResponse.ExpiresOn = authenticationResult.ExpiresOn;
        acquireTokenSilentResponse.ExtendedLifeTimeToken = authenticationResult.IsExtendedLifeTimeToken;
        acquireTokenSilentResponse.IdToken = authenticationResult.IdToken;
        acquireTokenSilentResponse.TenantId = authenticationResult.TenantId;
        return acquireTokenSilentResponse;
    }
    /// <summary>
    /// 取得帳號處理物件
    /// </summary>
    private AccountUIData AccountUIData { get; }
    /// <summary>
    /// 取得認證資料設定檔集合 
    /// </summary>
    private ClientApplicationSettingCollection ClientApplicationSettingCollection { get; }
    /// <summary>
    /// 取得客戶端應用程式資料
    /// </summary>
    private IConfidentialClientApplication ConfidentialClientApplication { get; }
}
