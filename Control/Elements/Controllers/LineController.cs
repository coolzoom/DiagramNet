// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.LineController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

using System.Drawing.Drawing2D;

internal class LineController : IController
{
  protected LineElement El;

  public LineController(LineElement element) => this.El = element;

  public BaseElement OwnerElement => (BaseElement) this.El;

  public bool HitTest(Point p)
  {
    var graphicsPath = new GraphicsPath();
    var matrix = new Matrix();
    var pen = new Pen(this.El.BorderColor, (float) (this.El.BorderWidth + 4))
    {
      StartCap = this.El.StartCap,
      EndCap = this.El.EndCap
    };
    graphicsPath.AddLine(this.El.Point1, this.El.Point2);
    graphicsPath.Transform(matrix);
    return graphicsPath.IsOutlineVisible(p, pen);
  }

  public bool HitTest(Rectangle r)
  {
    var graphicsPath = new GraphicsPath();
    var matrix = new Matrix();
    graphicsPath.AddRectangle(new Rectangle(this.El.Location.X, this.El.Location.Y, this.El.Size.Width, this.El.Size.Height));
    graphicsPath.Transform(matrix);
    var rect = Rectangle.Round(graphicsPath.GetBounds());
    return r.Contains(rect);
  }

  public void DrawSelection(Graphics g)
  {
  }
}