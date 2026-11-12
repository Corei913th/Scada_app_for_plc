#nullable disable
namespace Scada_App_For_plc.Views
{
    partial class MainView
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
            btnStart = new Button();
            btnStopAll = new Button();
            lblMachine = new Label();
            motorPanel1 = new MotorPanel();
            motorPanel2 = new MotorPanel();
            motorPanel3 = new MotorPanel();
            lblMessage = new Label();
            tmrPoll = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            //
            // btnStart
            //
            btnStart.BackColor = Color.LightGreen;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Location = new Point(24, 24);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(150, 44);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶  DÉMARRER";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += BtnStart_Click;
            //
            // btnStopAll
            //
            btnStopAll.BackColor = Color.LightCoral;
            btnStopAll.FlatStyle = FlatStyle.Flat;
            btnStopAll.Location = new Point(184, 24);
            btnStopAll.Name = "btnStopAll";
            btnStopAll.Size = new Size(150, 44);
            btnStopAll.TabIndex = 1;
            btnStopAll.Text = "■  ARRÊTER";
            btnStopAll.UseVisualStyleBackColor = false;
            btnStopAll.Click += BtnStopAll_Click;
            //
            // lblMachine
            //
            lblMachine.AutoSize = true;
            lblMachine.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMachine.ForeColor = Color.Gray;
            lblMachine.Location = new Point(360, 34);
            lblMachine.Name = "lblMachine";
            lblMachine.Size = new Size(309, 25);
            lblMachine.TabIndex = 2;
            lblMachine.Text = "Machine : automate non connecté";
            //
            // motorPanel1
            //
            motorPanel1.Location = new Point(24, 96);
            motorPanel1.Name = "motorPanel1";
            motorPanel1.Size = new Size(380, 140);
            motorPanel1.TabIndex = 3;
            motorPanel1.Title = "Moteur 1";
            //
            // motorPanel2
            //
            motorPanel2.Location = new Point(420, 96);
            motorPanel2.Name = "motorPanel2";
            motorPanel2.Size = new Size(380, 140);
            motorPanel2.TabIndex = 4;
            motorPanel2.Title = "Moteur 2";
            //
            // motorPanel3
            //
            motorPanel3.Location = new Point(24, 252);
            motorPanel3.Name = "motorPanel3";
            motorPanel3.Size = new Size(380, 140);
            motorPanel3.TabIndex = 5;
            motorPanel3.Title = "Moteur 3";
            //
            // lblMessage
            //
            lblMessage.Dock = DockStyle.Bottom;
            lblMessage.Location = new Point(0, 392);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new Padding(12, 0, 0, 0);
            lblMessage.Size = new Size(720, 30);
            lblMessage.TabIndex = 6;
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            //
            // tmrPoll
            //
            tmrPoll.Interval = 100;
            tmrPoll.Tick += TmrPoll_Tick;
            //
            // MainView
            //
            Controls.Add(btnStart);
            Controls.Add(btnStopAll);
            Controls.Add(lblMachine);
            Controls.Add(motorPanel1);
            Controls.Add(motorPanel2);
            Controls.Add(motorPanel3);
            Controls.Add(lblMessage);
            Name = "MainView";
            Size = new Size(720, 422);
            VisibleChanged += MainView_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private Button btnStopAll;
        private Label lblMachine;
        private MotorPanel motorPanel1;
        private MotorPanel motorPanel2;
        private MotorPanel motorPanel3;
        private Label lblMessage;
        private System.Windows.Forms.Timer tmrPoll;
    }
}
