using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class Property
    {
        [Key]
        public int propertyID { get; set; }
        [Required]
        [Display(Name = "Property Name")]
        public string title { get; set; }
        [Required]
        [Display(Name = "Derscription")]
        public string Desc { get; set; }

        [Display(Name ="Category")]
        public int categoryID { get; set; }
        public virtual ICollection<PropertyCategory> pCategory { get; set; }

        [Required]
        [Display(Name ="Price")]
        public double price { get; set; }

        [Required]
        [Display(Name ="Location")]
        public string location { get; set; }
        public string status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime date { get; set; }
        public int userID { get; set; }

        public byte[] image { get; set; }
    }
}