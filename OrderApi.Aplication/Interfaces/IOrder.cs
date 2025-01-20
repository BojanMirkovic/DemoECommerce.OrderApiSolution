using eCommerce.SharedLibrary.Interface;
using OrderApi.Domain.EntityModels;
using System.Linq.Expressions;

namespace OrderApi.Aplication.Interfaces
{
    public interface IOrder : IGenericInterface<OrderModel>
    {
        Task<IEnumerable<OrderModel>> GetOrdersAsync(Expression<Func<OrderModel, bool>> predicate);
    }
}
