using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.ComponentModel;

namespace Template_NetCoreWeb.Utils.Enums.Logging;

/// <summary>
/// 紀錄所發生的位置
/// </summary>
[DescriptiveEnumEnforcement(EnforcementType = DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
public enum LoggingScope : byte
{
    /// <summary>
    /// API
    /// </summary>
    [EnumDescription("API")]
    API = 1,
    /// <summary>
    /// 前台
    /// </summary>
    [EnumDescription("前台")]
    FrontEnd = 2,
    /// <summary>
    /// 後台
    /// </summary>
    [EnumDescription("後台")]
    BackEnd = 3,
    /// <summary>
    /// 排程服務
    /// </summary>
    [EnumDescription("排程服務")]
    SchedulerService = 4,
}
