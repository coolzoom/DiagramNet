// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ElipseElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using DiagramNet.Elements.Controllers;
using System.Drawing.Drawing2D;

[Serializable]
public class ElipseElement : RectangleElement, IControllable
{
  [NonSerialized]
  private ElipseController _controller;

  public ElipseElement()
  {
  }

  public ElipseElement(Rectangle rec)
    : base(rec)
  {
  }

  public ElipseElement(Point l, Size s)
    : base(l, s)
  {
  }

  public ElipseElement(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
  }

  internal override void Draw(Graphics g)
  {
    this.IsInvalidated = false;
    var unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(this.LocationValue.X, this.LocationValue.Y, this.SizeValue.Width, this.SizeValue.Height));
    Color color;
    Color color2;
    if (this.OpacityValue == 100)
    {
      color = this.FillColor1Value;
      color2 = this.FillColor2Value;
    }
    else
    {
      color = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this.FillColor1Value);
      color2 = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this.FillColor2Value);
    }
    var brush = !(this.FillColor2Value == Color.Empty) ? (Brush) new LinearGradientBrush(new Rectangle(unsignedRectangle.X, unsignedRectangle.Y, unsignedRectangle.Width + 1, unsignedRectangle.Height + 1), color, color2, LinearGradientMode.Horizontal) : (Brush) new SolidBrush(color);
    g.FillEllipse(brush, unsignedRectangle);
    var pen = new Pen(this.BorderColorValue, (float) this.BorderWidthValue);
    g.DrawEllipse(pen, unsignedRectangle);
    pen.Dispose();
    brush.Dispose();
  }

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new ElipseController((BaseElement) this));
}