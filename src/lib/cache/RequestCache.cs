
using ApiClient.Cache.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Cache
{
    public class RequestCache : IRequestCache
    {
        readonly IConfiguration _configuration;
        public RequestCache(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<bool> ContainsAsync(StructuredRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            
            var dbContext = await GetDbContextAsync();

            var matchedById = await dbContext
                                .Set<StructuredRequest>()
                                .FindAsync([request.Id], cancellationToken);

            if(matchedById is not null)
                return true;
                                
            var matchedResult = await dbContext
                .Set<StructuredRequest>()
                .AnyAsync(x => x.Command == request.Command, cancellationToken);

            return matchedResult;
        }

        /// <inheritdoc/>
        public async Task FlushAsync(CancellationToken cancellationToken)
        {
            var dbContext = await GetDbContextAsync();

            await dbContext.Database.EnsureDeletedAsync(cancellationToken);

            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<StructuredRequest> GetAsync(object id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id);

            var dbContext = await GetDbContextAsync();

            var result = await dbContext
                            .Set<StructuredRequest>()
                            .FindAsync([id], cancellationToken) ?? 
                                throw new InvalidOperationException(
                                    message: $"Could not locate record matching id:{id}.");
            return result;   
        }

        /// <inheritdoc/>
        public async Task<int> SetAsync(StructuredRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dbContext = await GetDbContextAsync();

            var existingRecord = await dbContext
                                    .Set<StructuredRequest>()
                                    .FindAsync([request.Id], cancellationToken);
            bool isUpdate = existingRecord is not null;

            if (isUpdate)
                dbContext.Entry(existingRecord!).CurrentValues.SetValues(request);
            else
                dbContext.Set<StructuredRequest>().Add(request);
            

            var count = await dbContext.SaveChangesAsync(cancellationToken);

            return count;
        }

        private async Task<RequestDbContext> GetDbContextAsync()
        {
            var dbContext = new RequestDbContext(_configuration);
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
