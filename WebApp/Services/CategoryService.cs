using COCOApp.Models;
using COCOApp.Repositories;

namespace COCOApp.Services
{
    public class CategoryService : StoreManagerService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }

        public List<Category> GetCategories(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            return _categoryRepository.GetCategories(nameQuery, pageNumber, pageSize, sellerId);
        }

        public Category GetCategoryById(int categoryId, int sellerId)
        {
            return _categoryRepository.GetCategoryById(categoryId, sellerId);
        }

        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }

        public void EditCategory(int categoryId, Category category)
        {
            _categoryRepository.EditCategory(categoryId, category);
        }

        public int GetTotalCategories(string nameQuery, int sellerId)
        {
            return _categoryRepository.GetTotalCategories(nameQuery, sellerId);
        }
    }
}
