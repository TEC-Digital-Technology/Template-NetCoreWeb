using Microsoft.AspNetCore.Authorization;
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

namespace Template_NetCoreWeb.WebMvc.Controllers
{
    public class AuthController : Controller
    {
        public AuthController(ILoggerFactory loggerFactory, ClientApplicationSettingCollection clientApplicationSettingCollection,
            AuthSettingCollection authSettingCollection,
            IConfidentialClientApplication confidentialClientApplication, Api003AccountApiHandler api003AccountApiHandler)
        {
            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.ConfidentialClientApplication = confidentialClientApplication ?? throw new ArgumentNullException(nameof(confidentialClientApplication));
            this.ClientApplicationSettingCollection = clientApplicationSettingCollection ?? throw new ArgumentNullException(nameof(clientApplicationSettingCollection));
            this.AuthSettingCollection = authSettingCollection ?? throw new ArgumentNullException(nameof(authSettingCollection));
            this.Api003AccountApiHandler = api003AccountApiHandler ?? throw new ArgumentNullException(nameof(api003AccountApiHandler));

        }

        /// <summary>
        /// 1. 預設 TEC Portal 登入流程
        /// </summary>
        /// <param name="redirectPath">登入後跳轉相對位置</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignIn(string redirectPath = "/AuthorizedRequired/Index")
        {
            string clientId = this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.ClientId].ToString()!;
            //以便在完成 TEC Portal 登入流程後，可以導向回原本要瀏覽的位址
            base.TempData["SignInRedirectPath"] = redirectPath;
            UriBuilder uriBuilder = new UriBuilder(this.PortalUri);
            uriBuilder.Path = "/AccountManagement/TEC/RedirectToSignIn";
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
            nameValueCollection.Add("sourcePlatformServiceId", clientId);
            uriBuilder.Query = nameValueCollection.ToString();
            return base.Redirect(uriBuilder.ToString());
        }

        /// <summary>
        /// 2. 從 TEC Portal 登入後的跳轉邏輯
        /// </summary>
        /// <param name="is_register">是否為全新註冊 TEC 的用戶</param>
        /// <returns></returns>
        /// <remarks>這個網址必須與 TEC Portal 的重新導向設定網址相同</remarks>
        public ActionResult Portal(bool? is_register)
        {
            return base.RedirectToAction("OAuthSignIn");
        }

        /// <summary>
        /// 3. OAuth 登入流程
        /// </summary>
        /// <returns></returns>
        /// <remarks>此時 ADFS 已經是登入狀態，所以重新導向過去，不用再輸入帳密就能完成登入。</remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OAuthSignIn()
        {
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
            if (!TEC.Internal.Web.Core.Security.AuthHelper.TryParseAccountCliamsInfo(acquireTokenByAuthorizationCodeResponse.AccessToken!, this.AuthSettingCollection, out TEC.Internal.Web.Core.Security.AccountCliamsInfo accountCliamsInfo))
            {
                return base.Content("登入發生問題，請稍後再試");
            }
            List<Claim> claims = new List<Claim>();
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
            string? redirectPath = base.TempData["SignInRedirectPath"]?.ToString()!;
            if (redirectPath == null || new Uri(redirectPath!, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
            {
                redirectPath = base.Url.Action("Index", "AuthorizedRequired")!;
            }
            // Return to the originating page where the user triggered the sign-in
            return base.Redirect(redirectPath);
        }
        /// <summary>
        /// 5. 登出系統
        /// </summary>
        /// <param name="redirectPath"></param>
        /// <returns></returns>
        public async Task<ActionResult> SignOut(string redirectPath = "/Home/Index")
        {
            await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string clientId = this.ClientApplicationSettingCollection[ClientApplicationSettingEnum.ClientId].ToString()!;
            //以便在完成 TEC Portal 登出流程後，可以導向到登出後要瀏覽的位址
            base.TempData["SignOutRedirectPath"] = redirectPath;
            UriBuilder uriBuilder = new UriBuilder(this.PortalUri);
            uriBuilder.Path = "/AccountManagement/TEC/SignOut";
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
            nameValueCollection.Add("sourcePlatformServiceId", clientId);
            uriBuilder.Query = nameValueCollection.ToString();
            return base.Redirect(uriBuilder.ToString());
        }

        /// <summary>
        /// 6. 登出完成後跳轉回的頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult SignedOut()
        {
            string? redirectPath = base.TempData["SignOutRedirectPath"]?.ToString()!;
            if (redirectPath == null || new Uri(redirectPath!, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
            {
                redirectPath = base.Url.Action("Index", "Home")!;
            }
            return base.Redirect(redirectPath);
        }

        /// <summary>
        /// 取得 TEC Portal 的 Uri
        /// </summary>
        private Uri PortalUri
        {
            get
            {
#if DEBUG
                return new Uri("https://portal-dev.tpe.tecyt.com/");
#else
                return new Uri("https://portal.tecyt.com/");
#endif
            }
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
        private AuthSettingCollection AuthSettingCollection { get; }
        /// <summary>
        /// 取得 API003Account 介接物件
        /// </summary>
        private Api003AccountApiHandler Api003AccountApiHandler { get; }
    }
}
