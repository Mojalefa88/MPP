using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class PropertyCategoryViewModel
    {
        public int propertyID { get; set; }
        public string title { get; set; }
        public string Desc { get; set; }

        public int categoryID { get; set; }
      
        public double price { get; set; }

        public string location { get; set; }
        public string status { get; set; }

        public DateTime date { get; set; }
        public int userID { get; set; }

        public byte[] image { get; set; }

        public string category { get; set; }

        public string agentName { get; set; }

        public string agentEmail { get; set; }
        public string agentPhone { get; set; }

        public string agentStatus { get; set; }
    }
}