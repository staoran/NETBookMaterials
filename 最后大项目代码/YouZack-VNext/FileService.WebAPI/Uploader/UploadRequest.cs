using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FileService.WebAPI.Uploader;
/// <summary>
/// 文件上传的请求参数和验证
/// </summary>
public class UploadRequest
{
    //不要声明为Action的参数，否则不会正常工作
    public IFormFile File { get; set; }
}
/// <summary>
/// 请求参数验证（用 fluentValidation 包）
/// </summary>
public class UploadRequestValidator : AbstractValidator<UploadRequest>
{
    public UploadRequestValidator()
    {
        //不用校验文件名的后缀，因为文件服务器会做好安全设置，所以即使用户上传exe、php等文件都是可以的
        long maxFileSize = 50 * 1024 * 1024;//最大文件大小
        // 文件不是空的，也不可超过最大文件大小
        RuleFor(e => e.File).NotNull().Must(f => f.Length > 0 && f.Length < maxFileSize);
    }
}