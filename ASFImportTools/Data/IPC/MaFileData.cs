using System.Text.Json.Serialization;

namespace ASFImportTools.Data.IPC;

public sealed record MaFileData
{
    [JsonPropertyName("BackingAccessToken")]
    public string? BackingAccessToken { get; init; }

    [JsonPropertyName("_MobileAuthenticator")]
    public SecretData? MobileAuthenticator { get; init; }

    [JsonPropertyName("BackingRefreshToken")]
    public string? BackingRefreshToken { get; init; }
}

public sealed record SecretData
{
    [JsonPropertyName("shared_secret")]
    public string? SharedSecret { get; init; }
    [JsonPropertyName("identity_secret")]
    public string? IdentitySecret { get; init; }
}
