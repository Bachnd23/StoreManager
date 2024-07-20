﻿using COCOApp.Models;
using COCOApp.Repositories;
using System.Collections.Generic;

namespace COCOApp.Services
{
    public class CustomerService : StoreManagerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        public List<Customer> GetCustomers(string nameQuery, int pageNumber, int pageSize, int sellerId, int statusId)
        {
            return _customerRepository.GetCustomers(nameQuery, pageNumber, pageSize, sellerId, statusId);
        }

        public Customer GetCustomerById(int customerId, int sellerId)
        {
            return _customerRepository.GetCustomerById(customerId, sellerId);
        }

        public int GetTotalCustomers(string nameQuery, int sellerId, int statusId)
        {
            return _customerRepository.GetTotalCustomers(nameQuery, sellerId, statusId);
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.AddCustomer(customer);
        }

        public void EditCustomer(int customerId, Customer customer)
        {
            _customerRepository.EditCustomer(customerId, customer);
        }
    }
}
