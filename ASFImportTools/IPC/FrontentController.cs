using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASFImportTools.IPC;

/// <summary>
/// 测试相关接口
/// </summary>
public sealed class FrontentController : ArchiController
{
    private static HttpClient? _httpClient;

    internal static void SetupApi()
    {
        var apiUrl = new Uri("https://import.chrxw.com");

        _httpClient = ASF.WebBrowser!.GenerateDisposableHttpClient(true);
        _httpClient.BaseAddress = apiUrl;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<string?> GetToString(string path, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
        {
            return null;
        }

        var response = await _httpClient.GetAsync(path, cancellationToken).ConfigureAwait(false);
        try
        {
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            var payload = System.Text.Encoding.UTF8.GetString(bytes);
            return payload;
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericWarningException(ex);
            ASFLogger.LogGenericError("Request failed");
            return null;
        }
    }

    /// <summary>
    /// 获取账号信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/Import")]
    [EndpointSummary("获取账号信息")]
    public async Task<ActionResult> Index()
    {
        if (_httpClient == null)
        {
            return Ok("");
        }

        var response = await GetToString("/").ConfigureAwait(false);
        if (!string.IsNullOrEmpty(response))
        {
            return Content(response, "text/html; charset=utf-8");
        }
        else
        {
            return BadRequest(Langs.NetworkError);
        }
    }

    //[HttpGet("/Import/_app/")]
    //public ActionResult Resource(string path)
    //{
    //    if (_httpClient == null)
    //    {
    //        return Ok("");
    //    }

    //    var response = await GetToString("/").ConfigureAwait(false);
    //    return Content(response ?? Langs.NetworkError);
    //}
}