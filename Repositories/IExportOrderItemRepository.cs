using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IExportOrderItemRepository
    {
        void addExportOrderItem(ExportOrderItem item);
    }
}
