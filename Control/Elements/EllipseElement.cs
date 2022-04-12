// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.EllipseElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.Drawing.Drawing2D;

[Serializable]
public class EllipseElement : RectangleElement, IControllable
{
  [NonSerialized]
  private EllipseController _controller;

  public EllipseElement()
  {
  }

  public EllipseElement(Rectangle rec)
    : base(rec)
  {
  }

  public EllipseElement(Point l, Size s)
    : base(l, s)
  {
  }

  public EllipseElement(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    var unsignedRectangle = GetUnsignedRectangle(new Rectangle(LocationValue.X, LocationValue.Y, SizeValue.Width, SizeValue.Height));
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
    var brush = !(FillColor2Value == Color.Empty) ? new LinearGradientBrush(new Rectangle(unsignedRectangle.X, unsignedRectangle.Y, unsignedRectangle.Width + 1, unsignedRectangle.Height + 1), color, color2, LinearGradientMode.Horizontal) : (Brush) new SolidBrush(color);
    g.FillEllipse(brush, unsignedRectangle);
    var pen = new Pen(BorderColorValue, BorderWidthValue);
    g.DrawEllipse(pen, unsignedRectangle);
    pen.Dispose();
    brush.Dispose();
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new EllipseController(this));
}