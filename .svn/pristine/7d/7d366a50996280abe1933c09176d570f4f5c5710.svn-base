using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorldRef.Models;

namespace WorldRef.Models
{
    public class TrainingSampleOrderDAO
    {
        public long Id { get; set; }
        public Nullable<long> TrainerId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderHr { get; set; }
        public string OrderMin { get; set; }
        public string OrderAm { get; set; }
        public string Email { get; set; }
        public string DateStr { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
       

        public List<TrainingSampleOrderDAO> GetHourList { get; set; }
        public List<TrainingSampleOrderDAO> GetMinList { get; set; }
        public List<TrainingSampleOrderDAO> GetAmList { get; set; }

        public List<TrainingSampleOrderDAO> GetHour()
        {
            List<TrainingSampleOrderDAO> list = new List<TrainingSampleOrderDAO>();

            list.Add(new TrainingSampleOrderDAO() { Name = "-Hour-", Value = "0" });

            for (int i = 1; i <= 12; i++)
            {
                list.Add(new TrainingSampleOrderDAO() { Name = i.ToString(), Value = i.ToString() });
            }
            
            return list;
        }
        public List<TrainingSampleOrderDAO> GetMin()
        {
            List<TrainingSampleOrderDAO> list = new List<TrainingSampleOrderDAO>();

            list.Add(new TrainingSampleOrderDAO() { Name = "-Minute-", Value = "0" });

            for (int i = 1; i <= 60; i++)
            {
                list.Add(new TrainingSampleOrderDAO() { Name = i.ToString() , Value = i.ToString() });
            }
            return list;
        }
        public List<TrainingSampleOrderDAO> GetAm()
        {
            List<TrainingSampleOrderDAO> list = new List<TrainingSampleOrderDAO>();
            
                
            list.Add(new TrainingSampleOrderDAO() { Name = "AM", Value = "AM" });

            list.Add(new TrainingSampleOrderDAO() { Name = "PM", Value = "PM" });

            return list;
        }
    }
}