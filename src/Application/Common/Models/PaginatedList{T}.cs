namespace TaskManagement.Application.Common.Models;

public class PaginatedList<T>
{
    private PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public List<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        int totalCount = source.Count();
        List<T> items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PaginatedList<T>(items, totalCount, pageNumber, pageSize));
    }
}