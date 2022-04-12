// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.ConnectorController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

internal class ConnectorController : RectangleController
{
  public ConnectorController(BaseElement element)
    : base(element)
  {
  }

  public override void DrawSelection(Graphics g)
  {
    var unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(El.Location.X - 1, El.Location.Y - 1, El.Size.Width + 2, El.Size.Height + 2));
    var solidBrush = new SolidBrush(Color.FromArgb(150, Color.Green));
    var pen = new Pen((Brush) solidBrush, 2f);
    g.DrawRectangle(pen, unsignedRectangle);
    pen.Dispose();
    solidBrush.Dispose();
  }
}