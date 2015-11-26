using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
namespace WorldRef.Models
{
    public class DiaryEvent
    {

        public int AppointmentID;
        public int id;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;
        public string DateTimeScheduled;


        public static List<DiaryEvent> LoadAllAppointmentsInDateRange(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            using ( I4IDBEntities ent = new  I4IDBEntities())
            {
                var rslt = ent.AppointmentDiaries.Where(s => s.DateTimeScheduled >= fromDate && System.Data.Objects.EntityFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate);

                List<DiaryEvent> result = new List<DiaryEvent>();
                foreach (var item in rslt)
                {
                    DiaryEvent rec = new DiaryEvent();
                    rec.AppointmentID = item.AppointmentID;
                    rec.SomeImportantKeyID = item.SomeImportantKey;
                    rec.StartDateString = item.DateTimeScheduled.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                    rec.EndDateString = item.DateTimeScheduled.AddMinutes(item.AppointmentLength).ToString("s"); // field AppointmentLength is in minutes
                    rec.Title = item.Title + " - " + item.AppointmentLength.ToString() + " mins";
                    rec.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
                    rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
                    string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
                    rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1, rec.StatusColor.Length - ColorCode.Length - 1);
                    rec.StatusColor = ColorCode;
                    result.Add(rec);
                }

                return result;
            }

        }

        public static List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end)
        {

            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            using ( I4IDBEntities ent = new  I4IDBEntities())
            {
                var rslt = ent.AppointmentDiaries.Where(s => s.DateTimeScheduled >= fromDate && System.Data.Objects.EntityFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate)
                                                        .GroupBy(s => System.Data.Objects.EntityFunctions.TruncateTime(s.DateTimeScheduled))
                                                        .Select(x => new { DateTimeScheduled = x.Key, Count = x.Count() });

                List<DiaryEvent> result = new List<DiaryEvent>();
                int i = 0;
                foreach (var item in rslt)
                {
                    DiaryEvent rec = new DiaryEvent();
                    rec.AppointmentID = i; //we dont link this back to anything as its a group summary but the fullcalendar needs unique IDs for each event item (unless its a repeating event)
                    rec.SomeImportantKeyID = -1;
                    string StringDate = string.Format("{0:dd-MM-yyyy}", item.DateTimeScheduled);
                    rec.StartDateString = StringDate + "T00:00:00"; //ISO 8601 format
                    rec.EndDateString = StringDate + "T23:59:59";
                    rec.Title = "Booked: " + item.Count.ToString();
                    result.Add(rec);
                    i++;
                }

                return result;
            }

        }

        public static void UpdateDiaryEvent(int id, string NewEventStart, string NewEventEnd)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using ( I4IDBEntities ent = new  I4IDBEntities())
            {
                var rec = ent.AppointmentDiaries.FirstOrDefault(s => s.AppointmentID == id);
                if (rec != null)
                {
                    DateTime DateTimeStart = DateTime.Parse(NewEventStart, null, DateTimeStyles.RoundtripKind).ToLocalTime(); // and convert offset to localtime
                    rec.DateTimeScheduled = DateTimeStart;
                    if (!String.IsNullOrEmpty(NewEventEnd))
                    {
                        TimeSpan span = DateTime.Parse(NewEventEnd, null, DateTimeStyles.RoundtripKind).ToLocalTime() - DateTimeStart;
                        rec.AppointmentLength = Convert.ToInt32(span.TotalMinutes);
                    }
                    ent.SaveChanges();
                }
            }

        }


        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
      
        public static bool CreateNewEvent(string Title, string NewEventDate)
        {
            try
            {
                I4IDBEntities ent = new  I4IDBEntities();
                AppointmentDiary rec = new AppointmentDiary();
                string userNoCookiesVale = HttpContext.Current.Request.Cookies["UserNo"].Value;
                var result =(from r in ent.RegisterUsers
                                 where r.UserNo==userNoCookiesVale && r.UserRole=="T"
                                 select new {
                                 r.Id
                                 });

                rec.id = Convert.ToInt32(result.FirstOrDefault().Id);       
                rec.Title = Title;
                DateTime dt = DateTime.ParseExact(NewEventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                rec.DateTimeScheduled = dt;
                //rec.AppointmentLength = Int32.Parse(NewEventDuration);
                ent.AppointmentDiaries.Add(rec);
                ent.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                       validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                return false;
            }
          return true;
        }
    }
}