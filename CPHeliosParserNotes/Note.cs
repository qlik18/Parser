using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CPJiraParserNotes
{
    // TODO dodać obsługę archiwizowania notatek

    /// <summary>
    /// Klasa umożliwiająca tworzenie, zapis, edycję i odczyt notatek skojarzonych ze zgłoszeniami  Heliosa.</summary>
    /// <remarks>
    /// Notatki przypisane do zgłoszenia o numerze 0 mogą pełnić rolę np. newsów czy swobodnych zapisków.
    /// Notatki są przechowywane we wskazanym pliku xml. Ponadto jest tworzony dodatkowy plik z rozszerzeniem .bu pełniący rolę backupu</remarks>
    public class Note
    {
        private int id;
        private int status;
        private int no;
        private String author;
        private DateTime published;
        private String content;
        private String path;

        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        public Note(String path)
        {
            this.path = path;
            this.status = 0;
            this.no = 0;
            this.author = "";
            this.published = DateTime.Now;
            this.content = "";
            this.id = DateTime.Now.GetHashCode();
        }
        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        /// <param name="content">Treść notatki</param>
        public Note(String path, String content)
        {
            this.path = path;
            this.status = 1;
            this.no = 0;
            this.author = "";
            this.published = DateTime.Now;
            this.content = content;
            this.id = DateTime.Now.GetHashCode();
        }
        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        /// <param name="content">Treść notatki</param>
        /// <param name="author">Autor notatki</param>
        public Note(String path, String content, String author)
        {
            this.path = path;
            this.status = 0;
            this.no = 0;
            this.author = author;
            this.published = DateTime.Now;
            this.content = content;
            this.id = DateTime.Now.GetHashCode();
        }
        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        /// <param name="content">Treść notatki</param>
        /// <param name="author">Autor notatki</param>
        /// <param name="published">Data dodania notatki</param>
        public Note(String path, String content, String author, DateTime published)
        {
            this.path = path;
            this.status = 0;
            this.no = 0;
            this.author = author;
            this.published = published;
            this.content = content;
            this.id = DateTime.Now.GetHashCode();
        }
        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        /// <param name="content">Treść notatki</param>
        /// <param name="author">Autor notatki</param>
        /// <param name="published">Data dodania notatki</param>
        /// <param name="no">Numer zgłoszenia, do którego odnosi się notatka</param>
        public Note(String path, String content, String author, DateTime published, int no)
        {
            this.path = path;
            this.status = 0;
            this.no = no;
            this.author = author;
            this.published = published;
            this.content = content;
            this.id = DateTime.Now.GetHashCode();
        }
        /// <summary>
        /// Konstruktor </summary>
        /// <param name="path">Ścieżka zapisu plików notatek</param>
        /// <param name="content">Treść notatki</param>
        /// <param name="author">Autor notatki</param>
        /// <param name="published">Data dodania notatki</param>
        /// <param name="no">Numer zgłoszenia, do którego odnosi się notatka</param>
        /// <param name="id">Indywidualny identyfikator notatki</param>
        public Note(String path, String content, String author, DateTime published, int no, int id)
        {
            this.path = path;
            this.status = 0;
            this.no = no;
            this.author = author;
            this.published = published;
            this.content = content;
            this.id = id;
        }

        /// <summary>
        /// Ustawia identyfikator notatki </summary>
        /// <param name="id">Nowy identyfikator</param>
        public Note SetId(int id)
        {
            this.id = id;
            return this;
        }
        /// <summary>
        /// Zwraca identyfikator notatki </summary>
        public int GetId()
        {
            return this.id;
        }
        /// <summary>
        /// Ustawia ścieżkę pliku notatki notatki </summary>
        /// <param name="path">Nowa ścieżka</param>
        public Note SetPath(String path)
        {
            this.path = path;
            return this;
        }
        /// <summary>
        /// Zwraca ścieżkę pliku xml notatki </summary>
        public String GetPath()
        {
            return this.path;
        }
        /// <summary>
        /// Ustawia numer zgłoszenia, do którego jest przypisana notatka </summary>
        /// <param name="no">Nowy numer zgłoszenia</param>
        public Note SetNo(int no)
        {
            this.no = no;
            this.status = 1;
            return this;
        }
        /// <summary>
        /// Zwraca numer zgłoszenia, do którego jest przypisana notatka </summary>
        public int GetNo()
        {
            return this.no;
        }
        /// <summary>
        /// Ustawia autora notatki </summary>
        /// <param name="author">Nowy autor</param>
        public Note SetAuthor(String author)
        {
            this.author = author;
            this.status = 1;
            return this;
        }
        /// <summary>
        /// Zwraca autora notatki </summary>
        public String GetAuthor()
        {
            return this.author;
        }
        /// <summary>
        /// Ustawia datę publikacji notatki </summary>
        /// <param name="published">Nowa data</param>
        public Note SetPublished(DateTime published)
        {
            this.published = published;
            this.status = 1;
            return this;
        }
        /// <summary>
        /// Zwraca datę publikacji notatki </summary>
        public DateTime GetPublished()
        {
            return this.published;
        }
        /// <summary>
        /// Ustawia treść notatki </summary>
        /// <param name="content">Nowa treść</param>
        public Note SetContent(String content)
        {
            this.content = content;
            return this;
        }
        /// <summary>
        /// Zwraca treść notatki </summary>
        public String GetContent()
        {
            return this.content;
        }

        private void CreateBackUp()
        {
            String path2 = this.path + ".bu";
            try
            {
                File.Delete(path2);
                File.Copy(this.path, path2);
            }
            catch
            {
                throw new Exception("Wystąpił błąd przy próbie stworzenia kopii zapasowej.");
            }
        }

        /// <summary>
        /// Zapisuje notatkę. Jeśli jej status jest równy 1, to edytuje istniejącą, jeśli 2 to tworzy nową </summary>
        public Note Save()
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (!File.Exists(this.path))
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(this.path, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Root");
                xmlWriter.Close();
            }

            switch (this.status)
            {
                case 0: //nowa notatka
                    this.CreateBackUp();
                    xmlDoc.Load(this.path);
                    XmlNode root = xmlDoc.DocumentElement;
                    XmlElement note = xmlDoc.CreateElement("Note");
                    XmlElement tmpEle;

                    tmpEle = xmlDoc.CreateElement("Id");
                    tmpEle.InnerText = this.id.ToString();
                    note.AppendChild(tmpEle.Clone());
                    tmpEle = xmlDoc.CreateElement("No");
                    tmpEle.InnerText = this.no.ToString();
                    note.AppendChild(tmpEle.Clone());
                    tmpEle = xmlDoc.CreateElement("Author");
                    tmpEle.InnerText = this.author;
                    note.AppendChild(tmpEle.Clone());
                    tmpEle = xmlDoc.CreateElement("Published");
                    tmpEle.InnerText = this.published.ToString();
                    note.AppendChild(tmpEle.Clone());
                    tmpEle = xmlDoc.CreateElement("Content");
                    tmpEle.InnerText = this.content;
                    note.AppendChild(tmpEle.Clone());

                    root.AppendChild(note);
                    xmlDoc.Save(this.path);
                    this.status = 2;
                    break;
                case 1: //notatka edytowana
                    this.CreateBackUp();
                    xmlDoc.Load(this.path);
                    XmlNode oldNode = xmlDoc.DocumentElement.SelectSingleNode("//Note[Id=" + this.id + "]");
                    if (oldNode != null)
                    {
                        oldNode.ChildNodes.Item(1).InnerText = this.no.ToString();
                        oldNode.ChildNodes.Item(2).InnerText = this.author;
                        oldNode.ChildNodes.Item(3).InnerText = this.published.ToString();
                        oldNode.ChildNodes.Item(4).InnerText = this.content;

                        xmlDoc.Save(this.path);
                    }
                    else
                    {
                        this.status = 0;
                        this.Save();
                    }

                    break;

                case 2: //notatka już zapisana
                    return this;
                default:
                    throw new Exception("Wystąpił nieznany błąd przy zapisie notatki.");
            }
            return this;
        }

        /// <summary>
        /// Pobiera notatki przypisane do danego zgłoszenia </summary>
        /// <param name="no">Numer zgłoszenia</param>
        public Note[] GetByNo(int no)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(this.path))
            {
                xmlDoc.Load(this.path);
            }
            else
            {
                throw new Exception("Nieprawidłowa ścieżka do pliku xml.");
            }
            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("//Note[No=" + no.ToString() + "]");
            Note[] result = null;
            if (nl != null)
            {
                result = new Note[nl.Count];
                int i = 0;
                foreach (XmlNode n in nl)
                {
                    Note tmp = new Note(this.path,
                        n.ChildNodes.Item(4).InnerText,
                        n.ChildNodes.Item(2).InnerText,
                        DateTime.Parse(n.ChildNodes.Item(3).InnerText),
                        int.Parse(n.ChildNodes.Item(1).InnerText),
                        int.Parse(n.ChildNodes.Item(0).InnerText)
                        );
                    result[i++] = tmp;
                }
            }
            return result;
        }
        /// <summary>
        /// Pobiera notatki stworzone przez danego autora </summary>
        /// <param name="author">Autor notatki</param>
        public Note[] GetByAuthor(String author)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(this.path))
            {
                xmlDoc.Load(this.path);
            }
            else
            {
                throw new Exception("Nieprawidłowa ścieżka do pliku xml.");
            }
            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("//Note[Author='" + author + "']");
            Note[] result = null;
            if (nl != null)
            {
                result = new Note[nl.Count];
                int i = 0;
                foreach (XmlNode n in nl)
                {
                    Note tmp = new Note(this.path,
                        n.ChildNodes.Item(4).InnerText,
                        n.ChildNodes.Item(2).InnerText,
                        DateTime.Parse(n.ChildNodes.Item(3).InnerText),
                        int.Parse(n.ChildNodes.Item(1).InnerText),
                        int.Parse(n.ChildNodes.Item(0).InnerText)
                        );
                    result[i++] = tmp;
                }
            }
            return result;
        }
        /// <summary>
        /// Pobiera notatki napisane później niż podana data </summary>
        /// <param name="published">Data od której są pobierane notatki</param>
        public Note[] GetSince(DateTime published)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(this.path))
            {
                xmlDoc.Load(this.path);
            }
            else
            {
                throw new Exception("Nieprawidłowa ścieżka do pliku xml.");
            }
            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("//Note");
            if (nl != null)
            {
                foreach (XmlNode n in nl)
                {
                    if (DateTime.Compare(DateTime.Parse(n.ChildNodes.Item(3).InnerText), published) < 0)
                    {
                        n.ParentNode.RemoveChild(n);
                    }
                }
            }
            nl = xmlDoc.DocumentElement.SelectNodes("//Note");
            Note[] result = null;
            if (nl != null)
            {
                result = new Note[nl.Count];
                int i = 0;
                foreach (XmlNode n in nl)
                {
                    Note tmp = new Note(this.path,
                        n.ChildNodes.Item(4).InnerText,
                        n.ChildNodes.Item(2).InnerText,
                        DateTime.Parse(n.ChildNodes.Item(3).InnerText),
                        int.Parse(n.ChildNodes.Item(1).InnerText),
                        int.Parse(n.ChildNodes.Item(0).InnerText)
                        );
                    result[i++] = tmp;
                }
            }
            return result;
        }
        /// <summary>
        /// Pobiera wszystkie notatki </summary>
        public Note[] GetAll()
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(this.path))
            {
                xmlDoc.Load(this.path);
            }
            else
            {
                throw new Exception("Nieprawidłowa ścieżka do pliku xml.");
            }
            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("//Note");
            Note[] result = null;
            if (nl != null)
            {
                result = new Note[nl.Count];
                int i = 0;
                foreach (XmlNode n in nl)
                {
                    Note tmp = new Note(this.path,
                        n.ChildNodes.Item(4).InnerText,
                        n.ChildNodes.Item(2).InnerText,
                        DateTime.Parse(n.ChildNodes.Item(3).InnerText),
                        int.Parse(n.ChildNodes.Item(1).InnerText),
                        int.Parse(n.ChildNodes.Item(0).InnerText)
                        );
                    result[i++] = tmp;
                }
            }
            return result;
        }

        /// <summary>
        /// Usuwa notatkę </summary>
        public void Delete()
        {
            this.CreateBackUp();
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(this.path))
            {
                xmlDoc.Load(this.path);
            }
            else
            {
                throw new Exception("Nieprawidłowa ścieżka do pliku xml.");
            }
            XmlNode dn = xmlDoc.DocumentElement.SelectSingleNode("//Note[Id=" + this.id + "]");
            xmlDoc.DocumentElement.RemoveChild(dn);
            xmlDoc.Save(this.path);
        }
        /// <summary>
        /// Cofa ostatnią akcję zapisu, edycji lub usunięcia.
        /// Ponowne użycia ponawia cofniętą akcję</summary>
        public void Undo()
        {
            String path2 = this.path + ".bu";
            File.Move(this.path, this.path + ".tmp");
            File.Move(path2, this.path);
            File.Move(this.path + ".tmp", path2);
        }

    }
}
