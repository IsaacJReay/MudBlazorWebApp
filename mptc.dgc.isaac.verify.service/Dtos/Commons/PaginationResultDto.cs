namespace mptc.dgc.isaac.verify.service.Dtos.Commons
{
    public class PaginationResultDto<T> : ResultDto<T>
    {
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public new IEnumerable<T> Data { get; set; } = null!;


        public PaginationResultDto(int pageSize, int totalCount, int pageNumber, IEnumerable<T> data)
        {
            PageSize = pageSize;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Data = data;
            Succeeded = true; // Assuming success, can be modified as needed
            StatusCode = 200; // OK status
        }
        public static PaginationResultDto<T> ToPaginateResult(int pageSize, int totalCount, int pageNumber, IEnumerable<T> data)
        {
            return new PaginationResultDto<T>(pageSize, totalCount, pageNumber, data);
        }
    }
}