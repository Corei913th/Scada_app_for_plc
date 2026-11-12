#nullable disable
namespace Scada_App_For_plc.Views
{
    partial class ParametresView
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> Clean up any resources being used. </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblSetHeader = new Label();
            lblFeedHeader = new Label();
            lblMotor1 = new Label();
            numSetpoint1 = new NumericUpDown();
            lblFeedback1 = new Label();
            lblMotor2 = new Label();
            numSetpoint2 = new NumericUpDown();
            lblFeedback2 = new Label();
            lblMotor3 = new Label();
            numSetpoint3 = new NumericUpDown();
            lblFeedback3 = new Label();
            btnApply = new Button();
            btnReload = new Button();
            btnSave = new Button();
            lblMessage = new Label();
            tmrFeedback = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)numSetpoint1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSetpoint2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSetpoint3).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(24, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(306, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Paramètres de vitesse des moteurs";
            // 
            // lblSetHeader
            // 
            lblSetHeader.AutoSize = true;
            lblSetHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSetHeader.Location = new Point(150, 62);
            lblSetHeader.Name = "lblSetHeader";
            lblSetHeader.Size = new Size(101, 15);
            lblSetHeader.TabIndex = 1;
            lblSetHeader.Text = "Consigne (écrite)";
            // 
            // lblFeedHeader
            // 
            lblFeedHeader.AutoSize = true;
            lblFeedHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFeedHeader.Location = new Point(320, 62);
            lblFeedHeader.Name = "lblFeedHeader";
            lblFeedHeader.Size = new Size(77, 15);
            lblFeedHeader.TabIndex = 2;
            lblFeedHeader.Text = "Mesure (lue)";
            // 
            // lblMotor1
            // 
            lblMotor1.AutoSize = true;
            lblMotor1.Location = new Point(24, 100);
            lblMotor1.Name = "lblMotor1";
            lblMotor1.Size = new Size(55, 15);
            lblMotor1.TabIndex = 3;
            lblMotor1.Text = "Moteur 1";
            // 
            // numSetpoint1
            // 
            numSetpoint1.Location = new Point(150, 96);
            numSetpoint1.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSetpoint1.Name = "numSetpoint1";
            numSetpoint1.Size = new Size(120, 23);
            numSetpoint1.TabIndex = 4;
            // 
            // lblFeedback1
            // 
            lblFeedback1.BackColor = Color.WhiteSmoke;
            lblFeedback1.BorderStyle = BorderStyle.FixedSingle;
            lblFeedback1.Location = new Point(320, 98);
            lblFeedback1.Name = "lblFeedback1";
            lblFeedback1.Size = new Size(120, 27);
            lblFeedback1.TabIndex = 5;
            lblFeedback1.Text = "-";
            lblFeedback1.TextAlign = ContentAlignment.MiddleCenter;
            lblFeedback1.Click += lblFeedback1_Click;
            // 
            // lblMotor2
            // 
            lblMotor2.AutoSize = true;
            lblMotor2.Location = new Point(24, 144);
            lblMotor2.Name = "lblMotor2";
            lblMotor2.Size = new Size(55, 15);
            lblMotor2.TabIndex = 6;
            lblMotor2.Text = "Moteur 2";
            // 
            // numSetpoint2
            // 
            numSetpoint2.Location = new Point(150, 140);
            numSetpoint2.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSetpoint2.Name = "numSetpoint2";
            numSetpoint2.Size = new Size(120, 23);
            numSetpoint2.TabIndex = 7;
            // 
            // lblFeedback2
            // 
            lblFeedback2.BackColor = Color.WhiteSmoke;
            lblFeedback2.BorderStyle = BorderStyle.FixedSingle;
            lblFeedback2.Location = new Point(320, 142);
            lblFeedback2.Name = "lblFeedback2";
            lblFeedback2.Size = new Size(120, 27);
            lblFeedback2.TabIndex = 8;
            lblFeedback2.Text = "-";
            lblFeedback2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMotor3
            // 
            lblMotor3.AutoSize = true;
            lblMotor3.Location = new Point(24, 188);
            lblMotor3.Name = "lblMotor3";
            lblMotor3.Size = new Size(55, 15);
            lblMotor3.TabIndex = 9;
            lblMotor3.Text = "Moteur 3";
            // 
            // numSetpoint3
            // 
            numSetpoint3.Location = new Point(150, 184);
            numSetpoint3.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSetpoint3.Name = "numSetpoint3";
            numSetpoint3.Size = new Size(120, 23);
            numSetpoint3.TabIndex = 10;
            // 
            // lblFeedback3
            // 
            lblFeedback3.BackColor = Color.WhiteSmoke;
            lblFeedback3.BorderStyle = BorderStyle.FixedSingle;
            lblFeedback3.Location = new Point(320, 186);
            lblFeedback3.Name = "lblFeedback3";
            lblFeedback3.Size = new Size(120, 27);
            lblFeedback3.TabIndex = 11;
            lblFeedback3.Text = "-";
            lblFeedback3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(24, 244);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(190, 38);
            btnApply.TabIndex = 12;
            btnApply.Text = "Appliquer les consignes";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += BtnApply_Click;
            // 
            // btnReload
            // 
            btnReload.Location = new Point(224, 244);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(160, 38);
            btnReload.TabIndex = 13;
            btnReload.Text = "Relire l'automate";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += BtnReload_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(394, 244);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(170, 38);
            btnSave.TabIndex = 14;
            btnSave.Text = "Enregistrer en base";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // lblMessage
            // 
            lblMessage.Dock = DockStyle.Bottom;
            lblMessage.Location = new Point(0, 392);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new Padding(12, 0, 0, 0);
            lblMessage.Size = new Size(720, 30);
            lblMessage.TabIndex = 15;
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tmrFeedback
            // 
            tmrFeedback.Interval = 500;
            tmrFeedback.Tick += TmrFeedback_Tick;
            // 
            // ParametresView
            // 
            Controls.Add(lblTitle);
            Controls.Add(lblSetHeader);
            Controls.Add(lblFeedHeader);
            Controls.Add(lblMotor1);
            Controls.Add(numSetpoint1);
            Controls.Add(lblFeedback1);
            Controls.Add(lblMotor2);
            Controls.Add(numSetpoint2);
            Controls.Add(lblFeedback2);
            Controls.Add(lblMotor3);
            Controls.Add(numSetpoint3);
            Controls.Add(lblFeedback3);
            Controls.Add(btnApply);
            Controls.Add(btnReload);
            Controls.Add(btnSave);
            Controls.Add(lblMessage);
            Name = "ParametresView";
            Size = new Size(720, 422);
            VisibleChanged += ParametresView_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)numSetpoint1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSetpoint2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSetpoint3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSetHeader;
        private Label lblFeedHeader;
        private Label lblMotor1;
        private NumericUpDown numSetpoint1;
        private Label lblFeedback1;
        private Label lblMotor2;
        private NumericUpDown numSetpoint2;
        private Label lblFeedback2;
        private Label lblMotor3;
        private NumericUpDown numSetpoint3;
        private Label lblFeedback3;
        private Button btnApply;
        private Button btnReload;
        private Button btnSave;
        private Label lblMessage;
        private System.Windows.Forms.Timer tmrFeedback;
    }
}
