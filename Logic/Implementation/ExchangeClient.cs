using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using Entities;
using System.IO;

namespace Logic.Implementation
{
    public class ExchangeClient
    {
        //private const string smtpAddress = "remote.billennium.pl";//"VKORNIK.billennium.local"; //
        //private const int portNumber = 25;
        //private const bool enableSSL = false;

        private const string smtpAddress = "outlook.office365.com";//"VKORNIK.billennium.local"; //
        private const int portNumber = 587;
        private const bool enableSSL = true;

        private User user;

        public ExchangeClient(User user)
        {
            this.user = user;
        }

        public void SendEMail(string subject, string body, string address = "")
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    string b = LoadTemplate("slaNotification");
                    b = b.Replace("{{DATE}}", DateTime.Now.ToString());
                    b = b.Replace("{{CONTENT}}", "<center>" + body + "</center>");

                    mail.From = new MailAddress(user.login + "@billennium.pl");

                    List<string> recipants = new List<string>();

                    if (address != "")
                        recipants.AddRange(address.Split(';').ToList());
                    else
                        recipants.Add(user.login + "@billennium.pl");

                    foreach (var item in recipants)
                    {
                        mail.To.Add(item);
                    }
                    mail.Subject = subject;
                    mail.Body = b;
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.


                    SmtpClient client = new SmtpClient()
                    {
                        Host = smtpAddress,
                        Port = portNumber,
                        EnableSsl = enableSSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        //Credentials = new NetworkCredential(user.login, user.password, "BILLENNIUM")
                        Credentials = new NetworkCredential("Pawel.Rekawek@billennium.pl", user.password)//, "BILLENNIUM")
                    };

                    client.Send(mail);

                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,"Błąd wysyłania maila");
                //throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public void SendEMail(string subject, string address, System.Windows.Forms.RichTextBox body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    //string b = LoadTemplate("slaNotification");
                    //b = b.Replace("{{DATE}}", DateTime.Now.ToString());
                    //b = b.Replace("{{CONTENT}}", "<center>" + body + "</center>");

                    mail.From = new MailAddress(user.login + "@billennium.pl");

                    List<string> recipants = new List<string>();

                    if (address != "")
                        recipants.AddRange(address.Split(';').ToList());
                    else
                        recipants.Add(user.login + "@billennium.pl");

                    foreach (var item in recipants)
                    {
                        mail.To.Add(item);
                    }
                    mail.Subject = subject;
                    mail.Body = body.Text;
                    mail.IsBodyHtml = false;
                    // Can set to false, if you are sending pure text.


                    SmtpClient client = new SmtpClient()
                    {
                        Host = smtpAddress,
                        Port = portNumber,
                        EnableSsl = enableSSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        //Credentials = new NetworkCredential(user.login, user.password, "BILLENNIUM")
                        Credentials = new NetworkCredential("Pawel.Rekawek@billennium.pl", user.password)//, "BILLENNIUM")
                    };

                    client.Send(mail);

                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Błąd wysyłania maila");
                //throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public void SendEMail(string subject, HtmlContent content, string address = "")
        {
            this.SendEMail(subject, content.ToString(), address);
        }

        public string LoadTemplate(string template)
        {
            string appDirectoryPath = Path.GetDirectoryName(GetType().Assembly.CodeBase);
            appDirectoryPath = Path.Combine(appDirectoryPath, "EmailTemplates");
            appDirectoryPath = Path.Combine(appDirectoryPath, template + ".html");
            appDirectoryPath = appDirectoryPath.Replace("file:\\", "");
            return File.ReadAllText(appDirectoryPath);
        }
    }

    public class HtmlColumn
    {
        public string Header { get; set; }

        public string ToHtml()
        {
            return string.Format("<th>{0}</th>", Header);
        }
    }

    public class HtmlRow
    {
        public List<string> Cells { get; set; }

        public HtmlRow()
        {
            this.Cells = new List<string>();
        }

        public HtmlRow(string[] cells)
            : this()
        {
            foreach (var item in cells)
            {
                Cells.Add(item);
            }
        }

        public string ToHtml()
        {
            string row = string.Empty;

            row += "<tr>";
            foreach (string cell in Cells)
            {
                row += string.Format("<td>{0}</td>", cell);
            }
            row += "</tr>";

            return row;
        }
    }

    public class HtmlTable
    {
        public List<HtmlColumn> Columns { get; set; }
        public List<HtmlRow> Rows { get; set; }

        public HtmlTable()
        {
            this.Columns = new List<HtmlColumn>();
            this.Rows = new List<HtmlRow>();
        }

        public void AddColumn(HtmlColumn column)
        {
            Columns.Add(column);
        }

        public void AddColumn(string columnHeader)
        {
            Columns.Add(new HtmlColumn() { Header = columnHeader });
        }

        public void AddRow(HtmlRow row)
        {
            Rows.Add(row);
        }

        public void AddRow(string[] cells)
        {
            Rows.Add(new HtmlRow(cells));
        }


        public string ToHtml()
        {
            string result = string.Empty;

            result += "<table width=\"80%\">";

            result += "<tr>";
            foreach (var item in Columns)
            {
                result += item.ToHtml();
            }
            result += "</tr>";

            foreach (var item in Rows)
            {
                result += item.ToHtml();
            }

            result += "</table>";

            return result;
        }

        public override string ToString()
        {
            return ToHtml();
        }
    }

    public class HtmlHeader
    {
        public string HeaderText { get; set; }

        public HtmlHeader()
        {

        }

        public HtmlHeader(string headerText)
            : this()
        {
            this.HeaderText = headerText;
        }

        public string ToHtml()
        {
            return string.Format("<h3><b>{0}</b></h3>", HeaderText);
        }

        public override string ToString()
        {
            return ToHtml();
        }
    }

    public class HtmlHyperlink
    {
        public string Link { get; set; }
        public string Display { get; set; }

        public HtmlHyperlink() { }

        public HtmlHyperlink(string link, string display)
        {
            this.Link = link;
            this.Display = display;
        }

        public HtmlHyperlink(string link)
            : this(link, string.Empty) { }

        public string ToHtml()
        {
            string result = string.Empty;

            if (Display != string.Empty)
            {
                result = string.Format("<a href=\"{0}\">{1}</a>", Link, Display);
            }
            else
            {
                result = string.Format("<a href=\"{0}\">{1}</a>", Link, Link);
            }

            return result;
        }

        public override string ToString()
        {
            return ToHtml();
        }
    }

    public class HtmlContent
    {
        public HtmlHeader Header { get; set; }
        public HtmlTable Table { get; set; }

        public HtmlContent() { }

        public HtmlContent(HtmlHeader header, HtmlTable table)
            : this()
        {
            this.Header = header;
            this.Table = table;
        }

        public HtmlContent(HtmlHeader header)
            : this(header, null) { }

        public HtmlContent(HtmlTable table)
            : this(null, table) { }

        public string ToHtml()
        {
            string html = string.Empty;

            html += "<center>";

            html += Header != null ? Header.ToHtml() + "<br><br>" : "";

            html += Table != null ? Table.ToHtml() + "<br>" : "";

            html += "</center>";

            return html;
        }

        public override string ToString()
        {
            return ToHtml();
        }
    }
}
