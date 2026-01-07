namespace ASFImportTools.Data.Plugin;

/// <summary>
/// 插件配置
/// </summary>
public sealed record PluginConfig
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eULA"></param>
    /// <param name="statistic"></param>
    public PluginConfig(bool eULA, bool statistic)
    {
        EULA = eULA;
        Statistic = statistic;
    }

    /// <summary>
    /// 是否同意使用协议
    /// </summary>
    public bool EULA { get; init; }

    /// <summary>
    /// 启用统计信息
    /// </summary>
    public bool Statistic { get; init; }
}