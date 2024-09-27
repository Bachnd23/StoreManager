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

        public List<ExportOrderItem> GetExportOrderItems(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.ExportOrderItems.AsQueryable();
            query = query.Include(o=>o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Customer);
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(o => o.Order.Customer.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(o => o.Id);

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int GetTotalExportOrderItems(string nameQuery, int sellerId)
        {
            var query = _context.ExportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Customer);
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Order.Customer.Name.Contains(nameQuery));
            }

            return query.Count();
        }
    }
}
