using FileService.Domain;
using FileService.Domain.Entities;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Zack.ASPNETCore;

namespace FileService.WebAPI.Uploader;

/// <summary>
/// 文件上传 控制器
/// 应用服务层
/// </summary>
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")] // 只有管理员才能访问
[UnitOfWork(typeof(FSDbContext))]
//todo：要做权限控制，这个接口即对内部系统开放、又对前端用户开放。
public class UploaderController : ControllerBase
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    private readonly FSDbContext dbContext;
    /// <summary>
    /// 领域服务
    /// </summary>
    private readonly FSDomainService domainService;
    /// <summary>
    /// 数据仓储
    /// </summary>
    private readonly IFSRepository repository;
    /// <summary>
    /// 构造函数，注入文件领域服务，文件数据库上下文，文件仓储服务
    /// </summary>
    /// <param name="domainService">文件领域服务</param>
    /// <param name="dbContext">文件数据库上下文</param>
    /// <param name="repository">文件仓储服务</param>
    public UploaderController(FSDomainService domainService, FSDbContext dbContext, IFSRepository repository)
    {
        this.domainService = domainService;
        this.dbContext = dbContext;
        this.repository = repository;
    }

    /// <summary>
    /// 检查是否有和指定的大小和SHA256完全一样的文件
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
    {
        var item = await repository.FindFileAsync(fileSize, sha256Hash);
        if (item == null)
        {
            return new FileExistsResponse(false, null);
        }
        else
        {
            // 有，返回地址
            return new FileExistsResponse(true, item.RemoteUrl);
        }
    }

    //todo: 做好校验，参考OSS的接口，防止被滥用
    //todo：应该由应用服务器向fileserver申请一个上传码（可以指定申请的个数，这个接口只能供应用服务器调用），
    //页面直传只能使用上传码上传一个文件，防止接口被恶意利用。应用服务器要控制发放上传码的频率。
    //todo：再提供一个非页面直传的接口，供服务器用
    /// <summary>
    /// 文件上传接口
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [RequestSizeLimit(60_000_000)] // 设置请求大小限制，60M
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
    {
        var file = request.File;
        string fileName = file.FileName;
        using Stream stream = file.OpenReadStream();
        // 领域服务中的上传方法，返回一个文件上传实体
        // 领域服务中不操作数据库，只是返回一个文件上传实体
        // 领域服务并不会真正的执行数据库插入，只是把实体对象生成，然后由应用服务和基础设施配合来真正的插入数据库！
        UploadedItem upItem = await domainService.UploadAsync(stream, fileName, cancellationToken);
        // 传入数据库
        dbContext.Add(upItem);
        return upItem.RemoteUrl;// 返回下载的url
    }
}
