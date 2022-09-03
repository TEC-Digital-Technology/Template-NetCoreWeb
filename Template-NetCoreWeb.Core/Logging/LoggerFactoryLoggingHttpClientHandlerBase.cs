using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Logging.Http;
using Template_NetCoreWeb.Utils.Enums.Logging;

namespace Template_NetCoreWeb.Core.Logging;

/// <summary>
/// 解決方案專用，使用 <see cref="ILoggerFactory"/> 的 HTTP 請求處理類別
/// </summary>
public abstract class LoggerFactoryLoggingHttpClientHandlerBase : LoggingHttpClientHandlerBase<LoggingSystemScope>, IRequireLoggerFactory
{
    /// <summary>
    /// 初始化使用 <see cref="ILoggerFactory"/> 的 HTTP 請求處理類別
    /// </summary>
    /// <param name="options">用於初始化 <see cref="LoggingHttpClientHandlerBase{LoggingSystemScope}"/> 的選項物件</param>
    /// <param name="loggerFactory">關於此物件所需使用的 <see cref="ILoggerFactory"/></param>
    public LoggerFactoryLoggingHttpClientHandlerBase(LoggingHttpClientHandlerOptions<LoggingSystemScope> options, ILoggerFactory? loggerFactory)
        : base(options)
    {
        this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
    /// <summary>
    /// 取得物件所需使用的 <see cref="ILoggerFactory"/>
    /// </summary>
    public ILoggerFactory LoggerFactory { get; }
}
