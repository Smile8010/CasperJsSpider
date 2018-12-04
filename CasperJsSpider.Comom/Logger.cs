using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Comom
{
    public static class Logger
    {
        static Logger()
        {
            string configFileName = "log4net.config";
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
            if (File.Exists(configFilePath))
            {
                FileInfo configFile = new FileInfo(configFilePath);
                XmlConfigurator.Configure(configFile);
            }
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            WriteLog(EnumLog.Error, msg);
        }

        /// <summary>
        /// 输出基本日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Default(string msg)
        {
            WriteLog(EnumLog.Default, msg);
        }

        /// <summary>
        /// 输出信息日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            WriteLog(EnumLog.Info, msg);
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="msg">消息</param>
        private static void WriteLog(EnumLog type, string msg)
        {
            switch (type)
            {
                case EnumLog.Error:
                    LogManager.GetLogger("Error").Debug(msg);
                    break;
                case EnumLog.Info:
                    LogManager.GetLogger("Info").Debug(msg);
                    break;
                default:
                    LogManager.GetLogger("Default").Debug(msg);
                    break;
            }
        }

        internal enum EnumLog
        {
            Default = 0,

            Error = 1,

            Info = 2
        }
    }


}
