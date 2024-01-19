namespace IdiomasLIB.Interfaces;

public interface ICRUDService<TypeT> where TypeT : class
{
    Task<int> CreateAsync(TypeT type);
    Task CreateAllAsync(IEnumerable<TypeT> types);
    Task<IQueryable<TypeT>> GetAllAsync(int? unit = default);
    Task<TypeT> GetAsync(int id);
    Task EditAsync(TypeT type);
    Task DeleteAsync(TypeT type);
}
