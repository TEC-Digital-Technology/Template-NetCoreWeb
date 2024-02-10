using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Core.Infos.Account;

/// <summary>
/// 帳號資料
/// </summary>
public class AccountInfo
{
    /// <summary>
    /// 設定或取得帳號 ID
    /// </summary>
    public Guid AccountId { get; set; }
    /// <summary>
    /// 設定或取得帳號名稱
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// 設定或取得密碼
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 設定或取得資料建立日期
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }
    /// <summary>
    /// 設定或取得資料最後更新時間
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }
}
