// Decompiled with JetBrains decompiler
// Type: DiagramNet.Document
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using DiagramNet.Elements.Controllers;
using DiagramNet.Events;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class Document : IDeserializationCallback
{
  private SmoothingMode _smoothingMode = SmoothingMode.HighQuality;
  private PixelOffsetMode _pixelOffsetMode;
  private CompositingQuality _compositingQuality = CompositingQuality.AssumeLinear;
  private DesignerAction _action;
  private ElementType _elementType = ElementType.RectangleNode;
  private LinkType _linkType = LinkType.RightAngle;
  private Point _location = new Point(100, 100);
  private Size _windowSize = new Size(0, 0);
  private float _zoom = 1f;
  private Size _gridSize = new Size(50, 50);
  private bool _canFireEvents = true;

  public Document()
  {
    this.SelectedNodes = new ElementCollection();
    this.SelectedElements = new ElementCollection();
    this.Elements = new ElementCollection();
  }

  public void AddElement(BaseElement el)
  {
    this.Elements.Add(el);
    el.AppearanceChanged += new EventHandler(this.ElementAppearanceChanged);
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void AddElements(ElementCollection els) => this.AddElements(els.GetArray());

  public void AddElements(BaseElement[] els)
  {
    this.Elements.EnabledCalc = false;
    foreach (var el in els)
      this.AddElement(el);
    this.Elements.EnabledCalc = true;
  }

  internal bool CanAddLink(ConnectorElement connStart, ConnectorElement connEnd) => connStart != connEnd && connStart.ParentElement != connEnd.ParentElement;

  public BaseLinkElement AddLink(
    ConnectorElement connStart,
    ConnectorElement connEnd)
  {
    if (!this.CanAddLink(connStart, connEnd))
      return (BaseLinkElement) null;
    var element = this._linkType != LinkType.Straight ? (BaseLinkElement) new RightAngleLinkElement(connStart, connEnd) : (BaseLinkElement) new StraightLinkElement(connStart, connEnd);
    this.Elements.Add((BaseElement) element);
    element.AppearanceChanged += new EventHandler(this.ElementAppearanceChanged);
    this.OnAppearancePropertyChanged(new EventArgs());
    return element;
  }

  public void DeleteElement(BaseElement el)
  {
    switch (el)
    {
      case null:
        return;
      case ConnectorElement _:
        return;
      case BaseLinkElement _:
        this.DeleteLink((BaseLinkElement) el);
        return;
      case NodeElement _:
        foreach (var connector in ((NodeElement) el).Connectors)
        {
          for (var index = connector.Links.Count - 1; index >= 0; --index)
            this.DeleteLink((BaseLinkElement) connector.Links[index]);
        }
        if (this.SelectedNodes.Contains(el))
        {
          this.SelectedNodes.Remove(el);
          break;
        }
        break;
    }
    if (this.SelectedElements.Contains(el))
      this.SelectedElements.Remove(el);
    this.Elements.Remove(el);
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void DeleteElement(Point p) => this.DeleteElement(this.FindElement(p));

  public void DeleteSelectedElements()
  {
    this.SelectedElements.EnabledCalc = false;
    this.SelectedNodes.EnabledCalc = false;
    for (var index = this.SelectedElements.Count - 1; index >= 0; --index)
      this.DeleteElement(this.SelectedElements[index]);
    this.SelectedElements.EnabledCalc = true;
    this.SelectedNodes.EnabledCalc = true;
  }

  public void DeleteLink(BaseLinkElement lnk, bool callHandler = true)
  {
    if (lnk == null)
      return;
    lnk.Connector1.RemoveLink(lnk);
    lnk.Connector2.RemoveLink(lnk);
    if (this.Elements.Contains((BaseElement) lnk))
      this.Elements.Remove((BaseElement) lnk);
    if (this.SelectedElements.Contains((BaseElement) lnk))
      this.SelectedElements.Remove((BaseElement) lnk);
    if (callHandler)
      this.OnLinkRemoved(new ElementEventArgs((BaseElement) lnk));
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void ClearSelection()
  {
    this.SelectedElements.Clear();
    this.SelectedNodes.Clear();
    this.OnElementSelection((object) this, new ElementSelectionEventArgs(this.SelectedElements));
  }

  public void SelectElement(BaseElement el)
  {
    this.SelectedElements.Add(el);
    if (el is NodeElement)
      this.SelectedNodes.Add(el);
    if (!this._canFireEvents)
      return;
    this.OnElementSelection((object) this, new ElementSelectionEventArgs(this.SelectedElements));
  }

  public void SelectElements(BaseElement[] els)
  {
    this.SelectedElements.EnabledCalc = false;
    this.SelectedNodes.EnabledCalc = false;
    this._canFireEvents = false;
    try
    {
      foreach (var el in els)
        this.SelectElement(el);
    }
    finally
    {
      this._canFireEvents = true;
    }
    this.SelectedElements.EnabledCalc = true;
    this.SelectedNodes.EnabledCalc = true;
    this.OnElementSelection((object) this, new ElementSelectionEventArgs(this.SelectedElements));
  }

  public void SelectElements(Rectangle selectionRectangle)
  {
    this.SelectedElements.EnabledCalc = false;
    this.SelectedNodes.EnabledCalc = false;
    foreach (BaseElement element in this.Elements)
    {
      if (element is IControllable && ((IControllable) element).GetController().HitTest(selectionRectangle))
      {
        if (!(element is ConnectorElement))
          this.SelectedElements.Add(element);
        if (element is NodeElement)
          this.SelectedNodes.Add(element);
      }
    }
    if (this.SelectedElements.Count > 1)
    {
      foreach (BaseElement element1 in this.Elements)
      {
        if (element1 is BaseLinkElement element2 && (!this.SelectedElements.Contains((BaseElement) element2.Connector1.ParentElement) || !this.SelectedElements.Contains((BaseElement) element2.Connector2.ParentElement)))
          this.SelectedElements.Remove((BaseElement) element2);
      }
    }
    this.SelectedElements.EnabledCalc = true;
    this.SelectedNodes.EnabledCalc = true;
    this.OnElementSelection((object) this, new ElementSelectionEventArgs(this.SelectedElements));
  }

  public void SelectAllElements()
  {
    this.SelectedElements.EnabledCalc = false;
    this.SelectedNodes.EnabledCalc = false;
    foreach (BaseElement element in this.Elements)
    {
      if (!(element is ConnectorElement))
        this.SelectedElements.Add(element);
      if (element is NodeElement)
        this.SelectedNodes.Add(element);
    }
    this.SelectedElements.EnabledCalc = true;
    this.SelectedNodes.EnabledCalc = true;
  }

  public BaseElement FindElement(Point point)
  {
    if (this.Elements != null && this.Elements.Count > 0)
    {
      for (var index = this.Elements.Count - 1; index >= 0; --index)
      {
        var element = this.Elements[index];
        if (!(element is BaseLinkElement))
        {
          if (element is NodeElement)
          {
            foreach (var connector in ((NodeElement) element).Connectors)
            {
              if (((IControllable) connector).GetController().HitTest(point))
                return (BaseElement) connector;
            }
          }
          if (element is IContainer)
          {
            var innerElement = this.FindInnerElement((IContainer) element, point);
            if (innerElement != null)
              return innerElement;
          }
          if (element is IControllable && ((IControllable) element).GetController().HitTest(point))
            return element;
        }
      }
      for (var index = this.Elements.Count - 1; index >= 0; --index)
      {
        var element = this.Elements[index];
        if (element is BaseLinkElement && element is IControllable && ((IControllable) element).GetController().HitTest(point))
          return element;
      }
    }
    return (BaseElement) null;
  }

  private BaseElement FindInnerElement(IContainer parent, Point hitPos)
  {
    foreach (BaseElement element in parent.Elements)
    {
      if (element is IContainer)
      {
        var innerElement = this.FindInnerElement((IContainer) element, hitPos);
        if (innerElement != null)
          return innerElement;
      }
      if (element is IControllable && ((IControllable) element).GetController().HitTest(hitPos))
        return element;
    }
    return (BaseElement) null;
  }

  public void MoveUpElement(BaseElement el)
  {
    var i = this.Elements.IndexOf(el);
    if (i == this.Elements.Count - 1)
      return;
    this.Elements.ChangeIndex(i, i + 1);
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void MoveDownElement(BaseElement el)
  {
    var i = this.Elements.IndexOf(el);
    if (i == 0)
      return;
    this.Elements.ChangeIndex(i, i - 1);
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void BringToFrontElement(BaseElement el)
  {
    var i = this.Elements.IndexOf(el);
    for (var y = i + 1; y <= this.Elements.Count - 1; ++y)
    {
      this.Elements.ChangeIndex(i, y);
      i = y;
    }
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  public void SendToBackElement(BaseElement el)
  {
    var i = this.Elements.IndexOf(el);
    for (var y = i - 1; y >= 0; --y)
    {
      this.Elements.ChangeIndex(i, y);
      i = y;
    }
    this.OnAppearancePropertyChanged(new EventArgs());
  }

  internal void CalcWindow(bool forceCalc)
  {
    this.Elements.CalcWindow(forceCalc);
    this.SelectedElements.CalcWindow(forceCalc);
    this.SelectedNodes.CalcWindow(forceCalc);
  }

  public ElementCollection Elements { get; private set; }

  public ElementCollection SelectedElements { get; private set; }

  public ElementCollection SelectedNodes { get; private set; }

  public Point Location => this.Elements.WindowLocation;

  public Size Size => this.Elements.WindowSize;

  internal Size WindowSize
  {
    set => this._windowSize = value;
  }

  public SmoothingMode SmoothingMode
  {
    get => this._smoothingMode;
    set
    {
      this._smoothingMode = value;
      this.OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public PixelOffsetMode PixelOffsetMode
  {
    get => this._pixelOffsetMode;
    set
    {
      this._pixelOffsetMode = value;
      this.OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public CompositingQuality CompositingQuality
  {
    get => this._compositingQuality;
    set
    {
      this._compositingQuality = value;
      this.OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public DesignerAction Action
  {
    get => this._action;
    set
    {
      this._action = value;
      this.OnPropertyChanged(new EventArgs());
    }
  }

  public float Zoom
  {
    get => this._zoom;
    set
    {
      if ((double) value < 0.100000001490116)
        value = 0.1f;
      var flag = (double) Math.Abs(this._zoom - value) > 1.40129846432482E-45;
      this._zoom = value;
      this.GridSize = new Size(Convert.ToInt32(10f * value), Convert.ToInt32(10f * value));
      this.OnPropertyChanged(new EventArgs());
      if (!flag || this.ZoomChanged == null)
        return;
      this.ZoomChanged((object) this, new EventArgs());
    }
  }

  public ElementType ElementType
  {
    get => this._elementType;
    set
    {
      this._elementType = value;
      this.OnPropertyChanged(new EventArgs());
    }
  }

  public LinkType LinkType
  {
    get => this._linkType;
    set
    {
      this._linkType = value;
      this.OnPropertyChanged(new EventArgs());
    }
  }

  public Size GridSize
  {
    get => this._gridSize;
    set
    {
      this._gridSize = value;
      this.OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public void DrawElementsToGraphics(Graphics g, Rectangle? clippingRegion)
  {
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      var element = this.Elements[index];
      if (element is BaseLinkElement && (!clippingRegion.HasValue || this.NeedDrawElement(element, clippingRegion.Value)))
        element.Draw(g);
      if (element is ILabelElement)
        ((ILabelElement) element).Label.Draw(g);
    }
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      var element = this.Elements[index];
      if (!(element is BaseLinkElement) && (!clippingRegion.HasValue || this.NeedDrawElement(element, clippingRegion.Value)))
      {
        if (element is NodeElement)
          ((NodeElement) element).Draw(g, this._action == DesignerAction.Connect);
        else
          element.Draw(g);
        if (element is ILabelElement)
          ((ILabelElement) element).Label.Draw(g);
      }
    }
  }

  public Rectangle DrawElements(Graphics g, Rectangle clippingRegion)
  {
    var rectangle = new Rectangle();
    var flag = false;
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      var element = this.Elements[index];
      if (element is BaseLinkElement && this.NeedDrawElement(element, clippingRegion))
        element.Draw(g);
      if (element is ILabelElement)
        ((ILabelElement) element).Label.Draw(g);
      if (!flag)
      {
        rectangle.X = element.Location.X;
        rectangle.Y = element.Location.Y;
        rectangle.Width = element.Location.X + element.Size.Width + 4;
        rectangle.Height = element.Location.Y + element.Size.Height + 1;
        flag = true;
      }
      if (element.Location.X < rectangle.X)
        rectangle.X = element.Location.X;
      if (element.Location.Y < rectangle.Y)
        rectangle.Y = element.Location.Y;
      if (element.Location.X + element.Size.Width > rectangle.Width)
        rectangle.Width = element.Location.X + element.Size.Width + 4;
      if (element.Location.Y + element.Size.Height > rectangle.Height)
        rectangle.Height = element.Location.Y + element.Size.Height + 1;
    }
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      var element = this.Elements[index];
      if (!(element is BaseLinkElement) && this.NeedDrawElement(element, clippingRegion))
      {
        if (element is NodeElement)
          ((NodeElement) element).Draw(g, this._action == DesignerAction.Connect);
        else
          element.Draw(g);
        if (element is ILabelElement)
          ((ILabelElement) element).Label.Draw(g);
        if (!flag)
        {
          rectangle.X = element.Location.X;
          rectangle.Y = element.Location.Y;
          flag = true;
        }
        if (element.Location.X < rectangle.X)
          rectangle.X = element.Location.X;
        if (element.Location.Y < rectangle.Y)
          rectangle.Y = element.Location.Y;
      }
    }
    return rectangle;
  }

  public Rectangle GetArea()
  {
    var area = new Rectangle();
    var flag = false;
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      var element = this.Elements[index];
      if (!flag)
      {
        area.X = element.Location.X;
        area.Y = element.Location.Y;
        area.Width = element.Location.X + element.Size.Width + 4;
        area.Height = element.Location.Y + element.Size.Height + 1;
        flag = true;
      }
      if (element.Location.X < area.X)
        area.X = element.Location.X;
      if (element.Location.Y < area.Y)
        area.Y = element.Location.Y;
      if (element.Location.X + element.Size.Width > area.Width)
        area.Width = element.Location.X + element.Size.Width + 4;
      if (element.Location.Y + element.Size.Height > area.Height)
        area.Height = element.Location.Y + element.Size.Height + 1;
    }
    return area;
  }

  private bool NeedDrawElement(BaseElement el, Rectangle clippingRegion)
  {
    if (!el.Visible)
      return false;
    var unsignedRectangle = el.GetUnsignedRectangle();
    unsignedRectangle.Inflate(5, 5);
    return clippingRegion.IntersectsWith(unsignedRectangle);
  }

  internal void DrawSelections(Graphics g, Rectangle clippingRegion)
  {
    for (var index = this.SelectedElements.Count - 1; index >= 0; --index)
    {
      if (this.SelectedElements[index] is IControllable)
      {
        ((IControllable) this.SelectedElements[index]).GetController().DrawSelection(g);
        if (this.SelectedElements[index] is BaseLinkElement)
        {
          var selectedElement = (BaseLinkElement) this.SelectedElements[index];
          ((IControllable) selectedElement.Connector1).GetController().DrawSelection(g);
          ((IControllable) selectedElement.Connector2).GetController().DrawSelection(g);
        }
      }
    }
  }

  internal void DrawGrid(Graphics g, Rectangle clippingRegion) => this.DrawGrid(g, clippingRegion, this._gridSize, 1f, this.Size.Width * (int) this.Zoom, this.Size.Height * (int) this.Zoom);

  internal void DrawGrid(
    Graphics g,
    Rectangle clippingRegion,
    Size gridSize,
    float scale,
    int w,
    int h)
  {
    var pen = new Pen((Brush) new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.LightGray, Color.Transparent), 1f);
    var x2 = this._location.X + w;
    var y2 = this._location.Y + h;
    if ((double) this._windowSize.Width / (double) this._zoom > (double) x2)
      x2 = (int) ((double) this._windowSize.Width / (double) this._zoom);
    if ((double) this._windowSize.Height / (double) this._zoom > (double) y2)
      y2 = (int) ((double) this._windowSize.Height / (double) this._zoom);
    for (var index = 0; index < x2; index += gridSize.Width)
      g.DrawLine(pen, index, 0, index, y2);
    for (var index = 0; index < y2; index += gridSize.Height)
      g.DrawLine(pen, 0, index, x2, index);
    pen.Dispose();
  }

  [field: NonSerialized]
  public event EventHandler PropertyChanged;

  [field: NonSerialized]
  public event EventHandler ZoomChanged;

  protected virtual void OnPropertyChanged(EventArgs e)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, e);
  }

  [field: NonSerialized]
  public event EventHandler AppearancePropertyChanged;

  protected virtual void OnAppearancePropertyChanged(EventArgs e)
  {
    this.OnPropertyChanged(e);
    if (this.AppearancePropertyChanged == null)
      return;
    this.AppearancePropertyChanged((object) this, e);
  }

  [field: NonSerialized]
  public event Document.ElementEventHandler LinkRemoved;

  protected virtual void OnLinkRemoved(ElementEventArgs e)
  {
    if (this.LinkRemoved == null)
      return;
    this.LinkRemoved((object) this, e);
  }

  [field: NonSerialized]
  public event EventHandler ElementPropertyChanged;

  protected virtual void OnElementPropertyChanged(object sender, EventArgs e)
  {
    if (this.ElementPropertyChanged == null)
      return;
    this.ElementPropertyChanged(sender, e);
  }

  [field: NonSerialized]
  public event Document.ElementSelectionEventHandler ElementSelection;

  protected virtual void OnElementSelection(object sender, ElementSelectionEventArgs e)
  {
    if (this.ElementSelection == null)
      return;
    this.ElementSelection(sender, e);
  }

  private void RecreateEventsHandlers()
  {
    foreach (BaseElement element in this.Elements)
      element.AppearanceChanged += new EventHandler(this.ElementAppearanceChanged);
  }

  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  private void ElementAppearanceChanged(object sender, EventArgs e) => this.OnElementPropertyChanged(sender, e);

  void IDeserializationCallback.OnDeserialization(object sender) => this.RecreateEventsHandlers();

  public int ElementCount()
  {
    var num = 0;
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      if (this.Elements[index] is DiagramBlock)
        ++num;
    }
    return num;
  }

  public int LinkCount()
  {
    var num = 0;
    for (var index = 0; index <= this.Elements.Count - 1; ++index)
    {
      if (this.Elements[index] is BaseLinkElement)
        ++num;
    }
    return num;
  }

  public delegate void ElementEventHandler(object sender, ElementEventArgs e);

  public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
}