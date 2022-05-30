using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_NetCoreWeb.Core.Logging
{
    /// <summary>
    /// 描述指定物件必須實作取得 <see cref="ILoggerFactory"/> 的屬性
    /// </summary>
    public interface IRequireLoggerFactory
    {
        /// <summary>
        /// 取得物件所需使用的 <see cref="ILoggerFactory"/>
        /// </summary>
        ILoggerFactory LoggerFactory { get; }
    }
}
