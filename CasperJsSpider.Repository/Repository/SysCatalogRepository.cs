using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository.Repository
{
    public class SysCatalogRepository : BaseRepository<SysCatalog>
    {
        //DbContext _db = null;

        //DbContext DB
        //{
        //    get
        //    {
        //        if (_db == null)
        //        {
        //            _db = DbContextFactory.Context;
        //        }
        //        return _db;
        //    }
        //}

        //~SysCatalogRepository()
        //{
        //    if (_db != null)
        //    {
        //        _db.Dispose();
        //    }
        //    Console.WriteLine("~SysCatalogRepository()析构函数！");
        //}

        /// <summary>
        /// 创建分类产品映射表
        /// </summary>
        /// <returns></returns>
        public string CreateCatalogProductMapTable()
        {
            string tableName = $"Map{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";
            //表创建语句
            string createSql = string.Format(@"CREATE TABLE IF NOT EXISTS {0}(
  [ID] VARCHAR(36) PRIMARY KEY NOT NULL, 
  [CatalogID] VARCHAR(36) NOT NULL, 
  [ProductID] VARCHAR(36) NOT NULL, 
  [RankLevel] INT NOT NULL, 
  [RankTime] TEXT NOT NULL, 
  [CreateTime] DATETIME NOT NULL
)", tableName);

            using (DbContext DB = DbContextFactory.Context)
            {

                DB.Database.ExecuteSqlCommand(createSql);
                return tableName;
            }

        }

        //public new SysCatalog FindSingle(Expression<Func<SysCatalog, bool>> exp)
        //{
        //    using (DbContext DB = DbContextFactory.Context)
        //    {
        //        return DB.Set<SysCatalog>().AsNoTracking().FirstOrDefault(exp);
        //    }
        //}

        //       public void AddProducts(List<SysProduct> list)
        //       {
        //           using (DbContext DB = DbContextFactory.Context)
        //           {
        //               StringBuilder _ececSql = new StringBuilder();
        //               List<SQLiteParameter> sqliterParameters = new List<SQLiteParameter>();
        //               int i = 0;
        //               list.ForEach(l =>
        //               {
        //                   _ececSql.AppendLine(string.Format(@"INSERT INTO tb_Sys_Product(ID,ImgPath,RankNumber,Name,Url,Price,Ref,Asin,CatalogID,CreateTime,UpdateString) 
        //SELECT @ID{0},@ImgPath{0},@RankNumber{0},@Name{0},@Url{0},@Price{0},@Ref{0},@Asin{0},@CatalogID{0},@CreateTime{0} 
        //WHERE NOT EXISTS ( SELECT 1 FROM  tb_Sys_Product WHERE Asin=@Asin{0});", i));

        //                   sqliterParameters.Add(new SQLiteParameter($"@ID{i}", l.ID));
        //                   sqliterParameters.Add(new SQLiteParameter($"@ImgPath{i}", l.ImgPath));
        //                   sqliterParameters.Add(new SQLiteParameter($"@RankNumber{i}", l.RankNumber));
        //                   sqliterParameters.Add(new SQLiteParameter($"@Name{i}", l.Name));
        //                   sqliterParameters.Add(new SQLiteParameter($"@Url{i}", l.Url));
        //                   sqliterParameters.Add(new SQLiteParameter($"@Price{i}", l.Price));
        //                   sqliterParameters.Add(new SQLiteParameter($"@Ref{i}", l.Ref));
        //                   sqliterParameters.Add(new SQLiteParameter($"@Asin{i}", l.Asin));
        //                   sqliterParameters.Add(new SQLiteParameter($"@CatalogID{i}", l.CatalogID));
        //                   sqliterParameters.Add(new SQLiteParameter($"@CreateTime{i}", l.CreateTime));
        //                   i++;

        //               });

        //               DB.Database.ExecuteSqlCommand(_ececSql.ToString(), sqliterParameters.ToArray());
        //           }
        //       }

        //public void AddProductAndMapping(SysProduct entity, string MapName, string updateString)
        //{
        //    using (DbContext DB = DbContextFactory.Context)
        //    {
        //        var SysProductDbSet = DB.Set<SysProduct>();
        //        SysProduct findEntity = SysProductDbSet.FirstOrDefault(o => o.Asin == entity.Asin);
        //        Guid ProductID = entity.ID;
        //        if (findEntity == null)
        //        {
        //            SysProductDbSet.Add(entity);
        //        }
        //        else
        //        {
        //            ProductID = findEntity.ID;
        //        }

        //        DB.Database.ExecuteSqlCommand($"INSERT INTO {MapName}(ID,CatalogID,ProductID,RankLevel,RankTime,CreateTime) VALUES(@ID,@CatalogID,@ProductID,@RankLevel,@RankTime,@CreateTime);", new SQLiteParameter[] {
        //            new SQLiteParameter("@ID",Guid.NewGuid()),
        //            new SQLiteParameter("@CatalogID",entity.CatalogID),
        //            new SQLiteParameter("@ProductID",ProductID),
        //            new SQLiteParameter("@RankLevel",entity.RankNumber),
        //            new SQLiteParameter("@RankTime",updateString),
        //            new SQLiteParameter("@CreateTime",entity.CreateTime)
        //        });

        //        DB.SaveChanges();
        //    }
        //}

        public void UpdateCatalogUpdateString(string ID, string updateString)
        {
            using (DbContext DB = DbContextFactory.Context)
            {
                string updateSql = "UPDATE tb_Sys_Catalog SET UpdateString =@UpdateString WHERE ID=@ID;";
                DB.Database.ExecuteSqlCommand(updateSql, new SQLiteParameter[] {
                    new SQLiteParameter("@UpdateString",updateString),
                    new SQLiteParameter("@ID",ID)
                });
            }
        }

        public Dictionary<string, string> GetCatalogProducts(string CatalogID)
        {
            using (DbContext DB = DbContextFactory.Context)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var findList = DB.Set<SysProduct>().Where(o => o.CatalogID == CatalogID).Select(o => new { o.ID, o.Asin }).ToList();
                findList.ForEach(l =>
                {
                    if (!dic.ContainsKey(l.Asin))
                    {
                        dic.Add(l.Asin, l.ID);
                    }
                });

                return dic;
            }
        }

        public void AddProducts(List<SysProduct> list)
        {
            if (list.Count <= 0) { return; }
            using (DbContext DB = DbContextFactory.Context)
            {
                DB.Set<SysProduct>().AddRange(list);
                DB.SaveChanges();
            }
        }

        public void AddCatalogProductMapping(string tableName, List<CatalogProductViewModel> list)
        {
            using (DbContext DB = DbContextFactory.Context)
            {
                string sqlTemplate = "INSERT INTO " + tableName + "(ID,CatalogID,ProductID,RankLevel,RankTime,CreateTime) VALUES(@ID_{0},@CatalogID_{0},@ProductID_{0},@RankLevel_{0},@RankTime_{0},@CreateTime_{0});";
                List<SQLiteParameter> paramList = new List<SQLiteParameter>();
                StringBuilder _execSql = new StringBuilder();
                int innerCount = 0;
                list.ForEach(l =>
                {
                    innerCount++;
                    _execSql.AppendLine(string.Format(sqlTemplate, innerCount));
                    paramList.Add(new SQLiteParameter(string.Format("@ID_{0}", innerCount),l.ID));
                    paramList.Add(new SQLiteParameter(string.Format("@CatalogID_{0}", innerCount),l.CatalogID));
                    paramList.Add(new SQLiteParameter(string.Format("@ProductID_{0}", innerCount),l.ProductID));
                    paramList.Add(new SQLiteParameter(string.Format("@RankLevel_{0}", innerCount),l.RankLevel));
                    paramList.Add(new SQLiteParameter(string.Format("@RankTime_{0}", innerCount),l.RankTime));
                    paramList.Add(new SQLiteParameter(string.Format("@CreateTime_{0}", innerCount),l.CreateTime));
                });

                DB.Database.ExecuteSqlCommand(_execSql.ToString(),paramList.ToArray());
            }
        }

    }
}
