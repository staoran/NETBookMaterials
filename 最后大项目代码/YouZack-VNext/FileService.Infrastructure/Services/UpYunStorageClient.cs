using FileService.Domain;
using Microsoft.Extensions.Options;
using System.Net.Http;
using UpYun.NETCore;
using Zack.Commons;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 又拍云存储服务
    /// </summary>
    class UpYunStorageClient : IStorageClient
    {
        private IOptionsSnapshot<UpYunStorageOptions> options;
        private IHttpClientFactory httpClientFactory;
        /// <summary>
        /// 注入 又拍云配置 的强类型配置 和 Http 客户端工厂
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpClientFactory"></param>
        public UpYunStorageClient(IOptionsSnapshot<UpYunStorageOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 定义当前实现的类型
        /// </summary>
        public StorageType StorageType => StorageType.Public;

        /// <summary>
        /// 生成 url
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        static string ConcatUrl(params string[] segments)
        {
            // 检查数组中的每一个元素，并去除开头结尾的/
            for (int i = 0; i < segments.Length; i++)
            {
                string s = segments[i];
                if (s.Contains(".."))
                {
                    throw new ArgumentException("路径中不能含有..");
                }
                segments[i] = s.Trim('/');//把开头结尾的/去掉
            }
            // 用指定的符号连接数组中的元素
            return string.Join('/', segments);
        }

        /// <summary>
        /// 又拍云的文件保存方法
        /// </summary>
        /// <param name="key">文件保存路径/文件名</param>
        /// <param name="content">文件流</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
        {
            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }
            byte[] bytes = content.ToArray();
            if (bytes.Length <= 0)
            {
                throw new ArgumentException("file cannot be empty", nameof(content));
            }
            string bucketName = options.Value.BucketName;
            string userName = options.Value.UserName;
            string password = options.Value.Password;
            string pathRoot = options.Value.WorkingDir;

            string url = ConcatUrl(options.Value.UrlRoot, pathRoot, key);//web访问的文件网址
            string fullPath = "/" + ConcatUrl(pathRoot, key);//又拍云的上传路径
            // 又拍云 SDK 实例
            UpYunClient upyun = new UpYunClient(bucketName, userName, password, httpClientFactory);
            var upyunResult = await upyun.WriteFileAsync(fullPath, bytes, true, cancellationToken);
            if (upyunResult.IsOK == false)
            {
                throw new HttpRequestException("uploading to upyun failed:" + upyunResult.Msg);
            }
            return new Uri(url);
        }
    }
}
