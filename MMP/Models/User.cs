using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string lastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }
        public string phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        [Display(Name = "Subscribe")]
        public bool subscribe { get; set; }

        public string status { get; set; }
        public string reason { get; set; }


        public bool isValid(string email, string password)
        {
            mmpDBContext context = new mmpDBContext();
            var qry = (from u in context.Users
                     where u.email == email && u.password == password
                     select u).ToList();
            if (qry.Count > 0)
                return true;
            else
                return false;
        }
    }
}