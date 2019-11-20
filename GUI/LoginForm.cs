using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogicLayer.Interface;
using System.Threading;
using Utility;
using Logging;
using LogicLayer;
namespace GUI
{
    public partial class Login : Form
    {
        private IParserEngineWFS wfs= null;
        public String login = "";
        public String haslo="";
        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private MaskedTextBox maskedTextBox1;
        private Label label2;
        private Button button2;
        public bool loginSuccess = false;
        private TextBox textBox3;
        private MaskedTextBox maskedTextBox3;
        private Label label5;
        private Label label6;
        private Entities.JiraLoggedUser jiraU;
        private Panel panel1;
        private CheckBox checkBox1;
        private Entities.HeliosLoggedInUser helU;
        private Label label3;
        private MaskedTextBox maskedTextBox2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private bool firstLogin;
        MainForm fm = new MainForm();

        private Login()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "yyyy-MM-dd")
            {
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
//                MessageBox.Show(@"Ustaw format daty na yyyy-MM-dd!
//Aby zmienić format daty w systemie: 
//1. Otwórz Panel sterowania (widok wg.: małe ikony) i kliknij Język.
//2. Kliknij Zmień format daty, godziny lub liczb.
//3. Kliknij Ustawienia dodatkowe, a następnie przejdź do zakładki Data.
//4. W polu data krótka, wpisz yyyy-MM-dd
//6. Gotowe
//", "Błędny format daty!",MessageBoxButtons.OK,MessageBoxIcon.Error);

//                this.Close();
            }
            else
            {
                InitializeComponent();
                //if(firstLogin)
                //    invokeLogin();
            }
        }
        public Login(IParserEngineWFS wfs, ref Entities.HeliosLoggedInUser helU, ref Entities.JiraLoggedUser jiraU, bool firstLogin):this()
        {
            this.helU = helU;
            this.wfs = wfs;
            this.jiraU = jiraU;
            this.firstLogin = firstLogin;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.helU.Login = "billennium";
            this.helU.Password = "billennium1";
            this.jiraU.Login = this.textBox3.Text;
            this.jiraU.Password = this.maskedTextBox3.Text;
            this.login = this.textBox1.Text;
            this.haslo = this.maskedTextBox1.Text;

            if (checkBox1.Checked)
            {
                Properties.Settings.Default.loginWFS = this.login;
                Properties.Settings.Default.hasloWFS = this.haslo;
                Properties.Settings.Default.loginJira = this.jiraU.Login;
                Properties.Settings.Default.hasloJira = this.jiraU.Password;
                Properties.Settings.Default.hasloBillennium = this.maskedTextBox2.Text;
                Properties.Settings.Default.Save();
            }

            //progressBar1.Visible = true;
            //progressBar1.Show();
            //label3.Show();
            //if(!firstLogin)

            invokeLogin((this.Tag.ToString()));

            if (wfs.getUser().Id == -1)
            {
                MessageBox.Show("Brak możliwości zalogowania do BPM \n" +
                    "popraw dane lub sprawdź dostępność BPM.","PROBLEMY Z LOGOWANIEM",MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                wfs = null;
                this.Close();
            }
        }

        private void invokeLogin(string message = "Logowanie...")
        {
            WaitingForm.InvokeWithWaitingForm(message, (Action)delegate ()
            {
                try
                {
                    loginSuccess = wfs.loginToWFSWithUserInfo(login, haslo);
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogError(ex, Logger.Instance, true);
                    loginSuccess = false;
                }
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            //Application.Exit();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.maskedTextBox3 = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(110, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 34);
            this.button1.TabIndex = 6;
            this.button1.Text = "Zaloguj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(27, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login BPM:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.Location = new System.Drawing.Point(110, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(209, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBox1_KeyPress);
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.maskedTextBox1.Location = new System.Drawing.Point(110, 40);
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(209, 22);
            this.maskedTextBox1.TabIndex = 3;
            this.maskedTextBox1.UseSystemPasswordChar = true;
            this.maskedTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBox1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(21, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hasło BPM:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(224, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 34);
            this.button2.TabIndex = 7;
            this.button2.Text = "Anuluj";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(110, 74);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(209, 20);
            this.textBox3.TabIndex = 2;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // maskedTextBox3
            // 
            this.maskedTextBox3.Location = new System.Drawing.Point(110, 100);
            this.maskedTextBox3.Name = "maskedTextBox3";
            this.maskedTextBox3.Size = new System.Drawing.Size(209, 20);
            this.maskedTextBox3.TabIndex = 4;
            this.maskedTextBox3.UseSystemPasswordChar = true;
            this.maskedTextBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBox1_KeyPress);
            this.maskedTextBox3.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBox3_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(35, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Login Jira:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(29, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Hasło Jira:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(2, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(24, 46);
            this.panel1.TabIndex = 10;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(110, 197);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(131, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Zapamiętaj ustawienia";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(-1, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Hasło Billennium";
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Location = new System.Drawing.Point(110, 126);
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(209, 20);
            this.maskedTextBox2.TabIndex = 5;
            this.maskedTextBox2.UseSystemPasswordChar = true;
            this.maskedTextBox2.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBox2_Validating);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GUI.Properties.Resources.ok;
            this.pictureBox1.Location = new System.Drawing.Point(325, 100);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GUI.Properties.Resources.ok;
            this.pictureBox2.Location = new System.Drawing.Point(325, 127);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // Login
            // 
            this.ClientSize = new System.Drawing.Size(352, 218);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maskedTextBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.maskedTextBox3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Tag = "Logowanie....";
            this.Text = "Logowanie";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(null, new EventArgs());
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "";
            //textBox2.Text = "";
            //maskedTextBox1.Text = "";
            //maskedTextBox2.Text = "";            
            //Properties.Settings.Default.Project = cb_Projekt.SelectedIndex;
            if (Properties.Settings.Default.loginWFS != null)
            {
                textBox1.Text = Properties.Settings.Default.loginWFS;
                checkBox1.Checked = true;
            }
            if (Properties.Settings.Default.hasloWFS != null)
            {
                maskedTextBox1.Text = Properties.Settings.Default.hasloWFS;
                checkBox1.Checked = true;
            }
            if (Properties.Settings.Default.loginJira != null)
            {
                textBox3.Text = Properties.Settings.Default.loginJira;
                checkBox1.Checked = true;
            }
            if (Properties.Settings.Default.hasloJira != null)
            {
                maskedTextBox3.Text = Properties.Settings.Default.hasloJira;

                if (textBox1.Text != string.Empty && fm.tryLogginToJira(textBox1.Text, maskedTextBox3.Text))
                {
                    pictureBox1.Visible = true;
                }
                checkBox1.Checked = true;
            }

            if (Properties.Settings.Default.hasloBillennium != null)
            {
                maskedTextBox2.Text = Properties.Settings.Default.hasloBillennium;

                if(fm.tryLogginToJira("billennium", maskedTextBox2.Text))
                {
                    pictureBox2.Visible = true;
                }

                checkBox1.Checked = true;
            }

            if(pictureBox2.Visible && pictureBox1.Visible && firstLogin)
            {
                this.Tag = "Logowanie zapisanymi danymi użytkownika";
                button1_Click(this,null);
            }

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //helU.Login = "Billennium";
            //helU.Password = "billennium1";
            //jiraU.Login = textBox3.Text = "Billennium";
            //jiraU.Password = maskedTextBox3.Text = "Chcedodomu1";
            //login = textBox1.Text = "dlipski";
            //haslo = maskedTextBox1.Text = "7ygv*UHB";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = textBox1.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            

        }
 
        private void maskedTextBox3_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text != string.Empty && fm.tryLogginToJira(textBox1.Text, maskedTextBox3.Text))
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;

            }
        }

        private void maskedTextBox2_Validating(object sender, CancelEventArgs e)
        {
            if (fm.tryLogginToJira("billennium", maskedTextBox2.Text))
            {
                pictureBox2.Visible = true;
            }
            else
            {
                pictureBox2.Visible = false;
                
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
