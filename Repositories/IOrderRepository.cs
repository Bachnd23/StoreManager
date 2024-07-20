using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        List<Order> GetOrdersByIds(List<int> orderIds, int sellerId);
        Order GetOrderById(int orderId, int sellerId);
        List<Order> GetOrders(string nameQuery, int pageNumber, int pageSize, int sellerId);
        int GetTotalOrders(string nameQuery, int sellerId);
        List<SelectListItem> GetCustomersSelectList(int sellerId);
        List<SelectListItem> GetProductsSelectList(int sellerId);
        void AddOrder(Order order);
        void EditOrder(int orderId, Order order);
        List<Order> GetOrders(string dateRange, int customerId, int sellerId);
    }
}
