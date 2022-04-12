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
    BorderWidthValue = 1;
    BorderColorValue = Color.Black;
    Connector1Value = conn1;
    Connector2Value = conn2;
    Connector1Value.AddLink(this);
    Connector2Value.AddLink(this);
  }

  [Browsable(false)]
  public ConnectorElement Connector1
  {
    get => Connector1Value;
    set
    {
      if (value == null)
        return;
      Connector1Value.RemoveLink(this);
      Connector1Value = value;
      NeedCalcLinkValue = true;
      Connector1Value.AddLink(this);
      OnConnectorChanged(new EventArgs());
    }
  }

  [Browsable(false)]
  public ConnectorElement Connector2
  {
    get => Connector2Value;
    set
    {
      if (value == null)
        return;
      Connector2Value.RemoveLink(this);
      Connector2Value = value;
      NeedCalcLinkValue = true;
      Connector2Value.AddLink(this);
      OnConnectorChanged(new EventArgs());
    }
  }

  [Browsable(false)]
  internal bool NeedCalcLink
  {
    get => NeedCalcLinkValue;
    set => NeedCalcLinkValue = value;
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
    get => StartCapValue;
    set => StartCapValue = value;
  }

  public virtual LineCap EndCap
  {
    get => EndCapValue;
    set => EndCapValue = value;
  }

  internal abstract void CalcLink();

  [field: NonSerialized]
  public event EventHandler ConnectorChanged;

  protected virtual void OnConnectorChanged(EventArgs e)
  {
    if (ConnectorChanged == null)
      return;
    ConnectorChanged((object) this, e);
  }
}