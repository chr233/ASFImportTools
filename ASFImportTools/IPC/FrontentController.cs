using ArchiSteamFarm.IPC.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASFImportTools.IPC;

/// <summary>
/// 测试相关接口
/// </summary>
public sealed class FrontentController : ArchiController
{
    /// <summary>
    /// 获取账号信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/Import")]
    [HttpPost("/Import")]
    [EndpointSummary("获取账号信息")]
    public ActionResult Index()
    {
        return Content("<h1>ASF Import Tools IPC Module</h1>");
    }
}