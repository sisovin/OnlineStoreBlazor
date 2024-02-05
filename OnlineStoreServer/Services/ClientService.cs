using OnlineStoreServer.Repositories;
using OnlineStoreServer.Services;
using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;


namespace OnlineStoreServer.Services
{
    public class ClientService(HttpClient httpClient) : IProductService
    {
        private const string ProductBaseUrl = "api/product";

        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        
        public bool IsVisible { get; set; }
       

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
