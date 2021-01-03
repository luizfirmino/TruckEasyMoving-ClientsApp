using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Configuration;
using MySql.Data.MySqlClient;

namespace ClientsApp.Models
{
    [Table("cli_vw_orders")]
    public class Order
    {
    
        [Key]
        public int orderId { get; set; }
        public string ID { get; set; }
        public string contractNumber { get; set; }
        public string dateSchedule { get; set; }
        public string timeSchedule { get; set; }
        public decimal? duration { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public int orderStatusId { get; set; }
        public string customerName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string service { get; set; }
        public virtual OrderReview review { get; set; }
        public virtual OrderBilling billing { get; set; }

        public string serviceDescription { get; set; }
        public string servicePreparation { get; set; }
        public string hourRate { get; set; }
        public string minimumHours { get; set; }
        public string minuteIncrements { get; set; }
        public bool isJobDay { get; set; }
        public bool isJobDone { get; set; }

        public IEnumerable<OrderResources> resources { get; set; }
        public IEnumerable<OrderAddresses> addresses { get; set; }
        public IEnumerable<OrderPayments> payments { get; set; }


        

    }

    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class OrderDBContext : DbContext
    {
        public OrderDBContext()
            : base("DbContext") { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderResources> Resources { get; set; }
        public DbSet<OrderAddresses> Addresses { get; set; }
        public DbSet<OrderPayments> Payments { get; set; }

        public DbSet<OrderBilling> Billing { get; set; }

        public DbSet<OrderReview> Review { get; set; }

        private class StoreProcOutput
        {
            public int p_code { get; set; }
            public string p_message { get; set; }
        }

        
        public bool CheckPayment(int orderId, string checkoutId)
        {

            MySqlParameter orderIdParam = new MySqlParameter("p_orderId", MySqlDbType.Int32);
            orderIdParam.Direction = System.Data.ParameterDirection.Input;
            orderIdParam.Value = orderId;

            MySqlParameter checkoutIdParam = new MySqlParameter("p_checkoutId", MySqlDbType.VarChar);
            checkoutIdParam.Direction = System.Data.ParameterDirection.Input;
            checkoutIdParam.Value = checkoutId;
                        

            MySqlParameter[] spParams = new MySqlParameter[] {
                orderIdParam, checkoutIdParam
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("CALL spCheckPayment(@p_orderId, @p_checkoutId)");
            string commandText = sb.ToString();

            try { 

                var result = base.Database.SqlQuery<StoreProcOutput>(commandText, spParams).FirstOrDefault();

                if (result.p_code == 0)
                {
                    return true;
                } else
                {
                    throw new Exception(result.p_message);
                }


            } catch(Exception e)
            {
                throw new Exception(e.ToString());
            }

        }


        public bool CreatePayment(int orderId, string checkoutId, string checkoutService)
        {

            MySqlParameter orderIdParam = new MySqlParameter("p_orderId", MySqlDbType.Int32);
            orderIdParam.Direction = System.Data.ParameterDirection.Input;
            orderIdParam.Value = orderId;

            MySqlParameter checkoutIdParam = new MySqlParameter("p_checkoutId", MySqlDbType.VarChar);
            checkoutIdParam.Direction = System.Data.ParameterDirection.Input;
            checkoutIdParam.Value = checkoutId;

            MySqlParameter checkoutServiceParam = new MySqlParameter("p_checkoutService", MySqlDbType.VarChar);
            checkoutServiceParam.Direction = System.Data.ParameterDirection.Input;
            checkoutServiceParam.Value = checkoutService;

            MySqlParameter[] spParams = new MySqlParameter[] {
                orderIdParam, checkoutIdParam, checkoutServiceParam
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("CALL spCreatePayment(@p_orderId, @p_checkoutId, @p_checkoutService)");
            string commandText = sb.ToString();


            try
            {

                var result = base.Database.SqlQuery<StoreProcOutput>(commandText, spParams).FirstOrDefault();

                if (result.p_code == 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception(result.p_message);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

        }

    }

    

}