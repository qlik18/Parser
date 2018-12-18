using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /*
     * namedQuery -> nazwa zapytania skladowanego na Heliosie
             * number -> zwraca naglowek konkretnego zgloszenia
             * title -> tytul zawiera ten ciag znakow
             * content -> tresc zawiera ten ciag znakow
             * dateFrom - (format YYYY-MM-DD) -> zgloszenia zgloszone od tego okresu
             * dateTo - (format YYYY-MM-DD) -> zgloszenia zgloszone do tego okresu            
             * severity -> zgloszenia o danej wadze
             * resolution -> zgłoszenia o danym statusie rozwiązania
     */
    public class IssueCriteria
    {
        public String namedQuery { get; set; }
        public String number { set; get; }
        public String title { set; get; }
        public String content { set; get; }
        public String dateFrom { set; get; }
        public String dateTo { set; get; }
        public String severity { set; get; }
        public String resolution { set; get; }
    }
}
