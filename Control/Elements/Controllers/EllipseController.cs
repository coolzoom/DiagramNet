// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.EllipseController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

using System.Drawing.Drawing2D;

internal class EllipseController : RectangleController
{
  public EllipseController(BaseElement element)
    : base(element)
  {
  }

  public override bool HitTest(Point p)
  {
    var graphicsPath = new GraphicsPath();
    var matrix = new Matrix();
    graphicsPath.AddEllipse(new Rectangle(El.Location.X, El.Location.Y, El.Size.Width, El.Size.Height));
    graphicsPath.Transform(matrix);
    return graphicsPath.IsVisible(p);
  }

  public override void DrawSelection(Graphics g)
  {
    var unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(El.Location.X - 3, El.Location.Y - 3, El.Size.Width + 6, El.Size.Height + 6));
    var hatchBrush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent);
    var pen = new Pen(hatchBrush, 3f);
    g.DrawEllipse(pen, unsignedRectangle);
    pen.Dispose();
    hatchBrush.Dispose();
  }
}