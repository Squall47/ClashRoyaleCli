using System;
using System.Globalization;
using Newtonsoft.Json;

namespace ClashRoyaleApi
{
    public class DateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateStr = serializer.Deserialize<string>(reader);
            DateTime dateout;
            DateTime.TryParseExact(dateStr, "yyyyMMdd'T'HHmmss.fff'Z'", CultureInfo.InvariantCulture,
                       DateTimeStyles.AdjustToUniversal, out dateout);

            var dateLocal = dateout.ToLocalTime();
            return dateLocal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            writer.WriteValue(date);
        }
    }
}
