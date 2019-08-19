using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Entities
{
    [DataContract]
    public class UserBpmJira 
    {
        [DataMember]
        public UserBpm UserBpm { set; get; }
        [DataMember]
        public UserJira UserJira { set; get; }


        public UserBpmJira()
        {
            return;
        }
        public UserBpmJira(UserBpm userBpm, UserJira userJira)
        {
            this.UserBpm = userBpm;
            this.UserJira = userJira;
        }


    }

    public class UserBpmJiraList
    {
        [DataMember]
        public List<UserBpmJira> UserBpmJira { set; get; }



        public UserBpmJiraList()
        {
            return;
        }
        public UserBpmJiraList(UserBpmJira user)
        {
            this.UserBpmJira.Add(user);
        }
        public UserBpmJiraList(List<UserBpmJira> user)
        {

            this.UserBpmJira = user;
        }

        public List<UserBpmJira> GetAllUserUserBpmJira()
        {
            return this.UserBpmJira.ToList();
        }

        public bool IsBillUser(string userJira)
        {
            return UserBpmJira.Exists(x => x.UserJira.login == userJira);
        }

        public KeyValuePair<int, string> GetBillUser(string userJira)
        {
            KeyValuePair<int, string> resTmp = new KeyValuePair<int, string>();
            UserBpmJira tmp = this.UserBpmJira.FirstOrDefault(x => x.UserJira.login == userJira);
            if (tmp != null)
            {
                resTmp = new KeyValuePair<int, string>(tmp.UserBpm.Id, tmp.UserBpm.FullName);
            }
            return resTmp;
        }
        public bool TryGetBillUser(string userJira, out UserBpmJira ubj)
        {
            UserBpmJira ubjtmp = new Entities.UserBpmJira();

            bool res = IsBillUser(userJira);
            if(!res)
            {
                ubj = ubjtmp;
                return res;
            }

            foreach (var item in UserBpmJira)
            {
                if (item.UserJira.login == userJira)
                {
                    ubjtmp = item;
                    break;
                }
            }

            //////

            ubj = ubjtmp;
            return res;
        }
    }

}
