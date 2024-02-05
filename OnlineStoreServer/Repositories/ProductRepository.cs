using Microsoft.EntityFrameworkCore;
using OnlineStoreApiBlazor.Data;
using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;

namespace OnlineStoreServer.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationDbContext appDbContext;
        public ProductRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            if (model is null) return new ServiceResponse(false, "Model is null.");
            var (flag, message) = await CheckName(model.Name!);
            if (flag)
            {
                appDbContext.Products.Add(model);
                await Commit();
                return new ServiceResponse(true, "Product saved.");
            }
            return new ServiceResponse(flag, message!);
        }
        public async Task<List<Product>> GetAllProducts(bool featuredProducts)
        {
            if (featuredProducts)
                return await appDbContext.Products.Where(x => x.Featured).ToListAsync();
            else
                return await appDbContext.Products.ToListAsync();
        }
        private async Task<ServiceResponse> CheckName(string name)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(x => x.Name!.ToLower()!.Equals(name.ToLower()));
            return product is null ? new ServiceResponse(true, null!) : new ServiceResponse(false, "Product already existed.");
        }
        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
