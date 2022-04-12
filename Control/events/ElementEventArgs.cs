// Decompiled with JetBrains decompiler
// Type: DiagramNet.Events.ElementEventArgs
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Events;

using Elements;

public class ElementEventArgs : EventArgs
{
  private readonly BaseElement _element;
  private readonly BaseElement _previousElement;

  public ElementEventArgs(BaseElement el) => _element = el;

  public ElementEventArgs(BaseElement el, BaseElement previousEl)
  {
    _element = el;
    _previousElement = previousEl;
  }

  public BaseElement Element => _element;

  public BaseElement PreviousElement => _previousElement;

  public override string ToString() => "el: " + (object) _element.GetHashCode();
}