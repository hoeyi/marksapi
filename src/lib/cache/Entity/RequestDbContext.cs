using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiClient.Cache.Entity
{
    public class RequestDbContext : DbContext
    {
        readonly IConfiguration _configuration;
        public RequestDbContext(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _configuration = configuration;
        }

        public RequestDbContext(
            IConfiguration configuration,
            DbContextOptions<RequestDbContext> options)
            : base(options)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _configuration = configuration;
        }

        public DbSet<StructuredRequest> CachedRequests => Set<StructuredRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StructuredRequestConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(!options.IsConfigured)
                options.UseSqlite(_configuration.GetConnectionString("SQLite"));
        }
    }
}