using System;
using System.Collections.Generic;
using BusinessObject;

namespace DataAccess.repositories.Orders
{
    public interface IOrderRepository
    {
        public Order Add(string memberId, DateTime orderDate, DateTime requiredDate, DateTime shippedDate, double freight);
        List<Order> GetWithFilter(DateTime startDate, DateTime endDate);
        void RemoveById(string id);
        Order GetById(string id);
        void Update(string orderId, string memberId, DateTime orderDate, DateTime requiredDate, DateTime shippedDate, double freight);
        void RemoveOrderContainsNoOrderDetails();
        void RemoveByMemberId(string id);
        List<Order> GetByMemberId(string id);
    }
}
