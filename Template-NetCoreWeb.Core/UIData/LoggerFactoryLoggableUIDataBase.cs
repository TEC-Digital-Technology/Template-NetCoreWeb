using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Logging.UIData;
using Template_NetCoreWeb.Core.Logging;

namespace Template_NetCoreWeb.Core.UIData
{
    /// <summary>
    /// 解決方案專用，使用 <see cref="ILoggerFactory"/> 的 UIData 處理類別 
    /// </summary>
    public class LoggerFactoryLoggableUIDataBase : LoggableUIDataBase, IRequireLoggerFactory
    {
        /// <summary>
        /// 初始化使用 <see cref="ILoggerFactory"/> 的 UIData 請求處理類別
        /// </summary>
        /// <param name="loggableUIDataOptions">用於初始化 <see cref="LoggableUIDataBase"/> 的選項物件</param>
        /// <param name="loggerFactory">關於此物件所需使用的 <see cref="ILoggerFactory"/></param>
        public LoggerFactoryLoggableUIDataBase(LoggableUIDataOptions loggableUIDataOptions, ILoggerFactory? loggerFactory)
            : base(loggableUIDataOptions)
        {
            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        /// <summary>
        /// 取得物件所需使用的 <see cref="ILoggerFactory"/>
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }
    }
}
