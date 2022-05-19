using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Logging;
using Template_NetCoreWeb.Utils.Enums;
using Template_NetCoreWeb.WebApi.Exceptions;
using Template_NetCoreWeb.WebApi.Models.Request.API002Logging;
using Template_NetCoreWeb.WebApi.Models.Response.API002Logging;
using Template_NetCoreWeb.Utils.Logging;

namespace Template_NetCoreWeb.WebApi.Controllers
{
    /// <summary>
    /// API-002 記錄檔控制器
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class API002LoggingController : ControllerBase
    {
        /// <summary>
        /// 初始化 API-002 記錄檔控制器
        /// </summary>
        public API002LoggingController(ILoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        /// <summary>
        /// 新增一筆紀錄
        /// </summary>
        /// <param name="request">新增記錄請求</param>
        [HttpPost]
        [IgnoreLogging]
        public NewLogResponse NewLog(NewLogRequest request)
        {
            Exception? actualException = null;
            if (request.Exception != null)
            {
                actualException = request.DeserializedException;
                if (request.DeserializedException == null)
                {
                    actualException = new Exception("遠端傳入例外資料，但無法反序列化，此資料將記錄至 Data 屬性中。");
                    if (request.ExtendedProperties == null || String.IsNullOrWhiteSpace(request.ExtendedProperties.ToString()))
                    {
                        request.ExtendedProperties = new Dictionary<string, object>()
                        {
                            ["RemoteException"] = request.Exception
                        };
                    }
                    else
                    {
                        Dictionary<string, object> newExtendedProperties = new Dictionary<string, object>()
                        {
                            ["RemoteException"] = request.Exception,
                            ["OriginalExtendedProperties"] = request.ExtendedProperties
                        };
                        request.ExtendedProperties = newExtendedProperties;
                    }
                }
            }
            if (request.ExtendedProperties is JObject jObject && !String.IsNullOrWhiteSpace(request.HostName))
            {
                jObject.Add("HostName", JToken.FromObject(request.HostName));
            }
            else if (request.ExtendedProperties is Dictionary<string, object> dictionary && !String.IsNullOrWhiteSpace(request.HostName))
            {
                dictionary.Add("HostName", request.HostName);
            }
            if (request.ExtendedProperties == null && !String.IsNullOrWhiteSpace(request.HostName))
            {
                Dictionary<string, object> newExtendedProperties = new Dictionary<string, object>()
                {
                    ["HostName"] = request.HostName
                };
                request.ExtendedProperties = newExtendedProperties;
            }
            LogState logState = new LogState()
            {
                ActivityId = request.ActivityId!.Value,
                EventTypeId = (int)request.MessageType!.Value,
                Exception = actualException,
                ExtendProperties = request.ExtendedProperties,
                IPAddress = request.IPAddress,
                LoggingTriggerType = request.TriggerType!.Value,
                Message = request.Message,
                MessageType = request.MessageType!.Value,
                Resource = request.Resource,
                Scope = request.Scope!.Value,
                SystemScope = request.SystemScope!.Value,
                TriggerReferenceId = request.TriggerReferenceId
            };
            switch (request.Level!.ToUpper())
            {
                case "DEBUG":
                    logState.LogLevel = LogLevel.Debug;
                    break;
                case "INFO":
                    logState.LogLevel = LogLevel.Information;
                    break;
                case "WARN":
                    logState.LogLevel = LogLevel.Warning;
                    break;
                case "ERROR":
                    logState.LogLevel = LogLevel.Error;
                    break;
                case "FATAL":
                    logState.LogLevel = LogLevel.Critical;
                    break;
                case "TRACE":
                    logState.LogLevel = LogLevel.Trace;
                    break;
                default:
                    throw new OperationFailedException(ResultCodeSettingEnum.InvalidArgument);
            }
            this.LoggerFactory.Log(logState);
            return new NewLogResponse();
        }
        /// <summary>
        /// 設定或取得處理記錄檔工廠
        /// </summary>
        private ILoggerFactory LoggerFactory { set; get; }
    }
}
