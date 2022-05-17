using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Utils.Logging
{
    /// <summary>
    /// 處理紀錄檔的靜態擴充類別
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// 紀錄偵錯資料
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="logState"></param>
        public static void Log(this ILoggerFactory loggerFactory, LogState logState)
        {
            loggerFactory.CreateLogger(logState.SystemScope.ToString()).Log(logState.LogLevel,
                new EventId(logState.EventTypeId, logState.MessageType.ToString()), logState, logState.Exception, (state, exception) => state.Message);
        }
    }
}
