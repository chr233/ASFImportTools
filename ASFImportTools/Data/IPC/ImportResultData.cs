namespace ASFImportTools.Data.IPC;

public sealed record ImportResultData(string BotName, bool Success,bool Has2Fa, string? Message);

