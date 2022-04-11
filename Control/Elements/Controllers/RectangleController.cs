// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.RectangleController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using System.Drawing.Drawing2D;

namespace DiagramNet.Elements.Controllers;

internal class RectangleController : IMoveController, IResizeController, IController
{
  protected const int SelCornerSize = 3;
  protected BaseElement El;
  protected Point DragOffset = new Point(0);
  protected bool IsDragging;
  protected bool CanMove1 = true;
  protected RectangleElement[] SelectionCorner = new RectangleElement[9];
  protected CornerPosition SelCorner = CornerPosition.Nothing;
  protected bool CanResize1 = true;

  public RectangleController(BaseElement element)
  {
    this.El = element;
    for (int index1 = 0; index1 < this.SelectionCorner.Length; ++index1)
    {
      RectangleElement[] selectionCorner = this.SelectionCorner;
      int index2 = index1;
      RectangleElement rectangleElement1 = new RectangleElement(0, 0, 6, 6);
      rectangleElement1.BorderColor = Color.Black;
      rectangleElement1.FillColor1 = Color.White;
      rectangleElement1.FillColor2 = Color.Empty;
      RectangleElement rectangleElement2 = rectangleElement1;
      selectionCorner[index2] = rectangleElement2;
    }
  }

  public BaseElement OwnerElement => this.El;

  public virtual bool HitTest(Point p)
  {
    GraphicsPath graphicsPath = new GraphicsPath();
    Matrix matrix = new Matrix();
    Point location = this.El.Location;
    Size size = this.El.Size;
    graphicsPath.AddRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));
    graphicsPath.Transform(matrix);
    return graphicsPath.IsVisible(p);
  }

  public virtual bool HitTest(Rectangle r)
  {
    GraphicsPath graphicsPath = new GraphicsPath();
    Matrix matrix = new Matrix();
    Point location = this.El.Location;
    Size size = this.El.Size;
    graphicsPath.AddRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));
    graphicsPath.Transform(matrix);
    Rectangle rect = Rectangle.Round(graphicsPath.GetBounds());
    return r.Contains(rect);
  }

  public virtual void DrawSelection(Graphics g)
  {
    Point location = this.El.Location;
    Size size = this.El.Size;
    Rectangle unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(location.X - 2, location.Y - 2, size.Width + 4, size.Height + 4));
    HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Gray, Color.Transparent);
    Pen pen = new Pen((Brush) hatchBrush, 2f);
    g.DrawRectangle(pen, unsignedRectangle);
    pen.Dispose();
    hatchBrush.Dispose();
  }

  void IMoveController.Start(Point posStart)
  {
    Point location = this.El.Location;
    this.DragOffset.X = location.X - posStart.X;
    this.DragOffset.Y = location.Y - posStart.Y;
    this.IsDragging = true;
  }

  bool IMoveController.WillMove(Point posCurrent)
  {
    if (!this.IsDragging)
      return false;
    Point point = posCurrent;
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
    Point point = posCurrent;
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

  public RectangleElement[] Corners => this.SelectionCorner;

  void IResizeController.UpdateCornersPos()
  {
    Rectangle rectangle = new Rectangle(this.El.Location, this.El.Size);
    this.SelectionCorner[7].Location = new Point(rectangle.Location.X - 3, rectangle.Location.Y - 3);
    this.SelectionCorner[8].Location = new Point(rectangle.Location.X + rectangle.Size.Width - 3, rectangle.Location.Y - 3);
    this.SelectionCorner[6].Location = new Point(rectangle.Location.X + rectangle.Size.Width / 2 - 3, rectangle.Location.Y - 3);
    this.SelectionCorner[1].Location = new Point(rectangle.Location.X - 3, rectangle.Location.Y + rectangle.Size.Height - 3);
    this.SelectionCorner[2].Location = new Point(rectangle.Location.X + rectangle.Size.Width - 3, rectangle.Location.Y + rectangle.Size.Height - 3);
    this.SelectionCorner[0].Location = new Point(rectangle.Location.X + rectangle.Size.Width / 2 - 3, rectangle.Location.Y + rectangle.Size.Height - 3);
    this.SelectionCorner[4].Location = new Point(rectangle.Location.X - 3, rectangle.Location.Y + rectangle.Size.Height / 2 - 3);
    this.SelectionCorner[3].Location = new Point(rectangle.Location.X + rectangle.Size.Width / 2 - 3, rectangle.Location.Y + rectangle.Size.Height / 2 - 3);
    this.SelectionCorner[5].Location = new Point(rectangle.Location.X + rectangle.Size.Width - 3, rectangle.Location.Y + rectangle.Size.Height / 2 - 3);
  }

  CornerPosition IResizeController.HitTestCorner(Point p)
  {
    for (int index = 0; index < this.SelectionCorner.Length; ++index)
    {
      if (((IControllable) this.SelectionCorner[index]).GetController().HitTest(p))
        return (CornerPosition) index;
    }
    return CornerPosition.Nothing;
  }

  void IResizeController.Start(Point posStart, CornerPosition corner)
  {
    this.SelCorner = corner;
    this.DragOffset.X = this.SelectionCorner[(int) this.SelCorner].Location.X - posStart.X;
    this.DragOffset.Y = this.SelectionCorner[(int) this.SelCorner].Location.Y - posStart.Y;
  }

  void IResizeController.Resize(Point posCurrent)
  {
    RectangleElement rectangleElement = this.SelectionCorner[(int) this.SelCorner];
    Point point1 = posCurrent;
    point1.Offset(this.DragOffset.X, this.DragOffset.Y);
    if (point1.X < 0)
      point1.X = 0;
    if (point1.Y < 0)
      point1.Y = 0;
    switch (this.SelCorner)
    {
      case CornerPosition.BottomCenter:
        rectangleElement.Location = new Point(rectangleElement.Location.X, point1.Y);
        this.El.Size = new Size(this.El.Size.Width, new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2).Y - this.El.Location.Y);
        break;
      case CornerPosition.BottomLeft:
        rectangleElement.Location = point1;
        Point point2 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        this.El.Size = new Size(this.El.Size.Width - (point2.X - this.El.Location.X), point2.Y - this.El.Location.Y);
        this.El.Location = new Point(point2.X, this.El.Location.Y);
        break;
      case CornerPosition.BottomRight:
        rectangleElement.Location = point1;
        Point point3 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        BaseElement el = this.El;
        int x1 = point3.X;
        Point location = this.El.Location;
        int x2 = location.X;
        int width = x1 - x2;
        int y1 = point3.Y;
        location = this.El.Location;
        int y2 = location.Y;
        int height = y1 - y2;
        Size size = new Size(width, height);
        el.Size = size;
        break;
      case CornerPosition.MiddleLeft:
        rectangleElement.Location = new Point(point1.X, rectangleElement.Location.Y);
        Point point4 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        this.El.Size = new Size(this.El.Size.Width + (this.El.Location.X - point4.X), this.El.Size.Height);
        this.El.Location = new Point(point4.X, this.El.Location.Y);
        break;
      case CornerPosition.MiddleRight:
        rectangleElement.Location = new Point(point1.X, rectangleElement.Location.Y);
        this.El.Size = new Size(new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2).X - this.El.Location.X, this.El.Size.Height);
        break;
      case CornerPosition.TopCenter:
        rectangleElement.Location = new Point(rectangleElement.Location.X, point1.Y);
        Point point5 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        this.El.Size = new Size(this.El.Size.Width, this.El.Size.Height + (this.El.Location.Y - point5.Y));
        this.El.Location = new Point(this.El.Location.X, point5.Y);
        break;
      case CornerPosition.TopLeft:
        rectangleElement.Location = point1;
        Point point6 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        this.El.Size = new Size(this.El.Size.Width + (this.El.Location.X - point6.X), this.El.Size.Height + (this.El.Location.Y - point6.Y));
        this.El.Location = point6;
        break;
      case CornerPosition.TopRight:
        rectangleElement.Location = point1;
        Point point7 = new Point(rectangleElement.Location.X + rectangleElement.Size.Width / 2, rectangleElement.Location.Y + rectangleElement.Size.Height / 2);
        this.El.Size = new Size(point7.X - this.El.Location.X, this.El.Size.Height - (point7.Y - this.El.Location.Y));
        this.El.Location = new Point(this.El.Location.X, point7.Y);
        break;
    }
  }

  void IResizeController.End(Point posEnd)
  {
    if (this.El.Size.Height < 0 || this.El.Size.Width < 0)
    {
      Rectangle unsignedRectangle = this.El.GetUnsignedRectangle();
      this.El.Location = unsignedRectangle.Location;
      this.El.Size = unsignedRectangle.Size;
    }
    this.SelCorner = CornerPosition.Nothing;
    this.DragOffset = Point.Empty;
  }

  bool IResizeController.IsResizing => this.SelCorner != CornerPosition.Nothing;

  bool IResizeController.CanResize => this.CanResize1;
}