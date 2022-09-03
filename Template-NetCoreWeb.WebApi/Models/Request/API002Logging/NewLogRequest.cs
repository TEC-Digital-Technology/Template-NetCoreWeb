using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TEC.Core.DataAnnotations;
using TEC.Core.Web.Logging;
using Template_NetCoreWeb.Utils.Enums.Logging;

namespace Template_NetCoreWeb.WebApi.Models.Request.API002Logging;

/// <summary>
/// 新增紀錄的請求
/// </summary>
public class NewLogRequest
{
    /// <summary>
    /// 取得或設定活動 ID
    /// </summary>
    public Guid? ActivityId { get; set; }

    /// <summary>
    /// 紀錄等級
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? Level { get; set; }

    /// <summary>
    /// 取得或設定例外
    /// </summary>
    public object? Exception { get; set; }

    /// <summary>
    /// 嘗試將 <see cref="Exception"/> 轉換成已知的例外類型，若轉型失敗或 <see cref="Exception"/> 為 <c>null</c> 參考時，則回傳 <c>null</c> 參考
    /// </summary>
    /// <remarks>
    /// 取得 Exception object 被轉型成 Exception 的物件
    /// 因為本 Action 會由其他外部應用程式待入例外資料，為了避免反序列化失敗導致例外，才會需要此程式碼轉成已之例外後，方便對應到資料庫的欄位。
    /// </remarks>
    public Exception? DeserializedException
    {
        get
        {
            if (this.Exception == null)
            {
                return null;
            }
            try
            {
                return JsonConvert.DeserializeObject<Exception>(this.Exception!.ToString()!);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 取得或設定訊息內容
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? Message { get; set; }

    /// <summary>
    /// 取得或設定訊息類型
    /// </summary>
    [Required(ErrorMessage = "1000")]
    [MappedEnumValue(typeof(LoggingMessageType), ErrorMessage = "1001")]
    public LoggingMessageType? MessageType { get; set; }

    /// <summary>
    /// 取得或設定發生的範圍
    /// </summary>
    [Required(ErrorMessage = "1000")]
    [MappedEnumValue(typeof(LoggingScope), ErrorMessage = "1001")]
    public LoggingScope? Scope { get; set; }

    /// <summary>
    /// 取得或設定發生的系統
    /// </summary>
    [Required(ErrorMessage = "1000")]
    [MappedEnumValue(typeof(LoggingSystemScope), ErrorMessage = "1001")]
    public LoggingSystemScope? SystemScope { get; set; }

    /// <summary>
    /// 取得或設定資源
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? Resource { get; set; }

    /// <summary>
    /// 取得或設定電腦名稱
    /// </summary>
    [Required(ErrorMessage = "1000")]
    public string? HostName { get; set; }

    /// <summary>
    /// 取得或設定觸發者類型
    /// </summary>
    [Required(ErrorMessage = "1000")]
    [MappedEnumValue(typeof(LoggingTriggerType), ErrorMessage = "1001")]
    public LoggingTriggerType? TriggerType { get; set; }

    /// <summary>
    /// 取得或設定觸發者 ID，選填
    /// </summary>
    public Guid? TriggerReferenceId { get; set; }

    /// <summary>
    /// 取得或設定額外資訊，選填
    /// </summary>
    public object? ExtendedProperties { get; set; }

    /// <summary>
    /// 取得或設定 IP 位置，選填
    /// </summary>
    public string? IPAddress { get; set; }
}
