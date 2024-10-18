using COCOApp.Models;

namespace COCOApp.Repositories
{
    public interface ISupplierRepository
    {
        List<Supplier> GetSuppliers();
        List<Supplier> GetSuppliers(string nameQuery, int pageNumber, int pageSize, int sellerId, int statusId);
        Supplier GetSupplierById(int supplierId, int sellerId);
        int GetTotalSuppliers(string nameQuery, int sellerId, int statusId);
        void AddSupplier(Supplier supplier);
        void EditSupplier(int supplierId, Supplier supplier);
    }
}
