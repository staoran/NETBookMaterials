
namespace FileService.WebAPI.Uploader;

/// <summary>
/// 文件上传后的响应结果
/// </summary>
/// <param name="IsExists">是否存在这样的文件</param>
/// <param name="Url">如果存在，则Url代表这个文件的路径</param>
public record FileExistsResponse(bool IsExists, Uri? Url);