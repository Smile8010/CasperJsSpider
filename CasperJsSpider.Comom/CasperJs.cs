using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CasperJsSpider.Comom
{
    /// <summary>
    /// CasperJs类
    /// </summary>
    public class CasperJs
    {
        static readonly string CasperJsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CasperJs", "bin");

        static readonly string ProxyAddress = ConfigurationManager.AppSettings["ProxyAddress"] ?? "";

        Action<string> ouputDataReceived = null;

        //Queue<string> ouputMsg=null;

        public CasperJs(Action<string> ouputDataReceived = null)
        {
            if (ouputDataReceived != null)
            {
                this.ouputDataReceived = ouputDataReceived;
            }

            //ouputMsg = new Queue<string>();
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="scriptPath">文件地址</param>
        /// <param name="args">参数</param>
        public void Exec(string scriptPath, params string[] args)
        {
            string argStr = $"casperjs";
            if (!string.IsNullOrEmpty(ProxyAddress)) {
                // --proxy=127.0.0.1:65500
                argStr += $" --proxy={ProxyAddress}";
            }
            argStr += $" --ignore-ssl-errors=yes --web-security=false {scriptPath} ";
            foreach (string arg in args)
            {
                argStr += $"{arg} ";
            }
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;       //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            p.StartInfo.CreateNoWindow = true; //不显示程序窗口


            p.Start();

            if (this.ouputDataReceived != null)
            {
                p.BeginOutputReadLine();
                p.OutputDataReceived += DataReceived;
            }

            p.StandardInput.WriteLine($"cd /d {CasperJsPath}");
            p.StandardInput.WriteLine(argStr);
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("exit");

            p.WaitForExit();

            p.Close();

            //while (ouputMsg.Count > 0)
            //{
            //    this.ouputDataReceived(ouputMsg.Dequeue());
            //}

            System.Threading.Thread.Sleep(500);
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            //ouputMsg.Enqueue(e.Data);
            this.ouputDataReceived(e.Data);
        }
    }
}
