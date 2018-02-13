using System;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace MyBucks.Core.ApiGateway.ApiClient.TypeConverters
{
    public class GrpcDateTimeConverter : JsonConverter
    {
        public class ProtoTimeStamp
        {

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            
            var val = (DateTime)(value ?? new DateTime());
            var ts = (val.ToUniversalTime().ToTimestamp());

            writer.WriteStartObject();
            writer.WritePropertyName("seconds");
            serializer.Serialize(writer, ts.Seconds);
            writer.WritePropertyName("nanos");
            serializer.Serialize(writer, ts.Nanos);
            writer.WriteEndObject();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            try
            {
                JObject jsonObject = JObject.Load(reader);
                var properties = jsonObject.Properties().ToList();
                var ts = new Timestamp();
                ts.Seconds = (long)properties[0].Value;
                ts.Nanos = (int)properties[1].Value;
                return ts.ToDateTime();
            }
            catch (Exception e)
            {
                return new DateTime();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType);
        }
    }
}