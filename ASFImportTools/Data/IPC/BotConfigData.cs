using System.Text.Json.Serialization;
using static ArchiSteamFarm.Steam.Storage.BotConfig;

namespace ASFImportTools.Data.IPC;

public record BotConfigData
{
    [JsonPropertyName("BotBehaviour")]
    public EBotBehaviour BotBehaviour { get; init; }

    [JsonPropertyName("Enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("SteamLogin")]
    public string? SteamLogin { get; init; }

    [JsonPropertyName("SteamPassword")]
    public string? SteamPassword { get; init; }

    [JsonPropertyName("Paused")]
    public bool Paused { get; init; }
}
