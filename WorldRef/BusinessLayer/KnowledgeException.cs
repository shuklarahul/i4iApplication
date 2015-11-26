using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WorldRef.BusinessLayer
{
    //Implement log4net functionality

    public class KnowledgeException : Exception
    {
        public KnowledgeException(string message)
            : base(message)
        {
            //to do 

        }

        public KnowledgeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            //to do
        }

        public KnowledgeException(string message, Exception innerException)
            : base(message, innerException)
        {
            //to do
        }

    }
}