using COCOApp.Models;

namespace COCOApp.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StoreManagerContext _context;

        public CategoryRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.AsQueryable().ToList();
        }

        public List<Category> GetCategories(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.CategoryName.Contains(nameQuery));
            }
            query = query.OrderByDescending(p => p.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public Category GetCategoryById(int categoryId, int sellerId)
        {
            var query = _context.Categories.AsQueryable();
            if (categoryId > 0)
            {
                query = query.Where(c => c.Id == categoryId);
            }
            return categoryId > 0 ? query.FirstOrDefault(u => u.Id == categoryId) : null;

        }

        public void EditCategory(int categoryId, Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Category not found");
            }
        }

        public int GetTotalCategories(string nameQuery, int sellerId)
        {
            // Khởi tạo truy vấn với bảng Category từ context
            var query = _context.Categories.AsQueryable();

            // Lọc theo điều kiện nameQuery (nếu có)
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.CategoryName.Contains(nameQuery));
            }

            // Lọc theo điều kiện sellerId (nếu có)
            if (sellerId > 0)
            {
                query = query.Where(c => c.Products.Any(p => p.SellerId == sellerId));
            }

            // Trả về tổng số danh mục sau khi đã áp dụng các điều kiện lọc
            return query.Count();
        }
    }
}
