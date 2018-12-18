using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Enums;

namespace Entities
{
    public abstract class IssueDto
    {
        abstract public string Idnumber { get; set; }
        public bool isInWFS { set; get; }
        public IssueWFS issueWFS { get; set; }
        public IssueType Type;
    }
}
