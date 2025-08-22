using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApp.Models.Pagination;

namespace TodoApp.Utiities;

public class QueryBuilder<T> where T : class
{
    private IQueryable<T> _query;
    private readonly BaseSpecification<T> _spec;

    public QueryBuilder(IQueryable<T> initialQuery, BaseSpecification<T> spec)
    {
        _query = initialQuery;
        _spec = spec;
    }


    public QueryBuilder<T> AddSearch()
    {
        if (string.IsNullOrEmpty(_spec.SearchTerm))
            return this;

        var searchTerm = _spec.SearchTerm.ToLower();
        var parameter = Expression.Parameter(typeof(T), "x");
        var searchableProperties = _spec.GetSearchableProperties()
            .Select(propName => typeof(T).GetProperty(propName))
            .Where(p => p != null);

        if (!searchableProperties.Any())
            return this;

        var expressions = new List<Expression>();

        foreach (var prop in searchableProperties)
        {
            if (prop == null) continue;

            var propExpr = Expression.Property(parameter, prop);
            Expression? searchExpression = null;

            if (prop.PropertyType == typeof(string))
            {
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                if (toLowerMethod != null && containsMethod != null)
                {
                    var nullCheck = Expression.NotEqual(propExpr, Expression.Constant(null, typeof(string)));
                    var toLowerExpr = Expression.Call(propExpr, toLowerMethod);
                    var containsExpr = Expression.Call(toLowerExpr, containsMethod, Expression.Constant(searchTerm));
                    searchExpression = Expression.AndAlso(nullCheck, containsExpr);
                }
            }
            else if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
            {
                if (Guid.TryParse(searchTerm, out Guid guidValue))
                {
                    var guidConstant = Expression.Constant(guidValue, prop.PropertyType);
                    searchExpression = Expression.Equal(propExpr, guidConstant);
                }
            }
            else if (prop.PropertyType == typeof(bool))
            {
                if (bool.TryParse(searchTerm, out bool boolValue))
                {
                    var boolConstant = Expression.Constant(boolValue);
                    searchExpression = Expression.Equal(propExpr, boolConstant);
                }
            }

            if (searchExpression != null)
            {
                expressions.Add(searchExpression);
            }
        }

        if (expressions.Any())
        {
            var combinedExpression = expressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            _query = _query.Where(lambda);
        }

        return this;
    }

    public QueryBuilder<T> AddFilters()
    {
        if (_spec.Filters == null || _spec.Filters.Count == 0)
            return this;

        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var filter in _spec.Filters)
        {
            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, filter.Key, StringComparison.OrdinalIgnoreCase));
            if (property == null) continue;

            try
            {
                var propertyExpr = Expression.Property(parameter, property);
                var convertedValue = Convert.ChangeType(filter.Value, property.PropertyType);

                Expression constantExpr;
                if (property.PropertyType == typeof(string))
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)
                        ?? throw new InvalidOperationException("ToLower method not found");
                    var toLowerExpr = Expression.Call(Expression.Constant(filter.Value), toLowerMethod);
                    constantExpr = Expression.Constant(filter.Value.ToString().ToLower());
                    var toLowerPropertyExpr = Expression.Call(propertyExpr, toLowerMethod);
                    var equalExpr = Expression.Equal(toLowerPropertyExpr, constantExpr);
                    var lambda = Expression.Lambda<Func<T, bool>>(equalExpr, parameter);
                    _query = _query.Where(lambda);
                }
                else
                {
                    constantExpr = Expression.Constant(convertedValue);
                    var equalExpr = Expression.Equal(propertyExpr, constantExpr);
                    var lambda = Expression.Lambda<Func<T, bool>>(equalExpr, parameter);
                    _query = _query.Where(lambda);
                }
            }
            catch
            {
                continue;
            }
        }

        return this;
    }

    public QueryBuilder<T> AddSort()
    {
        if (string.IsNullOrEmpty(_spec.SortBy))
            return this;

        var property = typeof(T).GetProperties()
            .FirstOrDefault(p => string.Equals(p.Name, _spec.SortBy, StringComparison.OrdinalIgnoreCase));
        if (property == null)
            return this;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpr = Expression.Property(parameter, property);
        var lambdaType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
        var lambda = Expression.Lambda(propertyExpr, parameter);

        var methodName = _spec.SortDescending.GetValueOrDefault() ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable)
            .GetMethods()
            .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2)
            ?? throw new InvalidOperationException($"Sort method {methodName} not found");

        var genericMethod = method.MakeGenericMethod(typeof(T), property.PropertyType);

        var result = genericMethod.Invoke(null, new object[] { _query, lambda }) ?? throw new InvalidOperationException("Sort operation returned null");
        _query = (IQueryable<T>)result;
        return this;
    }

    public async Task<PagedResponse<T>> BuildAsync()
    {
        if (_query == null)
            throw new InvalidOperationException("Query is not initialized");

        if (_spec.Page < 1) _spec.Page = 1;
        if (_spec.PageSize < 1) _spec.PageSize = 10;

        try
        {
            var totalItems = await _query.CountAsync();

            var items = await _query
                .Skip((_spec.Page - 1) * _spec.PageSize)
                .Take(_spec.PageSize)
                .ToListAsync();

            return new PagedResponse<T>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)_spec.PageSize),
                CurrentPage = _spec.Page
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to execute pagination query", ex);
        }
    }
}