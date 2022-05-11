using CommonInitializer;
using FileService.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
// ��װ�Ĺ����ķ���ע��
// ���ݿ�����
builder.ConfigureDbConfiguration();
// ��ʼ��һЩ�����ķ���
builder.ConfigureExtraServices(new InitializerOptions
{
    // ��־�ļ���ַ
    LogFilePath = "e:/temp/FileService.log",
    // �¼���������
    EventBusQueueName = "FileService.WebAPI",
});

// Add services to the container.
// �����ļ������������ó�Ϊǿ����������
builder.Services//.AddOptions() //asp.net core��Ŀ��AddOptions()��дҲ�У���Ϊ���һ���Զ�ִ����
    .Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"))
    .Configure<UpYunStorageOptions>(builder.Configuration.GetSection("FileService:UpYun"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FileService.WebAPI", Version = "v1" });
});

// ����������ɣ���ʼ���� WebHost web�ܵ�
var app = builder.Build();

// �������м��������
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
