using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMP.Models;
using System.Net.Mail;
using System.Net;

namespace MMP.Controllers
{
    public class HomeController : Controller
    {
        public string agentEmail = "";
        public string agentName2 = "";
        // GET: Home
        public ActionResult Index(string searchProperty)
        {
            var db = new mmpDBContext();
            var catName = "";

                if ((!string.IsNullOrEmpty(searchProperty)) && (isNumeric(searchProperty, System.Globalization.NumberStyles.Float)))
                {
                var price = 0.0;
                bool convert = double.TryParse(searchProperty, out price);

                var propertyModel = (from p in db.Properties
                                     where p.status == "Available"
                                     join pc in db.PropertyCategories
                                     on p.categoryID equals pc.categoryID
                                     join u in db.Users
                                     on p.userID equals u.ID
                                     select new PropertyCategoryViewModel
                                     {
                                         propertyID = p.propertyID,
                                         title = p.title,
                                         Desc = p.Desc,
                                         categoryID = p.categoryID,
                                         price = p.price,
                                         location = p.location,
                                         status = p.status,
                                         date = p.date,
                                         userID = p.userID,
                                         image = p.image,
                                         category = pc.category,
                                         agentName = u.lastName + " " + u.firstName,
                                         agentEmail = u.email,
                                         agentPhone = u.phone,
                                         agentStatus = u.status
                                     });


                propertyModel = propertyModel.Where(p => p.price.Equals(price));
                ViewBag.checkLocation = 1;
                return View(propertyModel);
            }
                else if ((!string.IsNullOrEmpty(searchProperty)) && (searchProperty.ToLower() != "all"))
                {
                ViewBag.checkLocation = 1;
                
                var propertyModel = (from p in db.Properties
                                     where p.status == "Available"
                                     join pc in db.PropertyCategories
                                     on p.categoryID equals pc.categoryID
                                     join u in db.Users
                                     on p.userID equals u.ID
                                     select new PropertyCategoryViewModel
                                     {
                                         propertyID = p.propertyID,
                                         title = p.title,
                                         Desc = p.Desc,
                                         categoryID = p.categoryID,
                                         price = p.price,
                                         location = p.location,
                                         status = p.status,
                                         date = p.date,
                                         userID = p.userID,
                                         image = p.image,
                                         category = pc.category,
                                         agentName = u.lastName+" "+u.firstName,
                                         agentEmail = u.email,
                                         agentPhone = u.phone,
                                         agentStatus = u.status
                                         
                                     });


                propertyModel = propertyModel.Where(p => p.title.StartsWith(searchProperty)
                                                    || p.title.StartsWith(searchProperty)
                                                    || p.location.Contains(searchProperty)
                                                    || p.location.StartsWith(searchProperty)
                                                    || p.Desc.StartsWith(searchProperty)
                                                    || p.Desc.Contains(searchProperty)
                                                    || p.category.Contains(searchProperty)
                                                    || p.category.StartsWith(searchProperty)
                                                    );

              
                return View(propertyModel);
                }
                else if((!string.IsNullOrEmpty(searchProperty)) && (searchProperty.ToLower() == "all"))
                {
                
                var propertyModel = (from p in db.Properties
                                     where p.status == "Available"
                                     join pc in db.PropertyCategories
                                     on p.categoryID equals pc.categoryID
                                     join u in db.Users
                                     on p.userID equals u.ID
                                     select new PropertyCategoryViewModel
                                     {
                                         propertyID = p.propertyID,
                                         title = p.title,
                                         Desc = p.Desc,
                                         categoryID = p.categoryID,
                                         price = p.price,
                                         location = p.location,
                                         status = p.status,
                                         date = p.date,
                                         userID = p.userID,
                                         image = p.image,
                                         category = pc.category,
                                         agentName = u.lastName+" "+u.firstName,
                                         agentEmail = u.email,
                                         agentPhone = u.phone
                                         
                                        
                                     });


                
                return View(propertyModel);
            }   
                else
                {
                

                var propertyModel = (from p in db.Properties
                                     where p.status == "Available"
                                     join pc in db.PropertyCategories
                                     on p.categoryID equals pc.categoryID
                                     join u in db.Users
                                     on p.userID equals u.ID
                                     select new PropertyCategoryViewModel
                                     {
                                         propertyID = p.propertyID,
                                         title = p.title,
                                         Desc = p.Desc,
                                         categoryID = p.categoryID,
                                         price = p.price,
                                         location = p.location,
                                         status = p.status,
                                         date = p.date,
                                         userID = p.userID,
                                         image = p.image,
                                         category = pc.category,
                                         agentName = u.lastName + " " + u.firstName,
                                         agentEmail = u.email,
                                         agentPhone = u.phone,
                                         agentStatus = u.status
                                     });

                return View(propertyModel);
                }
           
           
            
        }

        //Checking if the entered value in a textbox is an integer
        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        //End Checking

        [HttpGet]
        public ActionResult Packages()
        {
            //var db = new mmpDBContext();
            //var package = (from p in db.Packages
            //               select p).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddPackage(Package user)
        {
            return View();
        }

        //Property details
        [HttpGet]
        public ActionResult PropertyDetails(int id)
        {
            var db = new mmpDBContext();
            var proDetails = (from p in db.Properties
                              where p.propertyID == id
                              select p).ToList();

            Property pr = new Property();
            int cat = 0;
            int agentID = 0;
            foreach (var p in proDetails)
            {

                pr.propertyID = p.propertyID;
                pr.title = p.title;
                pr.price = p.price;
                pr.location = p.location;
                pr.status = p.status;
                pr.image = p.image;
                pr.Desc = p.Desc;
                cat = p.categoryID;
                agentID = p.userID;

            }

            //Selecting category
            var category = (from c in db.PropertyCategories
                            where c.categoryID == cat
                            select c).ToList();

            var catName = "";
            foreach (var c in category)
            {
                catName = c.category;
            }

            ViewBag.catName = catName;

            //Slecting Agent
            var agent = (from a in db.Users
                         where a.ID == agentID
                         select a).ToList();

            string agentName = "", agentPhone = "";
           
            foreach(var a in agent)
            {
                agentName = a.lastName.TrimEnd() + "" + a.firstName.TrimStart();
                agentPhone = a.phone;
                Session["agentEmail"] = a.email;
                Session["agentName"] = agentName;
            }


            ViewBag.agentName = agentName;
            ViewBag.agentPhone = agentPhone;
            //End Selecting agent

            //Getting details of logged on user

            int userID = Convert.ToInt32(Session["userID"]);

            var user = (from u in db.Users
                        where u.ID == userID
                        select u).ToList();
            string name = "", email = "", phoneNumber = "";

            foreach (var u in user)
            {
                name = u.firstName + " " + u.lastName.TrimEnd();
                email = u.email.TrimEnd();
                phoneNumber = u.phone.TrimEnd();
                
            }

            ViewBag.userName = name;
            ViewBag.userEmail = email;
            ViewBag.userPhone = phoneNumber;
            //End getting details of logged on user
            return View(pr);
        }

        [HttpPost]
        public ActionResult PropertyDetails(string name, string email, string phone, string messageM)
        {
            if (!string.IsNullOrEmpty(messageM))
            {
                SendMessage(name, email, phone, messageM);
            }
            return View();
        }

        public void SendMessage(string name, string email, string phone, string msg)
        {
            string agentFullName = Convert.ToString(Session["agentName"]);
            string agentEmailAddr = Convert.ToString(Session["agentEmail"]);


            

            MailMessage message = new MailMessage();
            message.From = new System.Net.Mail.MailAddress(email);
            message.To.Add(new System.Net.Mail.MailAddress(agentEmailAddr));
            message.Subject = "Property Interest Message";
            message.Body = string.Format("Hi Agent {0} ,<br/><br/>The following user showed interest in your property<br/> Name: {1}<br/> Phone Number: {2}<br/><br/><b>Message<b/><br/>{3}", agentFullName, name, phone, msg);
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Send(message);
        }
        //End Property details
    }
}