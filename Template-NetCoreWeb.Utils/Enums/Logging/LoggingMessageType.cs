using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.ComponentModel;

namespace Template_NetCoreWeb.Utils.Enums.Logging
{
    /// <summary>
    /// 紀錄的訊息類型
    /// </summary>
    [DescriptiveEnumEnforcement(EnforcementType = DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
    public enum LoggingMessageType : short
    {
        /// <summary>
        /// 一般訊息
        /// </summary>
        [EnumDescription("一般訊息")]
        General = 0x0000,
        #region 第三方 HTTP (0x01XX)
        /// <summary>
        /// 第三方HTTP請求
        /// </summary>
        [EnumDescription("第三方 HTTP 請求")]
        ThirdPartyHttpRequest = 0x0100,
        /// <summary>
        /// 第三方HTTP回應
        /// </summary>
        [EnumDescription("第三方 HTTP 回應")]
        ThirdPartyHttpResponse = 0x0101,
        /// <summary>
        /// 第三方HTTP錯誤
        /// </summary>
        [EnumDescription("第三方 HTTP 錯誤")]
        ThirdPartyHttpError = 0x0102,
        #endregion
        #region 第三方 SOAP (0x020X)
        /// <summary>
        /// 第三方SOAP請求
        /// </summary>
        [EnumDescription("第三方 SOAP 請求")]
        ThirdPartySoapRequest = 0x0200,
        /// <summary>
        /// 第三方SOAP回應
        /// </summary>
        [EnumDescription("第三方 SOAP 回應")]
        ThirdPartySoapResponse = 0x0201,
        /// <summary>
        /// 第三方SOAP錯誤
        /// </summary>
        [EnumDescription("第三方 SOAP 錯誤")]
        ThirdPartySoapError = 0x0202,
        #endregion
        #region 伺服器本身的請求/回應 (0x030X)
        /// <summary>
        /// 由客戶端接收請求
        /// </summary>
        [EnumDescription("由客戶端接收請求")]
        ReceivedClientRequest = 0x0300,
        /// <summary>
        /// 回應資料至客戶端
        /// </summary>
        [EnumDescription("回應資料至客戶端")]
        ResponseDataToClient = 0x0301,
        /// <summary>
        /// MVC 模組解析的請求
        /// </summary>
        [EnumDescription("MVC 模組解析的請求")]
        MvcRequest = 0x0302,
        /// <summary>
        /// MVC 模組回應的內容
        /// </summary>
        [EnumDescription("MVC 模組解析的請求")]
        MvcResponse = 0x0303,
        /// <summary>
        /// MVC 模組所記錄的錯誤資訊
        /// </summary>
        [EnumDescription("MVC 模組所記錄的錯誤資訊")]
        MvcError = 0x0304,
        #endregion
        #region 資料 (0x040X)
        /// <summary>
        /// 使用 UIData 新增資料
        /// </summary>
        [EnumDescription("UIData 新增資料")]
        UIDataAddData = 0x0400,
        /// <summary>
        /// 使用 UIData 修改資料
        /// </summary>
        [EnumDescription("UIData 修改資料")]
        UIDataModifyData = 0x0401,
        /// <summary>
        /// 使用 UIData 刪除資料
        /// </summary>
        [EnumDescription("UIData 刪除資料")]
        UIDataDeleteData = 0x0402,
        #endregion
    }
}
