using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    public class Component
    {
        string text;
        int value;

        [DataMember]
        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        [DataMember]
        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }
}
