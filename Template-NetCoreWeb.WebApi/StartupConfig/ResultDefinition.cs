using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TEC.Core.Settings.Collections;
using TEC.Core.Web.WebApi.Messaging;
using TEC.Core.Web.WebApi.Settings;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.StartupConfig;

/// <summary>
/// API 架構的回應訊息定義
/// </summary>
public class ResultDefinition : IResultDefinition<ResultCodeSettingEnum>
{
    /// <summary>
    /// 初始化 API 架構的回應訊息定義
    /// </summary>
    /// <param name="memoryCache">用於儲存訊息的記憶體快取</param>
    /// <exception cref="ArgumentNullException">當 <paramref name="memoryCache"/> 為 <c>null</c> 時擲出</exception>
    public ResultDefinition(MemoryCache memoryCache)
    {
        this.MemoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }
    /// <inheritdoc/>
    public Func<CachedValueConfig<ResultCodeSettingCollection<ResultCodeSettingEnum>>> GetSpecificCultureResultCodeCachedValueConfig(string cultureKey)
    {
#warning 本範例寫死不同區域皆回傳相同語系的訊息，實際上應該要針對不同語系回傳該語系的資料
        return new Func<CachedValueConfig<ResultCodeSettingCollection<ResultCodeSettingEnum>>>(() =>
        {
            ResultCodeSettingCollection<ResultCodeSettingEnum> resultCodeSettingCollection = new ResultCodeSettingCollection<ResultCodeSettingEnum>(cultureKey);
            resultCodeSettingCollection[ResultCodeSettingEnum.FieldRequired] = "欄位為必填";
            resultCodeSettingCollection[ResultCodeSettingEnum.InvalidArgument] = "參數驗證錯誤";
            resultCodeSettingCollection[ResultCodeSettingEnum.InvalidRange] = "無效的參數區間";
            resultCodeSettingCollection[ResultCodeSettingEnum.ItemRequired] = "必須要有至少一個元素";
            resultCodeSettingCollection[ResultCodeSettingEnum.AuthenticationFailed] = "認證失敗，相關訊息：{0}";
            resultCodeSettingCollection[ResultCodeSettingEnum.AuthenticationRequired] = "必須輸入認證資訊";
            resultCodeSettingCollection[ResultCodeSettingEnum.AuthorizationFailed] = "授權發生錯誤，詳細資料請參考相關訊息：{0}";
            resultCodeSettingCollection[ResultCodeSettingEnum.SilentTokenAcquisitionFailed] = "無法背景更新 Token";
            resultCodeSettingCollection[ResultCodeSettingEnum.Success] = "成功";
            resultCodeSettingCollection[ResultCodeSettingEnum.SystemError] = "系統發生未預期的錯誤";
            resultCodeSettingCollection[ResultCodeSettingEnum.UnrecognizedLanguage] = "無效的語系";
            return new CachedValueConfig<ResultCodeSettingCollection<ResultCodeSettingEnum>>(resultCodeSettingCollection, new MemoryCacheEntryOptions()
            {
                //30 分鐘清除一次
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        });
    }
    /// <inheritdoc/>
    public ResultCodeSettingEnum DefaultResultCode => ResultCodeSettingEnum.Success;
    /// <inheritdoc/>
    public ResultCodeSettingEnum DefaultExceptionResultCode => ResultCodeSettingEnum.SystemError;
    /// <inheritdoc/>
    public MemoryCache MemoryCache { private set; get; }
    /// <inheritdoc/>
    public List<CultureInfo> SupportedCultureInfoList => new List<CultureInfo>()
    {
        new CultureInfo("zh")
    };
    /// <inheritdoc/>
    public CultureInfo DefaultCultureInfo => new CultureInfo("zh");
    /// <inheritdoc/>
    public ResultCodeSettingEnum InvalidArgumentResultCode => ResultCodeSettingEnum.InvalidArgument;
}
