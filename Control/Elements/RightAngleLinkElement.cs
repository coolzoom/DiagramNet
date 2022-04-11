// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RightAngleLinkElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DiagramNet.Elements;

[Serializable]
public class RightAngleLinkElement : BaseLinkElement, IControllable, ILabelElement
{
  public LineElement[] LineElements = new LineElement[1]
  {
    new LineElement(0, 0, 0, 0)
  };
  public Orientation Orientation;
  private CardinalDirection _conn1Dir;
  private CardinalDirection _conn2Dir;
  private bool _needCalcLinkLocation = true;
  private bool _needCalcLinkSize = true;
  private LabelElement _label = new LabelElement();
  [NonSerialized]
  private RightAngleLinkController _controller;

  public RightAngleLinkElement(ConnectorElement conn1, ConnectorElement conn2)
    : base(conn1, conn2)
  {
    this.NeedCalcLinkValue = true;
    this.InitConnectors(conn1, conn2);
    foreach (LineElement lineElement in this.LineElements)
    {
      lineElement.StartCap = LineCap.Round;
      lineElement.EndCap = LineCap.Round;
    }
    this.StartCapValue = LineCap.Round;
    this.EndCapValue = LineCap.Round;
    this._label.PositionBySite((BaseElement) this.LineElements[1]);
  }

  [Browsable(false)]
  public override Point Point1 => this.LineElements[0].Point1;

  [Browsable(false)]
  public override Point Point2 => this.LineElements[this.LineElements.Length - 1].Point2;

  public override Color BorderColor
  {
    get => this.BorderColorValue;
    set
    {
      this.BorderColorValue = value;
      foreach (BaseElement lineElement in this.LineElements)
        lineElement.BorderColor = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override int BorderWidth
  {
    get => this.BorderWidthValue;
    set
    {
      this.BorderWidthValue = value;
      foreach (BaseElement lineElement in this.LineElements)
        lineElement.BorderWidth = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override Point Location
  {
    get
    {
      this.CalcLinkLocation();
      return this.LocationValue;
    }
    set
    {
      if (!((IMoveController) ((IControllable) this).GetController()).IsMoving)
        return;
      Point location = this.Location;
      Point point1 = value;
      Point point2 = new Point(point1.X - location.X, point1.Y - location.Y);
      foreach (LineElement lineElement in this.LineElements)
      {
        Point point1_1 = lineElement.Point1;
        Point point2_1 = lineElement.Point2;
        lineElement.Point1 = new Point(point1_1.X + point2.X, point1_1.Y + point2.Y);
        lineElement.Point2 = new Point(point2_1.X + point2.X, point2_1.Y + point2.Y);
      }
      this.NeedCalcLinkValue = true;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override Size Size
  {
    get
    {
      this.CalcLinkSize();
      return this.SizeValue;
    }
  }

  public override int Opacity
  {
    get => this.OpacityValue;
    set
    {
      this.OpacityValue = value;
      foreach (BaseElement lineElement in this.LineElements)
        lineElement.Opacity = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override LineCap StartCap
  {
    get => this.StartCapValue;
    set
    {
      this.StartCapValue = value;
      this.LineElements[0].StartCap = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override LineCap EndCap
  {
    get => this.EndCapValue;
    set
    {
      this.EndCapValue = value;
      this.LineElements[this.LineElements.Length - 1].EndCap = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override LineElement[] Lines => (LineElement[]) this.LineElements.Clone();

  internal override void Draw(Graphics g)
  {
    this.IsInvalidated = false;
    this.CalcLink();
    for (int index = 0; index < this.LineElements.Length; ++index)
    {
      if (index == this.LineElements.Length - 1)
        this.LineElements[index].EndCap = LineCap.ArrowAnchor;
      this.LineElements[index].Draw(g);
    }
  }

  private void InitConnectors(ConnectorElement conn1, ConnectorElement conn2)
  {
    this._conn1Dir = conn1.GetDirection();
    this._conn2Dir = conn2.GetDirection();
    this.Orientation = this._conn1Dir == CardinalDirection.North || this._conn1Dir == CardinalDirection.South ? Orientation.Vertical : Orientation.Horizontal;
    if ((this._conn1Dir == CardinalDirection.North || this._conn1Dir == CardinalDirection.South) && (this._conn2Dir == CardinalDirection.East || this._conn2Dir == CardinalDirection.West) || (this._conn1Dir == CardinalDirection.East || this._conn1Dir == CardinalDirection.West) && (this._conn2Dir == CardinalDirection.North || this._conn2Dir == CardinalDirection.South))
    {
      this.LineElements = new LineElement[2];
      this.LineElements[0] = new LineElement(0, 0, 0, 0);
      this.LineElements[1] = new LineElement(0, 0, 0, 0);
    }
    else
    {
      this.LineElements = new LineElement[3];
      this.LineElements[0] = new LineElement(0, 0, 0, 0);
      this.LineElements[1] = new LineElement(0, 0, 0, 0);
      this.LineElements[2] = new LineElement(0, 0, 0, 0);
    }
    this.CalcLinkFirtTime();
    this.CalcLink();
    this.RestartProps();
  }

  private void RestartProps()
  {
    foreach (LineElement lineElement in this.LineElements)
    {
      lineElement.BorderColor = this.BorderColorValue;
      lineElement.BorderWidth = this.BorderWidthValue;
      lineElement.Opacity = this.OpacityValue;
      lineElement.StartCap = this.StartCapValue;
      lineElement.EndCap = this.EndCapValue;
    }
  }

  protected override void OnConnectorChanged(EventArgs e)
  {
    this.InitConnectors(this.Connector1Value, this.Connector2Value);
    base.OnConnectorChanged(e);
  }

  internal void CalcLinkFirtTime()
  {
    if (this.LineElements == null)
      return;
    LineElement lineElement = this.LineElements[this.LineElements.Length - 1];
    Point location1 = this.Connector1Value.Location;
    Point location2 = this.Connector2Value.Location;
    Size size1 = this.Connector1Value.Size;
    Size size2 = this.Connector2Value.Size;
    this.LineElements[0].Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
    lineElement.Point2 = this.Orientation == Orientation.Horizontal ? new Point(location2.X, location2.Y + size1.Height / 2) : new Point(location2.X + size2.Width / 2, location2.Y);
    if (this.LineElements.Length != 3)
      return;
    Point point1 = this.LineElements[0].Point1;
    Point point2 = lineElement.Point2;
    if (this.Orientation == Orientation.Horizontal)
    {
      this.LineElements[0].Point2 = new Point(point1.X + (point2.X - point1.X) / 2, point1.Y);
      lineElement.Point1 = new Point(point1.X + (point2.X - point1.X) / 2, point2.Y);
    }
    else
    {
      if (this.Orientation != Orientation.Vertical)
        return;
      this.LineElements[0].Point2 = new Point(point1.X, point1.Y + (point2.Y - point1.Y) / 2);
      lineElement.Point1 = new Point(point2.X, point1.Y + (point2.Y - point1.Y) / 2);
    }
  }

  internal override void CalcLink()
  {
    if (!this.NeedCalcLinkValue || this.LineElements == null)
      return;
    LineElement lineElement = this.LineElements[this.LineElements.Length - 1];
    Point location1 = this.Connector1Value.Location;
    Point location2 = this.Connector2Value.Location;
    Size size1 = this.Connector1Value.Size;
    Size size2 = this.Connector2Value.Size;
    this.LineElements[0].Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
    lineElement.Point2 = this.Orientation == Orientation.Horizontal ? new Point(location2.X, location2.Y + size2.Height / 2) : new Point(location2.X + size2.Width / 2, location2.Y);
    if (this.LineElements.Length == 3)
    {
      if (this.Orientation == Orientation.Horizontal)
      {
        this.LineElements[0].Point2 = new Point(this.LineElements[0].Point2.X, this.LineElements[0].Point1.Y);
        lineElement.Point1 = new Point(lineElement.Point1.X, lineElement.Point2.Y);
        this.LineElements[1].Point1 = this.LineElements[0].Point2;
        this.LineElements[1].Point2 = this.LineElements[2].Point1;
      }
      else if (this.Orientation == Orientation.Vertical)
      {
        this.LineElements[0].Point2 = new Point(this.LineElements[0].Point1.X, this.LineElements[0].Point2.Y);
        lineElement.Point1 = new Point(lineElement.Point2.X, lineElement.Point1.Y);
        this.LineElements[1].Point1 = this.LineElements[0].Point2;
        this.LineElements[1].Point2 = this.LineElements[2].Point1;
      }
    }
    else if (this.LineElements.Length == 2)
    {
      this.LineElements[0].Point2 = this._conn1Dir == CardinalDirection.North || this._conn1Dir == CardinalDirection.South ? new Point(this.LineElements[0].Point1.X, lineElement.Point2.Y) : new Point(lineElement.Point2.X, this.LineElements[0].Point1.Y);
      lineElement.Point1 = this.LineElements[0].Point2;
    }
    this._needCalcLinkLocation = true;
    this._needCalcLinkSize = true;
    this.NeedCalcLinkValue = false;
  }

  private void CalcLinkLocation()
  {
    if (!this._needCalcLinkLocation)
      return;
    Point[] points = new Point[this.LineElements.Length * 2];
    int index = 0;
    foreach (LineElement lineElement in this.LineElements)
    {
      points[index] = lineElement.Point1;
      points[index + 1] = lineElement.Point2;
      index += 2;
    }
    this.LocationValue = DiagramUtil.GetUpperPoint(points);
    this._needCalcLinkLocation = false;
  }

  private void CalcLinkSize()
  {
    if (!this._needCalcLinkSize)
      return;
    Size size = Size.Empty;
    if (this.LineElements.Length > 1)
    {
      Point[] points = new Point[this.LineElements.Length * 2];
      int index = 0;
      foreach (LineElement lineElement in this.LineElements)
      {
        points[index] = lineElement.Point1;
        points[index + 1] = lineElement.Point2;
        index += 2;
      }
      Point upperPoint = DiagramUtil.GetUpperPoint(points);
      Point lowerPoint = DiagramUtil.GetLowerPoint(points);
      size = new Size(lowerPoint.X - upperPoint.X, lowerPoint.Y - upperPoint.Y);
    }
    this.SizeValue = size;
    this._needCalcLinkSize = false;
  }

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RightAngleLinkController(this));

  public virtual LabelElement Label
  {
    get => this._label;
    set
    {
      this._label = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }
}