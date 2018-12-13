using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CasperJsSpider.Comom
{
    public class AppConfig
    {
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetValue(string configName,string defaultValue="")
        {
            string val = ConfigurationManager.AppSettings[configName];
            if (string.IsNullOrEmpty(val))
            {
                val = defaultValue;
            }
            return val;
        }
    }
}
