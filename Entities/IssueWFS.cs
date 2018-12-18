using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class IssueWFS
    {
        String imie = "";
        String nazwisko = "";
        String emailLogin = "";
        String telefonKontaktowy = "";
        String dataIGodzina = "";
        String kategoria = "";
        String czyZglaszany = "False";
        String tresc = "";
        String wstepneRozpoznanie = "";
        String system = "";
        String komponent = "";
        String uwagiSerwisanta = "";
        String numerZgloszenia = "";
        String tytulZgloszenia = "";
        String czyInneKomputery = "False";
        String wfsState = "";
        #region Parametry po zmianach
        [DataMember]
        public int WFSIssueId { get; set; }
        [DataMember]
        public string WFSState {
            get { return wfsState; }
            set { wfsState = value; }
        }
        #endregion
        [DataMember]
        public int IdKomponentu { get; set; }
        [DataMember]
        public int IdKategorii { get; set; }
        [DataMember]
        public int IdWstepnegoRozpoznania { get; set; }
        [DataMember]
        public int IdSystemu { get; set; }
        [DataMember]
        public String Imie
        {
            get { return imie; }
            set { imie = value; }
        }
        [DataMember]
        public String Nazwisko
        {
            get { return nazwisko; }
            set { nazwisko = value; }
        }
        [DataMember]
        public String TelefonKontaktowy
        {
            get { return telefonKontaktowy; }
            set { telefonKontaktowy = value; }
        }
        [DataMember]
        public String DataIGodzina
        {
            get { return dataIGodzina; }
            set { dataIGodzina = value; }
        }
        [DataMember]
        public String Kategoria
        {
            get { return kategoria; }
            set { kategoria = value; }
        }
        [DataMember]
        public String CzyZglaszany
        {
            get { return czyZglaszany; }
            set { czyZglaszany = value; }
        }
        [DataMember]
        public String Tresc
        {
            get { return tresc; }
            set { tresc = value; }
        }
        [DataMember]
        public String EmailLogin
        {
            get { return emailLogin; }
            set { emailLogin = value; }
        }
        [DataMember]
        public String WstepneRozpoznanie
        {
            get { return wstepneRozpoznanie; }
            set { wstepneRozpoznanie = value; }
        }
        [DataMember]
        public String System
        {
            get { return system; }
            set { system = value; }
        }
        [DataMember]
        public String Komponent
        {
            get { return komponent; }
            set { komponent = value; }
        }
        [DataMember]
        public String UwagiSerwisanta
        {
            get { return uwagiSerwisanta; }
            set { uwagiSerwisanta = value; }
        }
        [DataMember]
        public String NumerZgloszenia
        {
            get { return numerZgloszenia; }
            set { numerZgloszenia = value; }
        }
        [DataMember]
        public String TytulZgloszenia
        {
            get { return tytulZgloszenia; }
            set { tytulZgloszenia = value; }
        }
        [DataMember]
        public String CzyInneKomputery
        {
            get { return czyInneKomputery; }
            set { czyInneKomputery = value; }
        }
    }
    
}
