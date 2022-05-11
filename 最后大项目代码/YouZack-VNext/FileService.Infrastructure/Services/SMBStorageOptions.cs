namespace FileService.Infrastructure.Services;
/// <summary>
/// 文件备份服务用到的实体类，保存了文件保存的根目录
/// 在项目启动时会初始化，从配置（环境变量）中读取后写入的
/// 一个强类型配置项，用于保存文件的根目录
/// </summary>
public class SMBStorageOptions
{
    /// <summary>
    /// 根目录
    /// </summary>
    public string WorkingDir { get; set; }///千万不要写成private set；会导致不注入。项目里除了实体类、ValueObject、DTO之外，别的类尽量不要写成private set;
}