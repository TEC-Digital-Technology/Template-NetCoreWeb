using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEC.Core.Logging;
using TEC.Core.Web;
using TEC.Core.Web.Mvc.Extensions;

namespace Template_NetCoreWeb.WebMvc.Controllers
{
    [Authorize]
    public class AuthorizedRequiredController : Controller
    {
        public AuthorizedRequiredController(ILoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IActionResult> Index()
        {
            return base.View();
        }

        /// <summary>
        /// 取得處理記錄檔工廠 
        /// </summary>
        private ILoggerFactory LoggerFactory { get; }
    }
}
