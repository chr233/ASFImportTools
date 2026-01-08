using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFImportTools.Data.IPC;

public sealed record BotSummaryData
{
    public string? BotName { get; set; }
    public string? NickName { get; set; }
    public EAccountFlags AccountFlags { get; set; }
    public string? AvatarHash { get; set; }
    public string? PublishIP { get; set; }
    public bool IsLimited { get; set; }
    public bool IsLocked { get; set; }
    public bool Has2Fa { get; set; }
    public bool Enabled { get; set; }
    public bool Paused { get; set; }
    public bool KeepRunning { get; set; }
    public bool IsOnline { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public ulong? SteamId { get; set; }
    public string? WalletBalance { get; set; }
    public string? WalletCurrency { get; set; }
}
