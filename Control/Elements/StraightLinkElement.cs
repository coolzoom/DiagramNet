// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.StraightLinkElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.ComponentModel;
using System.Drawing.Drawing2D;

[Serializable]
public class StraightLinkElement : BaseLinkElement, IControllable, ILabelElement
{
  protected LineElement Line1 = new(0, 0, 0, 0);
  private LabelElement _label = new();
  [NonSerialized]
  private LineController _controller;

  internal StraightLinkElement(ConnectorElement conn1, ConnectorElement conn2)
    : base(conn1, conn2)
  {
    _label.PositionBySite((BaseElement) Line1);
  }

  [Browsable(false)]
  public override Point Point1 => Line1.Point1;

  [Browsable(false)]
  public override Point Point2 => Line1.Point2;

  public override Color BorderColor
  {
    get => Line1.BorderColor;
    set => Line1.BorderColor = value;
  }

  public override int BorderWidth
  {
    get => Line1.BorderWidth;
    set => Line1.BorderWidth = value;
  }

  public override Point Location
  {
    get
    {
      CalcLink();
      return Line1.Location;
    }
  }

  public override Size Size
  {
    get
    {
      CalcLink();
      return Line1.Size;
    }
  }

  public override int Opacity
  {
    get => Line1.Opacity;
    set => Line1.Opacity = value;
  }

  public override LineCap StartCap
  {
    get => Line1.StartCap;
    set => Line1.StartCap = value;
  }

  public override LineCap EndCap
  {
    get => Line1.EndCap;
    set => Line1.EndCap = value;
  }

  public override LineElement[] Lines => new LineElement[1]
  {
    Line1
  };

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    Line1.Draw(g);
  }

  internal override void CalcLink()
  {
    if (!NeedCalcLinkValue)
      return;
    if (Line1 != null)
    {
      var location1 = Connector1Value.Location;
      var location2 = Connector2Value.Location;
      var size1 = Connector1Value.Size;
      var size2 = Connector2Value.Size;
      Line1.Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
      Line1.Point2 = new Point(location2.X + size2.Width / 2, location2.Y + size2.Height / 2);
      Line1.CalcLine();
    }
    NeedCalcLinkValue = false;
  }

  IController IControllable.GetController() => (IController) _controller ?? (IController) (_controller = new LineController(Line1));

  public virtual LabelElement Label
  {
    get => _label;
    set
    {
      _label = value;
      OnAppearanceChanged(new EventArgs());
    }
  }
}