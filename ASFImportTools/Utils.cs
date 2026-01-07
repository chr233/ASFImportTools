using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFImportTools.Data.IPC;
using ASFImportTools.Data.Plugin;
using System.Reflection;
using System.Text;
using static ArchiSteamFarm.Steam.Storage.BotConfig;

namespace ASFImportTools;

internal static class Utils
{
    internal const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    /// <summary>
    ///     插件配置
    /// </summary>
    internal static PluginConfig Config { get; set; } = null!;

    /// <summary>
    ///     获取版本号
    /// </summary>
    internal static Version MyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0.0");

    /// <summary>
    ///     获取ASF版本
    /// </summary>
    internal static Version ASFVersion => typeof(ASF).Assembly.GetName().Version ?? new Version("0.0.0.0");

    /// <summary>
    ///     获取插件所在路径
    /// </summary>
    internal static string MyLocation => Assembly.GetExecutingAssembly().Location;

    /// <summary>
    ///     获取插件所在文件夹路径
    /// </summary>
    internal static string MyDirectory => Path.GetDirectoryName(MyLocation) ?? ".";

    /// <summary>
    ///     日志
    /// </summary>
    internal static ArchiLogger ASFLogger => ASF.ArchiLogger;

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message) => $"<ASFE> {message}";

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message, params object?[] args) =>
        FormatStaticResponse(string.Format(message, args));

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message) => $"<{bot.BotName}> {message}";

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message, params object?[] args) =>
        bot.FormatBotResponse(string.Format(message, args));

    internal static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object?[] args) =>
        sb.AppendLine(string.Format(format, args));

#if DEBUG
    internal const bool IsDebug = true;
#else
    internal const bool IsDebug = false;
#endif

    /// <summary>
    /// 读取Bot数据库文件
    /// </summary>
    /// <param name="botName"></param>
    /// <returns></returns>
    private static async Task<MaFileData?> ReadBotDbFile(string botName)
    {
        try
        {
            var dbPath = Bot.GetFilePath(botName, Bot.EFileType.Database);
            if (!File.Exists(dbPath))
            {
                return null;
            }

            var json = await File.ReadAllTextAsync(dbPath, Encoding.UTF8).ConfigureAwait(false);

            return json?.ToJsonObject<MaFileData>();
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return null;
        }
    }

    /// <summary>
    /// 创建或者更新Bot数据库文件
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="identitySecret"></param>
    /// <param name="sharedSecret"></param>
    /// <returns></returns>
    internal static async Task<bool> CreateOrUpdateBotDbFile(string botName, string identitySecret, string sharedSecret)
    {
        try
        {
            var dbPath = Bot.GetFilePath(botName, Bot.EFileType.Database);

            MaFileData maFile;

            var oldData = await ReadBotDbFile(botName).ConfigureAwait(false);

            if (oldData != null)
            {
                maFile = new MaFileData
                {
                    BackingAccessToken = oldData.BackingAccessToken,
                    BackingRefreshToken = oldData.BackingRefreshToken,
                    MobileAuthenticator = new SecretData
                    {
                        SharedSecret = sharedSecret,
                        IdentitySecret = identitySecret,
                    }
                };
            }
            else
            {
                maFile = new MaFileData
                {
                    MobileAuthenticator = new SecretData
                    {
                        SharedSecret = sharedSecret,
                        IdentitySecret = identitySecret,
                    }
                };
            }

            var newJson = maFile.ToJsonText();
            await File.WriteAllTextAsync(dbPath, newJson, Encoding.UTF8).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return false;
        }
    }

    /// <summary>
    /// 创建机器人配置文件
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <param name="enabled"></param>
    /// <param name="paused"></param>
    /// <param name="botBehaviour"></param>
    /// <returns></returns>
    internal static async Task<bool> CreateBotConfigFile(string botName, string login, string password, bool enabled, bool paused, EBotBehaviour botBehaviour)
    {
        try
        {
            var filePath = Bot.GetFilePath(botName, Bot.EFileType.Config);
            var botConfig = new BotConfigData
            {
                Enabled = enabled,
                Paused = paused,
                BotBehaviour = botBehaviour,
                SteamLogin = login,
                SteamPassword = password,
            };

            var json = botConfig.ToJsonText();
            await File.WriteAllTextAsync(filePath, json, Encoding.UTF8).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return false;
        }
    }
}