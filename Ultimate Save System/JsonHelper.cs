using System;
using Codice.CM.Common.Serialization.Replication;
using Newtonsoft.Json;
using UnityEngine;

namespace UltimateSaveSystem
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _settings;

        private static JsonSerializerSettings Settings
        {
            get
            {
                if (_settings != null) return _settings;
                _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new Vector2Converter());
                _settings.Converters.Add(new Vector3Converter());
                _settings.Converters.Add(new QuaternionConverter());
                _settings.Converters.Add(new ColorConverter());
                return _settings;
            }
        }

        public static string Serialize<T>(this T obj) => JsonConvert.SerializeObject(obj, Settings);
        public static T Deserialize<T>(this string json) => JsonConvert.DeserializeObject<T>(json, Settings);
    }

    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WriteEndObject();
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            float x = 0, y = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    reader.Read();
                    if (propertyName == "x")
                        x = Convert.ToSingle(reader.Value);
                    else if (propertyName == "y")
                        y = Convert.ToSingle(reader.Value);
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return new Vector2(x, y);
        }
    }

    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            float x = 0, y = 0, z = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    reader.Read();
                    if (propertyName == "x")
                        x = Convert.ToSingle(reader.Value);
                    else if (propertyName == "y")
                        y = Convert.ToSingle(reader.Value);
                    else if (propertyName == "z")
                        z = Convert.ToSingle(reader.Value);
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return new Vector3(x, y, z);
        }
    }

    public class QuaternionConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
            writer.WriteEndObject();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            float x = 0f, y = 0f, z = 0f, w = 0f;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    reader.Read();
                    switch (propertyName)
                    {
                        case "x":
                            x = Convert.ToSingle(reader.Value);
                            break;
                        case "y":
                            y = Convert.ToSingle(reader.Value);
                            break;
                        case "z":
                            z = Convert.ToSingle(reader.Value);
                            break;
                        case "w":
                            w = Convert.ToSingle(reader.Value);
                            break;
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return new Quaternion(x, y, z, w);
        }
    }

    public class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            string hexValue = ColorUtility.ToHtmlStringRGBA(value);
            writer.WriteValue($"#{hexValue}");
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            string hexString = reader.Value.ToString();

            if (hexString.StartsWith("#"))
                hexString = hexString.Substring(1);

            if (ColorUtility.TryParseHtmlString($"#{hexString}", out Color color))
                return color;

            return Color.white;
        }
    }
}