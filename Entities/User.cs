using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public String login { set; get; }
        [DataMember]
        public String password { set; get; }
        [DataMember]
        public String name { set; get; }
        [DataMember]
        public String surname { set; get; }

        public User(String login, String password)
        {
            this.login = login;
            this.password = password;
        }
        public User(int Id, String login, String password, String name, String surname)
        {
            this.Id = Id;
            this.login = login;
            this.password = password;
            this.name = name;
            this.surname = surname;
        }
        public string FullName
        {
            get
            { return name + " " + surname; }
        }
    }
}
