using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
namespace WorldRef.BusinessLayer
{
    interface IknowledgeLogin
    {
        IKnowledgeLoginModel SignIn(string Username, string Password);

        IKnowledgeLoginModel SignUp(IKnowledgeLoginModel loginModel);

        string Forgetpassword(string EmailId);

        string ChangeUserPassword(string EmailId, string oldpassword, string newPassword);

    }
}