using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Identity.Client;
using System.Security.Claims;
using TEC.Core.Web;
using TEC.Internal.Web.Core.Security;
using TEC.Internal.Web.Core.Security.Settings;
using Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo;

namespace Template_NetCoreWeb.WebMvc.Security
{
    /// <summary>
    /// TEC 內部服務專用的 Cookie 認證事件
    /// </summary>
    public class TecCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        /// <summary>
        /// 初始化 TEC 內部服務專用的 Cookie 認證事件
        /// </summary>
        /// <param name="api003AccountApiHandler">用於更新 Access Token 的 API</param>
        /// <param name="authSettingCollection">用於驗證授權的資料</param>
        public TecCookieAuthenticationEvents(Api003AccountApiHandler api003AccountApiHandler, AuthSettingCollection authSettingCollection)
        {
            this.Api003AccountApiHandler = api003AccountApiHandler ?? throw new ArgumentNullException(nameof(api003AccountApiHandler));
            this.AuthSettingCollection = authSettingCollection ?? throw new ArgumentNullException(nameof(authSettingCollection));
        }
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var routeData = context.Request.HttpContext.GetRouteData();
            var routeValues = routeData.Values;
            if (!routeValues.Keys.Contains("Controller", StringComparer.InvariantCultureIgnoreCase) ||
                !routeValues.Keys.Contains("Action", StringComparer.InvariantCultureIgnoreCase))
            {
                //沒有 Controller / Action 就不繼續更新
                await base.ValidatePrincipal(context);
                return;
            }

            if (String.Compare(routeValues["Controller"]!.ToString(), "Auth", true) == 0)
            {
                //只要是 AuthController 就不進自動更新 Token 邏輯
                await base.ValidatePrincipal(context);
                return;
            }
            ClaimsIdentity identity = context.Principal!.Identities.Single();
            string accessToken = identity.FindFirst("AccessToken")!.Value;
            string homeAccountId = identity.FindFirst(nameof(IAccount.HomeAccountId))!.Value;
            bool shouldRenewToken = false;
            try
            {
                AccountCliamsInfo accountCliamsInfo = TEC.Internal.Web.Core.Security.AuthHelper.ParseAccountCliamsInfo(accessToken, this.AuthSettingCollection);
                if (DateTimeOffset.FromUnixTimeSeconds(accountCliamsInfo.Exp).ToLocalTime().Subtract(DateTimeOffset.Now).TotalMinutes < 5d)
                {
                    //剩下小於 5 分鐘內過期，要更新 Token
                    shouldRenewToken = true;
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                //過期就直接嘗試更新
                shouldRenewToken = true;
            }
            catch
            {
                context.RejectPrincipal();
                return;
            }
            if (shouldRenewToken)
            {
                try
                {
                    var acquireTokenSilentResult = await this.Api003AccountApiHandler.AcquireTokenSilentAsync(HttpContextProvider.CurrentActivityId!.Value!, homeAccountId!, null!);
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(nameof(IAccount.HomeAccountId), acquireTokenSilentResult.HomeAccountId!));
                    claims.Add(new Claim("AccessToken", acquireTokenSilentResult.AccessToken!));
                    claims.Add(new Claim("IdToken", acquireTokenSilentResult.IdToken!));
                    claims.Add(new Claim(ClaimTypes.Name, identity.Name!));
                    var applicationCookieIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    context.ReplacePrincipal(new ClaimsPrincipal(applicationCookieIdentity));
                    context.ShouldRenew = true;
                }
                catch
                {
                    context.RejectPrincipal();
                    return;
                }
            }
            await base.ValidatePrincipal(context);
        }
        /// <summary>
        /// 取得 API003Account 介接物件
        /// </summary>
        private Api003AccountApiHandler Api003AccountApiHandler { get; }
        /// <summary>
        /// 取得認證資料設定檔集合 
        /// </summary>
        private AuthSettingCollection AuthSettingCollection { get; }
    }
}
