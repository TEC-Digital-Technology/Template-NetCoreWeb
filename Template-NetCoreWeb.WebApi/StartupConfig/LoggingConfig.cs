using TEC.Core.Web.Logging;
using Template_NetCoreWeb.Utils.Enums.Logging;
using Template_NetCoreWeb.Utils.Logging;

namespace Template_NetCoreWeb.WebApi.StartupConfig
{
    /// <summary>
    /// 處理記錄檔的類別
    /// </summary>
    internal static class LoggingConfig
    {
        /// <summary>
        /// 設定記錄行為
        /// </summary>
        /// <param name="options">用於<see cref="TEC.Core.Web.Logging.WebContextLoggerOptions"/>的參數設定</param>
        internal static void ConfigureBasicLogging(TEC.Core.Web.Logging.WebContextLoggerOptions options)
        {
            options.ActivityIdHeaderKey = "ActivityId";
            options.LoggingTriggerTypeHeaderKey = "LoggingTriggerType";
            options.TriggerReferenceIdHeaderKey = "TriggerReferenceId";
        }
        /// <summary>
        /// 設定 API 記錄行為
        /// </summary>
        /// <param name="options">用於<see cref="TEC.Core.Web.Logging.WebContextLoggerOptions"/>的參數設定</param>
        internal static void ConfigureApiLogging(TEC.Core.Web.Logging.WebContextLoggerOptions options)
        {
            options.RequestParsedAction = (loggerFactory, data) =>
            {
                loggerFactory.Log(new LogState()
                {
                    ActivityId = data.ActivityId,
                    ExtendProperties = data.BodyString,
                    Exception = null,
                    IPAddress = data.IPAddress,
                    LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                    LogLevel = LogLevel.Debug,
                    Message = "接收請求",
                    MessageType = LoggingMessageType.ReceivedClientRequest,
                    Resource = data.Path,
                    Scope = LoggingScope.API,
                    SystemScope = LoggingSystemScope.Local,
                    TriggerReferenceId = data.TriggerReferenceID
                });
            };
            options.ResponseParsedAction = (loggerFactory, data) =>
            {
                loggerFactory.Log(new LogState()
                {
                    ActivityId = data.ActivityId,
                    ExtendProperties = data.BodyString,
                    Exception = null,
                    IPAddress = data.IPAddress,
                    LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                    LogLevel = LogLevel.Debug,
                    Message = "回應請求",
                    MessageType = LoggingMessageType.ResponseDataToClient,
                    Resource = String.Empty,
                    Scope = LoggingScope.API,
                    SystemScope = LoggingSystemScope.Local,
                    TriggerReferenceId = data.TriggerReferenceID
                });
            };
        }
    }
}
