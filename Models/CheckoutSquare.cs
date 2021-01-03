using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Square;
using Square.Models;
using Square.Apis;
using Square.Exceptions;
using System.Web.Configuration;

namespace ClientsApp.Models
{

    public class CheckoutSquare
    {
        public SquareClient client;
        public readonly string locationId;

        public CheckoutSquare()
        {
            // Get environment
            Square.Environment environment = WebConfigurationManager.AppSettings["SQUARE_Environment"] == "sandbox" ? Square.Environment.Sandbox : Square.Environment.Production;

            // Build base client
            client = new SquareClient.Builder()
              .Environment(environment)
              .AccessToken(WebConfigurationManager.AppSettings["SQUARE_AccessToken"])
              .Build();

            locationId = WebConfigurationManager.AppSettings["SQUARE_LocationId"];

        }

    }
}