using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }


        public bool isValid(string email, string password)
        {
            mmpDBContext context = new mmpDBContext();
            var qry = (from a in context.Admins
                       where a.email == email && a.password == password
                       select a).ToList();
            if (qry.Count > 0)
                return true;
            else
                return false;
        }
    }
}