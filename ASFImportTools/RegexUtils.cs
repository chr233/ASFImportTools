using System.Text.RegularExpressions;

namespace ASFImportTools;

internal static partial class RegexUtils
{
    [GeneratedRegex(@"tradeofferid_(\d+)")]
    public static partial Regex MatchTradeOfferId();
}
