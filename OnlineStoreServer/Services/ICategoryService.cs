using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;

namespace OnlineStoreServer.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction { get; set; }
        Task<ServiceResponse> AddCategory(Category model);
        Task GetAllCategories();
        List<Category> AllCategories { get; set; }
        Task<Category[]> GetCategoriesAsync();
        Task<Category?> GetCategoryBySlugAsync(string slug);
    }
}
