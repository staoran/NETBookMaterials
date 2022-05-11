using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Zack.Commons;
using Zack.JWT;

namespace FileService.SDK.NETCore
{
    /// <summary>
    /// 文件服务客户端，对外提供服务的SDK
    /// </summary>
    public class FileServiceClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly Uri serverRoot;
        private readonly JWTOptions optionsSnapshot;
        private readonly ITokenService tokenService;

        /// <summary>
        /// 构造函数，初始化时需提供所需服务和参数
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="serverRoot"></param>
        /// <param name="optionsSnapshot"></param>
        /// <param name="tokenService"></param>
        public FileServiceClient(IHttpClientFactory httpClientFactory, Uri serverRoot, JWTOptions optionsSnapshot, ITokenService tokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.serverRoot = serverRoot;
            this.optionsSnapshot = optionsSnapshot;
            this.tokenService = tokenService;
        }
        
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="sha256Hash"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task<FileExistsResponse> FileExistsAsync(long fileSize, string sha256Hash, CancellationToken stoppingToken = default)
        {
            string relativeUrl = FormattableStringHelper.BuildUrl($"api/Uploader/FileExists?fileSize={fileSize}&sha256Hash={sha256Hash}");
            Uri requestUri = new Uri(serverRoot, relativeUrl);
            var httpClient = httpClientFactory.CreateClient();
            return httpClient.GetJsonAsync<FileExistsResponse>(requestUri, stoppingToken)!;
        }

        /// <summary>
        /// 初始化一个 TOKEN
        /// </summary>
        /// <returns></returns>
        string BuildToken()
        {
            //因为JWT的key等机密信息只有服务器端知道，因此可以这样非常简单的读到配置
            Claim claim = new Claim(ClaimTypes.Role, "Admin");
            return tokenService.BuildToken(new Claim[] { claim }, optionsSnapshot);
        }

        /// <summary>
        /// 上传文件
        /// 封装自己的上传服务
        /// </summary>
        /// <param name="file"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Uri> UploadAsync(FileInfo file, CancellationToken stoppingToken = default)
        {
            string token = BuildToken();
            using MultipartFormDataContent content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenRead());
            content.Add(fileContent, "file", file.Name);
            var httpClient = httpClientFactory.CreateClient();
            Uri requestUri = new Uri(serverRoot + "/Uploader/Upload");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respMsg = await httpClient.PostAsync(requestUri, content, stoppingToken);
            if (!respMsg.IsSuccessStatusCode)
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                throw new HttpRequestException($"上传失败，状态码：{respMsg.StatusCode}，响应报文：{respString}");
            }
            else
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                return respString.ParseJson<Uri>()!;
            }
        }
    }
}
