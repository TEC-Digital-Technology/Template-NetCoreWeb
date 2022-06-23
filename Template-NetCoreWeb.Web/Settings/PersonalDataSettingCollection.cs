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
        /// <param name="session">用於存取個人資料設定檔的 Session 物件</param>
        public PersonalDataSettingCollection(ISession session)
            : base(nameof(PersonalDataSettingCollection))
        {
            this.Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <summary>
        /// 取得設定檔集合的預設值
        /// </summary>
        /// <param name="key">設定檔列舉</param>
        /// <returns></returns>
        public override string? GetDefaultValue(PersonalDataSettingEnum key)
        {
            return this.Session.GetString(this.GetSessionKey(key));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalDataSetting"></param>
        /// <returns></returns>
        public string GetSessionKey(PersonalDataSettingEnum personalDataSetting)
        {
            return $"{this.Id}_{personalDataSetting.ToString()}";
        }
        /// <summary>
        /// 取得用於存取個人資料設定檔的 Session 物件
        /// </summary>
        private ISession Session { get; }
    }
}
