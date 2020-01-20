using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Enums
{
    public enum IssueState 
    {
        NEW = 0, 
        INWFS = 1, 
        READYTOSAVE = 2, 
        SELECTED = 3,
        CLOSED = 4
    }
}
