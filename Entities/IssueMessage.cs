using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class IssueMessage
    {
        public string issuenumber { get; set; }
        public DateTime date { get; set; }
        public bool isNew { get; set; }
        public bool isWarning { get; set; }
    }
}
