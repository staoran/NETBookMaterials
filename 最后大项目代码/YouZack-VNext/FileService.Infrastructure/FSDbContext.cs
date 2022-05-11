using FileService.Domain.Entities;

namespace FileService.Infrastructure
{
    /// <summary>
    /// 文件服务的数据库上下文
    /// </summary>
    public class FSDbContext : BaseDbContext
    {
        public DbSet<UploadedItem> UploadItems { get; private set; }

        public FSDbContext(DbContextOptions<FSDbContext> options, IMediator mediator)
            : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
