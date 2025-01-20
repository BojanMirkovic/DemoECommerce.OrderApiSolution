using eCommerce.SharedLibrary.LogFolder;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderApi.Aplication.Interfaces;
using OrderApi.Domain.EntityModels;
using OrderApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(OrderModel entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (order.Id > 0) 
                {
                    return new Response(true, "Order placed successfully.");
                }
                return new Response(false, "Error occurred while placing order");
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                return new Response(false, "Error occured while placing order");
            }
        }

        public async Task<Response> DeleteAsync(OrderModel entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null) 
                {
                    return new Response(false, $"Oreder with given Id: {entity.Id} not found");
                }
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, $"Order with Id: {entity.Id} is successfully deleted.");
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                return new Response(false, "Error occured while deleting order");
            }
        }

        public async Task<OrderModel?> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                return order;
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                throw new Exception("An error occurred while retrieving the order.");

            }
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                if(orders.IsNullOrEmpty())
                {
                    return Enumerable.Empty<OrderModel>();
                }
                return orders;
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                throw new Exception("An error occurred while retrieving the orders.");
            }
        }

        public async Task<OrderModel?> GetByAsync(Expression<Func<OrderModel, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.AsNoTracking().FirstOrDefaultAsync(predicate);
                return order;
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                throw new Exception("An error occurred while retrieving the order.");
            }
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersAsync(Expression<Func<OrderModel, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders;

            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                throw new Exception("An error occurred while retrieving the orders.");
            }
        }

        public async Task<Response> UpadteAsync(OrderModel entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order == null)
                {
                    return new Response(false,"Order not found");
                }
                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order updated.");
            }
            catch (Exception ex)
            {
                //Log original exception 
                LogException.LogExceptions(ex);

                //Display scery-free message to client
                return new Response(false, "Error occured while updating the order");
            }
        }
    }
}
