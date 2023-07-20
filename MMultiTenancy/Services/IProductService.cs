namespace MultiTenancy.Services
{
    public interface IProductService
    {
           Task<Product?> GetByIdAsync(int id);
           Task<Product> CreateAsync(CreateProductDto dto);
          Task<IReadOnlyList<Product>> GetAllAsync();


    }
}
