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

    

}
