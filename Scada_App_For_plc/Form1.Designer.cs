#nullable disable
namespace Scada_App_For_plc
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            flpNav = new FlowLayoutPanel();
            btnPrincipal = new Button();
            btnParametres = new Button();
            btnHistorique = new Button();
            btnInfo = new Button();
            lblLink = new Label();
            pnlContent = new Panel();
            mainView1 = new Scada_App_For_plc.Views.MainView();
            parametresView1 = new Scada_App_For_plc.Views.ParametresView();
            historiqueView1 = new Scada_App_For_plc.Views.HistoriqueView();
            infoView1 = new Scada_App_For_plc.Views.InfoView();
            tmrReconnect = new System.Windows.Forms.Timer(components);
            pnlHeader.SuspendLayout();
            flpNav.SuspendLayout();
            pnlContent.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(32, 44, 66);
            pnlHeader.Controls.Add(flpNav);
            pnlHeader.Controls.Add(lblLink);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1000, 52);
            pnlHeader.TabIndex = 1;
            // 
            // flpNav
            // 
            flpNav.BackColor = Color.FromArgb(32, 44, 66);
            flpNav.Controls.Add(btnPrincipal);
            flpNav.Controls.Add(btnParametres);
            flpNav.Controls.Add(btnHistorique);
            flpNav.Controls.Add(btnInfo);
            flpNav.Dock = DockStyle.Fill;
            flpNav.Location = new Point(0, 0);
            flpNav.Name = "flpNav";
            flpNav.Padding = new Padding(8, 8, 0, 0);
            flpNav.Size = new Size(660, 52);
            flpNav.TabIndex = 0;
            // 
            // btnPrincipal
            // 
            btnPrincipal.BackColor = Color.FromArgb(70, 110, 190);
            btnPrincipal.FlatAppearance.BorderSize = 0;
            btnPrincipal.FlatStyle = FlatStyle.Flat;
            btnPrincipal.ForeColor = Color.White;
            btnPrincipal.Location = new Point(11, 11);
            btnPrincipal.Name = "btnPrincipal";
            btnPrincipal.Size = new Size(120, 36);
            btnPrincipal.TabIndex = 0;
            btnPrincipal.Text = "Principal";
            btnPrincipal.UseVisualStyleBackColor = false;
            btnPrincipal.Click += BtnPrincipal_Click;
            // 
            // btnParametres
            // 
            btnParametres.BackColor = Color.FromArgb(52, 66, 94);
            btnParametres.FlatAppearance.BorderSize = 0;
            btnParametres.FlatStyle = FlatStyle.Flat;
            btnParametres.ForeColor = Color.White;
            btnParametres.Location = new Point(137, 11);
            btnParametres.Name = "btnParametres";
            btnParametres.Size = new Size(120, 36);
            btnParametres.TabIndex = 1;
            btnParametres.Text = "Paramètres";
            btnParametres.UseVisualStyleBackColor = false;
            btnParametres.Click += BtnParametres_Click;
            // 
            // btnHistorique
            // 
            btnHistorique.BackColor = Color.FromArgb(52, 66, 94);
            btnHistorique.FlatAppearance.BorderSize = 0;
            btnHistorique.FlatStyle = FlatStyle.Flat;
            btnHistorique.ForeColor = Color.White;
            btnHistorique.Location = new Point(263, 11);
            btnHistorique.Name = "btnHistorique";
            btnHistorique.Size = new Size(120, 36);
            btnHistorique.TabIndex = 2;
            btnHistorique.Text = "Historique";
            btnHistorique.UseVisualStyleBackColor = false;
            btnHistorique.Click += BtnHistorique_Click;
            // 
            // btnInfo
            // 
            btnInfo.BackColor = Color.FromArgb(52, 66, 94);
            btnInfo.FlatAppearance.BorderSize = 0;
            btnInfo.FlatStyle = FlatStyle.Flat;
            btnInfo.ForeColor = Color.White;
            btnInfo.Location = new Point(389, 11);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new Size(120, 36);
            btnInfo.TabIndex = 3;
            btnInfo.Text = "Info";
            btnInfo.UseVisualStyleBackColor = false;
            btnInfo.Click += BtnInfo_Click;
            // 
            // lblLink
            // 
            lblLink.BackColor = Color.FromArgb(32, 44, 66);
            lblLink.Dock = DockStyle.Right;
            lblLink.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLink.ForeColor = Color.Salmon;
            lblLink.Location = new Point(660, 0);
            lblLink.Name = "lblLink";
            lblLink.Padding = new Padding(0, 0, 12, 0);
            lblLink.Size = new Size(340, 52);
            lblLink.TabIndex = 1;
            lblLink.Text = "● Automate hors ligne";
            lblLink.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(mainView1);
            pnlContent.Controls.Add(parametresView1);
            pnlContent.Controls.Add(historiqueView1);
            pnlContent.Controls.Add(infoView1);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 52);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1000, 548);
            pnlContent.TabIndex = 0;
            // 
            // mainView1
            // 
            mainView1.Dock = DockStyle.Fill;
            mainView1.Location = new Point(0, 0);
            mainView1.Name = "mainView1";
            mainView1.Size = new Size(1000, 548);
            mainView1.TabIndex = 0;
            // 
            // parametresView1
            // 
            parametresView1.Dock = DockStyle.Fill;
            parametresView1.Location = new Point(0, 0);
            parametresView1.Name = "parametresView1";
            parametresView1.Size = new Size(1000, 548);
            parametresView1.TabIndex = 1;
            // 
            // historiqueView1
            // 
            historiqueView1.Dock = DockStyle.Fill;
            historiqueView1.Location = new Point(0, 0);
            historiqueView1.Name = "historiqueView1";
            historiqueView1.Size = new Size(1000, 548);
            historiqueView1.TabIndex = 2;
            // 
            // infoView1
            // 
            infoView1.Dock = DockStyle.Fill;
            infoView1.Location = new Point(0, 0);
            infoView1.Name = "infoView1";
            infoView1.Size = new Size(1000, 548);
            infoView1.TabIndex = 3;
            // 
            // tmrReconnect
            // 
            tmrReconnect.Interval = 3000;
            tmrReconnect.Tick += TmrReconnect_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(860, 540);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SCADA - Automate Delta";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            pnlHeader.ResumeLayout(false);
            flpNav.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private FlowLayoutPanel flpNav;
        private Button btnPrincipal;
        private Button btnParametres;
        private Button btnHistorique;
        private Button btnInfo;
        private Label lblLink;
        private Panel pnlContent;
        private Views.MainView mainView1;
        private Views.ParametresView parametresView1;
        private Views.HistoriqueView historiqueView1;
        private Views.InfoView infoView1;
        private System.Windows.Forms.Timer tmrReconnect;
    }
}
