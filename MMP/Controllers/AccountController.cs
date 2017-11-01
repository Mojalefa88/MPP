using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMP.Models;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;

namespace MMP.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {

            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            mmpDBContext db = new mmpDBContext();

            var errors = ModelState
                  .Where(x => x.Value.Errors.Count > 0)
                  .Select(x => new { x.Key, x.Value.Errors })
                   .ToArray();
            try
            {
                if (ModelState.IsValid)
                {
                    user.status = "Active";
                    db.Users.Add(user);
                    db.SaveChanges();

                    MailMessage message = new MailMessage();
                    message.From = new System.Net.Mail.MailAddress("mprivateproperty@gmail.com");
                    message.To.Add(new System.Net.Mail.MailAddress(user.email));
                    message.Subject = "Welcome TO M Private Properties";
                    message.Body = string.Format("Hi {0} ,<br/>Thank you for choosing MPP<br/> The following are your login details.<br /><br />Username: {1} .<br /> Password: {2} <br /><br /> Regards, <br /> MPP DevTeam", user.lastName, user.email, user.password);
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

                    ModelState.Clear();
                    user.firstName = string.Empty;
                    user.lastName = string.Empty;
                    user.email = string.Empty;
                    user.phone = string.Empty;
                    user.password = string.Empty;
                    user.confirmPassword = string.Empty;
                    user.subscribe = false;
                }
            }
            catch(Exception ex)
            {
                
            }
            

            return View(user);
        }

        public ActionResult PartialRegister()
        {
            return PartialView("_PartialRegister");
        }

        [HttpPost]
        public ActionResult PartialRegister(User user)
        {
            mmpDBContext db = new mmpDBContext();

            var errors = ModelState
                  .Where(x => x.Value.Errors.Count > 0)
                  .Select(x => new { x.Key, x.Value.Errors })
                   .ToArray();
            try
            {
                if (ModelState.IsValid)
                {
                    user.status = "Active";
                    db.Users.Add(user);
                    db.SaveChanges();

                    MailMessage message = new MailMessage();
                    message.From = new System.Net.Mail.MailAddress("mprivateproperty@gmail.com");
                    message.To.Add(new System.Net.Mail.MailAddress(user.email));
                    message.Subject = "Welcome TO M Private Properties";
                    message.Body = string.Format("Hi {0} ,<br/>Thank you for choosing MPP<br/> The following are your login details.<br /><br />Username: {1} .<br /> Password: {2} <br /><br /> Regards, <br /> MPP DevTeam", user.lastName, user.email, user.password);
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

                    ModelState.Clear();
                    user.firstName = string.Empty;
                    user.lastName = string.Empty;
                    user.email = string.Empty;
                    user.phone = string.Empty;
                    user.password = string.Empty;
                    user.confirmPassword = string.Empty;
                    user.subscribe = false;
                }
            }
            catch (Exception ex)
            {

               
            }
           

            //return View(user);

            return RedirectToAction("Index", "Home");
        }

        
        public ActionResult LoginPartial()
        {
            return PartialView("_PartialLogin");
        }

        [HttpPost]
        public ActionResult LoginPartial(User user)
        {
            mmpDBContext db = new mmpDBContext();

            if(user.isValid(user.email, user.password)){

                var dataItem = (from u in db.Users
                                where u.email == user.email
                                select u).ToList();
                foreach (var usr in dataItem)
                {
                    Session["userID"] = usr.ID;
                    Session["userName"] = usr.firstName;
                    Session["lastName"] = usr.lastName;

                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
               ViewBag.loginError = "Incorrect Email/Password!Try again!";
                return PartialView("_PartialLogin");
            }
            
        }

        public ActionResult Logout()
        {

            Session["userID"] = null;
            Session["userName"] = null;
            Session["lastName"] = null;


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            mmpDBContext context = new mmpDBContext();
            var userModel = new User();
            int userID = Convert.ToInt32(Session["userID"]);

            var userQry = context.Users.Where(u => u.ID == userID).ToList();

            
            //var addressModel = Context.AddressModelSet.Where(a => a.customerID == id).ToList();

            foreach (var userValues in userQry)
            {
                userModel.firstName = userValues.firstName;
                userModel.lastName = userValues.lastName;
                userModel.email = userValues.email;
                userModel.phone = userValues.phone;
                userModel.password = userValues.password;
                userModel.confirmPassword = userValues.password;
                userModel.subscribe = userValues.subscribe;
            }

            return View(userModel);
        }

        [HttpPost]
        public ActionResult EditProfile(User model)
        {
            mmpDBContext context = new mmpDBContext();
            int userID = Convert.ToInt32(Session["userID"]);
            //var updateUser = 
            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            try
            {
                if (ModelState.IsValid)
                {
                    var userModel = new User();
                    userModel.ID = userID;
                    userModel.firstName = model.firstName;
                    userModel.lastName = model.lastName;
                    userModel.email = model.email;
                    userModel.phone = model.phone;
                    userModel.password = model.password;
                    userModel.confirmPassword = model.password;
                    userModel.subscribe = model.subscribe;

                    context.Entry(userModel).State = EntityState.Modified;
                    context.SaveChanges();
                    ViewBag.result = "Your Data has been Successfully Updated";
                }
            }
            catch (Exception ex)
            {
                

            }
            

                 //context.Entry(useM).State = EntityState.Modified;
                //Context.SaveChanges();
                // ViewBag.result = "Your Data has been Successfully Updated";
                return View(model);
        }
    }
}