#nullable disable
namespace Scada_App_For_plc.Views
{
    partial class MotorPanel
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
            grpMotor = new GroupBox();
            rotor = new Rotor();
            led = new Led();
            lblState = new Label();
            lblSpeed = new Label();
            btnForward = new Button();
            btnBackward = new Button();
            btnStop = new Button();
            grpMotor.SuspendLayout();
            SuspendLayout();
            //
            // grpMotor
            //
            grpMotor.Controls.Add(rotor);
            grpMotor.Controls.Add(led);
            grpMotor.Controls.Add(lblState);
            grpMotor.Controls.Add(lblSpeed);
            grpMotor.Controls.Add(btnForward);
            grpMotor.Controls.Add(btnBackward);
            grpMotor.Controls.Add(btnStop);
            grpMotor.Dock = DockStyle.Fill;
            grpMotor.Location = new Point(0, 0);
            grpMotor.Name = "grpMotor";
            grpMotor.Size = new Size(380, 140);
            grpMotor.TabIndex = 0;
            grpMotor.TabStop = false;
            grpMotor.Text = "Moteur";
            //
            // rotor
            //
            rotor.Location = new Point(296, 24);
            rotor.Name = "rotor";
            rotor.Size = new Size(76, 76);
            rotor.TabIndex = 5;
            //
            // led
            //
            led.Location = new Point(16, 26);
            led.Name = "led";
            led.Size = new Size(30, 30);
            led.TabIndex = 0;
            //
            // lblState
            //
            lblState.AutoSize = true;
            lblState.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblState.Location = new Point(56, 24);
            lblState.Name = "lblState";
            lblState.Size = new Size(87, 20);
            lblState.TabIndex = 1;
            lblState.Text = "Hors ligne";
            //
            // lblSpeed
            //
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSpeed.ForeColor = Color.DimGray;
            lblSpeed.Location = new Point(56, 50);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(83, 19);
            lblSpeed.TabIndex = 6;
            lblSpeed.Text = "Vitesse : -";
            //
            // btnForward
            //
            btnForward.Enabled = false;
            btnForward.Location = new Point(16, 90);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(88, 34);
            btnForward.TabIndex = 2;
            btnForward.Text = "▶ Avant";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += BtnForward_Click;
            //
            // btnBackward
            //
            btnBackward.Enabled = false;
            btnBackward.Location = new Point(112, 90);
            btnBackward.Name = "btnBackward";
            btnBackward.Size = new Size(88, 34);
            btnBackward.TabIndex = 3;
            btnBackward.Text = "◀ Arrière";
            btnBackward.UseVisualStyleBackColor = true;
            btnBackward.Click += BtnBackward_Click;
            //
            // btnStop
            //
            btnStop.Enabled = false;
            btnStop.Location = new Point(208, 90);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(88, 34);
            btnStop.TabIndex = 4;
            btnStop.Text = "■ Arrêt";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += BtnStop_Click;
            //
            // MotorPanel
            //
            Controls.Add(grpMotor);
            Name = "MotorPanel";
            Size = new Size(380, 140);
            grpMotor.ResumeLayout(false);
            grpMotor.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpMotor;
        private Rotor rotor;
        private Led led;
        private Label lblState;
        private Label lblSpeed;
        private Button btnForward;
        private Button btnBackward;
        private Button btnStop;
    }
}
