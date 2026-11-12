using System.ComponentModel;
using Scada_App_For_plc.Services;
using Scada_App_For_plc.Views;

namespace Scada_App_For_plc
{
    /// <summary>
    /// Fenêtre principale : navigation entre les vues et état de la liaison automate.
    /// La mise en page (titre, en-tête, boutons, vues) est dans Form1.Designer.cs / le concepteur.
    /// </summary>
    public partial class Form1 : Form
    {
        // Services partagés : paramètres, liaison Modbus avec l'automate, accès PostgreSQL.
        private readonly AppServices? _svc;

        // Associe chaque bouton de navigation à sa vue (même ordre : Principal, Paramètres, Historique, Info).
        private readonly (Button Button, ViewBase View)[] _pages = [];

        public Form1()
        {
            InitializeComponent();

            // DesignMode est toujours faux dans un constructeur : on teste l'usage de licence.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            _svc = AppServices.Create();

            _pages =
            [
                (btnPrincipal, mainView1),
                (btnParametres, parametresView1),
                (btnHistorique, historiqueView1),
                (btnInfo, infoView1)
            ];
            // Chaque vue reçoit les mêmes services : elles partagent donc une seule connexion à l'automate.
            foreach (var (_, view) in _pages)
                view.Initialize(_svc);

            ShowPage(0);

            // Dès que la liaison s'ouvre ou tombe, le voyant « Automate connecté / hors ligne » se met à jour.
            _svc.Plc.ConnectionChanged += UpdateLink;
        }

        // ------------------------------------------------------------ navigation

        private void BtnPrincipal_Click(object? sender, EventArgs e) => ShowPage(0);

        private void BtnParametres_Click(object? sender, EventArgs e) => ShowPage(1);

        private void BtnHistorique_Click(object? sender, EventArgs e) => ShowPage(2);

        private void BtnInfo_Click(object? sender, EventArgs e) => ShowPage(3);

        private void ShowPage(int index)
        {
            for (var i = 0; i < _pages.Length; i++)
            {
                var selected = i == index;
                _pages[i].View.Visible = selected;
                _pages[i].Button.BackColor = selected ? Color.FromArgb(70, 110, 190) : Color.FromArgb(52, 66, 94);
            }
            _pages[index].View.BringToFront();
        }

        // -------------------------------------------------------- liaison automate

        // Fenêtre affichée : première tentative de connexion à l'automate, puis reconnexion automatique toutes les 3 s.
        private async void Form1_Shown(object? sender, EventArgs e)
        {
            UpdateLink();
            await TryConnectAsync();
            tmrReconnect.Start();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            tmrReconnect.Stop();
            _svc?.Dispose();
        }

        private async void TmrReconnect_Tick(object? sender, EventArgs e) => await TryConnectAsync();

        private async Task TryConnectAsync()
        {
            if (_svc is null || _svc.Plc.IsConnected)
                return;
            try
            {
                await _svc.Plc.ConnectAsync();
            }
            catch
            {
                // Automate injoignable : on réessaiera au prochain tick.
            }
            UpdateLink();
        }

        private void UpdateLink()
        {
            if (_svc is null) return;
            var p = _svc.Settings.Plc;
            if (_svc.Plc.IsConnected)
            {
                lblLink.Text = $"● Automate connecté ({p.Ip}:{p.Port})";
                lblLink.ForeColor = Color.LightGreen;
            }
            else
            {
                lblLink.Text = $"● Automate hors ligne ({p.Ip}:{p.Port})";
                lblLink.ForeColor = Color.Salmon;
            }
        }
    }
}
