using Microsoft.Extensions.Logging;
using SoapDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEC.Core.Logging.Soap;

namespace Template_NetCoreWeb.Core.Logging.SoapManagers;

/// <summary>
/// 初始化 DEMO 使用的 <see cref="LoggerFactoryLoggingSoapManagerBase{SOAPDemoSoapClient, SoapDemo.SOAPDemoSoap}"/>
/// </summary>
public class SoapDemoLoggingManager : LoggerFactoryLoggingSoapManagerBase<SOAPDemoSoapClient, SoapDemo.SOAPDemoSoap>
{
    /// <summary>
    /// 初始化一個 <see cref="SoapDemoLoggingManager"/>
    /// </summary>
    public SoapDemoLoggingManager(ILoggerFactory? loggerFactory)
        : base(new TEC.Core.Logging.Soap.LoggingSoapManagerOptions<SOAPDemoSoapClient, SOAPDemoSoap, Utils.Enums.Logging.LoggingSystemScope>(Utils.Enums.Logging.LoggingSystemScope.DemoSoap), loggerFactory)
    {
    }
}
