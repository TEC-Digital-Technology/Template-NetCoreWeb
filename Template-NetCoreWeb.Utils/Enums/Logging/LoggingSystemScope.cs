using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.ComponentModel;

namespace Template_NetCoreWeb.Utils.Enums.Logging;

/// <summary>
/// 記錄檔所發生的系統
/// </summary>
[DescriptiveEnumEnforcement(EnforcementType = DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
public enum LoggingSystemScope : byte
{
    /// <summary>
    /// 本地系統，非第三方系統都屬於此種系統
    /// </summary>
    [EnumDescription("本地")]
    Local = 1,
    /// <summary>
    /// 雲騰
    /// </summary>
    [EnumDescription("TEC")]
    TEC = 2,
    /// <summary>
    /// DEMO 用的 SOAP 站台
    /// </summary>
    [EnumDescription("Demo SOAP")]
    DemoSoap = 3,
    /// <summary>
    /// 範例 API (Template-NetCoreWeb.WebApi)
    /// </summary>
    [EnumDescription("Template-NetCoreWeb.WebApi")]
    NetCoreDemo = 4
}
