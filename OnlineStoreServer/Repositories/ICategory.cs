using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;

namespace OnlineStoreServer.Repositories
{
    public interface ICategory
    {
        Task<ServiceResponse> AddCategory(Category model);
        Task<List<Category>> GetAllCategories();
    }
}
