using System;

namespace FileService.SDK.NETCore
{
    /// <summary>
    /// 文件服务SDK的返回结果，是否存在和文件地址
    /// </summary>
    /// <param name="IsExists"></param>
    /// <param name="Url"></param>
    public record FileExistsResponse(bool IsExists, Uri? Url);
}
