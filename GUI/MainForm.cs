using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using LogicLayer.Interface;
using Entities;
using Entities.Enums;
using Utility;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using Logging;
using GUI.Implementation;
using Atlassian.Jira;
using Atlassian.Jira.Remote;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Media;
using Logic.Implementation;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data.SqlClient;
using LogicLayer;

using Net.Sgoliver.NRtfTree.Core;
using Net.Sgoliver.NRtfTree.Util;

namespace GUI
{

    public partial class MainForm : Form
    {
        #region ZMIENNE PUBLICZNE
        //public ExceptionManager exManager;
        public IssueHelios zgloszenie = null;


        public IParserEngineWFS gujaczWFS;
        public INotes gujaczNotatki;
        public IUsers usersHelios;
        public Dictionary<int, HeliosUser> cpUsers;
        public System.Diagnostics.Process linkNavigationProcess = new System.Diagnostics.Process();
        public Dictionary<string, Dictionary<BillingIssueDto, IssueState>> issues = new Dictionary<string, Dictionary<BillingIssueDto, IssueState>>(); // string,Dictionary<BillingIssueDto, IssueState>    ten string to nazwa treeView, w którym znajdują się zgłoszenia, kolejne Value sie nie zmienilo

        public List<Entities.JiraUser> JiraUsers;

        #endregion

        #region ZMIENNE PRYWATNE
        // Akcje na zgłoszeniau
        private Dictionary<string, Dictionary<string, Dictionary<int, string>>> issueMove = new Dictionary<string, Dictionary<string, Dictionary<int, string>>>();
        // Formatka notatki
        private EditNote note_form;
        // Czy załadowane komponenty
        private bool isReadedComponents = false;
        // Background worker
        private BackgroundWorker backgroundWorker = null;
        // Czy sprawdzać zgłoszenia w powiadomieniu
        private bool checkIssues = false;
        // Czy trwa pauza w powiadomieniu
        private bool pauza = true;
        // Ilość zgłoszeń nieprzydzielonych
        private string nieprzydzielone = "0";
        // Ilość zgłoszeń przydzielonych
        private string przydzielone = "0";
        // Zalogowany użytkonik (parsera)
        private HeliosLoggedInUser helLIU = new HeliosLoggedInUser();
        // Użytkownik Jira
        private JiraLoggedUser jiraUser = new JiraLoggedUser();
        // Jira
        private Jira jira;
        // JiraIssues
        private JiraIssues jIssue;
        // Czas do kiedy będzie wyświetlana chmurka
        private DateTime alertShowUntil;
        // Projekt zespołu
        //private Logic.Implementation.JiraProject usersProject;
        // Czy zgłoszenia przydzielone: jeśli wybrana zakładka Moje - true; w przeciwnym wypadku - false;
        private bool assignedIssues = false;
        // Instancja formatki WFSModelerForm
        private WFSModelerForm wmf;
        // Lista zgłoszeń z WFS
        private Dictionary<string, List<BillingIssueDtoHelios>> wfsList = new Dictionary<string, List<BillingIssueDtoHelios>>();
        // Lista zgłoszeń w Multi
        private Dictionary<BillingIssueDto, IssueState> multiIssues = new Dictionary<BillingIssueDto, IssueState>();
        // Wybrane zgłoszenie z drzewka
        private BillingIssueDto selectIssue = null;        
        // Wybrane zgłoszenie z drzewka
        private Dictionary<string,BillingIssueDto> selectIssueList = new Dictionary<string, BillingIssueDto>();
        // Czy wysłać maila gdy pojawią się nowe zgłoszenia
        private bool emailNotification = false;
        // Czy aktualnie trwa sprawdzanie ilości zgłoszeń
        private bool isIssueCheckoutRunning = false;
        // Czy trwa odświeżanie zgłoszeń
        private bool isIssuesRefreshRunning = false;
        // Workload
        private Logic.Implementation.Workload workload;
        // Index aktualnie aktywnej zakładki
        private int currentTreeTabIndex;
        // Lista Aktualnych czasów SLA
        private List<List<string>> SLAlista = new List<List<string>>();
        // Lista Aktualnych czasów SLA
        private List<UserBpmJira> userBpmJira = new List<UserBpmJira>();
        // Czy quickSptep
        private bool __quickStep = false;
        // Czy quickSptep
        private bool WorkingSla = false;
        // Czy Pierwsze logowanie
        private bool firstLogin = true;
        #endregion

        #region STAŁE
        private const string NIEPRZYDZIELONE_TREE = "treeView1";
        private const string MOJE_TREE = "treeView2";
        #endregion

        #region MIGANIE IKONKI
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        public const UInt32 FLASHW_ALL = 3;
        public const UInt32 FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        public static bool FlashWindowEx(Form form)
        {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }
        #endregion

        #region METODY FORMATKI
        public MainForm()
        {
            InitializeComponent();

            //tp_tmp = TabAppearance.FlatButtons;
            tp_tmp.Size = new System.Drawing.Size(0, 1);
            //tp_tmp.ImeMode = ImeMode. TabSizeMode.Fixed;
            tp_tmp.Text = "";
            InitializeTempDirectory();

            Logger.Instance = new Logger(ConfigurationManager.AppSettings["DirectoryLogs"].ToString(), null);
            //exManager = new ExceptionManager();
            //exManager.log = new ExceptionManager.Loguj(log);
            //Login formLogin = new Login();
            try
            {
                //gujaczHelios = ServiceLocator.Instance.Retrieve<IParserEngineHelios>();
                gujaczWFS = Utility.ServiceLocator.Instance.Retrieve<IParserEngineWFS>();
                gujaczNotatki = Utility.ServiceLocator.Instance.Retrieve<INotes>();
                usersHelios = Utility.ServiceLocator.Instance.Retrieve<IUsers>();
                JiraUsers = new List<Entities.JiraUser>();
                //userManager = ServiceLocator.Instance.Retrieve<IUsers>();

            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e);
                NoticeForm.ShowNotice("Wystąpił błąd w działaniu aplikacji. Szczegóły zostały zapisane do pliku logów. !", "Błąd");
                //MessageBox.Show("Wystąpił błąd w działaniu aplikacji. Szczegóły zostały zapisane do pliku logów. !", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            note_form = new EditNote();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            t_EmailNotification.Interval = int.Parse(ConfigurationManager.AppSettings["SlaTimeout"]);
            textBox4.Text = DateTime.Now.ToString("yyyy/MM/dd").Replace('-', '/');

            tab_Raporty.Controls.Add(tp_Sla);
            tab_Raporty.Controls.Add(tp_DayReport);
            tab_Raporty.Controls.Add(tp_crm);
            tab_Raporty.Controls.Add(tp_zcc);
            tab_Raporty.Controls.Add(tp_billing);


            tc_awarie.Controls.Add(tp_awaria_nowa);
            tc_awarie.Controls.Add(tp_awaria_lista);

            this.Text = Properties.Settings.Default.AppVersion;

            if (Properties.Settings.Default.hasloBillennium != null)
            {
                mtb_BillenniumPass.Text = Properties.Settings.Default.hasloBillennium;
                if (mtb_BillenniumPass.Text != ""
                    && tryLogginToJira("billennium", mtb_BillenniumPass.Text)
                    )
                {
                    cb_KontoBillennium.Checked = true;
                    cb_KontoBillennium.Text = "OK!";
                }
                else
                {
                    cb_KontoBillennium.Checked = false;
                    cb_KontoBillennium.Text = "Błędne hasło lub brak połączenia JIRA!";
                }
            }

            currentTreeTabIndex = 0;

            Login formLogin = new Login(gujaczWFS, ref helLIU, ref jiraUser, firstLogin);
            firstLogin = false;

            notifyTimeoutTextBox.Text = "300";// Properties.Settings.Default.IssuesCheckTimeout.ToString();
            tb_SoundNotificationPath.Text = Properties.Settings.Default.dzwiekSciezka;
            cbox_RefreshBoth.Checked = Properties.Settings.Default.czyObaDrzewa;
            this.tb_AssignedFilterName.Text = Properties.Settings.Default.assignedFilterName;
            this.tb_UnassignedFilterName.Text = Properties.Settings.Default.unassignedFilterName;

            ToolTip assignedFilterNameToolTip = new ToolTip();
            assignedFilterNameToolTip.SetToolTip(tb_AssignedFilterName, "Wprowadź nazwę filtru w Jira lub pozostaw puste by skorystać z filtru zdefiniowanego w aplikacji.");
            ToolTip unassignedFilterNameToolTip = new ToolTip();
            unassignedFilterNameToolTip.SetToolTip(tb_UnassignedFilterName, "Wprowadź nazwę filtru w Jira lub pozostaw puste by skorystać z filtru zdefiniowanego w aplikacji.");

            //tb_filter1name.Text = tp_filter1name.Text = lb_filter1name.Text = Properties.Settings.Default.filter1name;
            //tb_filter2name.Text = tp_filter2name.Text = lb_filter2name.Text = Properties.Settings.Default.filter2name;

            filterNameUpdate("filter1name");
            filterNameUpdate("filter2name");

            //tp_filter2name.Text = lb_filter2name.Text;

            if (Properties.Settings.Default.czyPowiadomienie)
            {
                btn_PowiadomienieWlacz_Click(this, EventArgs.Empty);
                ckb_isNotifyEnabled.Checked = true;
            }

            cb_SoundNotification.Checked = Properties.Settings.Default.czyDzwiek;

            t_EmailNotification.Enabled = true;

            /*Raport 18 wyznaczenie dnia*/
            dtp_CRMRapDataDo.Value = DateTime.Now;
            dtp_dayReportDateTo.Value = DateTime.Now;

            int temp = 0;
            switch (dtp_CRMRapDataDo.Value.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    temp = -2;
                    break;
                case DayOfWeek.Monday:
                    temp = -3;
                    break;
                case DayOfWeek.Saturday:
                    temp = -1;
                    break;
                default:
                    temp = -1;
                    break;
            }

            dtp_CRMRapDataOd.Value = dtp_CRMRapDataDo.Value.AddDays(temp);
            dtp_dayReportDateFrom.Value = dtp_dayReportDateTo.Value.AddDays(temp);


            /**/

            if (zaloguj(formLogin))
            {
                issues.Add("treeView1", new Dictionary<BillingIssueDto, IssueState>());
                issues.Add("treeView2", new Dictionary<BillingIssueDto, IssueState>());
                issues.Add("treeView3", new Dictionary<BillingIssueDto, IssueState>());
                issues.Add("treeView4", new Dictionary<BillingIssueDto, IssueState>());


                helLIU.Login = helLIU.Login + "@cyfrowypolsat.pl";
                doByWorker(new DoWorkEventHandler(doGetComponents), null, new RunWorkerCompletedEventHandler(doGetComponentsCompleted));


                GetUsers();

                cpUsers = new Dictionary<int, HeliosUser>();
                cpUsers.Add(376, new HeliosUser() { imie = "Eryk", nazwisko = "Brzeziński" });

                List<List<string>> cpu = new List<List<string>>();
                try
                {
                    cpu = gujaczWFS.ExecuteStoredProcedure("User_GetSubordinates", new string[] { "376" }, DatabaseName.SupportBPM); // w parametrze id lidera zespołu
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, true);
                }

                foreach (var u in cpu)
                {
                    HeliosUser user = new HeliosUser()
                    {
                        imie = u[4],
                        nazwisko = u[5]
                    };

                    cpUsers.Add(int.Parse(u[0]), user);
                }
                cpUsers.Add(-1, new HeliosUser() { imie = "Cały", nazwisko = "Zespół" });

                foreach (KeyValuePair<int, HeliosUser> kvp in cpUsers)
                {
                    cb_diagnozaUsers.Items.Add(kvp.Value.ToString());
                }

                List<List<string>> userLoginInfo = gujaczWFS.ExecuteStoredProcedure("getJiraAndBpmLogins", null, DatabaseName.SupportCP);
                foreach (var item in userLoginInfo)
                {

                    UserBpmJira ubj = new UserBpmJira(
                                        new UserBpm(Convert.ToInt32(item[0]), item[1], item[2])
                                        , new UserJira((item[3].Contains(string.Empty) ? item[1] : item[3]), (item[4].Contains(string.Empty) ? item[2] : item[4]))
                                        );
                    userBpmJira.Add(ubj);
                }

                

            }
            else
            {
                NoticeForm.ShowNotice("Komponenty nie zostały załadowane. Musisz być zalogowany aby móc dodać sprawy do WFS.");
            }


            jira = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
            jIssue = new JiraIssues("http://jira", jiraUser.Login, jiraUser.Password);

            jira.MaxIssuesPerRequest = 200;

            dgv_SlaRaport.Columns.Add("dgvIssueId", "IssueId");
            dgv_SlaRaport.Columns.Add("dgvJiraNr", "Numer Jira");
            dgv_SlaRaport.Columns.Add("dgvAkcjaBPM", "Ostatnia akcja w BPM");

            dgv_SlaRaport.Columns.Add("dgvOdpowiedzialny", "Odpowiedzialny");
            dgv_SlaRaport.Columns.Add("dgvTypZgloszenia", "Typ zgłoszenia");
            dgv_SlaRaport.Columns.Add("dgvPriorytet", "Priorytet");
            dgv_SlaRaport.Columns.Add("dgvPauza", "Pauza");
            dgv_SlaRaport.Columns.Add("dgvAktCzasRealizacji", "Wypalony czas");
            dgv_SlaRaport.Columns.Add("dgvCzasRozwiazania", "Deklarowany czas");
            dgv_SlaRaport.Columns.Add("dgvPozostaloMin", "Pozostało minut");

            dgv_SlaRaport.Columns.Add("dgvAktualnyStan", "Aktualny stan");
            dgv_SlaRaport.Columns.Add("dgvAktualniePrzydzielony", "Przydzielony");
            dgv_SlaRaport.Columns.Add("dgvAktualnyPriorytet", "Act. Priorytet");
            dgv_SlaRaport.Columns.Add("dgvOstatniaAkcja", "ost. Aktualizacja");

            try
            {
                foreach (var item in jira.GetFilters())
                {
                    cbFilter1name.Items.Add(item);
                    cbFilter2name.Items.Add(item);
                    if (item.Name.Equals(Properties.Settings.Default.assignedFilterName))
                    {
                        cbFilter1name.SelectedItem = cbFilter1name.Items[cbFilter1name.Items.Count - 1];
                    }
                    if (item.Name.Equals(Properties.Settings.Default.unassignedFilterName))
                    {
                        cbFilter2name.SelectedItem = cbFilter2name.Items[cbFilter2name.Items.Count - 1];
                    }
                }
            }
            catch(Exception ex)
            { }


        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (workload != null && workload.IsLogging)
                workload.PauseLogging();

            ni_NewIssueAlert.Dispose();
        }

        ~MainForm()
        {
            gujaczWFS.LogOutFromHPService();
        }

        private void InitializeTempDirectory()
        {
            var localAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var localTempDir = Path.Combine(localAppDataDir, @"Parser\Temp");


            try
            {
                if (Directory.Exists(localTempDir))
                    Directory.Delete(localTempDir, true);

                Directory.CreateDirectory(localTempDir);

                Environment.SetEnvironmentVariable("TMP", localTempDir);
                Environment.SetEnvironmentVariable("TEMP", localTempDir);
            }
            catch (Exception ex)
            {
                //NOTE: wystapi tylko jeśli
                // - inny proces Infolinii jest uruchomiony
                // - inny porces grzebie nie po swoim tempie
            }

        }
        #endregion

        #region SZCZEGÓŁY
        // Zakładka szczegóły

        /// <summary>
        /// Utworzenie nowej notatki do zgłoszenia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DodajNotatke_Click(object sender, EventArgs e)
        {
            note_form.ShowDialog();
            TreeView tr;
            switch (issueTab.SelectedIndex)
            {
                case 0: tr = treeView1; break;
                case 1: tr = treeView2; break;
                default: tr = treeView2; break;
            }

            if (selectIssue != null && note_form.CzyZapisac)
            {
                Note note = generateNote();
                gujaczNotatki.AddNoteToDB(note);
                KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == selectIssue.Idnumber).FirstOrDefault();
                issues[tr.Name].Remove(tmp.Key);
                if (tmp.Key is BillingIssueDtoHelios)
                    ((BillingIssueDtoHelios)tmp.Key).note = note;
                issues[tr.Name].Add(tmp.Key, tmp.Value);
            }
        }

        /// <summary>
        /// Tworzy obiekt notatki uzupełniając go o odpowienie dane
        /// </summary>
        /// <returns></returns>
        private Note generateNote()
        {
            Note notatka = new Note();
            notatka.issueNumber = selectIssue.Idnumber;
            notatka.author = gujaczWFS.getUser().login;
            notatka.date = System.DateTime.Now.ToString();
            notatka.content = note_form.Notatka;
            notatka.content += "\nDodano: " + gujaczWFS.getUser().login + ", " + DateTime.Now.ToString();
            return notatka;
        }

        /// <summary>
        /// Otwiera formatkę Procesy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Procesy_Click(object sender, EventArgs e)
        {
            Procesy proc = new Procesy(this);
            proc.Show();
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu węzła w drzewku zgłoszeń
        /// Wczytuje szczegóły zgłoszenia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tr = sender as TreeView;

            if (tr == null) return;

            StringBuilder sb = new StringBuilder();

            int selectedImageIndex = 0;
            selectedImageIndex = tr.SelectedNode.ImageIndex;
            // Jeżeli zaznaczony węzeł nie jest zgłoszeniem
            //if (e.Node.Level == 0)
            //    return;
            KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == tr.SelectedNode.Text.Split(' ').First()).FirstOrDefault();
            if (tmp.Equals(default(KeyValuePair<IssueDto, IssueState>)))
                Logger.Instance.LogWarning("Nie udało się załadować zgłoszenia z węzła " + e.Node.Text);
            else
            {
                //AddToWorkload(tmp.Key); Workload wyłączony
                BillingIssueDtoHelios it = tmp.Key as BillingIssueDtoHelios;
                List<List<string>> priorytet = gujaczWFS.ExecuteStoredProcedure("Billing_GetListOfPriorities", new string[] { }, DatabaseName.SupportADDONS);

                sb.AppendLine("IssueId BPM: " + it.issueWFS.WFSIssueId.ToString());
                sb.AppendLine("Numer zgłoszenia: " + it.issueHelios.number + " (https://jira/browse/" + it.issueHelios.number + " )");
                sb.AppendLine("Oryginalne ID: " + ((it.issueHelios.oryginalneId != null) ? it.issueHelios.oryginalneId : ""));
                sb.AppendLine("Status: " + it.issueHelios.status);
                sb.AppendLine("Tytuł: " + it.issueWFS.TytulZgloszenia);
                sb.AppendLine("Email: " + it.issueWFS.Email);
                sb.AppendLine("Imię: " + it.issueWFS.Imie);
                sb.AppendLine("Nazwisko: " + it.issueWFS.Nazwisko);

                sb.AppendLine("Data utworzenia: " + it.issueWFS.DataIGodzinaOstatniegoKomentarza);
                sb.AppendLine("Data modyfikacji: " + it.issueWFS.DataWystapieniaBledu);
                if (String.IsNullOrEmpty(it.issueWFS.Priorytet))
                {
                    Logger.Instance.LogWarning("ZGŁOSZENIE " + it.issueHelios.number + " MA PUSTY PRIORYTET!");
                    System.Diagnostics.Debug.WriteLine("ZGŁOSZENIE " + it.issueHelios.number + " MA PUSTY PRIORYTET!");
                    if (it.issueHelios != null)
                        it.issueWFS.Priorytet = it.issueHelios.severity;
                }

                try
                {
                    if (string.IsNullOrEmpty(it.issueWFS.Priorytet) || it.issueWFS.Priorytet.Length > 0)
                        sb.AppendLine("Priorytet: " + priorytet.Where(x => x[0].Equals(it.issueWFS.Priorytet)).FirstOrDefault()[1]);
                    else
                        sb.AppendLine("Priorytet: ");
                }
                catch (Exception ex)
                {
                    sb.AppendLine("Priorytet: ");
                }

                sb.AppendLine("");

                sb.AppendLine("Typ zgłoszenia: " + ((it.issueWFS.System != null) ? it.issueWFS.System.Text : it.issueHelios.rodzaj_zgloszenia));
                sb.AppendLine("Obszar: " + ((it.issueWFS.Kategoria != null) ? it.issueWFS.Kategoria.Text : it.issueHelios.projekt));
                sb.AppendLine("Rodzaj błędu: " + ((it.issueWFS.Rodzaj != null) ? it.issueWFS.Rodzaj.Text : ""));
                sb.AppendLine("Rodzaj błędu w jira: " + ((it.issueHelios.rodzaj_bledu != null) ? it.issueHelios.rodzaj_bledu : ""));
                sb.AppendLine("Typ błedu: " + ((it.issueWFS.Typ != null) ? it.issueWFS.Typ.Text : ""));


                sb.AppendLine("");

                sb.AppendLine("Dane dodatkowe ");
                sb.AppendLine("IdKontraktu: " + ((it.issueWFS.IdKontraktu != null) ? it.issueWFS.IdKontraktu : ""));
                sb.AppendLine("IdKonta: " + ((it.issueHelios.idKonta != null) ? it.issueHelios.idKonta : ""));
                sb.AppendLine("IdZamówienia: " + ((it.issueHelios.idZamowienia != null) ? it.issueHelios.idZamowienia : ""));
                /*sb.AppendLine("Link: " + "https://jira/browse/" + it.issueHelios.number);*/

                sb.AppendLine("\n\nTreść: " + it.issueWFS.TrescZgloszenia);

                Note loadNote = null;
                if (it.note == null)
                    loadNote = gujaczNotatki.SearchIssueNote(it.issueHelios.oryginalneId);


                if (loadNote != null || it.note != null)
                {
                    if (it.note == null)
                    {
                        it.note = loadNote;
                        issues[tr.Name].Remove(tmp.Key);
                        issues[tr.Name].Add(tmp.Key, IssueState.SELECTED);
                    }
                    else
                        loadNote = it.note;
                    StringBuilder sb2 = new StringBuilder();

                    sb2.AppendLine(loadNote.content);
                    sb.AppendLine();
                    sb.AppendLine("------------------------------------------------------NOTATKI------------------------------------------------------\n");
                    sb.Append(sb2.ToString());
                    note_form.Notatka = loadNote.content;
                }
                else
                {
                    note_form.Notatka = "";
                }
            }
            this.richTextBox1.Text = sb.ToString();
            richTextBox1.SelectAll();
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Regular);
            richTextBox1.DeselectAll();

            this.richTextBox1.Select(richTextBox1.Find("IssueId BPM: "), 13);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Numer zgłoszenia: "), 18);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Oryginalne ID: "), 14);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Status: "), 8);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Tytuł: "), 7);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Email: "), 7);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Imię: "), 6);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Nazwisko: "), 10);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Data utworzenia: "), 16);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Data modyfikacji: "), 17);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Priorytet: "), 11);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Typ zgłoszenia: "), 16);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Obszar: "), 8);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Rodzaj błędu: "), 14);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Rodzaj błędu w jira: "), 20);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Typ błedu: "), 11);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);

            this.richTextBox1.Select(richTextBox1.Find("Dane dodatkowe "), 15);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);

            this.richTextBox1.Select(richTextBox1.Find("IdKontraktu: "), 12);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("IdKonta: "), 8);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("IdZamówienia: "), 13);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);
            this.richTextBox1.Select(richTextBox1.Find("Treść: "), 7);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 9, FontStyle.Bold);

            richTextBox1.DeselectAll();

            if (tc.SelectedIndex == 8)
            {
                string number = (sender as TreeView).SelectedNode.Text.Split(' ').FirstOrDefault();
                textBox2.Text = number;
                btn_HistoriaSzukaj_Click(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Po kliknięciu w link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            linkNavigationProcess = System.Diagnostics.Process.Start(e.LinkText);
        }
        #endregion

        #region MULTI
        // Zakładka Multi

        /// <summary>
        /// Przycisk wyczyść listę multi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MultiClear_Click(object sender, EventArgs e)
        {
            multiIssues.Clear();
            lb_MultiList.Items.Clear();
            p_MultiEventMoveButtons.Controls.Clear();
        }

        /// <summary>
        /// Przycisku usuń z multi 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MultiUsun_Click(object sender, EventArgs e)
        {
            string selectedItem = lb_MultiList.SelectedItem.ToString();
            BillingIssueDto tmp = multiIssues.Where(x => x.Key.Idnumber == lb_MultiList.SelectedItem.ToString()).FirstOrDefault().Key;

            multiIssues.Remove(tmp);
            lb_MultiList.Items.Remove(selectedItem);

            if (lb_MultiList.Items.Count == 0)
            {
                p_MultiEventMoveButtons.Controls.Clear();
            }
        }

        /// <summary>
        /// Dodaje zgłoszenie do listy multi
        /// </summary>
        /// <param name="issueNumber">numer zgłoszenia</param>
        private void AddIssueToMultiList(string issueNumber)
        {
            TreeView tr;
            switch (issueTab.SelectedIndex)
            {
                case 0: tr = treeView1; break;
                case 1: tr = treeView2; break;
                default: tr = treeView2; break;
            }

            KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == issueNumber).FirstOrDefault();
            bool onList = false;

            if (tmp.Value != 0)
            {
                try
                {
                    foreach (KeyValuePair<BillingIssueDto, IssueState> kvp in multiIssues)
                    {
                        if (tmp.Key.Idnumber == kvp.Key.Idnumber)
                        {
                            onList = true;
                            break;
                        }
                    }

                    if (!onList)
                    {
                        Dictionary<int, string> actions = new Dictionary<int, string>();
                        bool success = issueMove[tr.Name].TryGetValue(tmp.Key.Idnumber, out actions);
                        bool sameEvents = true;

                        if (success && multiIssues.Count > 0)
                        {
                            var actionsList = actions.ToList();

                            foreach (var item in actionsList)
                            {
                                bool foo = false;

                                foreach (Control c in p_MultiEventMoveButtons.Controls)
                                {
                                    if (item.Value == c.Name)
                                    {
                                        foo = true;
                                        break;
                                    }
                                }

                                if (!foo)
                                {
                                    sameEvents = false;
                                    break;
                                }
                            }
                        }

                        if (sameEvents)
                        {
                            multiIssues.Add(tmp.Key, tmp.Value);
                            lb_MultiList.Items.Add(tmp.Key.Idnumber);

                            if (issueMove[tr.Name] != null && issueMove[tr.Name].ContainsKey(tmp.Key.Idnumber) && success && multiIssues.Count == 1)
                            {
                                multi_ShowButtons(actions);
                            }
                        }
                        else
                        {
                            NoticeForm.ShowNotice("Zgłoszenie " + tmp.Key.Idnumber + " posiada inny status niż wybrane wcześniej zgłoszenia!", "Błąd");
                        }
                    }
                    else
                    {
                        NoticeForm.ShowNotice("Zgłoszenie " + tmp.Key.Idnumber + " znajduje się już na liście!", "Błąd");
                    }
                }
                catch (NullReferenceException ex)
                {
                }
            }
            else
                NoticeForm.ShowNotice("Zgłoszenie " + tmp.Key.Idnumber + " nie znajduje się jeszcze w WFS!", "Błąd");
        }

        /// <summary>
        /// Pokaż przyciski w zakładce Multi
        /// </summary>
        /// <param name="actions"></param>
        private void multi_ShowButtons(Dictionary<int, string> actions)
        {
            var actionsList = actions.ToList();

            p_MultiEventMoveButtons.Controls.Clear();

            int top = 3;
            int left = 3;
            Size btnSize = new Size(234, 32);

            foreach (var item in actionsList)
            {
                Button btn_EventMove = new Button();
                btn_EventMove.Name = item.Value;
                btn_EventMove.Tag = item.Key;
                btn_EventMove.Location = new Point(left, top);
                btn_EventMove.Size = btnSize;
                btn_EventMove.Text = item.Value;
                btn_EventMove.Parent = p_MultiEventMoveButtons;
                btn_EventMove.Click += this.btn_EventMove;

                top += 23 + 9;
            }
            if (actions.Count == 0)
            {
                if (selectIssue.issueWFS.WFSState.Equals("Zgłoszenie zamknięte", StringComparison.CurrentCultureIgnoreCase))
                {
                    Button btn_EventMove = new Button();
                    btn_EventMove.Name = "PonowneOtwarcie";
                    btn_EventMove.Tag = 608;
                    btn_EventMove.Location = new Point(left, top);
                    btn_EventMove.Size = btnSize;
                    btn_EventMove.Text = "Ponowne otwarcie";
                    btn_EventMove.Parent = p_MultiEventMoveButtons;
                    btn_EventMove.Click += this.btn_EventMove;

                    top += 23 + 9;
                }
            }
        }

        /// <summary>
        /// Zdarzenie do wykonania akcji po kliknięciu przycisku w zakładce Multi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EventMove(object sender, EventArgs e)
        {
            string p = sender.ToString();
            int eventMoveId = int.Parse((sender as Button).Tag.ToString());

            TreeView tr;
            switch (issueTab.SelectedIndex)
            {
                case 0: tr = treeView1; break;
                case 1: tr = treeView2; break;
                default: tr = treeView2; break;
            }

            //Pobranie Rozmieszczenia parametrów zdarzenia
            List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

            List<BillingIssueDto> issues = new List<BillingIssueDto>();
            foreach (var item in multiIssues)
            {
                issues.Add(item.Key);
            }

            //wmf = new WFSModelerForm(PolsatUsers, eventParamForFormByEventMove, (sender as Button).Name, issues, gujaczWFS, eventMoveId, new WFSModelerForm.callbackMultiDelegate(ModelerFormActionFinish), tr);
            wmf = new WFSModelerForm(null, eventParamForFormByEventMove, (sender as Button).Name, issues, gujaczWFS, eventMoveId, new WFSModelerForm.callbackMultiDelegate(ModelerFormActionFinish), tr);

            try
            {
                if (!wmf.IsDisposed)
                    wmf.Show();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogWarning("Błąd powiadomienia", Logger.Instance);
            }
        }

        /// <summary>
        /// Po zmianie zaznaczenia w liście multi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_MultiList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_MultiList.SelectedIndex >= 0)
                btn_MultiUsun.Enabled = true;
            else
                btn_MultiUsun.Enabled = false;
        }
        #endregion

        #region UŻYTKOWNICY
        // Zakładka Użytkownicy

        /// <summary>
        /// Zapisanie danych o użytkowniku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveUser_Click(object sender, EventArgs e)
        {
            //HeliosUser tmpUser = new HeliosUser()
            //{
            //    email = tbx_userEmail.Text,
            //    nazwisko = tbx_userSurname.Text,
            //    imie = tbx_userFirstname.Text,
            //    telefon = tbx_userPhone.Text
            //};
            //if (tmpUser.email != string.Empty && tmpUser.imie != string.Empty && tmpUser.nazwisko != string.Empty)
            //{
            //    bool isNew = !PolsatUsers.Any(x => x.email.ToLower() == tbx_userEmail.Text.ToLower());
            //    usersHelios.SaveUserData(tmpUser, isNew);
            //    GetUsers();
            //}
            //else
            //    NoticeForm.ShowNotice("Brak odpowienich danych użytkownika jak email, nazwisko czy imię.", "Brak danych");
        }

        /// <summary>
        /// Pobiera dane o użytkownikach z Bazy danych
        /// </summary>
        private void GetUsers()
        {
            //lv_Users.Items.Clear();
            //try
            //{
            //    WaitingForm.InvokeWithWaitingForm("Trwa wczytywanie danych", () =>
            //    {
            //        PolsatUsers = usersHelios.GetAllUsers();
            //        foreach (Entities.HeliosUser item in PolsatUsers)
            //        {
            //            ListViewItem it = new ListViewItem(item.email);
            //            it.SubItems.Add(item.imie);
            //            it.SubItems.Add(item.nazwisko);
            //            it.SubItems.Add(item.telefon);
            //            lv_Users.Invoke((MethodInvoker)delegate
            //            {
            //                lv_Users.Items.Add(it);
            //            });
            //        }
            //    });

            //}
            //catch (Exception ex)
            //{
            //    ExceptionManager.LogError(ex, Logger.Instance, true);
            //}
        }

        /// <summary>
        /// Filtruje użytkowników wg podanych kyteriów jak nazwisko lub email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_SearchChanged(object sender, EventArgs e)
        {
            //List<HeliosUser> tmp = usersHelios.FilterUsers(tb_surnameSearch.Text, tb_emailSearch.Text, PolsatUsers);
            //lv_Users.Items.Clear();
            //foreach (Entities.HeliosUser item in tmp)
            //{
            //    ListViewItem it = new ListViewItem(item.email);
            //    it.SubItems.Add(item.imie);
            //    it.SubItems.Add(item.nazwisko);
            //    it.SubItems.Add(item.telefon);
            //    lv_Users.Items.Add(it);
            //}

        }

        /// <summary>
        /// Po zmienie wyboru w liście użytkowników
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #endregion

        #region BILLING
        // Zakładka Billing

        /// <summary>
        /// Przycisk raport stanu zgłoszeń 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BillingRaportStanuZgloszen_Click(object sender, EventArgs e)
        {
            tabStatystyki.TabPages.Clear();

            List<List<string>> results = gujaczWFS.ExecuteStoredProcedure("BillingDTH_RaportStanuZgloszen", new string[] { dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.AddDays(1).ToShortDateString() }, DatabaseName.SupportADDONS);

            if (results.Count == 0)
            {
                NoticeForm.ShowNotice("Brak zgłoszeń.", "Info");
                tabStatystyki.Visible = false;
                richTextBoxStats.Visible = true;
                return;
            }

            tabStatystyki.Visible = true;
            richTextBoxStats.Visible = false;
            zglDgv.Visible = false;


            foreach (List<string> row in results)
            {
                if (!tabStatystyki.TabPages.ContainsKey(row[1]))
                {
                    tabStatystyki.TabPages.Add(row[1], row[1]);

                    DataGridView dgv = new DataGridView();
                    dgv.Columns.Add("NumerZgloszeniaWfs", "Numer zgłoszenia WFS");
                    dgv.Columns.Add("NumerZgloszeniaHelios", "Numer zgłoszenia Jira");
                    dgv.Columns.Add("DataUtworzeniaHelios", "Data utworzenia Jira");
                    dgv.Columns.Add("DataUtworzeniaWFS", "Data utworzenia WFS");
                    dgv.Dock = DockStyle.Fill;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader | DataGridViewAutoSizeColumnsMode.AllCells;

                    dgv.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);

                    tabStatystyki.TabPages[row[1]].Controls.Add(dgv);
                }

                DataGridView dg = tabStatystyki.TabPages[row[1]].Controls[0] as DataGridView;

                if (dg != null)
                {
                    dg.Rows.Insert(0, new DataGridViewRow());
                    dg.Rows[0].Cells[0].Value = row[0];
                    dg.Rows[0].Cells[1].Value = row[2];
                    dg.Rows[0].Cells[2].Value = row[3];
                    dg.Rows[0].Cells[3].Value = row[4];
                }
            }
        }

        /// <summary>
        /// Przycisk zgłoszenia z dzisiaj 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BillingZgloszeniaZDzisiaj_Click(object sender, EventArgs e)
        {
            tabStatystyki.Visible = false;
            zglDgv.Visible = false;
            richTextBoxStats.Visible = true;

            richTextBoxStats.Text = "Czekaj...";
            setBusy(true);
            try
            {
                int WszystkiePozostale = 0;
                int ZgloszeniaOtwartePozostale = 0;
                int ZgloszeniaZamknietePozostale = 0;
                string DiagnozaZgłoszeniaPozostale = "0";
                string ZgłoszeniaZamkniętePoDiagnoziePozostale = "0";
                string ZgłoszeniaZamkniętePoRealizacjiPozostale = "0";
                string ZgłoszeniaZamkniętePoWeryfikacjiPozostale = "0";
                string ZgłoszeniaZamknięteDuplikatPozostale = "0";
                string ZgłoszeniaZamkniętePrzekazaneDoDevCPPozostale = "0";
                string PonownieOtwartychZgłoszeniaPozostale = "0";
                string WeryfikacjaZgłoszeniaPozostale = "0";
                string CofnięcieDoPoprawyPozostale = "0";
                string PrzekazanieD0DeweloperePozostale = "0";
                string PrzekazanieDoKonsultacjiBiznesowejPozostale = "0";
                string PrzekazanieDoKonsultacjiDeweloperskiejPozostale = "0";
                string PrzyjęcieDoRealizacjiPozostale = "0";
                string RozpoczęcieDiagnozyPozostale = "0";
                string PrzyjęcieDoWeryfikacjiPozostale = "0";

                int Wszystkie = 0;
                string ZgloszeniaOtwarte = "0";
                string ZgloszeniaZamkniete = "0";
                string DiagnozaZgłoszenia = "0";
                string ZgłoszeniaZamkniętePoDiagnozie = "0";
                string ZgłoszeniaZamkniętePoRealizacji = "0";
                string ZgłoszeniaZamkniętePoWeryfikacji = "0";
                string ZgłoszeniaZamknięteDuplikat = "0";
                string ZgłoszeniaZamkniętePrzekazaneDoDevCP = "0";
                string PonownieOtwartychZgłoszenia = "0";
                string WeryfikacjaZgłoszenia = "0";
                string CofnięcieDoPoprawyZDzisiaj = "0";
                string PrzekazanieD0DewelopereZDzisiaj = "0";
                string PrzekazanieDoKonsultacjiBiznesowejZDzisiaj = "0";
                string PrzekazanieDoKonsultacjiDeweloperskiejZDzisiaj = "0";
                string PrzyjęcieDoRealizacjiZDzisiaj = "0";
                string RozpoczęcieDiagnozyZDzisiaj = "0";
                string PrzyjęcieDoWeryfikacjiZDzisiaj = "0";
                try
                {
                    List<List<string>> sts = gujaczWFS.ExecuteStoredProcedure("BillingDTH_RaportDziennyZgloszeniaZDzisiaj", new string[] { }, Entities.Enums.DatabaseName.SupportADDONS);
                    //Dictionary<int, string> stats = gujaczWFS.ExecProcedure("BillingDTH_RaportDziennyZgloszeniaZDzisiaj", null, "Support_ADDONS");

                    Dictionary<int, string> stats = new Dictionary<int, string>();
                    foreach (var item in sts)
                    {
                        try
                        {
                            stats.Add(Convert.ToInt32(item[0]), item[1]);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    foreach (KeyValuePair<int, string> st in stats)
                    {
                        // Gr1
                        if (st.Value.Equals("0"))
                        {
                            Wszystkie = st.Key;
                        }

                        if (st.Key == 702)
                        {
                            ZgloszeniaOtwarte = st.Value.ToString();
                        }
                        if (st.Key == 704)
                        {
                            ZgloszeniaZamkniete = st.Value.ToString();
                        }

                        if (st.Key == 614)
                        {
                            DiagnozaZgłoszenia = st.Value.ToString();
                        }

                        // Gr2
                        else if (st.Key == 605)
                        {
                            CofnięcieDoPoprawyZDzisiaj = st.Value.ToString();

                        }
                        else if (st.Key == 609)
                        {
                            PrzekazanieD0DewelopereZDzisiaj = st.Value.ToString();

                        }
                        else if (st.Key == 610)
                        {
                            PrzekazanieDoKonsultacjiBiznesowejZDzisiaj = st.Value.ToString();

                        }
                        else if (st.Key == 611)
                        {
                            PrzekazanieDoKonsultacjiDeweloperskiejZDzisiaj = st.Value.ToString();

                        }
                        else if (st.Key == 612)
                        {
                            PrzyjęcieDoRealizacjiZDzisiaj = st.Value.ToString();

                        }
                        else if (st.Key == 613)
                        {
                            PrzyjęcieDoWeryfikacjiZDzisiaj = st.Value.ToString();

                        }

                        else if (st.Key == 618)
                        {
                            ZgłoszeniaZamkniętePoRealizacji = st.Value.ToString();

                        }
                        else if (st.Key == 616)
                        {
                            WeryfikacjaZgłoszenia = st.Value.ToString();

                        }
                        // Gr4
                        else if (st.Key == 617)
                        {
                            ZgłoszeniaZamkniętePoDiagnozie = st.Value.ToString();

                        }
                        else if (st.Key == 619)
                        {
                            ZgłoszeniaZamkniętePoWeryfikacji = st.Value.ToString();

                        }
                        else if (st.Key == 620)
                        {
                            ZgłoszeniaZamknięteDuplikat = st.Value.ToString();

                        }
                        else if (st.Key == 723)
                        {
                            ZgłoszeniaZamkniętePrzekazaneDoDevCP = st.Value.ToString();

                        }
                    }
                }
                catch { }
                try
                {
                    List<List<string>> sts2 = gujaczWFS.ExecuteStoredProcedure("BillingDTH_RaportDziennyZgloszeniaZDzisiajPozostale", new string[] { }, Entities.Enums.DatabaseName.SupportADDONS);
                    //Dictionary<int, string> stats2 = gujaczWFS.ExecProcedure("BillingDTH_RaportDziennyZgloszeniaZDzisiajPozostale", null, "Support_ADDONS");

                    Dictionary<int, string> stats2 = new Dictionary<int, string>();

                    foreach (var item in sts2)
                    {
                        try
                        {
                            stats2.Add(Convert.ToInt32(item[0]), item[1]);
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                    foreach (KeyValuePair<int, string> st in stats2)
                    {
                        //// Gr1
                        //if (st.Value.Equals("0"))
                        //{
                        //    WszystkiePozostale = st.Key;
                        //}

                        //if (st.Key == 702)
                        //{
                        //    ZgloszeniaOtwartePozostale = st.Value.ToString();
                        //}
                        //if (st.Key == 704)
                        //{
                        //    ZgloszeniaZamknietePozostale = st.Value.ToString();
                        //}

                        if (st.Key == 614)
                        {
                            DiagnozaZgłoszeniaPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }

                        // Gr2
                        if (st.Key == 605)
                        {
                            CofnięcieDoPoprawyPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 609)
                        {
                            PrzekazanieD0DeweloperePozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 610)
                        {
                            PrzekazanieDoKonsultacjiBiznesowejPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 611)
                        {
                            PrzekazanieDoKonsultacjiDeweloperskiejPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 612)
                        {
                            PrzyjęcieDoRealizacjiPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 613)
                        {
                            PrzyjęcieDoWeryfikacjiPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);
                        }

                        else if (st.Key == 616)
                        {
                            WeryfikacjaZgłoszeniaPozostale = st.Value.ToString();
                            ZgloszeniaOtwartePozostale += Convert.ToInt32(st.Value);

                        }

                        else if (st.Key == 618)
                        {
                            ZgłoszeniaZamkniętePoRealizacjiPozostale = st.Value.ToString();
                            ZgloszeniaZamknietePozostale += Convert.ToInt32(st.Value);
                        }

                        // Gr4
                        else if (st.Key == 617)
                        {
                            ZgłoszeniaZamkniętePoDiagnoziePozostale = st.Value.ToString();
                            ZgloszeniaZamknietePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 619)
                        {
                            ZgłoszeniaZamkniętePoWeryfikacjiPozostale = st.Value.ToString();
                            ZgloszeniaZamknietePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 620)
                        {
                            ZgłoszeniaZamknięteDuplikatPozostale = st.Value.ToString();
                            ZgloszeniaZamknietePozostale += Convert.ToInt32(st.Value);
                        }
                        else if (st.Key == 723)
                        {
                            ZgłoszeniaZamkniętePrzekazaneDoDevCPPozostale = st.Value.ToString();
                            ZgloszeniaZamknietePozostale += Convert.ToInt32(st.Value);
                        }

                    }
                    WszystkiePozostale += ZgloszeniaOtwartePozostale + ZgloszeniaZamknietePozostale;
                }
                catch { }

                // Gr1 565 614 Rozpoczęcie diagnozy

                // Gr2 556 605 Cofnięcie do poprawy
                // Gr2 560 609 Przekazanie do dewelopere
                // Gr2 561 610 Przekazanie do konsultacji biznesowej
                // Gr2 562 611 Przekazanie do konsultacji deweloperskiej
                // Gr2 563 612 Przyjęcie do realizacji
                // Gr2 564 613 Przyjęcie do weryfikacji

                // Gr3 568 617 Zamknięcie zgłoszenia
                // Gr3 569 618 Zamknij zgłoszenie
                // Gr3 569 619 Zamknij zgłoszenie

                // Gr4 570 620 Zgłoszenie duplikatu zgłoszenia

                richTextBoxStats.Text = "";
                richTextBoxStats.AppendText("Statystyki zgłoszeń BillingDTH za dzień " + DateTime.Now.ToShortDateString() + ":");
                richTextBoxStats.AppendText("\n");
                richTextBoxStats.AppendText("\nZgłoszenia Jira utworzone w dniu " + DateTime.Now.ToShortDateString() + ":");
                richTextBoxStats.AppendText("\n");
                richTextBoxStats.AppendText("\nWszystkie: " + Wszystkie);
                richTextBoxStats.AppendText("\n     Zgłoszenia Otwarte: " + ZgloszeniaOtwarte);
                richTextBoxStats.AppendText("\n          Przekazanie do konsultacji deweloperskiej: " + PrzekazanieDoKonsultacjiDeweloperskiejZDzisiaj);
                richTextBoxStats.AppendText("\n          Przekazanie do konsultacji biznesowej: " + PrzekazanieDoKonsultacjiBiznesowejZDzisiaj);
                richTextBoxStats.AppendText("\n          Przekazanie do dewelopera: " + PrzekazanieD0DewelopereZDzisiaj);
                richTextBoxStats.AppendText("\n          Diagnoza zgłoszenia: " + DiagnozaZgłoszenia);
                richTextBoxStats.AppendText("\n          Cofnięcie do poprawy: " + CofnięcieDoPoprawyZDzisiaj);
                richTextBoxStats.AppendText("\n          Ponownie otwartych zgłoszenia: " + PonownieOtwartychZgłoszenia);
                richTextBoxStats.AppendText("\n          Weryfikacja zgłoszenia: " + WeryfikacjaZgłoszenia);
                richTextBoxStats.AppendText("\n          Przyjęcie do weryfikacji: " + PrzyjęcieDoWeryfikacjiZDzisiaj);
                richTextBoxStats.AppendText("\n          Przyjęcie do realizacji: " + PrzyjęcieDoRealizacjiZDzisiaj);
                richTextBoxStats.AppendText("\n     Zgłoszenia zamknięte: " + ZgloszeniaZamkniete);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po realizacji: " + ZgłoszeniaZamkniętePoRealizacji);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - przekazanie do dev CP: " + ZgłoszeniaZamkniętePrzekazaneDoDevCP);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po diagnozie: " + ZgłoszeniaZamkniętePoDiagnozie);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po weryfikacji: " + ZgłoszeniaZamkniętePoWeryfikacji);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - duplikat: " + ZgłoszeniaZamknięteDuplikat);

                richTextBoxStats.AppendText("\n");
                richTextBoxStats.AppendText("\n");
                richTextBoxStats.AppendText("\nPozostałe zgłoszenia Jira:");
                richTextBoxStats.AppendText("\n");
                richTextBoxStats.AppendText("\nWszystkie: " + WszystkiePozostale);
                richTextBoxStats.AppendText("\n     Zgłoszenia Otwarte: " + ZgloszeniaOtwartePozostale);
                richTextBoxStats.AppendText("\n          Przekazanie do konsultacji deweloperskiej: " + PrzekazanieDoKonsultacjiDeweloperskiejPozostale);
                richTextBoxStats.AppendText("\n          Przekazanie do konsultacji biznesowej: " + PrzekazanieDoKonsultacjiBiznesowejPozostale);
                richTextBoxStats.AppendText("\n          Przekazanie do dewelopera: " + PrzekazanieD0DeweloperePozostale);
                richTextBoxStats.AppendText("\n          Diagnoza zgłoszenia: " + DiagnozaZgłoszeniaPozostale);
                richTextBoxStats.AppendText("\n          Cofnięcie do poprawy: " + CofnięcieDoPoprawyPozostale);
                richTextBoxStats.AppendText("\n          Ponownie otwartych zgłoszenia: " + PonownieOtwartychZgłoszeniaPozostale);
                richTextBoxStats.AppendText("\n          Weryfikacja zgłoszenia: " + WeryfikacjaZgłoszeniaPozostale);
                richTextBoxStats.AppendText("\n          Przyjęcie do weryfikacji: " + PrzyjęcieDoWeryfikacjiPozostale);
                richTextBoxStats.AppendText("\n          Przyjęcie do realizacji: " + PrzyjęcieDoRealizacjiPozostale);
                richTextBoxStats.AppendText("\n     Zgłoszenia zamknięte: " + ZgloszeniaZamknietePozostale);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po realizacji: " + ZgłoszeniaZamkniętePoRealizacjiPozostale);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - przekazanie do dev CP: " + ZgłoszeniaZamkniętePrzekazaneDoDevCPPozostale);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po diagnozie: " + ZgłoszeniaZamkniętePoDiagnoziePozostale);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - po weryfikacji: " + ZgłoszeniaZamkniętePoWeryfikacjiPozostale);
                richTextBoxStats.AppendText("\n          Zgłoszenia zamknięte - duplikat: " + ZgłoszeniaZamknięteDuplikatPozostale);

                richTextBoxStats.SelectAll();
                richTextBoxStats.SelectionColor = Color.Black;
                richTextBoxStats.SelectionFont = new Font(richTextBoxStats.Font.FontFamily, 9, FontStyle.Regular);
                richTextBoxStats.DeselectAll();

                richTextBoxStats.Select(richTextBoxStats.Find("Pozostałe zgłoszenia Jira:"), 28);
                richTextBoxStats.SelectionColor = Color.Green;
                richTextBoxStats.SelectionFont = new Font(richTextBoxStats.Font.FontFamily, 10, FontStyle.Bold);

                richTextBoxStats.Select(richTextBoxStats.Find("Zgłoszenia Jira utworzone w dniu " + DateTime.Now.ToShortDateString() + ":"), 46);
                richTextBoxStats.SelectionColor = Color.Green;
                richTextBoxStats.SelectionFont = new Font(richTextBoxStats.Font.FontFamily, 10, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
            setBusy(false);
        }

        /// <summary>
        /// Przycisk zgłoszenia - kategoryzacja 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BillingZgloszeniaKategoryzacja_Click(object sender, EventArgs e)
        {
            List<List<string>> results = gujaczWFS.ExecuteStoredProcedure("BillingDTH_RaportKategoryzacja", null, DatabaseName.SupportADDONS);

            if (results.Count == 0)
            {
                NoticeForm.ShowNotice("Brak zgłoszeń.", "Info");
                //MessageBox.Show("Brak zgłoszeń.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabStatystyki.Visible = false;
                zglDgv.Visible = false;
                richTextBoxStats.Visible = true;
                return;
            }

            richTextBoxStats.Visible = false;
            tabStatystyki.Visible = false;
            zglDgv.Visible = true;

            zglDgv.Columns.Clear();

            zglDgv.Columns.Add("NumerZgłoszenia", "Numer zgłoszenia Jira");
            zglDgv.Columns.Add("IdKontraktu", "Id Kontraktu");
            zglDgv.Columns.Add("DataZgłoszenia", "Data Zgłoszenia");
            zglDgv.Columns.Add("System", "Rodzaj zgłoszenia");
            zglDgv.Columns.Add("Kategoria", "Katalog");
            zglDgv.Columns.Add("Rodzaj", "Rodzaj błędu");
            zglDgv.Columns.Add("Typ", "Typ");
            zglDgv.Columns.Add("Akcja", "Akcja");
            zglDgv.Columns.Add("Status", "Status");
            zglDgv.Columns.Add("Osoba", "Osoba");
            zglDgv.Columns.Add("Konsultant", "Konsultant");

            foreach (List<string> row in results)
            {
                zglDgv.Rows.Insert(0, new DataGridViewRow());
                zglDgv.Rows[0].Cells[0].Value = row[0];
                zglDgv.Rows[0].Cells[1].Value = row[1];
                zglDgv.Rows[0].Cells[2].Value = row[2];
                zglDgv.Rows[0].Cells[3].Value = row[3];
                zglDgv.Rows[0].Cells[4].Value = row[4];
                zglDgv.Rows[0].Cells[5].Value = row[5];
                zglDgv.Rows[0].Cells[6].Value = row[6];
                zglDgv.Rows[0].Cells[7].Value = row[7];
                zglDgv.Rows[0].Cells[8].Value = row[8];
                zglDgv.Rows[0].Cells[9].Value = row[9];
                zglDgv.Rows[0].Cells[10].Value = row[10];
            }
        }

        /// <summary>
        /// btn_BillingZapiszDoXls 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BillingZapiszDoXls_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // creating Excel Application 
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                // creating new WorkBook within Excel application 
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                // creating new Excelsheet in workbook 
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

                // see the excel sheet behind the program 
                app.Visible = false;

                // get the reference of first sheet. By default its name is Sheet1. 

                // store its reference to worksheet 
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                //worksheet = workbook.ActiveSheet;

                // changing the name of active sheet 
                worksheet.Name = "Exported from gridview";

                // storing header part in Excel 
                for (int i = 1; i < zglDgv.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = zglDgv.Columns[i - 1].HeaderText;
                }

                // storing Each row and column value to excel sheet 
                for (int i = 0; i < zglDgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < zglDgv.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = zglDgv.Rows[i].Cells[j].Value.ToString();
                    }
                }

                // save the application 
                workbook.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // Exit from the application 
                workbook.Close();
                app.Quit();
            }
        }

        /// <summary>
        /// Billing DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvUserDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgvr = sender as DataGridView;

            if (dgvr != null)
            {
                using (SolidBrush b = new SolidBrush(dgvr.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
                }
            }
        }
        #endregion

        #region CRM
        // Zakładka CRM

        /// <summary>
        /// Przycisk Raport18 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_CRMRaport18_Click(object sender, EventArgs e)
        {
            generateServiceReport(crm_rtb);
        }

        /*
        Private Shared Function InsertTableInRichTextBox(rows As Integer, cols As Integer, width As Integer) As[String]
	'Create StringBuilder Instance
	Dim sringTableRtf As New StringBuilder()

	'beginning of rich text format
	sringTableRtf.Append("{\rtf1 ")

	'Variable for cell width
	Dim cellWidth As Integer

	'Start row
	'sringTableRtf.Append(@"\trowd");

	'Loop to create table string

    Dim i As Integer = 1

    While i <= rows
        sringTableRtf.Append("\trowd\trqc")
		'trqc is center table
		Dim j As Integer = 1

        While j <= cols
			'Calculate cell end point for each cell
			cellWidth = (j) * width

			'A cell with width 1000 in each iteration.

            sringTableRtf.Append("\cellx" + cellWidth.ToString())
			j += 1
		End While

		'Append the row in StringBuilder

        sringTableRtf.Append("\intbl \cell \row")
		i += 1
	End While

    sringTableRtf.Append("\pard")
	sringTableRtf.Append("}")

	Return sringTableRtf.ToString()
End Function*/

        private void generateServiceReport(RichTextBox destyny)
        {

            ExchangeClient ex = new ExchangeClient(gujaczWFS.getUser());
            string mailBody = ex.LoadTemplate("availabilityReport");
            string SummaryBlock = "";
            StringBuilder AllBlock = new StringBuilder();

            toolStripStatusLabel6.Text = "Generowanie raportu dziennego";
            setBusy(true);
            try
            {
                DateTime now = dtp_dayReportDateTo.Value;
                DateTime yesterday = dtp_dayReportDateFrom.Value;

                string dateFrom = dtp_dayReportDateFrom.Value.ToShortDateString();
                string dateTo = dtp_dayReportDateTo.Value.ToShortDateString();

                string timeFrom = tb_dayReportTimeFrom.Text;
                string timeTo = tb_dayReportTimeTo.Text;

                string tab = "         ";
                string enter = "\n";
                string newLine = "\r\n";

                
                DateTime dtFrom;
                DateTime dtTo;

                List<string> obszaryB = new List<string>();

                try
                {
                    dtFrom = DateTime.Parse(dateFrom + " " + timeFrom);
                    dtTo = DateTime.Parse(dateTo + " " + timeTo);
                }
                catch (Exception error)
                {
                    NoticeForm.ShowNotice("Błędnie uzupełnione dane formularza!");
                    //MessageBox.Show("Błędnie uzupełnione dane formularza!");
                    return;
                }

                //Powitanie:

                try
                {
                    //GemBox 
                    //    ;.Document dd = new GemBox.Document();
                    

                    string header = "Witam, " +
                    enter + enter + "W dniach " + formatDate(dtp_dayReportDateFrom.Value) + " " + dtFrom.ToShortTimeString() + " - " + formatDate(dtp_dayReportDateTo.Value) + " " + dtTo.ToShortTimeString();


                    string actObszar = "ALL";
                    //dane z bazy:
                    List<List<string>> resultss = gujaczWFS.ExecuteStoredProcedure("spRaportDostepnosciSrodowisk_v6", new string[] { dtFrom.ToString(), dtTo.ToString() }, DatabaseName.SupportCP);

                    HtmlTable awTable = new HtmlTable();
                    awTable.AddColumn("Nr awarii");
                    awTable.AddColumn("Data początkowa");
                    awTable.AddColumn("Data zakończenia");
                    awTable.AddColumn("Uwagi");


                    if (resultss[0].Where(x => x == "AWARIA!!!").Count() > 0)
                    {
                        header += " wystąpiły następujące problemy z dostępnością wspieranych środowisk: ";
                        header += enter;
                        foreach (var item in resultss)
                        {
                            if (item[0] == "AWARIA!!!")
                            {
                                
                                header += "Sprawdź zgłoszenie: " + item[1] + enter;
                                awTable.AddRow(new string[] { item[1], string.Empty, string.Empty, string.Empty });
                            }
                        }
                    }
                    else
                    {
                        header += " nie wystąpiły żadne problemy z dostępnością wspieranych środowisk. ";
                    }

                    SautinSoft.HtmlToRtf h = new SautinSoft.HtmlToRtf();
                    string htmlString;
                    string rtfString;
                    string rtftable = InsertTableInRichtextbox(destyny);
                    //if (awTable.Rows.Count > 0)
                    //{

                    //    //SautinSoft.HtmlToRtf h = new SautinSoft.HtmlToRtf();
                    //    htmlString = awTable.ToHtml();
                    //    rtfString = h.ConvertString(htmlString);


                    //    //HtmlToRtf(awTable.ToHtml(), rtb_dayReportMessage);
                    //    rtb_dayReportMessage.Rtf += rtfString;
                    //}

                    rtfString = //"<html><p>" +
                                header +
                                enter +
                                enter + "Dane o zgłoszeniach od dnia " + dtFrom.ToShortTimeString() + " " + formatDate(dtp_dayReportDateFrom.Value) + " do dnia " + dtTo.ToShortTimeString() + " " + formatDate(dtp_dayReportDateTo.Value)
                                ;// + "</p></html>";

                    //destyny.Rtf += h.ToRtf(rtfString);
                    //destyny.Rtf += rtftable;
                    destyny.Text = rtfString;



                    foreach (var st in resultss)
                    {

                        if (st[0] != "AWARIA!!!")
                        {
                            if (actObszar != st[0])
                            {

                                //zmana obszaru
                                destyny.AppendText(newLine);
                                destyny.AppendText(enter + "OBSZAR " + st[0] + ": " + enter);
                                obszaryB.Add("OBSZAR " + st[0] + ": ");

                                //dodanie nowych
                                destyny.AppendText(enter);
                                for (int i = 0; i < Convert.ToInt32(st[1]); i++)
                                {
                                    destyny.AppendText(tab);
                                }
                                destyny.AppendText(st[2]);
                                destyny.AppendText(": ");
                                destyny.AppendText(st[3]);
                                
                            }
                            else
                            {
                                destyny.AppendText(newLine);
                                for (int i = 0; i < Convert.ToInt32(st[1]); i++)
                                {
                                    destyny.AppendText(tab);
                                }
                                destyny.AppendText(st[2]);
                                destyny.AppendText(": ");
                                destyny.AppendText(st[3]);
                                
                            }

                            actObszar = st[0];
                        }
                    }

                    destyny.SelectAll();
                    destyny.SelectionFont = new Font(destyny.Font.FontFamily, destyny.Font.Size, FontStyle.Regular);
                    destyny.DeselectAll();
                    if (obszaryB.Count > 0)
                    {
                        destyny.DeselectAll();
                        destyny.Select(0, destyny.Find(" do dnia " + dtTo.ToShortTimeString() + " " + formatDate(dtp_dayReportDateTo.Value)) + (" do dnia " + dtTo.ToShortTimeString() + " " + formatDate(dtp_dayReportDateTo.Value)).Length);
                        destyny.SelectionFont = new Font(destyny.Font.FontFamily, destyny.Font.Size, FontStyle.Bold);
                        destyny.DeselectAll();
                        foreach (string item in obszaryB)
                        {
                            destyny.Select(destyny.Find(item), item.Length + 1);
                            destyny.SelectionFont = new Font(destyny.Font.FontFamily, destyny.Font.Size, FontStyle.Bold);

                            destyny.DeselectAll();
                        }
                    }
                    destyny.DeselectAll();
                    
                    //mailBody.Replace("{{AllBlock}}", AllBlock.ToString());
                    //destyny.Text = mailBody;
                }
                catch (Exception ep)
                {
                    NoticeForm.ShowNotice(ep.Message);
                }
                

            }
            catch (Exception ep)
            {
                ExceptionManager.LogError(ep, Logger.Instance, true);
            }
            setBusy(false);
            toolStripStatusLabel6.Text = string.Empty;
        }
        private string InsertTableInRichtextbox(RichTextBox destyny)
        {
            //CreateStringBuilder object
            StringBuilder strTable = new StringBuilder();

            //Beginning of rich text format,don’t alter this line
            strTable.Append(@"{\rtf1 ");

            //Create 5 rows with 4 columns
            for (int i = 0; i < 5; i++)
            {
                //Start the row
                strTable.Append(@"\trowd");

                //First cell with width 1000.
                strTable.Append(@"\cellx1000");

                //Second cell with width 1000.Ending point is 2000, which is 1000+1000.
                strTable.Append(@"\cellx2000");

                //Third cell with width 1000.Endingat3000,which is 2000+1000.
                strTable.Append(@"\cellx3000");

                //Last cell with width 1000.Ending at 4000 (which is 3000+1000)
                strTable.Append(@"\cellx4000");

                //Append the row in StringBuilder
                strTable.Append(@"\intbl \cell \row"); //create the row
            }

            strTable.Append(@"\pard");

            strTable.Append(@"}");

            return strTable.ToString();
        }


        public void HtmlToRtf(string htmlText, RichTextBox destynation)
        {
            
            var webBrowser = new WebBrowser();
            webBrowser.CreateControl(); // only if needed
            webBrowser.DocumentText = htmlText;
            while (webBrowser.DocumentText != htmlText)
                Application.DoEvents();
            webBrowser.Document.ExecCommand("SelectAll", false, null);
            webBrowser.Document.ExecCommand("Copy", false, null);
            destynation.Paste();
            //*yourRichTextControl *.Paste();
        }
        /*//poprzednia wersja raportu  
        private void btn_CRMRaport18_Click(object sender, EventArgs e)
                {
                    crm_rtb.Text = "Czekaj...";
                    setBusy(true);
                    try
                    {
                        DateTime now = dtp_CRMRapDataDo.Value;
                        DateTime yesterday = dtp_CRMRapDataOd.Value;

                        string dateFrom = yesterday.ToShortDateString();
                        string dateTo = now.ToShortDateString();

                        string timeFrom = tb_CRMRapGodzinaOd.Text;
                        string timeTo = tb_CRMRapGodzinaDo.Text;

                        string tab = "\t";
                        string enter = "\n";


                        DateTime dtFrom;
                        DateTime dtTo;

                        try
                        {
                            dtFrom = DateTime.Parse(dateFrom + " " + timeFrom);
                            dtTo = DateTime.Parse(dateTo + " " + timeTo);
                        }
                        catch (Exception ex)
                        {
                            NoticeForm.ShowNotice("Błędnie uzupełnione dane formularza!");
                            //MessageBox.Show("Błędnie uzupełnione dane formularza!");
                            return;
                        }

                        List<int> ilosc = new List<int>();
                        String All	                         = "0";
                        String Nowe                         = "0";
                        String PonownieOtwarte              = "0";
                        String CRM_Nowe                     = "0";
                        String CRM_POtwarte                 = "0";
                        String CRM_Zamkniete                = "0";
                        String CRM_Zamkniete_Samodzielnie   = "0";
                        String CRM_Zamkniete_IIILinia       = "0";
                        String CRM_Zamkniete_IILinia        = "0";
                        String CRM_Zamkniete_Odrzucone      = "0";
                        String CRM_Zamkniete_Duplikat       = "0";
                        String CRM_Konsultacja              = "0";
                        String CRM_Konsultacja_biz          = "0";
                        String CRM_Konsultacja_dev          = "0";
                        String WIND_Nowe                    = "0";
                        String WIND_POtwarte                = "0";
                        String WIND_Zamkniete               = "0";
                        String WIND_Zamkniete_Samodzielnie  = "0";
                        String WIND_Zamkniete_IIILinia      = "0";
                        String WIND_Zamkniete_IILinia       = "0";
                        String WIND_Zamkniete_Odrzucone     = "0";
                        String WIND_Zamkniete_Duplikat      = "0";
                        String WIND_Konsultacja             = "0";
                        String WIND_Konsultacja_biz         = "0";
                        String WIND_Konsultacja_dev         = "0";
                        String ZCC_Nowe                     = "0";
                        String ZCC_POtwarte                 = "0";
                        String ZCC_Zamkniete                = "0";
                        String ZCC_Zamkniete_Samodzielnie   = "0";
                        String ZCC_Zamkniete_IIILinia       = "0";
                        String ZCC_Zamkniete_IILinia        = "0";
                        String ZCC_Zamkniete_Odrzucone      = "0";
                        String ZCC_Zamkniete_Duplikat       = "0";
                        String ZCC_Konsultacja              = "0";
                        String ZCC_Konsultacja_biz          = "0";
                        String ZCC_Konsultacja_dev          = "0";
                        String KKK_Nowe = "0";
                        String KKK_POtwarte = "0";
                        String KKK_Zamkniete = "0";
                        String KKK_Zamkniete_Samodzielnie = "0";
                        String KKK_Zamkniete_IIILinia = "0";
                        String KKK_Zamkniete_IILinia = "0";
                        String KKK_Zamkniete_Odrzucone = "0";
                        String KKK_Zamkniete_Duplikat = "0";
                        String KKK_Konsultacja = "0";
                        String KKK_Konsultacja_biz = "0";
                        String KKK_Konsultacja_dev = "0";

                        try
                        {
                            List<List<string>> resultss = gujaczWFS.ExecuteStoredProcedure("spRaportDostepnosciSrodowisk_v3", new string[] { dtFrom.ToString(), dtTo.ToString() }, DatabaseName.SupportCP);
                            foreach (var st in resultss)
                            {
                                if (st[0] == "All")
                                {
                                    All = st[1];
                                }
                                else if (st[0] == "Nowe")
                                {
                                    Nowe = st[1];
                                }
                                else if (st[0] == "PonownieOtwarte")
                                {
                                    PonownieOtwarte = st[1];
                                }
                                else if (st[0] == "CRM_Nowe")
                                {
                                    CRM_Nowe = st[1];
                                }
                                else if (st[0] == "CRM_POtwarte")
                                {
                                    CRM_POtwarte = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete")
                                {
                                    CRM_Zamkniete = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete_Samodzielnie")
                                {
                                    CRM_Zamkniete_Samodzielnie = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete_IIILinia")
                                {
                                    CRM_Zamkniete_IIILinia = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete_IILinia")
                                {
                                    CRM_Zamkniete_IILinia = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete_Odrzucone")
                                {
                                    CRM_Zamkniete_Odrzucone = st[1];
                                }
                                else if (st[0] == "CRM_Zamkniete_Duplikat")
                                {
                                    CRM_Zamkniete_Duplikat = st[1];
                                }
                                else if (st[0] == "CRM_Konsultacja")
                                {
                                    CRM_Konsultacja = st[1];
                                }
                                else if (st[0] == "CRM_Konsultacja_biz")
                                {
                                    CRM_Konsultacja_biz = st[1];
                                }
                                else if (st[0] == "CRM_Konsultacja_dev")
                                {
                                    CRM_Konsultacja_dev = st[1];
                                }
                                else if (st[0] == "WIND_Nowe")
                                {
                                    WIND_Nowe = st[1];
                                }
                                else if (st[0] == "WIND_POtwarte")
                                {
                                    WIND_POtwarte = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete")
                                {
                                    WIND_Zamkniete = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete_Samodzielnie")
                                {
                                    WIND_Zamkniete_Samodzielnie = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete_IIILinia")
                                {
                                    WIND_Zamkniete_IIILinia = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete_IILinia")
                                {
                                    WIND_Zamkniete_IILinia = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete_Odrzucone")
                                {
                                    WIND_Zamkniete_Odrzucone = st[1];
                                }
                                else if (st[0] == "WIND_Zamkniete_Duplikat")
                                {
                                    WIND_Zamkniete_Duplikat = st[1];
                                }
                                else if (st[0] == "WIND_Konsultacja")
                                {
                                    WIND_Konsultacja = st[1];
                                }
                                else if (st[0] == "WIND_Konsultacja_biz")
                                {
                                    WIND_Konsultacja_biz = st[1];
                                }
                                else if (st[0] == "WIND_Konsultacja_dev")
                                {
                                    WIND_Konsultacja_dev = st[1];
                                }
                                else if (st[0] == "ZCC_Nowe")
                                {
                                    ZCC_Nowe = st[1];
                                }
                                else if (st[0] == "ZCC_POtwarte")
                                {
                                    ZCC_POtwarte = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete")
                                {
                                    ZCC_Zamkniete = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete_Samodzielnie")
                                {
                                    ZCC_Zamkniete_Samodzielnie = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete_IIILinia")
                                {
                                    ZCC_Zamkniete_IIILinia = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete_IILinia")
                                {
                                    ZCC_Zamkniete_IILinia = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete_Odrzucone")
                                {
                                    ZCC_Zamkniete_Odrzucone = st[1];
                                }
                                else if (st[0] == "ZCC_Zamkniete_Duplikat")
                                {
                                    ZCC_Zamkniete_Duplikat = st[1];
                                }
                                else if (st[0] == "ZCC_Konsultacja")
                                {
                                    ZCC_Konsultacja = st[1];
                                }
                                else if (st[0] == "ZCC_Konsultacja_biz")
                                {
                                    ZCC_Konsultacja_biz = st[1];
                                }
                                else if (st[0] == "ZCC_Konsultacja_dev")
                                {
                                    ZCC_Konsultacja_dev = st[1];
                                }
                                else if (st[0] == "KKK_Nowe")
                                {
                                    KKK_Nowe = st[1];
                                }
                                else if (st[0] == "KKK_POtwarte")
                                {
                                    KKK_POtwarte = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete")
                                {
                                    KKK_Zamkniete = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete_Samodzielnie")
                                {
                                    KKK_Zamkniete_Samodzielnie = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete_IIILinia")
                                {
                                    KKK_Zamkniete_IIILinia = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete_IILinia")
                                {
                                    KKK_Zamkniete_IILinia = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete_Odrzucone")
                                {
                                    KKK_Zamkniete_Odrzucone = st[1];
                                }
                                else if (st[0] == "KKK_Zamkniete_Duplikat")
                                {
                                    KKK_Zamkniete_Duplikat = st[1];
                                }
                                else if (st[0] == "KKK_Konsultacja")
                                {
                                    KKK_Konsultacja = st[1];
                                }
                                else if (st[0] == "KKK_Konsultacja_biz")
                                {
                                    KKK_Konsultacja_biz = st[1];
                                }
                                else if (st[0] == "KKK_Konsultacja_dev")
                                {
                                    KKK_Konsultacja_dev = st[1];
                                }

                            }
                        }
                        catch (Exception ep)
                        {
                            NoticeForm.ShowNotice(ep.Message);
                        }

                        string header = "Witam, " +
                                enter + enter + "W dniach " + formatDate(yesterday) + " " + dtFrom.ToShortTimeString() + " - " + formatDate(now) + " " + dtTo.ToShortTimeString();
                        if (cb_Rap18Awaria.Checked)
                        {
                             header += " wystąpiły następujące problemy z dostępnością wspieranych środowisk: ";
                        }
                        else
                        {
                            header += " nie wystąpiły żadne problemy z dostępnością wspieranych środowisk. ";
                        }

                        crm_rtb.Text = 
                            header +
                            enter+
                            enter+"Dane o zgłoszeniach od dnia " + dtFrom.ToShortTimeString() + " " + formatDate(yesterday) + " do dnia " + dtTo.ToShortTimeString() + " " + formatDate(now) +
                            enter+"Liczba obsłużonych zgłoszeń Jira: " + All+
                            enter+tab+"Liczba nowych zgłoszeń Jira: " + Nowe+
                            enter+tab+"Liczba ponownie otwartych zgłoszeń: " + PonownieOtwarte+
                            enter+
                            enter+"OBSZAR CRM/CTI:"+
                            enter +
                            enter+"Liczba nowych zgłoszeń Jira: " + CRM_Nowe +
                            enter+"Liczba ponownie otwartych zgłoszeń: " + CRM_POtwarte +
                            enter+"Liczba zgłoszeń zamkniętych: " + CRM_Zamkniete +
                            enter+tab+"Liczba zgłoszeń rozwiązanych samodzielnie: " + CRM_Zamkniete_Samodzielnie +
                            enter+tab+"Liczba zgłoszeń przekazanych do III linii: " + CRM_Zamkniete_IIILinia +
                            enter+tab+"Liczba zgłoszeń przekazanych do innej II linii: " + CRM_Zamkniete_IILinia +
                            enter+tab+"Liczba zgłoszeń odrzuconych: " + CRM_Zamkniete_Odrzucone +
                            enter+tab+"Liczba zgłoszeń zamkniętych jako duplikat: " + CRM_Zamkniete_Duplikat +
                            enter+"Liczba zgłoszeń w konsultacji: " + CRM_Konsultacja +
                            enter+tab+"Liczba zgłoszeń w konsultacji biznesowej: " + CRM_Konsultacja_biz +
                            enter+tab+"Liczba zgłoszeń w konsultacji deweloperskiej: " + CRM_Konsultacja_dev +
                            enter +
                            enter + "OBSZAR CRM Windykacja:" +
                            enter + 
                            enter + "Liczba nowych zgłoszeń Jira: " + WIND_Nowe +
                            enter+"Liczba ponownie otwartych zgłoszeń: " + WIND_POtwarte +
                            enter+"Liczba zgłoszeń zamkniętych: " + WIND_Zamkniete +
                            enter+tab+"Liczba zgłoszeń rozwiązanych samodzielnie: " + WIND_Zamkniete_Samodzielnie +
                            enter+tab+"Liczba zgłoszeń przekazanych do III linii: " + WIND_Zamkniete_IIILinia +
                            enter+tab+"Liczba zgłoszeń przekazanych do innej II linii: " + WIND_Zamkniete_IILinia +
                            enter+tab+"Liczba zgłoszeń odrzuconych: " + WIND_Zamkniete_Odrzucone +
                            enter+tab+"Liczba zgłoszeń zamkniętych jako duplikat: " + WIND_Zamkniete_Duplikat +
                            enter+"Liczba zgłoszeń w konsultacji: " + WIND_Konsultacja +
                            enter+tab+"Liczba zgłoszeń w konsultacji biznesowej: " + WIND_Konsultacja_biz +
                            enter+tab+"Liczba zgłoszeń w konsultacji deweloperskiej: " + WIND_Konsultacja_dev+
                            enter + 
                            enter + "OBSZAR Zamówienia CC:" +
                            enter +
                            enter + "Liczba nowych zgłoszeń Jira: " + ZCC_Nowe +
                            enter+"Liczba ponownie otwartych zgłoszeń: " + ZCC_POtwarte +
                            enter+"Liczba zgłoszeń zamkniętych: " + ZCC_Zamkniete +
                            enter+tab+"Liczba zgłoszeń rozwiązanych samodzielnie: " + ZCC_Zamkniete_Samodzielnie +
                            enter+tab+"Liczba zgłoszeń przekazanych do III linii: " + ZCC_Zamkniete_IIILinia +
                            enter+tab+"Liczba zgłoszeń przekazanych do innej II linii: " + ZCC_Zamkniete_IILinia +
                            enter+tab+"Liczba zgłoszeń odrzuconych: " + ZCC_Zamkniete_Odrzucone +
                            enter+tab+"Liczba zgłoszeń zamkniętych jako duplikat: " + ZCC_Zamkniete_Duplikat +
                            enter+"Liczba zgłoszeń w konsultacji: " + ZCC_Konsultacja +
                            enter+tab+"Liczba zgłoszeń w konsultacji biznesowej: " + ZCC_Konsultacja_biz +
                            enter+tab+"Liczba zgłoszeń w konsultacji deweloperskiej: " + ZCC_Konsultacja_dev +

                            enter +
                            enter + "OBSZAR KKK:" +
                            enter +
                            enter + "Liczba nowych zgłoszeń Jira: " + KKK_Nowe +
                            enter + "Liczba ponownie otwartych zgłoszeń: " + KKK_POtwarte +
                            enter + "Liczba zgłoszeń zamkniętych: " + KKK_Zamkniete +
                            enter + tab + "Liczba zgłoszeń rozwiązanych samodzielnie: " + KKK_Zamkniete_Samodzielnie +
                            enter + tab + "Liczba zgłoszeń przekazanych do III linii: " + KKK_Zamkniete_IIILinia +
                            enter + tab + "Liczba zgłoszeń przekazanych do innej II linii: " + KKK_Zamkniete_IILinia +
                            enter + tab + "Liczba zgłoszeń odrzuconych: " + KKK_Zamkniete_Odrzucone +
                            enter + tab + "Liczba zgłoszeń zamkniętych jako duplikat: " + KKK_Zamkniete_Duplikat +
                            enter + "Liczba zgłoszeń w konsultacji: " + KKK_Konsultacja +
                            enter + tab + "Liczba zgłoszeń w konsultacji biznesowej: " + KKK_Konsultacja_biz +
                            enter + tab + "Liczba zgłoszeń w konsultacji deweloperskiej: " + KKK_Konsultacja_dev;

                        crm_rtb.DeselectAll();
                        crm_rtb.Select(0, crm_rtb.Find("Liczba obsłużonych zgłoszeń Jira") - 1);
                        crm_rtb.SelectionFont = new Font(crm_rtb.Font.FontFamily,crm_rtb.Font.Size, FontStyle.Bold);
                        crm_rtb.Select(crm_rtb.Find("OBSZAR CRM/CTI:"),15);
                        crm_rtb.SelectionFont = new Font(crm_rtb.Font.FontFamily,crm_rtb.Font.Size, FontStyle.Bold);
                        crm_rtb.Select(crm_rtb.Find("OBSZAR CRM Windykacja:"), 22);
                        crm_rtb.SelectionFont = new Font(crm_rtb.Font.FontFamily, crm_rtb.Font.Size, FontStyle.Bold);
                        crm_rtb.Select(crm_rtb.Find("OBSZAR Zamówienia CC:"), 21);
                        crm_rtb.SelectionFont = new Font(crm_rtb.Font.FontFamily, crm_rtb.Font.Size, FontStyle.Bold);
                        crm_rtb.Select(crm_rtb.Find("OBSZAR KKK:"), 12);
                        crm_rtb.SelectionFont = new Font(crm_rtb.Font.FontFamily, crm_rtb.Font.Size, FontStyle.Bold);
                        crm_rtb.DeselectAll();
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogError(ex, Logger.Instance, true);
                    }
                    setBusy(false);
                }*/

        const string RTFSpecialsInUTF = @"(\P{IsBasicLatin})";

        private static Regex UTFSpecialRegex = new Regex(RTFSpecialsInUTF, RegexOptions.Compiled);

        private static string ReplaceDirect(Match match)
        {
            int codepoint = (int)Convert.ToChar(match.Groups[1].Value);
            if (!(codepoint < 32768))
            {
                codepoint = codepoint - 65536;
            }
            return string.Format("\\u{0}?", codepoint);
        }

        /// <summary>
        /// Przycisk Podsumowanie 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CRMPodsumowanie_Click(object sender, EventArgs e)
        {
            //List<List<string>> lista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport", new string[] { }, DatabaseName.SupportCP);

            crm_rtb.Text = "Czekaj...";
            setBusy(true);
            try
            {
                crm_rtb.Clear();

                DateTime now = dtp_CRMRapDataDo.Value;
                DateTime yesterday = dtp_CRMRapDataOd.Value;

                string dateFrom = yesterday.ToShortDateString();
                string dateTo = now.ToShortDateString();

                string timeFrom = tb_CRMRapGodzinaOd.Text;
                string timeTo = tb_CRMRapGodzinaDo.Text;

                DateTime dtFrom;
                DateTime dtTo;


                string tab = "\t";
                string enter = "\n";

                //for (int i = 0; i < lista.Count; i++)
                //{
                //    crm_rtb.AppendText(lista[i][1].ToString());
                //    crm_rtb.AppendText(enter);
                //}

                List<List<string>> lista = gujaczWFS.ExecuteStoredProcedure("spRaportDziennyPodsumowanie", new string[] { dateFrom }, DatabaseName.SupportCP);
                List<List<string>> listaKonsultacja = gujaczWFS.ExecuteStoredProcedure("spRaportDziennyKonsultacja", new string[] { }, DatabaseName.SupportCP);
                List<List<string>> listaZagrozoneSLA = gujaczWFS.ExecuteStoredProcedure("spRaportDziennyZagrozoneSLA", new string[] { }, DatabaseName.SupportCP);
                List<List<string>> listaNotatki = gujaczWFS.ExecuteStoredProcedure("spRaportDziennyNotatka", new string[] { }, DatabaseName.SupportCP);

                lista.RemoveAll(x => x[0] == string.Empty);
                listaKonsultacja.RemoveAll(x => x[0] == string.Empty);
                listaZagrozoneSLA.RemoveAll(x => x[0] == string.Empty);
                listaNotatki.RemoveAll(x => x[0] == string.Empty);



                StringBuilder tableRtf = new StringBuilder();

                tableRtf.Append(@"{\rtf1\ansi\deff4\adeflang1025
{\fonttbl{\f0\froman\fprq2\fcharset0 Times New Roman;}{\f1\froman\fprq2\fcharset2 Symbol;}{\f2\fswiss\fprq2\fcharset0 Arial;}{\f3\froman\fprq2\fcharset0 Liberation Serif{\*\falt Times New Roman};}{\f4\froman\fprq2\fcharset0 Calibri;}{\f5\fswiss\fprq2\fcharset0 Liberation Sans{\*\falt Arial};}{\f6\froman\fprq2\fcharset0 Roboto Black;}{\f7\froman\fprq2\fcharset0 Roboto;}{\f8\fnil\fprq2\fcharset0 AR PL SungtiL GB;}{\f9\fnil\fprq2\fcharset0 Roboto Black;}{\f10\fnil\fprq2\fcharset0 Roboto;}{\f11\fnil\fprq2\fcharset0 FreeSans;}{\f12\fswiss\fprq0\fcharset128 FreeSans;}}
{\colortbl;\red0\green0\blue0;\red128\green128\blue128;\red211\green211\blue211;}
{\stylesheet{\s0\snext0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045 Normal;}
{\*\cs15\snext15 Default Paragraph Font;}
{\s16\sbasedon0\snext17\ql\widctlpar\sb240\sa120\keepn\ltrpar\dbch\af8\dbch\af11\afs28\loch\f5\fs28 Heading;}
{\s17\sbasedon0\snext17\sl288\slmult1\ql\widctlpar\sb0\sa140\ltrpar Text Body;}
{\s18\sbasedon17\snext18\sl288\slmult1\ql\widctlpar\sb0\sa140\ltrpar\dbch\af12 List;}
{\s19\sbasedon0\snext19\ql\widctlpar\sb120\sa120\noline\ltrpar\i\dbch\af12\afs24\ai\fs24 Caption;}
{\s20\sbasedon0\snext20\ql\widctlpar\noline\ltrpar\dbch\af12 Index;}
}{\*\generator LibreOffice/4.4.7.2$Linux_X86_64 LibreOffice_project/f3153a8b245191196a4b6b9abd1d0da16eead600}{\info{\author prekawek}{\creatim\yr2016\mo3\dy16\hr10\min51}{\author prekawek}{\revtim\yr2016\mo3\dy16\hr12\min50}{\printim\yr0\mo0\dy0\hr0\min0}}\deftab708
\viewscale100
{\*\pgdsctbl
{\pgdsc0\pgdscuse451\pgwsxn13483\pghsxn16838\marglsxn1133\margrsxn1133\margtsxn1133\margbsxn1133\pgdscnxt0 Default Style;}}
\formshade{\*\pgdscno0}\paperh16838\paperw13483\margl1133\margr1133\margt1133\margb1133\sectd\sbknone\sectunlocked1\pgndec\pgwsxn13483\pghsxn16838\marglsxn1133\margrsxn1133\margtsxn1133\margbsxn1133\ftnbj\ftnstart1\ftnrstcont\ftnnar\aenddoc\aftnrstcont\aftnstart1\aftnnrlc
{\*\ftnsep}\pgndec\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs28\loch\f6
RAPORT ZA DZIE\u323\'3f:");

                tableRtf.Append(@" " + dateTo);

                foreach (List<string> ogolne in lista)
                {
                    tableRtf.Append(@"\par \pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                    tableRtf.Append(UTFSpecialRegex.Replace(ogolne[1].Replace("\n", @" \par "), new MatchEvaluator(ReplaceDirect)) + @"}");

                }


                if (listaZagrozoneSLA.Count() > 0)
                {
                    tableRtf.Append(@"\par \pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Lista zg\u322\'3fosze\u324\'3f z zagro\u380\'3fonym czasem SLA:}");


                    tableRtf.Append(@"\par \trowd\trql\trleft0\ltrrow\trrh379\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx2303\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx4230\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx6192\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx8256\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Numer zg\u322\'3foszenia}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Typ Zg\u322\'3foszenia}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Priorytet}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Pozosta\u322\'3fy czas}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Zg\u322\'3foszenie w kroku}\cell\row\pard\trowd\trql\trleft0\ltrrow\trrh439\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx2303\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx4230\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx6192\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx8256\clbrdrt\brdrs\brdrw17\brdrcf4\clbrdrl\brdrs\brdrw17\brdrcf4\clbrdrb\brdrs\brdrw17\brdrcf4\clbrdrr\brdrs\brdrw17\brdrcf4\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0");

                    for (int i = 0; i < listaZagrozoneSLA.Count(); i++)
                    {
                        tableRtf.Append(@"{" + listaZagrozoneSLA[i][0] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        tableRtf.Append(@"{" + listaZagrozoneSLA[i][1] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        tableRtf.Append(@"{" + listaZagrozoneSLA[i][2] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        tableRtf.Append(@"{" + listaZagrozoneSLA[i][3] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        if (listaKonsultacja.Count() - 1 != i)
                        {
                            tableRtf.Append(@"{" + UTFSpecialRegex.Replace(listaZagrozoneSLA[i][4].Replace("\n", @" \par "), new MatchEvaluator(ReplaceDirect)) + @"}\cell\row\pard\trowd\trql\trleft0\ltrrow\trrh439\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx2303\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx4230\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx6192\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx8256\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        }
                        else
                        {
                            tableRtf.Append(@"{" + UTFSpecialRegex.Replace(listaZagrozoneSLA[i][4].Replace("\n", @" \par "), new MatchEvaluator(ReplaceDirect)) + @"}\cell\row\pard\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\rtlch \ltrch\loch ");
                        }
                    }

                }

                if (listaKonsultacja.Count() > 0)
                {
                    tableRtf.Append(@"\par\pard\plain\s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\cf1\b\dbch\af9\rtlch\ltrch\loch\fs20\loch\f6
Lista zg\u322\'3fosze\u324\'3f w konsultacji powy\u380\'3fej 14 dni:}
\par \trowd\trql\trleft0\ltrrow\trrh387\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx3739\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx7478\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Numer zg\u322\'3foszenia}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Konsultacja Dni}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\b\dbch\af9\rtlch \ltrch\loch\fs20\loch\f6
Osoba konsultuj\u261\'3fca}\cell\row\pard\trowd\trql\trleft0\ltrrow\trrh447\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx3739\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx7478\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7
");

                    for (int i = 0; i < listaKonsultacja.Count(); i++)
                    {
                        tableRtf.Append(@"{" + listaKonsultacja[i][0] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        tableRtf.Append(@"{" + listaKonsultacja[i][1] + @"}\cell\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");

                        if (listaKonsultacja.Count() - 1 != i)
                        {
                            tableRtf.Append(@"{" + listaKonsultacja[i][2] + @"}\cell\row\pard\trowd\trql\trleft0\ltrrow\trrh447\trpaddft3\trpaddt0\trpaddfl3\trpaddl0\trpaddfb3\trpaddb0\trpaddfr3\trpaddr0\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx3739\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx7478\clbrdrt\brdrs\brdrw17\brdrcf3\clbrdrl\brdrs\brdrw17\brdrcf3\clbrdrb\brdrs\brdrw17\brdrcf3\clbrdrr\brdrs\brdrw17\brdrcf3\clvertalc\cellx11217\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\intbl\sl240\slmult1\qc\sb0\sa0{\cf1\dbch\af10\rtlch \ltrch\loch\fs20\loch\f7 ");
                        }
                        else
                        {
                            tableRtf.Append(@"{" + listaKonsultacja[i][2] + @"}\cell\row\pard\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af13\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045\rtlch \ltrch\loch ");
                        }

                    }

                }

                if (listaNotatki.Count() > 0)
                {

                    tableRtf.Append(@"\par\pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af9\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\b\rtlch \ltrch\loch 
Szczeg\u243\'f3\u322\'3fy do zg\u322\'3fosze\u324\'3f w realizacji:}");

                    tableRtf.Append(@" \par ");
                    foreach (List<string> notatki in listaNotatki)
                    {
                        tableRtf.Append(@" \pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af9\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\b\rtlch \ltrch\loch ");
                        tableRtf.Append(notatki[1]);
                        tableRtf.Append(@": ");
                        tableRtf.Append(@"\par \pard\plain \s0\ql\widctlpar\ltrpar\sl259\slmult1{\*\hyphen2\hyphlead2\hyphtrail2\hyphmax0}\sb0\sa160\cf0\kerning1\dbch\af9\langfe1033\dbch\af14\afs22\alang1025\loch\f4\fs22\lang1045{\rtlch \ltrch\loch ");
                        tableRtf.Append(UTFSpecialRegex.Replace(notatki[2].Replace("\n", @" \par "), new MatchEvaluator(ReplaceDirect)));
                        tableRtf.Append(@"\par***************************************** \par");
                    }

                }


                crm_rtb.Rtf = tableRtf.ToString();

            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
            setBusy(false);
        }
        #endregion

        #region SŁOWNIKI
        // Zakładka Słowniki

        /// <summary>
        /// Przycisk Odśwież 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SlownikiOdswiez_Click(object sender, EventArgs e)
        {
            btn_SlownikiZapisz.Enabled = false;
            List<List<string>> results = gujaczWFS.ExecuteStoredProcedure("BillingDTH_Slownik", null, DatabaseName.SupportADDONS);

            if (results.Count == 0)
            {
                NoticeForm.ShowNotice("Błąd");
            }

            compDgv.Visible = true;

            compDgv.Columns.Clear();
            compDgv.Columns.Add("Katalog", "Katalog");
            compDgv.Columns.Add("RodzajBledu", "Rodzaj błędu");
            compDgv.Columns.Add("Typ", "Typ");
            compDgv.Columns.Add("SekcjaCP", "SekcjaCP");
            compDgv.Columns.Add("OsobaCP", "OsobaCP");
            compDgv.Columns.Add("OsobaBL", "OsobaBL");
            compDgv.Columns.Add("Scenariusz", "Scenariusz");

            foreach (List<string> roww in results)
            {
                compDgv.Rows.Insert(0, new DataGridViewRow());
                compDgv.Rows[0].Cells[0].Value = roww[0];
                compDgv.Rows[0].Cells[1].Value = roww[1];
                compDgv.Rows[0].Cells[2].Value = roww[2];
                compDgv.Rows[0].Cells[3].Value = roww[6];
                compDgv.Rows[0].Cells[4].Value = roww[7];
                compDgv.Rows[0].Cells[5].Value = roww[8];
                compDgv.Rows[0].Cells[6].Value = roww[9];
            }

            btn_SlownikiZapisz.Enabled = true;
        }

        /// <summary>
        /// Przycisk zapisz 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SlownikiZapisz_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // creating Excel Application 
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                // creating new WorkBook within Excel application 
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                // creating new Excelsheet in workbook 
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

                // see the excel sheet behind the program 
                app.Visible = true;

                // get the reference of first sheet. By default its name is Sheet1. 

                // store its reference to worksheet 
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                //worksheet = workbook.ActiveSheet;

                // changing the name of active sheet 
                worksheet.Name = "Exported from gridview";

                // storing header part in Excel 
                for (int i = 1; i < compDgv.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = compDgv.Columns[i - 1].HeaderText;
                }

                // storing Each row and column value to excel sheet 
                for (int i = 0; i < compDgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < compDgv.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = compDgv.Rows[i].Cells[j].Value.ToString();
                    }
                }

                // save the application 
                workbook.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // Exit from the application 
                workbook.Close();
                app.Quit();
            }
        }
        #endregion

        #region POWIADOMIENIE
        // Zakładka Powiadomienie

        /// <summary>
        /// Włącz/Wyłącz powiadomienie 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PowiadomienieWlacz_Click(object sender, EventArgs e)
        {
            if (notifyTimeoutTextBox.Text != "")
            {
                if (Int32.Parse(notifyTimeoutTextBox.Text) < 300)
                {
                    NoticeForm.ShowNotice("Minimalna wartość to 300 sekund! (5 Min)");
                    return;
                }

                try
                {
                    int seconds = Int32.Parse(notifyTimeoutTextBox.Text);
                    Properties.Settings.Default.IssuesCheckTimeout = seconds;
                    Properties.Settings.Default.Save();

                    if (pauza)
                    {
                        timer2.Interval = seconds * 1000;
                        timer2.Start();
                        messageToWebService.Start();
                        btn_PowiadomienieWlacz.Text = "Wyłącz";
                        pauza = false;
                        checkIssues = true;
                    }
                    else
                    {
                        timer2.Stop();
                        btn_PowiadomienieWlacz.Text = "Włącz";
                        pauza = true;
                        checkIssues = false;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                NoticeForm.ShowNotice("Nie uzupełniono wszystkich wymaganych pól!");
                return;
            }
        }


        private void SendEmail(IEnumerable<Issue> filterIssues, TreeView treeView)
        {
            try
            {
                List<string> myIssues = new List<string>();
                for (int i = 0; i < treeView.Nodes.Count; i++)
                {
                    myIssues.Add(treeView.Nodes[i].Text.Substring(0, treeView.Nodes[i].Text.IndexOf(' ')));
                }

                List<string> newIssues = new List<string>();
                foreach (var item in filterIssues)
                {
                    if (!myIssues.Contains(item.Key.Value))
                        newIssues.Add(item.Key.Value);
                }

                HtmlContent content = new HtmlContent();

                content.Header = new HtmlHeader("Masz nowe zgłoszenia:");
                HtmlTable table = new HtmlTable();
                table.AddColumn("Zgłoszenia");

                foreach (string s in newIssues)
                {
                    string link = (new HtmlHyperlink("http://jira/browse/" + s)).ToString();
                    table.AddRow(new string[] { link });
                }

                content.Table = table;
                Logic.Implementation.ExchangeClient exClient = new Logic.Implementation.ExchangeClient(gujaczWFS.getUser());
                exClient.SendEMail("Nowe zgłoszenia!", content);
            }
            catch (Exception ex)
            {
                NoticeForm.ShowNotice(ex.Message);
            }
        }

        private void SendEmail(RichTextBox messageOut)
        {
            try
            {

                
                Logic.Implementation.ExchangeClient exClient = new Logic.Implementation.ExchangeClient(gujaczWFS.getUser());
                exClient.SendEMail("Raport Test", "prekawek@billennium.com", messageOut);
            }
            catch (Exception ex)
            {
                NoticeForm.ShowNotice(ex.Message);
            }
        }

        /// <summary>
        /// Obsługa timera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (isIssueCheckoutRunning)
                    return;
                if (isIssuesRefreshRunning)
                    return;

                Thread thr = new Thread((ThreadStart)delegate
                {
                    try
                    {
                        //this.Invoke((MethodInvoker)delegate()
                        //{
                        //    this.issuesCheckoutStatus.Text = "Sprawdzanie: running";
                        //});
                        //this.issuesCheckoutStatus.Text = "Sprawdzanie: running";
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            this.issuesCheckoutStatus.Text = "Sprawdzanie: running";
                        });
                        Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");

                        if (jIssues.GetJiraIssuesTypes() == null)
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                this.issuesCheckoutStatus.Text = "Sprawdzanie: Brak połączenia JIRA";
                            });
                            return;
                        }

                        string assignedFilterName = Properties.Settings.Default.assignedFilterName;
                        if (string.IsNullOrEmpty(assignedFilterName))
                            assignedFilterName = "przydzielone";

                        string unassignedFilterName = Properties.Settings.Default.unassignedFilterName;
                        if (string.IsNullOrEmpty(unassignedFilterName))
                            unassignedFilterName = "nieprzydzielone";

                        var issuesNieprzydzielone = jIssues.GetIssuesFromFilterAsync(unassignedFilterName);
                        var issuesPrzydzielone = jIssues.GetIssuesFromFilterAsync(assignedFilterName);

                        StringBuilder balloonMessage = new StringBuilder();


                        List<Issue> allIssues = new List<Issue>();
                        allIssues.AddRange(issuesNieprzydzielone.ToList());
                        //issuesPrzydzielone.ToList()); stara Wersja

                        StringBuilder xml = XmlParser.IssuesToSlaRaportXML(allIssues);

                        List<List<string>> slaResult = gujaczWFS.ExecuteStoredProcedure("cp_UnassignedIssueJira_SLA", new string[] { xml.ToString() }, DatabaseName.SupportCP);

                        if (slaResult.Count > 0)
                        {
                            balloonMessage.AppendLine("Do przyjęcia w ciągu:");

                            foreach (var item in slaResult)
                            {
                                balloonMessage.AppendLine(string.Format(" - {0}: {1}min", item[0], item[1]));
                            }

                        }

                        //Obsługa Przydzielonych
                        if (issuesPrzydzielone.Count() != Int32.Parse(przydzielone))
                        {
                            if (issuesPrzydzielone.Count() > Int32.Parse(przydzielone) && this.emailNotification)
                            {
                                // powiadomienie e-mail
                                SendEmail(issuesPrzydzielone, treeView2);

                            }

                            balloonMessage.AppendLine("Masz nowe zgłoszenia przydzielone.");
                        }
                        if (!jiraUser.Login.Equals("Billennium", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (issuesNieprzydzielone.Count() != Int32.Parse(nieprzydzielone))
                            {
                                if (issuesNieprzydzielone.Count() > Int32.Parse(nieprzydzielone) && this.emailNotification)
                                {
                                    // powiadomienie e-mail
                                    SendEmail(issuesNieprzydzielone, treeView1);
                                }

                                balloonMessage.AppendLine("Masz nowe zgłoszenia nieprzydzielone.");
                            }
                        }


                        if (balloonMessage.Length > 0)
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                ni_NewIssueAlert.BalloonTipText = balloonMessage.ToString();
                                ShowBalloonTip(-1);
                                if (this.cb_SoundNotification.Checked)
                                {
                                    SoundPlayer sp = new SoundPlayer();
                                    sp.SoundLocation = Properties.Settings.Default.dzwiekSciezka;
                                    try
                                    {
                                        sp.Play();
                                    }
                                    catch (Exception ex) { }
                                }
                                FlashWindowEx(this);
                            });
                        }

                        System.Diagnostics.Debug.WriteLine("Zakończenie sprawdzania: " + DateTime.Now.ToShortTimeString());

                        this.Invoke((MethodInvoker)delegate ()
                        {
                            this.issuesCheckoutStatus.Text = "Sprawdzanie: finish";
                        });

                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            this.issuesCheckoutStatus.Text = "Sprawdzanie: ERROR " + ex.Message;
                            ExceptionManager.LogError(ex, Logger.Instance);
                        });
                    }

                    this.isIssueCheckoutRunning = false;
                });
                thr.IsBackground = true;

                this.isIssueCheckoutRunning = true;
                System.Diagnostics.Debug.WriteLine("Rozpoczęcie sprawdzania: " + DateTime.Now.ToShortTimeString());
                this.issuesCheckoutStatus.Text = "Sprawdzanie: start";
                thr.Start();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance);
            }

        }

        /// <summary>
        /// Pokaż chmurkę powiadomienia
        /// </summary>
        /// <param name="treeViewPage"></param>
        private void ShowBalloonTip(int treeViewPage)
        {
            ni_NewIssueAlert.BalloonTipIcon = ToolTipIcon.Warning;
            ni_NewIssueAlert.BalloonTipTitle = "Uwaga!";
            ni_NewIssueAlert.Tag = treeViewPage;
            alertShowUntil = DateTime.Now.AddSeconds(300000);
            ni_NewIssueAlert.ShowBalloonTip(300000);
            timer2.Stop();
            pauza = true;
        }

        /// <summary>
        /// Po zamknięciu chmurki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ni_NewIssueAlert_BalloonTipClosed(object sender, EventArgs e)
        {
            //if (alertShowUntil > DateTime.Now)
            //    ShowBalloonTip(Convert.ToInt32((sender as NotifyIcon).Tag));
        }

        /// <summary>
        /// Po kliknięciu chmurki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ni_NewIssueAlert_BalloonTipClicked(object sender, EventArgs e)
        {
            alertShowUntil = DateTime.MinValue;
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }

            int treeViewIndex = Convert.ToInt32(ni_NewIssueAlert.Tag);
            //MessageBox.Show(treeViewIndex.ToString());

            switch (treeViewIndex)
            {
                // przydzielone
                case 0:
                    {
                        issueTab.SelectedIndex = 0;
                        //if (this.WindowState == FormWindowState.Minimized)
                        //    this.WindowState = FormWindowState.Normal;

                        btn_NieprzydzieloneOdsiwez_Click(ni_NewIssueAlert, new EventArgs());

                        break;
                    }
                // nieprzydzielone
                case 1:
                    {
                        issueTab.SelectedIndex = 1;
                        //if (this.WindowState == FormWindowState.Minimized)
                        //    this.WindowState = FormWindowState.Normal;

                        btn_MojeOdswiez_Click(ni_NewIssueAlert, new EventArgs());
                        break;
                    }
            }
        }

        /// <summary>
        /// Po kliknięciu w checkbox włącz/wyłącz powiadomienia po starcie aplikacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                Properties.Settings.Default.czyPowiadomienie = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.czyPowiadomienie = false;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region HISTORIA
        // Zakładka Historia

        /// <summary>
        /// Przycisk szukaj 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_HistoriaSzukaj_Click(object sender, EventArgs e)
        {
            List<List<string>> results = gujaczWFS.ExecuteStoredProcedure("BillingDTH_IssueHistory", new string[] { textBox2.Text }, DatabaseName.SupportADDONS);

            if (results.Count == 0)
            {
                NoticeForm.ShowNotice("Brak akcji na zgłoszeniu.", "Info");
            }


            Historia_dgv.Visible = true;

            Historia_dgv.Columns.Clear();

            Historia_dgv.Columns.Add("Numer zgłoszenia", "Numer zgłoszenia");
            Historia_dgv.Columns.Add("Akcja", "Akcja");
            Historia_dgv.Columns.Add("Data akcji", "Data akcji");
            Historia_dgv.Columns.Add("Konsultant", "Konsultant");

            foreach (List<string> row in results)
            {
                Historia_dgv.Rows.Insert(0, new DataGridViewRow());
                Historia_dgv.Rows[0].Cells[0].Value = row[0];
                Historia_dgv.Rows[0].Cells[1].Value = row[1];
                Historia_dgv.Rows[0].Cells[2].Value = row[2];
                Historia_dgv.Rows[0].Cells[3].Value = row[3];
            }
        }

        private void btn_HistoriaSzukaj_Click2(object sender, EventArgs e)
        {
            List<List<string>> results = gujaczWFS.ExecuteStoredProcedure("BillingDTH_IssueHistory2", new string[] { textBox2.Text }, DatabaseName.SupportCP);

            if (results.Count == 0)
            {
                NoticeForm.ShowNotice("Brak akcji na zgłoszeniu.", "Info");
            }


            Historia_dgv.Visible = true;

            Historia_dgv.Columns.Clear();

            Historia_dgv.ReadOnly = true;

            Historia_dgv.Columns.Add("IssueId", "IssueId w BPM");
            Historia_dgv.Columns.Add("Numer zgłoszenia", "Numer zgłoszenia");
            Historia_dgv.Columns.Add("Akcja", "Akcja");
            Historia_dgv.Columns.Add("Data akcji", "Data akcji");
            Historia_dgv.Columns.Add("Konsultant", "Konsultant");

            foreach (List<string> row in results)
            {
                Historia_dgv.Rows.Insert(0, new DataGridViewRow());
                Historia_dgv.Rows[0].Cells[0].Value = row[0];
                Historia_dgv.Rows[0].Cells[1].Value = row[1];
                Historia_dgv.Rows[0].Cells[2].Value = row[2];
                Historia_dgv.Rows[0].Cells[3].Value = row[3];
                Historia_dgv.Rows[0].Cells[4].Value = row[4];
            }

            Historia_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            Historia_dgv.AutoSize = true;
        }

        private void Historia_dgvCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {

                    numerZgl.Text = Historia_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    System.Diagnostics.Process.Start("https://jira/browse/" + numerZgl.Text);
                }
                else if (e.ColumnIndex == 0)
                {

                    numerZgl.Text = Historia_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    System.Diagnostics.Process.Start("https://support.billennium.pl/Bpm/UserPanel/IssueDetails.aspx?issueId=" + numerZgl.Text);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                NoticeForm.ShowNotice(ex.Message);
            }
        }

        #endregion

        #region DIAGNOZA
        // Zakładka Diagnoza

        /// <summary>
        /// Przycisk Wczytaj 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DiagnozaWczytaj_Click(object sender, EventArgs e)
        {
            if (cb_diagnozaUsers.SelectedIndex >= 0)
            {
                if (dgv_Diagnoza.Rows.Count > 0)
                    dgv_Diagnoza.Rows.Clear();

                DateTime dateTo = DateTime.Now;
                DateTime dateFrom = dateTo.AddDays(-30);

                int userId = -1;
                foreach (KeyValuePair<int, HeliosUser> kvp in cpUsers)
                {
                    string userName = kvp.Value.ToString();
                    if (userName.Equals(cb_diagnozaUsers.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        userId = kvp.Key;
                }
                List<List<string>> lista = gujaczWFS.ExecuteStoredProcedure("BillingDTH_ZgloszeniaWDiagnozie", new string[] { userId.ToString() }, DatabaseName.SupportADDONS);

                if (lista.Count > 0)
                {
                    dgv_Diagnoza.Columns.Clear();
                    dgv_Diagnoza.Columns.Add("dgvColNumerZgloszeniaWFS", "Numer Zgłoszenia WFS");
                    dgv_Diagnoza.Columns.Add("dgvColNumerZgloszeniaJira", "Numer Zgłoszenia Jira");
                    dgv_Diagnoza.Columns.Add("dgvColStatusWFS", "Status");
                    dgv_Diagnoza.Columns.Add("dgvColKonsultant", "Konsultant");
                    dgv_Diagnoza.Columns.Add("dgvColData", "Data");

                    foreach (var row in lista)
                    {
                        string[] split = row[1].Split('-');
                        string projectName = split[0];
                        bool czyDodac = false;

                        int filterIndex = cb_DiagnozaFiltr.SelectedIndex;
                        switch (filterIndex)
                        {
                            case -1:
                                czyDodac = true;
                                break;
                            case 0:
                                czyDodac = true;
                                break;
                            case 1:
                                if (Properties.Settings.Default.CRM.Contains(projectName))
                                    czyDodac = true;
                                break;
                            case 2:
                                if (Properties.Settings.Default.Billing.Contains(projectName))
                                    czyDodac = true;
                                break;
                            case 3:
                                if (Properties.Settings.Default.Zamowienia.Contains(projectName))
                                    czyDodac = true;
                                break;
                            default:
                                throw new ArgumentException("Brak obsługi wybranego filtra!");
                        }

                        if (!ckb_pokazujDrzewko.Checked)
                        {

                            // DOROBIć sprawdzenie czy istnieją obiekty
                            if (treeView2.Nodes.Count > 0)
                            {
                                foreach (TreeNode v in treeView2.Nodes)
                                {
                                    string nr = v.Text.Split(' ').FirstOrDefault();
                                    if (nr == row[1])
                                        czyDodac = false;
                                }
                            }
                        }

                        if (czyDodac)
                        {
                            dgv_Diagnoza.Rows.Insert(0, new DataGridViewRow());
                            dgv_Diagnoza.Rows[0].Cells[0].Value = row[0];
                            dgv_Diagnoza.Rows[0].Cells[1].Value = row[1];
                            dgv_Diagnoza.Rows[0].Cells[2].Value = row[2];
                            dgv_Diagnoza.Rows[0].Cells[3].Value = row[3];
                            dgv_Diagnoza.Rows[0].Cells[4].Value = row[4];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Po kliknięciu w komórkę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Diagnoza_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                numerZgl.Text = dgv_Diagnoza.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Search(numerZgl.Text, true, treeView1);
                issueTab.SelectTab(0);
            }
        }
        #endregion

        #region ISSUE TAB
        // Obsług Issue Tab

        /// <summary>
        /// Uzupełnienie komponentu o odpowienie elementy
        /// </summary>
        private void AddToTree(TreeView tr, BillingIssueDtoHelios issue = null)
        {
            tr.Invoke((MethodInvoker)delegate
            {
                tr.Nodes.Clear();
                BillingIssueDtoHelios issueU = issue;

                AddToTreeHelios(tr, issue);
                // AddToTreeJira(tr);

                tr.EndUpdate();
            });
        }

        private int GetImageIndex(BillingIssueDtoHelios issue, IssueState state)
        {
            int index = (int)state;
            string priority = string.Empty;
            string type = string.Empty;


            switch (issue.issueWFS.Priorytet)
            {
                case "1": priority = "Blocker"; break;
                case "2": priority = "Critical"; break;
                case "3": priority = "Major"; break;
                case "4": priority = "Minor"; break;
                case "5": priority = "Trivial"; break;
            }

            //switch (issue.issueHelios.severity)
            //{
            //    case "1": priority = "Blocker"; break;
            //    case "2": priority = "Critical"; break;
            //    case "3": priority = "Major"; break;
            //    case "4": priority = "Minor"; break;
            //    case "5": priority = "Trivial"; break;
            //}

            if ((int)state == 0)
                type = "New";
            else if ((int)state == 1)
            {

                if (issue.issueHelios.rodzaj_zgloszenia == "Incydent")
                    type = "IN";
                else if (issue.issueHelios.rodzaj_zgloszenia == "Zlecenie operatorskie")
                    type = "ZO";
                else if (issue.issueHelios.rodzaj_zgloszenia == "Problem")
                    type = "PN";
                else if (issue.issueWFS.System != null)
                {
                    if (issue.issueWFS.System.Text == "Incydent")
                        type = "IN";
                    else if (issue.issueWFS.System.Text == "Zlecenie operatorskie")
                        type = "ZO";
                    else if (issue.issueWFS.System.Text == "Problem")
                        type = "PN";
                    else if (issue.issueWFS.System.Text == "Błąd produkcyjny" && issue.issueHelios.rodzaj_zgloszenia != "Błąd masowy - system produkcyjny")
                        type = "Prod";
                    else if (issue.issueWFS.System.Text == "Błąd produkcyjny" && issue.issueHelios.rodzaj_zgloszenia == "Błąd masowy - system produkcyjny")
                        type = "Mass";
                    else if (issue.issueWFS.System.Text == "Wewnętrzne")
                        type = "Wewnetrzne";
                }

            }

            if (priority != string.Empty && type != string.Empty)
            {
                string key = type + priority + ".png";
                int id = imageList1.Images.IndexOfKey(key);
                index = id;
            }
            else
            {
                index = imageList1.Images.IndexOfKey("wait.png");
            }

            return index;
        }
        /// <summary>
        /// Wypełnienie drzewa zgłoszeń
        /// </summary>
        /// <param name="tr"></param>
        private void AddToTreeHelios(TreeView tr, BillingIssueDtoHelios issue = null)
        {
            //dodanie węzłów testowych/ do usuniecia
            //tr.Nodes.Add(new TreeNode()
            //{
            //    Text = "Zgłoszenia Helios",
            //    ImageIndex = 1,
            //    ContextMenuStrip = cms_IssuePopup
            //});
            //ODKOMENTOWAC KIEDY BĘDZIE MOŻNA POBRAć ZGŁOSZENIA Z WFSa

            try
            {


                List<KeyValuePair<BillingIssueDto, IssueState>> lista;
                List<string> slaClosedIssue = new List<string>();
                List<List<string>> slaClosedIssues = new List<List<string>>();

                lista = issues[tr.Name].Where(x => x.Key.Type == Entities.Enums.IssueType.HELIOS).ToList();

                lista.Sort((x, y) =>
                {
                    return x.Key.Idnumber.CompareTo(y.Key.Idnumber);
                });

                foreach (KeyValuePair<BillingIssueDto, IssueState> item in lista)
                {
                    if (item.Key.issueWFS.WFSState == "Zgłoszenie zamknięte")
                    {
                        slaClosedIssue.Add(item.Key.issueWFS.WFSIssueId.ToString());
                    }
                }

                if (slaClosedIssue.Count > 0)
                {
                    StringBuilder xml = Logic.Implementation.XmlParser.IssuesSla(slaClosedIssue);

                    slaClosedIssues = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport", new string[] { xml.ToString() }, DatabaseName.SupportCP);
                }

                int tempIndex = 0;
                foreach (KeyValuePair<BillingIssueDto, IssueState> item in lista)
                {

                    tempIndex += 1;

                    BillingIssueDtoHelios it = item.Key as BillingIssueDtoHelios;
                    int index = this.GetImageIndex(item.Key as BillingIssueDtoHelios, item.Value);

                    string slaValue = null;
                    foreach (var slaItem in SLAlista)
                    {
                        if (item.Key.Idnumber == slaItem[1].ToString())
                        {
                            slaValue = string.Concat(" [", slaItem[8].ToString(), " min]");
                        }
                    }
                    foreach (var slaItem in slaClosedIssues)
                    {
                        if (item.Key.issueWFS.WFSIssueId.ToString() == slaItem[0].ToString())
                        {
                            slaValue = string.Concat(" [", slaItem[8].ToString(), " min]");
                        }
                    }


                    TreeNode node = new TreeNode()
                    {
                        ImageIndex = index, //(int)item.Value,
                        SelectedImageIndex = index,
                        Text = item.Key.Idnumber + ' ' + item.Key.issueWFS.WFSState + slaValue,
                        Name = "node_" + tempIndex,
                        ContextMenuStrip = cms_IssuePopup
                    };

                    if (item.Key.issueWFS.WFSState == "Zgłoszenie zamknięte")
                    {
                        node.BackColor = Color.OrangeRed;
                    }
                    else if (item.Key.issueWFS.WFSState == "Konsultacja zgłoszenia")
                    {
                        node.BackColor = Color.CadetBlue;
                    }
                    tr.Nodes.Add(node);




                    /* 
                    // Problemy zawieszenie
                    List<List<string>> child = gujaczWFS.ExecuteStoredProcedure("spGetProblemChild", new string[] { it.issueWFS.WFSIssueId.ToString() }, DatabaseName.SupportCP);

                    if (tr.Name == "treeView4" && child.Count > 0)
                    {

                        issues["treeView3"].Clear();

                        TreeNode parentNode = tr.Nodes["node_"+tempIndex] as TreeNode;
                            //SelectedNode ?? tr.Nodes[0];

                        if (parentNode != null)
                        {

                            List<BillingIssueDtoHelios> issueProblemChild = new List<BillingIssueDtoHelios>();
                            foreach (List<string> numerkiZg in child)
                            {
                                issueProblemChild.Add(new BillingIssueDtoHelios()
                                {
                                    issueHelios = new IssueHelios()
                                    {
                                        number = numerkiZg[2]
                                    }
                                });
                            }

                            wfsList[treeView3.Name] = gujaczWFS.compareBillingWithWFS(issueProblemChild);

                            foreach (BillingIssueDtoHelios itemProblem in wfsList[treeView3.Name])
                            {
                                BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(itemProblem);
                                IssueState state = (updatedIssue.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                                BillingIssueDtoHelios tmp = gujaczWFS.UpdateJiraInfo(updatedIssue);
                                issues[treeView3.Name].Add(tmp, state);
                            }

                            GetActionForIssues(treeView3);
                            AddToTree(treeView3);

                            //if (treeView3.Nodes.Count > 0)
                            //{
                            //    foreach (TreeNode nodeTW3 in treeView3.Nodes)
                            //    {
                            //        parentNode.Nodes.Add(nodeTW3);
                            //    }
                            //}
                            /**/
                    //parentNode.Nodes.Add("asdf");

                    /*foreach (var ch in child)
                    {
                        parentNode.Nodes.Add(ch[2].ToString());

                        // BillingIssueDtoHelios it = item.Key as BillingIssueDtoHelios;

                         index = this.GetImageIndex(item.Key as BillingIssueDtoHelios, item.Value);

                        TreeNode node2 = new TreeNode()
                        {
                            ImageIndex = index, //(int)item.Value,
                            SelectedImageIndex = index,
                            Text = item.Key.Idnumber + ' ' + item.Key.issueWFS.WFSState,
                            Name = "node_" + tempIndex,
                            ContextMenuStrip = cms_IssuePopup
                        };
                        tr.Nodes.Add(node2);
                    }
                    issues["treeView3"].Clear();
                }
            }*/

                    if (issue != null && item.Key.Idnumber.Equals(issue.Idnumber))
                        tr.SelectedNode = node;
                }
                tr.ExpandAll();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Zapis zgłoszeń do WFS
        /// </summary>
        /// <param name="tr"></param>
        private void ZapiszZgloszenia(TreeView tr, bool auto = false)
        {
            if (!issues.ContainsKey(tr.Name))
            {
                issues[tr.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }



            pb_SetVisibilityPanel(true);
            List<KeyValuePair<BillingIssueDto, IssueState>> tmp = (issues[tr.Name].Where(x => x.Value == IssueState.READYTOSAVE).ToList());
            if (tmp.Count != 0)
            {
                pb_SetMaxProgressBar(tmp.Count + 2);
                pb_UpdateProgressBar("Rozpoczynanie zapisu");
                DisableIssuesButtons();

                List<string> dubleZgloszen = new List<string>(tmp.Count());

                Thread t = new Thread((ThreadStart)delegate ()
                {
                    StringBuilder sb = new StringBuilder();

                    List<KeyValuePair<BillingIssueDto, IssueState>> toRemove = new List<KeyValuePair<BillingIssueDto, IssueState>>();
                    foreach (KeyValuePair<BillingIssueDto, IssueState> item in tmp)
                    {
                        try
                        {

                            Logger.Instance.LogInformation(string.Format("Automatyczny zapis nowego zgłoszenia {0} [{1}] \n\n", item.Key, DateTime.Now));

                            int wfsIssueId = 0;

                            //gujaczWFS.CreateNewIssue((BillingIssueDtoHelios)item.Key, out wfsIssueId);
                            gujaczWFS.addBillingIssueToWFS((BillingIssueDtoHelios)item.Key, out wfsIssueId); //Zapis zgłoszenia do WFS
                            item.Key.issueWFS.WFSIssueId = wfsIssueId;
                            //MessageBox.Show("wfsIssueId = " + wfsIssueId);
                            System.Diagnostics.Debug.WriteLine(string.Format("Zapisano zgłoszenie {0} - {1}", item.Key.ToString(), wfsIssueId.ToString()));
                            if (wfsIssueId == 0)
                            {
                                sb.AppendLine(item.Key.Idnumber + ": nie zapisano");
                                pb_UpdateProgressBar("Zablokowano zapis: " + item.Key.Idnumber);

                                toRemove.Add(item);
                            }
                            else
                            {

                                Logger.Instance.LogInformation(string.Format("Automatyczny zapis poprawny {0} [{1}] \n\n", item.Key, DateTime.Now));
                                sb.AppendLine(item.Key.Idnumber + ": Dodane poprawnie");
                                pb_UpdateProgressBar("Zapisano zgłoszenie: " + item.Key.Idnumber);
                                issues[tr.Name].Remove(item.Key);
                                issues[tr.Name].Add(item.Key, IssueState.INWFS);

                                //Dotyczy zgłoszeń z heliosa
                                int itemsCount = tr.Nodes.Count;

                                for (int i = 0; i < itemsCount; i++)
                                {
                                    TreeNode treeItem = tr.Nodes[i];
                                    if (treeItem.Text.Split(' ').First() == item.Key.Idnumber)
                                    {

                                        Logger.Instance.LogInformation(string.Format("Modyfikacja treeItem"));

                                        KeyValuePair<BillingIssueDto, IssueState> iss = issues[tr.Name].Where(x => x.Key.Idnumber == item.Key.Idnumber).FirstOrDefault();
                                        BillingIssueDtoHelios it = iss.Key as BillingIssueDtoHelios;

                                        int index = GetImageIndex(item.Key as BillingIssueDtoHelios, IssueState.INWFS);

                                        tr.Invoke((MethodInvoker)delegate
                                        {
                                            treeItem.ImageIndex = index;
                                            treeItem.SelectedImageIndex = index;
                                        });



                                    }
                                }
                                Logger.Instance.LogInformation(string.Format("Status Auto: {0} \n\n", auto.ToString()));
                                if (auto)
                                {
                                    Logger.Instance.LogInformation(string.Format("in if Auto \n\n"));
                                    tr.Invoke((MethodInvoker)delegate
                                    {

                                        Logger.Instance.LogInformation(string.Format("przed addCommentJira(item.Key.ToString());"));
                                        addCommentJira(item.Key.ToString());
                                        Logger.Instance.LogInformation(string.Format("po addCommentJira(item.Key.ToString());"));
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.LogError(ex, Logger.Instance);
                            sb.AppendLine(item.Key.Idnumber + ": " + ex.Message);
                        }
                    }
                    //                    NoticeForm.ShowNotice(sb.ToString()+sbDuble.ToString(), "Zapisano zgłoszenia");

                    if (toRemove.Count > 0)
                    {
                        foreach (KeyValuePair<BillingIssueDto, IssueState> item in toRemove)
                        {
                            tmp.Remove(item);
                        }
                    }
                    NoticeButtons nb = NoticeButtons.OK_CANCEL;
                    if (tmp.Count == 0)
                        nb = NoticeButtons.OK;
                    DialogResult dr;
                    if (!auto)
                    {
                        dr = NoticeForm.ShowNotice(sb.ToString(), nb, new string[] { "OK", "Dodaj komentarz do JIRA" });
                    }
                    else
                    {
                        dr = DialogResult.OK;
                    }


                    //MyMessageBox mms = new MyMessageBox(sb.ToString(), "Zapisno zgłoszenia");
                    //mms.ShowDialog();
                    //MessageBox.Show(sb.ToString(), "Zapisno zgłoszenia", MessageBoxButtons.OK);
                    pb_SetVisibilityPanel(false);
                    GetActionForIssues(tr);

                    /*Obsługa komentarza*/
                    try
                    {
                        if (dr == DialogResult.OK && nb == NoticeButtons.OK_CANCEL)
                        {

                            addCommentJira(tmp);


                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogError(ex, Logger.Instance);
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        EnableIssuesButtons();
                    });
                });

                t.Priority = ThreadPriority.Highest;
                t.IsBackground = true;
                t.Start();
            }
            //else
            //    NoticeForm.ShowNotice("Nie ma żadnych spraw do dodania do BPM.");
            //MessageBox.Show("Nie ma żadnych spraw do dodania do WFS.");
        }

        private void addCommentJira(List<KeyValuePair<BillingIssueDto, IssueState>> tmp)
        {
            bool czyPoprawneHaslo = true;
            foreach (KeyValuePair<BillingIssueDto, IssueState> item in tmp)
            {


                czyPoprawneHaslo = addCommentJira(item.Key.Idnumber.ToString());

            }

            if (!czyPoprawneHaslo)
                    MessageBox.Show("Komentarz dodany z imiennego konta. Sprawdź hasło do konta bilennium.");

        }
        private bool addCommentJira(string jiraKey)
        {                //komentarz przyjęcia z konta Billennium
            Jira jComment;
            bool czyPoprawneHaslo = true;
            try
            {
                jComment = Jira.CreateRestClient("http://jira", "billennium", Properties.Settings.Default.hasloBillennium);  
            }
            catch
            {
                czyPoprawneHaslo = false;
                jComment = jira;
                //.GetIssue(jiraKey).AddComment(
                //Properties.Settings.Default.KomentarzDoJira
                //);
            }

            if (jComment.GetIssue(jiraKey).GetComments().Any(x => x.Body == Properties.Settings.Default.hasloBillennium))
            {
                Logger.Instance.LogInformation(string.Format("addCommentJira string jiraKey {0} NIE DODANO KOMENTARZA BO ISTNIEJE WPIS", jiraKey.ToString()));
                return czyPoprawneHaslo;
            }

            jComment.GetIssue(jiraKey).AddComment(
                                Properties.Settings.Default.KomentarzDoJira
                                );

            Logger.Instance.LogInformation(string.Format("addCommentJira string jiraKey {0}", jiraKey.ToString()));

            return czyPoprawneHaslo;
        }

        private void addCommentJira(object sender, DoWorkEventArgs e)
        {
            string jiraKey = e.Argument.ToString();

            addCommentJira(jiraKey);
            Logger.Instance.LogInformation(string.Format("addCommentJira object sender, DoWorkEventArgs e {0}", jiraKey.ToString()));

                //    //komentarz przyjęcia z konta Billennium
                //    Jira jComment;
                //    jComment = Jira.CreateRestClient("http://jira", "billennium", Properties.Settings.Default.hasloBillennium);

                //    if (!jComment.GetIssue(jiraKey).GetComments().Any(x => x.Body == Properties.Settings.Default.hasloBillennium))
                //    {

                //        Logger.Instance.LogInformation(string.Format("addCommentJira string jiraKey {0} NIE DODANO KOMENTARZA BO ISTNIEJE WPIS", jiraKey.ToString()));
                //        return;
                //    }

                //    jComment.GetIssue(jiraKey).AddComment(
                //                        Properties.Settings.Default.KomentarzDoJira
                //                        );


                //}
                //catch
                //{
                //    jira.GetIssue(jiraKey).AddComment(
                //    Properties.Settings.Default.KomentarzDoJira
                //    );
            
        }

        /// <summary>
        /// btn_WyszukajWJira_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_WyszukajWJira_Click(object sender, EventArgs e)
        {
            numerZgl.Text.Trim();
            if (string.IsNullOrEmpty(numerZgl.Text))
            {
                NoticeForm.ShowNotice("Nie wprowadzono numeru zgłoszenia");
                return;
            }

            Search(numerZgl.Text, true, treeView3);
            issueTab.SelectTab(2);
        }

        /// <summary>
        /// Wyszukaj zgłoszenie w WFS z pominięciem Jira
        /// </summary>
        /// <param name="numer"></param>
        /// <param name="tr"></param>
        private void SearchWFS(string numer, TreeView tr)
        {
            DisableIssuesButtons();

            Thread thr = new Thread((ThreadStart)delegate ()
            {
                numerZgl.Text.Trim();

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();

                issue.Add(new BillingIssueDtoHelios()
                {
                    issueHelios = new IssueHelios()
                    {
                        number = numerZgl.Text
                    }
                }
                );

                if (!wfsList.ContainsKey(treeView3.Name))
                {
                    wfsList[treeView3.Name] = new List<BillingIssueDtoHelios>();
                }

                wfsList[treeView3.Name] = gujaczWFS.compareBillingWithWFS(issue);


                foreach (BillingIssueDtoHelios item in wfsList[treeView3.Name])
                {
                    BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);
                    IssueState state = (updatedIssue.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                    BillingIssueDtoHelios tmp = gujaczWFS.UpdateJiraInfo(updatedIssue);
                    issues[treeView3.Name].Add(tmp, state);
                }

                for (int i = 0; i < wfsList[treeView3.Name].Count; i++)
                {
                    wfsList[treeView3.Name][i] = gujaczWFS.UpdateJiraInfo(wfsList[treeView3.Name][i]);

                }

                GetActionForIssues(treeView3);
                AddToTree(treeView3);

                this.Invoke((MethodInvoker)delegate
                {
                    EnableIssuesButtons();
                });
            });
            thr.IsBackground = true;
            thr.Start();
        }

        /// <summary>
        /// btn_WyszukajWWFS_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_WyszukajWWFS_Click(object sender, EventArgs e)
        {
            numerZgl.Text.Trim();
            if (string.IsNullOrEmpty(numerZgl.Text))
            {
                NoticeForm.ShowNotice("Nie wprowadzono numeru zgłoszenia");
                return;
            }

            issues[treeView3.Name] = new Dictionary<BillingIssueDto, IssueState>();
            treeView3.Nodes.Clear();

            SearchWFS(numerZgl.Text, treeView3);
            issueTab.SelectTab(2);
        }

        /// <summary>
        /// Otwiera okno autouzupełniania
        /// </summary>
        /// <param name="tr"></param>
        //private void UzupelnijDane(TreeView tr)
        //{
        //    if (tr.SelectedNode != null)
        //    {
        //        KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == tr.SelectedNode.Text.Split(' ').First()).FirstOrDefault();
        //        //Czy element znajduje się w WFS??
        //        if (tmp.Key != null)
        //        {
        //            if (!tmp.Key.isInWFS)
        //            {
        //                BillingIssueDtoHelios it = tmp.Key as BillingIssueDtoHelios;
        //                bool trans = false;
        //                if (it.issueHelios.oryginalneId != it.issueHelios.number)
        //                    trans = true;

        //                WFSform wfs = new WFSform(tmp.Key, trans);
        //                wfs._tr = tr;

        //                wfs.callback = new WFSform.calbackDelegate(ReturnIssue);
        //                wfs.ShowDialog();
        //            }
        //            else
        //            {
        //                NoticeForm.ShowNotice("Wskazany element znajduje się w WFS nie można dodać go ponownie");
        //                //MessageBox.Show("Wskazany element znajduje się w WFS nie można dodać go ponownie");
        //            }
        //        }
        //    }
        //}

        private void UzupelnijDane(TreeView tr, bool auto = false)
        {
            for (int i = 0; i < tr.Nodes.Count; i++)
            {
                TreeNode trTmp = tr.Nodes[i];


                KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == trTmp.Text.Split(' ').First()).FirstOrDefault();
                //Czy element znajduje się w WFS??
                if (tmp.Key != null)
                {
                    if (!tmp.Key.isInWFS)
                    {
                        BillingIssueDtoHelios it = tmp.Key as BillingIssueDtoHelios;
                        bool trans = false;
                        if (it.issueHelios.oryginalneId != it.issueHelios.number)
                            trans = true;

                        WFSform wfs = new WFSform(tmp.Key, trans, false, auto);
                        wfs._tr = tr;

                        wfs.Visible = false;
                        wfs.callback = new WFSform.calbackDelegate(ReturnIssue);
                        if (auto)
                            doByWorker(new DoWorkEventHandler(addCommentJira), tmp.Key.Idnumber, null);
                        //addCommentJira(tmp.Key.Idnumber.ToString());
                        wfs.ShowDialog();

                    }
                    else
                    {
                        //NoticeForm.ShowNotice("Wskazany element znajduje się w WFS nie można dodać go ponownie");
                        //MessageBox.Show("Wskazany element znajduje się w WFS nie można dodać go ponownie");
                    }
                }
            }
            //if (tr.SelectedNode != null)
            //{
            //    KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == tr.SelectedNode.Text.Split(' ').First()).FirstOrDefault();
            //    //Czy element znajduje się w WFS??
            //    if (tmp.Key != null)
            //    {
            //        if (!tmp.Key.isInWFS)
            //        {
            //            BillingIssueDtoHelios it = tmp.Key as BillingIssueDtoHelios;
            //            bool trans = false;
            //            if (it.issueHelios.oryginalneId != it.issueHelios.number)
            //                trans = true;

            //            WFSform wfs = new WFSform(tmp.Key, trans);
            //            wfs._tr = tr;

            //            wfs.callback = new WFSform.calbackDelegate(ReturnIssue);
            //            wfs.ShowDialog();
            //        }
            //        else
            //        {
            //            NoticeForm.ShowNotice("Wskazany element znajduje się w WFS nie można dodać go ponownie");
            //            //MessageBox.Show("Wskazany element znajduje się w WFS nie można dodać go ponownie");
            //        }
            //    }
            //}
        }

        private void CreateNewIssue()
        {

            KeyValuePair<BillingIssueDto, IssueState> tmp = new KeyValuePair<BillingIssueDto, IssueState>(new BillingIssueDtoHelios(), IssueState.NEW);

            WFSform wfs = new WFSform(tmp.Key, false, true);
            wfs._tr = treeView3;
            wfs.NewIssue = true;

            wfs.callback = new WFSform.calbackDelegate(ReturnIssue);
            wfs.ShowDialog();
        }

        /// <summary>
        /// Context Menu zgłoszenia - Obsługa procesu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m1_Click(object sender, EventArgs e)
        {
            string p = sender.ToString();
            int eventMoveId = int.Parse((sender as ToolStripDropDownItem).Tag.ToString());

            TreeView tr = null;

            ToolStripDropDownItem ddi = sender as ToolStripDropDownItem;
            if (ddi == null)
            {
                return;
            }
            ContextMenuStrip cms = ddi.Owner as ContextMenuStrip;
            if (cms == null)
            {
                return;
            }
            else
            {
                tr = cms.SourceControl as TreeView;

                if (tr == null) return;
            }
            //Pobranie Rozmieszczenia parametrów zdarzenia
            List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

            //wmf = new WFSModelerForm(eventParamForFormByEventMove, sender.ToString(), selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerFormActionFinish));


            wmf = new WFSModelerForm(eventParamForFormByEventMove, sender.ToString(), selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerFormActionFinish), tr);

            try
            {
                if (!wmf.IsDisposed)
                    wmf.ShowDialog();

            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
        }

        /// <summary>
        /// Context Menu zgłoszenia - Uzupełnij dane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autouzupelnianieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string p = sender.ToString();
            //int eventMoveId = int.Parse((sender as ToolStripDropDownItem).Tag.ToString());

            TreeView tr = null;
            ToolStripDropDownItem ddi = sender as ToolStripDropDownItem;
            if (ddi == null)
            {
                return;
            }
            ContextMenuStrip cms = ddi.Owner as ContextMenuStrip;
            if (cms == null)
            {
                return;
            }
            else
            {
                tr = cms.SourceControl as TreeView;

                if (tr == null) return;
            }

            if (issues[tr.Name].Count > 0)
            {
                AutoCompleteForm acf = new AutoCompleteForm();
                acf.ShowDialog();

                BillingIssueDto[] keys = issues[tr.Name].Keys.ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    if (!keys[i].isInWFS)
                    {
                        WFSform wfs = new WFSform(keys[i], acf.idSystemu, acf.idKategorii, acf.idRodzaju, acf.idTypu, acf.idKontraktu);
                        wfs._tr = tr;
                        wfs.callback = new WFSform.calbackDelegate(AutoReturnIssue);
                        wfs.KliknijMnie();
                        //wfs.ShowDialog();
                    }
                }

                AddToTree(tr);
            }
            else
            {
                NoticeForm.ShowNotice("Brak zgłoszeń do uzupełnienia", "Info");
                //MessageBox.Show("Brak zgłoszeń do uzupełnienia", "Info");
            }
        }

        /// <summary>
        /// Context Menu zgłoszenia - Ponowne otwarcie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ponowneOtwieranieZgloszen_Click(object sender, EventArgs e)
        {
            TreeView tr = null;
            ToolStripDropDownItem ddi = sender as ToolStripDropDownItem;
            if (ddi == null)
            {
                return;
            }
            ContextMenuStrip cms = ddi.Owner as ContextMenuStrip;
            if (cms == null)
            {
                return;
            }
            else
            {
                tr = cms.SourceControl as TreeView;

                if (tr == null) return;
            }


            List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(608);
            WFSModelerForm wfsmf = new WFSModelerForm(eventParamForFormByEventMove, "Ponowne otwarcie zgłoszenia", selectIssue, gujaczWFS, 608, new WFSModelerForm.calbackDelegate(ModelerFormActionFinish), tr);


            try
            {
                if (!wfsmf.IsDisposed)
                    wfsmf.Show();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
        }

        /// <summary>
        /// Context Menu zgłoszenia - Uzupełnij dane (NIEPRZYDZIELONE)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uzupełnijDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_NieprzydzieloneUzupelnijDane_Click(null, null);
        }

        /// <summary>
        /// Context Menu zgłoszenia - Uzupełnij dane (MOJE)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uzupełnijDaneToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            btn_MojeUzupelnijDane_Click(null, null);
        }

        /// <summary>
        /// Context Menu zgłoszenia - Uzupełnij dane (BILLENNIUM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uzupełnijDaneToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            btn_BillenniumUzupelnijDane_Click(null, null);
        }

        /// <summary>
        /// Context Menu zgłoszenia - Multi 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddIssueToMulti_Click(object sender, EventArgs e)
        {
            return;
            AddIssueToMultiList(selectIssue.Idnumber);
        }

        /// <summary>
        /// Zdarzenie PPM na Nieprzydzielonych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeView tr = sender as TreeView;

                if (!issueMove.ContainsKey(tr.Name))
                {
                    issueMove[tr.Name] = new Dictionary<string, Dictionary<int, string>>();
                }


                if (tr == null) return;

                TreeNode tn = tr.GetNodeAt(e.X, e.Y);
                tr.SelectedNode = tn;

                if (tr.SelectedNode != null)
                {
                    treeView_AfterSelect(tr as Object, new TreeViewEventArgs(tr.SelectedNode));
                }

                string issueNumber = e.Node.Text.Split(' ').First();
                cms_IssuePopup.Items.Clear();
                KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x =>
                {
                    if (x.Key.Idnumber == e.Node.Text.Split(' ').First())
                        return true;
                    else
                        return false;
                }).FirstOrDefault();


                selectIssue = tmp.Key;
                //string tmp = (sender as TreeNode).Text;
                string number = e.Node.Text.Split(' ').First();
                if (issueMove[tr.Name] != null && issueMove[tr.Name].ContainsKey(number))
                {
                    cms_IssuePopup.Items.Clear();
                    string key = e.Node.Text.Split(' ').First();
                    Dictionary<int, string> actions = new Dictionary<int, string>();

                    GetActionForIssue(tmp.Key, tr.Name);

                    bool success = issueMove[tr.Name].TryGetValue(key, out actions);
                    if (success)
                    {
                        var actionsList = actions.ToList();
                        //if (tmp2.First().Value.Count!=0)
                        ToolStripMenuItem m1 = new ToolStripMenuItem();
                        foreach (var item in actionsList)
                        {

                            //cms_IssuePopup.Items.Add(item.Value + " (" + item.Key.ToString() + ")");
                            m1 = new ToolStripMenuItem(item.Value + " (" + item.Key.ToString() + ")");
                            m1.Click += new EventHandler(m1_Click);
                            m1.Tag = item.Key.ToString();
                            cms_IssuePopup.Items.Add(m1);
                        }
                        //ToolStripMenuItem m2 = new ToolStripMenuItem("Przydziel do mnie");
                        //m2.Click += new EventHandler(m1_Click);
                        //m2.Tag = 614;
                        //cms_IssuePopup.Items.Add(m2);
                        if (actions.Count == 0)
                        {
                            if (selectIssue.isInWFS || selectIssue.issueWFS.WFSState.Equals("Zgłoszenie zamknięte", StringComparison.CurrentCultureIgnoreCase))
                            {
                                m1 = new ToolStripMenuItem("Ponowne otwarcie zgłoszenia");
                                m1.Click += new EventHandler(ponowneOtwieranieZgloszen_Click);
                                cms_IssuePopup.Items.Add(m1);
                            }
                            else
                            {
                                m1 = new ToolStripMenuItem("Uzupełnij dane");

                                if (tr.Name == "treeView1")
                                {
                                    m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem_Click);
                                }
                                else if (tr.Name == "treeView2")
                                {
                                    m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem2_Click);
                                }
                                else if (tr.Name == "treeView3")
                                {
                                    m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem3_Click);
                                }

                                cms_IssuePopup.Items.Add(m1);
                            }
                        }
                        else
                        {
                            //dodanie listy podpowiedzi

                        }

                        ////wyłączenie multi
                        //m1 = new ToolStripMenuItem("Multi");
                        //m1.Click += new EventHandler(btn_AddIssueToMulti_Click);
                        //cms_IssuePopup.Items.Add(m1);
                    }
                }

                if (tn.Parent == null && tn.Text == "Zgłoszenia Helios")
                {
                    ToolStripMenuItem m1 = new ToolStripMenuItem("Autouzupełnianie");
                    m1.Click += new EventHandler(autouzupelnianieToolStripMenuItem_Click);
                    cms_IssuePopup.Items.Add(m1);
                }
            }
        }

        /// <summary>
        /// Zdarzenie PPM na Moich
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        /// <summary>
        /// Zdarzenie PPM na Billennium
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        /// <summary>
        /// Wyłączenie przycisków obsługi zgłoszeń
        /// </summary>
        private void DisableIssuesButtons()
        {
            issueTab.Enabled = false;
            this.isIssuesRefreshRunning = true;
        }

        /// <summary>
        /// Włączenie przycisków obsługi zgłoszeń
        /// </summary>
        private void EnableIssuesButtons()
        {
            issueTab.Enabled = true;
            this.isIssuesRefreshRunning = false;
        }

        /// <summary>
        /// btn_NieprzydzieloneOdsiwez_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_NieprzydzieloneOdsiwez_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            SLAlista.Clear();
            SLAlista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport_v2", new string[] { }, DatabaseName.SupportCP);

            if (!issues.ContainsKey(treeView1.Name))
            {
                issues[treeView1.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }

            if (cbox_RefreshBoth.Checked)
            {
                treeView2.Nodes.Clear();
                if (!issues.ContainsKey(treeView2.Name))
                {
                    issues[treeView2.Name] = new Dictionary<BillingIssueDto, IssueState>();
                }

                GetAllIssuesFromJira();
            }
            else
                GetIssuesFromJira("filtr1", false, treeView1);
        }

        /*//zawieszone
        private void btn_ProblemOdswiez_Click(object sender, EventArgs e)
        {
            treeView4.Nodes.Clear();
            if (!issues.ContainsKey(treeView4.Name))
            {
                issues[treeView4.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }

            GetIssuesFromJira("filtr3", false, treeView4);
        }
        */
        /// <summary>
        /// btn_NieprzydzieloneUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NieprzydzieloneUzupelnijDane_Click(object sender, EventArgs e)
        {
            UzupelnijDane(treeView1);
            UzupelnijDane(treeView2);

            ZapiszZgloszenia(treeView1);
            ZapiszZgloszenia(treeView2);
        }

        /// <summary>
        /// btn_MojeOdswiez_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MojeOdswiez_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
            if (!issues.ContainsKey(treeView2.Name))
            {
                issues[treeView2.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }

            if (cbox_RefreshBoth.Checked)
            {
                if (!issues.ContainsKey(treeView1.Name))
                {
                    issues[treeView1.Name] = new Dictionary<BillingIssueDto, IssueState>();
                }

                GetAllIssuesFromJira();
            }
            else
                GetIssuesFromJira("filtr2", false, treeView2);
        }


        private void btn_Odswiez_Click(object sender, EventArgs e)
        {
            try
            {
                switch (currentTreeTabIndex)
                {
                    case 0: /* ODŚWIEŻ NIEPRZYDZIELONE */
                        {

                            break;
                        }
                    case 1: /* ODŚWIEŻ MOJE */ break;
                    case 2: /* ODŚWIEŻ BILL */ break;
                    default: throw new IndexOutOfRangeException("Wybrano błędną zakładkę!");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Logger.Instance.LogWarning(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// btn_BillenniumUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BillenniumUzupelnijDane_Click(object sender, EventArgs e)
        {
            UzupelnijDane(treeView3);
        }


        /// <summary>
        /// btn_MojeUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MojeUzupelnijDane_Click(object sender, EventArgs e)
        {
            UzupelnijDane(treeView2);
            UzupelnijDane(treeView1);
        }

        private void btn_BillenniumZapisz_Click(object sender, EventArgs e)
        {
            ZapiszZgloszenia(treeView3);
        }

        /// <summary>
        /// btn_MojeZapiszDoWFS_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MojeZapiszDoWFS_Click(object sender, EventArgs e)
        {
            ZapiszZgloszenia(treeView2);
        }

        /// <summary>
        /// btn_NieprzydzieloneZapiszDoWFS_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NieprzydzieloneZapiszDoWFS_Click(object sender, EventArgs e)
        {
            ZapiszZgloszenia(treeView1);
        }


        /// <summary>
        /// Po zmienie zakładki zgłoszeń
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void issueTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as TabControl).SelectedIndex == 0)
                assignedIssues = false;
            else
                assignedIssues = true;
            //Uniewidocznienie zakładni pomocniczej
            if ((sender as TabControl).SelectedIndex == 3)
            {
                (sender as TabControl).SelectedIndex = 2;
            }

            this.currentTreeTabIndex = (sender as TabControl).SelectedIndex;
        }
        #endregion

        #region JIRA
        // Motedy wyszukiwania zgłoszeń w Jira

        /// <summary>
        /// Wyszukaj w Jira
        /// </summary>
        /// <param name="type"></param>
        /// <param name="is_number"></param>
        /// <param name="tr"></param>
        private void Search(string type, bool is_number, TreeView tr)
        {
            if (!issues.ContainsKey(tr.Name))
            {
                issues[tr.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }
            SearchJira(type, is_number, tr);
        }

        /// <summary>
        /// Zaloguj do Jira
        /// </summary>
        /// <returns>Czy zalogowano</returns>
        private bool LoginJira()
        {
            try
            {
                jira = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password );
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password); // http://jira01-t2 http://jira
                //var typy = jira.GetIssueTypes();
                var priorities = jira.GetIssuePriorities();

            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Nie można rozpoznać nazwy zdalnej: 'jira'"))
                    ExceptionManager.LogError(ex, Logger.Instance, true);
                else
                {
                    Exception exeption = new Exception("Błędne Hasło JIRA dla użytkownika " + jiraUser.Login, ex.InnerException);
                    ExceptionManager.LogError(exeption, Logger.Instance, true);
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Wyszukaj w Jira
        /// </summary>
        /// <param name="numerZgl"></param>
        /// <param name="is_number"></param>
        /// <param name="tr"></param>
        private void SearchJira(string numerZgl, bool is_number, TreeView tr)
        {
            if (LoginJira())
            {
                List<string> nieznalezione = new List<string>();
                //jira = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password);
                DisableIssuesButtons();
                pb_SetVisibilityPanel(true);
                Thread thr = new Thread((ThreadStart)delegate ()
                {
                    issues[tr.Name].Clear();
                    try
                    {
                        pb_SetMaxProgressBar(3);
                        pb_UpdateProgressBar("Rozpoczynanie pracy");

                        pb_UpdateProgressBar("Wczytywanie danych z Heliosa");
                        List<BillingIssueDtoHelios> issue;

                        Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
                        List<string> types = jIssues.GetJiraIssuesTypes();
                        List<string> stat = jIssues.GetJiraIssuesStatuses();
                        List<string> pro = jIssues.GetJiraProjects();

                        IEnumerable<Issue> issuesJira = jIssues.GetIssuesByNumberAsync(numerZgl);


                        List<BillingIssueDtoHelios> BillingissDtoHel = new List<BillingIssueDtoHelios>();

                        jIssues.ChangeDataModel(issuesJira, out BillingissDtoHel, ref JiraUsers);
                        issue = BillingissDtoHel.ToList();

                        pb_SetMaxProgressBar(issue.Count + 2);
                        pb_UpdateProgressBar("Wczytanie danych z Heliosa");

                        if (!wfsList.ContainsKey(tr.Name))
                        {
                            wfsList[tr.Name] = new List<BillingIssueDtoHelios>();
                        }

                        wfsList[tr.Name] = gujaczWFS.compareBillingWithWFS(issue);

                        pb_UpdateProgressBar("Porównanie z WFS");
                        foreach (BillingIssueDtoHelios item in wfsList[tr.Name])
                        {
                            BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);

                            IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                            issues[tr.Name].Add(updatedIssue, state);
                            pb_UpdateProgressBar("Analiza zgłoszenia:" + updatedIssue.Idnumber);
                        }

                        if (nieznalezione.Count > 0)
                        {
                            foreach (var asd in nieznalezione)
                            {
                                NoticeForm.ShowNotice("Nie znaleziono zgłoszenia: " + asd);
                            }
                        }

                        GetActionForIssues(tr);
                        AddToTree(tr);
                        pb_SetVisibilityPanel(false);
                        //Włączenie przycisków do pobierania zgłoszeń
                        this.Invoke((MethodInvoker)delegate
                        {
                            EnableIssuesButtons();
                        });
                    }
                    catch (Exception ex)
                    {

                        ExceptionManager.LogError(ex, Logger.Instance, true);
                    }
                });
                thr.IsBackground = true;
                thr.Start();
            }
            else
            {
                NoticeForm.ShowNotice("Błędne hasło do Jira");
                //MessageBox.Show("Błędne hasło do Jira");
            }
        }

        public void SearchJiraIssueAsync(string numerZgl, IEnumerable<Issue> sJira)
        {
            sJira = null;
            if (LoginJira())
            {
                jira = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password);
                //Thread thr = new Thread((ThreadStart)delegate ()
                //{
                //    try
                //    {


                Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(Properties.Settings.Default.loginJira, Properties.Settings.Default.hasloJira, "http://jira");
                IEnumerable<Issue> sJiraIssue = jIssues.GetIssuesByNumberAsync(numerZgl);

                sJira = sJiraIssue;

                //    }
                //    catch (Exception ex)
                //    {

                //        ExceptionManager.LogError(ex, Logger.Instance, true);
                //    }
                //});
                //thr.IsBackground = true;
                //thr.Start();
            }
            else
            {
                sJira = null;
                NoticeForm.ShowNotice("Błędne hasło do Jira");
                //MessageBox.Show("Błędne hasło do Jira");
            }
        }

        /// <summary>
        /// Wczytanie elementów z bazy przy użyciu wątków
        /// </summary>
        private void GetIssuesFromJira(string numerZgl, bool is_number, TreeView tr)
        {

            if (LoginJira())
            {
                jira = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password); // http://jira  http://jira01-t2
                DisableIssuesButtons();
                pb_SetVisibilityPanel(true);
                Thread thr = new Thread((ThreadStart)delegate ()
                {
                    issues[tr.Name].Clear();
                    try
                    {

                        pb_SetMaxProgressBar(3);
                        pb_UpdateProgressBar("Rozpoczynanie pracy");

                        pb_UpdateProgressBar("Wczytywanie danych z Jira");
                        List<BillingIssueDtoHelios> issue;
                        if (is_number)
                        {
                            issue = new List<BillingIssueDtoHelios>();

                            string[] _issues = numerZgl.Split(',');
                        }
                        else
                        {
                            Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
                            List<string> types = jIssues.GetJiraIssuesTypes();
                            List<string> stat = jIssues.GetJiraIssuesStatuses();
                            List<string> pro = jIssues.GetJiraProjects();

                            string filterName = string.Empty;

                            if (assignedIssues)
                                filterName = Properties.Settings.Default.assignedFilterName;
                            else
                                filterName = Properties.Settings.Default.unassignedFilterName;

                            IEnumerable<Issue> issuesJira = jIssues.GetIssues(assignedIssues, filterName);

                            List<BillingIssueDtoHelios> BillingissDtoHel = new List<BillingIssueDtoHelios>();

                            jIssues.ChangeDataModel(issuesJira, out BillingissDtoHel, ref JiraUsers);
                            issue = BillingissDtoHel;
                        }

                        pb_SetMaxProgressBar(issue.Count + 2);
                        pb_UpdateProgressBar("Wczytanie danych z Jira");

                        if (!wfsList.ContainsKey(tr.Name))
                        {
                            wfsList[tr.Name] = new List<BillingIssueDtoHelios>();
                        }

                        wfsList[tr.Name] = gujaczWFS.compareBillingWithWFS(issue);

                        pb_UpdateProgressBar("Porównanie z BPM");
                        foreach (BillingIssueDtoHelios item in wfsList[tr.Name])
                        {
                            BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);

                            IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                            issues[tr.Name].Add(updatedIssue, state);
                            pb_UpdateProgressBar("Analiza zgłoszenia:" + updatedIssue.Idnumber);
                        }

                        t_UpdateLiczbaZgloszen(issues["treeView1"].Count, issues["treeView2"].Count);
                        GetActionForIssues(tr);
                        AddToTree(tr);
                        pb_SetVisibilityPanel(false);
                        //Włączenie przycisków do pobierania zgłoszeń
                        this.Invoke((MethodInvoker)delegate
                        {
                            EnableIssuesButtons();
                            if (pauza && checkIssues)
                            {
                                timer2.Start();
                                pauza = false;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogError(ex, Logger.Instance, true);
                    }
                });
                btn_MojeZapiszDoWFS.Enabled = true;
                btn_NieprzydzieloneZapiszDoWFS.Enabled = true;
                btn_BillenniumZapisz.Enabled = true;
                thr.IsBackground = true;
                thr.Start();
            }
            else
            {
                NoticeForm.ShowNotice("Błędne hasło do Jira");
                //MessageBox.Show("Błędne hasło do Jira");
            }

        }
        private void GetAllIssuesFromJira(object sener, EventArgs e)
        {
            // POBIERAM DO MOICH I NIEPRZYDZIELONYCH!

            if (LoginJira())
            {
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password);
                //DisableIssuesButtons();
                pb_SetVisibilityPanel(true);
                //Thread thr = new Thread((ThreadStart)delegate ()
                //{
                    issues["treeView1"].Clear();
                    issues["treeView2"].Clear();
                    issues["treeView3"].Clear();
                    //issues["treeView4"].Clear();
                    try
                    {
                        pb_SetMaxProgressBar(3);
                        pb_UpdateProgressBar("Rozpoczynanie pracy");

                        pb_UpdateProgressBar("Wczytywanie danych z Jira");

                        // POBIERANIE ZGŁOSZEŃ Z OBU KATEGORII
                        List<BillingIssueDtoHelios> assignedIssues;
                        List<BillingIssueDtoHelios> unassignedIssues;



                        Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
                        //List<string> types = jIssues.GetJiraIssuesTypes();
                        //List<string> stat = jIssues.GetJiraIssuesStatuses();
                        //List<string> pro = jIssues.GetJiraProjects();

                        IEnumerable<Issue> issuesJira;

                        // NIEPRZYDZIELONE
                        string unassignedFilterName = Properties.Settings.Default.unassignedFilterName;
                        issuesJira = jIssues.GetIssues(false, unassignedFilterName);
                        jIssues.ChangeDataModel(issuesJira, out unassignedIssues, ref JiraUsers);

                        // PRZYDZIELONE
                        string assignedFilterName = Properties.Settings.Default.assignedFilterName;
                        issuesJira = jIssues.GetIssues(true, assignedFilterName);
                        jIssues.ChangeDataModel(issuesJira, out assignedIssues, ref JiraUsers);

                        /* //zawieszone
                        // PROBLEMY

                        List<BillingIssueDtoHelios> ProblemIssues;

                        string problemFilterName = "SOS&ZCC TODO v1";
                        issuesJira = jIssues.GetIssues(usersProject, true, problemFilterName);
                        jIssues.ChangeDataModel(issuesJira, out ProblemIssues);
                        */
                        pb_SetMaxProgressBar(unassignedIssues.Count + assignedIssues.Count + 3);
                        pb_UpdateProgressBar("Wczytywanie danych z Jira");

                        // ROZPOCZYNAM ANALIZĘ ZGŁOSZEŃ

                        if (!wfsList.ContainsKey("treeView1"))
                        {
                            wfsList["treeView1"] = new List<BillingIssueDtoHelios>();
                        }

                        wfsList["treeView1"] = gujaczWFS.compareBillingWithWFS(unassignedIssues);

                        if (!wfsList.ContainsKey("treeView2"))
                        {
                            wfsList["treeView2"] = new List<BillingIssueDtoHelios>();
                        }

                        wfsList["treeView2"] = gujaczWFS.compareBillingWithWFS(assignedIssues);

                        /*///zakładka problem zawieszona
                        if (!wfsList.ContainsKey("treeView4"))
                        {
                            wfsList["treeView4"] = new List<BillingIssueDtoHelios>();
                        }

                        wfsList["treeView4"] = gujaczWFS.compareBillingWithWFS(ProblemIssues);
                        */
                        pb_UpdateProgressBar("Porównanie z BPM");

                        // NIEPRZYDZIELONE
                        foreach (BillingIssueDtoHelios item in wfsList["treeView1"])
                        {
                            BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);

                            IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                            issues["treeView1"].Add(issue, state);

                            pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));

                        }

                        // PRZYDZIELONE
                        foreach (BillingIssueDtoHelios item in wfsList["treeView2"])
                        {
                            BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);

                            IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                            issues["treeView2"].Add(issue, state);

                            pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));

                        }
                        /*
                        // PROBLEMY zawieszona
                        foreach (BillingIssueDtoHelios item in wfsList["treeView4"])
                        {
                            BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);
                            IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                            issues["treeView4"].Add(issue, state);

                            pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));
                        }
                        */
                        t_UpdateLiczbaZgloszen(issues["treeView1"].Count, issues["treeView2"].Count);

                        GetActionForIssues(treeView1);
                        AddToTree(treeView1);
                        GetActionForIssues(treeView2);
                        AddToTree(treeView2);
                        /* ///Zakładka problem zawieszona
                            GetActionForIssues(treeView4);
                            AddToTree(treeView4);
                        */
                        pb_SetVisibilityPanel(false);



                        //Włączenie przycisków do pobierania zgłoszeń --> przeniesione do finally
                        //this.Invoke((MethodInvoker)delegate
                        //{
                        //    EnableIssuesButtons();
                        //    if (pauza && checkIssues)
                        //    {
                        //        timer2.Start();
                        //        pauza = false;
                        //    }
                        //});
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogError(ex, Logger.Instance, true);

                    }
                    finally
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            EnableIssuesButtons();
                            if (pauza && checkIssues)
                            {
                                timer2.Start();
                                pauza = false;
                            }
                        });
                    }
                //});
                //thr.IsBackground = true;
                //thr.Start();

                btn_MojeZapiszDoWFS.Enabled = true;
                btn_NieprzydzieloneZapiszDoWFS.Enabled = true;
                btn_BillenniumZapisz.Enabled = true;
            }
            else
            {
                //NoticeForm.ShowNotice("Błędne hasło do Jira!");
            }
        }

        private void GetAllIssuesFromJira()
        {
            // POBIERAM DO MOICH I NIEPRZYDZIELONYCH!

            if (LoginJira())
            {
                //new Jira("http://jira", jiraUser.Login, jiraUser.Password);
                DisableIssuesButtons();
                pb_SetVisibilityPanel(true);
                Thread thr = new Thread((ThreadStart)delegate ()
                    {
                        issues["treeView1"].Clear();
                        issues["treeView2"].Clear();
                        issues["treeView3"].Clear();
                        //issues["treeView4"].Clear();
                        try
                        {
                            pb_SetMaxProgressBar(3);
                            pb_UpdateProgressBar("Rozpoczynanie pracy");

                            pb_UpdateProgressBar("Wczytywanie danych z Jira");

                            // POBIERANIE ZGŁOSZEŃ Z OBU KATEGORII
                            List<BillingIssueDtoHelios> assignedIssues;
                            List<BillingIssueDtoHelios> unassignedIssues;



                            Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
                            //List<string> types = jIssues.GetJiraIssuesTypes();
                            //List<string> stat = jIssues.GetJiraIssuesStatuses();
                            //List<string> pro = jIssues.GetJiraProjects();

                            IEnumerable<Issue> issuesJira;

                            // NIEPRZYDZIELONE
                            string unassignedFilterName = Properties.Settings.Default.unassignedFilterName;
                            issuesJira = jIssues.GetIssues(false, unassignedFilterName);
                            jIssues.ChangeDataModel(issuesJira, out unassignedIssues    , ref JiraUsers);

                            // PRZYDZIELONE
                            string assignedFilterName = Properties.Settings.Default.assignedFilterName;
                            issuesJira = jIssues.GetIssues(true, assignedFilterName);
                            jIssues.ChangeDataModel(issuesJira, out assignedIssues, ref JiraUsers);

                            /* //zawieszone
                            // PROBLEMY

                            List<BillingIssueDtoHelios> ProblemIssues;

                            string problemFilterName = "SOS&ZCC TODO v1";
                            issuesJira = jIssues.GetIssues(usersProject, true, problemFilterName);
                            jIssues.ChangeDataModel(issuesJira, out ProblemIssues);
                            */
                            pb_SetMaxProgressBar(unassignedIssues.Count + assignedIssues.Count + 3);
                            pb_UpdateProgressBar("Wczytywanie danych z Jira");

                            // ROZPOCZYNAM ANALIZĘ ZGŁOSZEŃ

                            if (!wfsList.ContainsKey("treeView1"))
                            {
                                wfsList["treeView1"] = new List<BillingIssueDtoHelios>();
                            }

                            wfsList["treeView1"] = gujaczWFS.compareBillingWithWFS(unassignedIssues);

                            if (!wfsList.ContainsKey("treeView2"))
                            {
                                wfsList["treeView2"] = new List<BillingIssueDtoHelios>();
                            }

                            wfsList["treeView2"] = gujaczWFS.compareBillingWithWFS(assignedIssues);

                            /*///zakładka problem zawieszona
                            if (!wfsList.ContainsKey("treeView4"))
                            {
                                wfsList["treeView4"] = new List<BillingIssueDtoHelios>();
                            }

                            wfsList["treeView4"] = gujaczWFS.compareBillingWithWFS(ProblemIssues);
                            */
                            pb_UpdateProgressBar("Porównanie z BPM");

                            // NIEPRZYDZIELONE
                            foreach (BillingIssueDtoHelios item in wfsList["treeView1"])
                            {
                                BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);

                                IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                                issues["treeView1"].Add(issue, state);

                                pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));

                            }

                            // PRZYDZIELONE
                            foreach (BillingIssueDtoHelios item in wfsList["treeView2"])
                            {
                                BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);

                                IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                                issues["treeView2"].Add(issue, state);

                                pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));

                            }
                            /*
                            // PROBLEMY zawieszona
                            foreach (BillingIssueDtoHelios item in wfsList["treeView4"])
                            {
                                BillingIssueDtoHelios issue = gujaczWFS.UpdateIssue(item);
                                IssueState state = (issue.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                                issues["treeView4"].Add(issue, state);

                                pb_UpdateProgressBar(string.Format("Analiza zgłoszenia: {0}", issue.Idnumber));
                            }
                            */
                            t_UpdateLiczbaZgloszen(issues["treeView1"].Count, issues["treeView2"].Count);

                            GetActionForIssues(treeView1);
                            AddToTree(treeView1);
                            GetActionForIssues(treeView2);
                            AddToTree(treeView2);
                            /* ///Zakładka problem zawieszona
                                GetActionForIssues(treeView4);
                                AddToTree(treeView4);
                            */
                            pb_SetVisibilityPanel(false);



                            //Włączenie przycisków do pobierania zgłoszeń --> przeniesione do finally
                            //this.Invoke((MethodInvoker)delegate
                            //{
                            //    EnableIssuesButtons();
                            //    if (pauza && checkIssues)
                            //    {
                            //        timer2.Start();
                            //        pauza = false;
                            //    }
                            //});
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.LogError(ex, Logger.Instance, true);

                        }
                        finally
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                EnableIssuesButtons();
                                if (pauza && checkIssues)
                                {
                                    timer2.Start();
                                    pauza = false;
                                }
                            });
                        }
                    });
                thr.IsBackground = true;
                thr.Start();

                btn_MojeZapiszDoWFS.Enabled = true;
                btn_NieprzydzieloneZapiszDoWFS.Enabled = true;
                btn_BillenniumZapisz.Enabled = true;
            }
            else
            {
                //NoticeForm.ShowNotice("Błędne hasło do Jira!");
            }
        }
        #endregion

        #region WORKLOAD
        /*
        private void AddToWorkload(BillingIssueDto issue)
        {
            if (!workload.IsLogging)
            {
                lb_workloadTitle.Text = "Numer zgłoszenia: " + issue.Idnumber;
                workload.Issue = issue;
                lb_WorkloadLoggedTime.Text = "Zalogowany czas: " + workload.LoggedTime.ToString("h'h 'm'm 's's'");
                lb_WorkloadTotalTime.Text = "Całkowity czas: " + workload.TotalTime.ToString("h'h 'm'm 's's'");

                if (workload.Status == Logic.Implementation.WorkloadStatus.OtherUser)
                {
                    ssb_WorkloadButton.Enabled = false;
                    ssb_WorkloadStopButton.Enabled = false;
                    lb_workloadTitle.Text += " (realizowane przez innego użytkownika)";
                }
                else if (workload.Status == Logic.Implementation.WorkloadStatus.Pause || workload.Status == Logic.Implementation.WorkloadStatus.Stop)
                {
                    ssb_WorkloadButton.Enabled = true;
                    ssb_WorkloadStopButton.Enabled = false;
                }
                else
                {
                    ssb_WorkloadButton.Enabled = true;
                    ssb_WorkloadStopButton.Enabled = true;
                }
            }
        }

        private void AddToWorkload(string issueNumber)
        {
            List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();

            issue.Add(new BillingIssueDtoHelios()
            {
                issueHelios = new IssueHelios()
                {
                    number = issueNumber
                }
            }
            );

            List<BillingIssueDtoHelios> lista = new List<BillingIssueDtoHelios>();

            lista = gujaczWFS.compareBillingWithWFS(issue);

            foreach (BillingIssueDtoHelios item in lista)
            {
                IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;
                item.issueWFS.DataWystapieniaBledu = item.issueHelios.date;
                item.issueWFS.Email = item.issueHelios.email;
                item.issueWFS.Imie = item.issueHelios.firstName;
                item.issueWFS.Nazwisko = item.issueHelios.lastName;
                item.issueWFS.TrescZgloszenia = item.issueHelios.content;
                item.issueWFS.TytulZgloszenia = item.issueHelios.title;
                item.issueWFS.NumerZgloszenia = item.issueWFS.WFSIssueId.ToString();
                item.issueWFS.IdKontraktu = item.issueHelios.idKontraktu;

                if (item.issueWFS.NumerZgloszenia != "0")
                {

                    List<EventParam> evep = gujaczWFS.GetBillingBoundEventParamForIssue(item.issueWFS.WFSIssueId, new int[] { 2839, 2840, 2841, 2842, 2867 });

                    foreach (EventParam ep in evep)
                    {
                        if (ep.EventParamId == 2839)
                        {
                            item.issueWFS.System = new Entities.Component();
                            item.issueWFS.System.Text = ep.Value;
                            item.issueWFS.System.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                        }
                        else if (ep.EventParamId == 2840)
                        {
                            item.issueWFS.Kategoria = new Entities.Component();
                            item.issueWFS.Kategoria.Text = ep.Value;
                            item.issueWFS.Kategoria.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                        }
                        else if (ep.EventParamId == 2841)
                        {
                            item.issueWFS.Rodzaj = new Entities.Component();
                            item.issueWFS.Rodzaj.Text = ep.Value;
                            item.issueWFS.Rodzaj.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                        }
                        else if (ep.EventParamId == 2842)
                        {
                            item.issueWFS.Typ = new Entities.Component();
                            item.issueWFS.Typ.Text = ep.Value;
                            item.issueWFS.Typ.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                        }
                        else if (ep.EventParamId == 2867)
                        {
                            item.issueWFS.IdKontraktu = item.issueHelios.idKontraktu;
                        }
                        else if (ep.EventParamId == 3525 || ep.EventParamId == 3526 || ep.EventParamId == 3524)
                        {
                            item.issueWFS.IdZamowienia = item.issueHelios.idZamowienia;
                        }
                    }
                }
            }

            AddToWorkload(lista[0] as BillingIssueDto);
            System.Diagnostics.Debug.WriteLine(" ");
        }

        private void btn_WorkloadTimer_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (workload.IsLogging)
            {
                workload.PauseLogging();
            }
            else
            {
                workload.StartLogging();
            }
        }

        private void ssb_WorkloadButton_Click(object sender, EventArgs e)
        {
            if (ssb_WorkloadStopButton.Enabled)
            {
                // PAUSE

                try
                {
                    workload.PauseLogging();

                    tm_WorkloadTimer.Enabled = false;
                    ssb_WorkloadButton.Image = global::GUI.Properties.Resources.start4;
                    ssb_WorkloadStopButton.Enabled = false;
                    lb_WorkloadLoggedTime.Text = "Zalogowany czas: " + workload.LastTime.ToString("h'h 'm'm 's's'");
                    tslb_WorkloadStatus.Text = workload.Issue.Idnumber + ": " + workload.LastTime.ToString("h'h 'm'm 's's'");

                    ni_NewIssueAlert.Icon = global::GUI.Properties.Resources.parserIcon;


                    UpdateWorkloadLists();
                }
                catch (InvalidOperationException ex)
                {
                    NoticeForm.ShowNotice(ex.Message);
                }
                catch (ArgumentNullException ex)
                {
                    NoticeForm.ShowNotice(ex.ParamName);
                }
            }
            else
            {
                // START

                try
                {
                    workload.StartLogging();

                    //MessageBox.Show("START");
                    tm_WorkloadTimer.Enabled = true;
                    ssb_WorkloadButton.Image = global::GUI.Properties.Resources.pause4;
                    ssb_WorkloadStopButton.Enabled = true;

                    ni_NewIssueAlert.Icon = global::GUI.Properties.Resources.startIcon2;

                    UpdateWorkloadLists();
                }
                catch (InvalidOperationException ex)
                {
                    NoticeForm.ShowNotice(ex.Message);
                }
                catch (ArgumentNullException ex)
                {
                    NoticeForm.ShowNotice(ex.ParamName);
                }
            }
        }

        private void ssb_WorkloadStopButton_Click(object sender, EventArgs e)
        {
            // STOP 

            try
            {
                workload.StopLogging();

                tm_WorkloadTimer.Enabled = false;
                ssb_WorkloadButton.Image = global::GUI.Properties.Resources.start4;
                ssb_WorkloadStopButton.Enabled = false;
                lb_WorkloadLoggedTime.Text = "Zalogowany czas: " + workload.LastTime.ToString("h'h 'm'm 's's'");
                tslb_WorkloadStatus.Text = workload.Issue.Idnumber + ": " + workload.LastTime.ToString("h'h 'm'm 's's'");

                ni_NewIssueAlert.Icon = global::GUI.Properties.Resources.parserIcon;

                UpdateWorkloadLists();
            }
            catch (InvalidOperationException ex)
            {
                NoticeForm.ShowNotice(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                NoticeForm.ShowNotice(ex.ParamName);
            }
        }

        private void UpdateWorkloadLists()
        {
            if (workload == null)
                return;

            wl_OpenIssuesListView.Items.Clear();

            List<List<string>> otwarte = workload.GetOpenIssues();
            foreach (var item in otwarte)
            {
                ListViewItem i = new ListViewItem(item[0]);
                i.SubItems.Add(item[1]);
                wl_OpenIssuesListView.Items.Add(i);
            }

            wl_PausedIssuesListView.Items.Clear();

            List<List<string>> zatrzymane = workload.GetPausedIssues();
            foreach (var item in zatrzymane)
            {
                ListViewItem i = new ListViewItem(item[1]);
                wl_PausedIssuesListView.Items.Add(i);
            }
        }

        private void WorkloadEnable(bool flag)
        {
            lb_workloadTitle.Enabled = flag;
            lb_WorkloadLoggedTime.Enabled = flag;
        }

        private void tm_WorkloadTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan time = workload.LoggedTime + (DateTime.Now - workload.StartTime);

            tslb_WorkloadStatus.Text = workload.Issue.Idnumber + ": " + time.ToString("h'h 'm'm 's's'");

        }
        */
        #endregion

        #region PROGRESSBAR
        private void t_UpdateLiczbaZgloszen(int nieprzydzieloneCount, int przydzieloneCount)
        {
            statusStrip1.Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel5.Text = nieprzydzieloneCount.ToString();
                toolStripStatusLabel3.Text = przydzieloneCount.ToString();
                nieprzydzielone = nieprzydzieloneCount.ToString();
                przydzielone = przydzieloneCount.ToString();
            });
        }
        private void t_UpdateLiczbaZgloszen(string liczba)
        {
            statusStrip1.Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel5.Text = liczba;
                nieprzydzielone = liczba;
            });
        }
        private void pb_UpdateProgressBar(string tekst)
        {
            toolStripProgressBar2.ProgressBar.Invoke(
            (MethodInvoker)delegate
            {
                try
                {
                    toolStripStatusLabel6.Text = tekst;
                    toolStripProgressBar2.ProgressBar.Value++;
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, false);
                    toolStripStatusLabel6.Text = "Błąd obsługi progressBar";
                }
            });
        }

        private void pb_SetMaxProgressBar(int maxValue)
        {
            toolStripProgressBar2.ProgressBar.Invoke((MethodInvoker)delegate
            {

                toolStripProgressBar2.ProgressBar.Visible = true;
                toolStripProgressBar2.ProgressBar.Value = 0;
                toolStripProgressBar2.ProgressBar.Maximum = maxValue;
            });
        }

        private void pb_SetVisibilityPanel(bool isVisible)
        {
            statusStrip1.Invoke((MethodInvoker)delegate
            {
                toolStripProgressBar2.ProgressBar.Visible = isVisible;
                toolStripStatusLabel6.Visible = isVisible;
            });
        }
        #endregion

        /// <summary>
        /// Obsługa zwrotu zgłoszenia z uzupełnionymi danymi z okna WFSForm
        /// </summary>
        /// <param name="zgloszenie"></param>
        private void ReturnIssue(BillingIssueDtoHelios zgloszenie, TreeView tr, bool newIssue = false)
        {
            if (!newIssue)
            {
                KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == zgloszenie.Idnumber).FirstOrDefault();
                issues[tr.Name].Remove(tmp.Key);
                issues[tr.Name].Add(zgloszenie, IssueState.READYTOSAVE);
                AddToTree(tr, zgloszenie);
            }
            else
            {
                issues[tr.Name].Add(zgloszenie, IssueState.READYTOSAVE);
                AddToTree(tr, zgloszenie);
            }
        }

        /// <summary>
        /// Pobiera akcje z BPM jakie na każdej ze spraw może wykonać użytkownik
        /// </summary>
        /// <param name="tr">drzewko zgłoszeń</param>
        private void GetActionForIssues(TreeView tr)
        {
            int UserId = gujaczWFS.getUser().Id;
            if (UserId != 0)
            {
                try
                {
                    if (!issueMove.ContainsKey(tr.Name))
                    {
                        issueMove[tr.Name] = new Dictionary<string, Dictionary<int, string>>();
                    }

                    issueMove[tr.Name].Clear();

                    foreach (KeyValuePair<BillingIssueDto, IssueState> item in issues[tr.Name])
                    {
                        Dictionary<int, string> moves = gujaczWFS.GetActionForIssue(item.Key.issueWFS.WFSIssueId, UserId);
                        //Dictionary<int, string> moves = gujaczWFS.GetActionForIssueWorkaround(item.Key.issueWFS.WFSIssueId, UserId);
                        issueMove[tr.Name].Add(item.Key.Idnumber, moves);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, false);
                    issueMove[tr.Name].Clear();
                }
            }
        }


        private void GetActionForIssue(BillingIssueDto issue, string trName)
        {
            int UserId = gujaczWFS.getUser().Id;
            if (UserId != 0)
            {
                try
                {
                    if (!issueMove.ContainsKey(trName))
                    {
                        issueMove[trName] = new Dictionary<string, Dictionary<int, string>>();
                    }
                    else
                    {
                        issueMove[trName].Remove(issue.Idnumber);
                    }

                    Dictionary<int, string> moves = gujaczWFS.GetActionForIssue(issue.issueWFS.WFSIssueId, UserId);
                    issueMove[trName].Add(issue.Idnumber, moves);
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, true);
                    issueMove[trName].Clear();
                }
            }
        }

        /// <summary>
        /// Callback z WFSForm
        /// </summary>
        /// <param name="zgloszenie"></param>
        /// <param name="tr"></param>
        private void AutoReturnIssue(BillingIssueDtoHelios zgloszenie, TreeView tr, bool isNew)
        {
            KeyValuePair<BillingIssueDto, IssueState> tmp = issues[tr.Name].Where(x => x.Key.Idnumber == zgloszenie.Idnumber).FirstOrDefault();
            issues[tr.Name].Remove(tmp.Key);
            issues[tr.Name].Add(zgloszenie, IssueState.READYTOSAVE);
            //AddToTree(tr);
        }

        /// <summary>
        /// Callback z WFSModelerForm
        /// </summary>
        /// <param name="issueid"></param>
        /// <param name="eventName"></param>
        /// <param name="tr"></param>
        private void ModelerFormActionFinish(List<string> issueid, string eventName, TreeView tr)
        {
            if (!issueMove.ContainsKey(tr.Name))
            {
                issueMove[tr.Name] = new Dictionary<string, Dictionary<int, string>>();
            }

            if (wmf != null)
                wmf.Close();
            GetActionForIssues(tr);

            foreach (string issueNumber in issueid)
            {
                foreach (TreeNode tn in tr.Nodes)
                {
                    string number = tn.Text.Split(' ').First();
                    if (issueNumber == number)
                        tn.Text = number + " " + eventName;

                }
            }

            if (lb_MultiList.Items.Count > 0)
            {
                Dictionary<int, string> actions = new Dictionary<int, string>();
                bool success = issueMove[tr.Name].TryGetValue(issueid[0], out actions);

                if (success)
                    multi_ShowButtons(actions);
            }

            // odświeżenie Szczegółów
            List<BillingIssueDtoHelios> list = new List<BillingIssueDtoHelios>();

            foreach (BillingIssueDtoHelios biss in wfsList[tr.Name])
            {
                if (tr.SelectedNode.Text.Contains(biss.issueHelios.number))
                {
                    list.Add(biss);

                    list = gujaczWFS.compareBillingWithWFS(list);


                    IssueState state = (list[0].isInWFS) ? IssueState.INWFS : IssueState.NEW;
                    list[0] = gujaczWFS.UpdateIssue(list[0]);
                    int imageIndex = GetImageIndex(list[0], state);
                    tr.SelectedNode.ImageIndex = imageIndex;
                    tr.SelectedNode.SelectedImageIndex = imageIndex;
                    treeView_AfterSelect(tr as Object, new TreeViewEventArgs(tr.SelectedNode));

                    break;
                }
            }
        }

        /// <summary>
        /// Callback z WFSModelerForm
        /// </summary>
        /// <param name="issueid"></param>
        /// <param name="eventName"></param>
        /// <param name="tr"></param>
        private void ModelerFormActionFinish(string issueid, string eventName, TreeView tr)
        { if (!issueMove.ContainsKey(tr.Name))
            {
                issueMove[tr.Name] = new Dictionary<string, Dictionary<int, string>>();
            }

            if (wmf != null)
                wmf.Close();
            GetActionForIssues(tr);
            cms_IssuePopup.Items.Clear();
            var tmp2 = issueMove[tr.Name][issueid].ToList();
            //if (tmp2.First().Value.Count!=0)
            foreach (var item in tmp2)
            {
                //cms_IssuePopup.Items.Add(item.Value + " (" + item.Key.ToString() + ")");
                ToolStripMenuItem m1 = new ToolStripMenuItem(item.Value + " (" + item.Key.ToString() + ")");
                m1.Click += new EventHandler(m1_Click);
                m1.Tag = item.Key.ToString();
                cms_IssuePopup.Items.Add(m1);
            }
            if (tr.SelectedNode != null)
            {
                tr.SelectedNode.Text = tr.SelectedNode.Text.Split(' ').First() + ' ' + eventName;
            }

            // odświeżenie Szczegółów
            List<BillingIssueDtoHelios> list = new List<BillingIssueDtoHelios>();

            foreach (BillingIssueDtoHelios biss in wfsList[tr.Name])
            {
                if (tr.SelectedNode.Text.Contains(biss.issueHelios.number))
                {
                    list.Add(biss);

                    list = gujaczWFS.compareBillingWithWFS(list);

                    IssueState state = (list[0].isInWFS) ? IssueState.INWFS : IssueState.NEW;

                    list[0] = gujaczWFS.UpdateIssue(list[0]);
                    int imageIndex = GetImageIndex(list[0], state);
                    tr.SelectedNode.ImageIndex = imageIndex;
                    tr.SelectedNode.SelectedImageIndex = imageIndex;
                    treeView_AfterSelect(tr as Object, new TreeViewEventArgs(tr.SelectedNode));

                    break;
                }
            }

        }

        /// <summary>
        /// Notify WebSerive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageToWebService_Tick(object sender, EventArgs e)
        {
            gujaczWFS.notifyWebService();
        }

        /// <summary>
        /// Rekurencyjne włączenie wszystkich kontrolek w kolekcji
        /// </summary>
        /// <param name="ctls"></param>
        /// <param name="enable"></param>
        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Enabled = enable;
                EnableControls(ctl.Controls, enable);
            }
        }

        /// <summary>
        /// Brak odwołania
        /// </summary>
        /// <param name="tn"></param>
        private void GetBillingDictionaries(TreeNode tn)
        {
            for (int i = 0; i < tn.Nodes.Count; i++)
            {
                Dictionary<int, string> dir = gujaczWFS.getBillingComponents(Convert.ToInt32(tn.Nodes[i].Name));
                foreach (KeyValuePair<int, string> d in dir)
                {
                    tn.Nodes[i].Nodes.Add(d.Key.ToString(), d.Value);
                }

                if (tn.Nodes[i].Nodes.Count > 0)
                {
                    GetBillingDictionaries(tn.Nodes[i]);
                }
            }
        }

        /// <summary>
        /// Po wciśnięciu klawisza F5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void odświeżToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issueTab.SelectedIndex == 0)
            {
                btn_NieprzydzieloneOdsiwez_Click(btn_NieprzydzieloneOdswiez, new EventArgs());
            }
            else
            {
                btn_MojeOdswiez_Click(btn_MojeOdswiez, new EventArgs());
            }
        }

        /// <summary>
        /// Formatuj datę na 0:dd.MM.yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private String formatDate(DateTime date)
        {
            return String.Format("{0:dd.MM.yyyy}", date);
        }

        /// <summary>
        /// Kliknięcie Zaloguj się
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zalogujSięToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login formLogin = new Login(gujaczWFS, ref helLIU, ref jiraUser, firstLogin);
            if (zaloguj(formLogin) && !isReadedComponents)
            {
                doByWorker(new DoWorkEventHandler(doGetComponents), null, new RunWorkerCompletedEventHandler(doGetComponentsCompleted));
                GetUsers();
            }
        }

        /// <summary>
        /// Wywołuje akcję w tle
        /// </summary>
        /// <param name="work"></param>
        /// <param name="argument"></param>
        /// <param name="reaction"></param>
        private void doByWorker(DoWorkEventHandler work, object argument, RunWorkerCompletedEventHandler reaction)
        {
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += work;
            this.backgroundWorker.RunWorkerCompleted += reaction;
            this.backgroundWorker.RunWorkerAsync(argument);
        }

        private void cb_EmailNotification_CheckedChanged(object sender, EventArgs e)
        {
            this.emailNotification = (sender as CheckBox).Checked;
        }

        private void btn_ZccLoadNotes_Click(object sender, EventArgs e)
        {
            TreeView tr = null;
            if (assignedIssues)
                tr = this.treeView2;
            else
                tr = this.treeView1;

            List<BillingIssueDto> issues = new List<BillingIssueDto>();

            if (tr.Nodes.Count > 0)
            {
                if (tr.Nodes.Count > 0)
                {
                    foreach (TreeNode item in tr.Nodes)
                    {
                        string text = item.Text.Split(' ')[0];
                        KeyValuePair<BillingIssueDto, IssueState> tmp = this.issues[tr.Name].Where(x => x.Key.Idnumber == item.Text.Split(' ').First()).FirstOrDefault();
                        if (!tmp.Equals(default(KeyValuePair<IssueDto, IssueState>)) && (tmp.Value == IssueState.INWFS || tmp.Value == IssueState.SELECTED))
                        {
                            BillingIssueDto iss = tmp.Key as BillingIssueDto;
                            issues.Add(iss);
                        }
                    }
                }
            }


            List<string> projects = new List<string>();


            try
            {
                string projectsXml = Logic.Implementation.XmlParser.ProjectsXML(projects).ToString();

                string issuesXml = Logic.Implementation.XmlParser.IssuesXML(issues).ToString();

                List<List<string>> output = gujaczWFS.ExecuteStoredProcedure("CPGetNotes", new string[] { issuesXml, projectsXml }, DatabaseName.SupportADDONS);

                List<Entities.Note> notes = Logic.Implementation.XmlParser.ReadNotesXML(output[0][0]);
                if (notes == null)
                    return;

                this.zcc_rtb.Clear();

                foreach (var item in notes)
                {
                    this.zcc_rtb.AppendText(item.issueNumber + "\n");
                    this.zcc_rtb.AppendText("Autor: " + item.author + " Data: " + item.date + "\n");
                    this.zcc_rtb.AppendText("Treść:\n");
                    this.zcc_rtb.AppendText(item.content + "\n");
                    this.zcc_rtb.AppendText("\n\n");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(ex);
                NoticeForm.ShowNotice("Wystąpił błąd w działaniu aplikacji. Szczegóły zostały zapisane do pliku logów. !", "Błąd");
            }
        }

        private void btn_ZCCRaport_Click(object sender, EventArgs e)
        {


            crm_rtb.Text = "Czekaj...";
            setBusy(true);
            try
            {
                DateTime now = dtp_ZCCDataDo.Value;
                DateTime yesterday = dtp_ZCCDataOd.Value;

                string dateFrom = yesterday.ToShortDateString();
                string dateTo = now.ToShortDateString();

                string timeFrom = tb_CRMRapGodzinaOd.Text;
                string timeTo = tb_CRMRapGodzinaDo.Text;

                DateTime dtFrom;
                DateTime dtTo;

                try
                {
                    dtFrom = DateTime.Parse(dateFrom + " 00:00:00");
                    dtTo = DateTime.Parse(dateTo + " 00:00:00");
                }
                catch (Exception ex)
                {
                    NoticeForm.ShowNotice("Błędnie uzupełnione dane formularza!");
                    return;
                }

                List<List<string>> list = gujaczWFS.ExecuteStoredProcedure("CPRaportZCC", new string[] { dateFrom.ToString(), dateTo.ToString() }, DatabaseName.SupportADDONS);
                string xmlIn = "";
                foreach (var item in list)
                {
                    xmlIn += item[0];
                }
                StringBuilder result = Logic.Implementation.XmlParser.ZCCRaportFromXML(xmlIn);
                zcc_rtb.Clear();
                zcc_rtb.AppendText("Witam,\n");
                zcc_rtb.AppendText("przesyłam raport za okres od " + dtFrom.ToShortDateString() + " do " + dtTo.ToShortDateString() + "\n\n");
                zcc_rtb.AppendText(result.ToString());
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
            setBusy(false);
        }

        private void cb_SoundNotification_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.czyDzwiek = cb_SoundNotification.Checked;
            Properties.Settings.Default.Save();
        }


        //public ReportViewer raportRSR;

        private void btn_ZCCPodsumowanie_Click(object sender, EventArgs e)
        {
            //ReportViewer r = new ReportViewer();

            //raportRSR.HyperlinkTarget = "http://salsa/Reports_SQL2012/Pages/Report.aspx?ItemPath=%2fSupport%2fCP%2fRaportDzienny";

            zcc_rtb.Clear();
            DateTime yesterday = dtp_ZCCDataOd.Value;

            string dateFrom = yesterday.ToShortDateString();

            DateTime dtFrom;

            try
            {
                dtFrom = DateTime.Parse(dateFrom + " 00:00:00");
            }
            catch (Exception ex)
            {
                NoticeForm.ShowNotice("Błędnie uzupełnione dane formularza!");
                return;
            }
            zcc_rtb.AppendText("Raport za " + dtFrom.ToShortDateString() + "\n");
            zcc_rtb.AppendText(Logic.Implementation.XmlParser.ZCCRaportDziennyFromXML(gujaczWFS.ExecuteStoredProcedure("CPRaportDziennyZCC", new string[] { dtFrom.ToString() }, DatabaseName.SupportADDONS)[0][0]).ToString());

            TreeView tr = this.treeView1;

            List<BillingIssueDto> issues = new List<BillingIssueDto>();

            if (tr.Nodes.Count > 0)
            {
                if (tr.Nodes.Count > 0)
                {
                    foreach (TreeNode item in tr.Nodes)
                    {
                        string text = item.Text.Split(' ')[0];
                        KeyValuePair<BillingIssueDto, IssueState> tmp = this.issues[tr.Name].Where(x => x.Key.Idnumber == item.Text.Split(' ').First()).FirstOrDefault();
                        if (!tmp.Equals(default(KeyValuePair<IssueDto, IssueState>)) && (tmp.Value == IssueState.INWFS || tmp.Value == IssueState.SELECTED))
                        {
                            BillingIssueDto iss = tmp.Key as BillingIssueDto;
                            issues.Add(iss);
                        }
                    }
                }
            }

            List<string> projects = new List<string>();


            string projectsXml = Logic.Implementation.XmlParser.ProjectsXML(projects).ToString();

            string issuesXml = Logic.Implementation.XmlParser.IssuesXML(issues).ToString();

            List<List<string>> output = gujaczWFS.ExecuteStoredProcedure("CPGetNotes", new string[] { issuesXml, projectsXml }, DatabaseName.SupportADDONS);

            List<Entities.Note> notes = Logic.Implementation.XmlParser.ReadNotesXML(output[0][0]);

            if (notes != null && notes.Count > 0)
            {
                zcc_rtb.AppendText("Zgłoszenia do realizacji:\n");

                foreach (var item in notes)
                {
                    string content = item.content;
                    string[] lines = content.Split('\n');
                    if (string.IsNullOrWhiteSpace(lines[0]))
                        continue;

                    zcc_rtb.AppendText("\n");
                    zcc_rtb.AppendText("http://jira/browse/" + item.issueNumber + "\n");
                    zcc_rtb.AppendText(item.content + "\n");
                }
            }


            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KeyValuePair<BillingIssueDto, IssueState> tmp = issues[treeView1.Name].Where(x => x.Key.Idnumber == treeView1.SelectedNode.Text.Split(' ').First()).FirstOrDefault();
            if (tmp.Equals(default(KeyValuePair<IssueDto, IssueState>)))
                return;

            BillingIssueDtoHelios it = tmp.Key as BillingIssueDtoHelios;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Wave Form Audio Format (.wav)|*.wav";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tb_SoundNotificationPath.Text = dialog.FileName;
                Properties.Settings.Default.dzwiekSciezka = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void searchTab_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                ListView v = sender as ListView;
                if (v.SelectedItems.Count > 0)
                {
                    ListViewItem i = v.SelectedItems[0];
                    Clipboard.SetText(i.Text);
                }
            }
        }

        private void btn_NewIssue_Click(object sender, EventArgs e)
        {
            CreateNewIssue();
        }

        private void btn_BillenniumOdswiez_Click(object sender, EventArgs e)
        {
            treeView3.Nodes.Clear();
            if (!issues.ContainsKey(treeView3.Name))
            {
                issues[treeView3.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }

            GetIssuesFromBPM(treeView3);
        }

        private void GetIssuesFromBPM(TreeView tr)
        {
            DisableIssuesButtons();
            pb_SetVisibilityPanel(true);
            Thread thr = new Thread((ThreadStart)delegate ()
            {
                issues[tr.Name].Clear();
                try
                {

                    pb_SetMaxProgressBar(3);
                    pb_UpdateProgressBar("Rozpoczynanie pracy");

                    pb_UpdateProgressBar("Wczytywanie danych z Jira");
                    List<BillingIssueDtoHelios> issue;

                    // Pobierz zgłoszenia za pomocą procki Pawła
                    issue = gujaczWFS.GetIssuesFromBPM(gujaczWFS.getUser().Id);

                    pb_SetMaxProgressBar(issue.Count + 2);
                    pb_UpdateProgressBar("Wczytanie danych z Jira");

                    if (!wfsList.ContainsKey(tr.Name))
                    {
                        wfsList[tr.Name] = new List<BillingIssueDtoHelios>();
                    }

                    wfsList[tr.Name] = gujaczWFS.compareBillingWithWFS(issue);

                    pb_UpdateProgressBar("Porównanie z BPM");
                    foreach (BillingIssueDtoHelios item in wfsList[tr.Name])
                    {
                        BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);

                        IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                        issues[tr.Name].Add(updatedIssue, state);
                        pb_UpdateProgressBar("Analiza zgłoszenia:" + updatedIssue.Idnumber);
                    }

                    t_UpdateLiczbaZgloszen(issues["treeView1"].Count, issues["treeView2"].Count);
                    GetActionForIssues(tr);
                    AddToTree(tr);
                    pb_SetVisibilityPanel(false);
                    //Włączenie przycisków do pobierania zgłoszeń
                    this.Invoke((MethodInvoker)delegate
                    {
                        EnableIssuesButtons();
                        if (pauza && checkIssues)
                        {
                            timer2.Start();
                            pauza = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                }
            });

            btn_MojeZapiszDoWFS.Enabled = true;
            btn_NieprzydzieloneZapiszDoWFS.Enabled = true;
            btn_BillenniumZapisz.Enabled = true;
            thr.IsBackground = true;
            thr.Start();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            if (!issues.ContainsKey(treeView1.Name))
            {
                issues[treeView1.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }
            if (!issues.ContainsKey(treeView2.Name))
            {
                issues[treeView2.Name] = new Dictionary<BillingIssueDto, IssueState>();
            }

            GetAllIssuesFromJira();
        }

        private void cbox_RefreshBoth_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.czyObaDrzewa = (sender as CheckBox).Checked;
            Properties.Settings.Default.Save();
        }

        private void btn_SaveFilters_Click(object sender, EventArgs e)
        {
            string assignedFilterName = tb_AssignedFilterName.Text;
            string unassignedFilterName = tb_UnassignedFilterName.Text;

            Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
            List<string> usersFilters = jIssues.GetFiltersAsync();
            if (!string.IsNullOrEmpty(assignedFilterName))
            {
                if (usersFilters.Contains(assignedFilterName))
                    Properties.Settings.Default.assignedFilterName = assignedFilterName;
                else
                {
                    NoticeForm.ShowNotice(string.Format("Brak filtru {0} w Jira!", assignedFilterName));
                    return;
                }
            }
            else
                Properties.Settings.Default.assignedFilterName = assignedFilterName;

            if (!string.IsNullOrEmpty(unassignedFilterName))
            {
                if (usersFilters.Contains(unassignedFilterName))
                    Properties.Settings.Default.unassignedFilterName = unassignedFilterName;
                else
                {
                    NoticeForm.ShowNotice(string.Format("Brak filtru {0} w Jira!", unassignedFilterName));
                    return;
                }
            }
            else
                Properties.Settings.Default.unassignedFilterName = unassignedFilterName;

            Properties.Settings.Default.Save();
        }

        private bool isNullObjectOrEmptyString(object _object)
        {
            bool returnValue = false;

            if (_object == null)
            {
                returnValue = true;
            }
            else if ((_object as string) == string.Empty)
            {
                returnValue = true;
            }


            return returnValue;
        }


        public bool checkBillUser(string loginJira, LoginParamType paramType)
        {
            foreach (var item in userBpmJira)
            {
                if (paramType == LoginParamType.Login && item.UserBpm.login.Contains(loginJira))
                {
                    return true;
                }

                if (paramType == LoginParamType.FullName && item.UserBpm.FullName.Contains(loginJira))
                {
                    return true;
                }
            }
           
            return false;
        }

        public UserBpmJira getUserBpmJira(string loginJira, LoginParamType paramType = 0)
        {
            UserBpmJira returnUser = null;

            foreach (var item in userBpmJira)
            {
                if (paramType == LoginParamType.Login && item.UserBpm.login.Contains(loginJira))
                {
                    returnUser = item;
                    break;
                }
            }

            return returnUser;
        }

        public bool getUserBpmJira(string loginJira, out UserBpmJira returnUser, LoginParamType paramType = 0)
        {
            bool returnValue = false;
            UserBpmJira _returnUser = new UserBpmJira();

            foreach (var item in userBpmJira)
            {
                if (paramType == LoginParamType.Login && item.UserBpm.login.Contains(loginJira))
                {
                    _returnUser = item;
                    returnValue = true;
                    break;
                }
            }

            returnUser = _returnUser;
            return returnValue;
        }


        private void addIssueToTreeNode(string issueNumber, List<BillingIssueDtoHelios> issue, string treeViewName = "treeView4")
        {
            Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");
            List<Entities.BillingIssueDtoHelios> IssueHelios;

            //treeView4.Nodes.Clear();
            //issues[treeViewName].Clear();


            //List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();

            issue.Add(new BillingIssueDtoHelios()
            {
                issueHelios = new IssueHelios()
                {
                    number = issueNumber
                }
            }
            );

            if (!wfsList.ContainsKey(treeViewName))
            {
                wfsList[treeViewName] = new List<BillingIssueDtoHelios>();
            }

            wfsList[treeViewName] = gujaczWFS.compareBillingWithWFS(issue);


            foreach (BillingIssueDtoHelios item in wfsList[treeViewName])
            {
                BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);
                IssueState state = (updatedIssue.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                

                BillingIssueDtoHelios tmp2 = gujaczWFS.UpdateJiraInfo(updatedIssue);
                issues[treeViewName].Add(tmp2, state);
            }

            for (int i = 0; i < wfsList[treeViewName].Count; i++)
            {
                
                wfsList[treeViewName][i] = gujaczWFS.UpdateJiraInfo(wfsList[treeViewName][i]);

            }
            

            //jIssues.ChangeDataModel(issues[treeViewName], out IssueHelios, ref JiraUsers);

            object treeByName = Controls.Find(treeViewName, true)[0];
            GetActionForIssues((TreeView)treeByName);
            AddToTree((TreeView)treeByName);

            KeyValuePair<BillingIssueDto, IssueState> tmp = issues[treeViewName].Where(x =>
            {
                if (x.Key.Idnumber == issueNumber)
                    return true;
                else
                    return false;
            }).FirstOrDefault();

            if(!selectIssueList.Any(x => x.Key == issueNumber))
                selectIssueList.Add(issueNumber, tmp.Key);
            selectIssue = tmp.Key;
        }


        private void getActionToIssue(List<BillingIssueDtoHelios> issue, string issueNumber, string treeViewName = "treeView4", UserBpmJira ubj = null)
        {
            ToolStripMenuItem m1 = new ToolStripMenuItem();
            ToolStripMenuItem m2 = new ToolStripMenuItem();

            if (issueMove[treeViewName] != null && issueMove[treeViewName].ContainsKey(issueNumber) && selectIssue != null)
            {

                cms_IssuePopup.Items.Clear();

                Dictionary<int, string> actions = new Dictionary<int, string>();
                bool success = issueMove[treeViewName].TryGetValue(issueNumber, out actions);
                if (success)
                {
                    var actionsList = actions.ToList();


                    m1 = null;
                    m2 = null;// new ToolStripMenuItem();

                    m1 = new ToolStripMenuItem(issueNumber + " - " + issue[0].issueWFS.WFSState.ToString());
                    m1.Tag = issueNumber.ToString();
                    m1.Enabled = false;


                    cms_IssuePopup.Items.Add(m1);

                    foreach (KeyValuePair<int, string> item in actionsList)
                    {

                        List<object> tagList = new List<object>();

                        KeyValuePair<int, string> s = new KeyValuePair<int, string>();
                        //BillingIssueDto, KeyValuePair<int, string>> tagTmp = new KeyValuePair<BillingIssueDto, KeyValuePair<int, string>>(, s);
                        tagList.Add(selectIssueList[issueNumber]);   //1. BillingIssueDto            selectIssue
                        tagList.Add(item);          //2. KeyValuePair<int, string>  item
                        tagList.Add(s);             //3. KeyValuePair<int, string>  s


                        m1 = new ToolStripMenuItem(item.Value + " (" + item.Key.ToString() + ")");
                        m1.Tag = item.Key.ToString();

                        if (       item.Value.ToString().Equals("Weryfikacja zgłoszenia")
                                || item.Value.ToString().Equals("Weryfikacja negatywna, ponowienie diagnozy")
                                || item.Value.ToString().Equals("Zamknij zgłoszenie")
                                || item.Value.ToString().Equals("Odebranie z konsultacji")
                                || item.Value.ToString().Equals("Weryfikacja po wdrożeniu")
                            )
                        {
                            m1.Tag = tagList;
                            m1.Click += new EventHandler(btn_tmpQuickStep_Click);
                        }
                        else if (item.Value.ToString().Equals("Rozpoczęcie diagnozy")
                            )
                        {
                            if(!isNullObjectOrEmptyString(ubj))
                                tagList[2] = new KeyValuePair<int, string>(ubj.UserBpm.Id, ubj.UserBpm.FullName);
                            m1.Tag = tagList;
                            m1.Click += new EventHandler(btn_tmpQuickStep_Click);
                        }
                        else if (item.Value.ToString().Equals("Diagnoza zgłoszenia")
    )
                        {
                            if (!isNullObjectOrEmptyString(ubj))
                                tagList[2] = new KeyValuePair<int, string>(ubj.UserBpm.Id, ubj.UserBpm.FullName);
                            m1.Tag = tagList;
                            m1.Click += new EventHandler(btn_tmpQuickStep_Click);
                        }
                        else
                        {
                            m1.Tag = tagList;
                            m1.Click += new EventHandler(m2_Click);
                        }

                        if (item.Value.ToString().Equals("Zamknięcie zgłoszenia"))
                        {
                            Dictionary<int, string> propRozw = gujaczWFS.getBillingComponents(-2);

                            foreach (KeyValuePair<int, string> st in propRozw)
                            {
                                m2 = new ToolStripMenuItem(st.Value);

                                m2.Click += new EventHandler(btn_tmpQuickStep_Click);
                                tagList.Clear();

                                //BillingIssueDto, KeyValuePair<int, string>> tagTmp = new KeyValuePair<BillingIssueDto, KeyValuePair<int, string>>(, s);
                                tagList.Add(selectIssueList[issueNumber]);   //1. BillingIssueDto            selectIssue
                                tagList.Add(item);          //2. KeyValuePair<int, string>  item
                                tagList.Add(st);             //3. KeyValuePair<int, string>  s

                                m2.Tag = tagList;

                                m1.DropDownItems.Add(m2);
                            }

                        }
                        cms_IssuePopup.Items.Add(m1);
                    }

                    if (actions.Count == 0)
                    {
                        if (selectIssue.issueWFS.WFSState.Equals("Zgłoszenie zamknięte", StringComparison.CurrentCultureIgnoreCase))
                        {
                            m1 = new ToolStripMenuItem("Ponowne otwarcie zgłoszenia");
                            m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem4_Click);
                            cms_IssuePopup.Items.Add(m1);
                        }
                        else
                        {
                            m1 = new ToolStripMenuItem("Uzupełnij dane");
                            m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem4_Click);
                            cms_IssuePopup.Items.Add(m1);
                        }
                    }
                    else if (selectIssue.issueWFS.WFSState.Equals("Diagnoza zgłoszenia"))
                    {
                        //cms_IssuePopup.Items["Przyjęcie do realizacji"].

                        m1 = new ToolStripMenuItem("Zamknij po realizacji");
                        m1.Click += new EventHandler(uzupełnijDaneToolStripMenuItem4_Click);
                        //cms_IssuePopup.Items["Diagnoza zgłoszenia"].Add(m1);
                    }

                }
            }

            MainForm mf = this as MainForm;
            if (mf != null && mf.Tag != null)
            {
                if ((bool)mf.Tag == true)
                {
                    btn_tmpQuickStep_Click(m1, null);
                    //m1_Click(m1, null);
                    this.Tag = null;
                }
            }
            else
                cms_IssuePopup.Show(Cursor.Position);
        }

        /// <summary>
        /// Context Menu zgłoszenia w SLA - Obsługa procesu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmp = (ToolStripMenuItem)sender; //losowy komponet do przechowywania Tag

            List<object> tagTmp = new List<object>();
            if (tmp.Tag != null && tmp.Tag.GetType() == typeof(List<object>))
            {
                tagTmp = (List<object>)tmp.Tag;
            }

            //1. BillingIssueDto            selectIssue
            //2. Dictionary<int, string>    item
            //3. KeyValuePair<int, string>  s

            BillingIssueDto selectIssue = (BillingIssueDto)tagTmp[0];
            KeyValuePair<int, string> item = (KeyValuePair<int, string>)tagTmp[1];
            KeyValuePair<int, string> selectOption = (KeyValuePair<int, string>)tagTmp[2];


            string p = item.Value;
            int eventMoveId = item.Key;

            TreeView tr = null;

            ToolStripDropDownItem ddi = sender as ToolStripDropDownItem;
            if (ddi == null)
            {
                return;
            }
            ContextMenuStrip cms = ddi.Owner as ContextMenuStrip;
            if (cms == null)
            {
                return;
            }

            //Pobranie Rozmieszczenia parametrów zdarzenia
            List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);
            wmf = new WFSModelerForm(eventParamForFormByEventMove, sender.ToString(), selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4);

            try
            {
                if (!wmf.IsDisposed)
                    wmf.ShowDialog();

                if (wmf.DialogResult == DialogResult.OK && eventMoveId == 614 && (jira.GetIssue(selectIssue.issueWFS.JiraId).Assignee == string.Empty 
                    || jira.GetIssue(selectIssue.issueWFS.JiraId).Assignee == "billennium" ))
                       jira.GetIssue(selectIssue.issueWFS.JiraId).Assignee = "prekawek";
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
        }

        /// <summary>
        /// Context Menu zgłoszenia - Uzupełnij dane (BILLENNIUM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uzupełnijDaneToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            btn_tmpUzupelnijDane_Click(null, null);
        }

        /// <summary>
        /// btn_BillenniumUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tmpUzupelnijDane_Click(object sender, EventArgs e)
        {
            UzupelnijDane(treeView4);
        }


        /// <summary>
        /// btn_BillenniumUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tmpQuickStep_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmp = (ToolStripMenuItem)sender; //losowy komponet do przechowywania Tag


            List<object> tagTmp = new List<object>();
            if(tmp.Tag != null && tmp.Tag.GetType() == typeof(List<object>))
            {
                tagTmp = (List<object>)tmp.Tag;
            }
            try
            {
                //1. BillingIssueDto            selectIssue
                //2. Dictionary<int, string>    item
                //3. KeyValuePair<int, string>  s

                BillingIssueDto selectIssue = (BillingIssueDto)tagTmp[0];
                KeyValuePair<int, string> item = (KeyValuePair<int, string>)tagTmp[1];
                KeyValuePair<int, string> selectOption = (KeyValuePair<int, string>)tagTmp[2];

                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(item.Key);

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, item.Value, selectIssue, gujaczWFS, item.Key, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, selectOption);
            }
            catch(Exception ex)
            { }
        }
        /// <summary>
        /// Callback z WFSModelerForm
        /// </summary>
        /// <param name="issueid"></param>
        /// <param name="eventName"></param>
        /// <param name="tr"></param>
        private void ModelerForm_sla_ActionFinish(string issueid, string eventName, TreeView tr)
        {
            if (!issueMove.ContainsKey(tr.Name))
            {
                issueMove[tr.Name] = new Dictionary<string, Dictionary<int, string>>();
            }

            if (wmf != null)
                wmf.Close();
            GetActionForIssues(tr);
            cms_IssuePopup.Items.Clear();
            var tmp2 = issueMove[tr.Name][issueid].ToList();
            //if (tmp2.First().Value.Count!=0)
            foreach (var item in tmp2)
            {
                //cms_IssuePopup.Items.Add(item.Value + " (" + item.Key.ToString() + ")");
                ToolStripMenuItem m1 = new ToolStripMenuItem(item.Value + " (" + item.Key.ToString() + ")");
                m1.Click += new EventHandler(m1_Click);
                m1.Tag = item.Key.ToString();
                cms_IssuePopup.Items.Add(m1);
            }
            if (tr.SelectedNode != null)
            {
                tr.SelectedNode.Text = tr.SelectedNode.Text.Split(' ').First() + ' ' + eventName;
            }

            //zmiana opisu w ostanim kroku
            //dgv_SlaRaport.CurrentRow.Cells["dgvAkcjaBPM"].Value = eventName;
            //dgv_SlaRaport.CurrentRow.Cells["dgvOdpowiedzialny"].Value = toolStripDropDownButton1.Text;
            //var v = this.Tag.GetType();
            //autoCheck = //(bool)(this.Tag) == null ? false : true;
            //bool autoCheck = true;
            //if (autoCheck)
            //{
            //    //if ((bool)this.Tag == true)
            //    {
            foreach (DataGridViewRow item in dgv_SlaRaport.Rows)
            {
                if (item.Cells["dgvJiraNr"].Value == issueid)
                {
                    
                    UserBpmJira ubj;
                    item.Cells["dgvAkcjaBPM"].Value = eventName;
                    

                    if (item.Cells["dgvAktualniePrzydzielony"].Value != null 
                        && getUserBpmJira(item.Cells["dgvAktualniePrzydzielony"].Value.ToString(), out ubj, LoginParamType.Login))
                    {
                        item.Cells["dgvOdpowiedzialny"].Value = ubj.UserJira.FullName;

                    }
                    else
                    {
                        item.Cells["dgvOdpowiedzialny"].Value = gujaczWFS.getUser().FullName;
                    }
                    break;
                }
            }
            //    }
            //}
            //CurrentRow.Cells["dgvAkcjaBPM"].Value = eventName;

        }
         

        #region Powiadomienia SLA
        private void t_EmailNotification_Tick(object sender, EventArgs e)
        {
            string sendEmail = gujaczWFS.ExecuteStoredProcedure("sp_SendEmail", new string[] { }, DatabaseName.SupportCP)[0][0];

            //XmlDocument xmlUser = Properties.Settings.Default.Users;


            if (sendEmail == "1")
            {
                try
                {
                    Thread thr = new Thread((ThreadStart)delegate
                    {
                        List<List<string>> lista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport", new string[] { }, DatabaseName.SupportCP).Where(x => Int32.Parse(x[5]) == 0).ToList();

                        if (lista.Count > 0)
                        {
                            HtmlTable table = new HtmlTable();
                            table.AddColumn("Zgłoszenie");
                            table.AddColumn("Osoba odpowiedzialna");
                            table.AddColumn("Pozostały czas (minuty)");


                            foreach (var s in lista)
                            {
                                if (s[8] != string.Empty && Int32.Parse(s[8]) < 60)
                                {
                                    HtmlHyperlink link = new HtmlHyperlink(string.Format("http://jira/browse/{0}", s[1]), s[1]);
                                    table.AddRow(new string[] { link.ToString(), s[2], s[8] });
                                }
                            }
                            if (table.Rows.Count() > 0)
                            {
                                HtmlContent content = new HtmlContent(new HtmlHeader("Zgłoszenia z pozostałym czasem na realizację poniżej jednej godziny:"), table);
                                ExchangeClient exClient = new ExchangeClient(gujaczWFS.getUser());
                                string recipants = ConfigurationManager.AppSettings["SlaRecipants"];
                                exClient.SendEMail("Kończy się czas na realizację zgłoszeń!", content, recipants);

                                gujaczWFS.ExecuteStoredProcedure("sp_UpdateLastEmailSend", new string[] { }, DatabaseName.SupportCP);

                            }
                        }
                    });

                    thr.IsBackground = true;
                    thr.Start();
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, true);
                    NoticeForm.ShowNotice(ex.Message);
                }
            }
        }

        private void tbn_CRMpodsumowanieWyslij_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thr = new Thread((ThreadStart)delegate
                {

                    //if (crm_rtb.Text.ToString() != "" )
                    //{
                    //HtmlTable table = new HtmlTable();
                    //table.AddColumn("Zgłoszenie");
                    //table.AddColumn("Pozostały czas (minuty)");


                    //foreach (var s in lista)
                    //{
                    //    if (s[8] != string.Empty && Int32.Parse(s[8]) < 60)
                    //    {
                    //        HtmlHyperlink link = new HtmlHyperlink(string.Format("http://jira/browse/{0}", s[1]), s[1]);
                    //        table.AddRow(new string[] { link.ToString(), s[8] });
                    //    }
                    //}
                    //if (table.Rows.Count() > 0)
                    //{
                    HtmlContent content = new HtmlContent(new HtmlHeader("Zgłoszenia z pozostałym czasem na realizację poniżej jednej godziny:"));
                    ExchangeClient exClient = new ExchangeClient(gujaczWFS.getUser());
                    string recipants = ConfigurationManager.AppSettings["SlaRecipants"];
                    exClient.SendEMail("Podsumowanie zmiany", content, recipants);

                    HtmlDocument html; //= new HtmlDocument();
                   
                    //}
                    //}
                });

                thr.IsBackground = true;
                thr.Start();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                NoticeForm.ShowNotice(ex.Message);
            }
        }

        #endregion

        private void button1_Click_3(object sender, EventArgs e)
        {
            Jira jiraSlaSynch;
            jiraSlaSynch = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);

            int i = 0;
            for (int j = 0; i < 3000; j++)
            {

                //IEnumerable<Issue> issues = jiraSlaSynch.GetIssuesFromJql("issue in ('CRM-73224','CRM-73225','CRM-73226','CRM-73228','CRM-73229','CRM-73231','CRM-73234','CRM-73235','CRM-73236','CRM-73238','CRM-73239','CRM-73240','CRM-73243','CRM-73245','CRM-73246','CRM-73250','CRM-73252','CRM-73255','CRM-73256','CRM-73257','CRM-73259','CRM-73263','CRM-73264','CRM-73265','CRM-73267','CRM-73269','CRM-73270','CRM-73275','CRM-73276','CRM-73277','CRM-73279','CRM-73281','CRM-73282','CRM-73283','CRM-73286','CRM-73288','CRM-73290','CRM-73291','CRM-73292','CRM-73293','CRM-73294','CRM-73295','CRM-73296','CRM-73297','CRM-73298','CRM-73300','CRM-73301','CRM-73305','CRM-73306','CRM-73308','CRM-73311','CRM-73312','CRM-73313','CRM-73315','CRM-73316','CRM-73317','CRM-73322','CRM-73323','CRM-73325','CRM-73326','CRM-73329','CRM-73330','CRM-73331','CRM-73336','CRM-73337','CRM-73339','CRM-73340','CRM-73341','CRM-73345','CRM-73346','CRM-73347','CRM-73348','CRM-73352','CRM-73353','CRM-73354','CRM-73355','CRM-73359','CRM-73361','CRM-73365','CRM-73366','CRM-73369','CRM-73371','CRM-73374','CRM-73376','CRM-73377','CRM-73378','CRM-73379','CRM-73382','CRM-73383','CRM-73384','CRM-73385','CRM-73388','CRM-73389','CRM-73390','CRM-73392','CRM-73393','CRM-73394','CRM-73395','CRM-73400','CRM-73402','CRM-73404','CRM-73405','CRM-73406','CRM-73407','CRM-73409','CRM-73418','CRM-73444','CRM-73453','CRM-73481','CRM-73491','CRM-73501','CRM-73508','CRM-73509','CRM-73519','CRM-73539','CRM-73564','CRM-73625','CRM-73734','CRM-73886','CRM-73938','CRM-74122','CSLM-14131','CSLM-14193','CSLM-14330','CSLM-14368','CSLM-14508','CSLM-14542','CSLM-14588','CSLM-14690','CSLM-14691','CSLM-14720','CSLM-14926','CSS-10587','CSS-10588','CSS-10646','DWH-1701','EAI-12643','EAI-12681','EAI-12716','EAI-12728','EAI-12738','EAI-12755','EAI-12887','EAI-12888','EAI-12889','EAI-12927','EAI-12964','EAI-13002','EAI-13005','EAI-13068','EAI-13567','EARCH-921','HD2-5979','HD2-5997','HD2-6011','HD2-6027','HD2-6043','HD2-6047','HD2-6051','INFOLINIA-5805','INFOLINIA-5806','INFOLINIA-5811','INFOLINIA-5812','INFOLINIA-5813','INFOLINIA-5815','INFOLINIA-5817','INFOLINIA-5818','INFOLINIA-5821','INFOLINIA-5830','INFOLINIA-5833','INFOLINIA-5836','INFOLINIA-5841','INFOLINIA-5843','INFOLINIA-5844','INFOLINIA-5846','INFOLINIA-5847','INFOLINIA-5848','INFOLINIA-5851','INFOLINIA-5856','INFOLINIA-5859','INFOLINIA-5861','INFOLINIA-5862','INFOLINIA-5863','INFOLINIA-5864','INFOLINIA-5873','INFOLINIA-5878','INFOLINIA-5889','INFOLINIA-5890','INFOLINIA-5895','INFOLINIA-5901','INFOLINIA-5903','INFOLINIA-5905','INFOLINIA-5907','INFOLINIA-5908','INFOLINIA-5909','INFOLINIA-5911','INFOLINIA-5920','INFOLINIA-5922','INFOLINIA-5924','INFOLINIA-5928','INFOLINIA-5933','INFOLINIA-5934','INFOLINIA-5940','INFOLINIA-5941','INFOLINIA-5945','INFOLINIA-5946','INFOLINIA-5948','INFOLINIA-5949','INFOLINIA-5950','INFOLINIA-5958','INFOLINIA-5963','INFOLINIA-5969','INFOLINIA-5970','INFOLINIA-5972','INFOLINIA-5976','INFOLINIA-5979','INFOLINIA-5983','INFOLINIA-5984','INFOLINIA-5985','INFOLINIA-5988','INFOLINIA-5989','INFOLINIA-5990','INFOLINIA-5992','INFOLINIA-6000','INFOLINIA-6001','INFOLINIA-6004','INFOLINIA-6005','INFOLINIA-6006','INFOLINIA-6007','INFOLINIA-6008','INFOLINIA-6009','INFOLINIA-6010','INFOLINIA-6020','INFOLINIA-6021','INFOLINIA-6022','INFOLINIA-6024','INFOLINIA-6025','INFOLINIA-6029','INFOLINIA-6030','INFOLINIA-6031','INFOLINIA-6033','INFOLINIA-6034','INFOLINIA-6035','INFOLINIA-6038','INFOLINIA-6039','INFOLINIA-6040','INFOLINIA-6046','INFOLINIA-6052','INFOLINIA-6061','KK-1608','KKK-3198','KKK-3202','KKK-3205','KKK-3209','KKK-3210','KKK-3211','KKK-3212','KKK-3213','KKK-3214','KKK-3218','KKK-3219','KKK-3220','KKK-3223','KKK-3225','KKK-3227','KKK-3228','KKK-3236','KKK-3241','KKK-3244','KKK-3246','KKK-3248','KKK-3251','KKK-3255','KKK-3257','KKK-3258','KKK-3260','KKK-3263','KKK-3271','KKK-3276','KKK-3277','KKK-3280','KKK-3285','KKK-3303','KKK-3304','KKK-3305','KKK-3307','KKK-3308','KKK-3310','KKK-3315','KKK-3319','KKK-3320','KKK-3321','KKK-3322','KKK-3323','KKK-3324','KKK-3325','KKK-3327','KKK-3331','KKK-3332','KKK-3334','KKK-3335','KKK-3336','KKK-3338','KKK-3342','KKK-3343','KKK-3345','KKK-3349','KKK-3352','KKK-3354','KKK-3355','KKK-3356','KKK-3357','KKK-3358','KKK-3361','KKK-3363','KKK-3364','KKK-3365','KKK-3366','KKK-3368','KKK-3369','KKK-3370','KKK-3371','KKK-3372','KKK-3373','KKK-3375','KKK-3378','KKK-3380','KKK-3381','KKK-3382','KKK-3384','KKK-3385','KKK-3386','KKK-3387','KKK-3391','KKK-3393','KKK-3394','KKK-3399','KKK-3402','KKK-3403','KKK-3404','KKK-3405','KKK-3407','KKK-3408','KKK-3409','KKK-3410','KKK-3411','KKK-3412','KKK-3413','KKK-3414','KKK-3416','KKK-3417','KKK-3421','KKK-3423','KKK-3425','KKK-3427','KKK-3428','KKK-3430','KKK-3431','KKK-3432','KKK-3434','KKK-3435','KKK-3438','KKK-3442','KKK-3443','KKK-3450','KKK-3452','KKK-3454','KKK-3455','KKK-3457','KKK-3458','KKK-3459','KKK-3460','KKK-3461','KKK-3464','KKK-3465','KKK-3467','KKK-3468','KKK-3469','KKK-3471','KKK-3472','KKK-3474','KKK-3477','KKK-3478','KKK-3484','KKK-3485','KKK-3486','KKK-3487','KKK-3488','KKK-3489','KKK-3493','KKK-3494','KKK-3495','KKK-3497','KKK-3501','KKK-3502','KKK-3508','KKK-3509','KKK-3518','KKK-3539','KKK-3553','KKK-3570','KKK-3701','KMVNO-3600','KMVNO-3607','KMVNO-3610','KMVNO-3611','KMVNO-3616','KMVNO-3639','KMVNO-3640','KMVNO-3651','KMVNO-3652','KMVNO-3653','KMVNO-3654','KMVNO-3656','KMVNO-3657','KMVNO-3658','KMVNO-3664','KMVNO-3667','KMVNO-3674','KMVNO-3675','KMVNO-3684','KMVNO-3687','KMVNO-3699','KMVNO-3700','KMVNO-3705','KMVNO-3708','KMVNO-3714','KMVNO-3717','KMVNO-3721','KMVNO-3722','KMVNO-3726','KMVNO-3744','KMVNO-3749','KPW-667','LAWA-284','LIDKA-42','LIDKA-43','LIDKA-44','LIDKA-45','LIDKA-46','LIDKA-47','LIDKA-49','LIDKA-52','LIDKA-54','LIDKA-55','LIDKA-58','LIDKA-60','LIDKA-62','LIDKA-63','LIDKA-64','LIDKA-65','LIDKA-66','LIDKA-69','LIDKA-71','LIDKA-72','LIDKA-73','LIDKA-75','LIDKA-76','LIDKA-77','LIDKA-78','MAILGEN-31','MAILGEN-32','MBS-6601','MBS-6608','MBS-6611','MBS-6639','MBS-6667','MBS-6708','MBS-6744','MBS-6763','MBS-6768','MBS-6819','MBS-6860','MBS-6877','MBS-6878','MBS-6893','MBS-7033','MBS-7040','MBS-7044','MBS-7053','MBS-7059','MBS-7062','MBS-7066','MBS-7093','MBS-7153','MBS-7154','MBS-7182','MBS-7240','MBS-7270','MBS-7317','MBS-7342','MBS-7470','MBS-7524','MBS-7600','MBS-7653','MBS-7671','MBS-7684','MBS-7751','MBS-7788','MBS-7880','MBS-7881','MBS-7890','MBS-7892','MBS-7956','MBS-8003','MBS-8052','MNP-1220','MNP-1224','MNP-1260','MNP-1338','MNP-1355','MNP-1382','MNP-1386','MSR2-1409','NDS-920','NDTH-14120','NDTH-14168','NDTH-14260','NDTH-14291','NDTH-14292','NDTH-14309','NDTH-14343','NDTH-14349','NDTH-14353','NDTH-14376','NDTH-14406','NDTH-14407','NDTH-14417','NDTH-14430','NDTH-14431','NDTH-14507','NDTH-14511','NDTH-14521','NDTH-14534','OM-18667','OM-18686','OM-18903','OM-19112','OM-19243','OM-19266','OM-19278','OM-19375','OM-19423','OM-19511','OM-19517','OM-19753','OM-19769','OM-19900','OM-20060','OM-20194','OM-20253','OM-20336','OM-20396','OM-20405','OM-20469','OM-20826','OM-20837','OM-20838','OM-20879','OM-20934','OM-20997','OM-21253','OM-21305','OM-21308','OM-21631','OM-21807','PM-509','PSMS-102','PSMS-85','PSMS-99','PW-13651','PW-13714','PW-13779','PW-13844','PW-13859','PW-13908','PW-13997','PW-14004','PW-14011','PW-14028','PW-14034','PW-14099','PW-14100','PW-14107','PW-14131','PW-14133','PW-14139','PW-14157','PW-14165','PW-14166','PW-14178','PW-14184','PW-14192','PW-14209','PW-14266','PW-14299','PW-14318','PW-14323','PW-14331','PW-14345','PW-14366','PW-14372','PW-14382','PW-14391','PW-14414','PW-14508','PW-14541','PW-14542','PW-14584','PW-14695','PW-14700','PW-14703','PW-14713','PW-14740','PW-14741','PW-14769','PW-14795','PW-14796','PW-14818','PW-14827','RD2-991','REKL-213','SABD-6958','SABD-6976','SABD-7026','SABD-7027','SABD-7028','SABD-7103','SABD-7178','SABD-7215','SABD-7274','SABD-7395','SABD-7517','SBL-2315','SOS-18420','SOS-18561','SOS-18669','SR-16579','SR-16631','SR-16861','SR-16867','SR-16945','SR-17057','SR-17106','SR-17138','SR-17146','SR-17217','SR-17222','SR-17223','SR-17226','SR-17229','SR-17236','SR-17259','SR-17269','SR-17295','SR-17296','SR-17301','SUSCRM-5411','SUWI-6115','SUWI-6214','SUWI-6343','SUWI-6348','SUWI-6387','SUWI-6636','SUWI-6819','SUWI-7139','SUWI-7220','SUWI-7234','SUWI-7261','SUZ-1321','SUZ-1325','SUZ-1328','SUZ-1383','SUZ-1384','SUZ-1400','SUZ-1401','SUZ-1403','SUZ-1408','TIBCOESB-4269','TIBCOESB-4270','TIBCOESB-4590','TIBCOESB-4637','UM2-26356','UM2-26361','UM2-26374','UM2-26385','UM2-26415','UM2-26422','UM2-26459','UM2-26460','UM2-26481','UM2-26492','UM2-26493','UM2-26495','UM2-26496','UM2-26497','UM2-26515','UM2-26516','UM2-26520','UM2-26522','UM2-26551','UM2-26559','UM2-26575','UM2-26582','UM2-26583','UM2-26606','UM2-26616','UM2-26633','UM2-26637','UM2-26647','UM2-26654','UM2-26655','UM2-26658','UM2-26660','UM2-26661','UM2-26670','UM2-26672','UM2-26683','UM2-26685','UM2-26686','UM2-26691','UM2-26701','UM2-26703','UM2-26704','UM2-26718','UM2-26723','UM2-26727','UM2-26731','UM2-26755','UM2-26761','UM2-26762','UM2-26789','UM2-26790','UM2-26793','UM2-26850','UM2-26852','UM2-26854','UM2-26863','UM2-26864','UM2-26887','UM2-26894','UM2-26901','UM2-26911','UM2-26915','UM2-26927','UM2-26932','UM2-26940','UM2-26941','UM2-26949','UM2-26951','UM2-26957','UM2-26960','UM2-26966','UM2-26982','UM2-26984','UM2-26997','UM2-27011','UM2-27012','UM2-27013','UM2-27014','UM2-27043','UM2-27044','UM2-27045','UM2-27048','UM2-27049','UM2-27050','UM2-27051','UM2-27056','UM2-27057','UM2-27059','UM2-27063','UM2-27065','UM2-27071','UM2-27077','UM2-27091','UM2-27174','UM2-27180','UM2-27181','UM2-27189','UM2-27191','UM2-27292','UM2-27305','UM2-27306','UM2-27319','UM2-27320','UM2-27321','UM2-27322','UM2-27331','UM2-27332','UM2-27333','UM2-27334','UM2-27335','UM2-27357','UM2-27371','UM2-27416','UM2-27420','UM2-27461','UM2-27462','UM2-27463','UM2-27464','UM2-27465','UM2-27466','UM2-27467','UM2-27468','UM2-27469','UM2-27470','UM2-27480','UM2-27482','UM2-27483','UM2-27484','UM2-27512','UM2-27539','UM2-27546','UM2-27576','UM2-27577','UM2-27606','UM2-27623','UM2-27638','VOD-2136','WEO-1934','WEO-1955','WEO-1960','WEO-1982','WEO-2000','WEO-2004','WEO-2005','WEO-2019','WEO-2030','WEO-2038','WEO-2057','WEO-2093','WEO-2148','WEO-2183','WEO-2187','WEO-2188','WEO-2191','WEO-2192','WEO-2214','WEO-2217','WEO-2226','WEO-2229','WEO-2236','WEO-2242','WEO-2250','WEO-2265','WEO-2267','WEO-2271','WEO-2292','WIND-7022','WIND-7023','WIND-7024','WIND-7027','WIND-7028','WIND-7029','WIND-7030','WIND-7031','WIND-7033','WIND-7035','WIND-7036','WIND-7037','WIND-7038','WIND-7039','WIND-7040','WIND-7041','WIND-7042','WIND-7043','WIND-7044','WIND-7047','WIND-7052','WIND-7053','WIND-7054','WIND-7061','WIND-7063','WIND-7064','WIND-7065','WIND-7066','WIND-7067','WIND-7069','WIND-7070','WIND-7072','WIND-7074','WIND-7076','WIND-7077','WIND-7078','WIND-7079','WIND-7080','WIND-7083','WIND-7084','WIND-7086','WIND-7089','WIND-7092','WIND-7094','WIND-7096','WIND-7097','WIND-7105','WIND-7106','WIND-7107','WIND-7108','WIND-7111','WIND-7113','WIND-7114','WIND-7116','WIND-7119','WIND-7120','WIND-7121','WIND-7123','WIND-7124','WIND-7126','WIND-7128','WIND-7131','WIND-7132','WIND-7133','WIND-7134','WIND-7135','WIND-7136','WIND-7137','WIND-7139','WIND-7140','WIND-7141','WIND-7142','WIND-7143','WIND-7144','WIND-7145','WIND-7146','WIND-7147','WIND-7150','WIND-7153','WIND-7155','WIND-7156','WIND-7157','WIND-7158','WIND-7160','WIND-7161','WIND-7162','WIND-7163','WIND-7164','WIND-7165','WIND-7168','WIND-7171','WIND-7172','WIND-7173','WIND-7176','WIND-7177','WIND-7178','WIND-7179','WIND-7181','WIND-7182','WIND-7183','WIND-7184','WIND-7185','WIND-7187','WIND-7188','WIND-7189','WIND-7196','WIND-7197','WIND-7198','WIND-7202','WIND-7203','WIND-7212','WIND-7213','WIND-7216','WIND-7219','WIND-7220','WIND-7221','WIND-7222','WIND-7223','WIND-7224','WIND-7226','WIND-7228','WIND-7229','WIND-7230','WIND-7231','WIND-7233','WIND-7234','WIND-7236','WIND-7241','WIND-7242','WIND-7243','WIND-7244','WIND-7245','WIND-7246','WIND-7247','WIND-7249','WIND-7250','WIND-7251','WIND-7254','WIND-7255','WIND-7256','WIND-7257','WIND-7269','WIND-7272','WIND-7273','WIND-7274','WIND-7275','WIND-7276','WIND-7278','WIND-7279','WIND-7283')", i
                //    );

                foreach (var item in issues)
                {
                    Logger.Instance.LogInformation(string.Format("{0} - {1}", item.Value, item.Key));
                }
                i = i + 1000;
            }
            //var l = i.GetWorklogs();
            /*
            ReportParameter report = new ReportParameter("date", "2016-12-27");
            reportViewer1.LocalReport.ReportPath = @"Raporty\RaportDzienny.rdl";

            string exeFolder = Application.StartupPath;
            string reportPath = Path.Combine(exeFolder, @"Raporty\RaportDzienny.rdl");
            reportViewer1.LocalReport.ReportPath = reportPath;

            reportViewer1.ProcessingMode = ProcessingMode.Local; //Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource("Konsultacja");
            ReportDataSource rds2 = new ReportDataSource("Notatka");
            ReportDataSource rds3 = new ReportDataSource("ZagrozoneSLA");
            ReportDataSource rds4 = new ReportDataSource("Podsumowanie");// (@"Data Source=salsa\sql2012;Initial Catalog=Support_CP");

            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.DataSources.Add(rds2);
            reportViewer1.LocalReport.DataSources.Add(rds3);
            reportViewer1.LocalReport.DataSources.Add(rds4);
            reportViewer1.LocalReport.EnableHyperlinks = true;


            reportViewer1.RefreshReport();
            */

        }

        private void cb_SLA_JiraSynchro_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void bt_KontoBillenniumSave_Click(object sender, EventArgs e)
        {
            if (mtb_BillenniumPass.Text != ""
                && tryLogginToJira("billennium", mtb_BillenniumPass.Text)
                )
            {

                Properties.Settings.Default.hasloBillennium = mtb_BillenniumPass.Text;
                Properties.Settings.Default.Save();

                cb_KontoBillennium.Checked = true;
                cb_KontoBillennium.Text = "OK!";
            }
            else
            {
                cb_KontoBillennium.Checked = false;
                cb_KontoBillennium.Text = "Błędne hasło lub brak połączenia JIRA!";
            }
        }

        private bool tryLogginToJira(string Login, string Password, string adress = "http://jira")
        {

            try
            {
                Jira jbill;
                jbill = Jira.CreateRestClient(adress, Login, Password);

                jbill.GetIssuePriorities();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        private void button3_Click(object sender, EventArgs e)
        {

            webBrowser1.DocumentText = @"<!DOCTYPE html>
<html>
<head>
<style>
table { 
    display: table;
    border-collapse: separate;
    border-spacing: 2px;
    border-color: black;
}
</style>
</head>
<body><p>
	<strong>Witam, </strong>
</p>
<p>
	<strong>&nbsp;</strong>
</p>
<p>
	<strong>W dniach 28.02.2017 18:00 - 01.03.2017 18:00 wystąpiły następujące problemy z dostępnością wspieranych środowisk:</strong>
</p>
<p>&nbsp;</p>
<table width=""945"" border = ""1"" border-color = ""black"">
	<tbody>
		<tr>
			<td width=""113"">
				<p>Nr awarii</p>
			</td>
			<td width=""86"">
				<p>Data początkowa</p>
			</td>
			<td width=""87"">
				<p>Data zakończenia</p>
			</td>
			<td width=""66"">
				<p>Czas awarii (minuty)</p>
			</td>
			<td width=""593"">
				<p>Uwagi</p>
			</td>
		</tr>
		<tr>
			<td width=""113"">
				<p>
					<a href=""https://jira/browse/WIND-6012"">WIND-6012</a>
				</p>
			</td>
			<td width=""86"">
				<p>2017-03-01 01:45</p>
			</td>
			<td width=""87"">
				<p>2017-03-01 02:03</p>
			</td>
			<td width=""66"">
				<p>18</p>
			</td>
			<td width=""593"">
				<p>Problem z zasilaniem WIND na kroku: spWindykacjaDTHImportFormyWlasnosciSprzetu</p>
			</td>
		</tr>
	</tbody>
</table>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>
	<strong>Dane o zgłoszeniach od dnia 18:00 28.02.2017 do dnia 18:00 01.03.2017</strong>
</p>
<p>Liczba obsłużonych zgłoszeń Jira: 85<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba nowych zgłoszeń Jira: 46<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba ponownie otwartych zgłoszeń: 11<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń z poprzednich dni: 28</p>
<p>
	<strong>OBSZAR CRM: </strong>
</p>
<p>Liczba nowych zgłoszeń Jira: 19<br />
Liczba ponownie otwartych zgłoszeń: 4<br />
Liczba zgłoszeń z poprzednich dni: 13<br />
Liczba zgłoszeń zamkniętych: 30<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń rozwiązanych samodzielnie: 18<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń przekazanych do III linii: 0<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń przekazanych do innej II linii: 4<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń odrzuconych: 8<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń zamkniętych jako duplikat: 0<br />
Liczba zgłoszeń w konsultacji: 1<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń w konsultacji biznesowej: 0<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Liczba zgłoszeń w konsultacji deweloperskiej: 1</p>
<p>&nbsp;</p>
</body>
</html>

            ";
        }

        private void bt_awaria_zapisz_Click(object sender, EventArgs e)
        {
            if (tb_awaria_nrJira.Text != string.Empty
                && tb_awaria_nrJira.Text != null
                && tb_awaria_nrJira.Text.Contains("-")
                && tb_awaria_nrJira.Text.Count() > 2
                )
            {

                string numerjira = tb_awaria_nrJira.Text;
                string userId = gujaczWFS.getUser().Id.ToString();
                string datastart = dtp_awaria_start.Text;
                string dataend = (cb_awaria_niezakonczona.Checked ? null : dtp_awaria_stop.Text);
                string koment = tb_awaria_komentarz.Text;
                string opis_cp = tb_awaria_opis.Text;
                string czyoncall = "1";
                string czyblokujacy = "1";

                var radioButtons = gb_awaria_onCall.Controls.OfType<RadioButton>();
                foreach (RadioButton rb in radioButtons)
                {
                    if (rb.Checked)
                    {
                        czyoncall = (rb.Text == "Tak" ? "1" : "0");
                    }

                }
                radioButtons = gb_awaria_bloker.Controls.OfType<RadioButton>();
                foreach (RadioButton rb in radioButtons)
                {
                    if (rb.Checked)
                    {
                        czyblokujacy = (rb.Text == "Tak" ? "1" : "0");
                    }

                }

                List<List<string>> awariaStatus = gujaczWFS.ExecuteStoredProcedure("CP_dodaj_awsys", new string[] {
                    numerjira,userId,datastart,dataend,koment,opis_cp,czyoncall,czyblokujacy
                }, DatabaseName.SupportCP);

                if (awariaStatus[0][0].ToString() == "Jest OK")
                {
                    tb_awaria_nrJira.Text = string.Empty;
                    tb_awaria_komentarz.Text = string.Empty;
                    tb_awaria_opis.Text = string.Empty;

                }
                MessageBox.Show(awariaStatus[0][0].ToString());

            }
            else
            {
                MessageBox.Show("Błędny numer Jira");
            }
        }

        private void tab_Raporty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bt_LA_szukaj_Click(object sender, EventArgs e)
        {
            List<List<string>> awariaStatus = gujaczWFS.ExecuteStoredProcedure("CP_obsluz_awsys", new string[] {
                    tb_LA_nrJira.Text, dtp_LA_startDate.Text, dtp_LA_stopDate.Text
                }, DatabaseName.SupportCP);

            dgv_Awarie.Columns.Clear();
            dgv_Awarie.Columns.Add("dgvIssueId", "IssueId");
            dgv_Awarie.Columns.Add("dgvJiraNr", "Numer Jira");

            dgv_Awarie.Columns.Add("dgvDataStart", "Data Początkowa");
            dgv_Awarie.Columns.Add("dgvDataStop", "Data Końcowa");
            dgv_Awarie.Columns.Add("dgvDataUtworzenia", "Data Wpisu");
            dgv_Awarie.Columns.Add("dgvDodal", "Dodał");
            dgv_Awarie.Columns.Add("dgvKomentarz", "Komentarz");
            dgv_Awarie.Columns.Add("dgvOpisAwarii", "Opis awarii");
            dgv_Awarie.Columns.Add("dgvIleTrwalo", "Czas trwania");
            dgv_Awarie.Columns.Add("dgvBlokujacy", "Czy bloker");
            dgv_Awarie.Columns.Add("dgvOnCall", "Czy OnCall");

            //awariaStatus = awariaStatus.OrderByDescending(x => Int32.Parse(x[3])).ToList();
            foreach (var row in awariaStatus)
            {
                dgv_Awarie.Rows.Insert(0, new DataGridViewRow());
                dgv_Awarie.Rows[0].Cells["dgvIssueId"].Value = row[0];  //dgvIssueId
                dgv_Awarie.Rows[0].Cells["dgvJiraNr"].Value = row[1];  //dgvJiraNr
                dgv_Awarie.Rows[0].Cells["dgvDataStart"].Value = row[2];  //dgvOdpowiedzialny
                dgv_Awarie.Rows[0].Cells["dgvDataStop"].Value = row[3];  //dgvTypZgloszenia
                dgv_Awarie.Rows[0].Cells["dgvDataUtworzenia"].Value = row[4];  //dgvPriorytet
                dgv_Awarie.Rows[0].Cells["dgvDodal"].Value = row[5];  //dgvPauza
                dgv_Awarie.Rows[0].Cells["dgvKomentarz"].Value = row[6];  //dgvAktCzasRealizacji
                dgv_Awarie.Rows[0].Cells["dgvOpisAwarii"].Value = row[7];  //dgvOpisAwarii 
                dgv_Awarie.Rows[0].Cells["dgvIleTrwalo"].Value = row[8];  //dgvPozostaloMin  
                dgv_Awarie.Rows[0].Cells["dgvBlokujacy"].Value = row[9];  //dgvPozostaloMin  
                dgv_Awarie.Rows[0].Cells["dgvOnCall"].Value = row[10];  //dgvPozostaloMin  

            }

            dgv_Awarie.Columns["dgvDataUtworzenia"].Visible = false;

            /**/
            dgv_Awarie.Columns["dgvIssueId"].ReadOnly = true;
            dgv_Awarie.Columns["dgvIleTrwalo"].ReadOnly = true;
            dgv_Awarie.Columns["dgvDodal"].ReadOnly = true;
            dgv_Awarie.Columns["dgvIleTrwalo"].ReadOnly = true;
            /**/

            dgv_Awarie.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv_Awarie.Columns["dgvKomentarz"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_Awarie.Columns["dgvOpisAwarii"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgv_Awarie.Columns["dgvKomentarz"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            /*
            dgv_Awarie.Columns["dgvKomentarz"].Resizable = DataGridViewTriState.True;
            dgv_Awarie.Columns["dgvKomentarz"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgv_Awarie.Columns["dgvOpisAwarii"].Resizable = DataGridViewTriState.True;
            dgv_Awarie.Columns["dgvOpisAwarii"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            */


        }

        private void dgv_Awarie_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell cell = dgv[e.ColumnIndex, e.RowIndex];
            bool edit = false;
            DateTime dt;
            StringBuilder sb = new StringBuilder();
            NoticeButtons nb = NoticeButtons.OK_CANCEL;

            if (
                (cell.ColumnIndex == 2
                   || cell.ColumnIndex == 3
                   )
                && !cell.Tag.ToString().Equals(cell.Value.ToString())
                && DateTime.TryParse(cell.Value.ToString(), out dt)
                )
            {

                sb.AppendLine("Czy dokonać zmany wartości dla kolumny " + dgv.Columns[e.ColumnIndex].HeaderText);
                sb.AppendLine();
                sb.AppendLine("Stara wartosć: " + cell.Tag.ToString());
                sb.AppendLine();
                sb.AppendLine("Nowa wartosć: " + dt.ToString());



            }
            if (edit)
            {
                DialogResult dr = NoticeForm.ShowNotice(sb.ToString(), nb, new string[] { "Zapisz!", "Anuluj!" });

                if (dr.ToString() == "OK")
                {

                }
                else if (dr.ToString() == "Cancel")
                {
                    cell.Value = cell.Tag;
                }
            }
            cell.Value = cell.Tag;
            cell.Tag = null;
        }

        private void dgv_Awarie_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell cell = dgv[e.ColumnIndex, e.RowIndex];
            if (cell.Value != null)
                cell.Tag = cell.Value.ToString();
        }

        private void btn_BillenniumUzupelnijDane_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {


        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                textBox3.Text = "";

            Jira j;
            j = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
            j.MaxIssuesPerRequest = 20000;

            List<string> logins = new List<string>();

            DateTime dStart;
            DateTime dStop = DateTime.Now;

            if (DateTime.TryParse(textBox4.Text, out dStart))
            {
                DateTime tmpDay = dStart.AddMonths(1); 
                dStart = new DateTime(dStart.Year, dStart.Month, 1);
                dStop = new DateTime(tmpDay.Year, tmpDay.Month, 1);
            }
            else
            {
                return;
            }


            var eOm = textBox4.Text.Split('/');
            string dateEnd = eOm[0] + '/' + Convert.ToString(int.Parse(eOm[1]) + 1) + '/' + eOm[2];

            if (!checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        logins.Add(checkedListBox1.Items[i].ToString());

                        textBox3.Text += "\"" + checkedListBox1.Items[i].ToString() + "\",";

                    }
                }
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);

            }



            IEnumerable<Issue> IssueList;
            if (!checkBox1.Checked)
            {
                textBox3.Text = "worklogAuthor in (" + textBox3.Text + ") AND worklogDate >= '" + dStart.ToShortDateString() + "' AND worklogDate < '" + dStop.ToShortDateString() + "'";
                IssueList = j.GetIssuesFromJql(textBox3.Text);

            }
            else
                IssueList = j.GetIssuesFromJql(textBox3.Text);

            dataGridView1.Rows.Clear();

            foreach (var item in IssueList)
            {
                bool czyDS = false;

                foreach (var ds in item.Labels.Get())
                {
                    if (ds.Equals("billennium_dniowkiserwisowe_crm"))
                    {
                        czyDS = true;
                    }
                }
                int ileHsuma = 0;

                var icl = item.GetWorklogs();

                //foreach (var it in icl)
                //{
                //    foreach(var itt in it.Items)
                //    {
                //        System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1} - {2}", itt.FieldName , itt.FromValue, itt.ToValue));

                //    }

                //}
                if (!checkBox1.Checked)
                {
                    foreach (string login in logins)
                    {


                        foreach (var item2 in item.GetWorklogs())
                        {

                            if (item2.Author == login)
                            {
                                //Worklog wl2 = new Worklog(item2.TimeSpent, (DateTime)item2.StartDate, "P.K.");
                                //item.AddWorklog(wl2);

                                int ileH = 0;

                                var wl = item2.TimeSpent.ToString().Split(' ');
                                for (int i = 0; i < wl.Count(); i++)
                                {
                                    if (wl[i].IndexOf('h') > 0)
                                    {
                                        ileH += 60 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('h')));
                                    }
                                    else if (wl[i].IndexOf('d') > 0)
                                    {
                                        ileH += 60 * 8 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('d')));
                                    }
                                    else if (wl[i].IndexOf('w') > 0)
                                    {
                                        ileH += 60 * 40 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('w')));
                                    }
                                    else if (wl[i].IndexOf('m') > 0)
                                    {
                                        ileH += int.Parse(wl[i].Substring(0, wl[i].IndexOf('m')));
                                    }
                                }

                                ileHsuma += ileH;

                                dataGridView1.Rows.Add(item2.Author, item.Key.Value, item2.TimeSpent, ((double)ileH) / 60, ileH, item2.StartDate.Value.ToShortDateString(), item2.Comment.Replace("\n", " "), ((double)ileHsuma) / 60, czyDS, item2.CreateDate.ToString(), item2.UpdateDate.ToString());
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item2 in item.GetWorklogs())
                    {

                        int ileH = 0;

                        var wl = item2.TimeSpent.ToString().Split(' ');
                        for (int i = 0; i < wl.Count(); i++)
                        {
                            if (wl[i].IndexOf('h') > 0)
                            {
                                ileH += 60 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('h')));
                            }
                            else if (wl[i].IndexOf('d') > 0)
                            {
                                ileH += 60 * 8 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('d')));
                            }
                            else if (wl[i].IndexOf('w') > 0)
                            {
                                ileH += 60 * 40 * int.Parse(wl[i].Substring(0, wl[i].IndexOf('w')));
                            }
                            else if (wl[i].IndexOf('m') > 0)
                            {
                                ileH += int.Parse(wl[i].Substring(0, wl[i].IndexOf('m')));
                            }
                        }

                        ileHsuma += ileH;

                        dataGridView1.Rows.Add(item2.Author, item.Key.Value, item2.TimeSpent, ((double)ileH) / 60, ileH, item2.StartDate.Value.ToShortDateString(), item2.Comment.Replace("\n", " "), ((double)ileHsuma) / 60, czyDS, item2.CreateDate.ToString(), item2.UpdateDate.ToString());

                    }
                }
                //item.SaveChanges();
            }
        }


        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

        }

        /// <summary>
        /// btn_NieprzydzieloneUzupelnijDane_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool czyPrzyjacAuto = false;

        private void bt_autoAssigne_Click(object sender, EventArgs e)
        {
            tm_autoFrsh.Enabled = true;
            tm_autoFrsh.Interval = 20000;

            toolStripProgressBar3.Visible = true;
            toolStripStatusLabel8.Visible = true;

            tm_autoFrsh_Tick(this, null);
            bt_AutoAssigne.Enabled = false;
        }

        private void doSavingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Zgłoszenie zostało poprawnie zapisane pod numerem: " + Convert.ToString(e.Result));
            this.Dispose();
        }

        private void autoCheckAndAssigne()
        {
            //DisableIssuesButtons();
            doByWorker(new DoWorkEventHandler(GetAllIssuesFromJira), null, new RunWorkerCompletedEventHandler(newIssueAutoAddToBPM));


//            EnableIssuesButtons();
        }


        private void tm_autoFrsh_Tick(object sender, EventArgs e)
        {
            tm_autoFrsh.Interval = 300000;
            //if (!WorkingSla)
            //{
            //    treeView1.Nodes.Clear();
            //    treeView2.Nodes.Clear();

            //    doByWorker(new DoWorkEventHandler(btn_slaRaport_Load_Click), null, new RunWorkerCompletedEventHandler(SlaReportCompleted));
            //}

            autoCheckAndAssigne();
            //if (czyPrzyjacAuto)
            //{
            //    DisableIssuesButtons();
            //    bt_AutoAssigne.Text = "";
            //    newIssueAutoAddToBPM();
            //    bt_AutoAssigne.Text = "Do pobrania";
            //    tm_autoFrsh.Interval = 15000;
            //    czyPrzyjacAuto = false;
            //    //btn_slaRaport_Load_Click(this, null);
            //    if (!WorkingSla)
            //    {
            //        doByWorker(new DoWorkEventHandler(btn_slaRaport_Load_Click), null, new RunWorkerCompletedEventHandler(SlaReportCompleted));
            //    }
            //    EnableIssuesButtons();

            //}
            //else
            //{
            //    DisableIssuesButtons();
            //    treeView1.Nodes.Clear();
            //    treeView2.Nodes.Clear();
            //    //SLAlista.Clear();
            //    //SLAlista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport_v2", new string[] { }, DatabaseName.SupportCP);

            //    GetAllIssuesFromJira();

            //    bt_AutoAssigne.Text = "Do przyjęcia";
            //    tm_autoFrsh.Interval = 150000;
            //    czyPrzyjacAuto = true;
            //    EnableIssuesButtons();
            //}

            autoAssigneSecond = 0;
            toolStripStatusLabel8.Text = bt_AutoAssigne.Text;
            //autoCheckAndAssigne();
            toolStripProgressBar3.Maximum = tm_autoFrsh.Interval / 1000;
            toolStripProgressBar3.Minimum = 0;
            tm_AutoAssigne.Enabled = true;

        }


        public void newIssueAutoAddToBPM()
        {
       
            newIssueAutoAddToBPMUzupelnijDane();
            newIssueAutoAddToBPMZapiszZgloszenia();
            //Logger.Instance.LogInformation(string.Format("Automatyczne sprawdzanie zgłoszeń zakończono o: {0} \n\n", DateTime.Now));


        }

        public void newIssueAutoAddToBPM(object sender, EventArgs e)
        {

            newIssueAutoAddToBPMUzupelnijDane();
            newIssueAutoAddToBPMZapiszZgloszenia();
            //Logger.Instance.LogInformation(string.Format("Automatyczne sprawdzanie zgłoszeń zakończono o: {0} \n\n", DateTime.Now));
            doByWorker(new DoWorkEventHandler(btn_slaRaport_Load_Click), null, new RunWorkerCompletedEventHandler(SlaReportCompleted));

        }



        public void newIssueAutoAddToBPMUzupelnijDane()
        {
            Thread t = new Thread((ThreadStart)delegate ()
            {
                try
                {
                    UzupelnijDane(treeView1, true);
                    UzupelnijDane(treeView2, true);
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance);
                }
            });


            t.Priority = ThreadPriority.AboveNormal;
            t.IsBackground = true;
            t.Start();

        }

        public void newIssueAutoAddToBPMZapiszZgloszenia()
        {
            Thread t2 = new Thread((ThreadStart)delegate ()
            {
                try
                {

                    List<KeyValuePair<BillingIssueDto, IssueState>> tmp = (issues["treeView1"].Where(x => x.Value == IssueState.READYTOSAVE).ToList());
                    
                    ZapiszZgloszenia(tmp, treeView1, true);

                    tmp.Clear();
                    tmp = (issues["treeView2"].Where(x => x.Value == IssueState.READYTOSAVE).ToList());
                    
                    ZapiszZgloszenia(tmp, treeView2, true);

                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance);
                }

            });

            t2.Priority = ThreadPriority.AboveNormal;
            t2.IsBackground = true;
            t2.Start();

        }


        private int autoAssigneSecond = 0;

        private void tm_AutoAssigne_Tick(object sender, EventArgs e)
        {
            if (autoAssigneSecond < toolStripProgressBar3.Maximum)
            {
                autoAssigneSecond++;
                toolStripProgressBar3.Value = autoAssigneSecond;
            }
            else
            {
                tm_AutoAssigne.Enabled = false;
            }

        }


        private void ZapiszZgloszenia(List<KeyValuePair<BillingIssueDto, IssueState>> doZapisu, TreeView tr, bool auto = false)
        {

            pb_SetVisibilityPanel(true);
            if (doZapisu.Count != 0)
            {
                pb_SetMaxProgressBar(doZapisu.Count + 2);
                pb_UpdateProgressBar("Rozpoczynanie zapisu");
                DisableIssuesButtons();
                
                Thread t = new Thread((ThreadStart)delegate ()
                {
                    StringBuilder sb = new StringBuilder();
                    
                    foreach (KeyValuePair<BillingIssueDto, IssueState> item in doZapisu)
                    {
                        try
                        {

                            Logger.Instance.LogInformation(string.Format("Automatyczny zapis nowego zgłoszenia {0} [{1}] \n\n", item.Key, DateTime.Now));

                            int wfsIssueId = 0;

                            //gujaczWFS.CreateNewIssue((BillingIssueDtoHelios)item.Key, out wfsIssueId);
                            gujaczWFS.addBillingIssueToWFS((BillingIssueDtoHelios)item.Key, out wfsIssueId); //Zapis zgłoszenia do WFS
                            item.Key.issueWFS.WFSIssueId = wfsIssueId;
                            //MessageBox.Show("wfsIssueId = " + wfsIssueId);
                            System.Diagnostics.Debug.WriteLine(string.Format("Zapisano zgłoszenie {0} - {1}", item.Key.ToString(), wfsIssueId.ToString()));
                            if (wfsIssueId == 0)
                            {
                                sb.AppendLine(item.Key.Idnumber + ": nie zapisano");
                                pb_UpdateProgressBar("Zablokowano zapis: " + item.Key.Idnumber);
                            }
                            else
                            {

                                Logger.Instance.LogInformation(string.Format("Automatyczny zapis poprawny {0} [{1}] \n\n", item.Key, DateTime.Now));
                                sb.AppendLine(item.Key.Idnumber + ": Dodane poprawnie");
                                pb_UpdateProgressBar("Zapisano zgłoszenie: " + item.Key.Idnumber);
                                issues[tr.Name].Remove(item.Key);
                                issues[tr.Name].Add(item.Key, IssueState.INWFS);

                                //Dotyczy zgłoszeń z heliosa
                                int itemsCount = tr.Nodes.Count;

                                for (int i = 0; i < itemsCount; i++)
                                {
                                    TreeNode treeItem = tr.Nodes[i];
                                    if (treeItem.Text.Split(' ').First() == item.Key.Idnumber)
                                    {

                                        Logger.Instance.LogInformation(string.Format("Modyfikacja treeItem"));

                                        KeyValuePair<BillingIssueDto, IssueState> iss = issues[tr.Name].Where(x => x.Key.Idnumber == item.Key.Idnumber).FirstOrDefault();
                                        BillingIssueDtoHelios it = iss.Key as BillingIssueDtoHelios;

                                        int index = GetImageIndex(item.Key as BillingIssueDtoHelios, IssueState.INWFS);

                                        tr.Invoke((MethodInvoker)delegate
                                        {
                                            treeItem.ImageIndex = index;
                                            treeItem.SelectedImageIndex = index;
                                        });



                                    }
                                }
                                Logger.Instance.LogInformation(string.Format("Status Auto: {0} \n\n", auto.ToString()));
                                if (auto)
                                {
                                    Logger.Instance.LogInformation(string.Format("in if Auto \n\n"));
                                    tr.Invoke((MethodInvoker)delegate
                                    {

                                        Logger.Instance.LogInformation(string.Format("przed addCommentJira(item.Key.ToString());"));
                                        addCommentJira(item.Key.ToString());
                                        Logger.Instance.LogInformation(string.Format("po addCommentJira(item.Key.ToString());"));
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.LogError(ex, Logger.Instance);
                            sb.AppendLine(item.Key.Idnumber + ": " + ex.Message);
                        }
                    }
                    //                    NoticeForm.ShowNotice(sb.ToString()+sbDuble.ToString(), "Zapisano zgłoszenia");


                    NoticeButtons nb = NoticeButtons.OK_CANCEL;
                    if (doZapisu.Count == 0)
                        nb = NoticeButtons.OK;
                    DialogResult dr;
                    if (!auto)
                    {
                        dr = NoticeForm.ShowNotice(sb.ToString(), nb, new string[] { "OK", "Dodaj komentarz do JIRA" });
                    }
                    else
                    {
                        dr = DialogResult.OK;
                    }


                    //MyMessageBox mms = new MyMessageBox(sb.ToString(), "Zapisno zgłoszenia");
                    //mms.ShowDialog();
                    //MessageBox.Show(sb.ToString(), "Zapisno zgłoszenia", MessageBoxButtons.OK);
                    pb_SetVisibilityPanel(false);
                    GetActionForIssues(tr);

                    /*Obsługa komentarza*/
                    try
                    {
                        if (dr == DialogResult.OK && (nb == NoticeButtons.OK_CANCEL || auto))
                        {


                            addCommentJira(doZapisu);


                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogError(ex, Logger.Instance);
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        EnableIssuesButtons();
                    });
                });

                t.Priority = ThreadPriority.Highest;
                t.IsBackground = true;
                t.Start();
            }
            //else
            //    NoticeForm.ShowNotice("Nie ma żadnych spraw do dodania do BPM.");
            //MessageBox.Show("Nie ma żadnych spraw do dodania do WFS.");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Jira j;
            //j = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);


            //IEnumerable<Issue> IssueList;

            //IssueList = j.GetIssuesFromJql(textBox3.Text, 10000);
            //foreach (var item in IssueList)
            //{
            //    IEnumerable<IssueChangeLog> icl = item.GetChangeLogs();
            //    IEnumerable<IssueChangeLog> icli;
            //    foreach (var item2 in icl)
            //    {
            //        // if(item2.Items
            //    }
            //}
            throw (new NotImplementedException());
        }

        private void btn_MultiDodaj_Click(object sender, EventArgs e)
        {

        }

        private void bt_dayReportGenerate_Click(object sender, EventArgs e)
        {
            generateServiceReport(rtb_dayReportMessage);
        }

        private void bt_dayReportSend_Click(object sender, EventArgs e)
        {
            SendEmail(rtb_dayReportMessage);
        }

        private void bt_LogSearchPatch_Click(object sender, EventArgs e)
        {
            fb_logSearch.ShowDialog();

            tb_LogSearchPath.Text = fb_logSearch.SelectedPath;
        }

        private void bt_LogSearchRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isNullObjectOrEmptyString(tb_LogSearchPath.Text))
                {
                    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                    pProcess.StartInfo.FileName = tb_LogSearchPath.Text + @"\LogSearch.bat";
                    //pProcess.StartInfo.Arguments = "olaa"; //argument
                    pProcess.StartInfo.UseShellExecute = false;
                    //pProcess.StartInfo.RedirectStandardOutput = true;
                    pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                    //pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                    pProcess.Start();
                    //string output = pProcess.StandardOutput.ReadToEnd(); //The output result
                    pProcess.WaitForExit();

                }
            }
            catch(Exception ex)
            {

            }



            ProcessStartInfo startInfo = new ProcessStartInfo();
             var url = "configuration.json";
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false; // This must be false to use env variable
            startInfo.FileName = tb_LogSearchPath.Text + @"\LogSearch.jar";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;

            startInfo.Arguments =  url;

            try
            {
                using (System.Diagnostics.Process exeProcess = System.Diagnostics.Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(@"C:\Program Files (x86)\Common Files\Oracle\Java\javapath");
                process.StartInfo.FileName = (@"C:\Program Files (x86)\Common Files\Oracle\Java\javapath") + @"\Java.exe";
                string aArgument = string.Format("{0} {1}", @"C:\CP\usefull\LogSearch\LogSearch.jar", @"C:\CP\usefull\LogSearch\configuration.json");
                process.StartInfo.Arguments = string.Format(aArgument);
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();

                int holdStatus = process.ExitCode;

                if (holdStatus > 0)
                {
                    validJavaAppRun = false;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
                MessageBox.Show("Exception Occurred : " + ex.Message + " " + ex.StackTrace.ToString());
                validJavaAppRun = false;
            }

        }
        Boolean validJavaAppRun = true;

        private void bt_OtworzLog_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e as MouseEventArgs).Button == MouseButtons.Left)
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = Logger.Instance.LogsDirectoryPath;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }
                else if ((e as MouseEventArgs).Button == MouseButtons.Right)
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = Logger.Instance.LogsDirectoryPath + @"\" + DateTime.Now.ToShortDateString() + @".log";
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }
            }
            catch(Exception)
            { }

        }

        private void tb_filter1name_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if ((sender as TextBox).Name.Contains("tb_filter1name"))
                {
                    filterNameUpdateValue("filter1name",(sender as TextBox).Text);
                    filterNameUpdate("filter1name");
                    filterNameVisibleMode(lb_filter1name, tb_filter1name, true);
                }
                else
                {
                    filterNameUpdateValue("filter2name", (sender as TextBox).Text);
                    filterNameUpdate("filter2name");
                    filterNameVisibleMode(lb_filter2name, tb_filter2name, true);
                }
            }
        }

        private void lb_filter1name_Click(object sender, EventArgs e)
        {
            if ((sender as Label).Name.Contains("lb_filter1name"))
            {
                filterNameVisibleMode(lb_filter1name, tb_filter1name, false);
            }
            else
            {
                filterNameVisibleMode(lb_filter2name, tb_filter2name, false);
            }
        }

        private void filterNameVisibleMode(Label lb, TextBox tb, bool stateLb)
        {
            lb.Visible = stateLb;
            tb.Visible = !lb.Visible;
        }
        private void filterNameUpdateValue(string filterName, string newLabelValue)
        {
            Properties.Settings.Default[filterName] = newLabelValue;
            Properties.Settings.Default.Save();
        }

        private void filterNameUpdate(string filterName)
        {
            if (filterName.Contains("filter1name"))
            {
                tb_filter1name.Text = Properties.Settings.Default[filterName].ToString();
                tp_filter1name.Text = Properties.Settings.Default[filterName].ToString();
                lb_filter1name.Text = Properties.Settings.Default[filterName].ToString();
            }
            else if (filterName.Contains("filter2name"))
            {
                tb_filter2name.Text = Properties.Settings.Default[filterName].ToString();
                tp_filter2name.Text = Properties.Settings.Default[filterName].ToString();
                lb_filter2name.Text = Properties.Settings.Default[filterName].ToString();
            }
        }
    }
}
