using OnlineStoreServer.Repositories;
using OnlineStoreServer.Services;
using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;


namespace OnlineStoreServer.Services
{
    public class ClientService(HttpClient httpClient) : IProductService, ICategoryService
    {
        private const string ProductBaseUrl = "api/product";
        private const string CateogryBaseUrl = "api/category";

        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        
        public bool IsVisible { get; set; }
        public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; }


        // Related to Product Model Class
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var response = await httpClient.PostAsync(ProductBaseUrl, 
                General.GenerateStringContent(General.SerializeObj(model)));

            // Read Response by checking if it is successful
            var result = CheckResponse(response);
            if(!result.Flag) 
                return result;

            var apiResponse = await ReadContent(response);
            var data = General.DeserializeJsonString<ServiceResponse>(apiResponse);
            if(!data.Flag) return data;
            await ClearAndGetAllProducts();
            return data;
        }

        private async Task ClearAndGetAllProducts()
        {
            bool featuredProducts = true;
            bool allProducts = false;
            AllProducts = null!;
            FeaturedProducts = null!;
            await GetAllProducts(featuredProducts);
            await GetAllProducts(allProducts);
        }

        public async Task GetAllProducts(bool featuredProducts)
        {
            if(featuredProducts && FeaturedProducts is null)
            {
                IsVisible = true;
                FeaturedProducts = await GetProducts(featuredProducts);
                IsVisible = false;
                ProductAction?.Invoke();
                return;
            }
            else
            {
                if(!featuredProducts && AllProducts is null)
                {
                    IsVisible = true;
                    AllProducts = await GetProducts(featuredProducts);
                    IsVisible = false;
                    ProductAction?.Invoke();
                    return;
                }
            }
        }

        private async Task<List<Product>> GetProducts(bool featured)
        {
            var response = await httpClient.GetAsync($"{ProductBaseUrl}?featured={featured}");
            var (flag, message) = CheckResponse(response);
            if (!flag) return null!;

            var result = await ReadContent(response);
            return (List<Product>?)General.DeserializeJsonStringList<Product>(result)!;
        }

        public Product GetRandomProduct()
        {
            if (FeaturedProducts is null)
                return null!;

            Random RandomNumbers = new();
            int minimumNumber = FeaturedProducts.Min(rmp => rmp.Id);
            int maximumNumber = FeaturedProducts.Max(rmp => rmp.Id) + 1;
            int result = RandomNumbers.Next(minimumNumber, maximumNumber);
            return FeaturedProducts.FirstOrDefault(rmp => rmp.Id == result)!;
        }

        // Related to Category Model Class
        public async Task<ServiceResponse> AddCategory(Category model)
        {

            var response = await httpClient.PostAsync(CateogryBaseUrl,
                General.GenerateStringContent(General.SerializeObj(model)));

            var result = CheckResponse(response);
            if (!result.Flag)
                return result;

            var apiResponse = await ReadContent(response);

            var data = General.DeserializeJsonString<ServiceResponse>(apiResponse);
            if (!data.Flag) return data;
            await ClearAndGetAllCategories();
            return data;
        }
        public async Task GetAllCategories()
        {
            if (AllCategories is null)
            {
                var response = await httpClient.GetAsync($"{CateogryBaseUrl}");
                var (flag, _) = CheckResponse(response);
                if (!flag) return;

                var result = await ReadContent(response);
                AllCategories = (List<Category>?)General.DeserializeJsonStringList<Category>(result)!;
                CategoryAction?.Invoke();
            }
        }
        public async Task<Category[]> GetCategoriesAsync()
        {
            return await ExecutionOnContext(async () =>
            {
                // Replace this with your actual data fetching logic.
                var categories = await FetchCategoriesFromDatabase();
                return categories;
            });
        }
        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            Category? category = null;
            await ExecutionOnContext(async () =>
            {
                category = await FetchCategoryBySlugFromDatabase(slug);
                return null!;
            });
            return category;
        }
        private async Task<Category?> ExecutionOnSlugContext(Func<Task<Category?>> query)
        {
            return await query();
        }

        private async Task<Category?> FetchCategoryBySlugFromDatabase(string slug)
        {
            await GetAllCategories();
            var category = AllCategories.FirstOrDefault(c => c.Slug == slug);
            return category;
        }

        private async Task<Category[]> FetchCategoriesFromDatabase()
        {
            await GetAllCategories();
            AllCategories = AllCategories.ToList();
            return AllCategories.ToArray();
        }

        private async Task<Category[]> ExecutionOnContext(Func<Task<Category[]>> query)
        {
            return await query();
        }
        private async Task ClearAndGetAllCategories()
        {
            AllCategories = null!;
            await GetAllCategories();
        }

        // General Method
        private static async Task<string> ReadContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured. Try again later...");
            else
                return new ServiceResponse(true, null!);
        }

    }
}
