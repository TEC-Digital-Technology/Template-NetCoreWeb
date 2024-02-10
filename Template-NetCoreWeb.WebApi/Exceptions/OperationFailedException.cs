using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TEC.Core.Web.WebApi.Exceptions;
using Template_NetCoreWeb.Utils.Enums;

namespace Template_NetCoreWeb.WebApi.Exceptions;

/// <summary>
/// 當發生已知錯誤時，需要回傳至 API 結果的例外資訊
/// </summary>
public class OperationFailedException : ResultCodeException<ResultCodeSettingEnum>
{
    /// <summary>
    /// 使用指定的錯誤代碼，初始化 <see cref="ResultCodeException{TEnumType}"/> 類別的新執行個體
    /// </summary>
    /// <param name="resultCodeKey">解釋例外狀況原因的錯誤代碼列舉</param>
    /// <param name="data">關於此例外的相關參數，可由例外執行個體的<see cref="Data"/>屬性取得此資料</param>
    /// <param name="cultureInfo">與此例外相關的文化特性，若輸入<c>null</c>時則會使用<see cref="Thread.CurrentThread"/>的<see cref="Thread.CurrentUICulture"/></param>
    public OperationFailedException(ResultCodeSettingEnum resultCodeKey, Dictionary<object, object>? data = null, CultureInfo? cultureInfo = null)
        : base(resultCodeKey, data, cultureInfo)
    {
    }
    /// <summary>
    /// 使用指定的錯誤代碼和造成這個例外狀況原因的內部例外參考，初始化 <see cref="ResultCodeException{TEnumType}"/> 類別的新執行個體
    /// </summary>
    /// <param name="resultCodeKey">解釋例外狀況原因的錯誤代碼列舉</param>
    /// <param name="innerException">造成目前例外狀況的例外狀況，若未指定內部例外狀況，則為<c lang="C#">null</c> 參考 (Visual Basic 中為 Nothing)</param>
    /// <param name="data">關於此例外的相關參數，可由例外執行個體的<see cref="Data"/>屬性取得此資料</param>
    /// <param name="cultureInfo">與此例外相關的文化特性，若輸入<c>null</c>時則會使用<see cref="Thread.CurrentThread"/>的<see cref="Thread.CurrentUICulture"/></param>
    public OperationFailedException(ResultCodeSettingEnum resultCodeKey, Exception innerException, Dictionary<object, object>? data = null, CultureInfo? cultureInfo = null) 
        : base(resultCodeKey, innerException, data, cultureInfo)
    {
    }
}
