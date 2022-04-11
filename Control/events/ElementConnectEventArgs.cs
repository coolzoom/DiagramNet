// Decompiled with JetBrains decompiler
// Type: DiagramNet.Events.ElementConnectEventArgs
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Events;

using DiagramNet.Elements;

public class ElementConnectEventArgs : EventArgs
{
  private readonly NodeElement _node1;
  private readonly NodeElement _node2;
  private readonly BaseLinkElement _link;

  public ElementConnectEventArgs(NodeElement node1, NodeElement node2, BaseLinkElement link)
  {
    this._node1 = node1;
    this._node2 = node2;
    this._link = link;
  }

  public NodeElement Node1 => this._node1;

  public NodeElement Node2 => this._node2;

  public BaseLinkElement Link => this._link;

  public override string ToString()
  {
    var str = "";
    if (this._node1 != null)
      str = str + "Node1:" + (object) this._node1;
    if (this._node2 != null)
      str = str + "Node2:" + (object) this._node2;
    if (this._link != null)
      str = str + "Link:" + (object) this._link;
    return str;
  }
}