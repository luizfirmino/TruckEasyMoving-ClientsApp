using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{

    [Table("cli_vw_order_payments")]
    public class OrderPayments {
        [Key]
        [Column(Order = 1)]
        public int orderId { get; set; }    
        public string amount { get; set; }
        [Key]
        [Column(Order = 2)]
        public string type { get; set; }
    }

}