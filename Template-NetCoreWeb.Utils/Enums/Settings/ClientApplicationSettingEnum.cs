using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Settings.Providers;

namespace Template_NetCoreWeb.Utils.Enums.Settings;

/// <summary>
/// 描述客戶端應用程式的設定檔列舉
/// </summary>
public enum ClientApplicationSettingEnum : int
{
    /// <summary>
    /// 授權機構位置，屬於<see cref="System.String"/>
    /// </summary>
    [ConfigurationSettingProviderKeyTypeMapping("TEC:Adfs:Authority", typeof(string))]
    Authority,
    /// <summary>
    /// 客戶端 ID，屬於<see cref="System.String"/>
    /// </summary>
    [ConfigurationSettingProviderKeyTypeMapping("TEC:Adfs:ClientId", typeof(string))]
    ClientId,
    /// <summary>
    /// 資源 ID，屬於<see cref="System.String"/>
    /// </summary>
    [ConfigurationSettingProviderKeyTypeMapping("TEC:Adfs:GraphResourceId", typeof(string))]
    GraphResourceId,
    /// <summary>
    /// 客戶端密鑰，屬於<see cref="System.String"/>
    /// </summary>
    [ConfigurationSettingProviderKeyTypeMapping("TEC:Adfs:Secret", typeof(string))]
    ClientSecret,
    /// <summary>
    /// 前台網址根目錄，屬於<see cref="System.Uri"/>
    /// </summary>
    [ConfigurationSettingProviderKeyTypeMapping("TEC:Adfs:FrontendBaseUrl", typeof(Uri))]
    FrontendBaseUrl,
}
