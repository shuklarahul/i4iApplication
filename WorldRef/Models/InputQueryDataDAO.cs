using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class InputQueryDataDAO
    {

        public long Id { get; set; }
        public string Attachments { get; set; }
        public string QueryText { get; set; }
        public string UQueryNum { get; set; }
        public string acion { get; set; }
        public Nullable<System.DateTime> QDate { get; set; }
        public Nullable<long> Supplier { get; set; }
        public Nullable<System.DateTime> ForwardDate { get; set; }
        public string UserNo { get; set; }
        public string QType { get; set; }

        public string Name{ get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool Checkbox { get; set; }
        public string attachName { get; set; }

        public string QurAttachments { get; set; }

        public string QDateStr { get; set; }
        public string FQDateStr { get; set; }

        public long? suppId { get; set; }

        public string contPers { get; set; }

        public string comp { get; set; }

        public string propAttac { get; set; }

        public string propDate { get; set; }

    }
}