namespace RDLCReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BuyerDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int user_id { get; set; }

        [Required]
        [StringLength(255)]
        public string fullname { get; set; }

        [Required]
        [StringLength(255)]
        public string address { get; set; }

        [Required]
        [StringLength(255)]
        public string phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime dob { get; set; }

        public bool gender { get; set; }

        public virtual User User { get; set; }
    }
}
