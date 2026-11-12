using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Scada_App_For_plc.Views;

/// <summary>Voyant rond de couleur (remplace les PictureBox rouge/vert du cours).</summary>
[DesignerCategory("Code")]
public sealed class Led : Control
{
    private Color _ledColor = Color.Gray;

    public Led()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Size = new Size(30, 30);
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color LedColor
    {
        get => _ledColor;
        set
        {
            if (_ledColor == value) return;
            _ledColor = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(2, 2, Width - 5, Height - 5);
        using var fill = new SolidBrush(_ledColor);
        using var border = new Pen(Color.FromArgb(90, 0, 0, 0), 2);
        e.Graphics.FillEllipse(fill, rect);
        e.Graphics.DrawEllipse(border, rect);
    }
}
