using FileService.Domain;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 用局域网内共享文件夹或者本机磁盘当备份服务器的实现类
    /// </summary>
    class SMBStorageClient : IStorageClient
    {
        private IOptionsSnapshot<SMBStorageOptions> options;
        /// <summary>
        /// 注入配置，SMBStorageOptions格式的强类型配置
        /// </summary>
        /// <param name="options"></param>
        public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// 定义当前实现类的类型
        /// </summary>
        public StorageType StorageType => StorageType.Backup;

        /// <summary>
        /// 保存的实现
        /// 一个本地保存方案
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
        {
            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }
            string workingDir = options.Value.WorkingDir;
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);//get the directory
            if (!Directory.Exists(fullDir))//automatically create dir
            {
                Directory.CreateDirectory(fullDir);
            }
            if (File.Exists(fullPath))//如果已经存在，则尝试删除
            {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await content.CopyToAsync(outStream, cancellationToken);
            return new Uri(fullPath);
        }
    }
}
