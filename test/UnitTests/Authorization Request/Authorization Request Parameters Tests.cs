// ReSharper disable StringLiteralTypo

using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Xunit;
using System;

namespace UnitTests.Authorization_Request
{
    public class AuthorizationRequestParametersTests
    {
        [Fact(DisplayName = "Should generate authorization request URI")]
        public void Should_Create_Request_Params()
        {
            const string expectedQuery = "domain=telegrampassport" +
                                         "&bot_id=123" +
                                         "&scope=%7B%22data%22%3A%5B%5D%2C%22v%22%3A1%7D" +
                                         "&public_key=PUB%20KEY" +
                                         "&nonce=%2FNonce%21%2F";

            AuthorizationRequestParameters requestParameters = new(
                botId: 123,
                publicKey: "PUB KEY",
                nonce: "/Nonce!/",
                scope: new PassportScope(Array.Empty<IPassportScopeElement>())
            );

            Assert.Equal(123, requestParameters.BotId);
            Assert.Equal("PUB KEY", requestParameters.PublicKey);
            Assert.Equal("/Nonce!/", requestParameters.Nonce);
            Assert.NotNull(requestParameters.Scope);
            Assert.Equal(1, requestParameters.Scope.V);
            Assert.Empty(requestParameters.Scope.Data);
            Assert.Equal(expectedQuery, requestParameters.Query);
            Assert.Equal("tg:resolve?" + expectedQuery, requestParameters.AndroidUri);
            Assert.Equal("tg://resolve?" + expectedQuery, requestParameters.Uri);
            Assert.Equal("tg://resolve?" + expectedQuery, requestParameters.ToString());
        }
    }
}
