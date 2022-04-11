// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.NodeElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using System.Collections;
using System.ComponentModel;

[Serializable]
public abstract class NodeElement : BaseElement
{
  protected const int ConnectSize = 3;
  protected ConnectorElement[] Connects = new ConnectorElement[4];
  protected bool Overrided;

  protected NodeElement(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
    this.InitConnectors();
  }

  protected NodeElement()
  {
  }

  [Browsable(false)]
  public virtual ConnectorElement[] Connectors => this.Connects;

  public override Point Location
  {
    get => this.LocationValue;
    set
    {
      this.LocationValue = value;
      if (this.Overrided)
        return;
      this.UpdateConnectorsPosition();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override Size Size
  {
    get => this.SizeValue;
    set
    {
      this.SizeValue = value;
      this.UpdateConnectorsPosition();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override bool Visible
  {
    get => this.VisibleValue;
    set
    {
      this.VisibleValue = value;
      foreach (BaseElement connect in this.Connects)
        connect.Visible = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual bool IsConnected => ((IEnumerable<ConnectorElement>) this.Connects).Any<ConnectorElement>((Func<ConnectorElement, bool>) (c => c.Links.Count > 0));

  protected void InitConnectors()
  {
    this.Connects[0] = new ConnectorElement(this);
    this.Connects[1] = new ConnectorElement(this);
    this.Connects[2] = new ConnectorElement(this);
    this.Connects[3] = new ConnectorElement(this);
    this.UpdateConnectorsPosition();
  }

  protected void UpdateConnectorsPosition()
  {
    var point = new Point(this.LocationValue.X + this.SizeValue.Width / 2, this.LocationValue.Y);
    var connect1 = this.Connects[0];
    connect1.Location = new Point(point.X - 3, point.Y - 3);
    connect1.Size = new Size(6, 6);
    point = new Point(this.LocationValue.X + this.SizeValue.Width / 2, this.LocationValue.Y + this.SizeValue.Height);
    var connect2 = this.Connects[1];
    connect2.Location = new Point(point.X - 3, point.Y - 3);
    connect2.Size = new Size(6, 6);
    point = new Point(this.LocationValue.X, this.LocationValue.Y + this.SizeValue.Height / 2);
    var connect3 = this.Connects[2];
    connect3.Location = new Point(point.X - 3, point.Y - 3);
    connect3.Size = new Size(6, 6);
    point = new Point(this.LocationValue.X + this.SizeValue.Width, this.LocationValue.Y + this.SizeValue.Height / 2);
    var connect4 = this.Connects[3];
    connect4.Location = new Point(point.X - 3, point.Y - 3);
    connect4.Size = new Size(6, 6);
  }

  public override void Invalidate()
  {
    base.Invalidate();
    for (var index1 = this.Connects.Length - 1; index1 >= 0; --index1)
    {
      for (var index2 = this.Connects[index1].Links.Count - 1; index2 >= 0; --index2)
        this.Connects[index1].Links[index2].Invalidate();
    }
  }

  internal virtual void Draw(Graphics g, bool drawConnector)
  {
    this.Draw(g);
    if (!drawConnector)
      return;
    this.DrawConnectors(g);
  }

  protected void DrawConnectors(Graphics g)
  {
    foreach (BaseElement connect in this.Connects)
      connect.Draw(g);
  }

  public virtual ElementCollection GetLinkedNodes()
  {
    var linkedNodes = new ElementCollection();
    foreach (var connect in this.Connects)
    {
      foreach (BaseLinkElement link in (ReadOnlyCollectionBase) connect.Links)
        linkedNodes.Add(link.Connector1 == connect ? (BaseElement) link.Connector2.ParentElement : (BaseElement) link.Connector1.ParentElement);
    }
    return linkedNodes;
  }
}