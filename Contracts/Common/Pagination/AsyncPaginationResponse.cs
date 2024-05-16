namespace Contracts.Common.Pagination;

public record AsyncPaginationResponse<T>(
    IAsyncEnumerable<T> Data,
    long TotalRecords,
    int PageNumber,
    int PageSize);