using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer
{
    public class IKnowledgeLoginModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OrganizationName { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }
    }
}