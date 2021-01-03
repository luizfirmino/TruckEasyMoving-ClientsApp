using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{
    [Table("cli_vw_order_addresses")]
    public class OrderAddresses
    {

        [Key]
        [Column(Order = 1)]
        public int orderId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int addressId { get; set; }
        public string address { get; set; }
        public string addressComp { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string notes { get; set; }
        public string order { get; set; }
    }
}