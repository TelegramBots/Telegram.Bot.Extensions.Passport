// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions;

/// <summary>
/// Represents a fatal error in decryption of Telegram Passport Data
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public class PassportDataDecryptionException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    /// Initializes a new instance of <see cref="PassportDataDecryptionException"/>
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public PassportDataDecryptionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PassportDataDecryptionException"/>
    /// </summary>
    /// <param name="message">Error description</param>
    /// <param name="innerException">Root cause of the error</param>
    public PassportDataDecryptionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
