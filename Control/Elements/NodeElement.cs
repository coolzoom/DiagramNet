// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.NodeElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

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
    InitConnectors();
  }

  protected NodeElement()
  {
  }

  [Browsable(false)]
  public virtual ConnectorElement[] Connectors => Connects;

  public override Point Location
  {
    get => LocationValue;
    set
    {
      LocationValue = value;
      if (Overrided)
        return;
      UpdateConnectorsPosition();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override Size Size
  {
    get => SizeValue;
    set
    {
      SizeValue = value;
      UpdateConnectorsPosition();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override bool Visible
  {
    get => VisibleValue;
    set
    {
      VisibleValue = value;
      foreach (BaseElement connect in Connects)
        connect.Visible = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public virtual bool IsConnected => Connects.Any<ConnectorElement>(c => c.Links.Count > 0);

  protected void InitConnectors()
  {
    Connects[0] = new ConnectorElement(this);
    Connects[1] = new ConnectorElement(this);
    Connects[2] = new ConnectorElement(this);
    Connects[3] = new ConnectorElement(this);
    UpdateConnectorsPosition();
  }

  protected void UpdateConnectorsPosition()
  {
    var point = new Point(LocationValue.X + SizeValue.Width / 2, LocationValue.Y);
    var connect1 = Connects[0];
    connect1.Location = new Point(point.X - 3, point.Y - 3);
    connect1.Size = new Size(6, 6);
    point = new Point(LocationValue.X + SizeValue.Width / 2, LocationValue.Y + SizeValue.Height);
    var connect2 = Connects[1];
    connect2.Location = new Point(point.X - 3, point.Y - 3);
    connect2.Size = new Size(6, 6);
    point = new Point(LocationValue.X, LocationValue.Y + SizeValue.Height / 2);
    var connect3 = Connects[2];
    connect3.Location = new Point(point.X - 3, point.Y - 3);
    connect3.Size = new Size(6, 6);
    point = new Point(LocationValue.X + SizeValue.Width, LocationValue.Y + SizeValue.Height / 2);
    var connect4 = Connects[3];
    connect4.Location = new Point(point.X - 3, point.Y - 3);
    connect4.Size = new Size(6, 6);
  }

  public override void Invalidate()
  {
    base.Invalidate();
    for (var index1 = Connects.Length - 1; index1 >= 0; --index1)
    {
      for (var index2 = Connects[index1].Links.Count - 1; index2 >= 0; --index2)
        Connects[index1].Links[index2].Invalidate();
    }
  }

  internal virtual void Draw(Graphics g, bool drawConnector)
  {
    Draw(g);
    if (!drawConnector)
      return;
    DrawConnectors(g);
  }

  protected void DrawConnectors(Graphics g)
  {
    foreach (BaseElement connect in Connects)
      connect.Draw(g);
  }

  public virtual ElementCollection GetLinkedNodes()
  {
    var linkedNodes = new ElementCollection();
    foreach (var connect in Connects)
    {
      foreach (BaseLinkElement link in connect.Links)
        linkedNodes.Add(link.Connector1 == connect ? link.Connector2.ParentElement : (BaseElement) link.Connector1.ParentElement);
    }
    return linkedNodes;
  }
}