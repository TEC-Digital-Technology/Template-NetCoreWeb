using TEC.Core.Settings.Collections;
using Template_NetCoreWeb.Utils.Enums.Settings;

namespace Template_NetCoreWeb.WebMvc.Settings
{
    /// <summary>
    /// 屬於目前工作階段的使用者設定檔集合
    /// </summary>
    public class PersonalDataSettingCollection : SettingCollectionBase<PersonalDataSettingEnum, string?, string>
    {
        /// <summary>
        /// 初始化屬於目前工作階段的使用者設定檔集合
        /// </summary>
        public PersonalDataSettingCollection()
            : base(nameof(PersonalDataSettingCollection))
        { }

        /// <summary>
        /// 取得設定檔集合的預設值
        /// </summary>
        /// <param name="key">設定檔列舉</param>
        /// <returns></returns>
        public override string? GetDefaultValue(PersonalDataSettingEnum key)
        {
            switch (key)
            {
                case PersonalDataSettingEnum.RedirectRelativePathWhenLoggedIn:
                case PersonalDataSettingEnum.RedirectRelativePathWhenLoggedOut:
                    return null;
                default:
                    throw new NotImplementedException("不支援以此方法取得預設設定。");
            }
        }
    }
}
