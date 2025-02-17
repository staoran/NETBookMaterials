﻿namespace FileService.Infrastructure.Services;

/// <summary>
/// 又拍云接口配置项
/// 配合程序加载时读取的配置项成为一个强类型配置项
/// </summary>
public class UpYunStorageOptions
{
    public string BucketName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    /// <summary>
    /// 上传的根目录
    /// </summary>
    public string WorkingDir { get; set; }

    /// <summary>
    /// http(s)://等这样开头的Url的根目录
    /// </summary>
    public string UrlRoot { get; set; }
}