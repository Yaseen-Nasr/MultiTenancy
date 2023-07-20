namespace MultiTenancy.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(CreateProductDto dto)
        {
            Product product = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                Rate = dto.Rate,
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
         }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
