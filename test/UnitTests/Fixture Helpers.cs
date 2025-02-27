global using IDecrypter = Telegram.Bot.Passport.Decrypter;
global using Telegram.Bot;
global using System.Text.Json;
using System.IO;

namespace UnitTests
{
    public static class FixtureHelpers
    {
        internal static JsonSerializerOptions JsonOptions = new() { IncludeFields = true };

        /// <summary>
        /// Copies test files before unit tests parallel execution starts in order to avoid file access errors
        /// </summary>
        /// <param name="map">Mapping of source to destination files</param>
        public static void CopyTestFiles(
            params (string Src, string Dest)[] map
        )
        {
            foreach (var m in map)
                File.Copy($"Files/{m.Src}", $"Files/{m.Dest}", true);
        }

        public static string ArgError(string error, string param) => $@"^{error}\.\s+\(Parameter '{param}'\)$";
        public static string ArgNull(string param) => ArgError("Value cannot be null", param);
    }
}
