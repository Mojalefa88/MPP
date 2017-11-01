using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMP.Models;

namespace MMP.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCategory()
        {
            PropertyCategory pc = new PropertyCategory();

            if (Session["adminID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
           
            return View(pc);
        }

        [HttpPost]
        public ActionResult AddCategory(PropertyCategory pc)
        {
            var db = new mmpDBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    db.PropertyCategories.Add(pc);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

               
            }
            return View(pc);
        }
    }
}