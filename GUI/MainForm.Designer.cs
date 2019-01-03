using System;
using System.Windows.Forms;
using Entities;
using System.Collections.Generic;
using System.Threading;
using Utility;

namespace GUI
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void wyczysc()
        {

            //var textBox2 = (object)null;
            //textBox3.Clear();
            //comboBox1.ResetText();
            //maskedTextBox1.Clear();
            //maskedTextBox2.Clear();
            //richTextBox3.Clear();
        }

        private bool zaloguj(Login formLogin)
        {
            formLogin.ShowDialog();
            //gujaczWFS.setUser(new User(formLogin.login, formLogin.haslo));
            //Thread thr = new Thread((ThreadStart)delegate()
            //    {
            bool condition = formLogin.loginSuccess;
            //this.Invoke((MethodeInvokerf)delegate()
            //{
            toolStripStatusLabel1.Text = "Logowanie... ";
            //});
            //try
            //{
            //    condition = gujaczWFS.loginToWFS(new Entities.User(formLogin.login, formLogin.haslo));
            //}
            //catch (MyCustomException ex)
            //{
            //    exManager.LogException(ex);
            //}
            //finally
            //{
            //this.Invoke((MethodInvoker)delegate()
            //{
            if (condition)
            {
                toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                toolStripDropDownButton1.Text = formLogin.login;
                toolStripStatusLabel1.Text = "Zalogowany jako: ";
                zalogujSięToolStripMenuItem.Visible = false;
                przelaczStripMenuItem1.Visible = true;
            }
            else
            {
                toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                toolStripStatusLabel1.Text = "NIEZALOGOWANY: ";
                toolStripDropDownButton1.Text = "Zaloguj..";
                zalogujSięToolStripMenuItem.Visible = true;
                przelaczStripMenuItem1.Visible = false;
                if (NoticeForm.ShowNotice("Niestety nie udało się zalogować. Spróbuj ponownie!", NoticeButtons.OK_CANCEL, new string[] { "Zamknij", "Kontynuuj" }) == DialogResult.Cancel)
                    Application.Exit();
            }
            //});
            //}
            //    });
            //thr.IsBackground = true;
            //thr.Start();
            return condition;
        }

        private void setBusy(bool condition)
        {
            if (condition)
                this.toolStripStatusLabel6.Visible = condition;
            //this.saveButton.Enabled = !condition;
            //this.searchButton.Enabled = !condition;
            //this.cpIssues.Enabled = !condition;
            //this.myIssues.Enabled = !condition;
        }
        //private void LoadDatasources()
        //{
        //    jiraRodzajeComboBox.DataSource = gujaczWFS.GetRodzajeZgloszen();
        //    jiraRodzajeComboBox.DisplayMember = "Value";
        //    jiraRodzajeComboBox.ValueMember = "Id";
        //}


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_NewIssue = new System.Windows.Forms.Button();
            this.btn_WyszukajWWFS = new System.Windows.Forms.Button();
            this.issueTab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_NieprzydzieloneZapiszDoWFS = new System.Windows.Forms.Button();
            this.btn_NieprzydzieloneUzupelnijDane = new System.Windows.Forms.Button();
            this.btn_NieprzydzieloneOdswiez = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.btn_MojeZapiszDoWFS = new System.Windows.Forms.Button();
            this.btn_MojeUzupelnijDane = new System.Windows.Forms.Button();
            this.btn_MojeOdswiez = new System.Windows.Forms.Button();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.tp_Billennium = new System.Windows.Forms.TabPage();
            this.btn_BillenniumZapisz = new System.Windows.Forms.Button();
            this.btn_BillenniumUzupelnijDane = new System.Windows.Forms.Button();
            this.btn_BillenniumOdswiez = new System.Windows.Forms.Button();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.tp_tmp = new System.Windows.Forms.TabPage();
            this.treeView4 = new System.Windows.Forms.TreeView();
            this.numerZgl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_WyszukajWJira = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tc = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bt_AutoAssigne = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_DodajNotatke = new System.Windows.Forms.Button();
            this.btn_Procesy = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tp_Raporty = new System.Windows.Forms.TabPage();
            this.tab_Raporty = new System.Windows.Forms.TabControl();
            this.tp_Settings = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bt_LogSearchRun = new System.Windows.Forms.Button();
            this.bt_LogSearchPatch = new System.Windows.Forms.Button();
            this.tb_LogSearchPath = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_KontoBillennium = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.bt_KontoBillenniumSave = new System.Windows.Forms.Button();
            this.mtb_BillenniumPass = new System.Windows.Forms.MaskedTextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SaveFilters = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_UnassignedFilterName = new System.Windows.Forms.TextBox();
            this.tb_AssignedFilterName = new System.Windows.Forms.TextBox();
            this.cbox_RefreshBoth = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_SoundNotificationOpenFileDialog = new System.Windows.Forms.Button();
            this.btn_PowiadomienieWlacz = new System.Windows.Forms.Button();
            this.tb_SoundNotificationPath = new System.Windows.Forms.TextBox();
            this.notifyTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.cb_SoundNotification = new System.Windows.Forms.CheckBox();
            this.ckb_isNotifyEnabled = new System.Windows.Forms.CheckBox();
            this.cb_EmailNotification = new System.Windows.Forms.CheckBox();
            this.tp_billing = new System.Windows.Forms.TabPage();
            this.btn_BillingZapiszDoXls = new System.Windows.Forms.Button();
            this.zglDgv = new System.Windows.Forms.DataGridView();
            this.btn_BillingZgloszeniaKategoryzacja = new System.Windows.Forms.Button();
            this.tabStatystyki = new System.Windows.Forms.TabControl();
            this.btn_BillingZgloszeniaZDzisiaj = new System.Windows.Forms.Button();
            this.btn_BillingRaportStanuZgloszen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.richTextBoxStats = new System.Windows.Forms.RichTextBox();
            this.tp_crm = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tbn_CRMpodsumowanieWyslij = new System.Windows.Forms.Button();
            this.cb_Rap18Awaria = new System.Windows.Forms.CheckBox();
            this.btn_CRMPodsumowanie = new System.Windows.Forms.Button();
            this.tb_CRMRapGodzinaDo = new System.Windows.Forms.TextBox();
            this.tb_CRMRapGodzinaOd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtp_CRMRapDataDo = new System.Windows.Forms.DateTimePicker();
            this.dtp_CRMRapDataOd = new System.Windows.Forms.DateTimePicker();
            this.crm_rtb = new System.Windows.Forms.RichTextBox();
            this.btn_CRMRaport18 = new System.Windows.Forms.Button();
            this.tp_zcc = new System.Windows.Forms.TabPage();
            this.btn_ZCCPodsumowanie = new System.Windows.Forms.Button();
            this.btn_ZCCRaport = new System.Windows.Forms.Button();
            this.btn_ZccLoadNotes = new System.Windows.Forms.Button();
            this.zcc_rtb = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_ZCCDataDo = new System.Windows.Forms.DateTimePicker();
            this.dtp_ZCCDataOd = new System.Windows.Forms.DateTimePicker();
            this.tabSlowniki = new System.Windows.Forms.TabPage();
            this.btn_SlownikiZapisz = new System.Windows.Forms.Button();
            this.compDgv = new System.Windows.Forms.DataGridView();
            this.btn_SlownikiOdswiez = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.Historia_dgv = new System.Windows.Forms.DataGridView();
            this.btn_HistoriaSzukaj = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tp_diagnoza = new System.Windows.Forms.TabPage();
            this.ckb_pokazujDrzewko = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_DiagnozaFiltr = new System.Windows.Forms.ComboBox();
            this.cb_diagnozaUsers = new System.Windows.Forms.ComboBox();
            this.dgv_Diagnoza = new System.Windows.Forms.DataGridView();
            this.btn_DiagnozaWczytaj = new System.Windows.Forms.Button();
            this.tp_Sla = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tb_sla_ileWstrzymane = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tb_sla_ileRealizacja = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cb_SLAWstrzymane = new System.Windows.Forms.CheckBox();
            this.bt_SLA_synchronizacja = new System.Windows.Forms.Button();
            this.cb_SLA_JiraSynchro = new System.Windows.Forms.CheckBox();
            this.cb_SLApauza = new System.Windows.Forms.CheckBox();
            this.btn_slaRaport_Load = new System.Windows.Forms.Button();
            this.dgv_SlaRaport = new System.Windows.Forms.DataGridView();
            this.tp_Multi = new System.Windows.Forms.TabPage();
            this.btn_MultiUsun = new System.Windows.Forms.Button();
            this.p_MultiEventMoveButtons = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_MultiClear = new System.Windows.Forms.Button();
            this.btn_MultiDodaj = new System.Windows.Forms.Button();
            this.lb_MultiList = new System.Windows.Forms.ListBox();
            this.tp_Workload = new System.Windows.Forms.TabPage();
            this.gBox_WorkloadRaports = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.wl_PausedIssuesListView = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label10 = new System.Windows.Forms.Label();
            this.wl_OpenIssuesListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gBox_WorkloadCurrent = new System.Windows.Forms.GroupBox();
            this.lb_WorkloadTotalTime = new System.Windows.Forms.Label();
            this.lb_WorkloadLoggedTime = new System.Windows.Forms.Label();
            this.lb_workloadTitle = new System.Windows.Forms.Label();
            this.tp_awaria_nowa = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_awaria_niezakonczona = new System.Windows.Forms.CheckBox();
            this.bt_awaria_zapisz = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.gb_awaria_bloker = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.gb_awaria_onCall = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tb_awaria_nrJira = new System.Windows.Forms.TextBox();
            this.tb_awaria_komentarz = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tb_awaria_opis = new System.Windows.Forms.TextBox();
            this.dtp_awaria_stop = new System.Windows.Forms.DateTimePicker();
            this.dtp_awaria_start = new System.Windows.Forms.DateTimePicker();
            this.tp_awarie = new System.Windows.Forms.TabPage();
            this.tc_awarie = new System.Windows.Forms.TabControl();
            this.tp_awaria_lista = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_Awarie = new System.Windows.Forms.DataGridView();
            this.bt_LA_szukaj = new System.Windows.Forms.Button();
            this.tb_LA_nrJira = new System.Windows.Forms.TextBox();
            this.dtp_LA_stopDate = new System.Windows.Forms.DateTimePicker();
            this.dtp_LA_startDate = new System.Windows.Forms.DateTimePicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NrJira = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeSpent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZalogowanyCzas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZalogowanyCzasMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataLogowania = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Komentarz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Suma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DniowkiSerwisowe = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DataUtworzenia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataModyfikacji = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tp_DayReport = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.bt_dayReportSend = new System.Windows.Forms.Button();
            this.cb_dayReportAccident = new System.Windows.Forms.CheckBox();
            this.tb_dayReportTimeTo = new System.Windows.Forms.TextBox();
            this.tb_dayReportTimeFrom = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.dtp_dayReportDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtp_dayReportDateFrom = new System.Windows.Forms.DateTimePicker();
            this.rtb_dayReportMessage = new System.Windows.Forms.RichTextBox();
            this.bt_dayReportGenerate = new System.Windows.Forms.Button();
            this.tab_Awarie = new System.Windows.Forms.TabControl();
            this.cms_IssuePopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.zalogujSięToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.przelaczStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar3 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            this.issuesCheckoutStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslb_WorkloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.messageToWebService = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uzupełnijDaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ni_NewIssueAlert = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odświeżToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tm_AutoAssigne = new System.Windows.Forms.Timer(this.components);
            this.t_EmailNotification = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tm_autoFrsh = new System.Windows.Forms.Timer(this.components);
            this.pn_FolderSearch = new System.Windows.Forms.FolderBrowserDialog();
            this.fb_logSearch = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.issueTab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tp_Billennium.SuspendLayout();
            this.tp_tmp.SuspendLayout();
            this.tc.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tp_Raporty.SuspendLayout();
            this.tp_Settings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tp_billing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zglDgv)).BeginInit();
            this.tp_crm.SuspendLayout();
            this.tp_zcc.SuspendLayout();
            this.tabSlowniki.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compDgv)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Historia_dgv)).BeginInit();
            this.tp_diagnoza.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Diagnoza)).BeginInit();
            this.tp_Sla.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaRaport)).BeginInit();
            this.tp_Multi.SuspendLayout();
            this.p_MultiEventMoveButtons.SuspendLayout();
            this.tp_Workload.SuspendLayout();
            this.gBox_WorkloadRaports.SuspendLayout();
            this.gBox_WorkloadCurrent.SuspendLayout();
            this.tp_awaria_nowa.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gb_awaria_bloker.SuspendLayout();
            this.gb_awaria_onCall.SuspendLayout();
            this.tp_awarie.SuspendLayout();
            this.tp_awaria_lista.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Awarie)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tp_DayReport.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.btn_NewIssue);
            this.splitContainer1.Panel1.Controls.Add(this.btn_WyszukajWWFS);
            this.splitContainer1.Panel1.Controls.Add(this.issueTab);
            this.splitContainer1.Panel1.Controls.Add(this.numerZgl);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_WyszukajWJira);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.tc);
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            // 
            // btn_NewIssue
            // 
            resources.ApplyResources(this.btn_NewIssue, "btn_NewIssue");
            this.btn_NewIssue.Name = "btn_NewIssue";
            this.btn_NewIssue.UseVisualStyleBackColor = true;
            this.btn_NewIssue.Click += new System.EventHandler(this.btn_NewIssue_Click);
            // 
            // btn_WyszukajWWFS
            // 
            resources.ApplyResources(this.btn_WyszukajWWFS, "btn_WyszukajWWFS");
            this.btn_WyszukajWWFS.Name = "btn_WyszukajWWFS";
            this.btn_WyszukajWWFS.UseVisualStyleBackColor = true;
            this.btn_WyszukajWWFS.Click += new System.EventHandler(this.btn_WyszukajWWFS_Click);
            // 
            // issueTab
            // 
            resources.ApplyResources(this.issueTab, "issueTab");
            this.issueTab.Controls.Add(this.tabPage3);
            this.issueTab.Controls.Add(this.tabPage11);
            this.issueTab.Controls.Add(this.tp_Billennium);
            this.issueTab.Controls.Add(this.tp_tmp);
            this.issueTab.Name = "issueTab";
            this.issueTab.SelectedIndex = 0;
            this.issueTab.SelectedIndexChanged += new System.EventHandler(this.issueTab_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_NieprzydzieloneZapiszDoWFS);
            this.tabPage3.Controls.Add(this.btn_NieprzydzieloneUzupelnijDane);
            this.tabPage3.Controls.Add(this.btn_NieprzydzieloneOdswiez);
            this.tabPage3.Controls.Add(this.treeView1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_NieprzydzieloneZapiszDoWFS
            // 
            resources.ApplyResources(this.btn_NieprzydzieloneZapiszDoWFS, "btn_NieprzydzieloneZapiszDoWFS");
            this.btn_NieprzydzieloneZapiszDoWFS.Name = "btn_NieprzydzieloneZapiszDoWFS";
            this.btn_NieprzydzieloneZapiszDoWFS.UseVisualStyleBackColor = true;
            this.btn_NieprzydzieloneZapiszDoWFS.Click += new System.EventHandler(this.btn_NieprzydzieloneZapiszDoWFS_Click);
            // 
            // btn_NieprzydzieloneUzupelnijDane
            // 
            resources.ApplyResources(this.btn_NieprzydzieloneUzupelnijDane, "btn_NieprzydzieloneUzupelnijDane");
            this.btn_NieprzydzieloneUzupelnijDane.Name = "btn_NieprzydzieloneUzupelnijDane";
            this.btn_NieprzydzieloneUzupelnijDane.UseVisualStyleBackColor = true;
            this.btn_NieprzydzieloneUzupelnijDane.Click += new System.EventHandler(this.btn_NieprzydzieloneUzupelnijDane_Click);
            // 
            // btn_NieprzydzieloneOdswiez
            // 
            resources.ApplyResources(this.btn_NieprzydzieloneOdswiez, "btn_NieprzydzieloneOdswiez");
            this.btn_NieprzydzieloneOdswiez.Name = "btn_NieprzydzieloneOdswiez";
            this.btn_NieprzydzieloneOdswiez.UseVisualStyleBackColor = true;
            this.btn_NieprzydzieloneOdswiez.Click += new System.EventHandler(this.btn_NieprzydzieloneOdsiwez_Click);
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.StateImageList = this.imageList1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "new.png");
            this.imageList1.Images.SetKeyName(1, "old.png");
            this.imageList1.Images.SetKeyName(2, "wait.png");
            this.imageList1.Images.SetKeyName(3, "select.png");
            this.imageList1.Images.SetKeyName(4, "prod.png");
            this.imageList1.Images.SetKeyName(5, "request.png");
            this.imageList1.Images.SetKeyName(6, "ProdBlocker.png");
            this.imageList1.Images.SetKeyName(7, "ProdCritical.png");
            this.imageList1.Images.SetKeyName(8, "ProdMajor.png");
            this.imageList1.Images.SetKeyName(9, "ProdMinor.png");
            this.imageList1.Images.SetKeyName(10, "ProdTrivial.png");
            this.imageList1.Images.SetKeyName(11, "ReqBlocker.png");
            this.imageList1.Images.SetKeyName(12, "ReqCritical.png");
            this.imageList1.Images.SetKeyName(13, "ReqMajor.png");
            this.imageList1.Images.SetKeyName(14, "ReqMinor.png");
            this.imageList1.Images.SetKeyName(15, "ReqTrivial.png");
            this.imageList1.Images.SetKeyName(16, "NewBlocker.png");
            this.imageList1.Images.SetKeyName(17, "NewCritical.png");
            this.imageList1.Images.SetKeyName(18, "NewMajor.png");
            this.imageList1.Images.SetKeyName(19, "NewMinor.png");
            this.imageList1.Images.SetKeyName(20, "NewTrivial.png");
            this.imageList1.Images.SetKeyName(21, "MassBlocker.png");
            this.imageList1.Images.SetKeyName(22, "MassCritical.png");
            this.imageList1.Images.SetKeyName(23, "MassMajor.png");
            this.imageList1.Images.SetKeyName(24, "MassMinor.png");
            this.imageList1.Images.SetKeyName(25, "MassTrivial.png");
            this.imageList1.Images.SetKeyName(26, "IncidentTrivial.png");
            this.imageList1.Images.SetKeyName(27, "IncidentMinor.png");
            this.imageList1.Images.SetKeyName(28, "IncidentMajor.png");
            this.imageList1.Images.SetKeyName(29, "IncidentCritical.png");
            this.imageList1.Images.SetKeyName(30, "IncidentBlocker.png");
            this.imageList1.Images.SetKeyName(31, "dubel.png");
            this.imageList1.Images.SetKeyName(32, "problem.png");
            this.imageList1.Images.SetKeyName(33, "_ProblemBlocker.png");
            this.imageList1.Images.SetKeyName(34, "_ProblemCritical.png");
            this.imageList1.Images.SetKeyName(35, "_ProblemMajor.png");
            this.imageList1.Images.SetKeyName(36, "_ProblemMinor.png");
            this.imageList1.Images.SetKeyName(37, "_ProblemTrivial.png");
            this.imageList1.Images.SetKeyName(38, "INBlocker.png");
            this.imageList1.Images.SetKeyName(39, "INCritical.png");
            this.imageList1.Images.SetKeyName(40, "INMajor.png");
            this.imageList1.Images.SetKeyName(41, "INMinor.png");
            this.imageList1.Images.SetKeyName(42, "INTrivial.png");
            this.imageList1.Images.SetKeyName(43, "ZOBlocker.png");
            this.imageList1.Images.SetKeyName(44, "ZOCritical.png");
            this.imageList1.Images.SetKeyName(45, "ZOMajor.png");
            this.imageList1.Images.SetKeyName(46, "ZOMinor.png");
            this.imageList1.Images.SetKeyName(47, "ZOTrivial.png");
            this.imageList1.Images.SetKeyName(48, "PNBlocker.png");
            this.imageList1.Images.SetKeyName(49, "PNCritical.png");
            this.imageList1.Images.SetKeyName(50, "PNMajor.png");
            this.imageList1.Images.SetKeyName(51, "PNMinor.png");
            this.imageList1.Images.SetKeyName(52, "PNTrivial.png");
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.btn_MojeZapiszDoWFS);
            this.tabPage11.Controls.Add(this.btn_MojeUzupelnijDane);
            this.tabPage11.Controls.Add(this.btn_MojeOdswiez);
            this.tabPage11.Controls.Add(this.treeView2);
            resources.ApplyResources(this.tabPage11, "tabPage11");
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // btn_MojeZapiszDoWFS
            // 
            resources.ApplyResources(this.btn_MojeZapiszDoWFS, "btn_MojeZapiszDoWFS");
            this.btn_MojeZapiszDoWFS.Name = "btn_MojeZapiszDoWFS";
            this.btn_MojeZapiszDoWFS.UseVisualStyleBackColor = true;
            this.btn_MojeZapiszDoWFS.Click += new System.EventHandler(this.btn_MojeZapiszDoWFS_Click);
            // 
            // btn_MojeUzupelnijDane
            // 
            resources.ApplyResources(this.btn_MojeUzupelnijDane, "btn_MojeUzupelnijDane");
            this.btn_MojeUzupelnijDane.Name = "btn_MojeUzupelnijDane";
            this.btn_MojeUzupelnijDane.UseVisualStyleBackColor = true;
            this.btn_MojeUzupelnijDane.Click += new System.EventHandler(this.btn_NieprzydzieloneUzupelnijDane_Click);
            // 
            // btn_MojeOdswiez
            // 
            resources.ApplyResources(this.btn_MojeOdswiez, "btn_MojeOdswiez");
            this.btn_MojeOdswiez.Name = "btn_MojeOdswiez";
            this.btn_MojeOdswiez.UseVisualStyleBackColor = true;
            this.btn_MojeOdswiez.Click += new System.EventHandler(this.btn_MojeOdswiez_Click);
            // 
            // treeView2
            // 
            resources.ApplyResources(this.treeView2, "treeView2");
            this.treeView2.ImageList = this.imageList1;
            this.treeView2.Name = "treeView2";
            this.treeView2.ShowLines = false;
            this.treeView2.ShowRootLines = false;
            this.treeView2.StateImageList = this.imageList1;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView2.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // tp_Billennium
            // 
            this.tp_Billennium.Controls.Add(this.btn_BillenniumZapisz);
            this.tp_Billennium.Controls.Add(this.btn_BillenniumUzupelnijDane);
            this.tp_Billennium.Controls.Add(this.btn_BillenniumOdswiez);
            this.tp_Billennium.Controls.Add(this.treeView3);
            resources.ApplyResources(this.tp_Billennium, "tp_Billennium");
            this.tp_Billennium.Name = "tp_Billennium";
            this.tp_Billennium.UseVisualStyleBackColor = true;
            // 
            // btn_BillenniumZapisz
            // 
            resources.ApplyResources(this.btn_BillenniumZapisz, "btn_BillenniumZapisz");
            this.btn_BillenniumZapisz.Name = "btn_BillenniumZapisz";
            this.btn_BillenniumZapisz.UseVisualStyleBackColor = true;
            this.btn_BillenniumZapisz.Click += new System.EventHandler(this.btn_BillenniumZapisz_Click);
            // 
            // btn_BillenniumUzupelnijDane
            // 
            resources.ApplyResources(this.btn_BillenniumUzupelnijDane, "btn_BillenniumUzupelnijDane");
            this.btn_BillenniumUzupelnijDane.Name = "btn_BillenniumUzupelnijDane";
            this.btn_BillenniumUzupelnijDane.UseVisualStyleBackColor = true;
            this.btn_BillenniumUzupelnijDane.Click += new System.EventHandler(this.btn_BillenniumUzupelnijDane_Click_1);
            // 
            // btn_BillenniumOdswiez
            // 
            resources.ApplyResources(this.btn_BillenniumOdswiez, "btn_BillenniumOdswiez");
            this.btn_BillenniumOdswiez.Name = "btn_BillenniumOdswiez";
            this.btn_BillenniumOdswiez.UseVisualStyleBackColor = true;
            this.btn_BillenniumOdswiez.Click += new System.EventHandler(this.btn_BillenniumOdswiez_Click);
            // 
            // treeView3
            // 
            resources.ApplyResources(this.treeView3, "treeView3");
            this.treeView3.ImageList = this.imageList1;
            this.treeView3.Name = "treeView3";
            this.treeView3.ShowLines = false;
            this.treeView3.ShowRootLines = false;
            this.treeView3.StateImageList = this.imageList1;
            this.treeView3.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView3.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // tp_tmp
            // 
            this.tp_tmp.Controls.Add(this.treeView4);
            resources.ApplyResources(this.tp_tmp, "tp_tmp");
            this.tp_tmp.Name = "tp_tmp";
            this.tp_tmp.UseVisualStyleBackColor = true;
            // 
            // treeView4
            // 
            resources.ApplyResources(this.treeView4, "treeView4");
            this.treeView4.Name = "treeView4";
            // 
            // numerZgl
            // 
            resources.ApplyResources(this.numerZgl, "numerZgl");
            this.numerZgl.Name = "numerZgl";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btn_WyszukajWJira
            // 
            resources.ApplyResources(this.btn_WyszukajWJira, "btn_WyszukajWJira");
            this.btn_WyszukajWJira.Name = "btn_WyszukajWJira";
            this.btn_WyszukajWJira.UseVisualStyleBackColor = true;
            this.btn_WyszukajWJira.Click += new System.EventHandler(this.btn_WyszukajWJira_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // tc
            // 
            resources.ApplyResources(this.tc, "tc");
            this.tc.Controls.Add(this.tabPage1);
            this.tc.Controls.Add(this.tp_Raporty);
            this.tc.Controls.Add(this.tp_Settings);
            this.tc.Controls.Add(this.tp_billing);
            this.tc.Controls.Add(this.tp_crm);
            this.tc.Controls.Add(this.tp_zcc);
            this.tc.Controls.Add(this.tabSlowniki);
            this.tc.Controls.Add(this.tabPage6);
            this.tc.Controls.Add(this.tp_diagnoza);
            this.tc.Controls.Add(this.tp_Sla);
            this.tc.Controls.Add(this.tp_Multi);
            this.tc.Controls.Add(this.tp_Workload);
            this.tc.Controls.Add(this.tp_awaria_nowa);
            this.tc.Controls.Add(this.tp_awarie);
            this.tc.Controls.Add(this.tp_awaria_lista);
            this.tc.Controls.Add(this.tabPage2);
            this.tc.Controls.Add(this.tp_DayReport);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.bt_AutoAssigne);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btn_DodajNotatke);
            this.tabPage1.Controls.Add(this.btn_Procesy);
            this.tabPage1.Controls.Add(this.richTextBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bt_AutoAssigne
            // 
            resources.ApplyResources(this.bt_AutoAssigne, "bt_AutoAssigne");
            this.bt_AutoAssigne.Name = "bt_AutoAssigne";
            this.bt_AutoAssigne.UseVisualStyleBackColor = true;
            this.bt_AutoAssigne.Click += new System.EventHandler(this.bt_autoAssigne_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // btn_DodajNotatke
            // 
            resources.ApplyResources(this.btn_DodajNotatke, "btn_DodajNotatke");
            this.btn_DodajNotatke.Name = "btn_DodajNotatke";
            this.btn_DodajNotatke.UseVisualStyleBackColor = true;
            this.btn_DodajNotatke.Click += new System.EventHandler(this.btn_DodajNotatke_Click);
            // 
            // btn_Procesy
            // 
            resources.ApplyResources(this.btn_Procesy, "btn_Procesy");
            this.btn_Procesy.Name = "btn_Procesy";
            this.btn_Procesy.UseVisualStyleBackColor = true;
            this.btn_Procesy.Click += new System.EventHandler(this.btn_Procesy_Click);
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // tp_Raporty
            // 
            this.tp_Raporty.Controls.Add(this.tab_Raporty);
            resources.ApplyResources(this.tp_Raporty, "tp_Raporty");
            this.tp_Raporty.Name = "tp_Raporty";
            this.tp_Raporty.UseVisualStyleBackColor = true;
            // 
            // tab_Raporty
            // 
            resources.ApplyResources(this.tab_Raporty, "tab_Raporty");
            this.tab_Raporty.Multiline = true;
            this.tab_Raporty.Name = "tab_Raporty";
            this.tab_Raporty.SelectedIndex = 0;
            this.tab_Raporty.SelectedIndexChanged += new System.EventHandler(this.tab_Raporty_SelectedIndexChanged);
            // 
            // tp_Settings
            // 
            this.tp_Settings.Controls.Add(this.groupBox4);
            this.tp_Settings.Controls.Add(this.groupBox3);
            this.tp_Settings.Controls.Add(this.groupBox2);
            this.tp_Settings.Controls.Add(this.cbox_RefreshBoth);
            this.tp_Settings.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tp_Settings, "tp_Settings");
            this.tp_Settings.Name = "tp_Settings";
            this.tp_Settings.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.bt_LogSearchRun);
            this.groupBox4.Controls.Add(this.bt_LogSearchPatch);
            this.groupBox4.Controls.Add(this.tb_LogSearchPath);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // bt_LogSearchRun
            // 
            resources.ApplyResources(this.bt_LogSearchRun, "bt_LogSearchRun");
            this.bt_LogSearchRun.Name = "bt_LogSearchRun";
            this.bt_LogSearchRun.UseVisualStyleBackColor = true;
            this.bt_LogSearchRun.Click += new System.EventHandler(this.bt_LogSearchRun_Click);
            // 
            // bt_LogSearchPatch
            // 
            resources.ApplyResources(this.bt_LogSearchPatch, "bt_LogSearchPatch");
            this.bt_LogSearchPatch.Name = "bt_LogSearchPatch";
            this.bt_LogSearchPatch.UseVisualStyleBackColor = true;
            this.bt_LogSearchPatch.Click += new System.EventHandler(this.bt_LogSearchPatch_Click);
            // 
            // tb_LogSearchPath
            // 
            resources.ApplyResources(this.tb_LogSearchPath, "tb_LogSearchPath");
            this.tb_LogSearchPath.Name = "tb_LogSearchPath";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_KontoBillennium);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.bt_KontoBillenniumSave);
            this.groupBox3.Controls.Add(this.mtb_BillenniumPass);
            this.groupBox3.Controls.Add(this.label21);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // cb_KontoBillennium
            // 
            resources.ApplyResources(this.cb_KontoBillennium, "cb_KontoBillennium");
            this.cb_KontoBillennium.Name = "cb_KontoBillennium";
            this.cb_KontoBillennium.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // bt_KontoBillenniumSave
            // 
            resources.ApplyResources(this.bt_KontoBillenniumSave, "bt_KontoBillenniumSave");
            this.bt_KontoBillenniumSave.Name = "bt_KontoBillenniumSave";
            this.bt_KontoBillenniumSave.UseVisualStyleBackColor = true;
            this.bt_KontoBillenniumSave.Click += new System.EventHandler(this.bt_KontoBillenniumSave_Click);
            // 
            // mtb_BillenniumPass
            // 
            resources.ApplyResources(this.mtb_BillenniumPass, "mtb_BillenniumPass");
            this.mtb_BillenniumPass.Name = "mtb_BillenniumPass";
            this.mtb_BillenniumPass.UseSystemPasswordChar = true;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SaveFilters);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.tb_UnassignedFilterName);
            this.groupBox2.Controls.Add(this.tb_AssignedFilterName);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btn_SaveFilters
            // 
            resources.ApplyResources(this.btn_SaveFilters, "btn_SaveFilters");
            this.btn_SaveFilters.Name = "btn_SaveFilters";
            this.btn_SaveFilters.UseVisualStyleBackColor = true;
            this.btn_SaveFilters.Click += new System.EventHandler(this.btn_SaveFilters_Click);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // tb_UnassignedFilterName
            // 
            resources.ApplyResources(this.tb_UnassignedFilterName, "tb_UnassignedFilterName");
            this.tb_UnassignedFilterName.Name = "tb_UnassignedFilterName";
            // 
            // tb_AssignedFilterName
            // 
            resources.ApplyResources(this.tb_AssignedFilterName, "tb_AssignedFilterName");
            this.tb_AssignedFilterName.Name = "tb_AssignedFilterName";
            // 
            // cbox_RefreshBoth
            // 
            resources.ApplyResources(this.cbox_RefreshBoth, "cbox_RefreshBoth");
            this.cbox_RefreshBoth.Name = "cbox_RefreshBoth";
            this.cbox_RefreshBoth.UseVisualStyleBackColor = true;
            this.cbox_RefreshBoth.CheckedChanged += new System.EventHandler(this.cbox_RefreshBoth_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.btn_SoundNotificationOpenFileDialog);
            this.groupBox1.Controls.Add(this.btn_PowiadomienieWlacz);
            this.groupBox1.Controls.Add(this.tb_SoundNotificationPath);
            this.groupBox1.Controls.Add(this.notifyTimeoutTextBox);
            this.groupBox1.Controls.Add(this.cb_SoundNotification);
            this.groupBox1.Controls.Add(this.ckb_isNotifyEnabled);
            this.groupBox1.Controls.Add(this.cb_EmailNotification);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // btn_SoundNotificationOpenFileDialog
            // 
            resources.ApplyResources(this.btn_SoundNotificationOpenFileDialog, "btn_SoundNotificationOpenFileDialog");
            this.btn_SoundNotificationOpenFileDialog.Name = "btn_SoundNotificationOpenFileDialog";
            this.btn_SoundNotificationOpenFileDialog.UseVisualStyleBackColor = true;
            this.btn_SoundNotificationOpenFileDialog.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_PowiadomienieWlacz
            // 
            resources.ApplyResources(this.btn_PowiadomienieWlacz, "btn_PowiadomienieWlacz");
            this.btn_PowiadomienieWlacz.Name = "btn_PowiadomienieWlacz";
            this.btn_PowiadomienieWlacz.UseVisualStyleBackColor = true;
            this.btn_PowiadomienieWlacz.Click += new System.EventHandler(this.btn_PowiadomienieWlacz_Click);
            // 
            // tb_SoundNotificationPath
            // 
            resources.ApplyResources(this.tb_SoundNotificationPath, "tb_SoundNotificationPath");
            this.tb_SoundNotificationPath.Name = "tb_SoundNotificationPath";
            this.tb_SoundNotificationPath.ReadOnly = true;
            // 
            // notifyTimeoutTextBox
            // 
            resources.ApplyResources(this.notifyTimeoutTextBox, "notifyTimeoutTextBox");
            this.notifyTimeoutTextBox.Name = "notifyTimeoutTextBox";
            // 
            // cb_SoundNotification
            // 
            resources.ApplyResources(this.cb_SoundNotification, "cb_SoundNotification");
            this.cb_SoundNotification.Name = "cb_SoundNotification";
            this.cb_SoundNotification.UseVisualStyleBackColor = true;
            this.cb_SoundNotification.CheckedChanged += new System.EventHandler(this.cb_SoundNotification_CheckedChanged);
            // 
            // ckb_isNotifyEnabled
            // 
            resources.ApplyResources(this.ckb_isNotifyEnabled, "ckb_isNotifyEnabled");
            this.ckb_isNotifyEnabled.Name = "ckb_isNotifyEnabled";
            this.ckb_isNotifyEnabled.UseVisualStyleBackColor = true;
            this.ckb_isNotifyEnabled.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cb_EmailNotification
            // 
            resources.ApplyResources(this.cb_EmailNotification, "cb_EmailNotification");
            this.cb_EmailNotification.Name = "cb_EmailNotification";
            this.cb_EmailNotification.UseVisualStyleBackColor = true;
            this.cb_EmailNotification.CheckedChanged += new System.EventHandler(this.cb_EmailNotification_CheckedChanged);
            // 
            // tp_billing
            // 
            this.tp_billing.Controls.Add(this.btn_BillingZapiszDoXls);
            this.tp_billing.Controls.Add(this.zglDgv);
            this.tp_billing.Controls.Add(this.btn_BillingZgloszeniaKategoryzacja);
            this.tp_billing.Controls.Add(this.tabStatystyki);
            this.tp_billing.Controls.Add(this.btn_BillingZgloszeniaZDzisiaj);
            this.tp_billing.Controls.Add(this.btn_BillingRaportStanuZgloszen);
            this.tp_billing.Controls.Add(this.label3);
            this.tp_billing.Controls.Add(this.label2);
            this.tp_billing.Controls.Add(this.dateTimePicker2);
            this.tp_billing.Controls.Add(this.dateTimePicker1);
            this.tp_billing.Controls.Add(this.richTextBoxStats);
            resources.ApplyResources(this.tp_billing, "tp_billing");
            this.tp_billing.Name = "tp_billing";
            this.tp_billing.UseVisualStyleBackColor = true;
            // 
            // btn_BillingZapiszDoXls
            // 
            resources.ApplyResources(this.btn_BillingZapiszDoXls, "btn_BillingZapiszDoXls");
            this.btn_BillingZapiszDoXls.Name = "btn_BillingZapiszDoXls";
            this.btn_BillingZapiszDoXls.UseVisualStyleBackColor = true;
            this.btn_BillingZapiszDoXls.Click += new System.EventHandler(this.btn_BillingZapiszDoXls_Click);
            // 
            // zglDgv
            // 
            resources.ApplyResources(this.zglDgv, "zglDgv");
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.zglDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.zglDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.zglDgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.zglDgv.Name = "zglDgv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.zglDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            // 
            // btn_BillingZgloszeniaKategoryzacja
            // 
            resources.ApplyResources(this.btn_BillingZgloszeniaKategoryzacja, "btn_BillingZgloszeniaKategoryzacja");
            this.btn_BillingZgloszeniaKategoryzacja.Name = "btn_BillingZgloszeniaKategoryzacja";
            this.btn_BillingZgloszeniaKategoryzacja.UseVisualStyleBackColor = true;
            this.btn_BillingZgloszeniaKategoryzacja.Click += new System.EventHandler(this.btn_BillingZgloszeniaKategoryzacja_Click);
            // 
            // tabStatystyki
            // 
            resources.ApplyResources(this.tabStatystyki, "tabStatystyki");
            this.tabStatystyki.Name = "tabStatystyki";
            this.tabStatystyki.SelectedIndex = 0;
            // 
            // btn_BillingZgloszeniaZDzisiaj
            // 
            resources.ApplyResources(this.btn_BillingZgloszeniaZDzisiaj, "btn_BillingZgloszeniaZDzisiaj");
            this.btn_BillingZgloszeniaZDzisiaj.Name = "btn_BillingZgloszeniaZDzisiaj";
            this.btn_BillingZgloszeniaZDzisiaj.UseVisualStyleBackColor = true;
            this.btn_BillingZgloszeniaZDzisiaj.Click += new System.EventHandler(this.btn_BillingZgloszeniaZDzisiaj_Click);
            // 
            // btn_BillingRaportStanuZgloszen
            // 
            resources.ApplyResources(this.btn_BillingRaportStanuZgloszen, "btn_BillingRaportStanuZgloszen");
            this.btn_BillingRaportStanuZgloszen.Name = "btn_BillingRaportStanuZgloszen";
            this.btn_BillingRaportStanuZgloszen.UseVisualStyleBackColor = true;
            this.btn_BillingRaportStanuZgloszen.Click += new System.EventHandler(this.btn_BillingRaportStanuZgloszen_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dateTimePicker2
            // 
            resources.ApplyResources(this.dateTimePicker2, "dateTimePicker2");
            this.dateTimePicker2.Name = "dateTimePicker2";
            // 
            // dateTimePicker1
            // 
            resources.ApplyResources(this.dateTimePicker1, "dateTimePicker1");
            this.dateTimePicker1.Name = "dateTimePicker1";
            // 
            // richTextBoxStats
            // 
            resources.ApplyResources(this.richTextBoxStats, "richTextBoxStats");
            this.richTextBoxStats.Name = "richTextBoxStats";
            // 
            // tp_crm
            // 
            this.tp_crm.Controls.Add(this.button3);
            this.tp_crm.Controls.Add(this.webBrowser1);
            this.tp_crm.Controls.Add(this.tbn_CRMpodsumowanieWyslij);
            this.tp_crm.Controls.Add(this.cb_Rap18Awaria);
            this.tp_crm.Controls.Add(this.btn_CRMPodsumowanie);
            this.tp_crm.Controls.Add(this.tb_CRMRapGodzinaDo);
            this.tp_crm.Controls.Add(this.tb_CRMRapGodzinaOd);
            this.tp_crm.Controls.Add(this.label8);
            this.tp_crm.Controls.Add(this.label7);
            this.tp_crm.Controls.Add(this.dtp_CRMRapDataDo);
            this.tp_crm.Controls.Add(this.dtp_CRMRapDataOd);
            this.tp_crm.Controls.Add(this.crm_rtb);
            this.tp_crm.Controls.Add(this.btn_CRMRaport18);
            resources.ApplyResources(this.tp_crm, "tp_crm");
            this.tp_crm.Name = "tp_crm";
            this.tp_crm.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            // 
            // tbn_CRMpodsumowanieWyslij
            // 
            resources.ApplyResources(this.tbn_CRMpodsumowanieWyslij, "tbn_CRMpodsumowanieWyslij");
            this.tbn_CRMpodsumowanieWyslij.Name = "tbn_CRMpodsumowanieWyslij";
            this.tbn_CRMpodsumowanieWyslij.UseVisualStyleBackColor = true;
            this.tbn_CRMpodsumowanieWyslij.Click += new System.EventHandler(this.tbn_CRMpodsumowanieWyslij_Click);
            // 
            // cb_Rap18Awaria
            // 
            resources.ApplyResources(this.cb_Rap18Awaria, "cb_Rap18Awaria");
            this.cb_Rap18Awaria.Name = "cb_Rap18Awaria";
            this.cb_Rap18Awaria.UseVisualStyleBackColor = true;
            // 
            // btn_CRMPodsumowanie
            // 
            resources.ApplyResources(this.btn_CRMPodsumowanie, "btn_CRMPodsumowanie");
            this.btn_CRMPodsumowanie.Name = "btn_CRMPodsumowanie";
            this.btn_CRMPodsumowanie.UseVisualStyleBackColor = true;
            this.btn_CRMPodsumowanie.Click += new System.EventHandler(this.btn_CRMPodsumowanie_Click);
            // 
            // tb_CRMRapGodzinaDo
            // 
            resources.ApplyResources(this.tb_CRMRapGodzinaDo, "tb_CRMRapGodzinaDo");
            this.tb_CRMRapGodzinaDo.Name = "tb_CRMRapGodzinaDo";
            // 
            // tb_CRMRapGodzinaOd
            // 
            resources.ApplyResources(this.tb_CRMRapGodzinaOd, "tb_CRMRapGodzinaOd");
            this.tb_CRMRapGodzinaOd.Name = "tb_CRMRapGodzinaOd";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dtp_CRMRapDataDo
            // 
            resources.ApplyResources(this.dtp_CRMRapDataDo, "dtp_CRMRapDataDo");
            this.dtp_CRMRapDataDo.Name = "dtp_CRMRapDataDo";
            // 
            // dtp_CRMRapDataOd
            // 
            resources.ApplyResources(this.dtp_CRMRapDataOd, "dtp_CRMRapDataOd");
            this.dtp_CRMRapDataOd.Name = "dtp_CRMRapDataOd";
            // 
            // crm_rtb
            // 
            resources.ApplyResources(this.crm_rtb, "crm_rtb");
            this.crm_rtb.Name = "crm_rtb";
            this.crm_rtb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // btn_CRMRaport18
            // 
            resources.ApplyResources(this.btn_CRMRaport18, "btn_CRMRaport18");
            this.btn_CRMRaport18.Name = "btn_CRMRaport18";
            this.btn_CRMRaport18.UseVisualStyleBackColor = true;
            this.btn_CRMRaport18.Click += new System.EventHandler(this.btn_CRMRaport18_Click);
            // 
            // tp_zcc
            // 
            this.tp_zcc.Controls.Add(this.btn_ZCCPodsumowanie);
            this.tp_zcc.Controls.Add(this.btn_ZCCRaport);
            this.tp_zcc.Controls.Add(this.btn_ZccLoadNotes);
            this.tp_zcc.Controls.Add(this.zcc_rtb);
            this.tp_zcc.Controls.Add(this.label6);
            this.tp_zcc.Controls.Add(this.label5);
            this.tp_zcc.Controls.Add(this.dtp_ZCCDataDo);
            this.tp_zcc.Controls.Add(this.dtp_ZCCDataOd);
            resources.ApplyResources(this.tp_zcc, "tp_zcc");
            this.tp_zcc.Name = "tp_zcc";
            this.tp_zcc.UseVisualStyleBackColor = true;
            // 
            // btn_ZCCPodsumowanie
            // 
            resources.ApplyResources(this.btn_ZCCPodsumowanie, "btn_ZCCPodsumowanie");
            this.btn_ZCCPodsumowanie.Name = "btn_ZCCPodsumowanie";
            this.btn_ZCCPodsumowanie.UseVisualStyleBackColor = true;
            this.btn_ZCCPodsumowanie.Click += new System.EventHandler(this.btn_ZCCPodsumowanie_Click);
            // 
            // btn_ZCCRaport
            // 
            resources.ApplyResources(this.btn_ZCCRaport, "btn_ZCCRaport");
            this.btn_ZCCRaport.Name = "btn_ZCCRaport";
            this.btn_ZCCRaport.UseVisualStyleBackColor = true;
            this.btn_ZCCRaport.Click += new System.EventHandler(this.btn_ZCCRaport_Click);
            // 
            // btn_ZccLoadNotes
            // 
            resources.ApplyResources(this.btn_ZccLoadNotes, "btn_ZccLoadNotes");
            this.btn_ZccLoadNotes.Name = "btn_ZccLoadNotes";
            this.btn_ZccLoadNotes.UseVisualStyleBackColor = true;
            this.btn_ZccLoadNotes.Click += new System.EventHandler(this.btn_ZccLoadNotes_Click);
            // 
            // zcc_rtb
            // 
            resources.ApplyResources(this.zcc_rtb, "zcc_rtb");
            this.zcc_rtb.Name = "zcc_rtb";
            this.zcc_rtb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtp_ZCCDataDo
            // 
            resources.ApplyResources(this.dtp_ZCCDataDo, "dtp_ZCCDataDo");
            this.dtp_ZCCDataDo.Name = "dtp_ZCCDataDo";
            // 
            // dtp_ZCCDataOd
            // 
            resources.ApplyResources(this.dtp_ZCCDataOd, "dtp_ZCCDataOd");
            this.dtp_ZCCDataOd.Name = "dtp_ZCCDataOd";
            // 
            // tabSlowniki
            // 
            this.tabSlowniki.Controls.Add(this.btn_SlownikiZapisz);
            this.tabSlowniki.Controls.Add(this.compDgv);
            this.tabSlowniki.Controls.Add(this.btn_SlownikiOdswiez);
            resources.ApplyResources(this.tabSlowniki, "tabSlowniki");
            this.tabSlowniki.Name = "tabSlowniki";
            this.tabSlowniki.UseVisualStyleBackColor = true;
            // 
            // btn_SlownikiZapisz
            // 
            resources.ApplyResources(this.btn_SlownikiZapisz, "btn_SlownikiZapisz");
            this.btn_SlownikiZapisz.Name = "btn_SlownikiZapisz";
            this.btn_SlownikiZapisz.UseVisualStyleBackColor = true;
            this.btn_SlownikiZapisz.Click += new System.EventHandler(this.btn_SlownikiZapisz_Click);
            // 
            // compDgv
            // 
            resources.ApplyResources(this.compDgv, "compDgv");
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.compDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.compDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.compDgv.DefaultCellStyle = dataGridViewCellStyle5;
            this.compDgv.Name = "compDgv";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.compDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.compDgv.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);
            // 
            // btn_SlownikiOdswiez
            // 
            resources.ApplyResources(this.btn_SlownikiOdswiez, "btn_SlownikiOdswiez");
            this.btn_SlownikiOdswiez.Name = "btn_SlownikiOdswiez";
            this.btn_SlownikiOdswiez.UseVisualStyleBackColor = true;
            this.btn_SlownikiOdswiez.Click += new System.EventHandler(this.btn_SlownikiOdswiez_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.Historia_dgv);
            this.tabPage6.Controls.Add(this.btn_HistoriaSzukaj);
            this.tabPage6.Controls.Add(this.textBox2);
            resources.ApplyResources(this.tabPage6, "tabPage6");
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // Historia_dgv
            // 
            resources.ApplyResources(this.Historia_dgv, "Historia_dgv");
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Historia_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.Historia_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Historia_dgv.DefaultCellStyle = dataGridViewCellStyle8;
            this.Historia_dgv.Name = "Historia_dgv";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Historia_dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.Historia_dgv.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Historia_dgvCellContentClick);
            // 
            // btn_HistoriaSzukaj
            // 
            resources.ApplyResources(this.btn_HistoriaSzukaj, "btn_HistoriaSzukaj");
            this.btn_HistoriaSzukaj.Name = "btn_HistoriaSzukaj";
            this.btn_HistoriaSzukaj.UseVisualStyleBackColor = true;
            this.btn_HistoriaSzukaj.Click += new System.EventHandler(this.btn_HistoriaSzukaj_Click2);
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // tp_diagnoza
            // 
            this.tp_diagnoza.Controls.Add(this.ckb_pokazujDrzewko);
            this.tp_diagnoza.Controls.Add(this.label4);
            this.tp_diagnoza.Controls.Add(this.cb_DiagnozaFiltr);
            this.tp_diagnoza.Controls.Add(this.cb_diagnozaUsers);
            this.tp_diagnoza.Controls.Add(this.dgv_Diagnoza);
            this.tp_diagnoza.Controls.Add(this.btn_DiagnozaWczytaj);
            resources.ApplyResources(this.tp_diagnoza, "tp_diagnoza");
            this.tp_diagnoza.Name = "tp_diagnoza";
            this.tp_diagnoza.UseVisualStyleBackColor = true;
            // 
            // ckb_pokazujDrzewko
            // 
            resources.ApplyResources(this.ckb_pokazujDrzewko, "ckb_pokazujDrzewko");
            this.ckb_pokazujDrzewko.Name = "ckb_pokazujDrzewko";
            this.ckb_pokazujDrzewko.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cb_DiagnozaFiltr
            // 
            this.cb_DiagnozaFiltr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DiagnozaFiltr.FormattingEnabled = true;
            this.cb_DiagnozaFiltr.Items.AddRange(new object[] {
            resources.GetString("cb_DiagnozaFiltr.Items"),
            resources.GetString("cb_DiagnozaFiltr.Items1"),
            resources.GetString("cb_DiagnozaFiltr.Items2"),
            resources.GetString("cb_DiagnozaFiltr.Items3")});
            resources.ApplyResources(this.cb_DiagnozaFiltr, "cb_DiagnozaFiltr");
            this.cb_DiagnozaFiltr.Name = "cb_DiagnozaFiltr";
            // 
            // cb_diagnozaUsers
            // 
            this.cb_diagnozaUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_diagnozaUsers.FormattingEnabled = true;
            resources.ApplyResources(this.cb_diagnozaUsers, "cb_diagnozaUsers");
            this.cb_diagnozaUsers.Name = "cb_diagnozaUsers";
            // 
            // dgv_Diagnoza
            // 
            resources.ApplyResources(this.dgv_Diagnoza, "dgv_Diagnoza");
            this.dgv_Diagnoza.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Diagnoza.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_Diagnoza.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Diagnoza.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgv_Diagnoza.Name = "dgv_Diagnoza";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Diagnoza.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgv_Diagnoza.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Diagnoza_CellContentClick);
            // 
            // btn_DiagnozaWczytaj
            // 
            resources.ApplyResources(this.btn_DiagnozaWczytaj, "btn_DiagnozaWczytaj");
            this.btn_DiagnozaWczytaj.Name = "btn_DiagnozaWczytaj";
            this.btn_DiagnozaWczytaj.UseVisualStyleBackColor = true;
            this.btn_DiagnozaWczytaj.Click += new System.EventHandler(this.btn_DiagnozaWczytaj_Click);
            // 
            // tp_Sla
            // 
            this.tp_Sla.Controls.Add(this.panel4);
            this.tp_Sla.Controls.Add(this.cb_SLAWstrzymane);
            this.tp_Sla.Controls.Add(this.bt_SLA_synchronizacja);
            this.tp_Sla.Controls.Add(this.cb_SLA_JiraSynchro);
            this.tp_Sla.Controls.Add(this.cb_SLApauza);
            this.tp_Sla.Controls.Add(this.btn_slaRaport_Load);
            this.tp_Sla.Controls.Add(this.dgv_SlaRaport);
            resources.ApplyResources(this.tp_Sla, "tp_Sla");
            this.tp_Sla.Name = "tp_Sla";
            this.tp_Sla.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tb_sla_ileWstrzymane);
            this.panel4.Controls.Add(this.label29);
            this.panel4.Controls.Add(this.tb_sla_ileRealizacja);
            this.panel4.Controls.Add(this.label28);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // tb_sla_ileWstrzymane
            // 
            resources.ApplyResources(this.tb_sla_ileWstrzymane, "tb_sla_ileWstrzymane");
            this.tb_sla_ileWstrzymane.Name = "tb_sla_ileWstrzymane";
            this.tb_sla_ileWstrzymane.ReadOnly = true;
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // tb_sla_ileRealizacja
            // 
            resources.ApplyResources(this.tb_sla_ileRealizacja, "tb_sla_ileRealizacja");
            this.tb_sla_ileRealizacja.Name = "tb_sla_ileRealizacja";
            this.tb_sla_ileRealizacja.ReadOnly = true;
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // cb_SLAWstrzymane
            // 
            resources.ApplyResources(this.cb_SLAWstrzymane, "cb_SLAWstrzymane");
            this.cb_SLAWstrzymane.Name = "cb_SLAWstrzymane";
            this.cb_SLAWstrzymane.UseVisualStyleBackColor = true;
            this.cb_SLAWstrzymane.CheckedChanged += new System.EventHandler(this.cb_SLApauza_CheckedChanged);
            // 
            // bt_SLA_synchronizacja
            // 
            resources.ApplyResources(this.bt_SLA_synchronizacja, "bt_SLA_synchronizacja");
            this.bt_SLA_synchronizacja.Name = "bt_SLA_synchronizacja";
            this.bt_SLA_synchronizacja.UseVisualStyleBackColor = true;
            this.bt_SLA_synchronizacja.Click += new System.EventHandler(this.bt_SLA_synchronizacja_Click);
            // 
            // cb_SLA_JiraSynchro
            // 
            resources.ApplyResources(this.cb_SLA_JiraSynchro, "cb_SLA_JiraSynchro");
            this.cb_SLA_JiraSynchro.Checked = true;
            this.cb_SLA_JiraSynchro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_SLA_JiraSynchro.Name = "cb_SLA_JiraSynchro";
            this.cb_SLA_JiraSynchro.UseVisualStyleBackColor = true;
            this.cb_SLA_JiraSynchro.CheckedChanged += new System.EventHandler(this.cb_SLA_JiraSynchro_CheckedChanged);
            // 
            // cb_SLApauza
            // 
            resources.ApplyResources(this.cb_SLApauza, "cb_SLApauza");
            this.cb_SLApauza.Name = "cb_SLApauza";
            this.cb_SLApauza.UseVisualStyleBackColor = true;
            this.cb_SLApauza.CheckedChanged += new System.EventHandler(this.cb_SLApauza_CheckedChanged);
            // 
            // btn_slaRaport_Load
            // 
            resources.ApplyResources(this.btn_slaRaport_Load, "btn_slaRaport_Load");
            this.btn_slaRaport_Load.Name = "btn_slaRaport_Load";
            this.btn_slaRaport_Load.UseVisualStyleBackColor = true;
            this.btn_slaRaport_Load.Click += new System.EventHandler(this.btn_slaRaport_Load_Click);
            // 
            // dgv_SlaRaport
            // 
            resources.ApplyResources(this.dgv_SlaRaport, "dgv_SlaRaport");
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SlaRaport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgv_SlaRaport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_SlaRaport.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgv_SlaRaport.Name = "dgv_SlaRaport";
            this.dgv_SlaRaport.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaportCellContentClick);
            this.dgv_SlaRaport.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaport_CellEnter);
            this.dgv_SlaRaport.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaport_CellLeave);
            this.dgv_SlaRaport.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_SlaRaport_CellMouseClick);
            // 
            // tp_Multi
            // 
            this.tp_Multi.Controls.Add(this.btn_MultiUsun);
            this.tp_Multi.Controls.Add(this.p_MultiEventMoveButtons);
            this.tp_Multi.Controls.Add(this.btn_MultiClear);
            this.tp_Multi.Controls.Add(this.btn_MultiDodaj);
            this.tp_Multi.Controls.Add(this.lb_MultiList);
            resources.ApplyResources(this.tp_Multi, "tp_Multi");
            this.tp_Multi.Name = "tp_Multi";
            this.tp_Multi.UseVisualStyleBackColor = true;
            // 
            // btn_MultiUsun
            // 
            resources.ApplyResources(this.btn_MultiUsun, "btn_MultiUsun");
            this.btn_MultiUsun.Name = "btn_MultiUsun";
            this.btn_MultiUsun.UseVisualStyleBackColor = true;
            this.btn_MultiUsun.Click += new System.EventHandler(this.btn_MultiUsun_Click);
            // 
            // p_MultiEventMoveButtons
            // 
            resources.ApplyResources(this.p_MultiEventMoveButtons, "p_MultiEventMoveButtons");
            this.p_MultiEventMoveButtons.Controls.Add(this.label9);
            this.p_MultiEventMoveButtons.Name = "p_MultiEventMoveButtons";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btn_MultiClear
            // 
            resources.ApplyResources(this.btn_MultiClear, "btn_MultiClear");
            this.btn_MultiClear.Name = "btn_MultiClear";
            this.btn_MultiClear.UseVisualStyleBackColor = true;
            this.btn_MultiClear.Click += new System.EventHandler(this.btn_MultiClear_Click);
            // 
            // btn_MultiDodaj
            // 
            resources.ApplyResources(this.btn_MultiDodaj, "btn_MultiDodaj");
            this.btn_MultiDodaj.Name = "btn_MultiDodaj";
            this.btn_MultiDodaj.UseVisualStyleBackColor = true;
            this.btn_MultiDodaj.Click += new System.EventHandler(this.btn_MultiDodaj_Click);
            // 
            // lb_MultiList
            // 
            this.lb_MultiList.AllowDrop = true;
            resources.ApplyResources(this.lb_MultiList, "lb_MultiList");
            this.lb_MultiList.FormattingEnabled = true;
            this.lb_MultiList.Name = "lb_MultiList";
            this.lb_MultiList.SelectedIndexChanged += new System.EventHandler(this.lb_MultiList_SelectedIndexChanged);
            // 
            // tp_Workload
            // 
            this.tp_Workload.Controls.Add(this.gBox_WorkloadRaports);
            this.tp_Workload.Controls.Add(this.gBox_WorkloadCurrent);
            resources.ApplyResources(this.tp_Workload, "tp_Workload");
            this.tp_Workload.Name = "tp_Workload";
            this.tp_Workload.UseVisualStyleBackColor = true;
            // 
            // gBox_WorkloadRaports
            // 
            resources.ApplyResources(this.gBox_WorkloadRaports, "gBox_WorkloadRaports");
            this.gBox_WorkloadRaports.Controls.Add(this.label11);
            this.gBox_WorkloadRaports.Controls.Add(this.wl_PausedIssuesListView);
            this.gBox_WorkloadRaports.Controls.Add(this.label10);
            this.gBox_WorkloadRaports.Controls.Add(this.wl_OpenIssuesListView);
            this.gBox_WorkloadRaports.Name = "gBox_WorkloadRaports";
            this.gBox_WorkloadRaports.TabStop = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // wl_PausedIssuesListView
            // 
            resources.ApplyResources(this.wl_PausedIssuesListView, "wl_PausedIssuesListView");
            this.wl_PausedIssuesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.wl_PausedIssuesListView.GridLines = true;
            this.wl_PausedIssuesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.wl_PausedIssuesListView.Name = "wl_PausedIssuesListView";
            this.wl_PausedIssuesListView.UseCompatibleStateImageBehavior = false;
            this.wl_PausedIssuesListView.View = System.Windows.Forms.View.Details;
            this.wl_PausedIssuesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // wl_OpenIssuesListView
            // 
            resources.ApplyResources(this.wl_OpenIssuesListView, "wl_OpenIssuesListView");
            this.wl_OpenIssuesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.wl_OpenIssuesListView.GridLines = true;
            this.wl_OpenIssuesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.wl_OpenIssuesListView.Name = "wl_OpenIssuesListView";
            this.wl_OpenIssuesListView.UseCompatibleStateImageBehavior = false;
            this.wl_OpenIssuesListView.View = System.Windows.Forms.View.Details;
            this.wl_OpenIssuesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // gBox_WorkloadCurrent
            // 
            resources.ApplyResources(this.gBox_WorkloadCurrent, "gBox_WorkloadCurrent");
            this.gBox_WorkloadCurrent.Controls.Add(this.lb_WorkloadTotalTime);
            this.gBox_WorkloadCurrent.Controls.Add(this.lb_WorkloadLoggedTime);
            this.gBox_WorkloadCurrent.Controls.Add(this.lb_workloadTitle);
            this.gBox_WorkloadCurrent.Name = "gBox_WorkloadCurrent";
            this.gBox_WorkloadCurrent.TabStop = false;
            // 
            // lb_WorkloadTotalTime
            // 
            resources.ApplyResources(this.lb_WorkloadTotalTime, "lb_WorkloadTotalTime");
            this.lb_WorkloadTotalTime.Name = "lb_WorkloadTotalTime";
            // 
            // lb_WorkloadLoggedTime
            // 
            resources.ApplyResources(this.lb_WorkloadLoggedTime, "lb_WorkloadLoggedTime");
            this.lb_WorkloadLoggedTime.Name = "lb_WorkloadLoggedTime";
            // 
            // lb_workloadTitle
            // 
            resources.ApplyResources(this.lb_workloadTitle, "lb_workloadTitle");
            this.lb_workloadTitle.Name = "lb_workloadTitle";
            // 
            // tp_awaria_nowa
            // 
            this.tp_awaria_nowa.Controls.Add(this.panel1);
            resources.ApplyResources(this.tp_awaria_nowa, "tp_awaria_nowa");
            this.tp_awaria_nowa.Name = "tp_awaria_nowa";
            this.tp_awaria_nowa.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_awaria_niezakonczona);
            this.panel1.Controls.Add(this.bt_awaria_zapisz);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.gb_awaria_bloker);
            this.panel1.Controls.Add(this.gb_awaria_onCall);
            this.panel1.Controls.Add(this.tb_awaria_nrJira);
            this.panel1.Controls.Add(this.tb_awaria_komentarz);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.tb_awaria_opis);
            this.panel1.Controls.Add(this.dtp_awaria_stop);
            this.panel1.Controls.Add(this.dtp_awaria_start);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // cb_awaria_niezakonczona
            // 
            resources.ApplyResources(this.cb_awaria_niezakonczona, "cb_awaria_niezakonczona");
            this.cb_awaria_niezakonczona.Name = "cb_awaria_niezakonczona";
            this.cb_awaria_niezakonczona.UseVisualStyleBackColor = true;
            // 
            // bt_awaria_zapisz
            // 
            resources.ApplyResources(this.bt_awaria_zapisz, "bt_awaria_zapisz");
            this.bt_awaria_zapisz.Name = "bt_awaria_zapisz";
            this.bt_awaria_zapisz.UseVisualStyleBackColor = true;
            this.bt_awaria_zapisz.Click += new System.EventHandler(this.bt_awaria_zapisz_Click);
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // gb_awaria_bloker
            // 
            this.gb_awaria_bloker.Controls.Add(this.radioButton3);
            this.gb_awaria_bloker.Controls.Add(this.radioButton4);
            resources.ApplyResources(this.gb_awaria_bloker, "gb_awaria_bloker");
            this.gb_awaria_bloker.Name = "gb_awaria_bloker";
            this.gb_awaria_bloker.TabStop = false;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Checked = true;
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // gb_awaria_onCall
            // 
            this.gb_awaria_onCall.Controls.Add(this.radioButton2);
            this.gb_awaria_onCall.Controls.Add(this.radioButton1);
            resources.ApplyResources(this.gb_awaria_onCall, "gb_awaria_onCall");
            this.gb_awaria_onCall.Name = "gb_awaria_onCall";
            this.gb_awaria_onCall.TabStop = false;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tb_awaria_nrJira
            // 
            resources.ApplyResources(this.tb_awaria_nrJira, "tb_awaria_nrJira");
            this.tb_awaria_nrJira.Name = "tb_awaria_nrJira";
            // 
            // tb_awaria_komentarz
            // 
            resources.ApplyResources(this.tb_awaria_komentarz, "tb_awaria_komentarz");
            this.tb_awaria_komentarz.Name = "tb_awaria_komentarz";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // tb_awaria_opis
            // 
            resources.ApplyResources(this.tb_awaria_opis, "tb_awaria_opis");
            this.tb_awaria_opis.Name = "tb_awaria_opis";
            // 
            // dtp_awaria_stop
            // 
            resources.ApplyResources(this.dtp_awaria_stop, "dtp_awaria_stop");
            this.dtp_awaria_stop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_awaria_stop.Name = "dtp_awaria_stop";
            // 
            // dtp_awaria_start
            // 
            resources.ApplyResources(this.dtp_awaria_start, "dtp_awaria_start");
            this.dtp_awaria_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_awaria_start.Name = "dtp_awaria_start";
            // 
            // tp_awarie
            // 
            this.tp_awarie.Controls.Add(this.tc_awarie);
            resources.ApplyResources(this.tp_awarie, "tp_awarie");
            this.tp_awarie.Name = "tp_awarie";
            this.tp_awarie.UseVisualStyleBackColor = true;
            // 
            // tc_awarie
            // 
            resources.ApplyResources(this.tc_awarie, "tc_awarie");
            this.tc_awarie.Name = "tc_awarie";
            this.tc_awarie.SelectedIndex = 0;
            // 
            // tp_awaria_lista
            // 
            this.tp_awaria_lista.Controls.Add(this.panel2);
            resources.ApplyResources(this.tp_awaria_lista, "tp_awaria_lista");
            this.tp_awaria_lista.Name = "tp_awaria_lista";
            this.tp_awaria_lista.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_Awarie);
            this.panel2.Controls.Add(this.bt_LA_szukaj);
            this.panel2.Controls.Add(this.tb_LA_nrJira);
            this.panel2.Controls.Add(this.dtp_LA_stopDate);
            this.panel2.Controls.Add(this.dtp_LA_startDate);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // dgv_Awarie
            // 
            this.dgv_Awarie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_Awarie.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Awarie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgv_Awarie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Awarie.DefaultCellStyle = dataGridViewCellStyle16;
            resources.ApplyResources(this.dgv_Awarie, "dgv_Awarie");
            this.dgv_Awarie.Name = "dgv_Awarie";
            this.dgv_Awarie.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Awarie_CellEndEdit);
            this.dgv_Awarie.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Awarie_CellEnter);
            // 
            // bt_LA_szukaj
            // 
            resources.ApplyResources(this.bt_LA_szukaj, "bt_LA_szukaj");
            this.bt_LA_szukaj.Name = "bt_LA_szukaj";
            this.bt_LA_szukaj.UseVisualStyleBackColor = true;
            this.bt_LA_szukaj.Click += new System.EventHandler(this.bt_LA_szukaj_Click);
            // 
            // tb_LA_nrJira
            // 
            resources.ApplyResources(this.tb_LA_nrJira, "tb_LA_nrJira");
            this.tb_LA_nrJira.Name = "tb_LA_nrJira";
            // 
            // dtp_LA_stopDate
            // 
            resources.ApplyResources(this.dtp_LA_stopDate, "dtp_LA_stopDate");
            this.dtp_LA_stopDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_LA_stopDate.Name = "dtp_LA_stopDate";
            // 
            // dtp_LA_startDate
            // 
            resources.ApplyResources(this.dtp_LA_startDate, "dtp_LA_startDate");
            this.dtp_LA_startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_LA_startDate.Name = "dtp_LA_startDate";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel3);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button7);
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.checkedListBox1);
            this.panel3.Controls.Add(this.button6);
            this.panel3.Controls.Add(this.textBox4);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.textBox3);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            resources.GetString("checkedListBox1.Items"),
            resources.GetString("checkedListBox1.Items1"),
            resources.GetString("checkedListBox1.Items2"),
            resources.GetString("checkedListBox1.Items3"),
            resources.GetString("checkedListBox1.Items4"),
            resources.GetString("checkedListBox1.Items5"),
            resources.GetString("checkedListBox1.Items6"),
            resources.GetString("checkedListBox1.Items7"),
            resources.GetString("checkedListBox1.Items8")});
            resources.ApplyResources(this.checkedListBox1, "checkedListBox1");
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox4
            // 
            resources.ApplyResources(this.textBox4, "textBox4");
            this.textBox4.Name = "textBox4";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.User,
            this.NrJira,
            this.TimeSpent,
            this.ZalogowanyCzas,
            this.ZalogowanyCzasMin,
            this.DataLogowania,
            this.Komentarz,
            this.Suma,
            this.DniowkiSerwisowe,
            this.DataUtworzenia,
            this.DataModyfikacji});
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            // 
            // User
            // 
            resources.ApplyResources(this.User, "User");
            this.User.Name = "User";
            // 
            // NrJira
            // 
            resources.ApplyResources(this.NrJira, "NrJira");
            this.NrJira.Name = "NrJira";
            this.NrJira.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TimeSpent
            // 
            resources.ApplyResources(this.TimeSpent, "TimeSpent");
            this.TimeSpent.Name = "TimeSpent";
            // 
            // ZalogowanyCzas
            // 
            resources.ApplyResources(this.ZalogowanyCzas, "ZalogowanyCzas");
            this.ZalogowanyCzas.Name = "ZalogowanyCzas";
            // 
            // ZalogowanyCzasMin
            // 
            resources.ApplyResources(this.ZalogowanyCzasMin, "ZalogowanyCzasMin");
            this.ZalogowanyCzasMin.Name = "ZalogowanyCzasMin";
            this.ZalogowanyCzasMin.ReadOnly = true;
            // 
            // DataLogowania
            // 
            resources.ApplyResources(this.DataLogowania, "DataLogowania");
            this.DataLogowania.Name = "DataLogowania";
            // 
            // Komentarz
            // 
            resources.ApplyResources(this.Komentarz, "Komentarz");
            this.Komentarz.Name = "Komentarz";
            // 
            // Suma
            // 
            resources.ApplyResources(this.Suma, "Suma");
            this.Suma.Name = "Suma";
            // 
            // DniowkiSerwisowe
            // 
            resources.ApplyResources(this.DniowkiSerwisowe, "DniowkiSerwisowe");
            this.DniowkiSerwisowe.Name = "DniowkiSerwisowe";
            // 
            // DataUtworzenia
            // 
            resources.ApplyResources(this.DataUtworzenia, "DataUtworzenia");
            this.DataUtworzenia.Name = "DataUtworzenia";
            // 
            // DataModyfikacji
            // 
            resources.ApplyResources(this.DataModyfikacji, "DataModyfikacji");
            this.DataModyfikacji.Name = "DataModyfikacji";
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            // 
            // tp_DayReport
            // 
            this.tp_DayReport.Controls.Add(this.button2);
            this.tp_DayReport.Controls.Add(this.bt_dayReportSend);
            this.tp_DayReport.Controls.Add(this.cb_dayReportAccident);
            this.tp_DayReport.Controls.Add(this.tb_dayReportTimeTo);
            this.tp_DayReport.Controls.Add(this.tb_dayReportTimeFrom);
            this.tp_DayReport.Controls.Add(this.label30);
            this.tp_DayReport.Controls.Add(this.label31);
            this.tp_DayReport.Controls.Add(this.dtp_dayReportDateTo);
            this.tp_DayReport.Controls.Add(this.dtp_dayReportDateFrom);
            this.tp_DayReport.Controls.Add(this.rtb_dayReportMessage);
            this.tp_DayReport.Controls.Add(this.bt_dayReportGenerate);
            resources.ApplyResources(this.tp_DayReport, "tp_DayReport");
            this.tp_DayReport.Name = "tp_DayReport";
            this.tp_DayReport.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // bt_dayReportSend
            // 
            resources.ApplyResources(this.bt_dayReportSend, "bt_dayReportSend");
            this.bt_dayReportSend.Name = "bt_dayReportSend";
            this.bt_dayReportSend.UseVisualStyleBackColor = true;
            this.bt_dayReportSend.Click += new System.EventHandler(this.bt_dayReportSend_Click);
            // 
            // cb_dayReportAccident
            // 
            resources.ApplyResources(this.cb_dayReportAccident, "cb_dayReportAccident");
            this.cb_dayReportAccident.Name = "cb_dayReportAccident";
            this.cb_dayReportAccident.UseVisualStyleBackColor = true;
            // 
            // tb_dayReportTimeTo
            // 
            resources.ApplyResources(this.tb_dayReportTimeTo, "tb_dayReportTimeTo");
            this.tb_dayReportTimeTo.Name = "tb_dayReportTimeTo";
            // 
            // tb_dayReportTimeFrom
            // 
            resources.ApplyResources(this.tb_dayReportTimeFrom, "tb_dayReportTimeFrom");
            this.tb_dayReportTimeFrom.Name = "tb_dayReportTimeFrom";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // dtp_dayReportDateTo
            // 
            resources.ApplyResources(this.dtp_dayReportDateTo, "dtp_dayReportDateTo");
            this.dtp_dayReportDateTo.Name = "dtp_dayReportDateTo";
            // 
            // dtp_dayReportDateFrom
            // 
            resources.ApplyResources(this.dtp_dayReportDateFrom, "dtp_dayReportDateFrom");
            this.dtp_dayReportDateFrom.Name = "dtp_dayReportDateFrom";
            // 
            // rtb_dayReportMessage
            // 
            resources.ApplyResources(this.rtb_dayReportMessage, "rtb_dayReportMessage");
            this.rtb_dayReportMessage.Name = "rtb_dayReportMessage";
            // 
            // bt_dayReportGenerate
            // 
            resources.ApplyResources(this.bt_dayReportGenerate, "bt_dayReportGenerate");
            this.bt_dayReportGenerate.Name = "bt_dayReportGenerate";
            this.bt_dayReportGenerate.UseVisualStyleBackColor = true;
            this.bt_dayReportGenerate.Click += new System.EventHandler(this.bt_dayReportGenerate_Click);
            // 
            // tab_Awarie
            // 
            resources.ApplyResources(this.tab_Awarie, "tab_Awarie");
            this.tab_Awarie.Name = "tab_Awarie";
            this.tab_Awarie.SelectedIndex = 0;
            // 
            // cms_IssuePopup
            // 
            this.cms_IssuePopup.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_IssuePopup.Name = "cms_IssuePopup";
            resources.ApplyResources(this.cms_IssuePopup, "cms_IssuePopup");
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel8,
            this.toolStripProgressBar3,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel6,
            this.toolStripProgressBar2,
            this.issuesCheckoutStatus,
            this.tslb_WorkloadStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zalogujSięToolStripMenuItem,
            this.przelaczStripMenuItem1});
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            // 
            // zalogujSięToolStripMenuItem
            // 
            this.zalogujSięToolStripMenuItem.Name = "zalogujSięToolStripMenuItem";
            resources.ApplyResources(this.zalogujSięToolStripMenuItem, "zalogujSięToolStripMenuItem");
            this.zalogujSięToolStripMenuItem.Click += new System.EventHandler(this.zalogujSięToolStripMenuItem_Click);
            // 
            // przelaczStripMenuItem1
            // 
            this.przelaczStripMenuItem1.Name = "przelaczStripMenuItem1";
            resources.ApplyResources(this.przelaczStripMenuItem1, "przelaczStripMenuItem1");
            this.przelaczStripMenuItem1.Click += new System.EventHandler(this.zalogujSięToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            // 
            // toolStripProgressBar3
            // 
            this.toolStripProgressBar3.Name = "toolStripProgressBar3";
            resources.ApplyResources(this.toolStripProgressBar3, "toolStripProgressBar3");
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            resources.ApplyResources(this.toolStripStatusLabel7, "toolStripStatusLabel7");
            this.toolStripStatusLabel7.Spring = true;
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.toolStripStatusLabel6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripStatusLabel6, "toolStripStatusLabel6");
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            // 
            // toolStripProgressBar2
            // 
            this.toolStripProgressBar2.Name = "toolStripProgressBar2";
            resources.ApplyResources(this.toolStripProgressBar2, "toolStripProgressBar2");
            // 
            // issuesCheckoutStatus
            // 
            this.issuesCheckoutStatus.Name = "issuesCheckoutStatus";
            resources.ApplyResources(this.issuesCheckoutStatus, "issuesCheckoutStatus");
            // 
            // tslb_WorkloadStatus
            // 
            resources.ApplyResources(this.tslb_WorkloadStatus, "tslb_WorkloadStatus");
            this.tslb_WorkloadStatus.Name = "tslb_WorkloadStatus";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // messageToWebService
            // 
            this.messageToWebService.Enabled = true;
            this.messageToWebService.Interval = 60000;
            this.messageToWebService.Tick += new System.EventHandler(this.messageToWebService_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uzupełnijDaneToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
            // 
            // uzupełnijDaneToolStripMenuItem
            // 
            this.uzupełnijDaneToolStripMenuItem.Name = "uzupełnijDaneToolStripMenuItem";
            resources.ApplyResources(this.uzupełnijDaneToolStripMenuItem, "uzupełnijDaneToolStripMenuItem");
            this.uzupełnijDaneToolStripMenuItem.Click += new System.EventHandler(this.uzupełnijDaneToolStripMenuItem_Click);
            // 
            // ni_NewIssueAlert
            // 
            this.ni_NewIssueAlert.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            resources.ApplyResources(this.ni_NewIssueAlert, "ni_NewIssueAlert");
            this.ni_NewIssueAlert.BalloonTipClicked += new System.EventHandler(this.ni_NewIssueAlert_BalloonTipClicked);
            this.ni_NewIssueAlert.BalloonTipClosed += new System.EventHandler(this.ni_NewIssueAlert_BalloonTipClosed);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.odświeżToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            resources.ApplyResources(this.plikToolStripMenuItem, "plikToolStripMenuItem");
            // 
            // odświeżToolStripMenuItem
            // 
            this.odświeżToolStripMenuItem.Name = "odświeżToolStripMenuItem";
            resources.ApplyResources(this.odświeżToolStripMenuItem, "odświeżToolStripMenuItem");
            this.odświeżToolStripMenuItem.Click += new System.EventHandler(this.odświeżToolStripMenuItem_Click);
            // 
            // tm_AutoAssigne
            // 
            this.tm_AutoAssigne.Interval = 1000;
            this.tm_AutoAssigne.Tick += new System.EventHandler(this.tm_AutoAssigne_Tick);
            // 
            // t_EmailNotification
            // 
            this.t_EmailNotification.Interval = 900000;
            this.t_EmailNotification.Tick += new System.EventHandler(this.t_EmailNotification_Tick);
            // 
            // tm_autoFrsh
            // 
            this.tm_autoFrsh.Interval = 10;
            this.tm_autoFrsh.Tick += new System.EventHandler(this.tm_autoFrsh_Tick);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.issueTab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tp_Billennium.ResumeLayout(false);
            this.tp_tmp.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tp_Raporty.ResumeLayout(false);
            this.tp_Settings.ResumeLayout(false);
            this.tp_Settings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tp_billing.ResumeLayout(false);
            this.tp_billing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zglDgv)).EndInit();
            this.tp_crm.ResumeLayout(false);
            this.tp_crm.PerformLayout();
            this.tp_zcc.ResumeLayout(false);
            this.tp_zcc.PerformLayout();
            this.tabSlowniki.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.compDgv)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Historia_dgv)).EndInit();
            this.tp_diagnoza.ResumeLayout(false);
            this.tp_diagnoza.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Diagnoza)).EndInit();
            this.tp_Sla.ResumeLayout(false);
            this.tp_Sla.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaRaport)).EndInit();
            this.tp_Multi.ResumeLayout(false);
            this.p_MultiEventMoveButtons.ResumeLayout(false);
            this.p_MultiEventMoveButtons.PerformLayout();
            this.tp_Workload.ResumeLayout(false);
            this.gBox_WorkloadRaports.ResumeLayout(false);
            this.gBox_WorkloadRaports.PerformLayout();
            this.gBox_WorkloadCurrent.ResumeLayout(false);
            this.gBox_WorkloadCurrent.PerformLayout();
            this.tp_awaria_nowa.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gb_awaria_bloker.ResumeLayout(false);
            this.gb_awaria_bloker.PerformLayout();
            this.gb_awaria_onCall.ResumeLayout(false);
            this.gb_awaria_onCall.PerformLayout();
            this.tp_awarie.ResumeLayout(false);
            this.tp_awaria_lista.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Awarie)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tp_DayReport.ResumeLayout(false);
            this.tp_DayReport.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripMenuItem zalogujSięToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem przelaczStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem developerToolStripMenuItem;
        private ToolStripProgressBar toolStripProgressBar2;
        private Label label1;
        private Button btn_WyszukajWJira;
        private TextBox numerZgl;
        private ContextMenuStrip cms_IssuePopup;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer messageToWebService;
        private TabControl tc;
        private TabPage tabPage1;
        private Button btn_DodajNotatke;
        private Button btn_Procesy;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private TabPage tp_billing;
        private System.Windows.Forms.RichTextBox richTextBoxStats;
        private Button btn_BillingZgloszeniaZDzisiaj;
        private TabControl issueTab;
        private TabPage tabPage3;
        private Button btn_NieprzydzieloneUzupelnijDane;
        private Button btn_NieprzydzieloneOdswiez;
        private TreeView treeView1;
        private TabPage tabPage11;
        private Button btn_MojeUzupelnijDane;
        private Button btn_MojeOdswiez;
        private TreeView treeView2;
        private Button btn_MojeZapiszDoWFS;
        private Button btn_NieprzydzieloneZapiszDoWFS;
        private Label label3;
        private Label label2;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker1;
        private TabControl tabStatystyki;
        private Button btn_BillingRaportStanuZgloszen;
        private Button btn_SlownikiOdswiez;
        private TabPage tabSlowniki;
        private DataGridView compDgv;
        private Button btn_SlownikiZapisz;
        private SaveFileDialog saveFileDialog1;
        private Button btn_BillingZgloszeniaKategoryzacja;
        private DataGridView zglDgv;
        private Button btn_BillingZapiszDoXls;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem uzupełnijDaneToolStripMenuItem;
        private TabPage tp_Settings;
        private Button btn_PowiadomienieWlacz;
        private TabPage tabPage6;
        private DataGridView Historia_dgv;
        private Button btn_HistoriaSzukaj;
        private TextBox textBox2;
        private TabPage tp_crm;
        private System.Windows.Forms.RichTextBox crm_rtb;
        private Button btn_CRMRaport18;
        private TextBox tb_CRMRapGodzinaDo;
        private TextBox tb_CRMRapGodzinaOd;
        private Label label8;
        private Label label7;
        private DateTimePicker dtp_CRMRapDataDo;
        private DateTimePicker dtp_CRMRapDataOd;
        private Button btn_CRMPodsumowanie;
        private TabPage tp_diagnoza;
        private DataGridView dgv_Diagnoza;
        private Button btn_DiagnozaWczytaj;
        private ComboBox cb_diagnozaUsers;
        private NotifyIcon ni_NewIssueAlert;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem plikToolStripMenuItem;
        private ToolStripMenuItem odświeżToolStripMenuItem;
        private Label label12;
        private TextBox notifyTimeoutTextBox;
        private TabPage tp_Multi;
        private Button btn_MultiDodaj;
        private ListBox lb_MultiList;
        private Button btn_MultiClear;
        private Panel p_MultiEventMoveButtons;
        private Button btn_MultiUsun;
        private Label label4;
        private ComboBox cb_DiagnozaFiltr;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private Button btn_WyszukajWWFS;
        private CheckBox cb_Rap18Awaria;
        private CheckBox ckb_isNotifyEnabled;
        private CheckBox ckb_pokazujDrzewko;
        private CheckBox cb_EmailNotification;
        private CheckBox cb_SoundNotification;
        private TabPage tp_zcc;
        private DateTimePicker dtp_ZCCDataDo;
        private DateTimePicker dtp_ZCCDataOd;
        private Button btn_ZccLoadNotes;
        private System.Windows.Forms.RichTextBox zcc_rtb;
        private Label label6;
        private Label label5;
        private Button btn_ZCCRaport;
        private Button btn_ZCCPodsumowanie;
        private Button btn_SoundNotificationOpenFileDialog;
        private TextBox tb_SoundNotificationPath;
        private Label label9;
        private ToolStripStatusLabel issuesCheckoutStatus;
        private GroupBox groupBox1;
        private TabPage tp_Workload;
        private GroupBox gBox_WorkloadRaports;
        private GroupBox gBox_WorkloadCurrent;
        private Label lb_WorkloadLoggedTime;
        private Label lb_workloadTitle;
        private ToolStripStatusLabel toolStripStatusLabel7;
        private ToolStripStatusLabel tslb_WorkloadStatus;
        private System.Windows.Forms.Timer tm_AutoAssigne;
        private StatusStripButton ssb_WorkloadButton;
        private StatusStripButton ssb_WorkloadStopButton;
        private Label lb_WorkloadTotalTime;
        private TabPage tp_Raporty;
        private TabPage tp_Awarie;
        private TabControl tab_Raporty;
        private ListView wl_OpenIssuesListView;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private Label label10;
        private Button btn_NewIssue;
        private TabPage tp_Billennium;
        private Button btn_BillenniumZapisz;
        private Button btn_BillenniumUzupelnijDane;
        private Button btn_BillenniumOdswiez;
        private TreeView treeView3;
        private Label label11;
        private ListView wl_PausedIssuesListView;
        private ColumnHeader columnHeader7;
        private CheckBox cbox_RefreshBoth;
        private GroupBox groupBox2;
        private Label label13;
        private TextBox tb_UnassignedFilterName;
        private TextBox tb_AssignedFilterName;
        private Button btn_SaveFilters;
        private Label label14;
        private TabPage tp_Sla;
        private Button btn_slaRaport_Load;
        private DataGridView dgv_SlaRaport;
        private System.Windows.Forms.Timer t_EmailNotification;
        private Button tbn_CRMpodsumowanieWyslij;
        private CheckBox cb_SLApauza;
        private Button button1;
        private CheckBox cb_SLA_JiraSynchro;
        private TabPage tp_tmp;
        private TreeView treeView4;
        private GroupBox groupBox3;
        private MaskedTextBox mtb_BillenniumPass;
        private Label label21;
        private CheckBox cb_KontoBillennium;
        private Label label22;
        private Button bt_KontoBillenniumSave;
        private Button bt_SLA_synchronizacja;
        private Button bt_AutoAssigne;
        private Button button3;
        private WebBrowser webBrowser1;
        private TabPage tp_awaria_nowa;
        private TextBox tb_awaria_nrJira;
        private TextBox tb_awaria_komentarz;
        private TextBox tb_awaria_opis;
        private DateTimePicker dtp_awaria_start;
        private DateTimePicker dtp_awaria_stop;
        private Label label23;
        private Panel panel1;
        private GroupBox gb_awaria_onCall;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private GroupBox gb_awaria_bloker;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private Button bt_awaria_zapisz;
        private Label label27;
        private Label label26;
        private Label label25;
        private Label label24;
        private CheckBox cb_awaria_niezakonczona;
        private TabPage tp_awarie;
        private TabPage tp_awaria_lista;
        private TabControl tab_Awarie;
        private TabControl tc_awarie;
        private Panel panel2;
        private TextBox tb_LA_nrJira;
        private DateTimePicker dtp_LA_stopDate;
        private DateTimePicker dtp_LA_startDate;
        private Button bt_LA_szukaj;
        private DataGridView dgv_Awarie;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer tm_autoFrsh;
        private TabPage tabPage2;
        private Panel panel3;
        private Button button5;
        private TextBox textBox3;
        private DataGridView dataGridView1;
        private TextBox textBox4;
        private Button button6;
        private CheckedListBox checkedListBox1;
        private CheckBox checkBox1;
        private DataGridViewTextBoxColumn User;
        private DataGridViewTextBoxColumn NrJira;
        private DataGridViewTextBoxColumn TimeSpent;
        private DataGridViewTextBoxColumn ZalogowanyCzas;
        private DataGridViewTextBoxColumn ZalogowanyCzasMin;
        private DataGridViewTextBoxColumn DataLogowania;
        private DataGridViewTextBoxColumn Komentarz;
        private DataGridViewTextBoxColumn Suma;
        private DataGridViewCheckBoxColumn DniowkiSerwisowe;
        private DataGridViewTextBoxColumn DataUtworzenia;
        private DataGridViewTextBoxColumn DataModyfikacji;
        private Button button7;
        private CheckBox cb_SLAWstrzymane;
        private Panel panel4;
        private TextBox tb_sla_ileWstrzymane;
        private Label label29;
        private TextBox tb_sla_ileRealizacja;
        private Label label28;
        private ToolStripStatusLabel toolStripStatusLabel8;
        private ToolStripProgressBar toolStripProgressBar3;
        private TabPage tp_DayReport;
        private Button button2;
        private Button bt_dayReportSend;
        private CheckBox cb_dayReportAccident;
        private TextBox tb_dayReportTimeTo;
        private TextBox tb_dayReportTimeFrom;
        private Label label30;
        private Label label31;
        private DateTimePicker dtp_dayReportDateTo;
        private DateTimePicker dtp_dayReportDateFrom;
        private System.Windows.Forms.RichTextBox rtb_dayReportMessage;
        private Button bt_dayReportGenerate;
        private GroupBox groupBox4;
        private Button bt_LogSearchRun;
        private Button bt_LogSearchPatch;
        private TextBox tb_LogSearchPath;
        private FolderBrowserDialog pn_FolderSearch;
        private FolderBrowserDialog fb_logSearch;
    }
}

