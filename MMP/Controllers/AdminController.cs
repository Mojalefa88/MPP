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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            
            mmpDBContext db = new mmpDBContext();

            
            var user = (from u in db.Users
                        where u.status == "Active"
                        select u).ToList();

            var removedUsers = (from u in db.Users
                                where u.status == "Removed"
                                select u).ToList();

            var suspendedUsers = (from u in db.Users
                                where u.status == "Suspended"
                                select u).ToList();

            var property = (from p in db.Properties
                            where p.status == "Available"
                            select p).ToList();

            var categoryID1 = (from p in db.Properties
                            where p.categoryID == 1
                            select p).ToList();

            var categoryID2 = (from p in db.Properties
                               where p.categoryID == 2
                               select p).ToList();

            Session["userCounter"] = user.Count;
            Session["removedUserCounter"] = removedUsers.Count;
            Session["suspendedRserCounter"] = suspendedUsers.Count;
            Session["propertyCounter"] = property.Count;
            Session["ForSale"] = categoryID1.Count;
            Session["ToRent"] = categoryID2.Count;


            return View(property);
        }

        public ActionResult Register()
        {
            Admin admin = new Admin();
            return View(admin);
        }

        [HttpPost]
        public ActionResult Register(Admin admin)
        {

            var db = new mmpDBContext();
            try
            {
                var isExist = db.Admins.Where(m => m.email == admin.email).ToList();
                if (ModelState.IsValid)
                {
                    if (isExist.Count > 0)
                    {
                        ViewBag.ErrorMessage = "User already exist - Go to Login";
                    }
                    else
                    {
                        db.Admins.Add(admin);
                        db.SaveChanges();
                        ClearFiellds(admin);
                        ViewBag.successMessage = "You have been successfully registered";
                    }
                }
                
                
            }
            catch (Exception ex)
            {

               
            }
            return View(admin);
        }

        public void ClearFiellds(Admin admin)
        {
            ModelState.Clear();
            admin.name = string.Empty;
            admin.email = string.Empty;
            admin.password = string.Empty;
            admin.confirmPassword = string.Empty;
        }

        public ActionResult Login()
        {
            Admin admin = new Admin();

            return View(admin);
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var db = new mmpDBContext();
            if(admin.isValid(admin.email, admin.password))
            {
                var getValues = (from a in db.Admins
                                 where a.email == admin.email
                                 select a).ToList();

                foreach(var value in getValues)
                {
                    Session["adminID"] = value.ID;
                    Session["adminName"] = value.name;
                   
                }
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.loginError = "Incorrect Email/Password!Try again!";
                
            }
            return View("Login");
        }

        public ActionResult Logout()
        {

            Session["adminID"] = null;
            Session["adminName"] = null;

            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var db = new mmpDBContext();

            var users = db.Users.Where(u => u.status == "Active").ToList();
            return View(users);
           
        }

        [HttpGet]
        public ActionResult UsersToUpdate()
        {
            var db = new mmpDBContext();

            var users = db.Users.Where(u => u.status == "Active").ToList();
            return View(users);

        }

        [HttpGet]
        public ActionResult UsersToDelete()
        {
            var db = new mmpDBContext();

            var users = db.Users.Where(u => u.status == "Active").ToList();
            return View(users);

        }

        [HttpGet]
        public ActionResult UsersToSuspend()
        {
            var db = new mmpDBContext();

            var users = db.Users.Where(u => u.status == "Active").ToList();
            return View(users);

        }

        [HttpGet]
        public ActionResult AddUsers() {
            User user = new User();
            //string tempPassword = "password01";
            //user.password = tempPassword;
            //user.confirmPassword = tempPassword;
            return View(user);
        }

        [HttpPost]
        public ActionResult AddUsers(User userModel)
        {
            var db = new mmpDBContext();

            var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new { x.Key, x.Value.Errors })
                  .ToArray();
            try
            {
                
                if (ModelState.IsValid)
                {
                    var isExist = db.Users.Where(m => m.email == userModel.email).ToList();

                    if (isExist.Count > 0)
                    {
                        ViewBag.ErrorMessage = "User already exist - Go to Login";
                    }
                    else
                    {
                       
                        userModel.status = "Active";
                        db.Users.Add(userModel);
                        db.SaveChanges();

                        //Sending confirmation Email to the registered user
                        sendRegisterConfirmationEmail(userModel.lastName, userModel.email, userModel.password);

                        ModelState.Clear();
                        userModel.firstName = string.Empty;
                        userModel.lastName = string.Empty;
                        userModel.email = string.Empty;
                        userModel.phone = string.Empty;
                        userModel.password = string.Empty;
                        userModel.confirmPassword = string.Empty;
                        userModel.subscribe = false;
                        ViewBag.successMessage = "You have been successfully registered";
                    }


                    
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "User could not be added to the system..";
            }

            return View(userModel);
        }


        [HttpGet]
        public ActionResult EditUser(int id)
        {
            mmpDBContext context = new mmpDBContext();
            var userModel = new User();
            //int userID = Convert.ToInt32(Session["userID"]);

            var userQry = context.Users.Where(u => u.ID == id).ToList();


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
        public ActionResult EditUser(int id, User model)
        {
            mmpDBContext context = new mmpDBContext();
            //int userID = Convert.ToInt32(Session["userID"]);
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
                    userModel.ID = id;
                    userModel.firstName = model.firstName;
                    userModel.lastName = model.lastName;
                    userModel.email = model.email;
                    userModel.phone = model.phone;
                    userModel.password = model.password;
                    userModel.confirmPassword = model.password;
                    userModel.subscribe = model.subscribe;

                    context.Entry(userModel).State = EntityState.Modified;
                    context.SaveChanges();
                    ViewBag.result = "User Data has been Successfully Updated";
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

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            mmpDBContext context = new mmpDBContext();
            var userModel = new User();
            //int userID = Convert.ToInt32(Session["userID"]);

            var userQry = context.Users.Where(u => u.ID == id).ToList();

           

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
        public ActionResult DeleteUser(int id, User model)
        {
            mmpDBContext context = new mmpDBContext();
            //int userID = Convert.ToInt32(Session["userID"]);
            //var updateUser = 
            context.Configuration.ValidateOnSaveEnabled = false;

            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            try
            {
                if (ModelState.IsValid)
                {
                    var userModel = new User();
                    userModel.ID = Convert.ToInt32(id);
                    userModel.firstName = model.firstName;
                    userModel.lastName = model.lastName;
                    userModel.email = model.email;
                    userModel.phone = model.phone;
                    userModel.password = model.password;
                    userModel.confirmPassword = model.password;
                    userModel.subscribe = model.subscribe;
                    userModel.status = "Removed";
                    userModel.reason = model.reason;
                    context.Entry(userModel).State = EntityState.Modified;
                    context.SaveChanges();
                    //var user = new User() { ID = Convert.ToInt32(id), reason = model.reason };

                    
                    //context.Users.Attach(user);
                    //context.Entry(user).Property(r => r.reason).IsModified = true;
                    //context.SaveChanges();
 
                    ViewBag.result = "User Data has been Successfully Removed";
                    ModelState.Clear();

                }
            }
            catch (Exception ex)
            {


            }

            return View(model);
        }


        //SuspendUser
        [HttpGet]
        public ActionResult SuspendUser(int id)
        {
            mmpDBContext context = new mmpDBContext();
            var userModel = new User();
            //int userID = Convert.ToInt32(Session["userID"]);

            var userQry = context.Users.Where(u => u.ID == id).ToList();


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
        public ActionResult SuspendUser(int id, User model)
        {
            mmpDBContext context = new mmpDBContext();
            //int userID = Convert.ToInt32(Session["userID"]);
            //var updateUser = 
            context.Configuration.ValidateOnSaveEnabled = false;

            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            try
            {
                if (ModelState.IsValid)
                {
                    var userModel = new User();
                    userModel.ID = Convert.ToInt32(id);
                    userModel.firstName = model.firstName;
                    userModel.lastName = model.lastName;
                    userModel.email = model.email;
                    userModel.phone = model.phone;
                    userModel.password = model.password;
                    userModel.confirmPassword = model.password;
                    userModel.subscribe = model.subscribe;
                    userModel.status = "Removed";
                    userModel.reason = model.reason;
                    context.Entry(userModel).State = EntityState.Modified;
                    context.SaveChanges();
                    //var user = new User() { ID = Convert.ToInt32(id), reason = model.reason };


                    //context.Users.Attach(user);
                    //context.Entry(user).Property(r => r.reason).IsModified = true;
                    //context.SaveChanges();

                    ViewBag.result = "User Data has been Successfully Removed";
                    ModelState.Clear();

                }
            }
            catch (Exception ex)
            {


            }

            return View(model);
        }

        //End Suspend User
        public ActionResult RemovedUsers()
        {
            var db = new mmpDBContext();
            var removedUsers = (from u in db.Users
                                where u.status == "Removed"
                                select u).ToList();

            return View(removedUsers);
        }

        public ActionResult SuspendedUsers()
        {
            var db = new mmpDBContext();
            var suspendedUsers = (from u in db.Users
                                  where u.status == "Suspended"
                                  select u).ToList();

            return View(suspendedUsers);
        }

        //Property Section
        [HttpGet]
        public ActionResult Properties()
        {
            var db = new mmpDBContext();

            var property = (from p in db.Properties
                            select p).ToList();
            return View(property);
        }

        [HttpGet]
        public ActionResult PropertyDetails(int id)
        {
            var db = new mmpDBContext();
            var proDetails = (from p in db.Properties
                              where p.propertyID == id
                              select p).ToList();

            Property pr = new Property();
            int cat = 0;
            foreach(var p in proDetails)
            {
                
                pr.propertyID = p.propertyID;
                pr.title = p.title;
                pr.price = p.price;
                pr.location = p.location;
                pr.status = p.status;
                pr.image = p.image;
                pr.Desc = p.Desc;
                cat = p.categoryID;
                
            }

            var category = (from c in db.PropertyCategories
                            where c.categoryID == cat
                            select c).ToList();
            var catName = "";
            foreach(var c in category)
            {
                catName = c.category;
            }

            ViewBag.catName = catName;
            return View(pr);
        }

        [HttpGet]
        public ActionResult PropertiesForsale()
        {
            var db = new mmpDBContext();
            var propertyForSale = (from p in db.Properties
                               where p.categoryID == 1
                               select p).ToList();

            return View(propertyForSale);

           
        }

        public ActionResult PropertiesToRent()
        {

            var db = new mmpDBContext();
            var propertyToRent = (from p in db.Properties
                               where p.categoryID == 2
                               select p).ToList();
            return View(propertyToRent);
        }
            //End Property Section
            public void sendRegisterConfirmationEmail(string lastName, string email, string password)
        {
            MailMessage message = new MailMessage();
            message.From = new System.Net.Mail.MailAddress("mprivateproperty@gmail.com");
            message.To.Add(new System.Net.Mail.MailAddress(email));
            message.Subject = "Welcome TO M Private Properties";
            message.Body = string.Format("Hi {0} ,<br/><br/>Thank you for choosing MPP<br/> The following are your login details.<br /><br />Username: {1} .<br /> Password: {2} <br /><br /> Regards, <br /> MPP DevTeam", lastName, email, password);
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

       // public ActionResult ViewProperties()
    }
}