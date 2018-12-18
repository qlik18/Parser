using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class ProcessError
    {
        public int id { get; set; }
        public int idErrorType { get; set; }
        public string description { get; set; }
        public string descriptionFull { get; set; }
    }
}
