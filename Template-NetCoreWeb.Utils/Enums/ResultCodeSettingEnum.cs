using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.ComponentModel;

namespace Template_NetCoreWeb.Utils.Enums
{
    /// <summary>
    /// 回傳訊息列舉
    /// </summary>
    [DescriptiveEnumEnforcement(DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
    public enum ResultCodeSettingEnum
    {
        #region 系統(0000-0FFF)
        /// <summary>
        /// 成功
        /// </summary>
        [EnumDescription("0000")]
        Success,
        /// <summary>
        /// 參數驗證錯誤
        /// </summary>
        [EnumDescription("0001")]
        InvalidArgument,
        /// <summary>
        /// 無法辨識的語系
        /// </summary>
        [EnumDescription("0002")]
        UnrecognizedLanguage,
        #endregion 系統(0000-0FFF)
        #region 驗證 (1000-1FFF)
        /// <summary>
        /// 欄位為必填
        /// </summary>
        [EnumDescription("1000")]
        FieldRequired,
        /// <summary>
        /// 值區間不符規定
        /// </summary>
        [EnumDescription("1001")]
        InvalidRange,
        /// <summary>
        /// 值區間不符規定
        /// </summary>
        [EnumDescription("1002")]
        ItemRequired,
        #endregion
        /// <summary>
        /// 系統錯誤
        /// </summary>
        [EnumDescription("FFFF")]
        SystemError
    }
}
