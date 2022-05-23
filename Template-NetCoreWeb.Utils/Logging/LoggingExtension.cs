using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Web;
using TEC.Core.Web.Logging;
using Template_NetCoreWeb.Utils.Enums.Logging;
using Template_NetCoreWeb.Utils.Logging;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// 處理紀錄檔的靜態擴充類別
    /// </summary>
    public static class LoggingExtension
    {
        /// <summary>
        /// 紀錄偵錯資料
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="logState"></param>
        public static void Log(this ILoggerFactory loggerFactory, LogState logState)
        {
            loggerFactory.CreateLogger(logState.SystemScope.ToString()).Log(logState.LogLevel,
                new EventId((int)logState.MessageType, logState.MessageType.ToString()), logState, logState.Exception, (state, exception) => state.Message);
        }
        /// <summary>
        /// 記錄一筆偵錯訊息
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源</param>
        public static void LogDebug(this ILoggerFactory loggerFactory, string message,
            Guid activityId, Guid? triggerReferenceId, Dictionary<string, object>? extendProperties = null,
            LoggingMessageType loggingMessageType = LoggingMessageType.General,
            LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Debug,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }
        /// <summary>
        /// 記錄一筆偵錯訊息，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogDebug(this ILoggerFactory loggerFactory, string message,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System,
            string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Debug,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }
        /// <summary>
        /// 記錄一筆偵錯訊息，並以 TEC 紀錄模組內的資來設定其他資訊。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogDebug(this ILoggerFactory loggerFactory, string message,
            LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = HttpContextProvider.CurrentActivityId!.Value,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = HttpContextProvider.CurrentIPAddress,
                LoggingTriggerType = HttpContextProvider.CurrentTriggerType! ?? LoggingTriggerType.System,
                LogLevel = LogLevel.Debug,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }
        /// <summary>
        /// 記錄一筆錯誤
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogError(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Error,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }
        /// <summary>
        /// 記錄一筆錯誤，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogError(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Error,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆偵錯訊息，並以 TEC 紀錄模組內的資來設定其他資訊。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="exception">例外</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogError(this ILoggerFactory loggerFactory, string message, Exception exception, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = HttpContextProvider.CurrentActivityId!.Value,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = HttpContextProvider.CurrentIPAddress,
                LoggingTriggerType = HttpContextProvider.CurrentTriggerType! ?? LoggingTriggerType.System,
                LogLevel = LogLevel.Error,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆重大錯誤
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogFatal(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Critical,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }

        /// <summary>
        /// 記錄一筆重大錯誤，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogFatal(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Critical,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆重大錯誤，並以 TEC 紀錄模組內的資來設定其他資訊。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="exception">例外</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogFatal(this ILoggerFactory loggerFactory, string message, Exception exception,
            LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = HttpContextProvider.CurrentActivityId!.Value,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = HttpContextProvider.CurrentIPAddress,
                LoggingTriggerType = HttpContextProvider.CurrentTriggerType! ?? LoggingTriggerType.System,
                LogLevel = LogLevel.Critical,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆資訊
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogInfo(this ILoggerFactory loggerFactory, string message,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Information,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }

        /// <summary>
        /// 記錄一筆資訊，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogInfo(this ILoggerFactory loggerFactory, string message,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Information,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆資訊
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogInfo(this ILoggerFactory loggerFactory, string message, Exception? exception,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Information,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }

        /// <summary>
        /// 記錄一筆資訊，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogInfo(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Information,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆資訊，並以 TEC 紀錄模組內的資來設定其他資訊。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="exception">例外</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogInfo(this ILoggerFactory loggerFactory, string message,
            LoggingMessageType loggingMessageType = LoggingMessageType.General, Exception? exception = null,
            Dictionary<string, object>? extendProperties = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = HttpContextProvider.CurrentActivityId!.Value,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = HttpContextProvider.CurrentIPAddress,
                LoggingTriggerType = HttpContextProvider.CurrentTriggerType! ?? LoggingTriggerType.System,
                LogLevel = LogLevel.Information,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆警告
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogWarn(this ILoggerFactory loggerFactory, string message,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Warning,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }

        /// <summary>
        /// 記錄一筆警告，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogWarn(this ILoggerFactory loggerFactory, string message,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = null,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Warning,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆警告
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="triggerReferenceId">觸發的物件 ID，可為 null</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogWarn(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, Guid? triggerReferenceId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Warning,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = triggerReferenceId
            });
        }

        /// <summary>
        /// 記錄一筆警告，並以 TEC 紀錄模組內的資來設定觸發者 ID。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="activityId">事件 ID</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="triggerType">觸發類型</param>
        /// <param name="ipAddress">觸發者的 IP 位置，可為 null</param>
        /// <param name="exception">例外</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogWarn(this ILoggerFactory loggerFactory, string message, Exception exception,
            Guid activityId, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Dictionary<string, object>? extendProperties = null, LoggingTriggerType triggerType = LoggingTriggerType.System, string? ipAddress = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = activityId,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = ipAddress,
                LoggingTriggerType = triggerType,
                LogLevel = LogLevel.Warning,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }

        /// <summary>
        /// 記錄一筆警告，並以 TEC 紀錄模組內的資來設定其他資訊。
        /// </summary>
        /// <param name="loggerFactory">記錄管理器</param>
        /// <param name="loggingMessageType">訊息內容類型</param>
        /// <param name="message">訊息內容</param>
        /// <param name="exception">例外</param>
        /// <param name="extendProperties">額外屬性物件</param>
        /// <param name="resource">使用的資源，預設為呼叫端的 CallerMemberName</param>
        public static void LogWarn(this ILoggerFactory loggerFactory, string message, LoggingMessageType loggingMessageType = LoggingMessageType.General,
            Exception? exception = null, Dictionary<string, object>? extendProperties = null, [CallerMemberName] string resource = "")
        {
            loggerFactory.Log(new LogState()
            {
                ActivityId = HttpContextProvider.CurrentActivityId!.Value,
                ExtendProperties = extendProperties,
                Exception = exception,
                IPAddress = HttpContextProvider.CurrentIPAddress,
                LoggingTriggerType = HttpContextProvider.CurrentTriggerType! ?? LoggingTriggerType.System,
                LogLevel = LogLevel.Warning,
                Message = message,
                MessageType = loggingMessageType,
                Resource = resource,
                Scope = LoggingScope.API,
                SystemScope = LoggingSystemScope.Local,
                TriggerReferenceId = HttpContextProvider.CurrentTriggerReferenceID
            });
        }
    }
}
