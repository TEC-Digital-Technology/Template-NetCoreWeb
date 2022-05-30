using TEC.Core.Logging.Http;
using TEC.Core.Logging.Soap;
using TEC.Core.Logging.UIData;
using TEC.Core.Web.Logging;
using Template_NetCoreWeb.Core.Logging;
using Template_NetCoreWeb.Core.UIData;
using Template_NetCoreWeb.Utils.Enums.Logging;
using Template_NetCoreWeb.Utils.Logging;

namespace Template_NetCoreWeb.WebApi.StartupConfig
{
    /// <summary>
    /// 處理記錄檔的類別
    /// </summary>
    internal static class LoggingConfig
    {
        #region Web API
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
        #endregion
        #region HTTP Client Handler
        internal static void LoggingHttpClientHandler_LogHttpRequest(object? sender, LogHttpRequestEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            TEC.Core.Logging.Http.HttpRequestEventData httpRequestEventData = e.Request.ToEventData();
            requiredLoggerFactory.LoggerFactory.Log(new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = httpRequestEventData.Content,
                Exception = null,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = $"送出 {e.SystemScope.ToString()} 請求({httpRequestEventData.Method})",
                MessageType = LoggingMessageType.ResponseDataToClient,
                Resource = httpRequestEventData.RequestUri,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = null
            });
        }
        internal static void LoggingHttpClientHandler_LogHttpResponse(object? sender, LogHttpResponseEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            TEC.Core.Logging.Http.HttpResponseEventData httpResponseEventData = e.Response.ToEventData();
            requiredLoggerFactory.LoggerFactory.Log(new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = httpResponseEventData.Content,
                Exception = null,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = $"收到 {e.SystemScope.ToString()} 回應",
                MessageType = LoggingMessageType.ResponseDataToClient,
                Resource = e.RequestUri.AbsoluteUri,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = null
            });
        }
        internal static void LoggingHttpClientHandler_LogHttpError(object? sender, LogHttpErrorEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            TEC.Core.Logging.Http.HttpRequestEventData httpRequestEventData = e.Request.ToEventData();
            requiredLoggerFactory.LoggerFactory.Log(new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = httpRequestEventData.Content,
                Exception = e.Exception,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Error,
                Message = $"處理 {e.SystemScope.ToString()} 請求時發生錯誤",
                MessageType = LoggingMessageType.ResponseDataToClient,
                Resource = httpRequestEventData.RequestUri,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = null
            });
        }
        #endregion
        #region UIData
        public static void UIData_LoggingAction(object sender, DataModifiedEventArgs e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            var logState = new LogState()
            {
                ActivityId = e.ActivityId!.Value,
                ExtendProperties = e.ModifiedItems,
                Exception = null,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = null,
                MessageType = 0,
                Resource = $"{e.UIDataType.Name}/{e.MethodName}",
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = null
            };
            switch (e.DataModifyType)
            {
                case DataModifyType.Add:
                    logState.Message = "新增資料";
                    logState.MessageType = LoggingMessageType.UIDataAddData;
                    break;
                case DataModifyType.Modify:
                    logState.Message = "修改資料";
                    logState.MessageType = LoggingMessageType.UIDataModifyData;
                    break;
                case DataModifyType.Delete:
                    logState.Message = "刪除資料";
                    logState.MessageType = LoggingMessageType.UIDataDeleteData;
                    break;
            }
            requiredLoggerFactory.LoggerFactory.Log(logState);
        }
        #endregion
        #region SOAP
        public static void SoapLoggingManager_LogSoapResponse(object sender, LogSoapResponseEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            SoapEventData responseSoapData = e.Response.ToEventData();
            var logState = new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = responseSoapData.Content,
                Exception = null,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = $"接收 SOAP-{e.OperationName} 回應",
                MessageType = LoggingMessageType.ThirdPartySoapResponse,
                Resource = e.OperationName,
                Scope = LoggingScope.API,
                SystemScope = e.SystemScope,
                TriggerReferenceId = null
            };
            requiredLoggerFactory.LoggerFactory.Log(logState);
        }
        public static void SoapLoggingManager_LogSoapRequest(object sender, LogSoapRequestEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            SoapEventData requestSoapData = e.Request.ToEventData();
            var logState = new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = requestSoapData.Content,
                Exception = null,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = $"送出 SOAP-{e.OperationName} 請求",
                MessageType = LoggingMessageType.ThirdPartySoapRequest,
                Resource = e.OperationName,
                Scope = LoggingScope.API,
                SystemScope = e.SystemScope,
                TriggerReferenceId = null
            };
            requiredLoggerFactory.LoggerFactory.Log(logState);
        }
        public static void SoapLoggingManager_LogSoapError(object sender, LogSoapErrorEventArgs<LoggingSystemScope> e)
        {
            if (sender is not IRequireLoggerFactory requiredLoggerFactory)
            {
                throw new ArgumentException($"觸發事件的物件型別必須實作 {typeof(IRequireLoggerFactory).FullName} 介面", nameof(sender));
            }
            SoapEventData errorSoapData = e.Request.ToEventData();
            var logState = new LogState()
            {
                ActivityId = e.RequestId,
                ExtendProperties = errorSoapData.Content,
                Exception = e.Exception,
                IPAddress = String.Empty,
                LoggingTriggerType = LoggingTriggerType.System,
                LogLevel = LogLevel.Error,
                Message = $"送出 SOAP-{e.OperationName} 請求發生錯誤",
                MessageType = LoggingMessageType.ThirdPartySoapError,
                Resource = e.OperationName,
                Scope = LoggingScope.API,
                SystemScope = e.SystemScope,
                TriggerReferenceId = null
            };
            requiredLoggerFactory.LoggerFactory.Log(logState);
        }
        #endregion
    }
}
