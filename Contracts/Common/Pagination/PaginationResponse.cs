namespace Contracts.Common.Pagination;

public record PaginationResponse<T>(
    IEnumerable<T> Data,
    int TotalRecords,
    int PageNumber,
    int PageSize);