using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer
{
    public class BaseAccessor
    {
        private readonly I4IDBEntities context = null;

        public BaseAccessor()
        {
            context = new I4IDBEntities();
        }

        public I4IDBEntities DatabaseConnection
        {
            get
            {
                return context;
            }
        }
    }
}