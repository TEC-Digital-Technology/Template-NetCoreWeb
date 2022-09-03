using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Logging.UIData;
using Template_NetCoreWeb.Core.Infos.Account;

namespace Template_NetCoreWeb.Core.UIData;

/// <summary>
/// 處理帳號資料的商業邏輯類別
/// </summary>
public class AccountUIData : LoggerFactoryLoggableUIDataBase,
    IAddableUIData<AccountInfo>,
    ISelectableUIData<AccountInfo, Guid>,
    IEditableUIData<AccountInfo>,
    IDeletableUIData<Guid>
{
    static AccountUIData()
    {
        AccountUIData.SimulatedDataSource = new TEC.Core.Collections.ThreadSafeObservableCollection<AccountInfo>();
    }

    /// <summary>
    /// 初始化處理帳號資料的商業邏輯物件
    /// </summary>
    /// <param name="loggableUIDataOptions">初始化相關的選項</param>
    /// <param name="loggerFactory">關於此物件所需使用的 <see cref="ILoggerFactory"/></param>
    public AccountUIData(LoggableUIDataOptions loggableUIDataOptions, ILoggerFactory? loggerFactory)
        : base(loggableUIDataOptions, loggerFactory)
    {
    }

    /// <summary>
    /// 新增帳號
    /// </summary>
    /// <param name="username">帳號</param>
    /// <param name="password">密碼</param>
    /// <returns></returns>
    public Guid AddAccount(string username, string password)
    {
        Guid accountId = Guid.NewGuid();
        AccountInfo accountInfo = new AccountInfo()
        {
            AccountId = accountId,
            CreatedDate = DateTimeOffset.Now,
            UpdatedDate = null,
            Username = username,
            Password = password
        };
        base.Add(this, accountInfo);
        return accountId;
    }


    #region Implement Members
    void IAddableUIData<AccountInfo>.AddData(AccountInfo data)
    {
        AccountInfo? target = AccountUIData.SimulatedDataSource.SingleOrDefault(t => t.AccountId == data.AccountId);
        if (target != null)
        {
            throw new ArgumentException($"Primary Key {data.AccountId} already exist.", nameof(data));
        }
        AccountUIData.SimulatedDataSource.Add(data);
    }

    void IDeletableUIData<Guid>.DeleteData(Guid key)
    {
        AccountInfo? target = AccountUIData.SimulatedDataSource.SingleOrDefault(t => t.AccountId == key);
        if (target != null)
        {
            AccountUIData.SimulatedDataSource.Remove(target!);
        }
    }

    void IEditableUIData<AccountInfo>.EditData(AccountInfo data)
    {
        AccountInfo? target = AccountUIData.SimulatedDataSource.SingleOrDefault(t => t.AccountId == data.AccountId);
        if (target != null)
        {
            target.CreatedDate = data.CreatedDate;
            target.Username = data.Username;
            target.Password = data.Password;
            target.UpdatedDate = data.UpdatedDate;
        }
    }

    AccountInfo ISelectableUIData<AccountInfo, Guid>.GetData(Guid key)
    {
#pragma warning disable CS8603 // 可能有 Null 參考傳回。
        return AccountUIData.SimulatedDataSource.SingleOrDefault(t => t.AccountId == key);
#pragma warning restore CS8603 // 可能有 Null 參考傳回。
    }
    #endregion
    /// <summary>
    /// 取得模擬資料來源
    /// </summary>
    private static TEC.Core.Collections.ThreadSafeObservableCollection<AccountInfo> SimulatedDataSource { set; get; }
}
