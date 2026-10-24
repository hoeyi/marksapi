using Microsoft.EntityFrameworkCore;

namespace ApiClient.Cache.Entity
{
    public class RequestDbContext : DbContext
    {
        public RequestDbContext(DbContextOptions<RequestDbContext> options)
            : base(options)
        {
        }

        public DbSet<StructuredRequest> StructuredRequests => Set<StructuredRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StructuredRequestConfiguration());
        }
    }
}