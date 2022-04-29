using Telegram.Bot.Extensions;
using Telegram.Bot.Passport;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.PassportErrors;
using Telegram.Bot.Types.Passport;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot;

/// <summary>
/// Contains extension methods for <see cref="ITelegramBotClient"/> instances
/// </summary>
public static class TelegramBotClientPassportExtensions
{
    /// <summary>
    /// Informs a user that some of the Telegram Passport elements they provided contains errors. The user will
    /// not be able to re-submit their Passport to you until the errors are fixed (the contents of the field for
    /// which you returned the error must change). Returns True on success.
    /// Use this if the data submitted by the user doesn't satisfy the standards your service requires for
    /// any reason. For example, if a birthday date seems invalid, a submitted document is blurry, a scan shows
    /// evidence of tampering, etc. Supply some details in the error message to make sure the user knows how to
    /// correct the issues.
    /// </summary>
    /// <param name="botClient">Instance of bot client</param>
    /// <param name="userId">User identifier</param>
    /// <param name="errors">Descriptions of the errors</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <see href="https://core.telegram.org/bots/api#setpassportdataerrors"/>
    public static async Task SetPassportDataErrorsAsync(
        this ITelegramBotClient botClient,
        long userId,
        IEnumerable<PassportElementError> errors,
        CancellationToken cancellationToken = default
    ) =>
        await botClient.ThrowIfNull(nameof(botClient))
            .MakeRequestAsync(new SetPassportDataErrorsRequest(userId, errors), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Downloads an encrypted Passport file, decrypts it, and writes the content to
    /// <paramref name="destination"/> stream
    /// </summary>
    /// <param name="botClient">Instance of bot client</param>
    /// <param name="passportFile"></param>
    /// <param name="fileCredentials"></param>
    /// <param name="destination"></param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>File information of the encrypted Passport file on Telegram servers.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<Types.File> DownloadAndDecryptPassportFileAsync(
        this ITelegramBotClient botClient,
        PassportFile passportFile,
        FileCredentials fileCredentials,
        System.IO.Stream destination,
        CancellationToken cancellationToken = default
    )
    {
        if (passportFile is null)
            throw new ArgumentNullException(nameof(passportFile));
        if (fileCredentials is null)
            throw new ArgumentNullException(nameof(fileCredentials));
        if (destination is null)
            throw new ArgumentNullException(nameof(destination));

        Types.File fileInfo;

        var encryptedContentStream = (passportFile.FileSize is int fileSize and > 0)
            ? new MemoryStream(capacity: fileSize)
            : new MemoryStream();

        using (encryptedContentStream)
        {
            fileInfo = await botClient.GetInfoAndDownloadFileAsync(
                passportFile.FileId,
                encryptedContentStream,
                cancellationToken
            ).ConfigureAwait(false);

            encryptedContentStream.Position = 0;

            await new Decrypter().DecryptFileAsync(
                encryptedContentStream,
                fileCredentials,
                destination,
                cancellationToken
            ).ConfigureAwait(false);
        }

        return fileInfo;
    }
}
