// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RightAngleLinkElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.ComponentModel;
using System.Drawing.Drawing2D;

[Serializable]
public class RightAngleLinkElement : BaseLinkElement, IControllable, ILabelElement
{
  public LineElement[] LineElements = new LineElement[1]
  {
    new(0, 0, 0, 0)
  };
  public Orientation Orientation;
  private CardinalDirection _conn1Dir;
  private CardinalDirection _conn2Dir;
  private bool _needCalcLinkLocation = true;
  private bool _needCalcLinkSize = true;
  private LabelElement _label = new();
  [NonSerialized]
  private RightAngleLinkController _controller;

  public RightAngleLinkElement(ConnectorElement conn1, ConnectorElement conn2)
    : base(conn1, conn2)
  {
    NeedCalcLinkValue = true;
    InitConnectors(conn1, conn2);
    foreach (var lineElement in LineElements)
    {
      lineElement.StartCap = LineCap.Round;
      lineElement.EndCap = LineCap.Round;
    }
    StartCapValue = LineCap.Round;
    EndCapValue = LineCap.Round;
    _label.PositionBySite(LineElements[1]);
  }

  [Browsable(false)]
  public override Point Point1 => LineElements[0].Point1;

  [Browsable(false)]
  public override Point Point2 => LineElements[LineElements.Length - 1].Point2;

  public override Color BorderColor
  {
    get => BorderColorValue;
    set
    {
      BorderColorValue = value;
      foreach (BaseElement lineElement in LineElements)
        lineElement.BorderColor = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override int BorderWidth
  {
    get => BorderWidthValue;
    set
    {
      BorderWidthValue = value;
      foreach (BaseElement lineElement in LineElements)
        lineElement.BorderWidth = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override Point Location
  {
    get
    {
      CalcLinkLocation();
      return LocationValue;
    }
    set
    {
      if (!((IMoveController) ((IControllable) this).GetController()).IsMoving)
        return;
      var location = Location;
      var point1 = value;
      var point2 = new Point(point1.X - location.X, point1.Y - location.Y);
      foreach (var lineElement in LineElements)
      {
        var point1_1 = lineElement.Point1;
        var point2_1 = lineElement.Point2;
        lineElement.Point1 = new Point(point1_1.X + point2.X, point1_1.Y + point2.Y);
        lineElement.Point2 = new Point(point2_1.X + point2.X, point2_1.Y + point2.Y);
      }
      NeedCalcLinkValue = true;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override Size Size
  {
    get
    {
      CalcLinkSize();
      return SizeValue;
    }
  }

  public override int Opacity
  {
    get => OpacityValue;
    set
    {
      OpacityValue = value;
      foreach (BaseElement lineElement in LineElements)
        lineElement.Opacity = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override LineCap StartCap
  {
    get => StartCapValue;
    set
    {
      StartCapValue = value;
      LineElements[0].StartCap = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override LineCap EndCap
  {
    get => EndCapValue;
    set
    {
      EndCapValue = value;
      LineElements[LineElements.Length - 1].EndCap = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override LineElement[] Lines => (LineElement[]) LineElements.Clone();

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    CalcLink();
    for (var index = 0; index < LineElements.Length; ++index)
    {
      if (index == LineElements.Length - 1)
        LineElements[index].EndCap = LineCap.ArrowAnchor;
      LineElements[index].Draw(g);
    }
  }

  private void InitConnectors(ConnectorElement conn1, ConnectorElement conn2)
  {
    _conn1Dir = conn1.GetDirection();
    _conn2Dir = conn2.GetDirection();
    Orientation = _conn1Dir == CardinalDirection.North || _conn1Dir == CardinalDirection.South ? Orientation.Vertical : Orientation.Horizontal;
    if ((_conn1Dir == CardinalDirection.North || _conn1Dir == CardinalDirection.South) && (_conn2Dir == CardinalDirection.East || _conn2Dir == CardinalDirection.West) || (_conn1Dir == CardinalDirection.East || _conn1Dir == CardinalDirection.West) && (_conn2Dir == CardinalDirection.North || _conn2Dir == CardinalDirection.South))
    {
      LineElements = new LineElement[2];
      LineElements[0] = new LineElement(0, 0, 0, 0);
      LineElements[1] = new LineElement(0, 0, 0, 0);
    }
    else
    {
      LineElements = new LineElement[3];
      LineElements[0] = new LineElement(0, 0, 0, 0);
      LineElements[1] = new LineElement(0, 0, 0, 0);
      LineElements[2] = new LineElement(0, 0, 0, 0);
    }
    CalcLinkFirtTime();
    CalcLink();
    RestartProps();
  }

  private void RestartProps()
  {
    foreach (var lineElement in LineElements)
    {
      lineElement.BorderColor = BorderColorValue;
      lineElement.BorderWidth = BorderWidthValue;
      lineElement.Opacity = OpacityValue;
      lineElement.StartCap = StartCapValue;
      lineElement.EndCap = EndCapValue;
    }
  }

  protected override void OnConnectorChanged(EventArgs e)
  {
    InitConnectors(Connector1Value, Connector2Value);
    base.OnConnectorChanged(e);
  }

  internal void CalcLinkFirtTime()
  {
    if (LineElements == null)
      return;
    var lineElement = LineElements[LineElements.Length - 1];
    var location1 = Connector1Value.Location;
    var location2 = Connector2Value.Location;
    var size1 = Connector1Value.Size;
    var size2 = Connector2Value.Size;
    LineElements[0].Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
    lineElement.Point2 = Orientation == Orientation.Horizontal ? new Point(location2.X, location2.Y + size1.Height / 2) : new Point(location2.X + size2.Width / 2, location2.Y);
    if (LineElements.Length != 3)
      return;
    var point1 = LineElements[0].Point1;
    var point2 = lineElement.Point2;
    if (Orientation == Orientation.Horizontal)
    {
      LineElements[0].Point2 = new Point(point1.X + (point2.X - point1.X) / 2, point1.Y);
      lineElement.Point1 = new Point(point1.X + (point2.X - point1.X) / 2, point2.Y);
    }
    else
    {
      if (Orientation != Orientation.Vertical)
        return;
      LineElements[0].Point2 = new Point(point1.X, point1.Y + (point2.Y - point1.Y) / 2);
      lineElement.Point1 = new Point(point2.X, point1.Y + (point2.Y - point1.Y) / 2);
    }
  }

  internal override void CalcLink()
  {
    if (!NeedCalcLinkValue || LineElements == null)
      return;
    var lineElement = LineElements[LineElements.Length - 1];
    var location1 = Connector1Value.Location;
    var location2 = Connector2Value.Location;
    var size1 = Connector1Value.Size;
    var size2 = Connector2Value.Size;
    LineElements[0].Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
    lineElement.Point2 = Orientation == Orientation.Horizontal ? new Point(location2.X, location2.Y + size2.Height / 2) : new Point(location2.X + size2.Width / 2, location2.Y);
    if (LineElements.Length == 3)
    {
      if (Orientation == Orientation.Horizontal)
      {
        LineElements[0].Point2 = new Point(LineElements[0].Point2.X, LineElements[0].Point1.Y);
        lineElement.Point1 = new Point(lineElement.Point1.X, lineElement.Point2.Y);
        LineElements[1].Point1 = LineElements[0].Point2;
        LineElements[1].Point2 = LineElements[2].Point1;
      }
      else if (Orientation == Orientation.Vertical)
      {
        LineElements[0].Point2 = new Point(LineElements[0].Point1.X, LineElements[0].Point2.Y);
        lineElement.Point1 = new Point(lineElement.Point2.X, lineElement.Point1.Y);
        LineElements[1].Point1 = LineElements[0].Point2;
        LineElements[1].Point2 = LineElements[2].Point1;
      }
    }
    else if (LineElements.Length == 2)
    {
      LineElements[0].Point2 = _conn1Dir == CardinalDirection.North || _conn1Dir == CardinalDirection.South ? new Point(LineElements[0].Point1.X, lineElement.Point2.Y) : new Point(lineElement.Point2.X, LineElements[0].Point1.Y);
      lineElement.Point1 = LineElements[0].Point2;
    }
    _needCalcLinkLocation = true;
    _needCalcLinkSize = true;
    NeedCalcLinkValue = false;
  }

  private void CalcLinkLocation()
  {
    if (!_needCalcLinkLocation)
      return;
    var points = new Point[LineElements.Length * 2];
    var index = 0;
    foreach (var lineElement in LineElements)
    {
      points[index] = lineElement.Point1;
      points[index + 1] = lineElement.Point2;
      index += 2;
    }
    LocationValue = DiagramUtil.GetUpperPoint(points);
    _needCalcLinkLocation = false;
  }

  private void CalcLinkSize()
  {
    if (!_needCalcLinkSize)
      return;
    var size = Size.Empty;
    if (LineElements.Length > 1)
    {
      var points = new Point[LineElements.Length * 2];
      var index = 0;
      foreach (var lineElement in LineElements)
      {
        points[index] = lineElement.Point1;
        points[index + 1] = lineElement.Point2;
        index += 2;
      }
      var upperPoint = DiagramUtil.GetUpperPoint(points);
      var lowerPoint = DiagramUtil.GetLowerPoint(points);
      size = new Size(lowerPoint.X - upperPoint.X, lowerPoint.Y - upperPoint.Y);
    }
    SizeValue = size;
    _needCalcLinkSize = false;
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RightAngleLinkController(this));

  public virtual LabelElement Label
  {
    get => _label;
    set
    {
      _label = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }
}