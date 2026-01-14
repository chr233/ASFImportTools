# ASF Import Tools

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电][afdian_img]][afdian_link]
[![buy me a coffee][bmac_img]][bmac_link]

## EULA

> 请不要使用本插件来进行不受欢迎的行为, 包括但不限于: 刷好评, 发布广告 等.
>
> 详见 [插件配置说明](#插件配置说明)

## 安装方式

### 初次安装 / 手动更新

1. 从 [GitHub Releases](https://github.com/chr233/ASFImportTools/releases) 下载插件的最新版本
2. 解压后将 `ASFImportTools.dll` 丢进 `ArchiSteamFarm` 目录下的 `plugins` 文件夹
3. 重新启动 `ArchiSteamFarm` , 使用命令 `ASFIMPORTTOOLS` 或 `AIT` 来检查插件是否正常工作

### 捐赠

|               ![img][afdian_qr]                |                   ![img][bmac_qr]                   |                       ![img][usdt_qr]                       |
| :--------------------------------------------: | :-------------------------------------------------: | :---------------------------------------------------------: |
| ![爱发电][afdian_img] <br> [链接][afdian_link] | ![buy me a coffee][bmac_img] <br> [链接][bmac_link] | ![USDT][usdt_img] <br> `TW41eecZ199QK6zujgKP4j1cz2bXzRus3c` |

[afdian_qr]: https://raw.chrxw.com/chr233/master/afadian_qr.png
[afdian_img]: https://img.shields.io/badge/爱发电-@chr__-ea4aaa.svg?logo=github-sponsors
[afdian_link]: https://afdian.net/@chr233
[bmac_qr]: https://raw.chrxw.com/chr233/master/bmc_qr.png
[bmac_img]: https://img.shields.io/badge/buy%20me%20a%20coffee-@chr233-yellow?logo=buymeacoffee
[bmac_link]: https://www.buymeacoffee.com/chr233
[usdt_qr]: https://raw.chrxw.com/chr233/master/usdt_qr.png
[usdt_img]: https://img.shields.io/badge/USDT-TRC20-2354e6.svg?logo=bitcoin

### 更新日志

| ASFImportTools 版本                                                      | 适配 ASF 版本 | 更新说明       |
| ------------------------------------------------------------------------ | :-----------: | -------------- |
| [1.0.1.0](https://github.com/chr233/ASFImportTools/releases/tag/1.0.1.0) |    6.3.1.6    | ASF -> 6.3.1.6 |
| [1.0.0.0](https://github.com/chr233/ASFImportTools/releases/tag/1.0.0.0) |    6.3.0.2    | 第一个版本     |

## 插件配置说明

> 需要设置 ASFEnhance.EULA = true

ASF.json

```json
{
  //ASF 配置
  "CurrentCulture": "...",
  "IPCPassword": "...",
  "...": "...",
  //Asf Award Tool 配置
  "ASFEnhance": {
    "EULA": true,
    "Statistic": true
  }
}
```

| 配置项      | 类型   | 默认值  | 说明                                                               |
| ----------- | ------ | ------- | ------------------------------------------------------------------ |
| `EULA`      | `bool` | `false` | 是否同意 [EULA](#eula)                                             |
| `Statistic` | `bool` | `false` | 是否允许发送统计数据, 仅用于统计插件用户数量, 不会发送任何其他信息 |

## 插件指令说明

| 命令             | 缩写  | 权限            | 说明                       |
| ---------------- | ----- | --------------- | -------------------------- |
| `ASFIMPORTTOOLS` | `AAT` | `FamilySharing` | 查看 ASF Award Tool 的版本 |

## 网页导入

访问 `http://localhost:1242/import`, (假设你的 ASF IPC 接口为 `http://localhost:1242`)

> PS:
>
> 前端托管在 `https://import.page.chrxw.com` 上, 中国可以访问镜像站 `https://import.chrxw.com`
>
> 账号信息和令牌信息都在前端本地处理, 调用本地的插件接口, 账号信息不会上传到其他地方
>
> 前端的源码 [参见](https://github.com/chr233/asf-import-tools)

![botList](./docs/botList.png)

![import](./docs/import.png)
