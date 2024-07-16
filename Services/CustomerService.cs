using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class CustomerService : StoreManagerService
    {
        public List<Customer> GetCustomers()
        {
            var query = _context.Customers.AsQueryable();
            return query.ToList();
        }
        public List<Customer> GetCustomers(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Customers
                        .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(c => c.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public Customer GetCustomerById(int customerId,int sellerId)
        {
            var query = _context.Customers.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (customerId > 0)
            {
                return query.FirstOrDefault(u => u.Id == customerId);
            }
            else
            {
                return null;
            }
        }

        public int GetTotalCustomers(string nameQuery, int sellerId)
        {
            var query = _context.Customers
                        .AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }

            return query.Count();
        }
        public void AddCustormer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public void EditCustomer(int customerId, Customer customer)
        {
            // Retrieve the customer from the database
            Customer? existingCustomer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

            // Check if the customer exists
            if (existingCustomer != null)
            {
                // Update the properties of the existing customer
                existingCustomer.Name = customer.Name;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;
                existingCustomer.Status = customer.Status;
                existingCustomer.Note=customer.Note;
                existingCustomer.UpdatedAt = customer.UpdatedAt;

                // Save the changes to the database
                _context.SaveChanges();
            }
            else
            {
                // Handle the case when the customer is not found
                throw new ArgumentException("Customer not found");
            }
        }

    }
}
