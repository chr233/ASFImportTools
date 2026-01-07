using System.Text.Json.Serialization;

namespace ASFImportTools.Data.IPC;

public sealed record BotSummaryData
{
    public string? BotName { get; set; }
    public string? NickName { get; set; }
    public string? AccountName { get; set; }
    public bool IsLimit { get; set; }
    public bool Has2Fa { get; set; }
    public bool Enabled { get; set; }
    public bool Paused { get; set; }
    public bool IsOnline { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public ulong? SteamId { get; set; }
    public string? WalletBalance { get; set; }
    public string? WalletCurrency { get; set; }
}
