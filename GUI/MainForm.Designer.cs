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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bt_AutoAssigne = new System.Windows.Forms.Button();
            this.btn_NewIssue = new System.Windows.Forms.Button();
            this.btn_WyszukajWWFS = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.issueTab = new System.Windows.Forms.TabControl();
            this.tp_filter1name = new System.Windows.Forms.TabPage();
            this.btn_NieprzydzieloneZapiszDoWFS = new System.Windows.Forms.Button();
            this.btn_NieprzydzieloneUzupelnijDane = new System.Windows.Forms.Button();
            this.btn_NieprzydzieloneOdswiez = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tp_filter2name = new System.Windows.Forms.TabPage();
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
            this.button1 = new System.Windows.Forms.Button();
            this.btn_DodajNotatke = new System.Windows.Forms.Button();
            this.btn_Procesy = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tp_Raporty = new System.Windows.Forms.TabPage();
            this.tab_Raporty = new System.Windows.Forms.TabControl();
            this.tp_Settings = new System.Windows.Forms.TabPage();
            this.gb_Deweloper = new System.Windows.Forms.GroupBox();
            this.gb_DevCp_option = new System.Windows.Forms.GroupBox();
            this.rb_devCp_del = new System.Windows.Forms.RadioButton();
            this.rb_devCp_add = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.bt_devCP = new System.Windows.Forms.Button();
            this.tb_devCp_surname = new System.Windows.Forms.TextBox();
            this.tb_devCp_name = new System.Windows.Forms.TextBox();
            this.tb_InterwalAutomatu = new System.Windows.Forms.TextBox();
            this.bt_OtworzLog = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
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
            this.cbFilter2name = new System.Windows.Forms.ComboBox();
            this.tb_filter2name = new System.Windows.Forms.TextBox();
            this.cbFilter1name = new System.Windows.Forms.ComboBox();
            this.tb_filter1name = new System.Windows.Forms.TextBox();
            this.btn_SaveFilters = new System.Windows.Forms.Button();
            this.lb_filter2name = new System.Windows.Forms.Label();
            this.lb_filter1name = new System.Windows.Forms.Label();
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
            this.tp_crm = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tb_CRMRapGodzinaDo = new System.Windows.Forms.TextBox();
            this.tb_CRMRapGodzinaOd = new System.Windows.Forms.TextBox();
            this.crm_rtb = new System.Windows.Forms.RichTextBox();
            this.tbn_CRMpodsumowanieWyslij = new System.Windows.Forms.Button();
            this.cb_Rap18Awaria = new System.Windows.Forms.CheckBox();
            this.btn_CRMPodsumowanie = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtp_CRMRapDataDo = new System.Windows.Forms.DateTimePicker();
            this.dtp_CRMRapDataOd = new System.Windows.Forms.DateTimePicker();
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.cb_dgv_Wypalony = new System.Windows.Forms.CheckBox();
            this.cb_dgv_Pauza = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_dgv_IssueId = new System.Windows.Forms.CheckBox();
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
            this.rtb_dayReportMessage = new System.Windows.Forms.RichTextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.dtp_dayReportDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtp_dayReportDateFrom = new System.Windows.Forms.DateTimePicker();
            this.bt_dayReportGenerate = new System.Windows.Forms.Button();
            this.tp_PI = new System.Windows.Forms.TabPage();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.button9 = new System.Windows.Forms.Button();
            this.dgv_PI = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nrPi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PREPROD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iloscInstalacji = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.system = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sumasys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sredniomiesiecznie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sumaproblem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sredniaproblem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.tab_Awarie = new System.Windows.Forms.TabControl();
            this.cms_IssuePopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusStripButton1 = new GUI.StatusStripButton();
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
            this.tslb_WorkloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            this.issuesCheckoutStatus = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.spelling1 = new NetSpell.SpellChecker.Spelling(this.components);
            this.tc_awarie = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.issueTab.SuspendLayout();
            this.tp_filter1name.SuspendLayout();
            this.tp_filter2name.SuspendLayout();
            this.tp_Billennium.SuspendLayout();
            this.tp_tmp.SuspendLayout();
            this.tc.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tp_Raporty.SuspendLayout();
            this.tp_Settings.SuspendLayout();
            this.gb_Deweloper.SuspendLayout();
            this.gb_DevCp_option.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tp_crm.SuspendLayout();
            this.tp_zcc.SuspendLayout();
            this.tabSlowniki.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compDgv)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Historia_dgv)).BeginInit();
            this.tp_diagnoza.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Diagnoza)).BeginInit();
            this.tp_Sla.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaRaport)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tp_DayReport.SuspendLayout();
            this.tp_PI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.bt_AutoAssigne);
            this.splitContainer1.Panel1.Controls.Add(this.btn_NewIssue);
            this.splitContainer1.Panel1.Controls.Add(this.btn_WyszukajWWFS);
            this.splitContainer1.Panel1.Controls.Add(this.button10);
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
            // bt_AutoAssigne
            // 
            resources.ApplyResources(this.bt_AutoAssigne, "bt_AutoAssigne");
            this.bt_AutoAssigne.BackColor = System.Drawing.Color.Orange;
            this.bt_AutoAssigne.Name = "bt_AutoAssigne";
            this.bt_AutoAssigne.UseVisualStyleBackColor = false;
            this.bt_AutoAssigne.Click += new System.EventHandler(this.bt_autoAssigne_Click);
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
            // button10
            // 
            resources.ApplyResources(this.button10, "button10");
            this.button10.Name = "button10";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // issueTab
            // 
            resources.ApplyResources(this.issueTab, "issueTab");
            this.issueTab.Controls.Add(this.tp_filter1name);
            this.issueTab.Controls.Add(this.tp_filter2name);
            this.issueTab.Controls.Add(this.tp_Billennium);
            this.issueTab.Controls.Add(this.tp_tmp);
            this.issueTab.Name = "issueTab";
            this.issueTab.SelectedIndex = 0;
            this.issueTab.SelectedIndexChanged += new System.EventHandler(this.issueTab_SelectedIndexChanged);
            // 
            // tp_filter1name
            // 
            this.tp_filter1name.Controls.Add(this.btn_NieprzydzieloneZapiszDoWFS);
            this.tp_filter1name.Controls.Add(this.btn_NieprzydzieloneUzupelnijDane);
            this.tp_filter1name.Controls.Add(this.btn_NieprzydzieloneOdswiez);
            this.tp_filter1name.Controls.Add(this.treeView1);
            resources.ApplyResources(this.tp_filter1name, "tp_filter1name");
            this.tp_filter1name.Name = "tp_filter1name";
            this.tp_filter1name.UseVisualStyleBackColor = true;
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
            // tp_filter2name
            // 
            this.tp_filter2name.Controls.Add(this.btn_MojeZapiszDoWFS);
            this.tp_filter2name.Controls.Add(this.btn_MojeUzupelnijDane);
            this.tp_filter2name.Controls.Add(this.btn_MojeOdswiez);
            this.tp_filter2name.Controls.Add(this.treeView2);
            resources.ApplyResources(this.tp_filter2name, "tp_filter2name");
            this.tp_filter2name.Name = "tp_filter2name";
            this.tp_filter2name.UseVisualStyleBackColor = true;
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
            this.numerZgl.Validated += new System.EventHandler(this.numerZgl_Validated);
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
            this.tc.Controls.Add(this.tp_crm);
            this.tc.Controls.Add(this.tp_zcc);
            this.tc.Controls.Add(this.tabSlowniki);
            this.tc.Controls.Add(this.tabPage6);
            this.tc.Controls.Add(this.tp_diagnoza);
            this.tc.Controls.Add(this.tp_Sla);
            this.tc.Controls.Add(this.tabPage2);
            this.tc.Controls.Add(this.tp_DayReport);
            this.tc.Controls.Add(this.tp_PI);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btn_DodajNotatke);
            this.tabPage1.Controls.Add(this.btn_Procesy);
            this.tabPage1.Controls.Add(this.richTextBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
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
            this.tp_Settings.Controls.Add(this.gb_Deweloper);
            this.tp_Settings.Controls.Add(this.tb_InterwalAutomatu);
            this.tp_Settings.Controls.Add(this.bt_OtworzLog);
            this.tp_Settings.Controls.Add(this.label13);
            this.tp_Settings.Controls.Add(this.groupBox4);
            this.tp_Settings.Controls.Add(this.groupBox3);
            this.tp_Settings.Controls.Add(this.groupBox2);
            this.tp_Settings.Controls.Add(this.cbox_RefreshBoth);
            this.tp_Settings.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tp_Settings, "tp_Settings");
            this.tp_Settings.Name = "tp_Settings";
            this.tp_Settings.UseVisualStyleBackColor = true;
            // 
            // gb_Deweloper
            // 
            this.gb_Deweloper.Controls.Add(this.gb_DevCp_option);
            this.gb_Deweloper.Controls.Add(this.label16);
            this.gb_Deweloper.Controls.Add(this.label15);
            this.gb_Deweloper.Controls.Add(this.bt_devCP);
            this.gb_Deweloper.Controls.Add(this.tb_devCp_surname);
            this.gb_Deweloper.Controls.Add(this.tb_devCp_name);
            resources.ApplyResources(this.gb_Deweloper, "gb_Deweloper");
            this.gb_Deweloper.Name = "gb_Deweloper";
            this.gb_Deweloper.TabStop = false;
            // 
            // gb_DevCp_option
            // 
            this.gb_DevCp_option.Controls.Add(this.rb_devCp_del);
            this.gb_DevCp_option.Controls.Add(this.rb_devCp_add);
            resources.ApplyResources(this.gb_DevCp_option, "gb_DevCp_option");
            this.gb_DevCp_option.Name = "gb_DevCp_option";
            this.gb_DevCp_option.TabStop = false;
            // 
            // rb_devCp_del
            // 
            resources.ApplyResources(this.rb_devCp_del, "rb_devCp_del");
            this.rb_devCp_del.Name = "rb_devCp_del";
            this.rb_devCp_del.TabStop = true;
            this.rb_devCp_del.UseVisualStyleBackColor = true;
            this.rb_devCp_del.CheckedChanged += new System.EventHandler(this.rb_devCp_CheckedChanged);
            // 
            // rb_devCp_add
            // 
            resources.ApplyResources(this.rb_devCp_add, "rb_devCp_add");
            this.rb_devCp_add.Name = "rb_devCp_add";
            this.rb_devCp_add.TabStop = true;
            this.rb_devCp_add.UseVisualStyleBackColor = true;
            this.rb_devCp_add.CheckedChanged += new System.EventHandler(this.rb_devCp_CheckedChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // bt_devCP
            // 
            resources.ApplyResources(this.bt_devCP, "bt_devCP");
            this.bt_devCP.Name = "bt_devCP";
            this.bt_devCP.UseVisualStyleBackColor = true;
            this.bt_devCP.Click += new System.EventHandler(this.bt_devCP_Click);
            // 
            // tb_devCp_surname
            // 
            resources.ApplyResources(this.tb_devCp_surname, "tb_devCp_surname");
            this.tb_devCp_surname.Name = "tb_devCp_surname";
            // 
            // tb_devCp_name
            // 
            resources.ApplyResources(this.tb_devCp_name, "tb_devCp_name");
            this.tb_devCp_name.Name = "tb_devCp_name";
            // 
            // tb_InterwalAutomatu
            // 
            resources.ApplyResources(this.tb_InterwalAutomatu, "tb_InterwalAutomatu");
            this.tb_InterwalAutomatu.Name = "tb_InterwalAutomatu";
            // 
            // bt_OtworzLog
            // 
            resources.ApplyResources(this.bt_OtworzLog, "bt_OtworzLog");
            this.bt_OtworzLog.Name = "bt_OtworzLog";
            this.bt_OtworzLog.UseVisualStyleBackColor = true;
            this.bt_OtworzLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bt_OtworzLog_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
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
            this.tb_LogSearchPath.ReadOnly = true;
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
            this.groupBox2.Controls.Add(this.cbFilter2name);
            this.groupBox2.Controls.Add(this.tb_filter2name);
            this.groupBox2.Controls.Add(this.cbFilter1name);
            this.groupBox2.Controls.Add(this.tb_filter1name);
            this.groupBox2.Controls.Add(this.btn_SaveFilters);
            this.groupBox2.Controls.Add(this.lb_filter2name);
            this.groupBox2.Controls.Add(this.lb_filter1name);
            this.groupBox2.Controls.Add(this.tb_UnassignedFilterName);
            this.groupBox2.Controls.Add(this.tb_AssignedFilterName);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbFilter2name
            // 
            this.cbFilter2name.FormattingEnabled = true;
            resources.ApplyResources(this.cbFilter2name, "cbFilter2name");
            this.cbFilter2name.Name = "cbFilter2name";
            // 
            // tb_filter2name
            // 
            resources.ApplyResources(this.tb_filter2name, "tb_filter2name");
            this.tb_filter2name.Name = "tb_filter2name";
            this.tb_filter2name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_filter1name_KeyDown);
            // 
            // cbFilter1name
            // 
            this.cbFilter1name.FormattingEnabled = true;
            resources.ApplyResources(this.cbFilter1name, "cbFilter1name");
            this.cbFilter1name.Name = "cbFilter1name";
            // 
            // tb_filter1name
            // 
            resources.ApplyResources(this.tb_filter1name, "tb_filter1name");
            this.tb_filter1name.Name = "tb_filter1name";
            this.tb_filter1name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_filter1name_KeyDown);
            // 
            // btn_SaveFilters
            // 
            resources.ApplyResources(this.btn_SaveFilters, "btn_SaveFilters");
            this.btn_SaveFilters.Name = "btn_SaveFilters";
            this.btn_SaveFilters.UseVisualStyleBackColor = true;
            this.btn_SaveFilters.Click += new System.EventHandler(this.btn_SaveFilters_ClickCB);
            // 
            // lb_filter2name
            // 
            resources.ApplyResources(this.lb_filter2name, "lb_filter2name");
            this.lb_filter2name.Name = "lb_filter2name";
            this.lb_filter2name.DoubleClick += new System.EventHandler(this.lb_filter1name_Click);
            // 
            // lb_filter1name
            // 
            resources.ApplyResources(this.lb_filter1name, "lb_filter1name");
            this.lb_filter1name.Name = "lb_filter1name";
            this.lb_filter1name.DoubleClick += new System.EventHandler(this.lb_filter1name_Click);
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
            // tp_crm
            // 
            this.tp_crm.Controls.Add(this.button3);
            this.tp_crm.Controls.Add(this.webBrowser1);
            this.tp_crm.Controls.Add(this.tb_CRMRapGodzinaDo);
            this.tp_crm.Controls.Add(this.tb_CRMRapGodzinaOd);
            this.tp_crm.Controls.Add(this.crm_rtb);
            this.tp_crm.Controls.Add(this.tbn_CRMpodsumowanieWyslij);
            this.tp_crm.Controls.Add(this.cb_Rap18Awaria);
            this.tp_crm.Controls.Add(this.btn_CRMPodsumowanie);
            this.tp_crm.Controls.Add(this.label8);
            this.tp_crm.Controls.Add(this.label7);
            this.tp_crm.Controls.Add(this.dtp_CRMRapDataDo);
            this.tp_crm.Controls.Add(this.dtp_CRMRapDataOd);
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
            // crm_rtb
            // 
            resources.ApplyResources(this.crm_rtb, "crm_rtb");
            this.crm_rtb.Name = "crm_rtb";
            this.crm_rtb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.compDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.compDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.compDgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.compDgv.Name = "compDgv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.compDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Historia_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.Historia_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Historia_dgv.DefaultCellStyle = dataGridViewCellStyle5;
            this.Historia_dgv.Name = "Historia_dgv";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Historia_dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
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
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Diagnoza.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_Diagnoza.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Diagnoza.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_Diagnoza.Name = "dgv_Diagnoza";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Diagnoza.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
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
            this.tp_Sla.Controls.Add(this.panel5);
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
            // panel5
            // 
            this.panel5.Controls.Add(this.cb_dgv_Wypalony);
            this.panel5.Controls.Add(this.cb_dgv_Pauza);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.cb_dgv_IssueId);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // cb_dgv_Wypalony
            // 
            resources.ApplyResources(this.cb_dgv_Wypalony, "cb_dgv_Wypalony");
            this.cb_dgv_Wypalony.Name = "cb_dgv_Wypalony";
            this.cb_dgv_Wypalony.UseVisualStyleBackColor = true;
            this.cb_dgv_Wypalony.CheckedChanged += new System.EventHandler(this.cb_dgv_optionChange);
            // 
            // cb_dgv_Pauza
            // 
            resources.ApplyResources(this.cb_dgv_Pauza, "cb_dgv_Pauza");
            this.cb_dgv_Pauza.Name = "cb_dgv_Pauza";
            this.cb_dgv_Pauza.UseVisualStyleBackColor = true;
            this.cb_dgv_Pauza.CheckedChanged += new System.EventHandler(this.cb_dgv_optionChange);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // cb_dgv_IssueId
            // 
            resources.ApplyResources(this.cb_dgv_IssueId, "cb_dgv_IssueId");
            this.cb_dgv_IssueId.Checked = true;
            this.cb_dgv_IssueId.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_dgv_IssueId.Name = "cb_dgv_IssueId";
            this.cb_dgv_IssueId.UseVisualStyleBackColor = true;
            this.cb_dgv_IssueId.CheckedChanged += new System.EventHandler(this.cb_dgv_optionChange);
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
            this.cb_SLApauza.Checked = true;
            this.cb_SLApauza.CheckState = System.Windows.Forms.CheckState.Checked;
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SlaRaport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_SlaRaport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_SlaRaport.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgv_SlaRaport.Name = "dgv_SlaRaport";
            this.dgv_SlaRaport.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaportCellContentClick);
            this.dgv_SlaRaport.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaport_CellEnter);
            this.dgv_SlaRaport.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_SlaRaport_CellFormatting);
            this.dgv_SlaRaport.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaRaport_CellLeave);
            this.dgv_SlaRaport.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_SlaRaport_CellMouseClick);
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
            this.tp_DayReport.Controls.Add(this.rtb_dayReportMessage);
            this.tp_DayReport.Controls.Add(this.label30);
            this.tp_DayReport.Controls.Add(this.label31);
            this.tp_DayReport.Controls.Add(this.dtp_dayReportDateTo);
            this.tp_DayReport.Controls.Add(this.dtp_dayReportDateFrom);
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
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
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
            // rtb_dayReportMessage
            // 
            resources.ApplyResources(this.rtb_dayReportMessage, "rtb_dayReportMessage");
            this.rtb_dayReportMessage.Name = "rtb_dayReportMessage";
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
            // bt_dayReportGenerate
            // 
            resources.ApplyResources(this.bt_dayReportGenerate, "bt_dayReportGenerate");
            this.bt_dayReportGenerate.Name = "bt_dayReportGenerate";
            this.bt_dayReportGenerate.UseVisualStyleBackColor = true;
            this.bt_dayReportGenerate.Click += new System.EventHandler(this.bt_dayReportGenerate_Click);
            // 
            // tp_PI
            // 
            this.tp_PI.Controls.Add(this.dateTimePicker3);
            this.tp_PI.Controls.Add(this.button9);
            this.tp_PI.Controls.Add(this.dgv_PI);
            this.tp_PI.Controls.Add(this.dataGridView2);
            this.tp_PI.Controls.Add(this.button8);
            this.tp_PI.Controls.Add(this.textBox5);
            this.tp_PI.Controls.Add(this.button4);
            resources.ApplyResources(this.tp_PI, "tp_PI");
            this.tp_PI.Name = "tp_PI";
            this.tp_PI.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker3
            // 
            resources.ApplyResources(this.dateTimePicker3, "dateTimePicker3");
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.ValueChanged += new System.EventHandler(this.dateTimePicker3_ValueChanged);
            // 
            // button9
            // 
            resources.ApplyResources(this.button9, "button9");
            this.button9.Name = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // dgv_PI
            // 
            this.dgv_PI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_PI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.nrPi,
            this.INS,
            this.UAT,
            this.PREPROD,
            this.PROD,
            this.iloscInstalacji});
            resources.ApplyResources(this.dgv_PI, "dgv_PI");
            this.dgv_PI.Name = "dgv_PI";
            this.dgv_PI.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // nrPi
            // 
            resources.ApplyResources(this.nrPi, "nrPi");
            this.nrPi.Name = "nrPi";
            // 
            // INS
            // 
            resources.ApplyResources(this.INS, "INS");
            this.INS.Name = "INS";
            // 
            // UAT
            // 
            resources.ApplyResources(this.UAT, "UAT");
            this.UAT.Name = "UAT";
            // 
            // PREPROD
            // 
            resources.ApplyResources(this.PREPROD, "PREPROD");
            this.PREPROD.Name = "PREPROD";
            // 
            // PROD
            // 
            resources.ApplyResources(this.PROD, "PROD");
            this.PROD.Name = "PROD";
            // 
            // iloscInstalacji
            // 
            resources.ApplyResources(this.iloscInstalacji, "iloscInstalacji");
            this.iloscInstalacji.Name = "iloscInstalacji";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.system,
            this.sumasys,
            this.sredniomiesiecznie,
            this.sumaproblem,
            this.sredniaproblem});
            resources.ApplyResources(this.dataGridView2, "dataGridView2");
            this.dataGridView2.Name = "dataGridView2";
            // 
            // system
            // 
            resources.ApplyResources(this.system, "system");
            this.system.Name = "system";
            // 
            // sumasys
            // 
            resources.ApplyResources(this.sumasys, "sumasys");
            this.sumasys.Name = "sumasys";
            // 
            // sredniomiesiecznie
            // 
            resources.ApplyResources(this.sredniomiesiecznie, "sredniomiesiecznie");
            this.sredniomiesiecznie.Name = "sredniomiesiecznie";
            // 
            // sumaproblem
            // 
            resources.ApplyResources(this.sumaproblem, "sumaproblem");
            this.sumaproblem.Name = "sumaproblem";
            // 
            // sredniaproblem
            // 
            resources.ApplyResources(this.sredniaproblem, "sredniaproblem");
            this.sredniaproblem.Name = "sredniaproblem";
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // textBox5
            // 
            resources.ApplyResources(this.textBox5, "textBox5");
            this.textBox5.Name = "textBox5";
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
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
            this.statusStripButton1,
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel8,
            this.toolStripProgressBar3,
            this.tslb_WorkloadStatus,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel6,
            this.toolStripProgressBar2,
            this.issuesCheckoutStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // statusStripButton1
            // 
            this.statusStripButton1.Image = global::GUI.Properties.Resources.logSearch;
            this.statusStripButton1.Name = "statusStripButton1";
            resources.ApplyResources(this.statusStripButton1, "statusStripButton1");
            this.statusStripButton1.Click += new System.EventHandler(this.statusStripButton1_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
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
            this.toolStripStatusLabel8.IsLink = true;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            this.toolStripStatusLabel8.Click += new System.EventHandler(this.toolStripStatusLabel8_Click);
            // 
            // toolStripProgressBar3
            // 
            this.toolStripProgressBar3.Name = "toolStripProgressBar3";
            resources.ApplyResources(this.toolStripProgressBar3, "toolStripProgressBar3");
            // 
            // tslb_WorkloadStatus
            // 
            resources.ApplyResources(this.tslb_WorkloadStatus, "tslb_WorkloadStatus");
            this.tslb_WorkloadStatus.Name = "tslb_WorkloadStatus";
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
            // spelling1
            // 
            this.spelling1.Dictionary = null;
            // 
            // tc_awarie
            // 
            resources.ApplyResources(this.tc_awarie, "tc_awarie");
            this.tc_awarie.Name = "tc_awarie";
            this.tc_awarie.SelectedIndex = 0;
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
            this.tp_filter1name.ResumeLayout(false);
            this.tp_filter2name.ResumeLayout(false);
            this.tp_Billennium.ResumeLayout(false);
            this.tp_tmp.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tp_Raporty.ResumeLayout(false);
            this.tp_Settings.ResumeLayout(false);
            this.tp_Settings.PerformLayout();
            this.gb_Deweloper.ResumeLayout(false);
            this.gb_Deweloper.PerformLayout();
            this.gb_DevCp_option.ResumeLayout(false);
            this.gb_DevCp_option.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaRaport)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tp_DayReport.ResumeLayout(false);
            this.tp_DayReport.PerformLayout();
            this.tp_PI.ResumeLayout(false);
            this.tp_PI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
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
        private TabControl issueTab;
        private TabPage tp_filter1name;
        private Button btn_NieprzydzieloneUzupelnijDane;
        private Button btn_NieprzydzieloneOdswiez;
        private TreeView treeView1;
        private TabPage tp_filter2name;
        private Button btn_MojeUzupelnijDane;
        private Button btn_MojeOdswiez;
        private TreeView treeView2;
        private Button btn_MojeZapiszDoWFS;
        private Button btn_NieprzydzieloneZapiszDoWFS;
        private SaveFileDialog saveFileDialog1;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem uzupełnijDaneToolStripMenuItem;
        private NotifyIcon ni_NewIssueAlert;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem plikToolStripMenuItem;
        private ToolStripMenuItem odświeżToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private Button btn_WyszukajWWFS;
        private ToolStripStatusLabel issuesCheckoutStatus;
        private ToolStripStatusLabel tslb_WorkloadStatus;
        private System.Windows.Forms.Timer tm_AutoAssigne;
        private StatusStripButton ssb_WorkloadButton;
        private StatusStripButton ssb_WorkloadStopButton;
        private TabPage tp_Awarie;
        private Button btn_NewIssue;
        private TabPage tp_Billennium;
        private Button btn_BillenniumZapisz;
        private Button btn_BillenniumUzupelnijDane;
        private Button btn_BillenniumOdswiez;
        private TreeView treeView3;
        private System.Windows.Forms.Timer t_EmailNotification;
        private TabPage tp_tmp;
        private TreeView treeView4;
        private Button bt_AutoAssigne;
        private TabControl tab_Awarie;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer tm_autoFrsh;
        private ToolStripStatusLabel toolStripStatusLabel8;
        private ToolStripProgressBar toolStripProgressBar3;
        private FolderBrowserDialog pn_FolderSearch;
        private FolderBrowserDialog fb_logSearch;
        private ToolStripStatusLabel toolStripStatusLabel7;
        private NetSpell.SpellChecker.Spelling spelling1;
        private Button button10;
        private StatusStripButton statusStripButton1;
        private TabControl tc;
        private TabPage tabPage1;
        private Button button1;
        private Button btn_DodajNotatke;
        private Button btn_Procesy;
        private RichTextBox richTextBox1;
        private TabPage tp_Raporty;
        private TabControl tab_Raporty;
        private TabPage tp_Settings;
        private GroupBox gb_Deweloper;
        private GroupBox gb_DevCp_option;
        private RadioButton rb_devCp_del;
        private RadioButton rb_devCp_add;
        private Label label16;
        private Label label15;
        private Button bt_devCP;
        private TextBox tb_devCp_surname;
        private TextBox tb_devCp_name;
        private TextBox tb_InterwalAutomatu;
        private Button bt_OtworzLog;
        private Label label13;
        private GroupBox groupBox4;
        private Button bt_LogSearchRun;
        private Button bt_LogSearchPatch;
        private TextBox tb_LogSearchPath;
        private GroupBox groupBox3;
        private CheckBox cb_KontoBillennium;
        private Label label22;
        private Button bt_KontoBillenniumSave;
        private MaskedTextBox mtb_BillenniumPass;
        private Label label21;
        private GroupBox groupBox2;
        private ComboBox cbFilter2name;
        private TextBox tb_filter2name;
        private ComboBox cbFilter1name;
        private TextBox tb_filter1name;
        private Button btn_SaveFilters;
        private Label lb_filter2name;
        private Label lb_filter1name;
        private TextBox tb_UnassignedFilterName;
        private TextBox tb_AssignedFilterName;
        private CheckBox cbox_RefreshBoth;
        private GroupBox groupBox1;
        private Label label12;
        private Button btn_SoundNotificationOpenFileDialog;
        private Button btn_PowiadomienieWlacz;
        private TextBox tb_SoundNotificationPath;
        private TextBox notifyTimeoutTextBox;
        private CheckBox cb_SoundNotification;
        private CheckBox ckb_isNotifyEnabled;
        private CheckBox cb_EmailNotification;
        private TabPage tp_crm;
        private Button button3;
        private WebBrowser webBrowser1;
        private TextBox tb_CRMRapGodzinaDo;
        private TextBox tb_CRMRapGodzinaOd;
        private RichTextBox crm_rtb;
        private Button tbn_CRMpodsumowanieWyslij;
        private CheckBox cb_Rap18Awaria;
        private Button btn_CRMPodsumowanie;
        private Label label8;
        private Label label7;
        private DateTimePicker dtp_CRMRapDataDo;
        private DateTimePicker dtp_CRMRapDataOd;
        private Button btn_CRMRaport18;
        private TabPage tp_zcc;
        private Button btn_ZCCPodsumowanie;
        private Button btn_ZCCRaport;
        private Button btn_ZccLoadNotes;
        private RichTextBox zcc_rtb;
        private Label label6;
        private Label label5;
        private DateTimePicker dtp_ZCCDataDo;
        private DateTimePicker dtp_ZCCDataOd;
        private TabPage tabSlowniki;
        private Button btn_SlownikiZapisz;
        private DataGridView compDgv;
        private Button btn_SlownikiOdswiez;
        private TabPage tabPage6;
        private DataGridView Historia_dgv;
        private Button btn_HistoriaSzukaj;
        private TextBox textBox2;
        private TabPage tp_diagnoza;
        private CheckBox ckb_pokazujDrzewko;
        private Label label4;
        private ComboBox cb_DiagnozaFiltr;
        private ComboBox cb_diagnozaUsers;
        private DataGridView dgv_Diagnoza;
        private Button btn_DiagnozaWczytaj;
        private TabPage tp_Sla;
        private Panel panel5;
        private CheckBox cb_dgv_Wypalony;
        private CheckBox cb_dgv_Pauza;
        private Label label14;
        private CheckBox cb_dgv_IssueId;
        private Panel panel4;
        private TextBox tb_sla_ileWstrzymane;
        private Label label29;
        private TextBox tb_sla_ileRealizacja;
        private Label label28;
        private CheckBox cb_SLAWstrzymane;
        private Button bt_SLA_synchronizacja;
        private CheckBox cb_SLA_JiraSynchro;
        private CheckBox cb_SLApauza;
        private Button btn_slaRaport_Load;
        private DataGridView dgv_SlaRaport;
        private TabPage tabPage2;
        private Panel panel3;
        private Button button7;
        private CheckBox checkBox1;
        private CheckedListBox checkedListBox1;
        private Button button6;
        private TextBox textBox4;
        private DataGridView dataGridView1;
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
        private Button button5;
        private TextBox textBox3;
        private TabPage tp_DayReport;
        private Button button2;
        private Button bt_dayReportSend;
        private CheckBox cb_dayReportAccident;
        private TextBox tb_dayReportTimeTo;
        private TextBox tb_dayReportTimeFrom;
        private RichTextBox rtb_dayReportMessage;
        private Label label30;
        private Label label31;
        private DateTimePicker dtp_dayReportDateTo;
        private DateTimePicker dtp_dayReportDateFrom;
        private Button bt_dayReportGenerate;
        private TabPage tp_PI;
        private DateTimePicker dateTimePicker3;
        private Button button9;
        private DataGridView dgv_PI;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn nrPi;
        private DataGridViewTextBoxColumn INS;
        private DataGridViewTextBoxColumn UAT;
        private DataGridViewTextBoxColumn PREPROD;
        private DataGridViewTextBoxColumn PROD;
        private DataGridViewTextBoxColumn iloscInstalacji;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn system;
        private DataGridViewTextBoxColumn sumasys;
        private DataGridViewTextBoxColumn sredniomiesiecznie;
        private DataGridViewTextBoxColumn sumaproblem;
        private DataGridViewTextBoxColumn sredniaproblem;
        private Button button8;
        private TextBox textBox5;
        private Button button4;
        private TabControl tc_awarie;
    }
}

