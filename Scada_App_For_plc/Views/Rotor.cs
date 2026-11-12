using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Scada_App_For_plc.Views;

/// <summary>
/// Hélice animée qui tourne à la vitesse réelle du moteur (valeur lue dans D3100..D3102) :
/// plus la vitesse est grande, plus elle tourne vite ; elle change de sens avec Avant / Arrière.
/// Elle continue de tourner en ralentissant tant que la vitesse mesurée n'est pas retombée à 0.
/// </summary>
[DesignerCategory("Code")]
public sealed class Rotor : Control
{
    // Vitesse d'affichage = vitesse mesurée × 0,4 degré par seconde, plafonnée pour rester lisible à l'écran.
    private const double DegreesPerSecondPerUnit = 0.4;
    private const double MaxDegreesPerSecond = 720;

    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 30 };
    private readonly Stopwatch _clock = new();
    private double _angle;
    private double _speed;
    private bool _clockwise = true;
    private Color _ringColor = Color.Gray;

    public Rotor()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Size = new Size(76, 76);
        _timer.Tick += (_, _) => Step();
    }

    /// <summary>Vitesse mesurée du moteur (même unité que D3100..). 0 = hélice immobile.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double Speed
    {
        get => _speed;
        set => _speed = Math.Max(0, value);
    }

    /// <summary>Sens de rotation : vrai = sens horaire (marche avant), faux = sens antihoraire (marche arrière).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Clockwise
    {
        get => _clockwise;
        set => _clockwise = value;
    }

    /// <summary>Couleur du cercle extérieur (vert = avant, bleu = arrière, rouge = arrêté, gris = hors ligne).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color RingColor
    {
        get => _ringColor;
        set
        {
            if (_ringColor == value) return;
            _ringColor = value;
            Invalidate();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        // Dans le concepteur Visual Studio l'hélice reste figée ; l'animation ne démarre qu'à l'exécution.
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            _clock.Restart();
            _timer.Start();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _timer.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>Fait avancer l'angle selon le temps réellement écoulé depuis le dernier pas.</summary>
    private void Step()
    {
        var dt = _clock.Elapsed.TotalSeconds;
        _clock.Restart();
        if (_speed <= 0) return;

        var degrees = Math.Min(_speed * DegreesPerSecondPerUnit, MaxDegreesPerSecond) * dt;
        _angle = (_angle + (_clockwise ? degrees : -degrees)) % 360;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var radius = Math.Min(Width, Height) / 2f - 4;
        var cx = Width / 2f;
        var cy = Height / 2f;

        using (var disc = new SolidBrush(Color.White))
        using (var ring = new Pen(_ringColor, 4))
        {
            g.FillEllipse(disc, cx - radius, cy - radius, radius * 2, radius * 2);
            g.DrawEllipse(ring, cx - radius, cy - radius, radius * 2, radius * 2);
        }

        // Quatre pales identiques, une tous les 90°. La forme est courbée : on voit ainsi le sens de rotation.
        using var blade = new SolidBrush(Color.FromArgb(70, 110, 190));
        var state = g.Save();
        g.TranslateTransform(cx, cy);
        g.RotateTransform((float)_angle);
        var r = radius - 6;
        PointF[] shape =
        [
            new(0, 0),
            new(r * 0.45f, -r * 0.40f),
            new(r * 0.90f, -r * 0.30f),
            new(r * 0.95f, r * 0.05f),
            new(r * 0.45f, r * 0.10f)
        ];
        for (var i = 0; i < 4; i++)
        {
            g.FillClosedCurve(blade, shape, FillMode.Winding, 0.4f);
            g.RotateTransform(90);
        }
        g.Restore(state);

        using var hub = new SolidBrush(Color.FromArgb(40, 50, 70));
        g.FillEllipse(hub, cx - 6, cy - 6, 12, 12);
    }
}
