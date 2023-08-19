using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Luck.AspNetCore.Extensions;

/// <summary>
/// 自定义Json序列化
/// </summary>
public class SystemTextJsonConvert
{
    private const string DateFormat = "yyyy-MM-dd";
    private const string TimeFormat = "HH:mm:ss";
    private const string DateTimeFormat = $"{DateFormat} {TimeFormat}";

    /// <summary>
    /// 
    /// </summary>
    public class DecimalConverter : JsonConverter<decimal>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType == JsonTokenType.Number ? reader.GetDecimal() : decimal.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// 
    /// </summary>
    public class DecimalNullConverter : JsonConverter<decimal?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetDecimal()
                : string.IsNullOrEmpty(reader.GetString())
                    ? default(decimal?)
                    : decimal.Parse(reader.GetString()!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    public class IntConverter : JsonConverter<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetInt32()
                : string.IsNullOrEmpty(reader.GetString())
                    ? default
                    : int.Parse(reader.GetString()!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) => writer.WriteNumberValue(value);
    }

    /// <summary>
    /// 
    /// </summary>
    public class IntNullConverter : JsonConverter<int?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetInt32()
                : string.IsNullOrEmpty(reader.GetString())
                    ? default(int?)
                    : int.Parse(reader.GetString()!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value is not null) writer.WriteNumberValue(value.Value);
            else writer.WriteNullValue();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BoolConverter : JsonConverter<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.True or JsonTokenType.False
                ? reader.GetBoolean()
                : reader.TokenType == JsonTokenType.String
                    ? bool.Parse(reader.GetString()!)
                    : reader.TokenType == JsonTokenType.Number
                        ? reader.GetDouble() > 0
                        : throw new NotImplementedException($"un processed tokentype {reader.TokenType}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
    }

    /// <summary>
    /// 
    /// </summary>
    public class BoolNullConverter : JsonConverter<bool?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.True or JsonTokenType.False
                ? reader.GetBoolean()
                : reader.TokenType == JsonTokenType.Null
                    ? null
                    : reader.TokenType == JsonTokenType.String
                        ? bool.Parse(reader.GetString()!)
                        : reader.TokenType == JsonTokenType.Number
                            ? reader.GetDouble() > 0
                            : throw new NotImplementedException($"un processed tokentype {reader.TokenType}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value != null) writer.WriteBooleanValue(value.Value);
            else writer.WriteNullValue();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Convert.ToDateTime(reader.GetString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(DateTimeFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateTimeNullConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrEmpty(reader.GetString()) ? null : Convert.ToDateTime(reader.GetString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(DateTimeFormat));
    }
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Convert.ToDateTime(reader.GetString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => writer.WriteStringValue(value.LocalDateTime.ToString(DateTimeFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateTimeOffsetNullConverter : JsonConverter<DateTimeOffset?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrEmpty(reader.GetString()) ? null : Convert.ToDateTime(reader.GetString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.LocalDateTime.ToString(DateTimeFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => TimeSpan.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(TimeFormat));
    }

#if !NETSTANDARD
    /// <summary>
    /// 
    /// </summary>
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => TimeOnly.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(TimeFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeOnlyNullJsonConverter : JsonConverter<TimeOnly?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrWhiteSpace(reader.GetString()) ? null : TimeOnly.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(TimeFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateOnly.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(DateFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateOnlyNullJsonConverter : JsonConverter<DateOnly?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrWhiteSpace(reader.GetString()) ? null : DateOnly.Parse(reader.GetString()!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(DateFormat));
    }
#endif
}