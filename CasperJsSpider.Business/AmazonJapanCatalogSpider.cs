using CasperJsSpider.Comom;
using CasperJsSpider.Repository.Repository;
using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CasperJsSpider.Business
{
    /// <summary>
    /// 亚马逊日本分类数据抓取
    /// </summary>
    public static class AmazonJapanCatalogSpider
    {

        private static readonly string CatalogScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CatalogSpider.js");


        public static void Run()
        {
            //链接地址
            string linkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpiderLinks.txt");
            Queue<string> linksQueue = new Queue<string>();
            SysCatalogRepository SysCatalogDal = new SysCatalogRepository();
            //判断文件是否存在。
            if (File.Exists(linkPath))
            {
                foreach (var item in File.ReadAllLines(linkPath))
                {
                    string link = item.Trim();
                    if (!string.IsNullOrEmpty(link))
                    {
                        linksQueue.Enqueue(link);
                    }
                }
            }

            CasperJs casperJs = new CasperJs(DataReceived);

            while (linksQueue.Count > 0)
            {
                string link = linksQueue.Dequeue();
                SysCatalog entity = SysCatalogDal.FindSingle(o => o.Url == link);
                if (entity == null)
                {
                    Console.WriteLine($"执行获取分类：{link}");
                    casperJs.Exec(CatalogScriptPath, link);
                }
            }

        }

        /// <summary>
        /// cmd 消息接收事件
        /// </summary>
        /// <param name="ouput"></param>  
        private static void DataReceived(string ouput)
        {                          
            if (!string.IsNullOrEmpty(ouput)
                 && ouput.StartsWith("※"))
            {
                SysCatalogRepository SysCatalogDal = new SysCatalogRepository();
                SysCatalogViewModel entity = JsonConvert.DeserializeObject<SysCatalogViewModel>(ouput.TrimStart('※'));
                if (entity != null)
                {
                    List<SysCatalog> entities = new List<SysCatalog>();
                    string updateTime = DateTime.Now.AddDays(-5).ToString("yyyyMMdd");
                    entity.CatalogProductTableName = SysCatalogDal.CreateCatalogProductMapTable();
                    SysCatalog firstCatalog = new SysCatalog
                    {
                        ID = CommonMethods.NewGuidString,
                        UpdateString = updateTime,
                        CreateTime = DateTime.Now,
                        Name = entity.Name,
                        Url = entity.Url,
                        CatalogProductTableName = entity.CatalogProductTableName,
                        IsDel=false
                    };
                    entities.Add(firstCatalog);
                    entity.ChildCatalogs.ForEach(l =>
                    {
                        entities.Add(new SysCatalog
                        {
                            ID = CommonMethods.NewGuidString,
                            UpdateString = firstCatalog.UpdateString,
                            CreateTime = DateTime.Now,
                            Name = l.Name,
                            Url = l.Url,
                            CatalogProductTableName = firstCatalog.CatalogProductTableName,
                            ParentID = firstCatalog.ID   ,
                            IsDel=false
                        });
                    });

                    SysCatalogDal.AddRange(entities);

                    Console.WriteLine($"链接：{entity.Url} 分类数据添加完成！");
                }
            }
        }

    }
}
