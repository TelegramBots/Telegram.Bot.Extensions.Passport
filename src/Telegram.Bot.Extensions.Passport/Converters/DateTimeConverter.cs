using Newtonsoft.Json.Converters;

namespace Telegram.Bot.Converters;

internal class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        base.DateTimeFormat = "dd.MM.yyyy";
    }
}
