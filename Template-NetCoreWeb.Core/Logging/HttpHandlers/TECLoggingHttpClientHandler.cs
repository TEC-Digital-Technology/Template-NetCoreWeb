using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Logging.Http;
using Template_NetCoreWeb.Utils.Enums.Logging;

namespace Template_NetCoreWeb.Core.Logging.HttpHandlers
{
    /// <summary>
    /// 紀錄雲騰 HTTP 請求的處理器
    /// </summary>
    public class TECLoggingHttpClientHandler : LoggerFactoryLoggingHttpClientHandlerBase
    {
        /// <summary>
        /// 初始化一個 <see cref="TECLoggingHttpClientHandler"/>
        /// </summary>
        /// <param name="httpMessageHandler">用於初始化 <see cref="LoggerFactoryLoggingHttpClientHandlerBase"/> HTTP 處理物件</param>
        /// <param name="ensureSuccessStatusCode">當 HTTP 回傳訊息時，是否要再非 200 情況下擲出例外</param>
        /// <param name="loggerFactory">關於此物件所需使用的 <see cref="ILoggerFactory"/></param>
        public TECLoggingHttpClientHandler(HttpMessageHandler httpMessageHandler, bool ensureSuccessStatusCode, ILoggerFactory? loggerFactory)
            : base(new LoggingHttpClientHandlerOptions<LoggingSystemScope>(LoggingSystemScope.TEC, httpMessageHandler, ensureSuccessStatusCode), loggerFactory)
        {
        }
    }
}
