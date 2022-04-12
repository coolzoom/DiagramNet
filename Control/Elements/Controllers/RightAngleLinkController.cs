// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.RightAngleLinkController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

using System.Drawing.Drawing2D;

internal class RightAngleLinkController : 
  IMoveController,
  IResizeController,
  IController,
  ILabelController
{
  protected const int SelCornerSize = 3;
  protected RightAngleLinkElement El;
  protected Point DragOffset = new(0);
  protected bool IsDragging;
  protected bool CanMove1 = true;
  protected RectangleElement[] SelectionCorner;
  protected CornerPosition SelCorner = CornerPosition.Nothing;
  protected bool CanResize1 = true;

  public RightAngleLinkController(RightAngleLinkElement element)
  {
    El = element;
    if (El.LineElements.Length == 3)
    {
      SelectionCorner = new RectangleElement[1];
      var selectionCorner = SelectionCorner;
      var rectangleElement1 = new RectangleElement(0, 0, 6, 6);
      rectangleElement1.BorderColor = Color.Black;
      rectangleElement1.FillColor1 = Color.White;
      rectangleElement1.FillColor2 = Color.Empty;
      var rectangleElement2 = rectangleElement1;
      selectionCorner[0] = rectangleElement2;
    }
    else
      SelectionCorner = new RectangleElement[0];
  }

  public BaseElement OwnerElement => (BaseElement) El;

  public bool HitTest(Point p) => ((IEnumerable<LineElement>) El.LineElements).Select<LineElement, IController>((Func<LineElement, IController>) (l => ((IControllable) l).GetController())).Any<IController>((Func<IController, bool>) (ctrl => ctrl.HitTest(p)));

  bool IController.HitTest(Rectangle r)
  {
    var graphicsPath = new GraphicsPath();
    var matrix = new Matrix();
    var location = El.Location;
    var size = El.Size;
    graphicsPath.AddRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));
    graphicsPath.Transform(matrix);
    var rect = Rectangle.Round(graphicsPath.GetBounds());
    return r.Contains(rect);
  }

  public void DrawSelection(Graphics g)
  {
    foreach (IControllable lineElement in El.LineElements)
      lineElement.GetController().DrawSelection(g);
  }

  public RectangleElement[] Corners => SelectionCorner;

  void IResizeController.UpdateCornersPos()
  {
    if (SelectionCorner.Length != 1)
      return;
    var point1 = El.LineElements[1].Point1;
    var point2 = El.LineElements[1].Point2;
    SelectionCorner[0].Location = new Point(point1.X + (point2.X - point1.X) / 2 - 3, point1.Y + (point2.Y - point1.Y) / 2 - 3);
  }

  CornerPosition IResizeController.HitTestCorner(Point p)
  {
    if (SelectionCorner.Length != 1 || !((IControllable) SelectionCorner[0]).GetController().HitTest(p))
      return CornerPosition.Nothing;
    if (El.Orientation == Orientation.Horizontal)
      return CornerPosition.MiddleLeft;
    return El.Orientation != Orientation.Vertical ? CornerPosition.Undefined : CornerPosition.TopCenter;
  }

  void IResizeController.Start(Point posStart, CornerPosition corner)
  {
    SelCorner = corner;
    DragOffset.X = SelectionCorner[0].Location.X - posStart.X;
    DragOffset.Y = SelectionCorner[0].Location.Y - posStart.Y;
  }

  void IResizeController.Resize(Point posCurrent)
  {
    var rectangleElement = SelectionCorner[0];
    var point1 = posCurrent;
    point1.Offset(DragOffset.X, DragOffset.Y);
    if (point1.X < 0)
      point1.X = 0;
    if (point1.Y < 0)
      point1.Y = 0;
    if (El.Orientation == Orientation.Horizontal)
    {
      rectangleElement.Location = new Point(point1.X, rectangleElement.Location.Y);
      var point2 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
      El.LineElements[1].Point1 = new Point(point2.X, El.LineElements[1].Point1.Y);
      El.LineElements[1].Point2 = new Point(point2.X, El.LineElements[1].Point2.Y);
    }
    else
    {
      rectangleElement.Location = new Point(rectangleElement.Location.X, point1.Y);
      var point3 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
      El.LineElements[1].Point1 = new Point(El.LineElements[1].Point1.X, point3.Y);
      El.LineElements[1].Point2 = new Point(El.LineElements[1].Point2.X, point3.Y);
    }
    El.LineElements[0].Point2 = El.LineElements[1].Point1;
    El.LineElements[2].Point1 = El.LineElements[1].Point2;
    El.NeedCalcLink = true;
  }

  void IResizeController.End(Point posEnd)
  {
    SelCorner = CornerPosition.Nothing;
    DragOffset = Point.Empty;
  }

  bool IResizeController.IsResizing => SelCorner != CornerPosition.Nothing;

  bool IResizeController.CanResize => CanResize1;

  void IMoveController.Start(Point posStart)
  {
    DragOffset.X = El.Location.X - posStart.X;
    DragOffset.Y = El.Location.Y - posStart.Y;
    IsDragging = true;
  }

  bool IMoveController.WillMove(Point posCurrent)
  {
    if (!IsDragging)
      return false;
    var point = posCurrent;
    point.Offset(DragOffset.X, DragOffset.Y);
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    return El.Location.X != point.X || El.Location.Y != point.Y;
  }

  void IMoveController.Move(Point posCurrent)
  {
    if (!IsDragging)
      return;
    var point = posCurrent;
    point.Offset(DragOffset.X, DragOffset.Y);
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    El.Location = point;
  }

  void IMoveController.End() => IsDragging = false;

  bool IMoveController.IsMoving => IsDragging;

  bool IMoveController.CanMove => CanMove1;

  public void SetLabelPosition()
  {
    var label = El.Label;
    if (El.Lines.Length == 2)
      label.Location = El.Lines[0].Point2;
    else
      label.PositionBySite((BaseElement) El.Lines[1]);
  }
}