using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Address { get; set; } = null!;
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; } = null!;
        public string? Note { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
