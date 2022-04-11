// Decompiled with JetBrains decompiler
// Type: DiagramNet.Events.ElementSelectionEventArgs
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System;

namespace DiagramNet.Events
{
  public class ElementSelectionEventArgs : EventArgs
  {
    private readonly ElementCollection _elements;

    public ElementSelectionEventArgs(ElementCollection elements) => this._elements = elements;

    public ElementCollection Elements => this._elements;

    public override string ToString() => "ElementCollection: " + (object) this._elements.Count;
  }
}
