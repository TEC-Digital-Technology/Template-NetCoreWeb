using Microsoft.AspNetCore.Mvc;
using TEC.Core.Logging;
using TEC.Core.Web;
using TEC.Core.Web.Mvc.Extensions;
using Template_NetCoreWeb.WebMvc.Controllers.Models.TestFeature;

namespace Template_NetCoreWeb.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IActionResult> Index()
        {
            this.LoggerFactory.LogDebug("Test!!!");
            string partialViewHtml = await this.RenderViewToStringAsync("~/Views/TestFeature/_RenderPartialView.cshtml", new RenderPartialViewModel() { Name = "Test Test!!" }, true);
            return base.View();
        }

        [IgnoreLogging]
        public void TestActionIgnoreLogging()
        {
            if (!HttpContextProvider.CurrentIgnoreLogging)
            {
                throw new Exception($"標記 {nameof(IgnoreLoggingAttribute)} 的 Action 不應被判斷成不忽略紀錄行為。");
            }
        }
        /// <summary>
        /// 取得處理記錄檔工廠
        /// </summary>
        private ILoggerFactory LoggerFactory { get; }
    }
}
