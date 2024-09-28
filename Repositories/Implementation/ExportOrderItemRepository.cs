using COCOApp.Models;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
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
            ExportOrderItem exportOrderItem=_context.ExportOrderItems.FirstOrDefault(o=>o.OrderId== item.OrderId&&o.ProductId==item.ProductId);
            if (exportOrderItem == null)
            {
                _context.ExportOrderItems.Add(item);
            }
            else
            {
                exportOrderItem.Volume+=item.Volume;
            }
            _context.SaveChanges();
        }

        public ExportOrderItem GetExportOrderItemById(int orderItemId,int productId, int sellerId)
        {
            var query = _context.ExportOrderItems
                                .Include(o => o.Product)
                                .Include(o => o.Order)
                                .ThenInclude(c => c.Customer)
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            return orderItemId > 0 ? query.FirstOrDefault(u => u.OrderId == orderItemId) : null;
        }

        public List<ExportOrderItem> GetExportOrderItems(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            pageNumber = Math.Max(pageNumber, 1);

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
                query = query.Where(o => o.Order.Customer.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(o => o.Order.OrderDate);

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
