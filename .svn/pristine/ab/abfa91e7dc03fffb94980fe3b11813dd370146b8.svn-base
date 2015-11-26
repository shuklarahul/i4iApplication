using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ForwardQueryDAO
    {
        public long Id { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public Nullable<long> QueryId { get; set; }
        public List<SupplierDAO> SupplierList { get; set; }
        public List<InputQueryDataDAO> QueriesList { get; set; }

        public List<SupplierDAO> GetSupplier()
        {
            List<SupplierDAO> list = new List<SupplierDAO>();
            try
            {
                using ( I4IDBEntities context = new  I4IDBEntities())
                {
                    SupplierDAO obj = new SupplierDAO();
                    //var data= context.Suppliers ;
                    var data = context.RegisterUsers.Where(u => u.UserRole == "S");
                    //var data=from sup in context.Suppliers join forwQry in context.ForwardQueries on sup.;

                    foreach(RegisterUser item in data)
                    {
                        list.Add(new SupplierDAO {Id=item.Id,Company=item.Company +"-" +item.UserFirstName +" " +item.UserLastName });
                    }
                }
            }
            catch
            {
            }
            return list;
        }
        public List<InputQueryDataDAO> GetQueries()
        {
            List<InputQueryDataDAO> list = new List<InputQueryDataDAO>();
            try
            {
                using ( I4IDBEntities context = new  I4IDBEntities())
                {
                    InputQueryDataDAO obj = new InputQueryDataDAO();

                    var data = (from query in context.InputQueryDatas
                                join userDetail in context.RegisterUsers on query.UserNo equals userDetail.UserNo
                                //where qur.acion == "True" && !itemIds.Contains(query.Id) && qur.Supplier == SID
                        
                            select new
                            {
                                Id = query.Id,
                                UQueryNum = query.UQueryNum,
                                Name = userDetail.UserFirstName + " " + userDetail.UserLastName,
                                Email = userDetail.Email,
                                Phone = userDetail.phone,
                                //QueryText=query.QueryText,
                                acion = query.acion,
                                QDate = query.QDate,
                                Attachments=query.Attachments
                            }).OrderByDescending(m => m.QDate);
                    //var data = context.InputQueryDatas;

                    
                    //from inpQ in context.InputQueryDatas 
                    //           join frwQ in context.ForwardQueries on inpQ.Id equals frwQ.QueryId 
                    //           select new 
                    //           {

                    //           };
                    foreach (var item in data)
                    {
                        list.Add(new InputQueryDataDAO { UQueryNum = item.UQueryNum, Name = item.Name, Email = item.Email, Phone = item.Phone, acion = item.acion, Checkbox = false });//, QueryText = item.QueryText
                    }
                }
            }
            catch
            {
            }
            return list;
        }
    }
}