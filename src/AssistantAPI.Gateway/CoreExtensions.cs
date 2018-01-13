using System.Collections.Generic;
using System.Linq;

namespace AssistantAPI.Gateway
{
    public static class CoreExtensions
    {
        internal static string ToValuesString(this Dictionary<string, string> dictionary, string separator = ";") {
            return string.Join(separator, dictionary.Select(k => $"{k.Key}={k.Value}"));
        }
    }
}