// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RectangleElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.Drawing.Drawing2D;

[Serializable]
public class RectangleElement : BaseElement, IControllable, ILabelElement
{
  protected Color FillColor1Value = Color.White;
  protected Color FillColor2Value = Color.DodgerBlue;
  protected LabelElement LabelValue = new();
  [NonSerialized]
  private RectangleController _controller;

  public RectangleElement()
    : this(0, 0, 100, 100)
  {
  }

  public RectangleElement(Rectangle rec)
    : this(rec.Location, rec.Size)
  {
  }

  public RectangleElement(Point l, Size s)
    : this(l.X, l.Y, s.Width, s.Height)
  {
  }

  public RectangleElement(int top, int left, int width, int height)
  {
    LocationValue = new Point(top, left);
    SizeValue = new Size(width, height);
  }

  public override Point Location
  {
    get => base.Location;
    set => base.Location = value;
  }

  public override Size Size
  {
    get => base.Size;
    set => base.Size = value;
  }

  public virtual Color FillColor1
  {
    get => FillColor1Value;
    set
    {
      FillColor1Value = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public virtual Color FillColor2
  {
    get => FillColor2Value;
    set
    {
      FillColor2Value = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public virtual LabelElement Label
  {
    get => LabelValue;
    set
    {
      LabelValue = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  protected virtual Brush GetBrush(Rectangle r)
  {
    Color color;
    Color color2;
    if (OpacityValue == 100)
    {
      color = FillColor1Value;
      color2 = FillColor2Value;
    }
    else
    {
      color = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), FillColor1Value);
      color2 = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), FillColor2Value);
    }
    return !(FillColor2Value == Color.Empty) ? new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : new SolidBrush(color);
  }

  protected virtual void DrawBorder(Graphics g, Rectangle r)
  {
    var pen = new Pen(BorderColorValue, BorderWidthValue);
    g.DrawRectangle(pen, r);
    pen.Dispose();
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    var unsignedRectangle = GetUnsignedRectangle();
    var brush = GetBrush(unsignedRectangle);
    g.FillRectangle(brush, unsignedRectangle);
    DrawBorder(g, unsignedRectangle);
    brush.Dispose();
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RectangleController(this));
}