using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests.Abstractions;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests;

/// <summary>
/// This object represents the data to be requested.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class PassportScope
{
    /// <summary>
    /// List of requested elements, each type may be used only once in the entire array of
    /// <see cref="IPassportScopeElement"/> objects
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public IEnumerable<IPassportScopeElement> Data { get; }

    /// <summary>
    /// Scope version, must be 1
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public int V { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PassportScope"/> with required parameters
    /// </summary>
    /// <param name="data">
    /// List of requested elements, each type may be used only once in the entire array of
    /// <see cref="IPassportScopeElement"/> objects
    /// </param>
    /// <param name="v">Scope version, must be 1</param>
    /// <exception cref="ArgumentNullException"></exception>
    public PassportScope(IEnumerable<IPassportScopeElement> data, int v = 1)
    {
        Data = data.ThrowIfNull(nameof(data));
        V = v;
    }
}
