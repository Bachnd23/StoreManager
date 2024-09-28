using COCOApp.Models;
using COCOApp.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace COCOApp.Services
{
    public class ExportOrderItemService : StoreManagerService
    {
        private readonly IExportOrderItemRepository _orderItemRepository;

        public ExportOrderItemService(IExportOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public void AddExportOrderItem(ExportOrderItem order)
        {
            _orderItemRepository.addExportOrderItem(order);
        }
        public List<ExportOrderItem> GetExportOrderItems(int orderId,string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            return _orderItemRepository.GetExportOrderItems(orderId,nameQuery, pageNumber, pageSize, sellerId);
        }
        public int GetTotalExportOrderItems(string nameQuery, int sellerId)
        {
            return _orderItemRepository.GetTotalExportOrderItems(nameQuery, sellerId);
        }
        public ExportOrderItem GetExportOrderitemById(int orderId,int productId, int sellerId)
        {
            return _orderItemRepository.GetExportOrderItemById(orderId,productId, sellerId);
        }
        public void EditExportOrderItem(int orderId,int productId, ExportOrderItem order)
        {
            _orderItemRepository.EditExportOrderItem(orderId,productId, order);
        }
    }
}
