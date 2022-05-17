﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template_NetCoreWeb.Utils.Enums.Logging;

namespace Template_NetCoreWeb.Utils.Logging
{
    /// <summary>
    /// 描述一筆記錄檔資料
    /// </summary>
    public class LogState : TEC.Core.Web.Logging.LogStateBase<LoggingScope, LoggingSystemScope, LoggingMessageType>
    {
        /// <summary>
        /// 設定或取得事件分類 ID
        /// </summary>
        public int EventTypeId { set; get; }
    }
}
