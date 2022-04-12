// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ConnectorElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;

[Serializable]
public class ConnectorElement : RectangleElement, IControllable
{
  private readonly NodeElement _parentElement;
  public bool IsStart;
  public object State;
  public string ShortName;
  [NonSerialized]
  private ConnectorController _controller;
  private ElementCollection _links = new();

  public ConnectorElement()
  {
  }

  public ConnectorElement(NodeElement parent)
    : base(new Rectangle(0, 0, 0, 0))
  {
    _parentElement = parent;
    BorderColorValue = Color.Black;
    FillColor1Value = Color.LightGray;
    FillColor2Value = Color.Empty;
  }

  public NodeElement ParentElement => _parentElement;

  internal void AddLink(BaseLinkElement lnk) => Links.Add(lnk);

  public void RemoveLink(BaseLinkElement lnk) => Links.Remove(lnk);

  public ElementCollection Links
  {
    get => _links;
    set => _links = value;
  }

  internal CardinalDirection GetDirection() => DiagramUtil.GetDirection(new Rectangle(_parentElement.Location, _parentElement.Size), new Point(LocationValue.X - _parentElement.Location.X + SizeValue.Width / 2, LocationValue.Y - _parentElement.Location.Y + SizeValue.Height / 2));

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new ConnectorController(this));

  public override Point Location
  {
    get => base.Location;
    set
    {
      if (value == base.Location)
        return;
      foreach (BaseLinkElement link in Links)
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
      foreach (BaseLinkElement link in Links)
        link.NeedCalcLink = true;
      base.Size = value;
    }
  }
}