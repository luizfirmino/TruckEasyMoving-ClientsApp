using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{
    [Table("cli_vw_order_billing")]
    public class OrderBilling
    {
        [Key]
        [ForeignKey("Order")]
        public int orderId { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string hourRate { get; set; }
        public string minimumHours { get; set; }
        public string totalHours { get; set; }
        public string hourCharges { get; set; }
        public string paid { get; set; }
        public string totalCharges { get; set; }
        public string totalChargesCard { get; set; }
        public long totalChargesSquareAPI { get; set; }
        public string CheckoutId { get; set; }
        public string CheckoutService { get; set; }
        public DateTime updated_at { get; set; }
        public virtual Order Order { get; set; }
    }
}