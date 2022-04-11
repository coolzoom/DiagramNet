// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.CommentBoxController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

internal class CommentBoxController : RectangleController, ILabelController
{
  public CommentBoxController(BaseElement element)
    : base(element)
  {
  }

  public void SetLabelPosition()
  {
    LabelElement label = ((ILabelElement) this.El).Label;
    label.Location = this.El.Location;
    label.Size = this.El.Size;
  }
}