using CasperJsSpider.SqlLiteDomain.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Model
{
    /// <summary>
    /// 产品类
    /// </summary>
    [Table("Product")]
    public class Product: ProductTable
    {
        [Key]
        public new Guid ID { get; set; }
    }
}
