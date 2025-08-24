using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace TestWork.Data.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        string? sortField,
        string defaultSortField,
        bool ascending,
        int pageIndex,
        int pageSize)
    {
        if (pageIndex < 0)
            pageIndex = 0;

        if (pageSize <= 0)
            pageSize = 10;

        if (pageSize > 1000)
            pageSize = 1000;

        var total = await query.CountAsync();

        query = query.OrderBy(sortField, defaultSortField, ascending);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(items, items.Count);
        // return new PagedResult<T> { Items = items, Total = total };
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string? property, string defaultProperty,
        bool ascending)
    {
        var direction = ascending ? "asc" : "desc";

        if (string.IsNullOrEmpty(property) ||
            typeof(T).GetProperties().All(o => !o.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase)))
        {
            return source.OrderBy($"{defaultProperty} {direction}");
        }

        return source.OrderBy($"{property} {direction}");
    }
}