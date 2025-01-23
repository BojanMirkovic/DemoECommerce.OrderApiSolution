using AutoMapper;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Aplication.DTOs;
using OrderApi.Aplication.Interfaces;
using OrderApi.Aplication.Services;
using OrderApi.Domain.EntityModels;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IMapper mapper, IOrder orderInterface,IOrderServices orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No order detected in the database");
            }
            var orderList = mapper.Map<IEnumerable<OrderDTO>>(orders);
            if (orderList.Any()) 
            {
                return Ok(orderList);
            }
            return NotFound();           
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            //Check Model state if all annotation are valid
            if (!ModelState.IsValid) { return BadRequest(ModelState);}

            // Convert to Entity
            var getEntity = mapper.Map<OrderModel>(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            if (response.SuccessFlag)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order == null) { return NotFound("Order not found"); }
            return Ok(order);
        }
        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = mapper.Map<OrderModel>(orderDTO);
            var response = await orderInterface.UpadteAsync(order);
            if (response.SuccessFlag) { return Ok(response.Message); }
            return BadRequest(response.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            //Convert from DTO to Entity
            var order = mapper.Map<OrderModel>(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            if (response.SuccessFlag) { return Ok(response.Message); }
            return BadRequest(response.Message);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid data provided.");

            //var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            var orders = await orderService.GetOrdersByClientId(clientId);
            if (orders.Any()) 
            { 
                return Ok(orders);
            }
            return NotFound();
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid data provided.");

            var orderDetail = await orderService.GetOrderDetailsByOrderId(orderId);
            if (orderDetail.OrderId > 0)
            {
                return Ok(orderDetail);
            }
            return NotFound("No order found.");
        }
    }
}
