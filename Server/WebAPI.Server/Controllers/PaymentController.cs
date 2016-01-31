using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Server.Database;
using WebAPI.Server.Models;
using PayPal.Api;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WebAPI.Server.Controllers
{
    public class PaymentController : ApiController
    {
        private static string payPalClientId = "AekAtpGCqK5rRyQWjkkGLeB_xUBMMMlVy6LZAOksXyL_JTy_j12zU0cWH_u2AO-xxGpdTeekj-Z9GfD6";
        private static string payPalSecret = "EIbUl2oj26FDYC3po8sI0o1rqE0-o9yboJkGWJoEXyhd4fEaebhi187P_Ez2YmVzfK8QqRfFUK1V43i8";
        private static string key = "y8fN9sLekaKFNvi2apo409MxBv0e";

        // api/Payment
        /// <summary>
        /// Verify that payment was successful with PayPal. If successful, use Connector
        /// to connect to DB. Formulate insert statement to pass to DB to store transaction.
        /// Send purchase email to Willie's Cycles. Formulate delete statement to pass to DB to delete part.
        /// </summary>
        /// <param name="year">The year of the part.</param>
        /// <param name="make">The make of the part.</param>
        /// <param name="model">The model of the part.</param>
        /// <param name="partName">The name of the part.</param>
        /// <param name="location">The location of the part.</param>
        /// <param name="pkParts">The ID of the part.</param>
        /// <param name="price">The price of the part.</param>
        /// <param name="transaction">The transaction information as JSON.</param>
        /// <param name="modify">A boolean int indicating whether database should be modified.</param>
        /// <param name="token">The private key.</param>
        /// <returns>A string indicating success or the cause of failure.</returns>
        public string GetPerformPurchase(string year, string make, string model, string partName,
            string location, int pkParts, string price, string transaction, int modify, string token)
        {
            if (!token.Equals(key))
            {
                return "token";
            }
            else
            {
                try
                {
                    dynamic json = JObject.Parse(transaction);
                    string paymentId = json.response.id;

                    var config = ConfigManager.Instance.GetProperties();
                    var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                    var apiContext = new APIContext(accessToken);
                    var payment = Payment.Get(apiContext, paymentId);

                    var paymentString = payment.ConvertToJson();
                    dynamic paymentJson = JObject.Parse(payment.ConvertToJson());
                    string state = paymentJson.state;
                    double amount = paymentJson.transactions[0].related_resources[0].sale.amount.total;
                    string saleState = paymentJson.transactions[0].related_resources[0].sale.state;

                    if (state.Equals("approved") && saleState.Equals("completed") && amount == double.Parse(price.Trim()))
                    {
                        var formattedMake = "";
                        if (make != null && make.Trim().Length > 0)
                        {
                            formattedMake = make.Substring(0, 1);
                        }
                        if (model != null && model.Trim().Length > 0)
                        {
                            formattedMake = formattedMake + "-" + model;
                        }

                        var queryPartName = partName;
                        if (queryPartName != null && queryPartName.Contains("'"))
                        {
                            queryPartName = queryPartName.Replace("'", "''");
                        }

                        if (modify == 0)
                        {
                            bool emailSuccess = sendEmail(year, formattedMake, queryPartName,
                                    location, pkParts, double.Parse(price.Trim()), paymentId);

                            if (emailSuccess)
                            {
                                return "Successfully sent email!";
                            }
                            else
                            {
                                return "Failed to send email.";
                            }
                        }
                        else
                        {
                            Connector connector = new Connector();
                            bool insertSuccess = connector.Insert("INSERT INTO Transactions (YR, Make, PartName, Location, pkParts, Price, PaymentID) "
                                + " VALUES (\'" + year + "\',\'" + formattedMake + "\',\'" + queryPartName + "\'"
                                + ",\'" + location + "\'," + pkParts + ",\'" + price + "\',\'" + paymentId + "\')");

                            if (insertSuccess)
                            {
                                bool emailSuccess = sendEmail(year, formattedMake, partName,
                                    location, pkParts, double.Parse(price.Trim()), paymentId);

                                if (emailSuccess)
                                {
                                    bool deleteSuccess = connector.Delete("DELETE FROM Parts WHERE pkParts = " + pkParts);

                                    if (deleteSuccess)
                                    {
                                        return "Successfully sent email and modified database!";
                                    }
                                    else
                                    {
                                        return "Failed to delete.";
                                    }
                                }
                                else
                                {
                                    return "Failed to send email.";
                                }
                            }
                            else
                            {
                                return "Failed to Insert";
                            }
                        }
                    }
                    else
                    {
                        return "Payment Values Bad";
                    }
                }
                catch (Exception e)
                {
                    return "Exception";
                }
            }
        }

        private bool sendEmail(string year, string formattedMake, string partName, 
            string location, int pkParts, double price, string paymentId)
        {
            using (SmtpClient client = new SmtpClient())
            {
                try { 
                    MailMessage mail = new MailMessage("williescyclespurchase@gmail.com", "willie@williescycle.com");
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("williescyclespurchase@gmail.com", "motoparts8");
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    mail.Subject = "App Purchase";
                    mail.Body = "Year: " + year
                        + "\nMake: " + formattedMake
                        + "\nPart Name: " + partName
                        + "\nLocation: " + location
                        + "\npkParts: " + pkParts
                        + "\nPrice: " + price
                        + "\nPayment ID: " + paymentId;

                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate(object s, X509Certificate certificate, X509Chain chain,
                        SslPolicyErrors sslPolicyErrors)
                        { return true; };

                    client.Send(mail);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }          
            }
        }
    }
}
