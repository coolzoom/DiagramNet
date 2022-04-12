// Decompiled with JetBrains decompiler
// Type: DiagramNet.Document
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using Elements.Controllers;
using Events;
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
  private Point _location = new(100, 100);
  private Size _windowSize = new(0, 0);
  private float _zoom = 1f;
  private Size _gridSize = new(50, 50);
  private bool _canFireEvents = true;

  public Document()
  {
    SelectedNodes = new ElementCollection();
    SelectedElements = new ElementCollection();
    Elements = new ElementCollection();
  }

  public void AddElement(BaseElement el)
  {
    Elements.Add(el);
    el.AppearanceChanged += new EventHandler(ElementAppearanceChanged);
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void AddElements(ElementCollection els) => AddElements(els.GetArray());

  public void AddElements(BaseElement[] els)
  {
    Elements.EnabledCalc = false;
    foreach (var el in els)
      AddElement(el);
    Elements.EnabledCalc = true;
  }

  internal bool CanAddLink(ConnectorElement connStart, ConnectorElement connEnd) => connStart != connEnd && connStart.ParentElement != connEnd.ParentElement;

  public BaseLinkElement AddLink(
    ConnectorElement connStart,
    ConnectorElement connEnd)
  {
    if (!CanAddLink(connStart, connEnd))
      return (BaseLinkElement) null;
    var element = _linkType != LinkType.Straight ? (BaseLinkElement) new RightAngleLinkElement(connStart, connEnd) : (BaseLinkElement) new StraightLinkElement(connStart, connEnd);
    Elements.Add((BaseElement) element);
    element.AppearanceChanged += new EventHandler(ElementAppearanceChanged);
    OnAppearancePropertyChanged(new EventArgs());
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
        DeleteLink((BaseLinkElement) el);
        return;
      case NodeElement _:
        foreach (var connector in ((NodeElement) el).Connectors)
        {
          for (var index = connector.Links.Count - 1; index >= 0; --index)
            DeleteLink((BaseLinkElement) connector.Links[index]);
        }
        if (SelectedNodes.Contains(el))
        {
          SelectedNodes.Remove(el);
          break;
        }
        break;
    }
    if (SelectedElements.Contains(el))
      SelectedElements.Remove(el);
    Elements.Remove(el);
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void DeleteElement(Point p) => DeleteElement(FindElement(p));

  public void DeleteSelectedElements()
  {
    SelectedElements.EnabledCalc = false;
    SelectedNodes.EnabledCalc = false;
    for (var index = SelectedElements.Count - 1; index >= 0; --index)
      DeleteElement(SelectedElements[index]);
    SelectedElements.EnabledCalc = true;
    SelectedNodes.EnabledCalc = true;
  }

  public void DeleteLink(BaseLinkElement lnk, bool callHandler = true)
  {
    if (lnk == null)
      return;
    lnk.Connector1.RemoveLink(lnk);
    lnk.Connector2.RemoveLink(lnk);
    if (Elements.Contains((BaseElement) lnk))
      Elements.Remove((BaseElement) lnk);
    if (SelectedElements.Contains((BaseElement) lnk))
      SelectedElements.Remove((BaseElement) lnk);
    if (callHandler)
      OnLinkRemoved(new ElementEventArgs((BaseElement) lnk));
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void ClearSelection()
  {
    SelectedElements.Clear();
    SelectedNodes.Clear();
    OnElementSelection((object) this, new ElementSelectionEventArgs(SelectedElements));
  }

  public void SelectElement(BaseElement el)
  {
    SelectedElements.Add(el);
    if (el is NodeElement)
      SelectedNodes.Add(el);
    if (!_canFireEvents)
      return;
    OnElementSelection((object) this, new ElementSelectionEventArgs(SelectedElements));
  }

  public void SelectElements(BaseElement[] els)
  {
    SelectedElements.EnabledCalc = false;
    SelectedNodes.EnabledCalc = false;
    _canFireEvents = false;
    try
    {
      foreach (var el in els)
        SelectElement(el);
    }
    finally
    {
      _canFireEvents = true;
    }
    SelectedElements.EnabledCalc = true;
    SelectedNodes.EnabledCalc = true;
    OnElementSelection((object) this, new ElementSelectionEventArgs(SelectedElements));
  }

  public void SelectElements(Rectangle selectionRectangle)
  {
    SelectedElements.EnabledCalc = false;
    SelectedNodes.EnabledCalc = false;
    foreach (BaseElement element in Elements)
    {
      if (element is IControllable && ((IControllable) element).GetController().HitTest(selectionRectangle))
      {
        if (!(element is ConnectorElement))
          SelectedElements.Add(element);
        if (element is NodeElement)
          SelectedNodes.Add(element);
      }
    }
    if (SelectedElements.Count > 1)
    {
      foreach (BaseElement element1 in Elements)
      {
        if (element1 is BaseLinkElement element2 && (!SelectedElements.Contains((BaseElement) element2.Connector1.ParentElement) || !SelectedElements.Contains((BaseElement) element2.Connector2.ParentElement)))
          SelectedElements.Remove((BaseElement) element2);
      }
    }
    SelectedElements.EnabledCalc = true;
    SelectedNodes.EnabledCalc = true;
    OnElementSelection((object) this, new ElementSelectionEventArgs(SelectedElements));
  }

  public void SelectAllElements()
  {
    SelectedElements.EnabledCalc = false;
    SelectedNodes.EnabledCalc = false;
    foreach (BaseElement element in Elements)
    {
      if (!(element is ConnectorElement))
        SelectedElements.Add(element);
      if (element is NodeElement)
        SelectedNodes.Add(element);
    }
    SelectedElements.EnabledCalc = true;
    SelectedNodes.EnabledCalc = true;
  }

  public BaseElement FindElement(Point point)
  {
    if (Elements != null && Elements.Count > 0)
    {
      for (var index = Elements.Count - 1; index >= 0; --index)
      {
        var element = Elements[index];
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
            var innerElement = FindInnerElement((IContainer) element, point);
            if (innerElement != null)
              return innerElement;
          }
          if (element is IControllable && ((IControllable) element).GetController().HitTest(point))
            return element;
        }
      }
      for (var index = Elements.Count - 1; index >= 0; --index)
      {
        var element = Elements[index];
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
        var innerElement = FindInnerElement((IContainer) element, hitPos);
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
    var i = Elements.IndexOf(el);
    if (i == Elements.Count - 1)
      return;
    Elements.ChangeIndex(i, i + 1);
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void MoveDownElement(BaseElement el)
  {
    var i = Elements.IndexOf(el);
    if (i == 0)
      return;
    Elements.ChangeIndex(i, i - 1);
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void BringToFrontElement(BaseElement el)
  {
    var i = Elements.IndexOf(el);
    for (var y = i + 1; y <= Elements.Count - 1; ++y)
    {
      Elements.ChangeIndex(i, y);
      i = y;
    }
    OnAppearancePropertyChanged(new EventArgs());
  }

  public void SendToBackElement(BaseElement el)
  {
    var i = Elements.IndexOf(el);
    for (var y = i - 1; y >= 0; --y)
    {
      Elements.ChangeIndex(i, y);
      i = y;
    }
    OnAppearancePropertyChanged(new EventArgs());
  }

  internal void CalcWindow(bool forceCalc)
  {
    Elements.CalcWindow(forceCalc);
    SelectedElements.CalcWindow(forceCalc);
    SelectedNodes.CalcWindow(forceCalc);
  }

  public ElementCollection Elements { get; private set; }

  public ElementCollection SelectedElements { get; private set; }

  public ElementCollection SelectedNodes { get; private set; }

  public Point Location => Elements.WindowLocation;

  public Size Size => Elements.WindowSize;

  internal Size WindowSize
  {
    set => _windowSize = value;
  }

  public SmoothingMode SmoothingMode
  {
    get => _smoothingMode;
    set
    {
      _smoothingMode = value;
      OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public PixelOffsetMode PixelOffsetMode
  {
    get => _pixelOffsetMode;
    set
    {
      _pixelOffsetMode = value;
      OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public CompositingQuality CompositingQuality
  {
    get => _compositingQuality;
    set
    {
      _compositingQuality = value;
      OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public DesignerAction Action
  {
    get => _action;
    set
    {
      _action = value;
      OnPropertyChanged(new EventArgs());
    }
  }

  public float Zoom
  {
    get => _zoom;
    set
    {
      if ((double) value < 0.100000001490116)
        value = 0.1f;
      var flag = (double) Math.Abs(_zoom - value) > 1.40129846432482E-45;
      _zoom = value;
      GridSize = new Size(Convert.ToInt32(10f * value), Convert.ToInt32(10f * value));
      OnPropertyChanged(new EventArgs());
      if (!flag || ZoomChanged == null)
        return;
      ZoomChanged((object) this, new EventArgs());
    }
  }

  public ElementType ElementType
  {
    get => _elementType;
    set
    {
      _elementType = value;
      OnPropertyChanged(new EventArgs());
    }
  }

  public LinkType LinkType
  {
    get => _linkType;
    set
    {
      _linkType = value;
      OnPropertyChanged(new EventArgs());
    }
  }

  public Size GridSize
  {
    get => _gridSize;
    set
    {
      _gridSize = value;
      OnAppearancePropertyChanged(new EventArgs());
    }
  }

  public void DrawElementsToGraphics(Graphics g, Rectangle? clippingRegion)
  {
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      var element = Elements[index];
      if (element is BaseLinkElement && (!clippingRegion.HasValue || NeedDrawElement(element, clippingRegion.Value)))
        element.Draw(g);
      if (element is ILabelElement)
        ((ILabelElement) element).Label.Draw(g);
    }
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      var element = Elements[index];
      if (!(element is BaseLinkElement) && (!clippingRegion.HasValue || NeedDrawElement(element, clippingRegion.Value)))
      {
        if (element is NodeElement)
          ((NodeElement) element).Draw(g, _action == DesignerAction.Connect);
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
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      var element = Elements[index];
      if (element is BaseLinkElement && NeedDrawElement(element, clippingRegion))
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
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      var element = Elements[index];
      if (!(element is BaseLinkElement) && NeedDrawElement(element, clippingRegion))
      {
        if (element is NodeElement)
          ((NodeElement) element).Draw(g, _action == DesignerAction.Connect);
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
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      var element = Elements[index];
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
    for (var index = SelectedElements.Count - 1; index >= 0; --index)
    {
      if (SelectedElements[index] is IControllable)
      {
        ((IControllable) SelectedElements[index]).GetController().DrawSelection(g);
        if (SelectedElements[index] is BaseLinkElement)
        {
          var selectedElement = (BaseLinkElement) SelectedElements[index];
          ((IControllable) selectedElement.Connector1).GetController().DrawSelection(g);
          ((IControllable) selectedElement.Connector2).GetController().DrawSelection(g);
        }
      }
    }
  }

  internal void DrawGrid(Graphics g, Rectangle clippingRegion) => DrawGrid(g, clippingRegion, _gridSize, 1f, Size.Width * (int) Zoom, Size.Height * (int) Zoom);

  internal void DrawGrid(
    Graphics g,
    Rectangle clippingRegion,
    Size gridSize,
    float scale,
    int w,
    int h)
  {
    var pen = new Pen((Brush) new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.LightGray, Color.Transparent), 1f);
    var x2 = _location.X + w;
    var y2 = _location.Y + h;
    if ((double) _windowSize.Width / (double) _zoom > (double) x2)
      x2 = (int) ((double) _windowSize.Width / (double) _zoom);
    if ((double) _windowSize.Height / (double) _zoom > (double) y2)
      y2 = (int) ((double) _windowSize.Height / (double) _zoom);
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
    if (PropertyChanged == null)
      return;
    PropertyChanged((object) this, e);
  }

  [field: NonSerialized]
  public event EventHandler AppearancePropertyChanged;

  protected virtual void OnAppearancePropertyChanged(EventArgs e)
  {
    OnPropertyChanged(e);
    if (AppearancePropertyChanged == null)
      return;
    AppearancePropertyChanged((object) this, e);
  }

  [field: NonSerialized]
  public event ElementEventHandler LinkRemoved;

  protected virtual void OnLinkRemoved(ElementEventArgs e)
  {
    if (LinkRemoved == null)
      return;
    LinkRemoved((object) this, e);
  }

  [field: NonSerialized]
  public event EventHandler ElementPropertyChanged;

  protected virtual void OnElementPropertyChanged(object sender, EventArgs e)
  {
    if (ElementPropertyChanged == null)
      return;
    ElementPropertyChanged(sender, e);
  }

  [field: NonSerialized]
  public event ElementSelectionEventHandler ElementSelection;

  protected virtual void OnElementSelection(object sender, ElementSelectionEventArgs e)
  {
    if (ElementSelection == null)
      return;
    ElementSelection(sender, e);
  }

  private void RecreateEventsHandlers()
  {
    foreach (BaseElement element in Elements)
      element.AppearanceChanged += new EventHandler(ElementAppearanceChanged);
  }

  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  private void ElementAppearanceChanged(object sender, EventArgs e) => OnElementPropertyChanged(sender, e);

  void IDeserializationCallback.OnDeserialization(object sender) => RecreateEventsHandlers();

  public int ElementCount()
  {
    var num = 0;
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      if (Elements[index] is DiagramBlock)
        ++num;
    }
    return num;
  }

  public int LinkCount()
  {
    var num = 0;
    for (var index = 0; index <= Elements.Count - 1; ++index)
    {
      if (Elements[index] is BaseLinkElement)
        ++num;
    }
    return num;
  }

  public delegate void ElementEventHandler(object sender, ElementEventArgs e);

  public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
}