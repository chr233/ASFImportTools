using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFImportTools.Data.Plugin;
using System.ComponentModel;
using System.Composition;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ASFImportTools;

[Export(typeof(IPlugin))]
internal sealed class ASFImportTools : IASF, IBotCommand2, IGitHubPluginUpdates, IWebInterface
{
    private const string ShortName = "AIT";

    private bool ASFEBridge;

    private Timer? StatisticTimer;

    /// <summary>
    /// 获取插件信息
    /// </summary>
    private string PluginInfo => $"{Name} {Version}";

    public string Name => "ASF Import Tools";

    public Version Version => MyVersion;

    public bool CanUpdate => true;
    public string RepositoryName => "chr233/ASFImportTools";

    public string PhysicalPath => "ASFImportTools.www";

    public string WebPath => "/Import";

    /// <summary>
    /// ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null)
    {
        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach (var (configProperty, configValue) in additionalConfigProperties)
            {
                if (configProperty != "ASFEnhance" || configValue.ValueKind != JsonValueKind.Object)
                {
                    continue;
                }

                try
                {
                    config = configValue.ToJsonObject<PluginConfig>();
                    if (config != null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ASFLogger.LogGenericException(ex);
                }
            }
        }

        Config = config ?? new PluginConfig(false, true);

        var sb = new StringBuilder();

        //使用协议
        if (!Config.EULA)
        {
            sb.AppendLine();
            sb.AppendLine(Langs.Line);
            sb.AppendLineFormat(Langs.EulaWarning, Name);
            sb.AppendLine(Langs.Line);
        }

        if (sb.Length > 0)
        {
            ASFLogger.LogGenericWarning(sb.ToString());
        }

        //统计
        if (Config.Statistic && !ASFEBridge)
        {
            StatisticTimer = new Timer(DoStatistic, null, TimeSpan.FromSeconds(30), TimeSpan.FromHours(24));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 统计信息
    /// </summary>
    /// <param name="state"></param>
    private async void DoStatistic(object? state)
    {
        try
        {
            var request = new Uri("https://asfe.chrxw.com/asfimporttools");
            _ = await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
        }
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        ASFLogger.LogGenericInfo(Langs.PluginContact);
        ASFLogger.LogGenericInfo(Langs.PluginInfo);

        var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        var handler = typeof(ASFImportTools).GetMethod(nameof(ResponseCommand), flag);

        const string pluginId = nameof(ASFImportTools);
        const string cmdPrefix = ShortName;
        const string repoName = nameof(ASFImportTools);

        ASFEBridge = AdapterBridge.InitAdapter(Name, pluginId, cmdPrefix, repoName, handler);

        if (ASFEBridge)
        {
            ASFLogger.LogGenericDebug(Langs.ASFEnhanceRegisterSuccess);
        }
        else
        {
            ASFLogger.LogGenericInfo(Langs.ASFEnhanceRegisterFailed);
            ASFLogger.LogGenericWarning(Langs.PluginStandalongMode);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 处理命令事件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (ASFEBridge)
        {
            return null;
        }

        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            var cmd = args[0].ToUpperInvariant();

            if (cmd.StartsWith("AIT."))
            {
                cmd = cmd[4..];
            }

            var task = ResponseCommand(bot, access, cmd, args);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }

            return null;
        }
        catch (Exception ex)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                ASFLogger.LogGenericException(ex);
            }).ConfigureAwait(false);

            return ex.StackTrace;
        }
    }

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Task<string>? ResponseCommand(Bot bot, EAccess access, string cmd, string[] args)
    {
        var argLength = args.Length;

        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch //不带参数
            {
                //插件信息
                "ASFIMPORTTOOLS" or
                ShortName when access >= EAccess.FamilySharing => Task.FromResult(PluginInfo),

                _ => null
            },
            _ => cmd switch //带参数
            {

                _ => null
            }
        };
    }
}
