// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.ConnectorController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing;

namespace DiagramNet.Elements.Controllers
{
  internal class ConnectorController : RectangleController
  {
    public ConnectorController(BaseElement element)
      : base(element)
    {
    }

    public override void DrawSelection(Graphics g)
    {
      Rectangle unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(this.El.Location.X - 1, this.El.Location.Y - 1, this.El.Size.Width + 2, this.El.Size.Height + 2));
      SolidBrush solidBrush = new SolidBrush(Color.FromArgb(150, Color.Green));
      Pen pen = new Pen((Brush) solidBrush, 2f);
      g.DrawRectangle(pen, unsignedRectangle);
      pen.Dispose();
      solidBrush.Dispose();
    }
  }
}
