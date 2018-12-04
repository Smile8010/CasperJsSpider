using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Comom
{
    public class CommonMethods
    {
        public static string NewGuidString
        {
            get
            {
                return Guid.NewGuid().ToString().ToUpper();
            }
        }
    }
}
