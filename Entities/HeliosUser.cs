using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class HeliosUser
    {
        [DataMember]
        public String email { get; set; }
        [DataMember]
        public String nazwisko { get; set; }
        [DataMember]
        public String imie { get; set; }
        [DataMember]
        public String telefon { get; set; }

        public override string ToString()
        {
            return imie + " " + nazwisko;
        }
    }
}
