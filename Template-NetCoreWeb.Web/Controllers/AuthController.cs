﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEC.Core.Logging;
using TEC.Core.Web;
using TEC.Core.Web.Mvc.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using TEC.Internal.Web.Core.Security.Settings;
using Template_NetCoreWeb.WebMvc.Settings;
using Template_NetCoreWeb.Utils.Enums.Settings;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.Security.Claims;
using Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo;
using Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Template_NetCoreWeb.WebMvc.Controllers;

public class AuthController : Controller
{
    public AuthController(ILoggerFactory loggerFactory, ClientApplicationSettingCollection clientApplicationSettingCollection,
        TokenAuthSettingCollection tokenAuthSettingCollection, IConfidentialClientApplication confidentialClientApplication,
        Api003AccountApiHandler api003AccountApiHandler, PersonalDataSettingCollection personalDataSettingCollection)
    {
        this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        this.ConfidentialClientApplication = confidentialClientApplication ?? throw new ArgumentNullException(nameof(confidentialClientApplication));
        this.ClientApplicationSettingCollection = clientApplicationSettingCollection ?? throw new ArgumentNullException(nameof(clientApplicationSettingCollection));
        this.TokenAuthSettingCollection = tokenAuthSettingCollection ?? throw new ArgumentNullException(nameof(tokenAuthSettingCollection));
        this.Api003AccountApiHandler = api003AccountApiHandler ?? throw new ArgumentNullException(nameof(api003AccountApiHandler));
        this.PersonalDataSettingCollection = personalDataSettingCollection ?? throw new ArgumentNullException(nameof(personalDataSettingCollection));

    }
    /// <summary>
    /// OAuth 登入流程
    /// </summary>
    /// <returns></returns>
    /// <remarks>此時 ADFS 已經是登入狀態，所以重新導向過去，不用再輸入帳密就能完成登入。</remarks>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> OAuthSignIn()
    {
        this.PersonalDataSettingCollection[PersonalDataSettingEnum.RedirectRelativePathWhenLoggedIn] = null;
        Uri authorizationUrl = await this.ConfidentialClientApplication.GetAuthorizationRequestUrl(new[] { "profile", "openid", "email" })
                .WithExtraQueryParameters(new Dictionary<string, string>()
                {
                    {"resource", this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.GraphResourceId].ToString()! }
                })
                .ExecuteAsync();
        return base.Redirect(authorizationUrl.AbsoluteUri);
    }
    /// <summary>
    /// 4. 登入系統
    /// </summary>
    /// <param name="code"></param>
    /// <param name="error"></param>
    /// <param name="error_description"></param>
    /// <param name="resource"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<ActionResult> OAuth(string code, string error, string error_description, string resource, string state)
    {
        // Redeem the authorization code from the response for an access token and refresh token.
        if (String.IsNullOrWhiteSpace(code))
        {
            return base.Content("登入發生問題，請稍後再試");
        }

        AcquireTokenByAuthorizationCodeResponse acquireTokenByAuthorizationCodeResponse = await this.Api003AccountApiHandler.AcquireTokenByAuthorizationCodeAsync(HttpContextProvider.CurrentActivityId!.Value, code, null!);
        if (!TEC.Internal.Web.Core.Security.AuthHelper.TryParseAccountCliamsInfo(acquireTokenByAuthorizationCodeResponse.AccessToken!, this.TokenAuthSettingCollection, out TEC.Internal.Web.Core.Security.AccountCliamsInfo accountCliamsInfo))
        {
            return base.Content("登入發生問題，請稍後再試");
        }
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(nameof(IAccount.HomeAccountId), acquireTokenByAuthorizationCodeResponse.HomeAccountId!));
        claims.Add(new Claim("AccessToken", acquireTokenByAuthorizationCodeResponse.AccessToken!));
        claims.Add(new Claim("IdToken", acquireTokenByAuthorizationCodeResponse.IdToken!));
        claims.Add(new Claim(ClaimTypes.Name, accountCliamsInfo.UPN));
        var applicationCookieIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await base.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(applicationCookieIdentity), new AuthenticationProperties()
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddMonths(1),
            IsPersistent = true,//不因瀏覽器關閉而消滅
            IssuedUtc = DateTimeOffset.FromUnixTimeSeconds(accountCliamsInfo.Iat)
        });
        string? redirectPath = this.PersonalDataSettingCollection[PersonalDataSettingEnum.RedirectRelativePathWhenLoggedIn];
        if (redirectPath == null || new Uri(redirectPath!, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
        {
            redirectPath = base.Url.Action("Index", "AuthorizedRequired")!;
        }
        // Return to the originating page where the user triggered the sign-in
        return base.Redirect(redirectPath);
    }
    /// <summary>
    /// 登出系統
    /// </summary>
    /// <param name="redirectPath"></param>
    /// <returns></returns>
    public async Task<ActionResult> SignOut(string redirectPath = "/Home/Index")
    {
        await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Claim homeAccountIdClaim = base.HttpContext.User.Claims.Single(t => t.Type == nameof(IAccount.HomeAccountId));
        await this.Api003AccountApiHandler.SignOutAsync(HttpContextProvider.CurrentActivityId!.Value, homeAccountIdClaim.Value);
        string signoutUrlFormat = "https://adfs.tecyt.com/adfs/oauth2/logout?id_token_hint={0}&post_logout_redirect_uri={1}";
        //清除個人設定
        this.PersonalDataSettingCollection.Clear();
        //以便在完成 TEC Portal 登出流程後，可以導向到登出後要瀏覽的位址
        this.PersonalDataSettingCollection[PersonalDataSettingEnum.RedirectRelativePathWhenLoggedOut] = redirectPath;
        UriBuilder uriBuilder = new UriBuilder((Uri)this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.FrontendBaseUrl]);
        uriBuilder.Path = base.Url.Action(nameof(SignedOut), "Auth")!;
        Claim idTokenClaim = base.HttpContext.User.Claims.Single(t => t.Type == "IdToken");
        return base.Redirect(String.Format(signoutUrlFormat, idTokenClaim.Value, HttpUtility.UrlEncode(uriBuilder.ToString())));
    }

    /// <summary>
    /// 6. 登出完成後跳轉回的頁面
    /// </summary>
    /// <returns></returns>
    public ActionResult SignedOut()
    {
        string? redirectPath = this.PersonalDataSettingCollection[PersonalDataSettingEnum.RedirectRelativePathWhenLoggedOut];
        if (redirectPath == null || new Uri(redirectPath!, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
        {
            redirectPath = base.Url.Action("Index", "Home")!;
        }
        return base.Redirect(redirectPath);
    }

    /// <summary>
    /// 取得處理記錄檔工廠 
    /// </summary>
    private ILoggerFactory LoggerFactory { get; }
    /// <summary>
    /// 取得客戶端應用程式資料
    /// </summary>
    private IConfidentialClientApplication ConfidentialClientApplication { get; }
    /// <summary>
    /// 取得客戶端應用程式設定檔集合 
    /// </summary>
    private ClientApplicationSettingCollection ClientApplicationSettingCollection { get; }
    /// <summary>
    /// 取得認證資料設定檔集合 
    /// </summary>
    private TokenAuthSettingCollection TokenAuthSettingCollection { get; }
    /// <summary>
    /// 取得 API003Account 介接物件
    /// </summary>
    private Api003AccountApiHandler Api003AccountApiHandler { get; }
    /// <summary>
    /// 取得儲存於 Session 的個人化設定檔集合
    /// </summary>
    private PersonalDataSettingCollection PersonalDataSettingCollection { get; }
}
