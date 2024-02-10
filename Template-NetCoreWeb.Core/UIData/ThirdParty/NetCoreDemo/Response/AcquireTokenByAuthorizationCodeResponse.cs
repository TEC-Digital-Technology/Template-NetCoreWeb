using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Core.UIData.ThirdParty.NetCoreDemo.Response;

/// <summary>
/// 由 Authorization Code 取得認證結果的回應
/// </summary>
public class AcquireTokenByAuthorizationCodeResponse
{
    /// <summary>
    /// 設定代表該帳號的 HomeAccountId
    /// </summary>
    public string? HomeAccountId { set; get; }
    /// <summary>
    /// 設定或取得 Access Token 類型
    /// </summary>
    public string? AccessTokenType { set; get; }
    /// <summary>
    /// 設定或取得 Access Token
    /// </summary>
    public string? AccessToken { set; get; }
    /// <summary>
    /// 設定或取得 Access Token 的過期時間
    /// </summary>
    public DateTimeOffset ExpiresOn { set; get; }
    /// <summary>
    /// 設定或取得核發 AccessToken 目標的識別，若為 null 時代表無資料
    /// </summary>
    public string? TenantId { set; get; }
    /// <summary>
    /// 設定或取得 ID Token
    /// </summary>
    public string? IdToken { set; get; }
    /// <summary>
    /// 設定或取得授權單位
    /// </summary>
    public string? Authority { set; get; }
}
