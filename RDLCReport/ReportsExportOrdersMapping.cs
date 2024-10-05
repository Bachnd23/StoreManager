namespace RDLCReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReportsExportOrdersMapping")]
    public partial class ReportsExportOrdersMapping
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int report_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int order_id { get; set; }

        public int? seller_id { get; set; }

        public virtual ExportOrder ExportOrder { get; set; }

        public virtual Report Report { get; set; }

        public virtual User User { get; set; }
    }
}
