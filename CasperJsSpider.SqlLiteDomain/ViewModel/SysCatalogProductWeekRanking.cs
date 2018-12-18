using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.ViewModel
{
    /// <summary>
    /// 分类产品一周排名
    /// </summary>
    public class SysCatalogProductWeekRanking
    {

        public int RankLevel { get; set; }

        public DateTime CreateTime { get; set; }

        public string RankTime { get { return CreateTime.ToString("yyMMdd"); } }
    }

    public class SysCatalogProductNameWeekRanking
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string ImgPath { get; set; }
    }
}
