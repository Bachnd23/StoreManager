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
    }
}
