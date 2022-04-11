// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.RightAngleLinkController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing.Drawing2D;

namespace DiagramNet.Elements.Controllers;

internal class RightAngleLinkController : 
  IMoveController,
  IResizeController,
  IController,
  ILabelController
{
  protected const int SelCornerSize = 3;
  protected RightAngleLinkElement El;
  protected Point DragOffset = new Point(0);
  protected bool IsDragging;
  protected bool CanMove1 = true;
  protected RectangleElement[] SelectionCorner;
  protected CornerPosition SelCorner = CornerPosition.Nothing;
  protected bool CanResize1 = true;

  public RightAngleLinkController(RightAngleLinkElement element)
  {
    this.El = element;
    if (this.El.LineElements.Length == 3)
    {
      this.SelectionCorner = new RectangleElement[1];
      var selectionCorner = this.SelectionCorner;
      var rectangleElement1 = new RectangleElement(0, 0, 6, 6);
      rectangleElement1.BorderColor = Color.Black;
      rectangleElement1.FillColor1 = Color.White;
      rectangleElement1.FillColor2 = Color.Empty;
      var rectangleElement2 = rectangleElement1;
      selectionCorner[0] = rectangleElement2;
    }
    else
      this.SelectionCorner = new RectangleElement[0];
  }

  public BaseElement OwnerElement => (BaseElement) this.El;

  public bool HitTest(Point p) => ((IEnumerable<LineElement>) this.El.LineElements).Select<LineElement, IController>((Func<LineElement, IController>) (l => ((IControllable) l).GetController())).Any<IController>((Func<IController, bool>) (ctrl => ctrl.HitTest(p)));

  bool IController.HitTest(Rectangle r)
  {
    var graphicsPath = new GraphicsPath();
    var matrix = new Matrix();
    var location = this.El.Location;
    var size = this.El.Size;
    graphicsPath.AddRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));
    graphicsPath.Transform(matrix);
    var rect = Rectangle.Round(graphicsPath.GetBounds());
    return r.Contains(rect);
  }

  public void DrawSelection(Graphics g)
  {
    foreach (IControllable lineElement in this.El.LineElements)
      lineElement.GetController().DrawSelection(g);
  }

  public RectangleElement[] Corners => this.SelectionCorner;

  void IResizeController.UpdateCornersPos()
  {
    if (this.SelectionCorner.Length != 1)
      return;
    var point1 = this.El.LineElements[1].Point1;
    var point2 = this.El.LineElements[1].Point2;
    this.SelectionCorner[0].Location = new Point(point1.X + (point2.X - point1.X) / 2 - 3, point1.Y + (point2.Y - point1.Y) / 2 - 3);
  }

  CornerPosition IResizeController.HitTestCorner(Point p)
  {
    if (this.SelectionCorner.Length != 1 || !((IControllable) this.SelectionCorner[0]).GetController().HitTest(p))
      return CornerPosition.Nothing;
    if (this.El.Orientation == Orientation.Horizontal)
      return CornerPosition.MiddleLeft;
    return this.El.Orientation != Orientation.Vertical ? CornerPosition.Undefined : CornerPosition.TopCenter;
  }

  void IResizeController.Start(Point posStart, CornerPosition corner)
  {
    this.SelCorner = corner;
    this.DragOffset.X = this.SelectionCorner[0].Location.X - posStart.X;
    this.DragOffset.Y = this.SelectionCorner[0].Location.Y - posStart.Y;
  }

  void IResizeController.Resize(Point posCurrent)
  {
    var rectangleElement = this.SelectionCorner[0];
    var point1 = posCurrent;
    point1.Offset(this.DragOffset.X, this.DragOffset.Y);
    if (point1.X < 0)
      point1.X = 0;
    if (point1.Y < 0)
      point1.Y = 0;
    if (this.El.Orientation == Orientation.Horizontal)
    {
      rectangleElement.Location = new Point(point1.X, rectangleElement.Location.Y);
      var point2 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
      this.El.LineElements[1].Point1 = new Point(point2.X, this.El.LineElements[1].Point1.Y);
      this.El.LineElements[1].Point2 = new Point(point2.X, this.El.LineElements[1].Point2.Y);
    }
    else
    {
      rectangleElement.Location = new Point(rectangleElement.Location.X, point1.Y);
      var point3 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
      this.El.LineElements[1].Point1 = new Point(this.El.LineElements[1].Point1.X, point3.Y);
      this.El.LineElements[1].Point2 = new Point(this.El.LineElements[1].Point2.X, point3.Y);
    }
    this.El.LineElements[0].Point2 = this.El.LineElements[1].Point1;
    this.El.LineElements[2].Point1 = this.El.LineElements[1].Point2;
    this.El.NeedCalcLink = true;
  }

  void IResizeController.End(Point posEnd)
  {
    this.SelCorner = CornerPosition.Nothing;
    this.DragOffset = Point.Empty;
  }

  bool IResizeController.IsResizing => this.SelCorner != CornerPosition.Nothing;

  bool IResizeController.CanResize => this.CanResize1;

  void IMoveController.Start(Point posStart)
  {
    this.DragOffset.X = this.El.Location.X - posStart.X;
    this.DragOffset.Y = this.El.Location.Y - posStart.Y;
    this.IsDragging = true;
  }

  bool IMoveController.WillMove(Point posCurrent)
  {
    if (!this.IsDragging)
      return false;
    var point = posCurrent;
    point.Offset(this.DragOffset.X, this.DragOffset.Y);
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    return this.El.Location.X != point.X || this.El.Location.Y != point.Y;
  }

  void IMoveController.Move(Point posCurrent)
  {
    if (!this.IsDragging)
      return;
    var point = posCurrent;
    point.Offset(this.DragOffset.X, this.DragOffset.Y);
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    this.El.Location = point;
  }

  void IMoveController.End() => this.IsDragging = false;

  bool IMoveController.IsMoving => this.IsDragging;

  bool IMoveController.CanMove => this.CanMove1;

  public void SetLabelPosition()
  {
    var label = this.El.Label;
    if (this.El.Lines.Length == 2)
      label.Location = this.El.Lines[0].Point2;
    else
      label.PositionBySite((BaseElement) this.El.Lines[1]);
  }
}