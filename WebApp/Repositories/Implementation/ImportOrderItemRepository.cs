using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Repositories.Implementation
{
    public class ImportOrderItemRepository : IImportOrderItemRepository
    {
        private readonly StoreManagerContext _context;

        public ImportOrderItemRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public void addImportOrderItem(ImportOrderItem item)
        {
            ImportOrderItem importOrderItem = _context.ImportOrderItems.FirstOrDefault(o => o.OrderId == item.OrderId 
            && o.ProductId == item.ProductId);
            if (importOrderItem == null)
            {
                _context.ImportOrderItems.Add(item);
            }
            else
            {
                importOrderItem.Volume += item.Volume;
                Product product = _context.Products.FirstOrDefault(p => p.Id == importOrderItem.ProductId);
                //importOrderItem.Total = importOrderItem.Volume * product.Cost;
            }
            _context.SaveChanges();
        }

        public ImportOrderItem GetImportOrderItemById(int orderId, int productId, int sellerId)
        {
            var query = _context.ImportOrderItems
                                .Include(o => o.Product)
                                .Include(o => o.Order)
                                .ThenInclude(c => c.Supplier)
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.Order.SellerId == sellerId);
            }
            ImportOrderItem item = query.FirstOrDefault(u => u.OrderId == orderId && u.ProductId == productId);
            return item;
        }
        public List<ImportOrderItem> GetImportOrderItemsByIds(List<int> orderIds, int sellerId)
        {
            var query = _context.ImportOrderItems
                                  .Include(o => o.Product)
                                  .Include(o => o.Order)
                                  .ThenInclude(c => c.Supplier)
                                .Where(o => orderIds.Contains(o.OrderId))
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.Order.SellerId == sellerId);
            }

            return query.ToList();
        }
        public List<ImportOrderItem> GetImportOrderItems(int orderId, string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.ImportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Supplier);
            if (sellerId > 0)
            {
                query = query.Where(o => o.Order.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(o => o.Order.Supplier.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(o => o.Order.OrderDate);

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public List<ImportOrderItem> GetImportOrderItems(int orderId, int sellerId)
        {

            var query = _context.ImportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Supplier);
            if (sellerId > 0)
            {
                query = query.Where(o => o.Order.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            query = query.OrderByDescending(o => o.Order.OrderDate);

            return query.ToList();
        }

        public int GetTotalImportOrderItems(int orderId, string nameQuery, int sellerId)
        {
            var query = _context.ImportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Supplier);
            if (sellerId > 0)
            {
                query = query.Where(o => o.Order.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Order.Supplier.Name.Contains(nameQuery));
            }

            return query.Count();
        }
        public void EditImportOrderItem(int orderId, int productId, ImportOrderItem order)
        {
            var existingOrder = _context.ImportOrderItems
                .Include(o => o.Product)
                .ThenInclude(i => i.InventoryManagement)
                .FirstOrDefault(c => c.OrderId == orderId && c.ProductId == productId);

            if (existingOrder != null)
            {
                existingOrder.OrderId = order.OrderId;
                existingOrder.ProductId = order.ProductId;
                existingOrder.ProductCost = order.ProductCost;
                existingOrder.Volume = order.Volume;
                existingOrder.RealVolume = order.RealVolume;
                //existingOrder.Total = order.Total;
                //existingOrder.Order.SellerId = order.Order.SellerId;
                existingOrder.CreatedAt = order.CreatedAt;
                existingOrder.UpdatedAt = order.UpdatedAt;
                existingOrder.Status = order.Status;
                if (existingOrder.Status)
                {
                    InventoryManagement inventory = _context.InventoryManagements.FirstOrDefault(p => p.ProductId == existingOrder.ProductId);
                    //Update inventory changes
                    int realVolumeAsInt = Convert.ToInt32(existingOrder.RealVolume);
                    inventory.AllocatedVolume -= realVolumeAsInt;
                    inventory.RemainingVolume += realVolumeAsInt;
                }
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Order not found");
            }
        }
    }
}
