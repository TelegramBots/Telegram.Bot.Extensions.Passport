/* Telegram Passport Quickstart. Read the documentations at:
 * https://telegrambots.github.io/book/4/passport/
 */

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Passport;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart
{
    class Program
    {
        static TelegramBotClient bot;

        static void Main()
        {
            bot = new TelegramBotClient("YOUR_ACCESS_TOKEN_HERE");

            User me = bot.GetMe().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            bot.OnMessage += Bot_OnMessage;
            Thread.Sleep(int.MaxValue);
        }

        static async Task Bot_OnMessage(Message message, UpdateType type)
        {
            if (message.Text != null)
                await SendAuthorizationRequestAsync(message.From.Id);
            else if (message.PassportData != null)
                await DecryptPassportDataAsync(message);
        }

        static async Task SendAuthorizationRequestAsync(long userId)
        {
            PassportScope scope = new PassportScope
            {
                Data =
                [
                    new PassportScopeElementOne(EncryptedPassportElementType.Address),
                    new PassportScopeElementOne(EncryptedPassportElementType.PhoneNumber),
                ]
            };
            var authReq = new AuthorizationRequestParameters(bot.BotId, PublicKey, "Test nonce for this demo", scope);

            await bot.SendMessage(userId, """
                Share your *residential address* and *phone number* with bot using Telegram Passport.

                1. Click inline button
                2. Open link in browser so it redirects you to Telegram Passport3. Authorize bot to access the info
                """, ParseMode.Markdown,
                replyMarkup: new InlineKeyboardButton("Share via Passport", $"https://telegrambots.github.io/Telegram.Bot.Extensions.Passport/redirect.html?{authReq.Query}"));
        }

        static async Task DecryptPassportDataAsync(Message message)
        {
            var decrypter = new Decrypter();

            // Step 1: Decrypt credentials
            var credentials = decrypter.DecryptCredentials(message.PassportData.Credentials, GetRsaPrivateKey());

            // Step 2: Validate nonce
            if (credentials.Nonce != "Test nonce for this demo")
                throw new Exception($"Invalid nonce: \"{credentials.Nonce}\".");

            // Step 3: Decrypt residential address using credentials
            var addressElement = message.PassportData.Data.Single(el => el.Type == EncryptedPassportElementType.Address);
            var address = decrypter.DecryptData<ResidentialAddress>(addressElement.Data, credentials.SecureData.Address.Data);

            // Step 4: Get phone number
            var phoneNumber = message.PassportData.Data.Single(el => el.Type == EncryptedPassportElementType.PhoneNumber).PhoneNumber;

            await bot.SendMessage(message.From.Id, $"""
                Your 🏠 address is:
                {address.StreetLine1}
                {address.City}, {address.CountryCode}
                {address.PostCode}

                📱 {phoneNumber}
                """);
        }

        static RSA GetRsaPrivateKey()
        {
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(PrivateKey);
            return rsa;
        }

        const string PublicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0VElWoQA2SK1csG2/sY/
wlssO1bjXRx+t+JlIgS6jLPCefyCAcZBv7ElcSPJQIPEXNwN2XdnTc2wEIjZ8bTg
BlBqXppj471bJeX8Mi2uAxAqOUDuvGuqth+mq7DMqol3MNH5P9FO6li7nZxI1FX3
9u2r/4H4PXRiWx13gsVQRL6Clq2jcXFHc9CvNaCQEJX95jgQFAybal216EwlnnVV
giT/TNsfFjW41XJZsHUny9k+dAfyPzqAk54cgrvjgAHJayDWjapq90Fm/+e/DVQ6
BHGkV0POQMkkBrvvhAIQu222j+03frm9b2yZrhX/qS01lyjW4VaQytGV0wlewV6B
FwIDAQAB
-----END PUBLIC KEY-----
";

        const string PrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEA0VElWoQA2SK1csG2/sY/wlssO1bjXRx+t+JlIgS6jLPCefyC
AcZBv7ElcSPJQIPEXNwN2XdnTc2wEIjZ8bTgBlBqXppj471bJeX8Mi2uAxAqOUDu
vGuqth+mq7DMqol3MNH5P9FO6li7nZxI1FX39u2r/4H4PXRiWx13gsVQRL6Clq2j
cXFHc9CvNaCQEJX95jgQFAybal216EwlnnVVgiT/TNsfFjW41XJZsHUny9k+dAfy
PzqAk54cgrvjgAHJayDWjapq90Fm/+e/DVQ6BHGkV0POQMkkBrvvhAIQu222j+03
frm9b2yZrhX/qS01lyjW4VaQytGV0wlewV6BFwIDAQABAoIBAQCetcR46XYrLeIe
7Trv2yolGDRlmfAzfZOnogXE0YkRfouLKyb4aXcY/hzBuLy0KjUNo9zsc1jk6X0C
TIHUf60NnJPKv43V9JHUHponAfPbZrpdRxq9y3VZOo6JyDinZiv13H/9H5uSnU1E
qGegeL7XgEbWKZosHr8lDya7BRljxSD8owxOJUoplkFSErQ/T/zUiPwFvm47rkC/
RcnPPnlh17T3jg5ko1ks8zqo953Rc+lAV0DqONRnOfpuM0msSdoHpRo1drLRwUId
h+okP5uTI4xHdWMpjyV2AS3aLDkdh55p1ImXO9bgI1D/G68J93D9Pr7Zo83f5Gde
XgJu5AkhAoGBAOejHYsLBmq4vn4Wm9f5WgoiX5pdB0D7wxZeZgrzsUMydbdbuPY/
nByGclFa2xuhklu1/JiRCi58fGThFqjZMi3O9X+7klsrRdoFuasmPOCVEOC0uFF8
+p54yW58Eu++n/RqhUEKJ9zKkgE/SrAzF5cpmZPVs+UyWlp+ouxUsA2jAoGBAOdV
DdrHd8f8P6gKyERaD7ARi4K5deNK3nrJ5BRAwyleab0d5ZCpqv5VklgFORPYlQNn
LvkmYUoylqc071pY5glv84aoIjLooDiuNl0JL8pDt2YVpYqozLvjmML0t723ARcu
+R74QuvlPUFwO7CINK3MFcNNEcPKKG9D+CeODU39AoGAKZYZWbsy/boJSS3Z8N5t
keA19cq54KrSjZOJEnJJ4tyOUcr/3AXzixOANqbvK3jIg/qaTPHNOCdVVe8rWEkW
Py8m2DXewst0EP5yJQ4KY++fRhhr9wVPIWBiGZng9HXu1bzCC7k9CuC7ccnhKN0j
YRow3l/BmmZ93j1aFr/lk60CgYBjbkqDM+NHQSP3TZMg1fkSO6hUavTB2mdgLbDs
B54bBOq8D8KheFv37730OWJ8JkZ+bPZivt1ob/ATNIyAr99IRSdORKxWZ2ielDrn
qFAzRwHoTfuWatF9HOmHOnpTf/pnBZiseBcDn8fBfcUaLqE95o+gH1s4ZYcVtAhQ
sB0F3QKBgQDTXnepfBSQw3cVBGVYjwbBxQxoL9GBUTMhjRaQfqUBWGJVMSaxib+6
dAbaMIAVw0DIMKG8tTYNEjEpyGuHvSht4vZRgarCNqDk13EKY80LkyOv2H/J01rt
gaZk6+H62W5zGnIbtzodB2n7JasK561Ic/QcrEtheC4Qmr+RXe03pg==
-----END RSA PRIVATE KEY-----
";
    }
}
