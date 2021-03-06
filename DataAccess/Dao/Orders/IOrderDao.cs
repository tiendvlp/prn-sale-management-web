using System;
using System.Linq;
using BusinessObject;

namespace DataAccess.Dao.Orders
{
    public interface IOrderDao : IDao<Order>
    {
        IQueryable<Order> GetWithFilter(DateTime startDate, DateTime endDate);
        void RemoveOrderContainsNoOrderDetails();
        void RemoveByMemberId(string id);
        IQueryable<Order> GetByMemberId(string id);
    }
}
