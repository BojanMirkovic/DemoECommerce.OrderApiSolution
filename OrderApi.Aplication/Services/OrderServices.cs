using AutoMapper;
using OrderApi.Aplication.DTOs;
using OrderApi.Aplication.Interfaces;
using Polly.Registry;
using System.Net.Http.Json;


namespace OrderApi.Aplication.Services
{
    public class OrderServices(IOrder orderInterface,HttpClient httpClient, IMapper _mapper,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderServices
    {
        //GET PRODUCT
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //Redirect this callto the API Gateway since product Api is not response to outsiders
            //Call Product Api using HttpClient
            //var getProduct = await httpClient.GetAsync($"/api/{productId}");
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode) return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //GET USER
        public async Task<AppUserDTO> GetUser(int userId)
        {
            //Redirect this callto the API Gateway since product Api is not response to outsiders
            //Call Product Api using HttpClient
            var getUser = await httpClient.GetAsync($"/api/{userId}");
            if (!getUser.IsSuccessStatusCode) return null!;

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        //GET ORDER DETAILS BY ID
        public async Task<OrderDetailsDTO> GetOrderDetailsByOrderId(int orderId)
        {
            //Prepare Oreder
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order.Id <= 0)
            {
                return null!;
            }
            // Get Retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            //Prepare Product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //Prepare Client 
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            //Populate Order Details
            return new OrderDetailsDTO(
                 //order.Id,
                 //productDTO.Id,
                 //appUserDTO.Id,
                 //appUserDTO.Email,             
                 //appUserDTO.Address,               
                 //appUserDTO.TelephoneNumber,
                 //appUserDTO.Name,
                 //productDTO.Name,
                 //order.PurchaseQuantity,
                 //productDTO.Price,
                 //productDTO.Quantity * order.PurchaseQuantity,
                 //order.OrderDate
                 order.Id,
                 productDTO?.Id ?? 0,
                 appUserDTO?.Id ?? 0,
                 appUserDTO?.Email ?? "Unknown",
                 appUserDTO?.Address ?? "Unknown",
                 appUserDTO?.TelephoneNumber ?? "Unknown",
                 appUserDTO?.Name ?? "Unknown",
                 productDTO?.Name ?? "Unknown",
                 order.PurchaseQuantity,
                 productDTO?.Price ?? 0.0m,
                 (productDTO?.Price ?? 0.0m) * order.PurchaseQuantity,
                 order.OrderDate
            );
        }

        //GET ORDERS BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            //Get all orders for the client
            var clientOrders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!clientOrders.Any()) return null!;

            //Convert from Entity to DTO
            var orders = _mapper.Map<IEnumerable<OrderDTO>>(clientOrders);

            return orders;
        }
    }
}
