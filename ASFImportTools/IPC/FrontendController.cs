using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASFImportTools.IPC;

/// <summary>
/// 测试相关接口
/// </summary>
public sealed class FrontendController : ArchiController
{
    private const string ApiUrl = "https://import.chrxw.com";

    /// <summary>
    /// 获取页面
    /// </summary>
    /// <returns></returns>
    [HttpGet("/ImportGeneric")]
    [EndpointSummary("ASFImportTools 前端, 仅限 Generic 版本")]
    public async Task<ActionResult> Index()
    {
        if (ASF.WebBrowser == null)
        {
            return Ok("内部错误");
        }

        if (!Config.EULA)
        {
            return Ok("请设置 ASFEnhance.EULA");
        }

        try
        {
            var request = new Uri(ApiUrl);

            var response = await ASF.WebBrowser.UrlGetToBinary(request).ConfigureAwait(false);
            if (response?.Content == null || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Ok("请求失败");
            }

            var bytes = response.Content.ToArray();
            var payload = System.Text.Encoding.UTF8.GetString(bytes);
            return Content(payload, "text/html; charset=utf-8");
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericWarningException(ex);
            ASFLogger.LogGenericError("Request failed");
            return Ok(Langs.NetworkError);
        }
    }
}