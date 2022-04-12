// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ConnectorElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using DiagramNet.Elements.Controllers;
using System.Collections;

[Serializable]
public class ConnectorElement : RectangleElement, IControllable
{
  private readonly NodeElement _parentElement;
  public bool IsStart;
  public object State;
  public string ShortName;
  [NonSerialized]
  private ConnectorController _controller;
  private ElementCollection _links = new ElementCollection();

  public ConnectorElement()
  {
  }

  public ConnectorElement(NodeElement parent)
    : base(new Rectangle(0, 0, 0, 0))
  {
    this._parentElement = parent;
    this.BorderColorValue = Color.Black;
    this.FillColor1Value = Color.LightGray;
    this.FillColor2Value = Color.Empty;
  }

  public NodeElement ParentElement => this._parentElement;

  internal void AddLink(BaseLinkElement lnk) => this.Links.Add((BaseElement) lnk);

  public void RemoveLink(BaseLinkElement lnk) => this.Links.Remove((BaseElement) lnk);

  public ElementCollection Links
  {
    get => this._links;
    set => this._links = value;
  }

  internal CardinalDirection GetDirection() => DiagramUtil.GetDirection(new Rectangle(this._parentElement.Location, this._parentElement.Size), new Point(this.LocationValue.X - this._parentElement.Location.X + this.SizeValue.Width / 2, this.LocationValue.Y - this._parentElement.Location.Y + this.SizeValue.Height / 2));

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new ConnectorController((BaseElement) this));

  public override Point Location
  {
    get => base.Location;
    set
    {
      if (value == base.Location)
        return;
      foreach (BaseLinkElement link in this.Links)
        link.NeedCalcLink = true;
      base.Location = value;
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      if (value == base.Size)
        return;
      foreach (BaseLinkElement link in this.Links)
        link.NeedCalcLink = true;
      base.Size = value;
    }
  }
}