using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CasperJsSpider.Comom
{
    public static class ObjectExpansion
    {

        public static string ToJson(this object obj, string dateFormate = "")
        {

            if (!string.IsNullOrEmpty(dateFormate))
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = dateFormate;
                return JsonConvert.SerializeObject(obj, timeConverter);
            }
            return JsonConvert.SerializeObject(obj);
        }
    }
}
