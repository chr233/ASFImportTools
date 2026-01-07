using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Security;
using ArchiSteamFarm.Steam.Storage;
using System.Reflection;

namespace ASFImportTools;

internal static class ReflectionUtils
{
    private readonly static PropertyInfo? IdentitySecretFieldInfo = typeof(MobileAuthenticator).GetProperty("IdentitySecret", BindingFlags.NonPublic | BindingFlags.Instance);
    private readonly static PropertyInfo? SharedSecretFiledInfo = typeof(MobileAuthenticator).GetProperty("SharedSecret", BindingFlags.NonPublic | BindingFlags.Instance);
    private readonly static PropertyInfo? MobileAuthenticatorFiledInfo = typeof(BotDatabase).GetProperty("MobileAuthenticator", BindingFlags.NonPublic | BindingFlags.Instance);

    public static string? GetIdentitySecret(this Bot bot)
    {
        if (!bot.HasMobileAuthenticator)
        {
            return null;
        }

        if (MobileAuthenticatorFiledInfo == null || IdentitySecretFieldInfo == null)
        {
            ASFLogger.LogGenericWarning("MobileAuthenticator or IdentitySecret is null");
            return null;
        }

        var authenticator = MobileAuthenticatorFiledInfo.GetValue(bot.BotDatabase) as MobileAuthenticator;
        var value = IdentitySecretFieldInfo.GetValue(authenticator) as string;
        return value;
    }

    public static string? GetSharedSecret(this Bot bot)
    {
        if (!bot.HasMobileAuthenticator)
        {
            return null;
        }

        if (MobileAuthenticatorFiledInfo == null || SharedSecretFiledInfo == null)
        {
            ASFLogger.LogGenericWarning("MobileAuthenticator or IdentitySecret is null");
            return null;
        }

        var authenticator = MobileAuthenticatorFiledInfo.GetValue(bot.BotDatabase) as MobileAuthenticator;
        var value = SharedSecretFiledInfo.GetValue(authenticator) as string;
        return value;
    }
}
