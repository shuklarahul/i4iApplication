using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ConversationDAO
    {

        public int ConversationID { get; set; }
        public int ProjectID { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Proposalfile { get; set; }
        public string SentBy { get; set; }
        public string Subject { get; set; }
        public string FileAttachPath { get; set; }
        public List<ConversationDAO> _list { get; set; }
    }
}