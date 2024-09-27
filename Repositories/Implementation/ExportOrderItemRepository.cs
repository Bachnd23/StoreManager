using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace COCOApp.Repositories
{
    public class ExportOrderItemRepository : IExportOrderItemRepository
    {
        private readonly StoreManagerContext _context;

        public ExportOrderItemRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public void addExportOrderItem(ExportOrderItem item)
        {
            _context.ExportOrderItems.Add(item);
            _context.SaveChanges(); 
        }
    }
}
