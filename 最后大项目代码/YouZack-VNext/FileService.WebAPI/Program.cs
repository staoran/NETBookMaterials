using CommonInitializer;
using FileService.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
// 封装的公共的服务注册
// 数据库配置
builder.ConfigureDbConfiguration();
// 初始化一些公共的服务
builder.ConfigureExtraServices(new InitializerOptions
{
    // 日志文件地址
    LogFilePath = "e:/temp/FileService.log",
    // 事件总线名称
    EventBusQueueName = "FileService.WebAPI",
});

// Add services to the container.
// 加载文件服务的相关配置成为强类型配置项
builder.Services//.AddOptions() //asp.net core项目中AddOptions()不写也行，因为框架一定自动执行了
    .Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"))
    .Configure<UpYunStorageOptions>(builder.Configuration.GetSection("FileService:UpYun"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FileService.WebAPI", Version = "v1" });
});

// 以上配置完成，开始构建 WebHost web管道
var app = builder.Build();

// 以下是中间件的配置
// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}
app.UseStaticFiles();
app.UseZackDefault();

app.MapControllers();

app.Run();
