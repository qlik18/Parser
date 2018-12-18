using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
   public class JiraLoggedUser
    {
        
            private string login;
            private string password;

            public String Login
            {
                get { return login; }
                set { login = value; }
            }
            public String Password
            {
                get { return password; }
                set { password = value; }
            }
        
    }
}
