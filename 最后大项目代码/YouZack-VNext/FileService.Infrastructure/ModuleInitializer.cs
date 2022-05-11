using FileService.Domain;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace FileService.Infrastructure
{
    /// <summary>
    /// 当前项目需要注册的服务
    /// 通过实现 IModuleInitializer 接口，程序启动时会扫描并执行
    /// </summary>
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IStorageClient, SMBStorageClient>();
            //services.AddScoped<IStorageClient, UpYunStorageClient>();
            services.AddScoped<IStorageClient, MockCloudStorageClient>();
            services.AddScoped<IFSRepository, FSRepository>();
            services.AddScoped<FSDomainService>();
            services.AddHttpClient();
        }
    }
}
