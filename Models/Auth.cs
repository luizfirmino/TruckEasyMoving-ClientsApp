using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientsApp.Models
{
    [Table("cli_vw_auth")]
    public class Auth
    {

        [Key]
        public string contractNumber { get; set; }
        public string ID { get; set; }
        public int orderId { get; set; }
        public int orderStatusId { get; set; }
        public string phoneNumber { get; set; }
        
    }


    public class Login
    {

        [Required(ErrorMessage = "Please enter phone number.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [StringLength(10)]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Order Number.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Order Number")]
        [StringLength(10)]
        public string orderNumber { get; set; }

    }


    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class AuthDBContext : DbContext
    {
        public AuthDBContext()
            : base("DbContext") { }

        public DbSet<Auth> Auth { get; set; }
        
    }
}