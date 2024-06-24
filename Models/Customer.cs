using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Note { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
