using WorldRef.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ProjectSearchModel
    {
        public int id { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public string EmailId { get; set; }

        public List<ImageStructure> ListOfImages { get; set; }
        public List<ImageStructure> ListOfVideos { get; set; }
    }
}