using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.DataLayer
{
    interface ISignUpWorldRef
    {
        string Add(SignUpWorldRefModel signModel);

        string Save(SignUpWorldRefModel signModel);

        SignUpWorldRefModel Login(string userName, string Password);

        SignUpWorldRefModel ForgetPassword(string Email);
    }
}