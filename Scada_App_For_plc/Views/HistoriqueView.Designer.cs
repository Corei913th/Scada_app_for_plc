#nullable disable
namespace Scada_App_For_plc.Views
{
    partial class HistoriqueView
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
            dataGrid = new DataGridView();
            pnlToolbar = new Panel();
            btnRefresh = new Button();
            btnExport = new Button();
            lblMessage = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            //
            // dataGrid
            //
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.BackgroundColor = SystemColors.Window;
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.Location = new Point(0, 52);
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.RowHeadersVisible = false;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.Size = new Size(720, 340);
            dataGrid.TabIndex = 0;
            //
            // pnlToolbar
            //
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnExport);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(720, 52);
            pnlToolbar.TabIndex = 1;
            //
            // btnRefresh
            //
            btnRefresh.Location = new Point(12, 9);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(130, 34);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Actualiser";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            //
            // btnExport
            //
            btnExport.Location = new Point(152, 9);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(170, 34);
            btnExport.TabIndex = 1;
            btnExport.Text = "Exporter vers Excel";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += BtnExport_Click;
            //
            // lblMessage
            //
            lblMessage.Dock = DockStyle.Bottom;
            lblMessage.Location = new Point(0, 392);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new Padding(12, 0, 0, 0);
            lblMessage.Size = new Size(720, 30);
            lblMessage.TabIndex = 2;
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            //
            // HistoriqueView
            //
            Controls.Add(dataGrid);
            Controls.Add(pnlToolbar);
            Controls.Add(lblMessage);
            Name = "HistoriqueView";
            Size = new Size(720, 422);
            VisibleChanged += HistoriqueView_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            pnlToolbar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGrid;
        private Panel pnlToolbar;
        private Button btnRefresh;
        private Button btnExport;
        private Label lblMessage;
    }
}
