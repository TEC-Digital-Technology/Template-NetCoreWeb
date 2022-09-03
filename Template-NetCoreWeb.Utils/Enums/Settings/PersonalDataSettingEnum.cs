using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Utils.Enums.Settings;

/// <summary>
/// 屬於目前工作階段的使用者設定檔列舉
/// </summary>
public enum PersonalDataSettingEnum : int
{
    /// <summary>
    /// 當使用者重新登入後，需要重新導向的相對位址
    /// </summary>
    RedirectRelativePathWhenLoggedIn = 1,
    /// <summary>
    /// 當使用者重新登出後，需要重新導向的相對位址
    /// </summary>
    RedirectRelativePathWhenLoggedOut = 2
}
