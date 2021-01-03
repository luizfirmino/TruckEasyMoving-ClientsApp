using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{
    [Table("order_reviews")]
    public class OrderReview
    {
        [Key]
        [ForeignKey("Order")]
        public int orderId { get; set; }
        public int stars { get; set; }    
        public string review { get; set; }
        public virtual Order Order { get; set; }
    }
}