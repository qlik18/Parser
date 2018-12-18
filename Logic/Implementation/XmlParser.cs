using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Atlassian.Jira;

namespace Logic.Implementation
{
    public class XmlParser
    {
        public static List<Entities.Note> ReadNotesXML(string xmlString)
        {
            StringBuilder output = new StringBuilder();
            List<Entities.Note> result = new List<Entities.Note>();

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    string nrZgloszenia = "";
                    string content = "";
                    string author = "";
                    string date = "";

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "NrZgloszenia": reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        nrZgloszenia = reader.Value;
                                    break;
                                case "content": reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        content = reader.Value;
                                    break;
                                case "author": reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        author = reader.Value;
                                    break;
                                case "date": reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        date = reader.Value;
                                    break;
                            }
                        }
                        else
                        {
                            if (reader.Name == "r")
                            {
                                Entities.Note note = new Entities.Note()
                                {
                                    issueNumber = nrZgloszenia,
                                    author = author,
                                    date = date.Replace('T', ' '),
                                    content = content
                                };

                                result.Add(note);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public static StringBuilder IssuesXML(List<Entities.BillingIssueDto> issues)
        {
            if (issues.Count == 0)
                return new StringBuilder();

            StringBuilder output = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            Encoding utf8 = new UTF8Encoding(false);
            setting.Encoding = utf8;
            setting.Indent = true;

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, setting))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Issues");

                    foreach (var item in issues)
                    {
                        writer.WriteStartElement("Issue");
                        writer.WriteElementString("ID", item.issueWFS.NumerZgloszenia);
                        writer.WriteElementString("Numer", item.Idnumber.ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                output.Append(Encoding.Default.GetString(stream.ToArray()));
            }

            return output;
        }

        public static StringBuilder ProjectsXML(List<string> projects)
        {
            if (projects.Count == 0)
                return new StringBuilder();

            StringBuilder output = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            Encoding utf8 = new UTF8Encoding(false);
            setting.Encoding = utf8;
            setting.Indent = true;

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, setting))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Projects");

                    foreach (var item in projects)
                    {
                        writer.WriteElementString("Project", item);
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                output.Append(Encoding.Default.GetString(stream.ToArray()));
            }

            return output;
        }

        public static StringBuilder ZCCRaportFromXML(string xmlString)
        {
            StringBuilder output = new StringBuilder();

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Zgloszenia":
                                string obszar;
                                string nazwa;
                                string ilosc;
                                reader.MoveToFirstAttribute();
                                nazwa = reader.Value;
                                reader.MoveToNextAttribute();
                                obszar = reader.Value;
                                reader.MoveToNextAttribute();
                                ilosc = reader.Value;

                                output.AppendLine();
                                output.AppendLine("Obszar: " + obszar.ToUpper());
                                output.AppendLine("  " + nazwa + ": " + ilosc);
                                break;
                            case "Rodzaj":
                                string rodzaj;
                                string ilosc1;

                                reader.MoveToFirstAttribute();
                                rodzaj = reader.Value;
                                reader.MoveToNextAttribute();
                                ilosc1 = reader.Value;

                                if (rodzaj == "Liczba zgłoszeń zamkniętych")
                                {
                                    output.AppendLine();
                                    output.AppendLine();
                                }

                                output.AppendLine("    " + rodzaj + ": " + ilosc1);
                                break;
                            case "Typ":
                                string typ;
                                string ilosc2;

                                reader.MoveToFirstAttribute();
                                typ = reader.Value;
                                reader.MoveToNextAttribute();
                                ilosc2 = reader.Value;

                                output.AppendLine("      " + typ + ": " + ilosc2);
                                break;
                            case "Dev":
                                string dev;
                                string ilosc3;

                                reader.MoveToFirstAttribute();
                                dev = reader.Value;
                                reader.MoveToNextAttribute();
                                ilosc3 = reader.Value;

                                output.AppendLine("        " + dev + ": " + ilosc3);
                                break;
                        }
                    }
                }
            }

            return output;
        }

        public static StringBuilder ZCCRaportDziennyFromXML(string xmlString)
        {
            StringBuilder output = new StringBuilder();

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                bool header = false;
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "raport" :

                                int count = reader.AttributeCount;

                                reader.MoveToFirstAttribute();
                                if (reader.Name == "nazwa")
                                {
                                    string nazwa = reader.Value;
                                    reader.MoveToNextAttribute();
                                    string ile = reader.Value;

                                    if (count > 2)
                                    {
                                        reader.MoveToNextAttribute();
                                        string obszar = reader.Value;
                                        output.AppendLine("\t" + obszar.ToUpper() + ": " + ile);
                                    }
                                    else
                                    {
                                        output.AppendLine();
                                        output.AppendLine(nazwa + ": " + ile);
                                    }
                                }
                                else if (reader.Name == "NrZgloszenia")
                                {
                                    if (!header)
                                    {
                                        output.AppendLine();
                                        output.AppendLine("Zgłoszenia z dziś do realizacji:");
                                        output.AppendLine();
                                        header = true;
                                    }

                                    string nr = reader.Value;
                                    reader.MoveToNextAttribute();
                                    string content = reader.Value;
                                    output.AppendLine("/***************** " + nr + " *****************/");
                                    output.AppendLine(content);
                                    output.AppendLine("/*******************************************/");
                                    output.AppendLine();
                                }

                                break;
                        }
                    }
                }
            }

            return output;
        }

        public static StringBuilder CRMPodsumowanieFromXML(string xmlString)
        {
            StringBuilder output = new StringBuilder();

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    bool header = false;
                    bool flag = false;
                    string uzytkownik = "";
                    StringBuilder akcje = new StringBuilder();
                    string lacznie = "";

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Uzytkownik":
                                    
                                    reader.MoveToFirstAttribute();
                                    reader.MoveToNextAttribute();
                                    uzytkownik = reader.Value;

                                    if (uzytkownik == "Zgłoszenia, które na nas pozostają" || uzytkownik == "Łącznie zakończonych")
                                    {
                                        output.AppendLine();
                                        flag = true;
                                        reader.MoveToNextAttribute();
                                        lacznie = reader.Value;
                                        output.AppendLine(uzytkownik + ": " + lacznie);
                                        header = true;
                                    }
                                    else
                                        flag = false;
                                    break;
                                case "Akcja":
                                    if (flag)
                                    {
                                        if (header)
                                        {
                                            output.AppendLine("W tym:");
                                            header = false;
                                        }
                                        reader.MoveToFirstAttribute();
                                        string akcja = reader.Value;
                                        reader.MoveToNextAttribute();
                                        string ile = reader.Value;

                                        output.AppendLine("\t" + akcja + ": " + ile);
                                    }
                                    break;
                                case "Dev":
                                    if (flag)
                                    {
                                        reader.MoveToFirstAttribute();
                                        string nazwa = reader.Value;
                                        reader.MoveToNextAttribute();
                                        string ile1 = reader.Value;

                                        output.AppendLine("\t\t" + nazwa + ": " + ile1);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (reader.Name == "Akcja")
                                output.AppendLine();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return output;
        }

        public static StringBuilder InsertProcessLogXML(List<Entities.nProcess> processes)
        {
            if (processes.Count == 0)
                return null;

            StringBuilder output = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            Encoding utf8 = new UTF8Encoding(false);
            setting.Encoding = utf8;
            setting.Indent = true;
            try
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(stream, setting))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("ProcessLog");

                        writer.WriteElementString("ProcesTypeId", processes[0].IdProcess.ToString());

                        writer.WriteStartElement("ProcessId");
                        foreach (var item in processes)
                        {
                            writer.WriteElementString("Id", item.Number.ToString());
                        }
                        writer.WriteEndElement();

                        writer.WriteElementString("SolutionId", processes[0].IdSolutions.ToString());
                        writer.WriteElementString("ErrorId", processes[0].IdError.ToString());
                        writer.WriteElementString("Comment", processes[0].Comment);
                        writer.WriteElementString("InsertDate", DateTime.Now.ToString());
                        writer.WriteElementString("User", processes[0].Author);

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }

                    output.Append(Encoding.Default.GetString(stream.ToArray()));
                }

                return output;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static StringBuilder IssuesToSlaRaportXML(List<Issue> issues)
        {
            if (issues.Count == 0)
                return new StringBuilder();

            StringBuilder output = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            Encoding utf8 = new UTF8Encoding(false);
            setting.Encoding = utf8;
            setting.Indent = true;

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, setting))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Jira");

                    foreach (var item in issues)
                    {
                        writer.WriteStartElement("NumerJira");
                        writer.WriteAttributeString("id", item.Key.Value);


                        string priorytet = string.Empty;

                        switch(item.Priority.Id)
                        {
                            case "1": priorytet = "Blokujacy"; break;
                            case "2": priorytet = "Krytyczny"; break;
                            case "3": priorytet = "Wazny"; break;
                            case "4": priorytet = "Sredni"; break;
                            case "5": priorytet = "Niski"; break;
                        }

                        writer.WriteElementString("priorytet", priorytet);

                        string typ = string.Empty;
                        if (item.Type.Name.Equals("Zlecenie operatorskie", StringComparison.CurrentCultureIgnoreCase))
                            typ = "Zlecenie operatorskie";
                        else if (item.Type.Name.Equals("Incydent", StringComparison.CurrentCultureIgnoreCase))
                            typ = "Incydent";
                        else if (item.Type.Name.Equals("Problem", StringComparison.CurrentCultureIgnoreCase))
                            typ = "Problem";
                        else if (item.Type.Name.Equals("Błąd - system produkcyjny", StringComparison.CurrentCultureIgnoreCase))
                            typ = "Błąd produkcyjny";
                        else if (item.Type.Name.Equals("Błąd masowy - system produkcyjny", StringComparison.CurrentCultureIgnoreCase))
                            typ = "Błąd produkcyjny";
                        else
                            throw new InvalidOperationException(string.Format("Zgłoszenie {0} ma nieobsługiwany typ! ({1})", item.Key.Value, item.Type.Name));

                        writer.WriteElementString("Typ", typ);
                        writer.WriteElementString("DataUtworzenia", item.Updated.Value.ToString());

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                output.Append(Encoding.Default.GetString(stream.ToArray()));
            }

            return output;
        }

        public static StringBuilder IssuesSla(List<string> issues)
        {
            if (issues.Count == 0)
                return new StringBuilder();

            StringBuilder output = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            Encoding utf8 = new UTF8Encoding(false);
            setting.Encoding = utf8;
            setting.Indent = true;

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, setting))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Issues");

                    foreach (var item in issues)
                    {
                        writer.WriteStartElement("row");
                        writer.WriteAttributeString("IssueId", item);
                        //writer.WriteElementString("IssueId", item);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                output.Append(Encoding.Default.GetString(stream.ToArray()));
            }

            return output;
        }
    }
}
