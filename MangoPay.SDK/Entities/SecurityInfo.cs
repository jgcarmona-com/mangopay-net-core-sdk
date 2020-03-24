using MangoPay.SDK.Core.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MangoPay.SDK.Entities
{
    public class SecurityInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AVSResult AVSResult { get; set; }
    }
}
