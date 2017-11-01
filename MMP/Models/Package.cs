using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class Package
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Package Type")]
        [Required]
        public string type { get; set; }

        [Display(Name ="Price")]
        [Required]
        public decimal price { get; set; }
      
    }
}