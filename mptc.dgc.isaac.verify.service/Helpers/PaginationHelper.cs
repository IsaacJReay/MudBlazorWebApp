using Microsoft.EntityFrameworkCore;
using mptc.dgc.isaac.verify.service.Dtos.Commons;

namespace mptc.dgc.isaac.verify.service.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PaginationResultDto<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            var totalCount = await source.CountAsync(cancellationToken: cancellationToken);
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var data = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginationResultDto<T>.ToPaginateResult(pageSize, totalCount, pageNumber, data);
        }
    }
}