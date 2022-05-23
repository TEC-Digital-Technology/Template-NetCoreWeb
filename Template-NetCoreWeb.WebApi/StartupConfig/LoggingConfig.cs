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
        /// <param name="options">用於<see cref="TEC.Core.Web.Logging.Filters.LogRequestFilter"/>的參數設定</param>
        internal static void ConfigureLogging(TEC.Core.Web.Logging.Filters.LogRequestResponseFilterOptions options)
        {
            options.LogAspNetActionExecutingAction = (loggerFactory, data) =>
            {
                loggerFactory.Log(new LogState()
                {
                    ActivityId = data.ActivityId,
                    ExtendProperties = data.ActionArguments,
                    Exception = null,
                    IPAddress = data.IPAddress,
                    LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                    LogLevel = LogLevel.Information,
                    Message = $"執行請求",
                    MessageType = LoggingMessageType.MvcRequest,
                    Resource = $"{data.ControllerName}.{data.ActionName}",
                    Scope = LoggingScope.API,
                    SystemScope = LoggingSystemScope.Local,
                    TriggerReferenceId = data.TriggerReferenceID
                });
            };
            options.LogAspNetActionExecutedAction = (loggerFactory, data) =>
            {
                loggerFactory.Log(new LogState
                {
                    ActivityId = data.ActivityId,
                    ExtendProperties = data.ResponseBodyString,
                    Exception = null,
                    IPAddress = data.IPAddress,
                    LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                    LogLevel = LogLevel.Information,
                    Message = "回應請求",
                    MessageType = LoggingMessageType.MvcResponse,
                    Resource = $"{data.ControllerName}.{data.ActionName}",
                    Scope = LoggingScope.API,
                    SystemScope = LoggingSystemScope.Local,
                    TriggerReferenceId = data.TriggerReferenceID
                });
            };
            options.LogAspNetActionExeceptionAction = (loggerFactory, data) =>
            {
                loggerFactory.Log(new LogState
                {
                    ActivityId = data.ActivityId,
                    ExtendProperties = data.ResponseBodyString,
                    Exception = data.Exception,
                    IPAddress = data.IPAddress,
                    LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                    LogLevel = LogLevel.Error,
                    Message = $"執行發生錯誤",
                    MessageType = LoggingMessageType.MvcError,
                    Resource = data.Exception.GetType().FullName,
                    Scope = LoggingScope.API,
                    SystemScope = LoggingSystemScope.Local,
                    TriggerReferenceId = data.TriggerReferenceID
                });
            };
        }
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
        /// 設定記錄行為
        /// </summary>
        /// <param name="options">用於<see cref="TEC.Core.Web.Logging.WebContextLoggerOptions"/>的參數設定</param>
        internal static void ConfigureLogging(TEC.Core.Web.Logging.WebContextLoggerOptions options)
        {
            options.RequestParsedAction = (loggerFactory, data) =>
            {
                if (!data.HasMappedControllerAction.HasValue || !data.HasMappedControllerAction.Value)
                {
                    loggerFactory.Log(new LogState()
                    {
                        ActivityId = data.ActivityId,
                        ExtendProperties = data.BodyString,
                        Exception = null,
                        IPAddress = data.IPAddress,
                        LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                        LogLevel = LogLevel.Information,
                        Message = "接收請求",
                        MessageType = LoggingMessageType.ReceivedClientRequest,
                        Resource = data.Path,
                        Scope = LoggingScope.API,
                        SystemScope = LoggingSystemScope.Local,
                        TriggerReferenceId = data.TriggerReferenceID
                    });
                }
            };
            options.ResponseParsedAction = (loggerFactory, data) =>
            {
                if (!data.HasMappedControllerAction.HasValue || !data.HasMappedControllerAction.Value)
                {
                    loggerFactory.Log(new LogState()
                    {
                        ActivityId = data.ActivityId,
                        ExtendProperties = data.BodyString,
                        Exception = null,
                        IPAddress = data.IPAddress,
                        LoggingTriggerType = data.LoggingTriggerType ?? LoggingTriggerType.User,
                        LogLevel = LogLevel.Information,
                        Message = "回應請求",
                        MessageType = LoggingMessageType.ResponseDataToClient,
                        Resource = data.Path,
                        Scope = LoggingScope.API,
                        SystemScope = LoggingSystemScope.Local,
                        TriggerReferenceId = data.TriggerReferenceID
                    });
                }
            };
        }
    }
}
