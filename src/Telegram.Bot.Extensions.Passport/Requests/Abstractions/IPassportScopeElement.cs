// ReSharper disable once CheckNamespace

namespace Telegram.Bot.Requests.Abstractions;

/// <summary>
/// A marker interface for object represents a requested element
/// </summary>
public interface IPassportScopeElement
{
    /// <summary>
    /// Optional. Use this parameter if you want to request a selfie with the document
    /// from this list that the user chooses to upload.
    /// </summary>
    bool? Selfie { get; }

    /// <summary>
    /// Optional. Use this parameter if you want to request a translation of the document
    /// from this list that the user chooses to upload. Note: We suggest to only request
    /// translations after you have received a valid document that requires one.
    /// </summary>
    bool? Translation { get; }
}
