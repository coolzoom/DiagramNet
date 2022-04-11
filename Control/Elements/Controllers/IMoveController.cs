// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.IMoveController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing;

namespace DiagramNet.Elements.Controllers
{
  internal interface IMoveController : IController
  {
    bool IsMoving { get; }

    void Start(Point posStart);

    void Move(Point posCurrent);

    bool WillMove(Point posCurrent);

    void End();

    bool CanMove { get; }
  }
}
