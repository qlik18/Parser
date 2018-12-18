using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{

    public class IssueHelios
    {
        public String number {get; set; }
        public String jiraIdentifier { get; set; }
        public String title { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String email { get; set; }
        public String status { get; set; }
        public String severity { get; set; }
        public String date { get; set; }
        public String updated { get; set; }
        public String time { get; set; }
        public String content { get; set; }
        public List<Note> Comments { get; set; }
        public string assigned_to { get; set; }
        public string rodzaj_zgloszenia { get; set; }
        public string idKontraktu { get; set; }
        public string idKonta { get; set; }
        public string idZamowienia { get; set; }
        public string rodzaj_bledu { get; set; }
        public string projekt { get; set; }
        public string oryginalneId { get; set; }
        /*Zmiany w procesie*/
        public String czyOnCall { get; set; }
        public String srodowiskoProblemu { get; set; }
    }
           
}
