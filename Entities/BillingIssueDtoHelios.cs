using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class BillingIssueDtoHelios : BillingIssueDto
    {
        public override string Idnumber
        {
            get
            {
                return issueHelios.number;
            }
            set
            {
                issueHelios.number = value;
            }
        }
        public IssueHelios issueHelios { get; set; }
        public Note note { get; set; }     
    }
}
