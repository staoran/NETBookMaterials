using FileService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zack.Commons;

namespace FileService.Domain;

/// <summary>
/// 文件领域服务层
/// </summary>
public class FSDomainService
{
    private readonly IFSRepository repository;
    private readonly IStorageClient backupStorage;//备份服务器
    private readonly IStorageClient remoteStorage;//文件存储服务器

    /// <summary>
    /// 注入文件仓储层，备份服务器，文件存储服务器
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="storageClients"></param>
    public FSDomainService(IFSRepository repository,
        IEnumerable<IStorageClient> storageClients)
    {
        this.repository = repository;
        //用这种方式可以解决内置DI不能使用名字注入不同实例的问题，而且从原则上来讲更加优美
        this.backupStorage = storageClients.First(c => c.StorageType == StorageType.Backup);
        this.remoteStorage = storageClients.First(c => c.StorageType == StorageType.Public);
    }

    //领域服务只有抽象的业务逻辑
    /// <summary>
    /// 领域层的业务逻辑，上传文件
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UploadedItem> UploadAsync(Stream stream, string fileName,
        CancellationToken cancellationToken)
    {
        // 计算文件的hash值
        string hash = HashHelper.ComputeSha256Hash(stream);
        long fileSize = stream.Length;
        DateTime today = DateTime.Today;
        //用日期把文件分散在不同文件夹存储，同时由于加上了文件hash值作为目录，又用用户上传的文件夹做文件名，
        //所以几乎不会发生不同文件冲突的可能
        //用用户上传的文件名保存文件名，这样用户查看、下载文件的时候，文件名更灵活
        string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";

        //查询是否有和上传文件的大小和SHA256一样的文件，如果有的话，就认为是同一个文件
        //虽然说前端可能已经调用FileExists接口检查过了，但是前端可能跳过了，或者有并发上传等问题，所以这里再检查一遍。
        var oldUploadItem = await repository.FindFileAsync(fileSize, hash);
        if (oldUploadItem != null)
        {
            return oldUploadItem;
        }
        //backupStorage实现很稳定、速度很快，一般都使用本地存储（文件共享或者NAS）
        // 备份用的 IStorageClient 实现
        Uri backupUrl = await backupStorage.SaveAsync(key, stream, cancellationToken);//保存到本地备份
        stream.Position = 0;
        // 对外使用的 IStorageClient 实现，一般是外部服务 如 OSS 七牛云 又拍云 或 自己实现的文件服务器
        Uri remoteUrl = await remoteStorage.SaveAsync(key, stream, cancellationToken);//保存到生产的存储系统
        stream.Position = 0;
        Guid id = Guid.NewGuid();
        //领域服务并不会真正的执行数据库插入，只是把实体对象生成，然后由应用服务和基础设施配合来真正的插入数据库！
        //DDD中尽量避免直接在领域服务中执行数据库的修改（包含删除、新增）操作。
        // 返回后由调用方自己去更新数据库
        return UploadedItem.Create(id, fileSize, fileName, hash, backupUrl, remoteUrl);// 生成 UploadedItem 实体对象
    }
}
