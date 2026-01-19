using ArchiSteamFarm.IPC.Controllers.Api;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using ASFImportTools.Data.IPC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ASFImportTools.IPC;

/// <summary>
/// 测试相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class ImportController : ArchiController
{
    /// <summary>
    /// 获取账号信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("{botNames:required}")]
    [EndpointSummary("获取账号信息")]
    public ActionResult GetBotList(string botNames = "ASF")
    {
        if (!Config.EULA)
        {
            return Ok(new GenericResponse(false, Langs.EulaNotConfirmed));
        }

        var bots = Bot.GetBots(botNames);
        if (bots == null)
        {
            return Ok(new GenericResponse(false, Langs.InternalError));
        }

        Dictionary<string, BotSummaryData> response = [];

        foreach (var bot in bots)
        {
            var walletBalance = Convert.ToDecimal(bot.WalletBalance) / 100;

            var summary = new BotSummaryData
            {
                BotName = bot.BotName,
                NickName = bot.Nickname,
                AccountFlags = bot.AccountFlags,
                AvatarHash = bot.AvatarHash,
                PublishIP = bot.PublicIP,
                IsLimited = bot.IsAccountLimited,
                IsLocked = bot.IsAccountLocked,
                Has2Fa = bot.HasMobileAuthenticator,
                Enabled = bot.BotConfig.Enabled,
                Paused = bot.CardsFarmer.Paused,
                KeepRunning = bot.KeepRunning,
                IsOnline = bot.IsConnectedAndLoggedOn,
                SteamId = bot.SteamID,
                WalletBalance = walletBalance.ToString(CultureInfo.CurrentCulture),
                WalletCurrency = bot.WalletCurrency.ToString()
            };

            response.TryAdd(bot.BotName, summary);
        }

        return Ok(new GenericResponse<Dictionary<string, BotSummaryData>>(true, "ok", response));
    }

    /// <summary>
    /// 批量导入账号
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="allowReplace"></param>
    /// <returns></returns>
    [HttpPost]
    [EndpointSummary("批量导入账号")]
    public async Task<ActionResult> ImportBots(
        [FromBody] List<ImportAccountsData> accounts, [FromQuery] bool allowReplace)
    {
        if (!Config.EULA)
        {
            return Ok(new GenericResponse(false, Langs.EulaNotConfirmed));
        }

        Dictionary<string, ImportResultData> importResult = [];

        List<string> updatedBotNames = [];

        foreach (var item in accounts)
        {
            if (string.IsNullOrEmpty(item.BotName))
            {
                continue;
            }

            ImportResultData result;

            var oldBot = Bot.GetBot(item.BotName);
            if (oldBot != null)
            {
                if (!allowReplace)
                {
                    result = new ImportResultData(item.BotName, true, oldBot.HasMobileAuthenticator, "Bot已存在，跳过更新");
                }
                else
                {
                    updatedBotNames.Add(item.BotName);

                    if (!oldBot.HasMobileAuthenticator && !string.IsNullOrEmpty(item.IdentitySecret) && !string.IsNullOrEmpty(item.SharedSecret))
                    {
                        if (oldBot.IsConnectedAndLoggedOn)
                        {
                            await oldBot.Actions.Stop().ConfigureAwait(false);
                        }

                        var success =
                            await CreateOrUpdateBotDbFile(item.BotName, item.IdentitySecret, item.SharedSecret).ConfigureAwait(false);

                        result = new ImportResultData(item.BotName, success, success, success ? "Bot更新成功" : "更新Bot令牌信息失败");
                    }
                    else
                    {
                        result = new ImportResultData(item.BotName, true, oldBot.HasMobileAuthenticator, "Bot已存在, 无需更新");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(item.SteamLogin) && !string.IsNullOrEmpty(item.SteamPassword))
                {
                    var addDbSuccess = false;
                    if (!string.IsNullOrEmpty(item.SharedSecret))
                    {
                        addDbSuccess =
                     await CreateOrUpdateBotDbFile(item.BotName, item.IdentitySecret, item.SharedSecret).ConfigureAwait(false);
                    }

                    var addBotSuccess = await CreateBotConfigFile(item.BotName, item.SteamLogin, item.SteamPassword,
                            item.Enabled, item.Paused, BotConfig.EBotBehaviour.DismissInventoryNotifications).ConfigureAwait(false);

                    var success = addBotSuccess && addDbSuccess;

                    result = new ImportResultData(item.BotName, success, addDbSuccess, success ? "创建Bot成功" : "创建Bot失败");
                }
                else
                {
                    result = new ImportResultData(item.BotName, false, false, "缺少登录信息，无法创建Bot");
                }
            }

            importResult.TryAdd(item.BotName, result);
        }

        return Ok(new GenericResponse<Dictionary<string, ImportResultData>>(true, "导入结束", importResult));
    }
}