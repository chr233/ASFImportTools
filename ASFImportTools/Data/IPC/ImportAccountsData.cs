namespace ASFImportTools.Data.IPC;

public sealed record ImportAccountsData(
    bool Enabled,
    bool Paused,
    string BotName,
    string? SteamLogin,
    string? SteamPassword,
    string? IdentitySecret,
    string? SharedSecret
);
