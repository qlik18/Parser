using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Entities
{
    public class UserBpm
    {
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public String login { set; get; }

        [DataMember]
        public String FullName { set; get; }
        [DataMember]
        public String Email { get { return login + "@billennium.com"; } }

        public UserBpm(String login, String FullName)
        {
            this.login = login;
            this.FullName = FullName;
        }
        public UserBpm(int Id, String login, String FullName)
        {
            this.Id = Id;
            this.login = login;
            this.FullName = FullName;
        }


    }
}
