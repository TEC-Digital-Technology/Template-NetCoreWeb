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

namespace Template_NetCoreWeb.StartupConfig
{
    public class ResultDefinition : IResultDefinition<ResultCodeSettingEnum>
    {
        public ResultDefinition(MemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }
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
        public ResultCodeSettingEnum DefaultResultCode => ResultCodeSettingEnum.Success;
        public ResultCodeSettingEnum DefaultExceptionResultCode => ResultCodeSettingEnum.SystemError;
        public MemoryCache MemoryCache { private set; get; }
        public List<CultureInfo> SupportedCultureInfoList => new List<CultureInfo>()
        {
            new CultureInfo("zh")
        };
        public CultureInfo DefaultCultureInfo => new CultureInfo("zh");
        public ResultCodeSettingEnum InvalidArgumentResultCode => ResultCodeSettingEnum.InvalidArgument;
    }
}
