using MangoPay.SDK.Core.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MangoPay.SDK.Entities
{
    /// <summary>
    /// Birth Place
    /// </summary>
    public class Birthplace
    {
        /// <summary>City.</summary>
        public string City;
        
        /// <summary>Country.</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CountryIso? Country;
    }
}