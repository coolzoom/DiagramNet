// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.BaseLinkElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using System.ComponentModel;
using System.Drawing.Drawing2D;

[Serializable]
public abstract class BaseLinkElement : BaseElement
{
  protected ConnectorElement Connector1Value;
  protected ConnectorElement Connector2Value;
  protected LineCap StartCapValue;
  protected LineCap EndCapValue;
  protected bool NeedCalcLinkValue = true;

  internal BaseLinkElement(ConnectorElement conn1, ConnectorElement conn2)
  {
    this.BorderWidthValue = 1;
    this.BorderColorValue = Color.Black;
    this.Connector1Value = conn1;
    this.Connector2Value = conn2;
    this.Connector1Value.AddLink(this);
    this.Connector2Value.AddLink(this);
  }

  [Browsable(false)]
  public ConnectorElement Connector1
  {
    get => this.Connector1Value;
    set
    {
      if (value == null)
        return;
      this.Connector1Value.RemoveLink(this);
      this.Connector1Value = value;
      this.NeedCalcLinkValue = true;
      this.Connector1Value.AddLink(this);
      this.OnConnectorChanged(new EventArgs());
    }
  }

  [Browsable(false)]
  public ConnectorElement Connector2
  {
    get => this.Connector2Value;
    set
    {
      if (value == null)
        return;
      this.Connector2Value.RemoveLink(this);
      this.Connector2Value = value;
      this.NeedCalcLinkValue = true;
      this.Connector2Value.AddLink(this);
      this.OnConnectorChanged(new EventArgs());
    }
  }

  [Browsable(false)]
  internal bool NeedCalcLink
  {
    get => this.NeedCalcLinkValue;
    set => this.NeedCalcLinkValue = value;
  }

  public abstract override Point Location { get; }

  public abstract override Size Size { get; }

  public abstract LineElement[] Lines { get; }

  [Browsable(false)]
  public abstract Point Point1 { get; }

  [Browsable(false)]
  public abstract Point Point2 { get; }

  public virtual LineCap StartCap
  {
    get => this.StartCapValue;
    set => this.StartCapValue = value;
  }

  public virtual LineCap EndCap
  {
    get => this.EndCapValue;
    set => this.EndCapValue = value;
  }

  internal abstract void CalcLink();

  [field: NonSerialized]
  public event EventHandler ConnectorChanged;

  protected virtual void OnConnectorChanged(EventArgs e)
  {
    if (this.ConnectorChanged == null)
      return;
    this.ConnectorChanged((object) this, e);
  }
}