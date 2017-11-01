using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class PropertyCategory
    {
       [Key]
        public int categoryID { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string category { get; set; }

        public virtual ICollection<Property> property { get; set; }

        public virtual ICollection<Advice> propAdvice { get; set; }

    }
}