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
using Entities.Enums;
using Utility;
using LogicLayer;
using LogicLayer.Interface;

namespace GUI
{
    public partial class Procesy : Form
    {
        private BackgroundWorker backgroundWorker = null;
        public static List<Komponent> listaKomp = new List<Komponent>();
        IParserEngineWFS gujacz2;
        private MainForm parent { get; set; }
        List<Entities.ProcessError> errors;
        List<Entities.ProcessSolution> solutions;
        int processId;
        int[] dodatkoweDane = new int[3] { -1, -1, -1 };

        public Procesy(MainForm form)
        {
            InitializeComponent();
            errors = new List<ProcessError>();
            solutions = new List<ProcessSolution>();
            this.parent = form;
            //cbTmp.SelectedIndex = 0;
            gujacz2 = ServiceLocator.Instance.Retrieve<IParserEngineWFS>();
            LoadComponents();
        }

        private void LoadComponents()
        {
            Dictionary<int, string> kat = gujacz2.getBillingComponents(0);
            dodatkoweDane[0] = kat.Where(x => x.Value == "CRM").FirstOrDefault().Key;

            Dictionary<int, string> rodz = new Dictionary<int, string>();
            rodz = gujacz2.getBillingComponents(kat.Where(x => x.Value == "CRM").FirstOrDefault().Key); //Dictionary<int, string>
            dodatkoweDane[1] = rodz.Where(x => x.Value == "błąd procesu").FirstOrDefault().Key;

            Dictionary<int, string> typy = new Dictionary<int, string>();
            typy = gujacz2.getBillingComponents(rodz.Where(x => x.Value == "błąd procesu").FirstOrDefault().Key);

            foreach (var k in typy)
            {
                cbTypProcesu.Items.Add(new Entities.BillingDthLBItem() { Text = k.Value, Value = k.Key });
            }
            //cbTypProcesu.Items.AddRange(typy.Values.ToArray());

            List<List<string>> solutionsData = gujacz2.ExecuteStoredProcedure("GetSolutions", new string[] { }, DatabaseName.SupportCP);
            foreach (var a in solutionsData)
            {
                Entities.ProcessSolution solution = new ProcessSolution();
                solution.id = int.Parse(a[0]);
                solution.nameCP = a[1];
                solution.nameBuissness = a[2];
                solutions.Add(solution);
            }

            cbRozwiazanie.Items.Clear();
            foreach (Entities.ProcessSolution so in solutions)
            {
                cbRozwiazanie.Items.Add(so.nameCP);
            }

            dodatkoweInfo();
        }

        private void cbTmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Komponent> list = Implementation.GeneralStatic.listaKomp;
            Komponent kk = list.Where(x => x.IdKomponentu == 1).FirstOrDefault();
            list.Clear();
            list.Add(kk);
           
            //cbTmp.DataSource = list;
            cbTmp.DisplayMember = "NazwaKomponentu";
            cbTmp.ValueMember = "IdKomponentu";

            foreach (Komponent kom in list)
            {
                
                if (kom != null && kk.IdKomponentu == kom.IdKomponentu)
                {
                    //cbTypProcesu.DataSource = kom.Diagnozy;
                    cbTypProcesu.DisplayMember = "NazwaDiagnozy";
                    cbTypProcesu.ValueMember = "IdDiagnozy";
                }
            }

            if (cbTypProcesu.Items.Count > 0)
                cbTypProcesu.SelectedIndex = 0;

            if (cbBlad.Items.Count > 0)
                cbBlad.SelectedIndex = 0;
            
            if (cbRozwiazanie.Items.Count > 0)
                cbRozwiazanie.SelectedIndex = 0;
        }

        private void btn_Dodaj_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not wurking yet");

            if (string.IsNullOrWhiteSpace(tb_ProcessId.Text) || cbTypProcesu.SelectedIndex == -1 || cbBlad.SelectedIndex == -1 || cbRozwiazanie.SelectedIndex == -1)
                return;

            string[] processes = tb_ProcessId.Text.Split(',');

            List<nProcess> list = new List<nProcess>();

            foreach (string s in processes)
            {
                nProcess proc = new nProcess()
                {
                    IdProcess = processId,
                    IdError = errors.Where(x => x.description == cbBlad.SelectedItem.ToString()).First().id,
                    IdSolutions = solutions.Where(x => x.nameCP == cbRozwiazanie.SelectedItem.ToString()).First().id,
                    Number = int.Parse(s.Trim()),
                    Comment = "",
                    Date = DateTime.Now,
                    Author = gujacz2.getUser().login,
                    FromTask = false
                };

                list.Add(proc);
            }

            //LogicLayer.Implementation.ProcesManagerImpl procManagerImpl = new LogicLayer.Implementation.ProcesManagerImpl();
            //MessageBox.Show(Logic.Implementation.XmlParser.InsertProcessLogXML(list).ToString());


            gujacz2.InsertNewProcessLog(Logic.Implementation.XmlParser.InsertProcessLogXML(list));
            //procManagerImpl.AddNewProcess(list);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void doByWorker(DoWorkEventHandler work, object argument, RunWorkerCompletedEventHandler reaction)
        {

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += work;
            this.backgroundWorker.RunWorkerCompleted += reaction;
            this.backgroundWorker.RunWorkerAsync(argument);
        }

        private void doGetComponents(object sender, DoWorkEventArgs e)
        {
           // listaKomp = gujacz2.getComponents();

        }

        private void doGetComponentsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //pobrał juz do statycznej listy
        }

        private void cbBlad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processId != -1)
            {
                List<List<string>> solutionsData = gujacz2.ExecuteStoredProcedure("GetSolutions", new string[] { }, DatabaseName.SupportCP);
                foreach (var a in solutionsData)
                {
                    Entities.ProcessSolution solution = new ProcessSolution();
                    solution.id = int.Parse(a[0]);
                    solution.nameCP = a[1];
                    solution.nameBuissness = a[2];
                    solutions.Add(solution);
                }

                Entities.ProcessError errorInfo = errors.Where(x => x.description == cbBlad.SelectedItem.ToString()).First();
                List<List<string>> topSolution = gujacz2.ExecuteStoredProcedure("GetTopErrorAndSolutionForProcess", new string[] { processId.ToString(), errorInfo.id.ToString() }, DatabaseName.SupportCP);

                cbRozwiazanie.Items.Clear();
                foreach (Entities.ProcessSolution so in solutions)
                {
                    cbRozwiazanie.Items.Add(so.nameCP);
                }

                if (topSolution.Count > 0)
                    cbRozwiazanie.SelectedItem = solutions.Where(x => x.id == int.Parse(topSolution[0][2])).FirstOrDefault().nameCP;
            }
        }

        private void cbTypProcesu_SelectedIndexChanged(object sender, EventArgs e)
        {
            processId = -1;
            string procType = cbTypProcesu.SelectedItem.ToString().Split(' ').First();

            List<List<string>> processInfo = gujacz2.ExecuteStoredProcedure("GetProcessByName", new string[] { procType }, DatabaseName.SupportCP);
            if (processInfo != null && processInfo.Count > 0)
            {
                processId = int.Parse(processInfo[0][0]);
            }
            else
                return;

            List<List<string>> topErrorSolution = gujacz2.ExecuteStoredProcedure("GetTopErrorAndSolutionForProcess", new string[] { processId.ToString(), "-1" }, DatabaseName.SupportCP);

            int topError = -1;
            //int topSolution = -1;
            try
            {
                topError = int.Parse(topErrorSolution[0][1]);
                //topSolution = int.Parse(topErrorSolution[0][2]);
            }
            catch
            { }

            List<List<string>> errorsData = gujacz2.ExecuteStoredProcedure("GetErrorsForProcess", new string[] { processId.ToString() }, DatabaseName.SupportCP);
            foreach (var a in errorsData)
            {
                Entities.ProcessError error = new ProcessError();
                error.id = int.Parse(a[0]);
                error.idErrorType = int.Parse(a[1]);
                error.description = a[2];
                error.descriptionFull = a[3];
                errors.Add(error);
            }
            

            cbBlad.Items.Clear();
            foreach (Entities.ProcessError er in errors)
            {
                cbBlad.Items.Add(er.description);
            }

            if (topErrorSolution.Count > 0)
            {
                cbBlad.SelectedItem = errors.Where(x => x.id == int.Parse(topErrorSolution[0][1])).FirstOrDefault().description;

                Entities.ProcessError errorInfo = errors.Where(x => x.description == cbBlad.SelectedItem.ToString()).First();
                List<List<string>> topSolution = gujacz2.ExecuteStoredProcedure("GetTopErrorAndSolutionForProcess", new string[] { processId.ToString(), errorInfo.id.ToString() }, DatabaseName.SupportCP);


                cbRozwiazanie.SelectedItem = solutions.Where(x => x.id == int.Parse(topSolution[0][2])).FirstOrDefault().nameCP;
            }

            dodatkoweDane[2] = ((Entities.BillingDthLBItem)cbTypProcesu.SelectedItem).Value;
            dodatkoweInfo();
        }

        private void dodatkoweInfo()
        {

            richTextBox1.Text = "";

            List<List<string>> dane = gujacz2.ExecuteStoredProcedure("Billing_DTH_WybranieDodatkowychInformacji", new string[] { dodatkoweDane[0].ToString(), dodatkoweDane[1].ToString(), dodatkoweDane[2].ToString() }, DatabaseName.SupportADDONS);
            foreach (var a in dane)
            {
                richTextBox1.AppendText(a[0].ToString());
                richTextBox1.AppendText(" ");
            }

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            UstawieniaProcesow up = new UstawieniaProcesow(this.gujacz2);
            up.ShowDialog();
        }
    }
}

