// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.ElipseController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing.Drawing2D;

namespace DiagramNet.Elements.Controllers;

internal class ElipseController : RectangleController
{
  public ElipseController(BaseElement element)
    : base(element)
  {
  }

  public override bool HitTest(Point p)
  {
    GraphicsPath graphicsPath = new GraphicsPath();
    Matrix matrix = new Matrix();
    graphicsPath.AddEllipse(new Rectangle(this.El.Location.X, this.El.Location.Y, this.El.Size.Width, this.El.Size.Height));
    graphicsPath.Transform(matrix);
    return graphicsPath.IsVisible(p);
  }

  public override void DrawSelection(Graphics g)
  {
    Rectangle unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(this.El.Location.X - 3, this.El.Location.Y - 3, this.El.Size.Width + 6, this.El.Size.Height + 6));
    HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent);
    Pen pen = new Pen((Brush) hatchBrush, 3f);
    g.DrawEllipse(pen, unsignedRectangle);
    pen.Dispose();
    hatchBrush.Dispose();
  }
}