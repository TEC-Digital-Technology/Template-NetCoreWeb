using TEC.Core.Settings.Collections;
using Template_NetCoreWeb.Utils.Enums.Settings;

namespace Template_NetCoreWeb.WebMvc.Settings;

/// <summary>
/// 描述客戶端應用程式的設定檔集合
/// </summary>
public class ClientApplicationSettingCollection : SettingCollectionBase<ClientApplicationSettingEnum, object, string>
{
    /// <summary>
    /// 初始化描述客戶端應用程式的設定檔集合
    /// </summary>
    public ClientApplicationSettingCollection()
        : base(nameof(ClientApplicationSettingCollection))
    { }

    /// <summary>
    /// 取得設定檔集合的預設值
    /// </summary>
    /// <param name="key">設定檔列舉</param>
    /// <returns></returns>
    public override object GetDefaultValue(ClientApplicationSettingEnum key)
    {
        switch (key)
        {
            default:
                throw new NotImplementedException("不支援以此方法取得預設設定。");
        }
    }
}
