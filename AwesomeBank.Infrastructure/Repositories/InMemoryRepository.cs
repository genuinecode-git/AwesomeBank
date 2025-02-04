namespace AwesomeBank.Infrastructure.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly ConcurrentDictionary<string, T> _store = new();
    private readonly Func<T, string> _keySelector;

    public InMemoryRepository(Func<T, string> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }

    public IQueryable<T> GetAll()
    {
        return _store.Values.AsQueryable();
    }

    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return _store.Values.AsQueryable().FirstOrDefault(predicate);
    }

    public T FirstOrDefaultWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = _store.Values.AsQueryable();

        // Simulating EF Core's Include behavior (in-memory)
        foreach (var include in includes)
        {
            foreach (var entity in query)
            {
                var navigationProperty = include.Compile()(entity);
            }
        }

        return query.FirstOrDefault(predicate);
    }

    public void Add(T entity)
    {
        var key = _keySelector(entity);
        if (string.IsNullOrEmpty(key)) throw new ArgumentException("Invalid entity key.");
        _store[key] = entity;
    }

    public void Update(T entity)
    {
        var key = _keySelector(entity);
        if (string.IsNullOrEmpty(key)) throw new ArgumentException("Invalid entity key.");
        if (_store.ContainsKey(key))
        {
            _store[key] = entity;
        }
        else
        {
            throw new InvalidOperationException("Entity not found.");
        }
    }

    public void Remove(T entity)
    {
        var key = _keySelector(entity);
        if (string.IsNullOrEmpty(key)) throw new ArgumentException("Invalid entity key.");

        if (_store.ContainsKey(key))
        {
            _store.TryRemove(key, out _);
        }
        else
        {
            throw new KeyNotFoundException("Entity not found.");
        }
    }
}