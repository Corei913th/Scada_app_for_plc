using System.ComponentModel;
using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>Carte d'un moteur : voyant, état et boutons Avant / Arrière / Arrêt.</summary>
public partial class MotorPanel : UserControl
{
    /// <summary>Levé quand l'opérateur clique Avant / Arrière / Arrêt ; MainView transmet la demande à l'automate.</summary>
    public event Action<MotorDirection>? DirectionRequested;

    public MotorPanel()
    {
        InitializeComponent();
    }

    /// <summary>Titre affiché sur le cadre (« Moteur 1 », …).</summary>
    [Category("Appearance")]
    [DefaultValue("Moteur")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => grpMotor.Text;
        set => grpMotor.Text = value;
    }

    /// <summary>
    /// Affiche l'état lu ; les boutons ne sont actifs que si la machine est en marche.
    /// <paramref name="speed"/> est la vitesse réelle lue dans l'automate (D3100.. ) : elle fait tourner l'hélice.
    /// </summary>
    public void SetState(MotorDirection direction, bool commandsEnabled, int speed)
    {
        // Couleur du voyant et texte selon les relais M avant / arrière lus dans l'automate.
        (led.LedColor, lblState.Text) = direction switch
        {
            MotorDirection.Forward => (Color.LimeGreen, "Marche avant"),
            MotorDirection.Backward => (Color.DodgerBlue, "Marche arrière"),
            MotorDirection.Conflict => (Color.Orange, "Conflit avant/arrière"),
            _ => (Color.Firebrick, "Arrêté")
        };
        SetButtons(commandsEnabled);

        // Hélice : même couleur que le voyant, vitesse = mesure lue, sens = dernier sens demandé.
        // Si le moteur vient d'être coupé, on garde le sens précédent : l'hélice ralentit au lieu de repartir à l'envers.
        if (direction == MotorDirection.Forward) rotor.Clockwise = true;
        else if (direction == MotorDirection.Backward) rotor.Clockwise = false;
        rotor.RingColor = led.LedColor;
        rotor.Speed = speed;
        lblSpeed.Text = $"Vitesse : {speed}";
    }

    /// <summary>État inconnu (automate hors ligne) : voyant gris, hélice immobile, commandes désactivées.</summary>
    public void SetUnknown()
    {
        led.LedColor = Color.Gray;
        lblState.Text = "Hors ligne";
        rotor.RingColor = Color.Gray;
        rotor.Speed = 0;
        lblSpeed.Text = "Vitesse : -";
        SetButtons(false);
    }

    private void SetButtons(bool enabled)
    {
        btnForward.Enabled = enabled;
        btnBackward.Enabled = enabled;
        btnStop.Enabled = enabled;
    }

    private void BtnForward_Click(object? sender, EventArgs e) => DirectionRequested?.Invoke(MotorDirection.Forward);

    private void BtnBackward_Click(object? sender, EventArgs e) => DirectionRequested?.Invoke(MotorDirection.Backward);

    private void BtnStop_Click(object? sender, EventArgs e) => DirectionRequested?.Invoke(MotorDirection.Stopped);
}
