using CasperJsSpider.Comom;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostAddress = AppConfig.GetValue("hostAddress", "http://localhost:12345");
            using (WebApp.Start(hostAddress))
            {
                Console.WriteLine($"程序已启动，地址：{hostAddress}");
                System.Diagnostics.Process.Start($"{hostAddress}/Pages/index.html");
                Console.WriteLine($"按任意键退出...");
                Console.ReadLine();
            }
        }

    }
}
