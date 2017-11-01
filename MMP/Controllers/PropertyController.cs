using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMP.Models;

namespace MMP.Controllers
{
    public class PropertyController : Controller
    {
        // GET: Property
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProperty()
        {
            var db = new mmpDBContext();
            Property property = new Property();
            //if (Session["userID"] != null)
            //{
                IEnumerable<SelectListItem> items = new SelectList(db.PropertyCategories, "categoryID", "category");
                ViewBag.categoryID = items;
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //ViewBag.categoryID = new SelectList(db.PropertyCategories, "prodCategoryID", "categoryName", prop.categoryID);
            return View(property);
        }

        [HttpPost]
        public ActionResult AddProperty(Property prop, HttpPostedFileBase upload)
        {
            int userID = Convert.ToInt32(Session["userID"]);

            var db = new mmpDBContext();

            var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                     .ToArray();

            // ViewBag.categoryID = new SelectList(db.PropertyCategories, "categoryID", "category", prop.categoryID);
            try
            {
                if(userID > 0)
                {
                    if (ModelState.IsValid)
                    {
                        if (upload != null)
                        {
                            prop.image = new byte[upload.ContentLength];
                            upload.InputStream.Read(prop.image, 0, upload.ContentLength);
                        }

                        prop.userID = userID;
                        prop.date = DateTime.Now;
                        prop.status = "Available";

                        db.Properties.Add(prop);
                        db.SaveChanges();

                        //Clearing the textboxes
                        ModelState.Clear();
                        prop.title = string.Empty;
                        prop.price = 0;
                        prop.Desc = string.Empty;
                        prop.location = string.Empty;

                        ViewBag.successMessage = "Property saved successfully";
                        ViewBag.categoryID = new SelectList(db.PropertyCategories, "categoryID", "category", prop.categoryID);
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.successMessage = "Property was not saved- Please retry";

            }
           
            return View(prop);
        }

        [HttpPost]
        public ActionResult Payment()
        {
            return View();
        }
    }
}