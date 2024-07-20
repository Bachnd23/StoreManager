using COCOApp.Models;
using COCOApp.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace COCOApp.Services
{
    public class OrderService : StoreManagerService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetOrders()
        {
            return _orderRepository.GetOrders();
        }

        public List<Order> GetOrdersByIds(List<int> orderIds, int sellerId)
        {
            return _orderRepository.GetOrdersByIds(orderIds, sellerId);
        }

        public Order GetOrderById(int orderId, int sellerId)
        {
            return _orderRepository.GetOrderById(orderId, sellerId);
        }

        public List<Order> GetOrders(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            return _orderRepository.GetOrders(nameQuery, pageNumber, pageSize, sellerId);
        }

        public int GetTotalOrders(string nameQuery, int sellerId)
        {
            return _orderRepository.GetTotalOrders(nameQuery, sellerId);
        }

        public List<SelectListItem> GetCustomersSelectList(int sellerId)
        {
            return _orderRepository.GetCustomersSelectList(sellerId);
        }

        public List<SelectListItem> GetProductsSelectList(int sellerId)
        {
            return _orderRepository.GetProductsSelectList(sellerId);
        }

        public void AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }

        public void EditOrder(int orderId, Order order)
        {
            _orderRepository.EditOrder(orderId, order);
        }

        public List<Order> GetOrders(string dateRange, int customerId, int sellerId)
        {
            return _orderRepository.GetOrders(dateRange, customerId, sellerId);
        }
    }
}
