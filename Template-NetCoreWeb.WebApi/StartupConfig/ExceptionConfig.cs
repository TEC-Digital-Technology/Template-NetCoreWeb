using TEC.Core;
using TEC.Core.Web.WebApi.Exceptions;
using TEC.Core.Web.WebApi.Messaging;
using TEC.Core.Web.WebApi.Response;
using TEC.Internal.Web.AccountService.Enums;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.StartupConfig;

/// <summary>
/// 負責處理例外的設定
/// </summary>
internal static class ExceptionConfig
{
    /// <summary>
    /// 回應資訊處理
    /// </summary>
    internal static void ResultFormat(ExceptionResponse<ResultCodeSettingEnum> exceptionResponse, ResultFormattedEventArgs<ResultCodeSettingEnum> formattedArgs)
    {
        if (formattedArgs.ResultFormatType != TEC.Core.Web.WebApi.Messaging.ResultFormatType.MessageOnly)
        {
            return;
        }
        List<string> messageList = new List<string>();
        switch (exceptionResponse.ResultCodeKey)
        {
            case ResultCodeSettingEnum.AuthenticationFailed:
            case ResultCodeSettingEnum.AuthorizationFailed:
                //處理授權失敗的訊息
                messageList.Add(String.Join(Environment.NewLine, exceptionResponse.ExceptionObject.FromHierarchy(t => t.InnerException, t => t.InnerException!, t => t.InnerException != null).Select(t => t!.Message)));
                break;
            default:
                return;
        }
        formattedArgs.FormattedString = String.Format(formattedArgs.FormattedString, messageList.ToArray());
    }
}