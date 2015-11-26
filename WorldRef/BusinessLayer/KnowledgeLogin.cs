using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer
{
    public class KnowledgeLogin : BaseAccessor, IknowledgeLogin
    {
        #region private Members

        private IKnowledgeLoginModel _loginModel = null;
        private const string _knowledgeUserRole = "K";

        #endregion

        #region Constructor

        public KnowledgeLogin()
        {
            _loginModel = new IKnowledgeLoginModel();
        }

        #endregion

        #region Public Members

        public IKnowledgeLoginModel SignIn(string Username, string Password)
        {
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException("User Name Can not be null");
            if (string.IsNullOrEmpty(Password)) throw new ArgumentNullException("Password can not be null");
            try
            {
                RegisterUser userDetail = (from regUser in DatabaseConnection.RegisterUsers
                                           where regUser.Email == Username && regUser.Password == Password
                                           && regUser.UserRole == _knowledgeUserRole
                                           select regUser).FirstOrDefault();

                if (userDetail != null)
                {
                    _loginModel.EmailId = userDetail.Email;
                    _loginModel.Id = userDetail.Id;
                }
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }
            return _loginModel;
        }

        public IKnowledgeLoginModel SignUp(IKnowledgeLoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Name)) throw new ArgumentNullException("Name can not be null");
            if (string.IsNullOrEmpty(loginModel.OrganizationName)) throw new ArgumentNullException("Organization can not be null");
            if (string.IsNullOrEmpty(loginModel.EmailId)) throw new ArgumentNullException("Email Id can not be null");
            if (string.IsNullOrEmpty(loginModel.Password)) throw new ArgumentNullException("Password can not be null");

            string returnStatus = string.Empty;
            try
            {
                DatabaseConnection.RegisterUsers.Add(new RegisterUser()
                {
                    Email = loginModel.EmailId,
                    UserFirstName = loginModel.Name,
                    UserLastName = loginModel.OrganizationName,
                    Password = loginModel.Password
                });

                DatabaseConnection.SaveChanges();

                RegisterUser userDetail = (from regUser in DatabaseConnection.RegisterUsers
                                           where regUser.Email == loginModel.EmailId && regUser.Password == loginModel.Password
                                           && regUser.UserRole == _knowledgeUserRole
                                           select regUser).FirstOrDefault();

                if (userDetail != null)
                {
                    _loginModel.EmailId = userDetail.Email;
                    _loginModel.Id = userDetail.Id;
                }

            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }
            return _loginModel;
        }

        public string Forgetpassword(string EmailId)
        {
            if (string.IsNullOrEmpty(EmailId)) throw new ArgumentNullException("Email Id can not be null");

            string password = (from regUser in DatabaseConnection.RegisterUsers
                               where regUser.Email == EmailId
                               && regUser.UserRole == _knowledgeUserRole
                               select regUser.Password).FirstOrDefault();
            return password;
        }

        public string ChangeUserPassword(string EmailId, string oldpassword, string newPassword)
        {
            if (string.IsNullOrEmpty(EmailId)) throw new ArgumentNullException("Email Id can not be null");
            if (string.IsNullOrEmpty(oldpassword)) throw new ArgumentNullException("Old Password  can not be null");
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentNullException(" New Password Can not be null");

            string returnStatus = string.Empty;
            try
            {
                RegisterUser userDetail = (from regUser in DatabaseConnection.RegisterUsers
                                           where regUser.Email == EmailId && regUser.Password == oldpassword
                                           && regUser.UserRole == _knowledgeUserRole
                                           select regUser).FirstOrDefault();
                if (userDetail != null)
                {
                    userDetail.Password = newPassword;
                    DatabaseConnection.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                returnStatus = "Fail";
                new KnowledgeException(ex.Message);
            }
            return returnStatus;
        }

        #endregion
    }
}