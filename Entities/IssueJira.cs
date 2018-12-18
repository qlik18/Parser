using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class IssueJira
    {
        public String NumerZgloszenia { get; set; }
        public String TytulZgloszenia { get; set; }
        public String TrescZgloszenia { get; set; }
        public String Imie { get; set; }
        public String Nazwisko { get; set; }
        public String DataIGodzina { get; set; }
        public IssueTypeJira RodzajZgloszenia { get; set; }
    }
    public class IssueTypeJira
    {
        public int IdRodzajuZgloszenia { get; set; }
        public string RodzajZgloszenia { get; set; }
    }
}
