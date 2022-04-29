using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Telegram.Bot.Converters;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types.Passport;

/// <summary>
/// This object represents the data of an identity document.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class IdDocumentData : IDecryptedValue
{
    /// <summary>
    /// Document number
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string DocumentNo { get; set; } = default!;

    /// <summary>
    /// Optional. Date of expiry, in DD.MM.YYYY format
    /// </summary>
    [JsonConverter(typeof(CustomDateTimeConverter))]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public DateTime? ExpiryDate { get; set; }
}
