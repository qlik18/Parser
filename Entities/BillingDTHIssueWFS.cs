using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class BillingDTHIssueWFS
    {
        String numerZgloszenia = "";
        String tytulZgloszenia = "";
        String imie = "";
        String nazwisko = "";
        String email = "";
        String dataWystapieniaBledu = "";
        String dataIGodzinaUtworzeniaZgloszenia = "";
        String dataIGodzinaOstatniegoKomentarza = "";
        String idKontraktu = "";
        Component system;
        Component kategoria;
        Component rodzaj;
        Component typ;
        String trescZgloszenia = "";
        String wfsState = "";
        String idZamowienia = "";
        String priorytet = "";
        String jiraIdentifier = "";
        String czyOnCall = "";
        String srodowiskoProblemu = "";
        #region Parametry po zmianach
        [DataMember]
        public int WFSIssueId { get; set; }
        [DataMember]
        public string WFSState
        {
            get { return wfsState; }
            set { wfsState = value; }
        }
        #endregion
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
        public String Email
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public String DataWystapieniaBledu
        {
            get { return dataWystapieniaBledu; }
            set { dataWystapieniaBledu = value; }
        }
        [DataMember]
        public String DataIGodzinaUtworzeniaZgloszenia
        {
            get { return dataIGodzinaUtworzeniaZgloszenia; }
            set { dataIGodzinaUtworzeniaZgloszenia = value; }
        }
        [DataMember]
        public String DataIGodzinaOstatniegoKomentarza
        {
            get { return dataIGodzinaOstatniegoKomentarza; }
            set { dataIGodzinaOstatniegoKomentarza = value; }
        }
        [DataMember]
        public String IdKontraktu
        {
            get { return idKontraktu; }
            set { idKontraktu = value; }
        }
        [DataMember]
        public Component System
        {
            get { return system; }
            set { system = value; }
        }
        [DataMember]
        public Component Kategoria
        {
            get { return kategoria; }
            set { kategoria = value; }
        }
        [DataMember]
        public Component Rodzaj
        {
            get { return rodzaj; }
            set { rodzaj = value; }
        }
        [DataMember]
        public Component Typ
        {
            get { return typ; }
            set { typ = value; }
        }
        [DataMember]
        public String TrescZgloszenia
        {
            get { return trescZgloszenia; }
            set { trescZgloszenia = value; }
        }
        [DataMember]
        public String IdZamowienia
        {
            get { return idZamowienia; }
            set { idZamowienia = value; }
        }
        [DataMember]
        public String Priorytet
        {
            get { return priorytet; }
            set { priorytet = value; }
        }
        [DataMember]
        public String JiraId
        {
            get { return jiraIdentifier; }
            set { jiraIdentifier = value; }
        }

        [DataMember]
        public String CzyOnCall
        {
            get { return czyOnCall; }
            set { czyOnCall = value; }
        }
        [DataMember]
        public String SrodowiskoProblemu
        {
            get { return srodowiskoProblemu; }
            set { srodowiskoProblemu = value; }
        }

    }
}
