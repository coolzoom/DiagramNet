// Decompiled with JetBrains decompiler
// Type: DiagramNet.Events.ElementEventArgs
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements;
using System;

namespace DiagramNet.Events
{
  public class ElementEventArgs : EventArgs
  {
    private readonly BaseElement _element;
    private readonly BaseElement _previousElement;

    public ElementEventArgs(BaseElement el) => this._element = el;

    public ElementEventArgs(BaseElement el, BaseElement previousEl)
    {
      this._element = el;
      this._previousElement = previousEl;
    }

    public BaseElement Element => this._element;

    public BaseElement PreviousElement => this._previousElement;

    public override string ToString() => "el: " + (object) this._element.GetHashCode();
  }
}
