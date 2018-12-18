using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Note
    {
        public string issueNumber {set; get;}
        public int noteNumber { set; get; }
        public string author { set; get; }
        public string date { set; get; }
        public string content { set; get; }

        public bool isChanged = false;

        
    }
}
