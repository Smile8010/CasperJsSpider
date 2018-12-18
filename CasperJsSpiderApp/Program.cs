using CasperJsSpider.Business;
using CasperJsSpider.Comom;
using System;

namespace CasperJsSpiderApp
{
    class Program
    {
        //static readonly string ScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spider2.js");
        //static readonly string CasperJsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CasperJs", "bin");
        static void Main(string[] args)
        {
            Run();
            
           

            //TaskExecute<string> tasks = new TaskExecute<string>(o=> {
            //    Console.WriteLine(o);
            //    System.Threading.Thread.Sleep(2000);
            //},4);

            //int i = 5;
            //while (i > 0) {

            //    tasks.AddQueue($"{i}_Count");
            //    i--;
            //}

            //tasks.Run();




            //AmazonJapanRankSpider.Test();
            //Console.WriteLine("运行结束");
        }

        private static void Run() {
            //Logger.Info("运行开始！");
            AmazonJapanCatalogSpider.Run();
            AmazonJapanRankSpider.Run();
             

        }

        /// <summary>
        /// 执行获取 json 数据
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        //static void ExecCasperJsEXE(string link = "")
        //{
        //    string argStr = $"casperjs {ScriptPath} ";
        //    if (!string.IsNullOrEmpty(link))
        //    {
        //        argStr += $"{link} ";
        //    }
        //    //Console.WriteLine($"程序启动，参数：{argStr}");
        //    Process p = new Process();
        //    p.StartInfo.FileName = "cmd.exe";
        //    //p.StartInfo.Arguments = ;
        //    p.StartInfo.UseShellExecute = false;       //是否使用操作系统shell启动
        //    p.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
        //    p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
        //    p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
        //    p.StartInfo.CreateNoWindow = true; //不显示程序窗口

           

        //    p.Start();

        //    // 异步获取命令行内容  
        //    p.BeginOutputReadLine();
        //    p.OutputDataReceived += P_OutputDataReceived;

        //    p.StandardInput.WriteLine($"cd {CasperJsPath}");
        //    p.StandardInput.WriteLine(argStr);
        //    p.StandardInput.AutoFlush = true;
        //    p.StandardInput.WriteLine("exit");
        //    //StreamReader reader = p.StandardOutput;//截取输出流  
        //    //string ouputText = string.Empty;
        //    //do
        //    //{
        //    //    string ouput = reader.ReadLine();//每次读取一行
        //    //    if (!string.IsNullOrEmpty(ouput)
        //    //        && ouput.StartsWith("※"))
        //    //    {
        //    //        ouputText = ouput.TrimStart('※');
        //    //    }
        //    //}
        //    //while (!reader.EndOfStream);

        //    p.WaitForExit();

        //    p.Close();
        //    //return ouputText;
        //}

        //private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    Console.WriteLine(e.Data);
        //}
    }

}
