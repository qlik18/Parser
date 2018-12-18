using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Xml.Linq;

namespace GUI.Implementation
{
    public static class GeneralStatic
    {
        public static List<UserInfo> Developers 
        {
            get
            {
                XDocument xml = XDocument.Load("HeliosDevelopers.xml");
                var q = from dev in xml.Element("developers").Elements("developer")
                        select new UserInfo()
                        {
                            id = 0,
                            firstname = dev.Attribute("firstname").Value,
                            surname = dev.Attribute("surname").Value,
                            login = dev.Attribute("login").Value,
                            email = dev.Attribute("email").Value
                        };
                return q.ToList();
                }   
        }
            /*
            = new List<UserInfo>(){
            new UserInfo() { id = 0, firstname = "Sebastian", surname = "Brzozowski", login = "sbrzozowski" },
            new UserInfo() { id = 0, firstname = "Krzysztof", surname = "Pieńkowski", login = "kpienkowski" },
            new UserInfo() { id = 0, firstname = "Michał", surname = "Minicki", login = "mminicki" },
            new UserInfo() { id = 0, firstname = "Katarzyna", surname = "Szylin", login = "knowik" },
            new UserInfo() { id = 0, firstname = "Krzysztof", surname = "Pudłowski", login = "kpudłowski" },
            new UserInfo() { id = 0, firstname = "Łukasz", surname = "Pietrzak", login = "lpietrzak" },
            new UserInfo() { id = 0, firstname = "Tadeusz", surname = "Cichy", login = "tcichy" },
            new UserInfo() { id = 0, firstname = "Adam", surname = "Paździoch", login = "apazdzioch" },
            new UserInfo() { id = 0, firstname = "Tomasz", surname = "Kacperak", login = "tkacperak" },
            new UserInfo() { id = 0, firstname = "Romuald", surname = "Kiliszek", login = "rkiliszek" },
            new UserInfo() { id = 0, firstname = "Jakub", surname = "Kowalczuk", login = "jkowalczuk" },
            new UserInfo() { id = 0, firstname = "Piotr", surname = "Jawad Atheer", login = "ajawad" }
             
        };
    */
        public static List<Entities.HeliosUser> PolsatUsers;
        public static List<Komponent> listaKomp = new List<Komponent>();

    }
    public class UserInfo{
        public int id;
        public string firstname;
        public string surname;
        public string login;
        public string email;
        public override string ToString()
        {
            return firstname + ' ' + surname;
        }
    }
}
