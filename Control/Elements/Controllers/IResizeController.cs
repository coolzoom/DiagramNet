// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.IResizeController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing;

namespace DiagramNet.Elements.Controllers
{
  internal interface IResizeController : IController
  {
    RectangleElement[] Corners { get; }

    void UpdateCornersPos();

    CornerPosition HitTestCorner(Point p);

    void Start(Point posStart, CornerPosition corner);

    void Resize(Point posCurrent);

    void End(Point posEnd);

    bool IsResizing { get; }

    bool CanResize { get; }
  }
}
