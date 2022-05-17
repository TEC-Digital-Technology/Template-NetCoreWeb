using Microsoft.AspNetCore.Mvc;
using TEC.Core.Logging;
using TEC.Core.Web;
using TEC.Core.Web.Mvc.Extensions;
using Template_NetCoreWeb.WebMvc.Controllers.Models.TestFeature;

namespace Template_NetCoreWeb.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string partialViewHtml = await this.RenderViewToStringAsync("~/Views/TestFeature/_RenderPartialView.cshtml", new RenderPartialViewModel() { Name = "Test Test!!" }, true);
            return View();
        }

        [IgnoreLogging]
        public void TestActionIgnoreLogging()
        {
            if (!HttpContextProvider.CurrentIgnoreLogging)
            {
                throw new Exception($"標記 {nameof(IgnoreLoggingAttribute)} 的 Action 不應被判斷成不忽略紀錄行為。");
            }
        }

    }
}
