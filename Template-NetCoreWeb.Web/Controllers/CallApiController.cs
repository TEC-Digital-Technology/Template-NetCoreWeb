using Microsoft.AspNetCore.Mvc;
using TEC.Core.Logging;
using TEC.Core.Web;
using TEC.Core.Web.Mvc.Extensions;
using Template_NetCoreWeb.Core.UIData.ThirdParty.TEC;

namespace Template_NetCoreWeb.WebMvc.Controllers
{
    public class CallApiController : Controller
    {
        public CallApiController(TecApiHandler tecApiHandler)
        {
            this.TecApiHandler = tecApiHandler ?? throw new ArgumentNullException(nameof(tecApiHandler));
        }

        public async Task<IActionResult> Index()
        {
            var code3Task1 = this.TecApiHandler.GetZipCode3Async();
            var code3Task2 = this.TecApiHandler.GetZipCode3Async();
            var code3Result1 = await code3Task1;
            var code3Result2 = await code3Task2;
            return base.View();
        }

        /// <summary>
        /// 取得連接 TEC API 的介接物件
        /// </summary>
        private TecApiHandler TecApiHandler { get; }
    }
}
