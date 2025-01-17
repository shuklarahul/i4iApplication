﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace WorldRef.DataLayer
{

    /// <summary>
    /// It will have all the operation on the Table
    /// </summary>
    public class SignUpWorldRef : ISignUpWorldRef
    {
        private I4IDBEntities context = new I4IDBEntities();
        private string _userRole = "C";


        /// <summary>
        /// Register worlref user
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public string Add(SignUpWorldRefModel signModel)
        {
            string ReturnStatus = "Success";
            string UserRole = "W";
            try
            {
                RegisterUser register = new RegisterUser()
                {

                    UserFirstName = signModel.Name,
                    Company = signModel.Name,
                    Type = signModel.Type,
                    Email = signModel.Email,
                    phone = signModel.ContactNumber,
                    Industries = signModel.Industry,
                    OfficialNumber=signModel.OfficialNumber,
                    CountryName = signModel.Country.ToString(),
                    BusinessInterestCountry = signModel.BusinessInterestCountry.ToString(),
                    ProfileAttach = signModel.ProfilePath,
                    PhotoAttach = signModel.ProfileFileName,
                    UserNo = signModel.UserName,
                    Password = signModel.Password,
                    UserRole = UserRole,
                    UploaderType=signModel.UploaderType,
                    OrganisationName=signModel.OrganisationName,
                    BussinessUnitName=signModel.BusinessUnitName,
                    MyCompany=signModel.MyCompany,
                    RecoveryMail=signModel.RecoveryMail,
                    OtherMail = signModel.OtherMail,
                    ProfileUrl = signModel.ProfileUrl,
                    ProfileLanguageID = Convert.ToInt16(signModel.Language),
                    Date = DateTime.Now
                };

                context.RegisterUsers.Add(register);
                context.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return ReturnStatus;
        }

        /// <summary>
        /// Update Detail of Worldref user
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public string Save(SignUpWorldRefModel signModel)
        {
            string ReturnStatus = "Success";
            try
            {
                LoginDetailWorldRef loginDetail = context.LoginDetailWorldRefs.Where(x => x.id == signModel.Id).FirstOrDefault();

                //loginDetail.Name            = signModel.Name;
                //loginDetail.Type            = signModel.Type;
                //loginDetail.Email           = signModel.Email;
                //loginDetail.Industry        = signModel.Industry;
                //loginDetail.Country         = signModel.Country;
                //loginDetail.ProfileName     = signModel.ProfileFileName;
                //loginDetail.ProfilePath     = signModel.ProfilePath;
                //loginDetail.dateModified    = DateTime.Now;

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                ReturnStatus = ex.Message;
            }
            return ReturnStatus;
        }
        /// <summary>
        /// Login The worldref model
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public SignUpWorldRefModel Login(string userName, string Password)
        {
            SignUpWorldRefModel signUp = null;

            RegisterUser loginDetail = context.RegisterUsers.Where(x => x.UserNo == userName && x.Password == Password && (x.UserRole == "W" || x.UserRole == "P" || x.UserRole == "C")).FirstOrDefault();

            if (loginDetail != null)
            {
                signUp = new SignUpWorldRefModel()
                {
                    Id = loginDetail.Id,
                    Name = loginDetail.UserFirstName,
                    UserRole = loginDetail.UserRole,
                    PhotoAttach = loginDetail.PhotoAttach


                };
            }

            return signUp;

        }

        /// <summary>
        /// Send email when user fill forget the username and password
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public SignUpWorldRefModel ForgetPassword(string Email)
        {
            SignUpWorldRefModel signModel = null;

            try
            {
                RegisterUser loginDetail = context.RegisterUsers.Where(x => x.Email == Email && x.UserRole == "W").FirstOrDefault();

                if (loginDetail != null)
                {
                    signModel = new SignUpWorldRefModel();
                    signModel.Name = loginDetail.UserFirstName;
                    signModel.Email = loginDetail.Email;
                    signModel.UserName = loginDetail.UserNo;
                    signModel.Password = loginDetail.Password;
                }
            }
            catch
            {
            }
            return signModel;
        }

        /// <summary>
        /// Sign Up for the common user
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="ContactNumber"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CPerson"></param>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string SignUpLikeUser(string CPerson, string CompanyName, string ContactNumber, string emailId, string password, string UserName, string filename)
        {
            try
            {
                RegisterUser CheckRegister = (from user in context.RegisterUsers
                                              where user.Email.ToLower() == emailId.ToLower()
                                              select user).FirstOrDefault();
                if (CheckRegister == null)
                {
                    CheckRegister = (from user in context.RegisterUsers
                                     where user.UserNo.ToLower() == UserName.ToLower()
                                     select user).FirstOrDefault();

                    if (CheckRegister == null)
                    {
                        context.RegisterUsers.Add(new RegisterUser()
                        {
                            UserNo = UserName,
                            Password = password,
                            Email = emailId,
                            phone = ContactNumber,
                            Company = CompanyName,
                            UserFirstName = CPerson,
                            UserRole = _userRole,
                            PhotoAttach = filename,
                            Date = DateTime.Now
                        });

                        context.SaveChanges();
                    }
                    else
                    {
                        return "User Name already exists.";
                    }

                }
                else
                {
                    return "Email Id already exists";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "User Registered Successfully .Please sign in with your email id and password to continue.";
        }

        public SignUpWorldRefModel SignInCommonUser(string UserName, string Password)
        {
            try
            {

                SignUpWorldRefModel signUp = null;

                RegisterUser loginDetail = context.RegisterUsers.Where(x => x.UserNo.ToLower() == UserName.ToLower() && x.Password == Password && x.UserRole == _userRole).FirstOrDefault();

                if (loginDetail != null)
                {
                    signUp = new SignUpWorldRefModel()
                    {
                        Id = loginDetail.Id,
                        Name = loginDetail.UserFirstName
                    };
                }

                return signUp;
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

    }
}