using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Scada_App_For_plc.Views;

/// <summary>
/// Hélice animée qui tourne à la vitesse réelle du moteur (valeur lue dans D3100..D3102) :
/// plus la vitesse est grande, plus elle tourne vite ; elle change de sens avec Avant / Arrière.
/// Elle continue de tourner en ralentissant tant que la vitesse mesurée n'est pas retombée à 0.
/// Des traits de flux d'air défilent autour d'elle pendant qu'elle tourne, dans le sens
/// correspondant (vers l'extérieur en marche avant, vers l'intérieur en marche arrière) :
/// l'ensemble moteur + hélice représente un ventilateur, pas une simple pièce qui tourne.
/// </summary>
[DesignerCategory("Code")]
public sealed class Rotor : Control
{
    // Vitesse d'affichage = vitesse mesurée × 0,4 degré par seconde, plafonnée pour rester lisible à l'écran.
    private const double DegreesPerSecondPerUnit = 0.4;
    private const double MaxDegreesPerSecond = 720;

    // Flux d'air : des traits courts défilent du moyeu vers le bord (marche avant) ou l'inverse (marche arrière),
    // pour montrer que l'hélice déplace de l'air et dans quel sens, pas seulement qu'elle tourne.
    private const double FlowPixelsPerSecondPerUnit = 0.15;
    private const double MaxFlowPixelsPerSecond = 160;
    private const float FlowTravel = 36f;

    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 30 };
    private readonly Stopwatch _clock = new();
    private double _angle;
    private double _speed;
    private double _flowOffset;
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

        var flowPixels = Math.Min(_speed * FlowPixelsPerSecondPerUnit, MaxFlowPixelsPerSecond) * dt;
        _flowOffset = (_flowOffset + flowPixels) % FlowTravel;

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

        if (_speed > 0)
            DrawAirflow(g, cx, cy, radius);
    }

    /// <summary>
    /// Traits qui défilent le long de trois rayons pour représenter l'air déplacé par l'hélice.
    /// Marche avant : ils partent du moyeu vers le bord (l'air est poussé vers l'extérieur).
    /// Marche arrière : ils partent du bord vers le moyeu (le sens du flux s'inverse avec le sens du moteur).
    /// </summary>
    private void DrawAirflow(Graphics g, float cx, float cy, float radius)
    {
        var inward = !_clockwise;
        double[] angles = [-35, 0, 35]; // trois rayons, de part et d'autre de l'axe horizontal

        foreach (var angleDeg in angles)
        {
            var rad = angleDeg * Math.PI / 180.0;
            var dx = (float)Math.Cos(rad);
            var dy = (float)Math.Sin(rad);

            for (var k = 0; k < 2; k++) // deux traits par rayon, décalés pour un défilement continu
            {
                var travel = (float)((_flowOffset + k * (FlowTravel / 2)) % FlowTravel);
                var alpha = (byte)(170 * (1 - travel / FlowTravel));
                var r = inward ? radius + 2 - travel : radius + 6 + travel;
                if (r < 2) continue; // trop proche du moyeu : pas de trait, pour ne pas le traverser

                var x0 = cx + dx * r;
                var y0 = cy + dy * r;
                var tailLength = inward ? -9f : 9f; // le trait pointe vers l'extérieur ou vers l'intérieur selon le sens
                var x1 = x0 + dx * tailLength;
                var y1 = y0 + dy * tailLength;

                using var flowPen = new Pen(Color.FromArgb(alpha, 150, 190, 230), 2.5f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
                g.DrawLine(flowPen, x0, y0, x1, y1);
            }
        }
    }
}
