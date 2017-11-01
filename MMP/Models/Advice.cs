using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class Advice
    {
        [Key]
        public int advideID { get; set; }

        [Required]
        [Display(Name = "Tittle")]
        public string advice { get; set; }

        [Display(Name = "Description")]
        public string adiveDesc { get; set; }

        public int categoryID { get; set; }
        public virtual ICollection<PropertyCategory> propCategory { get; set; }
    }
}