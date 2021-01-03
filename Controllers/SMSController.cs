using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Nexmo.Api.Messaging;
using ClientsApp.Models;
using Microsoft.Ajax.Utilities;

namespace ClientsApp.Controllers
{
    public class SMSController : Controller
    {
        [HttpPost]
        public ActionResult InboundSms()
        {

            Notification email = new Notification();

            try
            {
                InboundSms sms;
                using (StreamReader reader = new StreamReader(Request["Body"], Encoding.UTF8))
                {
                    sms = JsonConvert.DeserializeObject<InboundSms>(reader.ReadToEndAsync().Result);
                }
                email.SendEmail("noreply@truckeasymoving.com", "info@truckeasymoving.com", "InboundSMS", "From: " + sms.Msisdn + " Message: " + sms.Text);

            }
            catch (Exception ex)
            {
                email.SendEmail("noreply@truckeasymoving.com", "info@truckeasymoving.com", "InboundSMS", ex.Message);
            }

            Response.StatusCode = 200;
            return new EmptyResult();
        }


    }
}