// Decompiled with JetBrains decompiler
// Type: DiagramNet.Designer
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using Elements.Controllers;
using Events;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Designer : UserControl
{
  private readonly System.ComponentModel.IContainer components;
  private Document _document = new();
  private MoveAction _moveAction;
  private BaseElement _selectedElement;
  private bool _isMultiSelection;
  private readonly RectangleElement _selectionArea = new(0, 0, 0, 0);
  private IController[] _controllers;
  private BaseElement _mousePointerElement;
  private ResizeAction _resizeAction;
  private bool _isAddSelection;
  private bool _isAddLink;
  private ConnectorElement _connStart;
  private ConnectorElement _connEnd;
  private BaseLinkElement _linkLine;
  private bool _isEditLabel;
  private readonly TextBox _labelTextBox = new();
  private EditLabelAction _editLabelAction;
  [NonSerialized]
  private readonly UndoManager _undo = new(5);

  private BaseElement PreviousSelectedElement { get; set; }

  private BaseElement SelectedElement
  {
    get => _selectedElement;
    set
    {
      PreviousSelectedElement = _selectedElement;
      _selectedElement = value;
    }
  }

  public bool Changed { get; set; }

  public Designer(System.ComponentModel.IContainer components)
  {
    this.components = components;
    InitializeComponent();
    SetStyle(ControlStyles.UserPaint, true);
    SetStyle(ControlStyles.ResizeRedraw, true);
    SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    SetStyle(ControlStyles.DoubleBuffer, true);
    _selectionArea.Opacity = 40;
    _selectionArea.FillColor1 = SystemColors.Control;
    _selectionArea.FillColor2 = Color.Empty;
    _selectionArea.BorderColor = SystemColors.Control;
    _labelTextBox.BorderStyle = BorderStyle.FixedSingle;
    _labelTextBox.Multiline = true;
    _labelTextBox.Hide();
    Controls.Add((Control) _labelTextBox);
    RecreateEventsHandlers();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && components != null)
      components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    SuspendLayout();
    AutoScroll = true;
    BackColor = SystemColors.Window;
    Name = nameof (Designer);
    ResumeLayout(false);
  }

  public new void Invalidate()
  {
    if (_document.Elements.Count > 0)
    {
      for (var index = 0; index <= _document.Elements.Count - 1; ++index)
      {
        var element = _document.Elements[index];
        Invalidate(element);
        if (element is ILabelElement)
          Invalidate((BaseElement) ((ILabelElement) element).Label);
      }
    }
    else
      base.Invalidate();
    AutoScrollMinSize = new Size((int) ((double) (_document.Location.X + _document.Size.Width + 10) * (double) _document.Zoom), (int) ((double) (_document.Location.Y + _document.Size.Height) * (double) _document.Zoom));
  }

  private void Invalidate(BaseElement el, bool force = false)
  {
    if (el == null || !force && !el.IsInvalidated)
      return;
    var rc = Goc2Gsc(el.InvalidateRec);
    rc.Inflate(10, 10);
    Invalidate(rc);
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    var graphics = e.Graphics;
    graphics.PageUnit = GraphicsUnit.Pixel;
    var autoScrollPosition = AutoScrollPosition;
    graphics.TranslateTransform((float) autoScrollPosition.X, (float) autoScrollPosition.Y);
    var transform = graphics.Transform;
    var container = graphics.BeginContainer();
    graphics.SmoothingMode = _document.SmoothingMode;
    graphics.PixelOffsetMode = _document.PixelOffsetMode;
    graphics.CompositingQuality = _document.CompositingQuality;
    graphics.ScaleTransform(_document.Zoom, _document.Zoom);
    var clippingRegion = Gsc2Goc(e.ClipRectangle);
    _document.DrawElements(graphics, clippingRegion);
    if (_resizeAction == null || !_resizeAction.IsResizing)
      _document.DrawSelections(graphics, e.ClipRectangle);
    if (_isMultiSelection || _isAddSelection)
      DrawSelectionRectangle(graphics);
    if (_isAddLink)
    {
      _linkLine.CalcLink();
      _linkLine.Draw(graphics);
    }
    if (_resizeAction != null && (_moveAction == null || !_moveAction.IsMoving))
      _resizeAction.DrawResizeCorner(graphics);
    if (_mousePointerElement != null && _mousePointerElement is IControllable)
      ((IControllable) _mousePointerElement).GetController().DrawSelection(graphics);
    graphics.EndContainer(container);
    graphics.Transform = transform;
    base.OnPaint(e);
  }

  protected override void OnPaintBackground(PaintEventArgs e)
  {
    base.OnPaintBackground(e);
    var graphics = e.Graphics;
    graphics.PageUnit = GraphicsUnit.Pixel;
    var transform = graphics.Transform;
    var container = graphics.BeginContainer();
    var clippingRegion = Gsc2Goc(e.ClipRectangle);
    _document.DrawGrid(graphics, clippingRegion, _document.GridSize, 1f, Size.Width, Size.Height);
    graphics.EndContainer(container);
    graphics.Transform = transform;
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    _document.WindowSize = Size;
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    switch (_document.Action)
    {
      case DesignerAction.Select:
      case DesignerAction.Connect:
        if (e.Button == MouseButtons.Left)
        {
          var point1 = Gsc2Goc(new Point(e.X, e.Y));
          StartResizeElement(point1);
          if (_resizeAction == null || !_resizeAction.IsResizing)
          {
            if (_isEditLabel)
              EndEditLabel();
            SelectedElement = _document.FindElement(point1);
            if (SelectedElement != null)
            {
              OnElementMouseDown(new ElementMouseEventArgs(SelectedElement, e.X, e.Y));
              if (e.Clicks != 2 || !(SelectedElement is ILabelElement))
              {
                if (SelectedElement is ConnectorElement)
                {
                  StartAddLink((ConnectorElement) SelectedElement, point1);
                  SelectedElement = (BaseElement) null;
                }
                else
                  StartSelectElements(SelectedElement, point1);
              }
              else
                break;
            }
            else
            {
              _document.ClearSelection();
              var point2 = Gsc2Goc(new Point(e.X, e.Y));
              _isMultiSelection = true;
              _selectionArea.Visible = true;
              _selectionArea.Location = point2;
              _selectionArea.Size = new Size(0, 0);
              if (_resizeAction != null)
                _resizeAction.ShowResizeCorner(false);
            }
            base.Invalidate();
            break;
          }
          break;
        }
        break;
      case DesignerAction.Add:
        if (e.Button == MouseButtons.Left)
        {
          StartAddElement(Gsc2Goc(new Point(e.X, e.Y)));
          break;
        }
        break;
      case DesignerAction.Delete:
        if (e.Button == MouseButtons.Left)
        {
          DeleteElement(Gsc2Goc(new Point(e.X, e.Y)));
          break;
        }
        break;
    }
    base.OnMouseDown(e);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (e.Button == MouseButtons.None)
    {
      Cursor = Cursors.Arrow;
      var point = Gsc2Goc(new Point(e.X, e.Y));
      if (_resizeAction != null && (_document.Action == DesignerAction.Select || _document.Action == DesignerAction.Connect && _resizeAction.IsResizingLink))
        Cursor = _resizeAction.UpdateResizeCornerCursor(point);
      if (_document.Action == DesignerAction.Connect)
      {
        var element = _document.FindElement(point);
        if (_mousePointerElement != element)
        {
          if (element is ConnectorElement)
          {
            _mousePointerElement = element;
            _mousePointerElement.Invalidate();
            Invalidate(_mousePointerElement, true);
          }
          else if (_mousePointerElement != null)
          {
            _mousePointerElement.Invalidate();
            Invalidate(_mousePointerElement, true);
            _mousePointerElement = (BaseElement) null;
          }
        }
      }
      else
      {
        Invalidate(_mousePointerElement, true);
        _mousePointerElement = (BaseElement) null;
      }
    }
    if (e.Button == MouseButtons.Left)
    {
      var point1 = Gsc2Goc(new Point(e.X, e.Y));
      if (_resizeAction != null && _resizeAction.IsResizing)
      {
        _resizeAction.Resize(point1);
        Invalidate();
      }
      if (_moveAction != null && _moveAction.IsMoving)
      {
        _moveAction.Move(point1);
        Invalidate();
      }
      if (_isMultiSelection || _isAddSelection)
      {
        var point2 = Gsc2Goc(new Point(e.X, e.Y));
        _selectionArea.Size = new Size(point2.X - _selectionArea.Location.X, point2.Y - _selectionArea.Location.Y);
        _selectionArea.Invalidate();
        Invalidate((BaseElement) _selectionArea, true);
      }
      if (_isAddLink)
      {
        SelectedElement = _document.FindElement(point1);
        _linkLine.Connector2 = !(SelectedElement is ConnectorElement) || !_document.CanAddLink(_connStart, (ConnectorElement) SelectedElement) ? _connEnd : (ConnectorElement) SelectedElement;
        ((IMoveController) ((IControllable) _connEnd).GetController()).Move(point1);
        base.Invalidate();
      }
    }
    base.OnMouseMove(e);
  }

  protected override void OnMouseDoubleClick(MouseEventArgs e)
  {
    if (SelectedElement == null)
      return;
    OnElementDoubleClick(new ElementEventArgs(SelectedElement));
    if (_moveAction == null)
      return;
    _moveAction.End();
    _moveAction = (MoveAction) null;
    OnElementMouseUp(new ElementMouseEventArgs(SelectedElement, e.X, e.Y));
    if (Changed)
      AddUndo();
    RestartInitValues();
    base.Invalidate();
    base.OnMouseUp(e);
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    var unsignedRectangle = _selectionArea.GetUnsignedRectangle();
    if (_moveAction != null && _moveAction.IsMoving)
    {
      OnElementClick(new ElementEventArgs(SelectedElement, PreviousSelectedElement));
      _moveAction.End();
      _moveAction = (MoveAction) null;
      OnElementMouseUp(new ElementMouseEventArgs(SelectedElement, e.X, e.Y));
      if (Changed)
        AddUndo();
      CheckControlClick();
    }
    if (_isMultiSelection)
      EndSelectElements(unsignedRectangle);
    else if (_isAddSelection)
      EndAddElement(unsignedRectangle);
    else if (_isAddLink)
    {
      EndAddLink();
      AddUndo();
    }
    if (_resizeAction != null)
    {
      if (_resizeAction.IsResizing)
      {
        _resizeAction.End(Gsc2Goc(new Point(e.X, e.Y)));
        AddUndo();
      }
      _resizeAction.UpdateResizeCorner();
    }
    RestartInitValues();
    base.Invalidate();
    Invalidate();
    base.OnMouseUp(e);
  }

  private void CheckControlClick()
  {
    if ((ModifierKeys & Keys.Control) != Keys.Control || SelectedElement == null || PreviousSelectedElement == null || !(SelectedElement is NodeElement) || !(PreviousSelectedElement is NodeElement))
      return;
    var connStart = ((IEnumerable<ConnectorElement>) ((NodeElement) PreviousSelectedElement).Connectors).OrderBy<ConnectorElement, int>((Func<ConnectorElement, int>) (c => c.Links.Count)).FirstOrDefault<ConnectorElement>((Func<ConnectorElement, bool>) (c => !c.IsStart));
    var connEnd = ((IEnumerable<ConnectorElement>) ((NodeElement) SelectedElement).Connectors).FirstOrDefault<ConnectorElement>((Func<ConnectorElement, bool>) (c => c.IsStart && c.Links.Count == 0));
    if (connStart == null || connEnd == null)
      return;
    Document.AddLink(connStart, connEnd);
  }

  [Category("Element")]
  public event ElementEventHandler ElementClick;

  protected virtual void OnElementClick(ElementEventArgs e)
  {
    if (ElementClick == null)
      return;
    ElementClick((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler ElementDoubleClick;

  protected virtual void OnElementDoubleClick(ElementEventArgs e)
  {
    if (ElementDoubleClick == null)
      return;
    ElementDoubleClick((object) this, e);
  }

  [Category("Element")]
  public event ElementMouseEventHandler ElementMouseDown;

  protected virtual void OnElementMouseDown(ElementMouseEventArgs e)
  {
    if (ElementMouseDown == null)
      return;
    ElementMouseDown((object) this, e);
  }

  [Category("Element")]
  public event ElementMouseEventHandler ElementMouseUp;

  protected virtual void OnElementMouseUp(ElementMouseEventArgs e)
  {
    if (ElementMouseUp == null)
      return;
    ElementMouseUp((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler ElementMoving;

  protected virtual void OnElementMoving(ElementEventArgs e)
  {
    if (ElementMoving == null)
      return;
    ElementMoving((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler ElementMoved;

  protected virtual void OnElementMoved(ElementEventArgs e)
  {
    if (ElementMoved == null)
      return;
    ElementMoved((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler ElementResizing;

  protected virtual void OnElementResizing(ElementEventArgs e)
  {
    if (ElementResizing == null)
      return;
    ElementResizing((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler ElementResized;

  protected virtual void OnElementResized(ElementEventArgs e)
  {
    if (ElementResized == null)
      return;
    ElementResized((object) this, e);
  }

  [Category("Element")]
  public event ElementConnectEventHandler ElementConnecting;

  protected virtual void OnElementConnecting(ElementConnectEventArgs e)
  {
    if (ElementConnecting == null)
      return;
    ElementConnecting((object) this, e);
  }

  [Category("Element")]
  public event ElementConnectEventHandler ElementConnected;

  protected virtual void OnElementConnected(ElementConnectEventArgs e)
  {
    if (ElementConnected == null)
      return;
    ElementConnected((object) this, e);
  }

  [Category("Element")]
  public event ElementSelectionEventHandler ElementSelection;

  protected virtual void OnElementSelection(ElementSelectionEventArgs e)
  {
    if (ElementSelection == null)
      return;
    ElementSelection((object) this, e);
  }

  [Category("Element")]
  public event ElementEventHandler LinkRemoved;

  protected virtual void OnLinkRemoved(ElementEventArgs e)
  {
    if (LinkRemoved == null)
      return;
    LinkRemoved((object) this, e);
  }

  private void DocumentPropertyChanged(object sender, EventArgs e)
  {
    if (IsChanging())
      return;
    base.Invalidate();
  }

  private void DocumentAppearancePropertyChanged(object sender, EventArgs e)
  {
    if (IsChanging())
      return;
    AddUndo();
    base.Invalidate();
  }

  private void DocumentElementPropertyChanged(object sender, EventArgs e)
  {
    Changed = true;
    if (IsChanging())
      return;
    AddUndo();
    base.Invalidate();
  }

  private void DocumentElementSelection(object sender, ElementSelectionEventArgs e) => OnElementSelection(e);

  private void DocumentLinkRemoved(object sender, ElementEventArgs e) => OnLinkRemoved(e);

  public Document Document => _document;

  public bool CanUndo => _undo.CanUndo;

  public bool CanRedo => _undo.CanRedo;

  private bool IsChanging()
  {
    if (_moveAction != null && _moveAction.IsMoving || _isAddLink || _isMultiSelection)
      return true;
    return _resizeAction != null && _resizeAction.IsResizing;
  }

  public Point Gsc2Goc(Point gsp)
  {
    var zoom = _document.Zoom;
    gsp.X = (int) ((double) (gsp.X - AutoScrollPosition.X) / (double) zoom);
    gsp.Y = (int) ((double) (gsp.Y - AutoScrollPosition.Y) / (double) zoom);
    return gsp;
  }

  public Rectangle Gsc2Goc(Rectangle gsr)
  {
    var zoom = _document.Zoom;
    gsr.X = (int) ((double) (gsr.X - AutoScrollPosition.X) / (double) zoom);
    gsr.Y = (int) ((double) (gsr.Y - AutoScrollPosition.Y) / (double) zoom);
    gsr.Width = (int) ((double) gsr.Width / (double) zoom);
    gsr.Height = (int) ((double) gsr.Height / (double) zoom);
    return gsr;
  }

  public Rectangle Goc2Gsc(Rectangle gsr)
  {
    var zoom = _document.Zoom;
    gsr.X = (int) ((double) (gsr.X + AutoScrollPosition.X) * (double) zoom);
    gsr.Y = (int) ((double) (gsr.Y + AutoScrollPosition.Y) * (double) zoom);
    gsr.Width = (int) ((double) gsr.Width * (double) zoom);
    gsr.Height = (int) ((double) gsr.Height * (double) zoom);
    return gsr;
  }

  internal void DrawSelectionRectangle(Graphics g) => _selectionArea.Draw(g);

  public void SaveBinary(string fileName)
  {
    var formatter = (IFormatter) new BinaryFormatter();
    var serializationStream = (Stream) new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
    formatter.Serialize(serializationStream, (object) _document);
    serializationStream.Close();
  }

  public Image GetThumbnail()
  {
    var image = GetImage(false, true);
    var num = Math.Min(170f / (float) image.Width, 120f / (float) image.Height);
    if ((double) num > 1.0)
      num = 1f;
    var thumbnail = new Bitmap(180, 130);
    var graphics = Graphics.FromImage((Image) thumbnail);
    graphics.InterpolationMode = InterpolationMode.High;
    graphics.CompositingQuality = CompositingQuality.HighQuality;
    graphics.SmoothingMode = SmoothingMode.AntiAlias;
    var width = (int) ((double) image.Width * (double) num);
    var height = (int) ((double) image.Height * (double) num);
    var x = (170 - width) / 2 + 5;
    var y = (120 - height) / 2 + 5;
    graphics.FillRectangle((Brush) new SolidBrush(Color.White), new Rectangle(0, 0, 180, 130));
    graphics.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), new Rectangle(0, 0, 179, 129));
    graphics.DrawImage((Image) image, new Rectangle(x, y, width, height));
    var memoryStream = new MemoryStream();
    thumbnail.Save((Stream) memoryStream, ImageFormat.Png);
    return (Image) thumbnail;
  }

  public void OpenBinary(string fileName)
  {
    var formatter = (IFormatter) new BinaryFormatter();
    var serializationStream = (Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    _document = (Document) formatter.Deserialize(serializationStream);
    serializationStream.Close();
    RecreateEventsHandlers();
  }

  public void SetDocument(Document document)
  {
    _document = document;
    RecreateEventsHandlers();
  }

  public void Copy()
  {
    if (_document.SelectedElements.Count == 0)
      return;
    var formatter = (IFormatter) new BinaryFormatter();
    var stream = (Stream) new MemoryStream();
    var arrayClone = _document.SelectedElements.GetArrayClone();
    foreach (var baseElement in arrayClone)
    {
      if (baseElement is NodeElement && ((NodeElement) baseElement).Connectors != null)
      {
        foreach (var connector in ((NodeElement) baseElement).Connectors)
          connector.Links = new ElementCollection();
      }
    }
    formatter.Serialize(stream, (object) arrayClone);
    Clipboard.SetDataObject((object) new DataObject(DataFormats.GetFormat("Diagram.NET Element Collection").Name, (object) stream));
  }

  public void Paste()
  {
    _undo.Enabled = false;
    var dataObject = Clipboard.GetDataObject();
    var format = DataFormats.GetFormat("Diagram.NET Element Collection");
    if (dataObject != null && dataObject.GetDataPresent(format.Name))
    {
      var formatter = (IFormatter) new BinaryFormatter();
      var data = (Stream) dataObject.GetData(format.Name);
      var els = (BaseElement[]) formatter.Deserialize(data);
      data.Close();
      foreach (var baseElement in els)
      {
        if (baseElement is NodeElement)
        {
          foreach (var connector in (baseElement as NodeElement).Connectors)
            connector.Links = new ElementCollection();
        }
        baseElement.Location = new Point(baseElement.Location.X + 20, baseElement.Location.Y + 20);
      }
      _document.AddElements(els);
      _document.ClearSelection();
      _document.SelectElements(els);
    }
    _undo.Enabled = true;
    AddUndo();
    EndGeneralAction();
  }

  public void Cut()
  {
    Copy();
    DeleteSelectedElements();
    EndGeneralAction();
  }

  private void EndGeneralAction()
  {
    RestartInitValues();
    if (_resizeAction == null)
      return;
    _resizeAction.ShowResizeCorner(false);
  }

  private void RestartInitValues()
  {
    _moveAction = (MoveAction) null;
    _isMultiSelection = false;
    _isAddSelection = false;
    _isAddLink = false;
    Changed = false;
    _connStart = (ConnectorElement) null;
    _selectionArea.FillColor1 = SystemColors.Control;
    _selectionArea.BorderColor = SystemColors.Control;
    _selectionArea.Visible = false;
    _document.CalcWindow(true);
  }

  private void StartSelectElements(BaseElement selectedElem, Point mousePoint)
  {
    if (!_document.SelectedElements.Contains(selectedElem))
    {
      if ((ModifierKeys & Keys.Shift) != Keys.Shift)
        _document.ClearSelection();
      _document.SelectElement(selectedElem);
    }
    else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
      _document.SelectedElements.Remove(selectedElem);
    Changed = false;
    _moveAction = new MoveAction();
    var onElementMovingDelegate = new MoveAction.OnElementMovingDelegate(OnElementMoving);
    _moveAction.Start(mousePoint, _document, onElementMovingDelegate);
    _controllers = new IController[_document.SelectedElements.Count];
    for (var index = _document.SelectedElements.Count - 1; index >= 0; --index)
      _controllers[index] = !(_document.SelectedElements[index] is IControllable) ? (IController) null : ((IControllable) _document.SelectedElements[index]).GetController();
    _resizeAction = new ResizeAction();
    _resizeAction.Select(_document);
  }

  private void EndSelectElements(Rectangle selectionRectangle) => _document.SelectElements(selectionRectangle);

  private void StartResizeElement(Point mousePoint)
  {
    if (_resizeAction == null || _document.Action != DesignerAction.Select && (_document.Action != DesignerAction.Connect || !_resizeAction.IsResizingLink))
      return;
    var onElementResizingDelegate = new ResizeAction.OnElementResizingDelegate(OnElementResizing);
    _resizeAction.Start(mousePoint, onElementResizingDelegate);
    if (_resizeAction.IsResizing)
      return;
    _resizeAction = (ResizeAction) null;
  }

  private void StartAddLink(ConnectorElement connectorStart, Point mousePoint)
  {
    if (_document.Action != DesignerAction.Connect)
      return;
    _connStart = connectorStart;
    var connectorElement = new ConnectorElement(connectorStart.ParentElement);
    connectorElement.Location = connectorStart.Location;
    _connEnd = connectorElement;
    ((IMoveController) ((IControllable) _connEnd).GetController()).Start(mousePoint);
    _isAddLink = true;
    switch (_document.LinkType)
    {
      case LinkType.Straight:
        _linkLine = (BaseLinkElement) new StraightLinkElement(connectorStart, _connEnd);
        break;
      case LinkType.RightAngle:
        _linkLine = (BaseLinkElement) new RightAngleLinkElement(connectorStart, _connEnd);
        break;
    }
    _linkLine.Visible = true;
    _linkLine.BorderColor = Color.FromArgb(150, Color.Black);
    _linkLine.BorderWidth = 1;
    Invalidate((BaseElement) _linkLine, true);
    OnElementConnecting(new ElementConnectEventArgs(connectorStart.ParentElement, (NodeElement) null, _linkLine));
  }

  private void EndAddLink()
  {
    if (_connEnd != _linkLine.Connector2)
    {
      _linkLine.Connector1.RemoveLink(_linkLine);
      _linkLine = _document.AddLink(_linkLine.Connector1, _linkLine.Connector2);
      var e = new ElementConnectEventArgs(_linkLine.Connector1.ParentElement, _linkLine.Connector2.ParentElement, _linkLine);
      var flag = true;
      if (_linkLine.Connector1.ParentElement is DiagramBlock)
        flag = (_linkLine.Connector1.ParentElement as DiagramBlock).OnElementConnected(this, e);
      if (flag)
        OnElementConnected(e);
    }
    _connStart = (ConnectorElement) null;
    _connEnd = (ConnectorElement) null;
    _linkLine = (BaseLinkElement) null;
  }

  private void StartAddElement(Point mousePoint)
  {
    _document.ClearSelection();
    _selectionArea.FillColor1 = Color.LightSteelBlue;
    _selectionArea.BorderColor = Color.WhiteSmoke;
    _isAddSelection = true;
    _selectionArea.Visible = true;
    _selectionArea.Location = mousePoint;
    _selectionArea.Size = new Size(0, 0);
  }

  private void EndAddElement(Rectangle selectionRectangle)
  {
    BaseElement el;
    switch (_document.ElementType)
    {
      case ElementType.Rectangle:
        el = (BaseElement) new RectangleElement(selectionRectangle);
        break;
      case ElementType.RectangleNode:
        el = (BaseElement) new RectangleNode(selectionRectangle);
        break;
      case ElementType.Ellipse:
        el = (BaseElement) new EllipseElement(selectionRectangle);
        break;
      case ElementType.EllipseNode:
        el = (BaseElement) new EllipseNode(selectionRectangle);
        break;
      case ElementType.CommentBox:
        el = (BaseElement) new CommentBoxElement(selectionRectangle);
        break;
      default:
        el = (BaseElement) new RectangleNode(selectionRectangle);
        break;
    }
    _document.AddElement(el);
    _document.Action = DesignerAction.Select;
  }

  private void EndEditLabel()
  {
    if (_editLabelAction != null)
    {
      _editLabelAction.EndEdit();
      _editLabelAction = (EditLabelAction) null;
    }
    _isEditLabel = false;
  }

  private void DeleteElement(Point mousePoint)
  {
    _document.DeleteElement(mousePoint);
    SelectedElement = (BaseElement) null;
    _document.Action = DesignerAction.Select;
  }

  private void DeleteSelectedElements() => _document.DeleteSelectedElements();

  public void Undo()
  {
    _document = (Document) _undo.Undo();
    RecreateEventsHandlers();
    if (_resizeAction != null)
      _resizeAction.UpdateResizeCorner();
    base.Invalidate();
  }

  public void Redo()
  {
    _document = (Document) _undo.Redo();
    RecreateEventsHandlers();
    if (_resizeAction != null)
      _resizeAction.UpdateResizeCorner();
    base.Invalidate();
  }

  private void AddUndo() => _undo.AddUndo((object) _document);

  private void RecreateEventsHandlers()
  {
    _document.PropertyChanged += new EventHandler(DocumentPropertyChanged);
    _document.AppearancePropertyChanged += new EventHandler(DocumentAppearancePropertyChanged);
    _document.ElementPropertyChanged += new EventHandler(DocumentElementPropertyChanged);
    _document.ElementSelection += new Document.ElementSelectionEventHandler(DocumentElementSelection);
    _document.LinkRemoved += new Document.ElementEventHandler(DocumentLinkRemoved);
  }

  private void MoveElement(Keys key)
  {
    var num = (ModifierKeys & Keys.Shift) == Keys.Shift ? 10 : 1;
    foreach (BaseElement selectedElement in Document.SelectedElements)
    {
      var location = selectedElement.Location;
      if ((key & Keys.Down) == Keys.Down)
        location.Y += num;
      else if ((key & Keys.Right) == Keys.Right)
        location.X += num;
      else if ((key & Keys.Up) == Keys.Up)
        location.Y -= num;
      else if ((key & Keys.Left) == Keys.Left)
        location.X -= num;
      selectedElement.Location = location;
    }
    Refresh();
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (e.KeyCode == Keys.Delete)
    {
      DeleteSelectedElements();
      EndGeneralAction();
      base.Invalidate();
    }
    if (e.Control && e.KeyCode == Keys.Z && _undo.CanUndo)
      Undo();
    if (e.Control && e.KeyCode == Keys.C)
      Copy();
    if (e.Control && e.KeyCode == Keys.V)
      Paste();
    if (e.Control && e.KeyCode == Keys.X)
      Cut();
    if (e.Control && e.KeyCode == Keys.A)
    {
      Document.SelectAllElements();
      Refresh();
    }
    base.OnKeyDown(e);
  }

  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {
    if (msg.Msg == 256 || msg.Msg == 260)
    {
      if ((ModifierKeys & Keys.Control) != Keys.Control)
      {
        if ((keyData & Keys.Up) == Keys.Up || (keyData & Keys.Down) == Keys.Down || (keyData & Keys.Right) == Keys.Right || (keyData & Keys.Left) == Keys.Left)
          MoveElement(keyData);
      }
      else
      {
        if ((keyData & Keys.Down) == Keys.Down)
        {
          foreach (BaseElement selectedElement in Document.SelectedElements)
            Document.SendToBackElement(selectedElement);
        }
        if ((keyData & Keys.Up) == Keys.Up)
        {
          foreach (BaseElement selectedElement in Document.SelectedElements)
            Document.BringToFrontElement(selectedElement);
        }
      }
    }
    return base.ProcessCmdKey(ref msg, keyData);
  }

  public Bitmap GetImage(bool drawGrid, bool whitebackground)
  {
    var area = Document.GetArea();
    var image = new Bitmap(area.Width - area.X, area.Height - area.Y);
    var g = Graphics.FromImage((Image) image);
    if (whitebackground || drawGrid)
      g.FillRectangle((Brush) new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
    if (drawGrid)
      Document.DrawGrid(g, new Rectangle(0, 0, Document.Size.Width + Document.Location.X, Document.Size.Height + Document.Location.Y));
    g.TranslateTransform((float) (area.X * -1), (float) (area.Y * -1));
    Document.DrawElementsToGraphics(g, new Rectangle?());
    return image;
  }

  public bool DrawGraphics(
    Graphics graphics,
    bool drawGrid,
    bool whitebackground,
    int x,
    int y,
    int width,
    int height,
    bool scaleToFitPaper,
    bool allowStretch,
    int pageNumber)
  {
    var num = 1f;
    graphics.SetClip(new Rectangle(-5, -5, width + 5, height + 5));
    var area = Document.GetArea();
    graphics.FillRectangle((Brush) new SolidBrush(Color.White), 0, 0, width, height);
    if (scaleToFitPaper && area.Width > 0 && area.Height > 0)
    {
      num = Math.Min((float) width * 1f / (float) (area.Width - area.X), (float) height * 1f / (float) (area.Height - area.Y));
      if (!allowStretch)
        num = Math.Min(num, 1f);
    }
    if (drawGrid)
      Document.DrawGrid(graphics, new Rectangle(), new Size(Convert.ToInt32(10f * num), Convert.ToInt32(10f * num)), num, width, height);
    if (scaleToFitPaper && area.Width > 0 && area.Height > 0)
      graphics.ScaleTransform(num, num);
    if (pageNumber > 0)
      graphics.TranslateTransform((float) ((area.X + width * pageNumber) * -1), (float) ((area.Y + height * pageNumber) * -1));
    else
      graphics.TranslateTransform((float) (area.X * -1), (float) (area.Y * -1));
    Document.DrawElementsToGraphics(graphics, new Rectangle?());
    return !scaleToFitPaper && (area.X + width * (pageNumber + 1) <= area.Width + area.X || area.Y + height * (pageNumber + 1) <= area.Height + area.Y);
  }

  public delegate void ElementEventHandler(object sender, ElementEventArgs e);

  public delegate void ElementMouseEventHandler(object sender, ElementMouseEventArgs e);

  public delegate void ElementConnectEventHandler(object sender, ElementConnectEventArgs e);

  public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
}