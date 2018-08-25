using Newtonsoft.Json.Converters;

namespace ClashRoyalCli
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
