﻿using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderApp.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public void AddRange(List<Product> list)
        {
            using (DbContext Context = new SpiderContext())
            {
                DbSet<Product> dbSetEntity = Context.Set<Product>();
                Product first = list[0];
                list.ForEach(l =>
                {
                    l.ID = Guid.NewGuid();
                });
                dbSetEntity.AddRange(list);
                Save(Context);
            }
        }

        public void DeleteProduct(string RefCatalogLink, string RankTime)
        {
            using (DbContext Context = new SpiderContext())
            {
                SQLiteParameter[] sqlParameter = new SQLiteParameter[] {
                   new SQLiteParameter("@RefCatalogLink",RefCatalogLink),
                   new SQLiteParameter("@RankTime",RankTime)
                };
                Context.Database.ExecuteSqlCommand($"DELETE FROM Product WHERE RefCatalogLink=@RefCatalogLink AND RankTime=@RankTime; UPDATE Catalog SET RankTime=@RankTime WHERE CatalogLink=@RefCatalogLink;", sqlParameter);
            }
        }

        public bool TodayIsGetProduct(string RefCatalogLink)
        {
            using (DbContext Context = new SpiderContext())
            {
                string rankTime = DateTime.Now.ToString("yyyy-MM-dd");
                return Context.Set <Catalog>().Count(o => o.CatalogLink == RefCatalogLink && o.RankTime == rankTime) > 0;
            }
        }
    }
}
