using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Passport.Request;

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
    /// Scope version
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
    /// <param name="v">Scope version. Defaults to 1.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public PassportScope(IEnumerable<IPassportScopeElement> data, int v = 1)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        V = v;
    }
}
