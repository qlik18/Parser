using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class IssueDtoJira : IssueDto
    {
        public override string Idnumber
        {
            get
            {
                return issueJira.NumerZgloszenia;
            }
            set
            {
                issueJira.NumerZgloszenia = value;
            }
        }
        public IssueJira issueJira { get; set; }
    }
}
