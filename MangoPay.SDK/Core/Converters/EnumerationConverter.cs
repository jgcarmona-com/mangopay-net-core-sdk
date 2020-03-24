using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace MangoPay.SDK.Core.Converters
{
    /// <summary>
    /// Enumeration Converter
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Converters.StringEnumConverter" />
    public class EnumerationConverter : StringEnumConverter
    {
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="JsonSerializationException"></exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = (Nullable.GetUnderlyingType(objectType) != null);

            if (reader.TokenType != JsonToken.Null || (reader.TokenType == JsonToken.Null && isNullable))
                return base.ReadJson(reader, objectType, existingValue, serializer);


            Type enumType = (Nullable.GetUnderlyingType(objectType) ?? objectType);
            if (!enumType.IsEnum)
                throw new JsonSerializationException(string.Format("Type {0} is not an enum type.", enumType.FullName));

            object result = Enum.GetValues(enumType).GetValue(0);
            return result;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}
