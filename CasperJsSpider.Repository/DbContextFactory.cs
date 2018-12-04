using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository
{
    public static class DbContextFactory
    {
        public static DbContext Context
        {
            get
            {
                return new SpiderContext();
            }
        }
    }
}
