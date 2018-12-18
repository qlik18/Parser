using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{

    public class UserJira
    {
        [DataMember]
        public String login { set; get; }

        [DataMember]
        public String FullName { set; get; }

        public UserJira(String login, String FullName)
        {
            this.login = login;
            this.FullName = FullName;
        }

    }
}