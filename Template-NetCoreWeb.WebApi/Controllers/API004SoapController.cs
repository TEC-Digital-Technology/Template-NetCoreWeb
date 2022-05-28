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
using Template_NetCoreWeb.WebApi.Models.Request.API004Soap;
using Template_NetCoreWeb.WebApi.Models.Response.API004Soap;

namespace Template_NetCoreWeb.WebApi.Controllers
{
    /// <summary>
    /// API-004 SOAP 控制器
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class API004SoapController : ControllerBase
    {
        public API004SoapController(SoapDemoLoggingManager soapDemoLoggingManager)
        {
            this.SoapDemoLoggingManager = soapDemoLoggingManager ?? throw new ArgumentNullException(nameof(soapDemoLoggingManager));
        }
        /// <summary>
        /// 將兩個整數相加
        /// </summary>
        /// <param name="addIntegerRequest">將兩個整數相加的請求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddIntegerResponse> AddInteger([FromBody] AddIntegerRequest addIntegerRequest)
        {
            long result = await this.SoapDemoLoggingManager.ExecuteAsync(HttpContextProvider.CurrentActivityId!.Value, async client =>
            {
                return await client.AddIntegerAsync(addIntegerRequest.Number1!.Value, addIntegerRequest.Number2!.Value);
            });
            return new AddIntegerResponse()
            {
                Result = result
            };
        }

        /// <summary>
        /// 設定或取得帳號處理物件
        /// </summary>
        private SoapDemoLoggingManager SoapDemoLoggingManager { set; get; }
    }
}
