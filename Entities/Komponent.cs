using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{   [DataContract]
    public class Diagnoza
        {
            [DataMember]
            public int IdDiagnozy { get; set; }
            [DataMember]
            public String NazwaDiagnozy { get; set; }
            public override string ToString()
            {
                return NazwaDiagnozy;
            }
        }
    [DataContract]
    public class Komponent
    {
        [DataMember]
        public int IdKomponentu { get; set; }
        [DataMember]
        public String NazwaKomponentu { get; set; }
        [DataMember]
        public List<Diagnoza> Diagnozy;

        public override string ToString()
        {
            return NazwaKomponentu;
        }
    }

        
}
