// Decompiled with JetBrains decompiler
// Type: DiagramNet.Events.ElementMouseEventArgs
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Events;

using Elements;

public class ElementMouseEventArgs : ElementEventArgs
{
  public ElementMouseEventArgs(BaseElement el, int x, int y)
    : base(el)
  {
    X = x;
    Y = y;
  }

  public int X { get; set; }

  public int Y { get; set; }

  public override string ToString() => base.ToString() + " X:" + (object) X + " Y:" + (object) Y;
}