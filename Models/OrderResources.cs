using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{

    [Table("cli_vw_order_resources")]
    public class OrderResources
    {

        [Key]
        [Column(Order = 1)]
        public int orderId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int resourceId { get; set; }
        public string resourceName { get; set; }
        public string role { get; set; }
        public string profilePicture { get; set; }
        public string phoneNumber { get; set; }
        public bool isLeader { get; set; }

    }
}