using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ProjectImageModel
    {
        public int id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public string ProjectCertificates { get; set; }
    }

}