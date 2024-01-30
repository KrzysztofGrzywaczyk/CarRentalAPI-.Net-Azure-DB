using CarRentalAPI.Models.Queries;

namespace CarRentalAPI.Models.Pagination;

public class PagedQuery
{
    public string? SearchPhrase { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }

    public SortDirection? SortDirection { get; set; }
}
