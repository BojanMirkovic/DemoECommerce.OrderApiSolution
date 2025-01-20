
using OrderApi.Aplication.DTOs;

namespace OrderApi.Aplication.Services
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId);
        Task<OrderDetailsDTO> GetOrderDetailsByOrderId(int orderId);
    }
}
