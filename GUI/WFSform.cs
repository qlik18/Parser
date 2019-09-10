using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections;
using Entities;
using Utility;
using LogicLayer.Interface;
using Atlassian.Jira;
using Atlassian.Jira.Remote;
using Entities.Enums;

namespace GUI
{
    public partial class WFSform : Form
    {

        public delegate void calbackDelegate(BillingIssueDtoHelios ppp, TreeView tr, bool isNew = false);
        public calbackDelegate callback;
        BillingIssueDtoHelios issue;
        public TreeView _tr;
        JiraLoggedUser jiraUser = new JiraLoggedUser();
        HeliosLoggedInUser helLIU = new HeliosLoggedInUser();
        public IParserEngineWFS gujaczWFS;
        Jira jira;
        int[] dodatkoweDane = new int[3] { -1, -1, -1 };
        private BackgroundWorker backgroundWorker = null;
        private MainForm parent { get; set; }
        IParserEngineWFS gujacz2;
        bool mass = false;//Sprawdza, czy zgłoszenia są dodawane masowo

        //public bool _auto;

        private bool _newIssue = false;

        public bool NewIssue 
        {
            get { return _newIssue; }
            set { _newIssue = value; }
        }

        //public bool _auto
        //{
        //    get { return _auto; }
        //    set { _auto = value; }
        //}

        public WFSform(MainForm form)
        {
            InitializeComponent();
            this.parent = form;

            //Login kk = new Login();
            //Login formLogin = new Login(gujaczWFS, ref helLIU, ref jiraUser);
            uzupelnijFormularz();
            //ServiceLocator locator = ServiceLocator.Instance;
            gujacz2 = Utility.ServiceLocator.Instance.Retrieve<IParserEngineWFS>();

            if (onCallZaznacz())
            {
                cb_CzyOncall.BackColor = Color.Tomato;
                cb_CzyOncall.Checked = true;
            }

            Dictionary<int, string> sys = gujacz2.getBillingComponents(-1);
            Dictionary<int, string> kat = gujacz2.getBillingComponents(0);
        }
        #region masowa obsługa zgłoszeń
        /// <summary>
        /// Konstuktor wykorzystywany przy masowym dodawaniu spraw do WFSa Ten właściwy
        /// </summary>
        /// <param name="zgloszenie"></param>
        public WFSform(BillingIssueDto zgloszenie, bool trans = false, bool newIssue = false, bool auto = false)//: base()
        {
            this._newIssue = newIssue;
            this.issue = (BillingIssueDtoHelios)zgloszenie;
            mass = true;
            
            InitializeComponent();

            //Login formLogin = new Login(gujaczWFS, ref helLIU, ref jiraUser);
            uzupelnijFormularz((BillingIssueDtoHelios)zgloszenie, trans);
            gujacz2 = Utility.ServiceLocator.Instance.Retrieve<IParserEngineWFS>();

            if (onCallZaznacz() && (zgloszenie.issueWFS.Priorytet == "Blokujący" || zgloszenie.issueWFS.Priorytet == "Krytyczny"))
            {
                cb_CzyOncall.BackColor = Color.Tomato;
                cb_CzyOncall.Checked = true;
            }

            //if (String.IsNullOrWhiteSpace(((BillingIssueDtoHelios)zgloszenie).issueHelios.assigned_to))
            //    MessageBox.Show("Pamiętaj o przydzieleniu zgłoszenia w JIRA!");

            // PIERWOTNIE: "CRM", "CP", "INFOLINIA", "NDTH", "SR", "SOS", "ZCC"
            // "NDTH", "SR", "CP", "CRM", "INFOLINIA", "ZCC", "SOS"
            //stere: string[] kate = new string[16] { "NDTH", "SR", "CP", "CRM", "INFOLINIA", "WIND", "ZCC", "SOS", "SBL", "PLK", "BILL", "KDTH", "NDTHW", "KKK", "ZU", "PRIO" };
            //string[] kate = new string[8] { "CRM", "INFOLINIA", "WIND", "ZCC", "BILL", "KKK", "PRIO", "ZU" };


            Dictionary<int, string> kat = gujacz2.getBillingComponents(0);

            Dictionary<int, string> sys = new Dictionary<int, string>();
            sys = gujacz2.getBillingComponents(kat.Keys.ToArray()[0]); //Dictionary<int, string>

            Dictionary<int, string> rodz = new Dictionary<int, string>();
            rodz = gujacz2.getBillingComponents(sys.Keys.ToArray()[0]); //Dictionary<int, string>

            Dictionary<int, string> typy = new Dictionary<int, string>();
            typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);

            foreach (KeyValuePair<int, string> s in sys)
            {
                tbSystem.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in kat)
            {
                tbKategoria.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });

            }
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }


            if (!_newIssue)
            {
                
                /*
                    Mapowanie
                    Kategoryzacji
                */
                List<List<string>> MapObszar = gujacz2.ExecuteStoredProcedure("spMapujWartosc", new string[] { issue.issueHelios.projekt, "Obsługa kodu projektu jako nazwy" }, DatabaseName.SupportCP);
                

                for (int index = 0; index < tbKategoria.Items.Count; index++)
                {
                    if (tbKategoria.Items[index].ToString() == issue.issueHelios.projekt)
                    {
                        tbKategoria.SelectedIndex = index;
                        break;
                    }
                     
                }

                if (tbKategoria.SelectedIndex == -1 && MapObszar.Count > 0)
                {
                    for (int index = 0; index < tbKategoria.Items.Count; index++)
                    {

                        if (tbKategoria.Items[index].ToString() == MapObszar[0][0])
                        {
                            tbKategoria.SelectedIndex = index;
                            break;
                        }

                    }
                   
                }

                //if (tbKategoria.SelectedIndex == -1)
                //{
                //    for (int index = 0; index < tbSystem.Items.Count; index++)
                //    {
                //        if (tbKategoria.Items[index].ToString() == issue.issueHelios.rodzaj_zgloszenia)
                //        {
                //            tbKategoria.SelectedIndex = index;
                //            break;
                //        }

                //    }
                //}

                if (tbKategoria.SelectedIndex == -1)
                    tbKategoria.SelectedIndex = 0;

                string rodzaj, typ;
                rodzaj = typ = string.Empty;


                /**/

                for (int index = 0; index < tbKategoria.Items.Count; index++)
                {
                    if (tbSystem.Items[index].ToString() == issue.issueHelios.rodzaj_zgloszenia)
                    {
                        tbSystem.SelectedIndex = index;
                        break;
                    }

                }


                if (tbKategoria.SelectedIndex == -1)
                    tbKategoria.SelectedIndex = 0;
                /**/
                if (issue.issueHelios.rodzaj_bledu != null)
                {
                    rodzaj = issue.issueHelios.rodzaj_bledu.TrimEnd();

                    for (int i = 0; i < tbRodzaj.Items.Count; i++)
                    {
                        try
                        {
                            //if (issue.issueHelios.projekt == "ZCC")
                            //{
                            //    string[] rodzajTyp = issue.issueHelios.rodzaj_bledu.Split('-');
                            //    rodzaj = rodzajTyp[0].TrimEnd();
                            //    if (rodzajTyp.Length > 1)
                            //        typ = rodzajTyp[1].TrimStart();
                            //}
                            //else


                            if (((BillingDthLBItem)tbRodzaj.Items[i]).Text.Equals(rodzaj, StringComparison.CurrentCultureIgnoreCase))
                            {
                                tbRodzaj.SelectedIndex = i;
                                break;
                            }

                        }
                        catch (Exception e) { }
                    }
                }
                 
                if (tbRodzaj.SelectedIndex == 0 && issue.issueHelios.rodzaj_bledu == null)
                {
                    for (int i = 0; i < tbRodzaj.Items.Count; i++)
                    {
                        if (tbRodzaj.Items[i].ToString().Equals("inne", StringComparison.CurrentCultureIgnoreCase))
                            tbRodzaj.SelectedIndex = i;

                    }
                    if(tbRodzaj.SelectedIndex == 0)
                    {
                        for (int i = 0; i < tbRodzaj.Items.Count; i++)
                        {
                            if (tbRodzaj.Items[i].ToString().Equals("brak", StringComparison.CurrentCultureIgnoreCase))
                                tbRodzaj.SelectedIndex = i;

                        }
                    }
                }

                typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);
                if (typ != string.Empty)
                {
                    for (int i = 0; i < tbTyp.Items.Count; i++)
                    {
                        try
                        {
                            if (((BillingDthLBItem)tbTyp.Items[i]).Text.Equals(typ, StringComparison.CurrentCultureIgnoreCase))
                            {
                                tbTyp.SelectedIndex = i;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }

            List<List<string>> priorytet = gujacz2.ExecuteStoredProcedure("Billing_GetListOfPriorities", new string[] { }, DatabaseName.SupportADDONS);

            for (int i = 0; i < priorytet.Count; i++)
            {
                tbPriorytet.Items.Add(new BillingDthLBItem() { Text = priorytet[i][1], Value = Convert.ToInt32(priorytet[i][0]) });
            }

            if (!_newIssue)
            {
                for (int i = 0; i < priorytet.Count; i++)
                {
                    string s = priorytet[i][1];
                    if (priorytet[i][0].Equals(issue.issueHelios.severity, StringComparison.InvariantCultureIgnoreCase)
                        || priorytet[i][1].Equals(issue.issueHelios.severity, StringComparison.InvariantCultureIgnoreCase))
                    {
                        tbPriorytet.SelectedIndex = i;
                        break;
                    }

                }
            }

            if (newIssue)
            {
                User u = gujacz2.getUser();
                tbFirstName.Text = u.name;
                tbLastName.Text = u.surname;
                tbEmail.Text = u.login + "@billennium.pl";
                tbDataOstKoment.Text = DateTime.Now.ToString();
                tbDataUtworzeniaZgl.Text = tbDataWystBledu.Text = DateTime.Now.ToString();
                tbIdKontraktu.Text = "-1";
                tbIdZamowienia.Text = "-1";
                tbNo.Enabled = false;
                for (int i = 0; i < tbSystem.Items.Count; i++)
                {
                    if (tbSystem.Items[i].ToString().Equals("wewnętrzne", StringComparison.CurrentCultureIgnoreCase))
                        tbSystem.SelectedIndex = i;
                }
                for (int i = 0; i < tbKategoria.Items.Count; i++)
                {
                    if (tbKategoria.Items[i].ToString().Equals("zgłoszenie wewnętrzne", StringComparison.CurrentCultureIgnoreCase))
                        tbKategoria.SelectedIndex = i;
                }
                for (int i = 0; i < tbPriorytet.Items.Count; i++)
                {
                    if (tbPriorytet.Items[i].ToString().Equals("sredni", StringComparison.CurrentCultureIgnoreCase))
                        tbPriorytet.SelectedIndex = i;
                }
            }
            if (auto && !cb_CzyOncall.Checked)
            {
                
                Logging.Logger.Instance.LogInformation(string.Format("Zgłoszenie {0} dodano automatycznie o godzinie {1}!\n", zgloszenie.JiraKey, DateTime.Now));
                addButton_Click(this, null);
            }

        }

        /// <summary>
        /// Konstuktor wykorzystywany przy masowym dodawaniu spraw do WFSa
        /// </summary>
        /// <param name="zgloszenie"></param>
        public WFSform(BillingIssueDto zgloszenie, int idSystemu, int idKategorii, int idRodzaju, int idTypu, string idKontraktu)//: base()
        {
            this.issue = (BillingIssueDtoHelios)zgloszenie;
            mass = true;
            InitializeComponent();


            //Login formLogin = new Login(gujaczWFS, ref helLIU, ref jiraUser);
            bool trans = false;
            if (issue.issueHelios.number != issue.issueHelios.oryginalneId)
                trans = true;

            uzupelnijFormularz((BillingIssueDtoHelios)zgloszenie, trans);
            gujacz2 = Utility.ServiceLocator.Instance.Retrieve<IParserEngineWFS>();

            if (onCallZaznacz() && (zgloszenie.issueWFS.Priorytet == "Blokujący" || zgloszenie.issueWFS.Priorytet == "Krytyczny") )
            {
                cb_CzyOncall.BackColor = Color.Tomato;
                cb_CzyOncall.Checked = true;
            }

            Dictionary<int, string> sys = gujacz2.getBillingComponents(-1);
            Dictionary<int, string> kat = gujacz2.getBillingComponents(0);
            Dictionary<int, string> rodz = gujacz2.getBillingComponents(kat.Keys.ToArray()[0]);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);

            foreach (KeyValuePair<int, string> s in sys)
            {
                tbSystem.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in kat)
            {
                tbKategoria.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            for (int i = 0; i < tbSystem.Items.Count; i++)
            {
                if (((BillingDthLBItem)tbSystem.Items[i]).Value == idSystemu)
                {
                    tbSystem.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < tbKategoria.Items.Count; i++)
            {
                if (((BillingDthLBItem)tbKategoria.Items[i]).Value == idKategorii)
                {
                    tbKategoria.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < tbRodzaj.Items.Count; i++)
            {
                if (((BillingDthLBItem)tbRodzaj.Items[i]).Value == idRodzaju)
                {
                    tbRodzaj.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < tbTyp.Items.Count; i++)
            {
                if (((BillingDthLBItem)tbTyp.Items[i]).Value == idTypu)
                {
                    tbTyp.SelectedIndex = i;
                    break;
                }
            }
        }

        public void KliknijMnie()
        {
            addButton_Click("autousupelnianie", null);
        }

        /// <summary>
        /// Uzupełnianie formularza przy dodawaniu zgłoszeń masowo
        /// </summary>
        /// <param name="zgloszenie"></param>
        public void uzupelnijFormularz(BillingIssueDtoHelios zgloszenie, bool trans)
        {

            if (zgloszenie != null && !_newIssue)
            {
                tbNo.Text = zgloszenie.issueHelios.number;
                tbTitle.Text = zgloszenie.issueHelios.title;
                tbFirstName.Text = zgloszenie.issueWFS.Imie;
                tbLastName.Text = zgloszenie.issueWFS.Nazwisko;
                tbEmail.Text = zgloszenie.issueWFS.Email == "" ? "---" : zgloszenie.issueWFS.Email;
                tbDataWystBledu.Text = zgloszenie.issueWFS.DataWystapieniaBledu;
                //tbDataUtworzeniaZgl.Text = zgloszenie.issueWFS.DataWystapieniaBledu;
                tbDataUtworzeniaZgl.Text = DateTime.Now.ToString();// zgloszenie.issueWFS.DataWystapieniaBledu;
                tbDataOstKoment.Text = zgloszenie.issueWFS.DataIGodzinaOstatniegoKomentarza;
                tbTrescZgloszenia.Text = zgloszenie.issueWFS.TrescZgloszenia;
                tbIdKontraktu.Text = zgloszenie.issueWFS.IdKontraktu;
                tbIdZamowienia.Text = zgloszenie.issueWFS.IdZamowienia;
                tbJiraId.Text = zgloszenie.issueHelios.jiraIdentifier;
                tbSrodowiskoProblemu.Text = zgloszenie.issueHelios.srodowiskoProblemu;
                cb_CzyOncall.Checked = (zgloszenie.issueHelios.czyOnCall == "True" ? true : false);
                if (trans)
                {
                    tbDataWystBledu.BackColor = Color.Red;
                }

                //severity ma byc intem który bedzie oznaczal powage zgloszenia
                //cbBugSeverity.SelectedIndex = Int32.Parse(zgloszenie.issueHelios.severity);   
                //tbSystem.SelectedIndex = 0;
                //tbKategoria.SelectedIndex = 0;
                //tbRodzaj.SelectedIndex = 0;
            }
        }
        #endregion

        public void uzupelnijFormularz()
        {
            if (parent.zgloszenie != null)
            {
                tbNo.Text = parent.zgloszenie.number;
                tbTitle.Text = parent.zgloszenie.title;
                tbFirstName.Text = parent.zgloszenie.firstName;
                tbLastName.Text = parent.zgloszenie.lastName;
                tbEmail.Text = parent.zgloszenie.email;
                tbDataWystBledu.Text = parent.zgloszenie.date;
                tbTrescZgloszenia.Text = parent.zgloszenie.content;

                //severity ma byc intem który bedzie oznaczal powage zgloszenia
                //cbBugSeverity.SelectedIndex = Int32.Parse(parent.zgloszenie.severity);          
                //tbSystem.SelectedIndex = 0;
                //tbKategoria.SelectedIndex = 0;
                //tbRodzaj.SelectedIndex = 0;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Validate_DateFormat(string text)
        {
            bool result = false;

            DateTime dt;

            try
            {
                DateTime.TryParse(text, out dt);
            }
            catch (Exception ex)
            {
                NoticeForm.ShowNotice(ex.Message);
                return false;
            }

            if (dt.ToString() != "0001-01-01 00:00:00")
            {
                result = true;
            }

            return result;
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            if (tbFirstName.Text != string.Empty && tbLastName.Text != string.Empty && tbSystem.SelectedItem != null && tbKategoria.SelectedItem != null && tbRodzaj.SelectedItem != null && tbTyp.SelectedItem != null && tbJiraId.Text != string.Empty)
            {
                if (!Validate_DateFormat(tbDataWystBledu.Text))
                {
                    NoticeForm.ShowNotice("Pole: Data wystąpienia błędu, zostało błędnie uzupełnione. Należy poprawić dane.");
                    return;
                }

                cancelButton.Enabled = false;
                addButton.Enabled = false;
                issue.issueWFS = new BillingDTHIssueWFS()
                    {
                        NumerZgloszenia = tbNo.Text,
                        TytulZgloszenia = tbTitle.Text,
                        Imie = tbFirstName.Text,
                        Nazwisko = tbLastName.Text,
                        Email = tbEmail.Text,
                        DataWystapieniaBledu = tbDataWystBledu.Text,
                        DataIGodzinaUtworzeniaZgloszenia = tbDataUtworzeniaZgl.Text,
                        DataIGodzinaOstatniegoKomentarza = tbDataOstKoment.Text,
                        IdKontraktu = tbIdKontraktu.Text,
                        IdZamowienia = tbIdZamowienia.Text,
                        System = new Entities.Component() { Text = ((BillingDthLBItem)tbSystem.SelectedItem).Text, Value = ((BillingDthLBItem)tbSystem.SelectedItem).Value },
                        Kategoria = new Entities.Component() { Text = ((BillingDthLBItem)tbKategoria.SelectedItem).Text, Value = ((BillingDthLBItem)tbKategoria.SelectedItem).Value },
                        Rodzaj = new Entities.Component() { Text = ((BillingDthLBItem)tbRodzaj.SelectedItem).Text, Value = ((BillingDthLBItem)tbRodzaj.SelectedItem).Value },
                        Typ = new Entities.Component() { Text = ((BillingDthLBItem)tbTyp.SelectedItem).Text, Value = ((BillingDthLBItem)tbTyp.SelectedItem).Value },
                        TrescZgloszenia = tbTrescZgloszenia.Text.PadLeft(10000),
                        Priorytet = ((BillingDthLBItem)tbPriorytet.SelectedItem).Value.ToString(),
                        JiraId = tbJiraId.Text,
                        CzyOnCall = (cb_CzyOncall.Checked == true ? "True" : "False"),
                        SrodowiskoProblemu = (tbSrodowiskoProblemu.Text != "" ? tbSrodowiskoProblemu.Text : "---")
                    };

                

                List<List<string>> zgloszenie = gujacz2.ExecuteStoredProcedure("[spCheckJiraId]", new string[] { issue.issueWFS.JiraId.ToString() }, DatabaseName.SupportCP);
                /* stara obsługa
                if(issue.issueWFS.System.Text != "Problem")
                    zgloszenie = gujacz2.ExecuteStoredProcedure("[spCheckJiraId]", new string[] { issue.issueWFS.JiraId.ToString() }, DatabaseName.SupportCP);
                else 
                    zgloszenie = gujacz2.ExecuteStoredProcedure("[spCheckJiraId]", new string[] { issue.issueWFS.NumerZgloszenia.ToString() }, DatabaseName.SupportCP);
                */

                if (zgloszenie.Count > 0)
                {
                    NoticeForm nf = new NoticeForm(("Numer zgłoszenia " + issue.issueWFS.NumerZgloszenia + " istnieje już zapisany pod nr sprawy: " + zgloszenie[0][1].ToString()), NoticeButtons.OK, new string[] { "OK" });

                    nf.ShowDialog();

                    this.Close();


                }

                else
                {

                    if (_newIssue)
                    {
                        issue.issueHelios = new IssueHelios()
                        {
                            number = tbNo.Text,
                            title = tbTitle.Text,
                            firstName = tbFirstName.Text,
                            lastName = tbLastName.Text,
                            email = tbEmail.Text,
                            date = tbDataOstKoment.Text,
                            updated = tbDataWystBledu.Text,
                            projekt = ((BillingDthLBItem)tbKategoria.SelectedItem).Text,
                            rodzaj_zgloszenia = ((BillingDthLBItem)tbSystem.SelectedItem).Text,
                            rodzaj_bledu = ((BillingDthLBItem)tbRodzaj.SelectedItem).Text,
                            severity = ((BillingDthLBItem)tbPriorytet.SelectedItem).Value.ToString(),
                            czyOnCall = (cb_CzyOncall.Checked == true ? "True" : "False"),
                            srodowiskoProblemu = (tbSrodowiskoProblemu.Text != "" ? tbSrodowiskoProblemu.Text : "---")
                        };
                    }
                    if (mass && callback != null)//Jeśli dodawana masowo, to wyślij obiekt przez callback
                    {
                        callback(issue, _tr, _newIssue);
                        this.Close();
                    }
                    else
                    { // jeśli nie dodawane masowo tylko pojedyńczo to zapisz obiekt
                        doByWorker(new DoWorkEventHandler(doWorkSaveToWFS), issue, new RunWorkerCompletedEventHandler(doSavingCompleted));
                    }
                }
                    
            }
            else
            {
                if (sender.ToString() != "autousupelnianie")
                {
                    MessageBox.Show("Przed dodaniem zgłoszenia do WFS należy uzupełnić wszystkie wymagane pola.", "Brak danych", MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);
                }
            }

        }

        private void doByWorker(DoWorkEventHandler work, object argument, RunWorkerCompletedEventHandler reaction)
        {
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += work;
            this.backgroundWorker.RunWorkerCompleted += reaction;
            this.backgroundWorker.RunWorkerAsync(argument);
        }



        private void doWorkSaveToWFS(object sender, DoWorkEventArgs e)
        {
            //gujacz2
            int tmp = 0;
            gujacz2.addBillingIssueToWFS(e.Argument as BillingIssueDtoHelios, out tmp);
        }

        private void doSavingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Zgłoszenie zostało poprawnie zapisane pod numerem: " + Convert.ToString(e.Result));
            this.Dispose();
        }

        /*stara wersja
        private void tbKategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox kat = (ComboBox)sender;

            Dictionary<int, string> rodz = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)kat.SelectedItem).Value);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);
            dodatkoweDane[0] = ((Entities.BillingDthLBItem)kat.SelectedItem).Value;

            tbRodzaj.Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            tbTyp.Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (tbRodzaj.Items.Count > 0)
            {
                tbRodzaj.SelectedIndex = 0;
                if (tbSystem.SelectedIndex == 0)
                {
                    for (int i = 0; i < tbRodzaj.Items.Count; i++)
                    {
                        if (tbRodzaj.Items[i].ToString().Equals("inne", StringComparison.CurrentCultureIgnoreCase))
                            tbRodzaj.SelectedIndex = i;
                    }
                }
            }
            if (tbTyp.Items.Count > 0)
            {
                tbTyp.SelectedIndex = 0;
            }


            if (_newIssue)
            {
                List<List<string>> klucz = gujacz2.ExecuteStoredProcedure("CP_GenerateKey", new string[] { ((Entities.BillingDthLBItem)kat.SelectedItem).Value.ToString() }, DatabaseName.SupportCP);
                tbNo.Text = klucz[0][0];
            }
        }
        */
        private void tbKategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox kat = (ComboBox)sender;

            Dictionary<int, string> sys = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)kat.SelectedItem).Value);

            Dictionary<int, string> rodz = gujacz2.getBillingComponents(sys.Keys.ToArray()[0]);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);
            dodatkoweDane[0] = ((Entities.BillingDthLBItem)kat.SelectedItem).Value;

            tbSystem.Items.Clear();
            foreach (KeyValuePair<int, string> s in sys)
            {
                tbSystem.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            tbRodzaj.Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            tbTyp.Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (tbSystem.Items.Count > 0)
            {
                tbSystem.SelectedIndex = 0;
                //if (tbSystem.SelectedIndex == 0)
                //{
                //    for (int i = 0; i < tbRodzaj.Items.Count; i++)
                //    {
                //        if (tbRodzaj.Items[i].ToString().Equals("inne", StringComparison.CurrentCultureIgnoreCase))
                //            tbRodzaj.SelectedIndex = i;
                //    }
                //}
            }
            if (tbRodzaj.Items.Count > 0)
            {
                tbRodzaj.SelectedIndex = 0;
            }
            if (tbTyp.Items.Count > 0)
            {
                tbTyp.SelectedIndex = 0;
            }


            if (_newIssue)
            {
                List<List<string>> klucz = gujacz2.ExecuteStoredProcedure("CP_GenerateKey", new string[] { ((Entities.BillingDthLBItem)kat.SelectedItem).Value.ToString() }, DatabaseName.SupportCP);
                tbNo.Text = klucz[0][0];
            }
        }

        private void tbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox sys = (ComboBox)sender;
            
            Dictionary<int, string> rodz = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)sys.SelectedItem).Value);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);
            dodatkoweDane[0] = ((Entities.BillingDthLBItem)sys.SelectedItem).Value;

            tbRodzaj.Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            tbTyp.Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (tbRodzaj.Items.Count > 0)
            {
                tbRodzaj.SelectedIndex = 0;
                if (tbSystem.SelectedIndex == 0)
                {
                    for (int i = 0; i < tbRodzaj.Items.Count; i++)
                    {
                        if (tbRodzaj.Items[i].ToString().Equals("inne", StringComparison.CurrentCultureIgnoreCase))
                            tbRodzaj.SelectedIndex = i;
                    }
                }
            }
            if (tbTyp.Items.Count > 0)
            {
                tbTyp.SelectedIndex = 0;
            }
        }

        private void tbRodzaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox rodz = (ComboBox)sender;

            Dictionary<int, string> typy = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)rodz.SelectedItem).Value);
            dodatkoweDane[1] = ((Entities.BillingDthLBItem)rodz.SelectedItem).Value;
            tbTyp.Items.Clear();
            int blankIndex = -1;

            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                //if (s.Value.Equals("---"))
                //    blankIndex = tbTyp.Items.Count - 1;
            }

            for (int i=0 ;i<tbTyp.Items.Count; i++)
            {
                if (tbTyp.Items[i].ToString().Equals("---"))
                    blankIndex = i;
                else if (tbTyp.Items[i].ToString().Equals("Inne"))
                    blankIndex = i;
            }

            if (tbTyp.Items.Count > 0)
            {
                if (blankIndex == -1)
                    tbTyp.SelectedIndex = 0;
                else
                tbTyp.SelectedIndex = blankIndex;
            }
            dodatkoweInfo();
        }


        private void tbTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox typ = (ComboBox)sender;
            dodatkoweDane[2] = ((Entities.BillingDthLBItem)typ.SelectedItem).Value;
            dodatkoweInfo();
        }

        private void dodatkoweInfo()
        {
            infor_dodatkoweRTB.Text = "";

            List<List<string>> dan = gujacz2.ExecuteStoredProcedure("billing_dth_wybraniedodatkowychinformacji", new string[] { dodatkoweDane[0].ToString(), dodatkoweDane[1].ToString(), dodatkoweDane[2].ToString() }, DatabaseName.SupportADDONS);
            foreach (var a in dan)
            {
                infor_dodatkoweRTB.AppendText(a[0].ToString());
            }

        }
        private void AdjustWidthComboBox_DropDown(object sender, System.EventArgs e)
        {
            if (((ComboBox)sender).Items.Count == 0) return;

            ComboBox senderComboBox = (ComboBox)sender;
            int width = 0;// senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            foreach (BillingDthLBItem s in ((ComboBox)sender).Items)
            {
                newWidth = (int)g.MeasureString(s.Text, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        private void WFSform_Load(object sender, EventArgs e)
        {

        }

        private bool onCallZaznacz()
        {
            DateTime aktData = DateTime.Now;
            if ( aktData.DayOfWeek == DayOfWeek.Saturday || aktData.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else if (aktData.Hour >= 18 || aktData.Hour < 8)
                return true;
            else 
            {
                List<List<string>> holidays;
                    holidays= gujacz2.ExecuteStoredProcedure("GetHolidaysSP", new string[] { aktData.Year.ToString()}, DatabaseName.SupportCP);
                foreach (var item in holidays)
                {
                    if (Convert.ToDateTime(item[0]).Date.ToShortDateString() == aktData.Date.ToShortDateString())
                        return true;
                }
            }

            return false;
        }


    }
}