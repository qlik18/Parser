using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogicLayer.Interface;
using Entities;
using Entities.Enums;
using Atlassian.Jira;
using System.Diagnostics;

namespace GUI
{
    public partial class WFSModelerForm : Form
    {
        Entities.EventParamModeler ep;
        public string nazwa;
        EventParamSource komponents;
        EventParamSource subkomponents;
        System.Windows.Forms.RichTextBox RichTextBox1;
        public delegate void calbackDelegate(string issueId, string eventName, TreeView tr);
        public delegate void callbackMultiDelegate(List<string> issueId, string eventName, TreeView tr);
        public calbackDelegate callback;
        public callbackMultiDelegate callbackMulti;
        List<EventParamSource> sources = new List<EventParamSource>();
        private List<HeliosUser> PolsatUsers;
        public IParserEngineWFS gujaczWFS;
        public BillingIssueDto issue;
        
        #region do process managera
        private bool isProcessError = false;
        Panel procManagerPanel = null;
        ComboBox cb_Error = null;
        ComboBox cb_Solution = null;
        TextBox tb_ProcessId = null;
        private int processId;
        List<Entities.ProcessError> errors;
        List<Entities.ProcessSolution> solutions;
        #endregion


        int[] dodatkoweDane = new int[3] { -1, -1, -1 };
        int eventMoveId;
        TreeNode selectedNode;
        String eventName;
        List<Entities.EventParamModeler> list;
        FlowLayoutPanel fl;
        List<GroupBox> gbList = new List<GroupBox>();
        Entities.Note note;
        RichTextBox[] rich = new RichTextBox[2];
        TextBox[] tbBox = new TextBox[4];
        ComboBox[] combo = new ComboBox[4];
        TreeView _tr;

        #region multi obsługa zgłoszeń
        bool isMulti = false;
        bool _quickStep = false;
        List<Entities.BillingIssueDto> issues;
        #endregion



        public WFSModelerForm(List<HeliosUser> PolsatUsers, List<Entities.EventParamModeler> list, string name, BillingIssueDto issue, IParserEngineWFS gujaczWFS, int eventMoveId, calbackDelegate callback, TreeView tr, bool quickStep = false, object selectOption = null)
        {
            this._tr = tr;
            this.PolsatUsers = PolsatUsers;
            this.eventName = name.Split('(').First(); ;
            this.callback = callback;
            this.eventMoveId = eventMoveId;
            this.gujaczWFS = gujaczWFS;
            this.issue = issue;
            InitializeComponent();
            this.list = list;
            List<Entities.EventParam> boundEventParams;
            KeyValuePair<int, string> _selectOption = new KeyValuePair<int, string>();

            if (quickStep)
            {
                _quickStep = quickStep;
                _selectOption = (KeyValuePair < int, string >)selectOption;
                this.Visible = false;
            }

            EventParam kom = new EventParam();
            try
            {
                if (eventMoveId == 617)
                {

                    boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.BoundEventParamId > 0).Select(x => x.BoundEventParamId).ToArray<int>());

                    //boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.EventParamId == 2852).Select(x => x.EventParamId).ToArray<int>());

                    //kom.DBExtValue = null;
                    //kom.DBValue = _selectOption.Key;
                    //kom.EventParamId = 2852;
                    //kom.Value = _selectOption.Value;

                    //boundEventParams.Add(kom);


                    //boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.EventParamId == 2853).Select(x => x.EventParamId).ToArray<int>());

                    //kom.DBExtValue = null;
                    //kom.DBValue = null;
                    //kom.EventParamId = 2853;
                    //kom.Value = "QuickStep";

                    //boundEventParams.Add(kom);
                }
                else
                {
                    boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.BoundEventParamId > 0).Select(x => x.BoundEventParamId).ToArray<int>());

                }
                //if (eventMoveId != 618)
                //{
                foreach (Entities.EventParam item in boundEventParams)
                {
                    list.First(x => x.BoundEventParamId == item.EventParamId).BoundEventParam = item;
                    //list.Last(x => x.BoundEventParamId == item.EventParamId).BoundEventParam = item;
                }
                //}

                this.Text = name + " zgłoszenie: " + issue.issueWFS.NumerZgloszenia;


                //if (eventMoveId == 616)
                //{
                //    if (issue.issueWFS.Rodzaj.Value == 888 && issue.issueWFS.Typ.Value != 1036)
                //    {
                //        // Twórz process manager panel
                //    }
                //}
                //if (eventMoveId == 608)
                //{

                //}
                //if (eventMoveId == 609)
                //{

                //}
                if (!_quickStep)
                    GenerateForm(false);
                else
                {
                    GenerateForm(false);
                    this.Visible = false;



                    //Logging.Logger.Instance.LogWarning(string.Format("Dodanie parametrów dla issueId: {0}", issue.issueWFS.WFSIssueId));

                    foreach (var item in sources)
                    {



                        if (item.param.EventParamId == 2852)
                        {
                            item.param.DBValue = _selectOption.Key;
                            item.param.Value = _selectOption.Value;

                            if (item.type == typeof(ComboBox))
                            {
                                ((ComboBox)item.source).Text = _selectOption.Value;
                                ((ComboBox)item.source).SelectedValue = _selectOption.Key;

                                ((ComboBox)item.source).SelectedItem = ((ComboBox)item.source).Items.IndexOf(_selectOption.Value);

                                //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                                //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                                //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                                break;
                            }
                        }
                        if (item.param.EventParamId == 2830 && _selectOption.Value != null)
                        {

                            if (item.type == typeof(SimpleData))
                            {
                                ((SimpleData)item.source).ID = _selectOption.Key;
                                ((SimpleData)item.source).Value = _selectOption.Value;
                            }

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                        else if (item.param.EventParamId == 2853)
                        {
                            if (item.type == typeof(RichTextBox))
                            {
                                ((RichTextBox)item.source).Text = "quickStep test";
                            }
                            //item.param.DBValue = _selectOption.Key;
                            item.param.Value = "quickStep test";

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                    }

                    btn_Save_Click(this, null);
                }
                if (quickStep)
                {
                    _quickStep = false;
                    this.Visible = false;
                }
            }
            catch(Exception ex)
            {
                Logging.Logger.Instance.LogException(ex);
                MessageBox.Show(ex.Message,"Błąd wczytania danych z BPM");
            }
        }


        public WFSModelerForm(List<Entities.EventParamModeler> list, string name, Entities.BillingIssueDto issue, IParserEngineWFS gujaczWFS, int eventMoveId, calbackDelegate callback, TreeView tr, bool quickStep = false, object selectOption = null)
        {
            this._tr = tr;
            this.PolsatUsers = PolsatUsers;
            this.eventName = name.Split('(').First(); ;
            this.callback = callback;
            this.eventMoveId = eventMoveId;
            this.gujaczWFS = gujaczWFS;
            this.issue = issue;
            InitializeComponent();
            this.list = list;
            List<Entities.EventParam> boundEventParams;
            KeyValuePair<int, string> _selectOption = new KeyValuePair<int, string>();

            _quickStep = quickStep;
            if (quickStep)
            {
                if(eventMoveId != 610 && selectOption != null)
                    _selectOption = (KeyValuePair<int, string>)selectOption;
                this.Visible = false;
            }

            EventParam kom = new EventParam();
            try
            {

                boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.BoundEventParamId > 0).Select(x => x.BoundEventParamId).ToArray<int>());

                    //boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.EventParamId == 2852).Select(x => x.EventParamId).ToArray<int>());

                    //kom.DBExtValue = null;
                    //kom.DBValue = _selectOption.Key;
                    //kom.EventParamId = 2852;
                    //kom.Value = _selectOption.Value;

                    //boundEventParams.Add(kom);


                    //boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issue.issueWFS.WFSIssueId, list.Where(x => x.EventParamId == 2853).Select(x => x.EventParamId).ToArray<int>());

                    //kom.DBExtValue = null;
                    //kom.DBValue = null;
                    //kom.EventParamId = 2853;
                    //kom.Value = "QuickStep";

                    //boundEventParams.Add(kom);

                //if (eventMoveId != 618)
                //{
                foreach (Entities.EventParam item in boundEventParams)
                {
                    list.First(x => x.BoundEventParamId == item.EventParamId).BoundEventParam = item;
                    //list.Last(x => x.BoundEventParamId == item.EventParamId).BoundEventParam = item;
                    Debug.WriteLine(string.Format("{0} - {1} - {2}", item.EventParamId, item.Value, item.DBValue));
                }
                //}

                this.Text = name + " zgłoszenie: " + issue.issueWFS.NumerZgloszenia;


                //if (eventMoveId == 616)
                //{
                //    if (issue.issueWFS.Rodzaj.Value == 888 && issue.issueWFS.Typ.Value != 1036)
                //    {
                //        // Twórz process manager panel
                //    }
                //}
                //if (eventMoveId == 608)
                //{

                //}
                //if (eventMoveId == 609)
                //{

                //}
                if (!_quickStep)
                {
                    GenerateForm(false);
                }
                else
                {
                    //GenerateForm(false);
                    GenerateForm(null, 0);
                    this.Visible = false;

                    foreach (var item in sources)
                    {
                        
                        if (item.param.EventParamId == 2852)
                        {
                            item.param.DBValue = _selectOption.Key;
                            item.param.Value = _selectOption.Value;

                            if (item.type == typeof(ComboBox))
                            {
                                ((ComboBox)item.source).Text = _selectOption.Value;
                                ((ComboBox)item.source).SelectedValue = _selectOption.Key;

                                ((ComboBox)item.source).SelectedItem = ((ComboBox)item.source).Items.IndexOf(_selectOption.Value);

                                //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                                //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                                //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                                break;
                            }
                        }
                        if (item.param.EventParamId == 2830 && _selectOption.Value != null)
                        {

                            if (item.type == typeof(SimpleData))
                            {
                                ((SimpleData)item.source).ID = _selectOption.Key;
                                ((SimpleData)item.source).Value = _selectOption.Value;
                            }

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                        else if (item.param.EventParamId == 2853)
                        {
                            if (item.type == typeof(RichTextBox))
                            {
                                ((RichTextBox)item.source).Text = "quickStep test";
                            }
                            //item.param.DBValue = _selectOption.Key;
                            item.param.Value = "quickStep test";

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                        else if (item.param.EventParamId == (int)EventParamNames.Osoba_PRZRKAZANIE_DO_KONS_BIZ && quickStep)
                        {
                            if (item.type == typeof(TextBox))
                            {
                                //((SimpleData)item.source).ID = _selectOption.Key;
                                ((TextBox)item.source).Text = (selectOption as string[])[0];
                            }

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                        else if (item.param.EventParamId == (int)EventParamNames.Mail_PRZRKAZANIE_DO_KONS_BIZ && quickStep)
                        {
                            if (item.type == typeof(TextBox))
                            {
                                ((TextBox)item.source).Text = (selectOption as string[])[1];
                            }

                            //Logging.Logger.Instance.LogInformation(string.Format("EventParamId == {0}", item.param.EventParamId));
                            //Logging.Logger.Instance.LogInformation(string.Format("DBValue == {0}", _selectOption.Key));
                            //Logging.Logger.Instance.LogInformation(string.Format("Value == {0}", _selectOption.Value));
                            break;
                        }
                    }

                    btn_Save_Click(this, null);
                }
                if (quickStep)
                {
                    _quickStep = false;
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.LogException(ex);
                //MessageBox.Show(ex.Message, "Błąd wczytania danych z BPM");
            }
        }

        [Obsolete("Aktualnie multi nie jest w użyciu")]
        public WFSModelerForm(List<EventParamModeler> eventParamForFormByEventMove, List<HeliosUser> PolsatUsers, List<Entities.EventParamModeler> list, string name, List<Entities.BillingIssueDto> issues, IParserEngineWFS gujaczWFS, int eventMoveId, callbackMultiDelegate callback, TreeView tr)
        {
            this._tr = tr;
            this.PolsatUsers = PolsatUsers;
            this.eventName = name.Split('(').First(); ;
            this.callbackMulti = callback;
            this.eventMoveId = eventMoveId;
            this.gujaczWFS = gujaczWFS;
            this.issues = issues;
            this.issue = issues[0];
            InitializeComponent();
            this.list = list;
            List<Entities.EventParam> boundEventParams = gujaczWFS.GetBoundEventParamForIssue(issues[0].issueWFS.WFSIssueId, list.Where(x => x.BoundEventParamId > 0).Select(x => x.BoundEventParamId).ToArray<int>());
            foreach (Entities.EventParam item in boundEventParams)
            {
                list.First(x => x.BoundEventParamId == item.EventParamId).BoundEventParam = item;
            }

            isMulti = true;

            this.Text = name;

            GenerateForm();
        }

        
        private void GenerateForm(bool visible = true)
        {
            GenerateForm(null, 0);
            flowLayoutPanel1.Size = new Size(gbList.First().Size.Width + 10, gbList.Sum(y => y.Size.Height) + 50);
            this.ClientSize = new Size(flowLayoutPanel1.Size.Width + 5, flowLayoutPanel1.Size.Height + 5);
            this.Visible = visible;

        }

        /// <summary>
        /// Rekurencyjne generowanie elementów na formie
        /// </summary>
        /// <param name="group"></param>
        /// <param name="ParamGroupId"></param>
        [STAThreadAttribute]
        private void GenerateForm(GroupBox group, int ParamGroupId)
        {
           
            //if(group== null) group= new GroupBox();
            foreach (Entities.EventParamModeler item in list.Where(x => x.ParamGroupId == ParamGroupId))
            {
                if (item.ParamGroupId == 0) group = null;
                if (item.EventParamId == 727)
                {
                    item.TechName = "ddl";
                }
                switch (item.TechName)
                {
                    case "group": group = AddGroup(item, group);
                        break;
                    case "textbox":
                        AddTextBox(item, group);
                        break;
                    case "comment": AddRichBox(item, group);
                        break;
                    case "checkbox": AddCheckBox(item, group);
                        break;
                    case "label": AddLabel(item, group);
                        break;
                    case "autouser": AddAutoUser(item, group);
                        break;
                    case "calendar": AddTextBox(item, group);
                        break;
                    case "ddl":
                        AddComboBox(item, group);
                        break;
                    default: break;
                }
                if (item.TechName == "group")
                {
                    if (item.ParamGroupId == 0) gbList.Add(group);
                    GenerateForm(group, item.EventParamId);
                }
            }
                
            

        }

        private void GetDataFromProcedure(Entities.EventParamModeler item, GroupBox group)
        {
            var p = gujaczWFS.getUser().login;
            int UserId = gujaczWFS.getUser().Id;

            Dictionary<int, string> result = new Dictionary<int, string>();
            List<List<string>> resultList = new List<List<string>>();
            switch (item.Content)
            {
                case "Support_IssuesForUser": resultList = gujaczWFS.ExecuteStoredProcedure(item.Content, new string[] { UserId.ToString() }); break;
                case "EDS_GetUsersByGroup": resultList = gujaczWFS.ExecuteStoredProcedure(item.Content, new string[] { "436" }); break;
                case "DS_TestDoKasacji": break;
                case "CP_GetInitialDiagnosis": result = Implementation.GeneralStatic.listaKomp.First(x => x.NazwaKomponentu == komponents.boundParam.Value).Diagnozy.ToDictionary(x => x.IdDiagnozy, x => x.NazwaDiagnozy); break;
                case "CP_GetComponents": result = Implementation.GeneralStatic.listaKomp.ToDictionary(x => x.IdKomponentu, x => x.NazwaKomponentu); break;
                default:
                    resultList = gujaczWFS.ExecuteStoredProcedure(item.Content, new string[] { });
                    break;
            }

            if (resultList.Count > 0)
            {
                foreach (var line in resultList)
                {
                    try
                    {
                        result.Add(Convert.ToInt32(line[0]), line[1]);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            List<SimpleData> list = new List<SimpleData>();
            foreach (var it in result)
            {
                list.Add(new SimpleData() { ID = it.Key, Value = it.Value });
            }
            AddComboBoxWithSimpleDataList(item, group, list);
        }

        #region Dodawanie odpowienich kontrolek na formularzu
        private GroupBox AddGroup(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.GroupBox groupBox1 = new GroupBox();
            groupBox1.AutoSize = true;
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Name = ep.Name;
            groupBox1.TabIndex = 1;
            // groupBox1.TabStop = false;
            groupBox1.Text = "Grupa: " + ep.Name;
            if (group == null)
                flowLayoutPanel1.Controls.Add(groupBox1);
            else group.Controls.Add(groupBox1);
            return groupBox1;

        }

        private void AddConsultTypeComboBox(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
            comboBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            comboBox1.Name = ep.Name;
            comboBox1.Size = new System.Drawing.Size(300, ep.Height);
            comboBox1.TabIndex = 1;
            // comboBox1.TabStop = false;
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            //TODO: dodać obsługę tego comboBoxa         
        }
        //[STAThreadAttribute]
        private void AddComboBox(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();

            System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
            comboBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            comboBox1.Name = ep.Name;
            comboBox1.Size = new System.Drawing.Size(180, ep.Height);
            comboBox1.TabIndex = 1;
            // comboBox1.TabStop = false;
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            ///Obsługa deweloperów jako lisa piśmienna
            //comboBox1.DropDownStyle = ( ep.BoundEventParamId == 2800 ||
            //                            ep.BoundEventParamId == 2814 ||
            //                            ep.BoundEventParamId == 3317 ) 
            //                            ? ComboBoxStyle.DropDown//List
            //                            : ComboBoxStyle.DropDownList;

            if (ep.EventParamId == 2800 ||
                ep.EventParamId == 2814 ||
                ep.EventParamId == 3317)
                comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            else
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.DropDown += new EventHandler(AdjustWidthComboBox_DropDown);
            
            int MapId = 0;
            string issueState = gujaczWFS.ExecuteStoredProcedure("spGetIssueState", new string[] { issue.issueWFS.WFSIssueId.ToString() }, DatabaseName.SupportCP)[0][1];


            /*
            combo[0] Obszar
            combo[1] Typ   
            combo[2] Gruba b
            combo[3] Typ b

                    Developer_PRZRKAZANIE_DO_DEV = 2800,
        Developer_PRZRKAZANIE_DO_DEV_CP = 3317,
        Developer_PRZRKAZANIE_DO_KONS_DEV = 3317,
            */
            
            //Deweloper CP
            if (ep.EventParamId == (int)EventParamNames.Developer_PRZRKAZANIE_DO_DEV 
                || ep.EventParamId == (int)EventParamNames.Developer_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.Developer_PRZRKAZANIE_DO_KONS_DEV)
            {
                //BillingDTH_GetCPDevelopers
                List<List<string>> list;
                if (ep.EventParamId == (int)EventParamNames.Developer_PRZRKAZANIE_DO_DEV)
                {
                    list = gujaczWFS.ExecuteStoredProcedure("BillingDTH_GetDevelopers", null, DatabaseName.SupportADDONS);
                    
                }

                else
                {
                    list = gujaczWFS.ExecuteStoredProcedure("BillingDTH_GetCPDevelopers", null, DatabaseName.SupportADDONS);
                }
                Dictionary<int, string> dev = new Dictionary<int, string>();

                foreach (var item in list)
                {
                    dev.Add(int.Parse(item[0]), item[1]);
                }

                foreach (KeyValuePair<int, string> s in dev)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Proponowane rozwiązanie
            else if (ep.EventParamId == (int)EventParamNames.PropRozwiazanie_PRZRKAZANIE_DO_DEV)
            {
                Dictionary<int, string> prRoz = gujaczWFS.getBillingComponents(-3);

                foreach (KeyValuePair<int, string> s in prRoz)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Obszar
            else if (ep.EventParamId == (int)EventParamNames.Obszar_PRZRKAZANIE_DO_DEV
                || ep.EventParamId == (int)EventParamNames.Obszar_PRZYJECIE_DO_REALIZAJCJI
                || ep.EventParamId == (int)EventParamNames.Obszar_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.Obszar_ROZP_DIAGNOZY
                || ep.EventParamId == (int)EventParamNames.Obszar_AKTUALIZACJA1
                || ep.EventParamId == (int)EventParamNames.Obszar_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.Obszar_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.Obszar_AKTUALIZACJA4)
            {
                Dictionary<int, string> kat = gujaczWFS.getBillingComponents(0);

                foreach (KeyValuePair<int, string> s in kat)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                List<EventParam> evep = new List<EventParam>();


                
                if (issueState == "Czeka na diagnoze")
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.Obszar_UTWORZENIE_ZGLOSZENIA });
                else
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.Obszar_ROZP_DIAGNOZY });

                dodatkoweDane[0] = (int)evep[0].DBValue;
                for (int j = 0; j < comboBox1.Items.Count; j++)
                {
                    if (((BillingDthLBItem)comboBox1.Items[j]).Value == evep[0].DBValue)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }
                        break;
                    }
                }

                if(comboBox1.SelectedIndex == -1)
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Text == evep[0].Value)
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }

                comboBox1.SelectedIndexChanged += new EventHandler(tbKategoria_SelectedIndexChanged);

                combo[0] = comboBox1; //Obszar 
                dodatkoweInfo();
                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Typ zgłoszenia
            else if (ep.EventParamId == (int)EventParamNames.TypZgl_PRZRKAZANIE_DO_DEV
                || ep.EventParamId == (int)EventParamNames.TypZgl_PRZYJECIE_DO_REALIZAJCJI
                || ep.EventParamId == (int)EventParamNames.TypZgl_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.TypZgl_ROZP_DIAGNOZY
                || ep.EventParamId == (int)EventParamNames.TypZgl_AKTUALIZACJA1
                || ep.EventParamId == (int)EventParamNames.TypZgl_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.TypZgl_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.TypZgl_AKTUALIZACJA4)
            {
                Dictionary<int, string> sys = gujaczWFS.getBillingComponents(((BillingDthLBItem)combo[0].SelectedItem).Value);
                //gujaczWFS.getBillingComponents(-1);

                foreach (KeyValuePair<int, string> s in sys)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }
                List<EventParam> evep = new List<EventParam>();


                if (issueState == "Czeka na diagnoze")
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.TypZgl_UTWORZENIE_ZGLOSZENIA });
                else
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.TypZgl_ROZP_DIAGNOZY });

                for (int j = 0; j < comboBox1.Items.Count; j++)
                {
                    if (((BillingDthLBItem)comboBox1.Items[j]).Value == evep[0].DBValue)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }
                        break;
                    }
                }

                if (comboBox1.SelectedIndex == -1)
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Text == evep[0].Value)
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }

                combo[1] = comboBox1;

                comboBox1.SelectedIndexChanged += new EventHandler(tbSystem_SelectedIndexChanged);

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }

            //Rodzaj błędu
            else if (ep.EventParamId == (int)EventParamNames.RodzBledu_PRZRKAZANIE_DO_DEV
                || ep.EventParamId == (int)EventParamNames.RodzBledu_PRZYJECIE_DO_REALIZAJCJI
                || ep.EventParamId == (int)EventParamNames.RodzBledu_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.RodzBledu_ROZP_DIAGNOZY
                || ep.EventParamId == (int)EventParamNames.RodzBledu_AKTUALIZACJA1
                || ep.EventParamId == (int)EventParamNames.RodzBledu_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.RodzBledu_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.RodzBledu_AKTUALIZACJA4)
            {
                Dictionary<int, string> rodz = gujaczWFS.getBillingComponents(((BillingDthLBItem)combo[1].SelectedItem).Value);
                
                foreach (KeyValuePair<int, string> s in rodz)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                List<EventParam> evep = new List<EventParam>();


                if (issueState == "Czeka na diagnoze")
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.RodzBledu_UTWORZENIE_ZGLOSZENIA});
                else
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.RodzBledu_ROZP_DIAGNOZY });


                dodatkoweDane[1] = (int)evep[0].DBValue;

                List<List<string>> RodzajMap = gujaczWFS.ExecuteStoredProcedure("spPobierzZmapowanaWartosc", new string[] { evep[0].DBValue.ToString() }, DatabaseName.SupportCP);
                

                if(RodzajMap.Count() > 0)
                {
                    MapId = Convert.ToInt32(RodzajMap[0][0]);
                }

                for (int j = 0; j < comboBox1.Items.Count; j++)
                {
                    if (((BillingDthLBItem)comboBox1.Items[j]).Value == evep[0].DBValue)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }
                        
                        break;
                    }
                    else if (((BillingDthLBItem)comboBox1.Items[j]).Value == MapId && MapId != 0)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }

                        break;
                    }

                }

                if (comboBox1.SelectedIndex == -1)
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Text == evep[0].Value)
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }

                if (comboBox1.SelectedIndex == -1)
                {
                    comboBox1.SelectedIndex = 0;
                }

                comboBox1.SelectedIndexChanged += new EventHandler(tbRodzaj_SelectedIndexChanged);

                if (comboBox1.SelectedIndex == 2)
                    isProcessError = true;
                else
                    isProcessError = false;

                combo[2] = comboBox1;
                dodatkoweInfo();

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Typ błędu
            else if (ep.EventParamId == (int)EventParamNames.TypBledu_PRZRKAZANIE_DO_DEV
                || ep.EventParamId == (int)EventParamNames.TypBledu_PRZYJECIE_DO_REALIZAJCJI
                || ep.EventParamId == (int)EventParamNames.TypBledu_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.TypBledu_ROZP_DIAGNOZY
                || ep.EventParamId == (int)EventParamNames.TypBledu_AKTUALIZACJA1
                || ep.EventParamId == (int)EventParamNames.TypBledu_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.TypBledu_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.TypBledu_AKTUALIZACJA4)
            {
                Dictionary<int, string> typy = gujaczWFS.getBillingComponents(((BillingDthLBItem)combo[2].SelectedItem).Value);

                foreach (KeyValuePair<int, string> s in typy)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                List<EventParam> evep = new List<EventParam>();

                if (issueState == "Czeka na diagnoze")
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.TypBledu_UTWORZENIE_ZGLOSZENIA });
                else
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.TypBledu_ROZP_DIAGNOZY });


                dodatkoweDane[2] = (int)evep[0].DBValue;

                List<List<string>> RodzajMap = gujaczWFS.ExecuteStoredProcedure("spPobierzZmapowanaWartosc", new string[] { evep[0].DBValue.ToString() }, DatabaseName.SupportCP);
                
                if (RodzajMap.Count() > 0)
                {
                    MapId = Convert.ToInt32(RodzajMap[0][0]);
                }

                for (int j = 0; j < comboBox1.Items.Count; j++)
                {
                    if (((BillingDthLBItem)comboBox1.Items[j]).Value == evep[0].DBValue)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }
                        break;
                    }
                    else if (((BillingDthLBItem)comboBox1.Items[j]).Value == MapId && MapId != 0)
                    {
                        if (comboBox1.Items.Count >= j)
                        {
                            comboBox1.SelectedIndex = j;
                        }

                        break;
                    }
                }

                if (comboBox1.SelectedIndex == -1)
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Text == evep[0].Value)
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }
               
                if (comboBox1.SelectedIndex == -1)
                { //ostatecznie ustaw pierwszą pozycję
                    comboBox1.SelectedIndex = 0;
                }

                comboBox1.SelectedIndexChanged += new EventHandler(tbTyp_SelectedIndexChanged);
                
                if (isProcessError)
                {
                    if (comboBox1.SelectedIndex != 87)
                        processManager(comboBox1.SelectedItem.ToString());
                    else
                        disposeProcManagerPanel();
                }
                else
                    disposeProcManagerPanel();

                combo[3] = comboBox1;

                dodatkoweInfo();

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Konsultant
            else if (ep.EventParamId == (int)EventParamNames.OsOdpowiedzialna_AKTUALIZACJA1 
                || ep.EventParamId == (int)EventParamNames.OsOdpowiedzialna_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.OsOdpowiedzialna_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.OsOdpowiedzialna_AKTUALIZACJA4
                || ep.EventParamId == (int)EventParamNames.OsOdpowiedzialna_ZM_WYKONAWCY)
            {

                List<List<string>> list = gujaczWFS.ExecuteStoredProcedure("EDS_GetUsersByGroup", new string[] { "436" }, DatabaseName.SupportBPM);
                Dictionary<int, string> konsultanci = new Dictionary<int, string>();

                foreach (var item in list)
                {
                    konsultanci.Add(int.Parse(item[0]), item[1]);
                }

                int selectedIndex = 0;

                foreach (KeyValuePair<int, string> s in konsultanci)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                    if (s.Value.Equals(gujaczWFS.getUser().surname + " " + gujaczWFS.getUser().name))
                        selectedIndex = comboBox1.Items.Count - 1;
                }

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = selectedIndex;

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1, //new SimpleData() { ID = gujaczWFS.getUser().Id, Value = gujaczWFS.getUser().FullName },
                    isRequired = true
                });
            }
            //Podód zamknięcia zgłoszenia
            else if (ep.EventParamId == (int)EventParamNames.Powod_ZAMKNIECIE_ZGLOSZENIA)
            {
                Dictionary<int, string> propRozw = gujaczWFS.getBillingComponents(-2);

                foreach (KeyValuePair<int, string> s in propRozw)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                }

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }
            //Priorytet zgłoszenia
            else if (ep.EventParamId == (int)EventParamNames.Priorytet_PRZRKAZANIE_DO_DEV
                || ep.EventParamId == (int)EventParamNames.Priorytet_PRZYJECIE_DO_REALIZAJCJI
                || ep.EventParamId == (int)EventParamNames.Priorytet_PRZRKAZANIE_DO_DEV_CP
                || ep.EventParamId == (int)EventParamNames.Priorytet_ROZP_DIAGNOZY
                || ep.EventParamId == (int)EventParamNames.Priorytet_AKTUALIZACJA1
                || ep.EventParamId == (int)EventParamNames.Priorytet_AKTUALIZACJA2
                || ep.EventParamId == (int)EventParamNames.Priorytet_AKTUALIZACJA3
                || ep.EventParamId == (int)EventParamNames.Priorytet_AKTUALIZACJA4)
            {
                List<List<string>> priorytet = gujaczWFS.ExecuteStoredProcedure("Billing_GetListOfPriorities", new string[] { }, DatabaseName.SupportADDONS);

                foreach (var item in priorytet)
                {
                    comboBox1.Items.Add(new Entities.BillingDthLBItem() { Text = item[1], Value = Convert.ToInt32(item[0]) });
                }

                List<EventParam> evep = new List<EventParam>();

                if (issueState == "Czeka na diagnoze")
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.Priorytet_UTWORZENIE_ZGLOSZENIA });
                else
                    evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { (int)EventParamNames.Priorytet_ROZP_DIAGNOZY });

                if (evep.Count > 0)
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Value == evep[0].DBValue)
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < comboBox1.Items.Count; j++)
                    {
                        if (((BillingDthLBItem)comboBox1.Items[j]).Value == Convert.ToInt32(issue.issueWFS.Priorytet))
                        {
                            if (comboBox1.Items.Count >= j)
                            {
                                comboBox1.SelectedIndex = j;
                            }
                            break;
                        }
                    }
                }

                sources.Add(new EventParamSource()
                {
                    type = typeof(ComboBox),
                    param = new EventParam() { EventParamId = ep.EventParamId },
                    source = comboBox1,
                    isRequired = true
                });
            }


            group.Controls.Add(comboBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
        }

        private void AddComboBoxWithSimpleDataList(Entities.EventParamModeler ep, GroupBox group, List<SimpleData> list)
        {

            this.SuspendLayout();
            System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
            if (ep.EventParamId == 780) { komponents = new EventParamSource() { source = comboBox1, boundParam = ep.BoundEventParam }; comboBox1.SelectedIndexChanged += new EventHandler(changeKomponents); }
            if (ep.EventParamId == 778) subkomponents = new EventParamSource() { source = comboBox1 };
            comboBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            comboBox1.Name = ep.Name;

            foreach (SimpleData item in list)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
            if (ep.BoundEventParam != null)
            {
                comboBox1.SelectedItem = comboBox1.Items[comboBox1.Items.IndexOf(list.First(x => x.Value == ep.BoundEventParam.Value))];
            }
            comboBox1.Size = new System.Drawing.Size(300, ep.Height);
            comboBox1.TabIndex = 1;
            // comboBox1.TabStop = false;
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(ComboBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = comboBox1,
                boundParam = ep.BoundEventParam
            });
            group.Controls.Add(comboBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);

        }
        private void changeKomponents(object sender, EventArgs e)
        {
            if (subkomponents != null)
            {
                (subkomponents.source as ComboBox).Items.Clear();
                try
                {
                    foreach (Diagnoza item in Implementation.GeneralStatic.listaKomp.FirstOrDefault(x => x.NazwaKomponentu == ((sender as ComboBox).SelectedItem as SimpleData).Value).Diagnozy.ToList())
                    {
                        (subkomponents.source as ComboBox).Items.Add(new SimpleData() { ID = item.IdDiagnozy, Value = item.NazwaDiagnozy });
                    }
                    (subkomponents.source as ComboBox).SelectedIndex = 0;
                }
                catch { }
            }
        }


        private void AddPoslatUsers(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
            comboBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            comboBox1.Name = ep.Name;
            comboBox1.Size = new System.Drawing.Size(300, ep.Height);
            comboBox1.TabIndex = 1;
            comboBox1.TabStop = false;
            foreach (Entities.HeliosUser item in PolsatUsers.OrderBy(x => x.nazwisko).ToList())
            {
                comboBox1.Items.Add(new SimpleData() { Value = item.ToString() });
            }
            foreach (Implementation.UserInfo item in Implementation.GeneralStatic.Developers.OrderBy(x => x.surname).ToList())
            {
                comboBox1.Items.Add(new SimpleData() { Value = item.ToString() });
            }
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(ComboBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = comboBox1,
                boundParam = ep.BoundEventParam
            });
            if (issue is BillingIssueDtoHelios)
            {
                var p = PolsatUsers.Where(x => x.email == ((BillingIssueDtoHelios)issue).issueHelios.email).FirstOrDefault();
                SimpleData sp = new SimpleData() { Value = p.ToString() };
                var i = comboBox1.Items.IndexOf(p);
                comboBox1.SelectedIndex = i;
            }
            group.Controls.Add(comboBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
        }


        private void AddTextBox(Entities.EventParamModeler ep, GroupBox p)
        {
            this.SuspendLayout();
            System.Windows.Forms.TextBox TextBox1 = new TextBox();
            TextBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            TextBox1.Name = "textBox_" + ep.EventParamId.ToString();
            TextBox1.Size = new System.Drawing.Size(ep.Width, ep.Height);
            TextBox1.TabIndex = 2;
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(TextBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = TextBox1,
                boundParam = ep.BoundEventParam
            });

            if (ep.EventParamId == (int)EventParamNames.Osoba_PRZRKAZANIE_DO_KONS_BIZ)
            {
                TextBox1.Text = issue.issueWFS.Imie + " " + issue.issueWFS.Nazwisko;
            }
            else if (ep.EventParamId == (int)EventParamNames.Mail_PRZRKAZANIE_DO_KONS_BIZ)
            {
                TextBox1.Text = issue.issueWFS.Email;
            }
            else if (ep.EventParamId == 2868 || ep.EventParamId == 2869)
            {
                List<EventParam> evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { 2867 });
                TextBox1.Text = evep[0].Value;
            }
            else if (ep.EventParamId == 3526 || ep.EventParamId == 3524)
            {
                List<EventParam> evep = gujaczWFS.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { 3525 });

                if (evep.Count > 0)
                    TextBox1.Text = evep[0].Value;
                else
                    TextBox1.Text = issue.issueWFS.IdZamowienia;
            }
            else if (ep.EventParamId == 3705 || ep.EventParamId == 2831 || ep.EventParamId == 3767 || ep.EventParamId == 3736 ||
                ep.EventParamId == 3745 || ep.EventParamId == 3754 || ep.EventParamId == 3763)
            {
                string number = string.Empty;

                if (ep.BoundEventParam != null)
                {
                    if (issue.Idnumber != ep.BoundEventParam.Value)
                    {
                        number = issue.Idnumber;
                    }
                    else
                    {
                        number = ep.BoundEventParam.Value;
                    }
                }

                TextBox1.Text = number;
            }
            else if (ep.EventParamId == 2865)//przekazanie do dewelopera - Data wystąpienia błędu
            {
                getValueForEventParamId(ep, TextBox1);
                //TextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
                //TextBox1.BackColor = Color.PeachPuff;
                //data_epv_2865 = TextBox1.Text;
                //tbBox[0] = TextBox1;
                
                //if (rich[1] != null)
                //{
                //    rich[1].Clear();
                //    rich[1].AppendText("Pobrano datę utworzenia problemu z Incydentu\n");
                //    rich[1].AppendText(data_epv_2865);
                //}
            }
            else if (ep.EventParamId == 4794)//przekazanie do dewelopera - Numer problemu JIRA
            {
                TextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
                TextBox1.TextChanged += new EventHandler(tbDataUtrzorzeniaProblemuAutoComplete);
            }
            else if (ep.EventParamId == 6826)//przekazanie do dewelopera - ID JIRA
            {
                getValueForEventParamId(ep, TextBox1);

                //TextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
                //TextBox1.BackColor = Color.PeachPuff;
                //data_epv_6826 = TextBox1.Text;
                //tbBox[1] = TextBox1;

                //if (rich[1] != null)
                //{
                //    rich[1].Clear();
                //    rich[1].AppendText("Pobrano Id Jira problemu z Incydentu\n");
                //    rich[1].AppendText(data_epv_6826);
                //}
            }
            else if (ep.EventParamId == 6827)//przekazanie do dewelopera - Środowisko problemu
            {
                getValueForEventParamId(ep, TextBox1);

                /*
                TextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
                TextBox1.BackColor = Color.PeachPuff;
                data_epv_6827 = TextBox1.Text;
                tbBox[2] = TextBox1;

                if (rich[1] != null)
                {
                    rich[1].Clear();
                    rich[1].AppendText("Pobrano Środowisko problemu\n");
                    rich[1].AppendText(data_epv_6827);
                }
                */
            }
            else
            {
                TextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
            }

            if (ep.EventParamId == 5799 || ep.EventParamId == 5800)
            {
                //Uniewidocznienie paramertrów HID
                TextBox1.Visible = false;
                ep.LabelLeft = 0;
                ep.LabelTop = 0;
            }

            p.Controls.Add(TextBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0) 
                AddLabel(ep, p);
        }

        IEnumerable<Issue> sJiraIssue;
        string data_epv_2865;
        string data_epv_6826;
        string data_epv_6827;

        private void getValueForEventParamId(EventParamModeler epm, TextBox tb)
        {
            tb.Text = epm.BoundEventParam != null ? epm.BoundEventParam.Value : "";
            tb.BackColor = Color.PeachPuff;

            switch (epm.EventParamId)
            {
                case 2865: data_epv_2865 = tb.Text; tbBox[0] = tb; break;
                case 6826: data_epv_6826 = tb.Text; tbBox[1] = tb; break;
                case 6827: data_epv_6827 = tb.Text; tbBox[2] = tb; break;
            }

            if (rich[1] != null)
            {
                rich[1].Clear();
                rich[1].AppendText("Pobrano " + epm.Name);
                rich[1].AppendText(tb.Text);
            }
        }

        private void tbDataUtrzorzeniaProblemuAutoComplete(object sender, EventArgs e)
        {

            TextBox tb = ((TextBox)sender);
            //INFOLINIA - 5320
            string nrProblemu = tb.Text.TrimStart().TrimEnd();
            int ileCyfr = 0;
            if(nrProblemu.IndexOf("-") > 0 && nrProblemu.Substring(nrProblemu.IndexOf("-") + 1).Length > 0)
            {
                string stringInt = nrProblemu.Substring(nrProblemu.IndexOf("-")+ 1);
                if(!Int32.TryParse(stringInt,out ileCyfr))
                {
                    ileCyfr = 0;
                }
               // Convert.ToInt32(nrProblemu.Substring(nrProblemu.IndexOf("-")));
            }
            if (ileCyfr > 0)
            {
                //Jira j = new Jira("http://jira", GUI. . .Login, jiraUser.Password);
                
                new MainForm().SearchJiraIssueAsync(nrProblemu,out sJiraIssue);

                if (sJiraIssue != null && sJiraIssue.Count() > 0)
                {
                    foreach (var item in sJiraIssue)
                    {

                        //((TextBox)Container.Components["textBox_2865"]).Text
                        tbBox[0].Text = item.Created.ToString();
                        tbBox[0].BackColor = Color.LightGreen;

                        tbBox[1].Text = item.JiraIdentifier.ToString();
                        tbBox[1].BackColor = Color.LightGreen;

                        tbBox[2].Text = item.CustomFields["Środowisko Problemu"].Values[0].ToString();
                        tbBox[2].BackColor = Color.LightGreen;

                        rich[1].Clear();
                        rich[1].AppendText("Automatycznie pobrano datę utworzenia problemu");
                        rich[1].AppendText("\nDla zgłoszenia JIRA: " + nrProblemu + " (" + item.JiraIdentifier +")");
                        
                        rich[1].AppendText("\n");
                           
                        rich[1].AppendText("\nWybrano - 'Data utworzenia zgłoszenia': " + item.Created.ToString());
                        rich[1].AppendText("\nOpcjonalnie - 'Data ostatniej aktualizacji': " + item.Updated.ToString());
                    }
                }
                else
                {
                    tbBox[0].Text = data_epv_2865;
                    tbBox[0].BackColor = Color.PeachPuff;

                    tbBox[1].Text = data_epv_6826;
                    tbBox[1].BackColor = Color.PeachPuff;

                    tbBox[2].Text = data_epv_6827;
                    tbBox[2].BackColor = Color.PeachPuff;

                    rich[1].Clear();
                    rich[1].AppendText("Pobrano datę utworzenia problemu z Incydentu\n");
                    rich[1].AppendText(data_epv_2865);

                }

                tb.Text = nrProblemu;
            }
            else
            {
                tbBox[0].Text = data_epv_2865;
                tbBox[0].BackColor = Color.PeachPuff;

                tbBox[1].Text = data_epv_6826;
                tbBox[1].BackColor = Color.PeachPuff;

                tbBox[2].Text = data_epv_6827;
                tbBox[2].BackColor = Color.PeachPuff;

                rich[1].Clear();
                rich[1].AppendText("Pobrano datę utworzenia problemu z Incydentu\n");
                rich[1].AppendText(data_epv_2865);

            }

        }

        private void AddAutoUser(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.TextBox TextBox1 = new TextBox();
            TextBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            TextBox1.Name = "textBox_" + ep.EventParamId.ToString();
            TextBox1.Size = new System.Drawing.Size(ep.Width, ep.Height);
            TextBox1.TabIndex = 2;
            User user = gujaczWFS.getUser();
            TextBox1.Text = gujaczWFS.getUser().FullName;
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(SimpleData),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = new SimpleData() { ID = gujaczWFS.getUser().Id, Value = gujaczWFS.getUser().FullName },
                boundParam = ep.BoundEventParam
            });

            group.Controls.Add(TextBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
        }

        private void AddRichBox(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            this.RichTextBox1 = new RichTextBox();
            RichTextBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            RichTextBox1.Name = "richTextBox_" + ep.EventParamId.ToString();
            RichTextBox1.Size = new System.Drawing.Size(ep.Width, ep.Height);
            RichTextBox1.TabIndex = 3;
            RichTextBox1.Text = ep.BoundEventParam != null ? ep.BoundEventParam.Value : "";
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(RichTextBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = RichTextBox1,
                boundParam = ep.BoundEventParam
            });
            if (ep.EventParamId == 3198 || ep.EventParamId == 3196)
            {
                rich[0] = RichTextBox1;
                dodatkoweInfo();
            }
            else if (ep.EventParamId == 2806)//przekazanie do dewelopera - Komentarz
            {
                rich[1] = RichTextBox1;
                
            }
            group.Controls.Add(RichTextBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
            nazwa = RichTextBox1.Name.ToString();
        }
        private void AddCheckBox(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.CheckBox CheckBox1 = new CheckBox();
            CheckBox1.AutoSize = true;
            CheckBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            CheckBox1.Name = "checkBox_" + ep.EventParamId.ToString();
            CheckBox1.Size = new System.Drawing.Size(ep.Width, ep.Height);
            CheckBox1.TabIndex = 1;
            if (ep.EventParamId == 2825)
            {
                CheckBox1.Text = "";
            }
            else if (ep.Name == "Czy on call?")// (ep.EventParamId == 6819 || ep.EventParamId == 6817)
            {
                CheckBox1.Text = "";
                if (ep.BoundEventParam != null)
                {
                    CheckBox1.Checked = (ep.BoundEventParam.Value == "True" ? true : false);
                }
                else
                {
                    CheckBox1.Checked = false;
                }
            }
            else
            {
                CheckBox1.Text = ep.Details;
            }
            CheckBox1.UseVisualStyleBackColor = true;
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(CheckBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = CheckBox1,
                boundParam = ep.BoundEventParam
            });

            group.Controls.Add(CheckBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
        }

        private void AddComboBoxDictionary(EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
            comboBox1.Location = new System.Drawing.Point(ep.Left, ep.Top);
            comboBox1.Name = ep.Name;
            comboBox1.Size = new System.Drawing.Size(ep.Width, ep.Height);
            comboBox1.TabIndex = 1;
            comboBox1.TabStop = false;
            List<SimpleData> list = new List<SimpleData>();
            foreach (var item in ep.Content.Split(';'))
            {
                list.Add(new SimpleData() { Value = item, ID = -1 });
            }
            foreach (SimpleData item in list)
            {
                comboBox1.Items.Add(new SimpleData() { Value = item.Value, ID = -1 });
            }
            if (ep.BoundEventParam != null)
            {
                int idx = -1;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if ((comboBox1.Items[i] as SimpleData).Value == ep.BoundEventParam.Value)
                        idx = i;

                }
                comboBox1.SelectedIndex = idx;

            }
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            sources.Add(new EventParamSource()
            {
                isRequired = ep.isRequired,
                type = typeof(ComboBox),
                param = new EventParam() { EventParamId = ep.EventParamId },
                source = comboBox1,
                boundParam = ep.BoundEventParam
            });
            group.Controls.Add(comboBox1);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
            //TODO: dodać obsługę tego comboBoxa  
        }

        private void AddLabel(Entities.EventParamModeler ep, GroupBox p)
        {
            this.SuspendLayout();
            System.Windows.Forms.Label Label1 = new Label();
            Label1.AutoSize = true;
            Label1.Location = new System.Drawing.Point(ep.LabelLeft, ep.LabelTop);
            Label1.Name = "label1_" + ep.EventParamId.ToString();
            Label1.AutoSize = true;
            Label1.TabIndex = 1;
            if (ep.Details == string.Empty || ep.Details.Contains('$') || ep.Details.Contains('0'))
                Label1.Text = ep.Name;
            else
                Label1.Text = ep.Details;
            if(ep.EventParamId == (int)EventParamNames.Developer_PRZRKAZANIE_DO_DEV)
            {
                Label1.ForeColor = Color.Red;

            }

            p.Controls.Add(Label1);
        }

        private void AddCalendar(Entities.EventParamModeler ep, GroupBox group)
        {
            this.SuspendLayout();
            System.Windows.Forms.DateTimePicker dtp = new DateTimePicker();
            dtp.AutoSize = true;
            dtp.Location = new System.Drawing.Point(ep.Left, ep.Top);
            dtp.Name = "dateTimePicker_" + ep.EventParamId.ToString();
            dtp.Size = new System.Drawing.Size(ep.Width, ep.Height);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "yyyy-MM-dd hh:mm tt";
            dtp.TabIndex = 1;
            //dtp.Text = ep.Details;
            dtp.Value = Convert.ToDateTime(ep.BoundEventParam.Value);
            group.Controls.Add(dtp);

            group.Controls.Add(dtp);
            if (ep.LabelTop != 0 && ep.LabelLeft != 0)
                AddLabel(ep, group);
        }
        #endregion
        //[STAThreadAttribute]
        private void btn_Save_Click(object sender, EventArgs e)
        {
            bool isValidate = true;
            foreach (EventParamSource item in sources)
            {
                if (item.type == typeof(ComboBox))
                {
                    ComboBox cb = (ComboBox)item.source;

                    BillingDthLBItem tmp = (BillingDthLBItem)cb.SelectedItem;

                    item.param.DBValue = tmp.Value;
                    item.param.Value = tmp.Text;

                    System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1}", tmp.Value, tmp.Text));
                }
                else if (item.type == typeof(RichTextBox))
                {
                    item.param.Value = (string)(item.source as RichTextBox).Text;
                }
                else if (item.type == typeof(TextBox))
                {
                    item.param.Value = (string)(item.source as TextBox).Text;
                }
                else if (item.type == typeof(CheckBox))
                {
                    item.param.Value = (item.source as CheckBox).Checked.ToString();
                }
                else if (item.type == typeof(SimpleData))
                {
                    item.param.Value = (item.source as SimpleData).Value;
                    item.param.DBValue = (item.source as SimpleData).ID;
                }
                if (item.isRequired && item.param.Value == "")
                {
                    isValidate = false;
                    break;

                }
            }
            if (isProcessError && procManagerPanel != null)
            {
                if (string.IsNullOrWhiteSpace(tb_ProcessId.Text))
                {
                    isValidate = false;
                }
                else if (cb_Error.SelectedIndex < 0 || cb_Solution.SelectedIndex < 0)
                {
                    isValidate = false;
                }
                else
                {
                    string[] processes = tb_ProcessId.Text.Split(',');

                    List<nProcess> list = new List<nProcess>();

                    foreach (string s in processes)
                    {
                        nProcess proc = new nProcess()
                        {
                            IdProcess = processId,
                            IdError = errors.Where(x => x.description == cb_Error.SelectedItem.ToString()).First().id,
                            IdSolutions = solutions.Where(x => x.nameCP == cb_Solution.SelectedItem.ToString()).First().id,
                            Number = int.Parse(s.Trim()),
                            Comment = "",
                            Date = DateTime.Now,
                            Author = gujaczWFS.getUser().login,
                            FromTask = false
                        };

                        list.Add(proc);
                    }

                    gujaczWFS.InsertNewProcessLog(Logic.Implementation.XmlParser.InsertProcessLogXML(list));
                }
            }
            if (isValidate)
                try
                {
                    if (isMulti)
                    {
                        List<string> issuesId = new List<string>();
                        foreach (BillingIssueDto iss in issues)
                        {
                            List<Entities.EventParam> epList = sources.Select(x => x.param).ToList();
                            if (this.eventMoveId == 608)
                            {
                                if (gujaczWFS.IsIssueInArchive(iss.issueWFS.WFSIssueId))
                                {
                                    gujaczWFS.ExecuteStoredProcedure("Tibco_UndoIssueArchive", new string[] { iss.issueWFS.NumerZgloszenia }, DatabaseName.SupportADDONS);
                                }
                            }
                            gujaczWFS.DoActionInIssue(iss.issueWFS.WFSIssueId, eventMoveId, epList);
                            issuesId.Add(iss.Idnumber);

                        }

                        if (callbackMulti != null)
                        {
                            this.Close();
                            callbackMulti(issuesId, eventName, _tr);
                        }

                    }
                    else if(_quickStep)
                    {
                        List<Entities.EventParam> epList = sources.Select(x => x.param).ToList();

                        if (this.eventMoveId == 608)
                        {
                            if (gujaczWFS.IsIssueInArchive(issue.issueWFS.WFSIssueId))
                            {
                                gujaczWFS.ExecuteStoredProcedure("Tibco_UndoIssueArchive", new string[] { issue.issueWFS.NumerZgloszenia.ToString() }, DatabaseName.SupportADDONS);
                            }

                        }

                        gujaczWFS.DoActionInIssue(Convert.ToInt32(issue.issueWFS.WFSIssueId), eventMoveId, epList);

                        if (callback != null)
                        {
                            this.Close();
                            callback(issue.Idnumber, eventName, _tr);
                        }
                    }
                    else
                    {
                        List<Entities.EventParam> epList = sources.Select(x => x.param).ToList();

                        if (this.eventMoveId == 608)
                        {
                            if (gujaczWFS.IsIssueInArchive(issue.issueWFS.WFSIssueId))
                            {
                                gujaczWFS.ExecuteStoredProcedure("Tibco_UndoIssueArchive", new string[] { issue.issueWFS.NumerZgloszenia.ToString() }, DatabaseName.SupportADDONS);
                            }

                        }

                        gujaczWFS.DoActionInIssue(issue.issueWFS.WFSIssueId, eventMoveId, epList);

                        if (callback != null)
                        {
                            this.Close();
                            callback(issue.Idnumber, eventName, _tr);
                        }
                    }
                    //this.Close();

                }
                catch (Exception ex)
                {
                    Logging.Logger.Instance.LogWarning(ex.Message);
                    //MessageBox.Show(ex.Message);
                    //new Utility.MyCustomException(ex.Message, ex);
                }
            else
                MessageBox.Show("Nie wszystkie wymagane pola zostały uzupełnione");
        }

        private bool addNotExistsJiraUser(string user)
        {
            //Obsługa "Imię Nazwisko"
            string[] userParam = user.Trim().Split(' ');

            List<List<string>> userList =  gujaczWFS.ExecuteStoredProcedure("BillingDTH_GetCPDevelopers", null, DatabaseName.SupportADDONS);

            foreach (var item in userList)
            {
                if(item[0] == userParam[0]
                    && item[1] == userParam[1])
                {
                    return true;
                }
            }

            return false;
        }

        private void tbKategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox kat = (ComboBox)sender;

            Dictionary<int, string> sys = gujaczWFS.getBillingComponents(((Entities.BillingDthLBItem)kat.SelectedItem).Value);
            Dictionary<int, string> rodz = gujaczWFS.getBillingComponents(sys.Keys.ToArray()[0]);
            Dictionary<int, string> typy = gujaczWFS.getBillingComponents(rodz.Keys.ToArray()[0]);
            dodatkoweDane[0] = ((Entities.BillingDthLBItem)kat.SelectedItem).Value;

            combo[1].Items.Clear();
            foreach (KeyValuePair<int, string> s in sys)
            {
                combo[1].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });

                int blankIndex = -1;

                if (s.Value.Equals("Inne"))
                {
                    blankIndex = combo[1].Items.Count - 1;

                }

                combo[1].SelectedIndex = blankIndex;

            }

            combo[2].Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                combo[2].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            combo[3].Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                combo[3].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }



            if (sys.Count > 0)
            {
                combo[1].SelectedIndex = 0;
            }
            if (rodz.Count > 0)
            {
                combo[2].SelectedIndex = 0;
            }
            if (typy.Count > 0)
            {
                combo[3].SelectedIndex = 0;
            }
        }

        private void tbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox sys = (ComboBox)sender;
            
            Dictionary<int, string> rodz = gujaczWFS.getBillingComponents(((Entities.BillingDthLBItem)sys.SelectedItem).Value);
            Dictionary<int, string> typy = gujaczWFS.getBillingComponents(rodz.Keys.ToArray()[0]);
            dodatkoweDane[0] = ((Entities.BillingDthLBItem)sys.SelectedItem).Value;

            combo[2].Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                combo[2].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });

            }

            combo[3].Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                combo[3].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (rodz.Count > 0)
            {
                combo[2].SelectedIndex = 0;
            }
            if (typy.Count > 0)
            {
                combo[3].SelectedIndex = 0;
            }
        }

        private void tbRodzaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox rodz = (ComboBox)sender;

            if (rodz.SelectedIndex == 2)
                isProcessError = true;
            else
                isProcessError = false;

            Dictionary<int, string> typy = gujaczWFS.getBillingComponents(((Entities.BillingDthLBItem)rodz.SelectedItem).Value);
            dodatkoweDane[1] = ((Entities.BillingDthLBItem)rodz.SelectedItem).Value;

            if (typy.Count > 0)
            {
                combo[3].Items.Clear();

                int blankIndex = -1;
                foreach (KeyValuePair<int, string> s in typy)
                {
                    combo[3].Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
                    if (s.Value.Equals("Inne"))
                    {
                        blankIndex = combo[3].Items.Count - 1;
                       
                    }
                    else if (s.Value.Equals("---"))
                    {
                        blankIndex = combo[3].Items.Count - 1;
                      
                    }
                }
                
                combo[3].SelectedIndex = blankIndex;
            }
            dodatkoweInfo();
        }

        private void tbTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox typ = (ComboBox)sender;

            dodatkoweDane[2] = ((Entities.BillingDthLBItem)typ.SelectedItem).Value;
            dodatkoweInfo();

            if (isProcessError)
            {
                if (typ.SelectedIndex != 87)
                    processManager(typ.SelectedItem.ToString());
                else
                    disposeProcManagerPanel();
            }
            else
                disposeProcManagerPanel();
        }

        #region obsługa process managera
        private void disposeProcManagerPanel()
        {
            if (procManagerPanel != null)
            {
                procManagerPanel.Controls.Clear();
                procManagerPanel.Dispose();
                procManagerPanel = null;
            }

            if (errors != null)
                errors.Clear();
            else
                errors = new List<ProcessError>();

            if (solutions != null)
                solutions.Clear();
            else
                solutions = new List<ProcessSolution>();
        }

        private void processManager(string procName)
        {
            disposeProcManagerPanel();
            if (procName != null && procName != "---")
            {
                GroupBox gBox = null;
                RichTextBox rtb = null;

                foreach (Control c in gbList.Last().Controls)
                    if (c is GroupBox)
                        gBox = c as GroupBox;

                if (gBox == null)
                    return;

                foreach (Control c in gBox.Controls)
                    if (c is RichTextBox && c.Name == "richTextBox_3196")
                        rtb = c as RichTextBox;

                if (rtb == null)
                    return;

                if (procManagerPanel != null)
                    procManagerPanel.Controls.Clear();

                string procType = procName.Split(' ').First();
                List<List<string>> processInfo = gujaczWFS.ExecuteStoredProcedure("GetProcessByName", new string[] { procType }, DatabaseName.SupportCP);
                if (processInfo != null && processInfo.Count > 0)
                {
                    processId = int.Parse(processInfo[0][0]);
                }
                else
                    return;

                List<List<string>> topErrorSolution = gujaczWFS.ExecuteStoredProcedure("GetTopErrorAndSolutionForProcess", new string[] { processId.ToString(), "-1" }, DatabaseName.SupportCP);


                int topError = -1;
                int topSolution = -1;
                try
                {
                    topError = int.Parse(topErrorSolution[0][1]);
                    topSolution = int.Parse(topErrorSolution[0][2]);
                }
                catch
                { }

                List<List<string>> errorsData = gujaczWFS.ExecuteStoredProcedure("GetErrorsForProcess", new string[] { processId.ToString() }, DatabaseName.SupportCP);
                foreach (var a in errorsData)
                {
                    Entities.ProcessError error = new ProcessError();
                    error.id = int.Parse(a[0]);
                    error.idErrorType = int.Parse(a[1]);
                    error.description = a[2];
                    error.descriptionFull = a[3];
                    errors.Add(error);
                }
                List<List<string>> solutionsData = gujaczWFS.ExecuteStoredProcedure("GetSolutions", new string[] { }, DatabaseName.SupportCP);
                foreach (var a in solutionsData)
                {
                    Entities.ProcessSolution solution = new ProcessSolution();
                    solution.id = int.Parse(a[0]);
                    solution.nameCP = a[1];
                    solution.nameBuissness = a[2];
                    solutions.Add(solution);
                }

                procManagerPanel = new Panel();
                procManagerPanel.Name = "panel_ProcessManager";
                procManagerPanel.Size = new Size(222, 160);
                procManagerPanel.Location = new Point(rtb.Left, rtb.Bottom + 10);
                procManagerPanel.Parent = gBox;
                procManagerPanel.Visible = true;
                procManagerPanel.BorderStyle = BorderStyle.FixedSingle;

                Label tBoxLabel = new Label();
                tBoxLabel.Name = "label_idPorcess";
                tBoxLabel.AutoSize = true;
                tBoxLabel.Location = new Point(3, 10);
                tBoxLabel.Text = "id procesu";
                tBoxLabel.Parent = procManagerPanel;
                tb_ProcessId = new TextBox();
                tb_ProcessId.Name = "tb_ProcessId";
                tb_ProcessId.Location = new Point(tBoxLabel.Right + 3, 7);
                tb_ProcessId.Width = procManagerPanel.Width - tb_ProcessId.Left - 6;
                tb_ProcessId.Parent = procManagerPanel;

                Label lbl_ProcessType = new Label();
                lbl_ProcessType.Name = "lbl_ProcessType";
                lbl_ProcessType.AutoSize = true;
                lbl_ProcessType.Location = new Point(3, tb_ProcessId.Bottom + 10);
                lbl_ProcessType.Text = "Typ procesu: " + procType;
                lbl_ProcessType.Parent = procManagerPanel;

                Label lbl_ErrorLabel = new Label();
                lbl_ErrorLabel.Name = "lbl_ErrorLabel";
                lbl_ErrorLabel.AutoSize = true;
                lbl_ErrorLabel.Location = new Point(3, lbl_ProcessType.Bottom + 10);
                lbl_ErrorLabel.Text = "Błąd procesu:";
                lbl_ErrorLabel.Parent = procManagerPanel;
                cb_Error = new ComboBox();
                cb_Error.Name = "cb_Error";
                cb_Error.Location = new Point(3, lbl_ErrorLabel.Bottom + 7);
                cb_Error.Width = procManagerPanel.Width - cb_Error.Left - 6;
                cb_Error.Parent = procManagerPanel;
                cb_Error.AutoCompleteMode = AutoCompleteMode.Suggest;
                cb_Error.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb_Error.DropDownStyle = ComboBoxStyle.DropDownList;
                cb_Error.SelectedIndexChanged += new EventHandler(cb_Error_SelectedIndexChanged);
                foreach (Entities.ProcessError e in errors)
                {
                    cb_Error.Items.Add(e.description);
                }

                Label lbl_SolutionLabel = new Label();
                lbl_SolutionLabel.Name = "lbl_SolutionLabel";
                lbl_SolutionLabel.AutoSize = true;
                lbl_SolutionLabel.Location = new Point(3, cb_Error.Bottom + 10);
                lbl_SolutionLabel.Text = "Rozwiązanie:";
                lbl_SolutionLabel.Parent = procManagerPanel;
                cb_Solution = new ComboBox();
                cb_Solution.Name = "cb_Solution";
                cb_Solution.Location = new Point(3, lbl_SolutionLabel.Bottom + 7);
                cb_Solution.Width = procManagerPanel.Width - cb_Solution.Left - 6;
                cb_Solution.Parent = procManagerPanel;
                cb_Solution.AutoCompleteMode = AutoCompleteMode.Suggest;
                cb_Solution.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb_Solution.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (Entities.ProcessSolution s in solutions)
                {
                    cb_Solution.Items.Add(s.nameCP);
                }

                foreach (string s in cb_Error.Items)
                {
                    string topErrorDescription = errors.Where(x => x.id == topError).First().description;
                    if (s.Equals(topErrorDescription))
                        cb_Error.SelectedItem = s;
                }
            }
        }

        void cb_Error_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox error = sender as ComboBox;

            if (error.SelectedIndex >= 0)
            {
                Entities.ProcessError errorInfo = errors.Where(x => x.description == error.SelectedItem.ToString()).First();
                List<List<string>> topSolution = gujaczWFS.ExecuteStoredProcedure("GetTopErrorAndSolutionForProcess", new string[] { processId.ToString(), errorInfo.id.ToString() }, DatabaseName.SupportCP);
                if (topSolution != null && topSolution.Count > 0)
                    cb_Solution.SelectedItem = solutions.Where(x => x.id == int.Parse(topSolution[0][2])).First().nameCP;
            }
        }

        #endregion

        private void dodatkoweInfo()
        {
            if (list.Where(x => x.EventParamId == 3196).FirstOrDefault() == null)
                return;

            rich[0].Text = "";

            List<List<string>> dane = gujaczWFS.ExecuteStoredProcedure("Billing_DTH_WybranieDodatkowychInformacji", new string[] { dodatkoweDane[0].ToString(), dodatkoweDane[1].ToString(), dodatkoweDane[2].ToString() }, DatabaseName.SupportADDONS);
            foreach (var a in dane)
            {
                rich[0].AppendText(a[0].ToString());
                rich[0].AppendText(" ");
            }

        }

        private void AdjustWidthComboBox_DropDown(object sender, System.EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;

            if (senderComboBox.Items.Count == 0) return;

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
    }


    public class SimpleData
    {
        public int ID { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class EventParamSource
    {
        public Type type;
        public object source;
        public Entities.EventParam param;
        public bool isRequired;

        public Entities.EventParam boundParam;
    }

}
