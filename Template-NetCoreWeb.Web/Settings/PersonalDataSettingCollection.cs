using Newtonsoft.Json;
using TEC.Core.Settings.Collections;
using Template_NetCoreWeb.Utils.Enums.Settings;

namespace Template_NetCoreWeb.WebMvc.Settings;

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
    /// 將指定的物件序列化為 Json ，並儲存至 Session 中
    /// </summary>
    /// <typeparam name="T">要序列化的型別</typeparam>
    /// <param name="personalDataSetting">索引鍵</param>
    /// <param name="value">要序列化的值</param>
    /// <param name="converters">參與序列化的值轉換器</param>
    public void SetValueAsJson<T>(PersonalDataSettingEnum personalDataSetting, T value, params JsonConverter[] converters)
    {
        JsonSerializerSettings jsonSerializerSettings = new();
        jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
        converters.ToList().ForEach(t => jsonSerializerSettings.Converters.Add(t));
        this[personalDataSetting] = JsonConvert.SerializeObject(value, jsonSerializerSettings);
    }

    /// <summary>
    /// 從 Session 中取得指定索引鍵的字串，透過 Json 反序列化為指定型別的物件
    /// </summary>
    /// <typeparam name="T">要反序列化的型別</typeparam>
    /// <param name="personalDataSetting">索引鍵</param>
    /// <param name="converters">參與反序列化的值轉換器</param>
    public T? GetValueFromJson<T>(PersonalDataSettingEnum personalDataSetting, params JsonConverter[] converters)
    {
        JsonSerializerSettings jsonSerializerSettings = new();
        jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
        converters.ToList().ForEach(t => jsonSerializerSettings.Converters.Add(t));
        string? value = this[personalDataSetting];
        if (String.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings)!;
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
