using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IExportOrderItemRepository
    {
        void addExportOrderItem(ExportOrderItem item);
        List<ExportOrderItem> GetExportOrderItems(string nameQuery, int pageNumber, int pageSize, int sellerId);
        int GetTotalExportOrderItems(string nameQuery, int sellerId);
        ExportOrderItem GetExportOrderItemById(int orderItemId, int sellerId);
    }
}
