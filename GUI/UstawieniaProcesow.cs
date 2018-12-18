using LogicLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogicLayer;
using Entities;

namespace GUI
{
    public partial class UstawieniaProcesow : Form
    {
        private GroupBox groupBox;
        private IParserEngineWFS gujacz;
        private List<Control> sources;
        private ActionMode mode;

        public UstawieniaProcesow()
        {
            InitializeComponent();

            sources = new List<Control>();
        }

        public UstawieniaProcesow(IParserEngineWFS gujacz)
            : this()
        {
            this.gujacz = gujacz;
        }

        private void GenerateNewProcessControls(GroupBox container)
        {
            sources = new List<Control>();
            TextBox name = new TextBox();
            Label nameLabel = new Label();
            Button saveButton = new Button();

            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(6, 16);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(31, 13);
            nameLabel.TabIndex = 4;
            nameLabel.Text = "Opis:";
            nameLabel.Parent = container;
            //
            // name
            // 
            name.Location = new System.Drawing.Point(9, nameLabel.Bottom + 3);
            name.Multiline = false;
            name.Name = "name";
            name.Size = new System.Drawing.Size(300, 20);
            name.TabIndex = 1;
            name.Parent = container;
            sources.Add(name);
            //
            // saveButton
            //
            saveButton.Location = new Point(9, name.Bottom + 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(300, 23);
            saveButton.TabIndex = 3;
            saveButton.Parent = container;
            saveButton.Text = "Zapisz";
            saveButton.Click += saveButton_Click;

            container.Height = saveButton.Bottom + 10;
            container.Width = saveButton.Right + 9;
        }

        private void GenerateNewErrorControls(GroupBox container)
        {
            sources = new List<Control>();
            ComboBox errorType = new ComboBox();
            TextBox description = new TextBox();
            TextBox descriptionFull = new TextBox();
            Label errorTypeLabel = new Label();
            Label descriptionLabel = new Label();
            Label descriptionFullLabel = new Label();
            Button saveButton = new Button();

            // 
            // errorTypeLabel
            // 
            errorTypeLabel.AutoSize = true;
            errorTypeLabel.Location = new System.Drawing.Point(6, 16);
            errorTypeLabel.Name = "errorTypeLabel";
            errorTypeLabel.Size = new System.Drawing.Size(59, 13);
            errorTypeLabel.TabIndex = 3;
            errorTypeLabel.Text = "Typ błędu:";
            errorTypeLabel.Parent = container;
            //
            // errorType
            //
            errorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            errorType.FormattingEnabled = true;
            errorType.Location = new System.Drawing.Point(9, errorTypeLabel.Bottom + 3);
            errorType.Name = "errorType";
            errorType.Size = new System.Drawing.Size(300, 21);
            errorType.TabIndex = 0;
            errorType.Parent = container;
            sources.Add(errorType);
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(6, errorType.Bottom + 3);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(31, 13);
            descriptionLabel.TabIndex = 4;
            descriptionLabel.Text = "Opis:";
            descriptionLabel.Parent = container;
            //
            // description
            // 
            description.Location = new System.Drawing.Point(9, descriptionLabel.Bottom + 3);
            description.Multiline = true;
            description.Name = "description";
            description.Size = new System.Drawing.Size(300, 100);
            description.TabIndex = 1;
            description.Parent = container;
            sources.Add(description);
            // 
            // descriptionFullLabel
            //
            descriptionFullLabel.AutoSize = true;
            descriptionFullLabel.Location = new System.Drawing.Point(6, description.Bottom + 3);
            descriptionFullLabel.Name = "descriptionFullLabel";
            descriptionFullLabel.Size = new System.Drawing.Size(60, 13);
            descriptionFullLabel.TabIndex = 5;
            descriptionFullLabel.Text = "Pełny opis:";
            descriptionFullLabel.Parent = container;
            // 
            // descriptionFull
            // 
            descriptionFull.Location = new System.Drawing.Point(9, descriptionFullLabel.Bottom + 3);
            descriptionFull.Multiline = true;
            descriptionFull.Name = "descriptionFull";
            descriptionFull.Size = new System.Drawing.Size(300, 100);
            descriptionFull.TabIndex = 2;
            descriptionFull.Parent = container;
            sources.Add(descriptionFull);
            //
            // saveButton
            //
            saveButton.Location = new Point(9, descriptionFull.Bottom + 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(300, 23);
            saveButton.TabIndex = 3;
            saveButton.Parent = container;
            saveButton.Text = "Zapisz";
            saveButton.Click += saveButton_Click;

            if (this.gujacz != null)
            {
                List<ErrorType> errorTypes = gujacz.GetErrorTypes();

                errorType.Items.AddRange(errorTypes.ToArray());

                if (errorType.Items.Count > 0)
                    errorType.SelectedIndex = 0;
            }

            container.Height = saveButton.Bottom + 10;
            container.Width = saveButton.Right + 9;
        }

        private void GenerateNewSolutionControls(GroupBox container)
        {
            sources = new List<Control>();
            TextBox nameCp = new TextBox();
            TextBox nameBusiness = new TextBox();
            Label nameCpLabel = new Label();
            Label nameBusinessLabel = new Label();
            Button saveButton = new Button();

            // 
            // nameCpLabel
            // 
            nameCpLabel.AutoSize = true;
            nameCpLabel.Location = new System.Drawing.Point(6, 16);
            nameCpLabel.Name = "nameCpLabel";
            nameCpLabel.Size = new System.Drawing.Size(31, 13);
            nameCpLabel.TabIndex = 4;
            nameCpLabel.Text = "Nazwa CP:";
            nameCpLabel.Parent = container;
            //
            // nameCp
            // 
            nameCp.Location = new System.Drawing.Point(9, nameCpLabel.Bottom + 3);
            nameCp.Multiline = false;
            nameCp.Name = "nameCp";
            nameCp.Size = new System.Drawing.Size(300, 20);
            nameCp.TabIndex = 1;
            nameCp.Parent = container;
            sources.Add(nameCp);
            // 
            // nameBusinessLabel
            // 
            nameBusinessLabel.AutoSize = true;
            nameBusinessLabel.Location = new System.Drawing.Point(6, nameCp.Bottom + 3);
            nameBusinessLabel.Name = "nameBusinessLabel";
            nameBusinessLabel.Size = new System.Drawing.Size(31, 13);
            nameBusinessLabel.TabIndex = 5;
            nameBusinessLabel.Text = "Nazwa biznes:";
            nameBusinessLabel.Parent = container;
            //
            // nameBusiness
            // 
            nameBusiness.Location = new System.Drawing.Point(9, nameBusinessLabel.Bottom + 3);
            nameBusiness.Multiline = false;
            nameBusiness.Name = "nameBusiness";
            nameBusiness.Size = new System.Drawing.Size(300, 20);
            nameBusiness.TabIndex = 2;
            nameBusiness.Parent = container;
            sources.Add(nameBusiness);
            //
            // saveButton
            //
            saveButton.Location = new Point(9, nameBusiness.Bottom + 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(300, 23);
            saveButton.TabIndex = 3;
            saveButton.Parent = container;
            saveButton.Text = "Zapisz";
            saveButton.Click += saveButton_Click;

            container.Height = saveButton.Bottom + 10;
            container.Width = saveButton.Right + 9;
        }

        private void GenerateBoundErrorControls(GroupBox container)
        {
            sources = new List<Control>();
            Label processTypeLabel = new Label();
            ComboBox processType = new ComboBox();
            Label errorTypeLabel = new Label();
            ComboBox errorType = new ComboBox();
            CheckBox addRemove = new CheckBox();
            Button saveButton = new Button();

            // 
            // procesLabel
            // 
            processTypeLabel.AutoSize = true;
            processTypeLabel.Location = new System.Drawing.Point(6, 16);
            processTypeLabel.Name = "procesLabel";
            processTypeLabel.Size = new System.Drawing.Size(31, 13);
            processTypeLabel.TabIndex = 4;
            processTypeLabel.Text = "Proces:";
            processTypeLabel.Parent = container;
            //
            // processType
            //
            processType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            processType.FormattingEnabled = true;
            processType.Location = new System.Drawing.Point(9, processTypeLabel.Bottom + 3);
            processType.Name = "processType";
            processType.Size = new System.Drawing.Size(300, 21);
            processType.TabIndex = 0;
            processType.Parent = container;
            sources.Add(processType);
            // 
            // errorTypeLabel
            // 
            errorTypeLabel.AutoSize = true;
            errorTypeLabel.Location = new System.Drawing.Point(6, processType.Bottom + 3);
            errorTypeLabel.Name = "errorTypeLabel";
            errorTypeLabel.Size = new System.Drawing.Size(31, 13);
            errorTypeLabel.TabIndex = 4;
            errorTypeLabel.Text = "Błąd:";
            errorTypeLabel.Parent = container;
            //
            // errorType
            //
            errorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            errorType.FormattingEnabled = true;
            errorType.Location = new System.Drawing.Point(9, errorTypeLabel.Bottom + 3);
            errorType.Name = "errorType";
            errorType.Size = new System.Drawing.Size(300, 21);
            errorType.TabIndex = 1;
            errorType.Parent = container;
            sources.Add(errorType);
            //
            // addRemove
            //
            addRemove.Location = new Point(6, errorType.Bottom + 3);
            addRemove.Text = "Czy usunąć?";
            addRemove.Name = "addRemove";
            addRemove.TabIndex = 2;
            addRemove.Parent = container;
            addRemove.Checked = false;
            sources.Add(addRemove);
            //
            // saveButton
            //
            saveButton.Location = new Point(9, addRemove.Bottom + 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(300, 23);
            saveButton.TabIndex = 3;
            saveButton.Parent = container;
            saveButton.Text = "Zapisz";
            saveButton.Click += saveButton_Click;

            if (this.gujacz != null)
            {
                List<Process> processes = gujacz.GetProcesses();
                List<Entities.Error> errors = gujacz.GetErrors();

                errorType.Items.AddRange(errors.ToArray());
                processType.Items.AddRange(processes.ToArray());

                if (errorType.Items.Count > 0)
                    errorType.SelectedIndex = 0;

                if (processType.Items.Count > 0)
                    processType.SelectedIndex = 0;
            }


            container.Height = saveButton.Bottom + 10;
            container.Width = saveButton.Right + 9;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            switch(mode)
            {
                case ActionMode.NewProcess:
                    {
                        foreach (var item in sources)
                        {
                            TextBox t = item as TextBox;
                            try
                            {
                                if (t.Name == "name" && !string.IsNullOrEmpty(t.Text))
                                {
                                    Process p = new Process()
                                    {
                                        Name = t.Text
                                    };

                                    gujacz.CreateNewProcess(p);
                                    NoticeForm.ShowNotice("Dodano poprawnie!");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                    NoticeForm.ShowNotice(ex.InnerException.Message);
                                else
                                    NoticeForm.ShowNotice(ex.Message);
                            }
                        }
                        break;
                    }
                case ActionMode.NewError:
                    {
                        int errorTypeId = -1;
                        string description = string.Empty;
                        string descriptionFull = string.Empty;

                        foreach (var item in sources)
                        {
                            if (item is ComboBox)
                            {
                                errorTypeId = ((item as ComboBox).SelectedItem as Entities.ErrorType).Id;
                            }
                            else if (item is TextBox)
                            {
                                if (item.Name.Equals("description", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    description = item.Text;
                                }
                                else if (item.Name.Equals("descriptionFull", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    descriptionFull = item.Text;
                                }
                            }
                        }

                        if (errorTypeId != -1 && !string.IsNullOrEmpty(description))
                        {
                            try
                            {
                                Entities.Error error = new Entities.Error()
                                {
                                    ErrorTypeId = errorTypeId,
                                    Description = description,
                                    DescriptionFull = descriptionFull
                                };

                                gujacz.CreateNewError(error);
                                NoticeForm.ShowNotice("Dodano poprawnie!");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                    NoticeForm.ShowNotice(ex.InnerException.Message);
                                else
                                    NoticeForm.ShowNotice(ex.Message);
                            }
                        }
                        else
                        {
                            NoticeForm.ShowNotice("Nie uzupełniono wszystkich wymaganych pól!");
                        }

                        break;
                    }
                case ActionMode.NewSolution:
                    {
                        string nameCP = string.Empty;
                        string nameBusiness = string.Empty;

                        foreach (var item in sources)
                        {
                            if (item is TextBox)
                            {
                                TextBox tex = item as TextBox;

                                if (tex.Name.Equals("nameCp", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    nameCP = tex.Text;
                                }
                                else if (tex.Name.Equals("nameBusiness", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    nameBusiness = tex.Text;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(nameCP) && !string.IsNullOrEmpty(nameBusiness))
                        {
                            try
                            {
                                Solution solution = new Solution()
                                {
                                    NameCP = nameCP,
                                    NameBussiness = nameBusiness
                                };

                                gujacz.CreateNewSolution(solution);
                                NoticeForm.ShowNotice("Dodano poprawnie!");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                    NoticeForm.ShowNotice(ex.InnerException.Message);
                                else
                                    NoticeForm.ShowNotice(ex.Message);
                            }
                        }
                        else
                        {
                            NoticeForm.ShowNotice("Nie uzupełniono wszystkich wymaganych pól!");
                        }

                        break;
                    }
                case ActionMode.Bound:
                    {
                        int processId = -1;
                        int errorId = -1;

                        Process process = null;
                        Entities.Error error = null;

                        bool remove = false;

                        foreach (var item in sources)
                        {
                            if (item is ComboBox)
                            {
                                ComboBox combo = item as ComboBox;

                                if (combo.Name.Equals("processType", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    process = (combo.SelectedItem as Process);
                                }
                                else if (combo.Name.Equals("errorType", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    error = combo.SelectedItem as Entities.Error;
                                }
                            }
                            else if (item is CheckBox)
                            {
                                remove = (item as CheckBox).Checked;
                            }
                        }

                        if (process != null && error != null)
                        {
                            try
                            {
                                gujacz.BoundErrorWithProcess(process, new List<Entities.Error> { error }, remove);
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                    NoticeForm.ShowNotice(ex.InnerException.Message);
                                else
                                    NoticeForm.ShowNotice(ex.Message);
                            }
                        }
                        else
                        {
                            NoticeForm.ShowNotice("Nie uzupełniono wszystkich wymaganych pól!");
                        }

                        break;
                    }
            }

            rbNewProcess_CheckedChanged(null, EventArgs.Empty);
        }

        private void UstawieniaProcesow_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;
        }

        private void rbNewProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (groupBox != null)
                groupBox.Dispose();

            if (rbNewProcess.Checked)
            {
                mode = ActionMode.NewProcess;
                groupBox = new GroupBox();
                groupBox.Text = "Nowy proces";
                groupBox.Location = new Point(this.groupBox1.Right + 3, this.groupBox1.Top);
                groupBox.Size = new Size(300, 300);
                groupBox.Parent = this;
                GenerateNewProcessControls(groupBox);
            }
            else if (rbNewError.Checked)
            {
                mode = ActionMode.NewError;
                groupBox = new GroupBox();
                groupBox.Text = "Nowy błąd";
                groupBox.Location = new Point(this.groupBox1.Right + 3, this.groupBox1.Top);
                groupBox.Size = new Size(300, 300);
                groupBox.Parent = this;
                GenerateNewErrorControls(groupBox);
            }
            else if (rbNewSolution.Checked)
            {
                mode = ActionMode.NewSolution;
                groupBox = new GroupBox();
                groupBox.Text = "Nowe rozwiązanie";
                groupBox.Location = new Point(this.groupBox1.Right + 3, this.groupBox1.Top);
                groupBox.Size = new Size(300, 300);
                groupBox.Parent = this;
                GenerateNewSolutionControls(groupBox);
            }
            else if (rbBounds.Checked)
            {
                mode = ActionMode.Bound;
                groupBox = new GroupBox();
                groupBox.Text = "Powiązanie";
                groupBox.Location = new Point(this.groupBox1.Right + 3, this.groupBox1.Top);
                groupBox.Size = new Size(300, 300);
                groupBox.Parent = this;
                GenerateBoundErrorControls(groupBox);
            }
        }
    }

    enum ActionMode
    {
        NewProcess,
        NewError,
        NewSolution,
        Bound
    }
}
