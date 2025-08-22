namespace TodoApp.Models.Pagination;

public class BaseSpecification<T>
{
    public string? SearchTerm { get; set; }
    public Dictionary<string, string>? Filters { get; set; }
    public string? SortBy { get; set; }
    public bool? SortDescending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public virtual IEnumerable<string> GetSearchableProperties()
    {
        return typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => p.Name);
    }
}


