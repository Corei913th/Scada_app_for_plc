#nullable disable
namespace Scada_App_For_plc.Views
{
    partial class InfoView
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
            lblInfo = new Label();
            lblMessage = new Label();
            SuspendLayout();
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInfo.Location = new Point(24, 20);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(131, 20);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Informations SCADA";
            //
            // lblMessage
            //
            lblMessage.Dock = DockStyle.Bottom;
            lblMessage.Location = new Point(0, 392);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new Padding(12, 0, 0, 0);
            lblMessage.Size = new Size(720, 30);
            lblMessage.TabIndex = 1;
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            //
            // InfoView
            //
            Controls.Add(lblInfo);
            Controls.Add(lblMessage);
            Name = "InfoView";
            Size = new Size(720, 422);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInfo;
        private Label lblMessage;
    }
}
