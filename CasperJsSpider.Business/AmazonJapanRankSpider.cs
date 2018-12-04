using CasperJsSpider.Comom;
using CasperJsSpider.Repository.Repository;
using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Business
{
    public static class AmazonJapanRankSpider
    {
        static readonly string ScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spider.js");

        public static void Run()
        {
            string updateString = DateTime.Now.ToString("yyyyMMdd");
            SysCatalogRepository SysCatalogDal = new SysCatalogRepository();
            List<SysCatalog> SysCatalogList = SysCatalogDal.Find(o => !o.IsDel && o.UpdateString != updateString);
            while (SysCatalogList.Count > 0)
            {
                _run(SysCatalogList, updateString);
                System.Threading.Thread.Sleep(500);
                SysCatalogList = SysCatalogDal.Find(o => o.UpdateString != updateString);
            }
            //TaskExecute<SysCatalog> tasks = new TaskExecute<SysCatalog>(o =>
            //{
            //    Console.WriteLine($"获取分类：{o.Url}");
            //    new CasperJs(s =>
            //    {
            //       DataReceived(s, o, SysCatalogDal, updateString);
            //    }).Exec(ScriptPath, o.Url);
            //},5);
            //SysCatalogList.ForEach(l =>
            //{
            //    tasks.AddQueue(l);

            //    //Console.WriteLine($"获取分类：{l.Url}");
            //    //new CasperJs(o =>
            //    //{
            //    //    DataReceived(o, l, SysCatalogDal, updateString);
            //    //}).Exec(ScriptPath, l.Url);
            //});

            //tasks.Run();
        }

        private static void _run(List<SysCatalog> SysCatalogList, string updateString)
        {
            TaskExecute<SysCatalog> tasks = new TaskExecute<SysCatalog>(o =>
            {
                Console.WriteLine($"获取分类：{o.Url}");
                new CasperJs(s =>
                {
                    DataReceived(s, o, new SysCatalogRepository(), updateString);
                }).Exec(ScriptPath, o.Url);
            }, 4);
            SysCatalogList.ForEach(l =>
            {
                tasks.AddQueue(l);

                //Console.WriteLine($"获取分类：{l.Url}");
                //new CasperJs(o =>
                //{
                //    DataReceived(o, l, SysCatalogDal, updateString);
                //}).Exec(ScriptPath, l.Url);
            });

            tasks.Run();

        }

        private static void DataReceived(string ouput, SysCatalog entity, SysCatalogRepository SysCatalogDal, string updateString)
        {
            Console.WriteLine(ouput);
            if (!string.IsNullOrEmpty(ouput)
               && ouput.StartsWith("※"))
            {
                List<SysProduct> ProductList = JsonConvert.DeserializeObject<List<SysProduct>>(ouput.TrimStart('※'));
                if (ProductList != null &&
                    ProductList.Count > 0)
                {
                    Dictionary<string, string> dic = SysCatalogDal.GetCatalogProducts(entity.ID);
                    List<SysProduct> AddList = new List<SysProduct>();
                    List<CatalogProductViewModel> AddMappingList = new List<CatalogProductViewModel>();
                    ProductList.ForEach(l =>
                    {
                        l.ID = CommonMethods.NewGuidString;
                        l.CatalogID = entity.ID;
                        l.CreateTime = DateTime.Now;

                        if (!dic.ContainsKey(l.Asin))
                        {
                            AddList.Add(l);
                        }
                        else {
                            l.ID = dic[l.Asin];
                        }

                        AddMappingList.Add(new CatalogProductViewModel()
                        {
                            ID = CommonMethods.NewGuidString,
                            CatalogID = entity.ID,
                            CreateTime = DateTime.Now,
                            ProductID = l.ID,
                            RankLevel = l.RankNumber,
                            RankTime = updateString
                        });


                        //SysCatalogDal.AddProductAndMapping(l,entity.CatalogProductTableName,updateString);
                    });

                    SysCatalogDal.AddProducts(AddList);

                    SysCatalogDal.AddCatalogProductMapping(entity.CatalogProductTableName, AddMappingList);

                    SysCatalogDal.UpdateCatalogUpdateString(entity.ID, updateString);
                }
            }
        }

        public static void Test()
        {
            // Guid ID = new Guid("5817603E-3F35-453D-AE85-FDD72483A1EC");
            //Console.WriteLine( new SysCatalogRepository().FindSingle(o => o.ID == ID).ToJson());
        }

        //static readonly string CasperJsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CasperJs", "bin");

        //public static void Run(Action<string> outMsgCallback = null)
        //{
        //    Queue<string> linksQueue = new Queue<string>();
        //    string linkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpiderLinks.txt");
        //    ProductRepository productDal = new ProductRepository();
        //    //判断文件是否存在。
        //    if (File.Exists(linkPath))
        //    {
        //        foreach (var item in File.ReadAllLines(linkPath))
        //        {
        //            string link = item.Trim();
        //            if (!string.IsNullOrEmpty(link))
        //            {
        //                //判断今天是否已经添加了产品了。
        //                if (!productDal.TodayIsGetProduct(link))
        //                {
        //                    linksQueue.Enqueue(link);
        //                }
        //            }
        //        }
        //    }
        //    //StringBuilder _execText = new StringBuilder();
        //    while (linksQueue.Count > 0)
        //    {
        //        string link = linksQueue.Dequeue();
        //        if (outMsgCallback != null)
        //        {
        //            outMsgCallback.BeginInvoke($"当前执行请求：{link},剩余:{linksQueue.Count}", null, null);
        //        }
        //        string content = ExecCasperJsEXE(link);
        //        if (!string.IsNullOrEmpty(content))
        //        {
        //            Insert(productDal, JsonConvert.DeserializeObject<CatalogViewModel>(content));
        //        }
        //    }

        //}

        //private static void Insert(ProductRepository productDal, CatalogViewModel entity)
        //{
        //    if (entity != null)
        //    {
        //        new CatalogRepository().AddIfNoExist(new Catalog
        //        {
        //            CatalogLink = entity.CatalogLink,
        //            CatalogName = entity.CatalogName
        //        });

        //        int start = 0, limit = 20;
        //        string createTime = DateTime.Now.ToString("yyyy-MM-dd");
        //        List<Product> ProductList;

        //        while ((ProductList = entity.Products.Skip(start).Take(limit).ToList().ConvertAll<Product>(o => new Product
        //        {
        //            Asin = o.Asin,
        //            CreateTime = DateTime.Now,
        //            ImgPath = o.ImgPath,
        //            Link = o.Link,
        //            Name = o.Name,
        //            Price = o.Price,
        //            RankNumber = o.RankNumber,
        //            Ref = o.Ref,
        //            RefCatalogLink = o.RefCatalogLink,
        //            RankTime = createTime
        //        })).Count > 0)
        //        {
        //            if (start == 0)
        //            {
        //                productDal.DeleteProduct(ProductList[0].RefCatalogLink, ProductList[0].RankTime);
        //            }
        //            start += limit;
        //            productDal.AddRange(ProductList);
        //        }

        //    }
        //}

        ///// <summary>
        ///// 执行获取 json 数据
        ///// </summary>
        ///// <param name="link"></param>
        ///// <returns></returns>
        //static string ExecCasperJsEXE(string link = "")
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

        //    p.StandardInput.WriteLine($"cd {CasperJsPath}");
        //    p.StandardInput.WriteLine(argStr);
        //    p.StandardInput.AutoFlush = true;
        //    p.StandardInput.WriteLine("exit");
        //    StreamReader reader = p.StandardOutput;//截取输出流  
        //    string ouputText = string.Empty;
        //    do
        //    {
        //        string ouput = reader.ReadLine();//每次读取一行
        //        if (!string.IsNullOrEmpty(ouput)
        //            && ouput.StartsWith("※"))
        //        {
        //            ouputText = ouput.TrimStart('※');
        //        }
        //    }
        //    while (!reader.EndOfStream);

        //    p.WaitForExit();

        //    p.Close();
        //    return ouputText;
        //}


        //public static void Test()
        //{
        //    Console.WriteLine(new CatalogRepository().Find().ToJson());
        //    //new CatalogRepository().Find()
        //}
    }
}
