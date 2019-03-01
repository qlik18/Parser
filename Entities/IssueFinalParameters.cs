using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class IssueFinalParameters
    {
        public BillingIssueDto issueNumber { set; get; }
        public KeyValuePair<int, string> item { set; get; }
        public KeyValuePair<int, string> selectOption { set; get; }


        public IssueFinalParameters()
        {
            return;
        }
        public IssueFinalParameters(BillingIssueDto _issueNumber, KeyValuePair<int, string> _item, KeyValuePair<int, string> _selectOption)
        {
            this.issueNumber = _issueNumber;
            this.item = _item;
            this.selectOption = _selectOption;
        }
    }
}


/*
 
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

     */
