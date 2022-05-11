using FileService.Domain;
using FileService.Domain.Entities;

namespace FileService.Infrastructure
{
    /// <summary>
    /// 文件服务的仓储接口实现
    /// </summary>
    class FSRepository : IFSRepository
    {
        private readonly FSDbContext dbContext;

        public FSRepository(FSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 在数据库中查找是否存在相同的文件记录，通过文件大小和哈希值
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="sha256Hash"></param>
        /// <returns></returns>
        public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
        {
            return dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
            && u.FileSHA256Hash == sha256Hash);
        }
    }
}
