// Decompiled with JetBrains decompiler
// Type: DiagramNet.Designer
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using DiagramNet.Elements.Controllers;
using DiagramNet.Events;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Designer : UserControl
{
  private readonly System.ComponentModel.IContainer components;
  private Document _document = new Document();
  private MoveAction _moveAction;
  private BaseElement _selectedElement;
  private bool _isMultiSelection;
  private readonly RectangleElement _selectionArea = new RectangleElement(0, 0, 0, 0);
  private IController[] _controllers;
  private BaseElement _mousePointerElement;
  private ResizeAction _resizeAction;
  private bool _isAddSelection;
  private bool _isAddLink;
  private ConnectorElement _connStart;
  private ConnectorElement _connEnd;
  private BaseLinkElement _linkLine;
  private bool _isEditLabel;
  private readonly TextBox _labelTextBox = new TextBox();
  private EditLabelAction _editLabelAction;
  [NonSerialized]
  private readonly UndoManager _undo = new UndoManager(5);

  private BaseElement PreviousSelectedElement { get; set; }

  private BaseElement SelectedElement
  {
    get => this._selectedElement;
    set
    {
      this.PreviousSelectedElement = this._selectedElement;
      this._selectedElement = value;
    }
  }

  public bool Changed { get; set; }

  public Designer(System.ComponentModel.IContainer components)
  {
    this.components = components;
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint, true);
    this.SetStyle(ControlStyles.ResizeRedraw, true);
    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    this.SetStyle(ControlStyles.DoubleBuffer, true);
    this._selectionArea.Opacity = 40;
    this._selectionArea.FillColor1 = SystemColors.Control;
    this._selectionArea.FillColor2 = Color.Empty;
    this._selectionArea.BorderColor = SystemColors.Control;
    this._labelTextBox.BorderStyle = BorderStyle.FixedSingle;
    this._labelTextBox.Multiline = true;
    this._labelTextBox.Hide();
    this.Controls.Add((Control) this._labelTextBox);
    this.RecreateEventsHandlers();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScroll = true;
    this.BackColor = SystemColors.Window;
    this.Name = nameof (Designer);
    this.ResumeLayout(false);
  }

  public new void Invalidate()
  {
    if (this._document.Elements.Count > 0)
    {
      for (var index = 0; index <= this._document.Elements.Count - 1; ++index)
      {
        var element = this._document.Elements[index];
        this.Invalidate(element);
        if (element is ILabelElement)
          this.Invalidate((BaseElement) ((ILabelElement) element).Label);
      }
    }
    else
      base.Invalidate();
    this.AutoScrollMinSize = new Size((int) ((double) (this._document.Location.X + this._document.Size.Width + 10) * (double) this._document.Zoom), (int) ((double) (this._document.Location.Y + this._document.Size.Height) * (double) this._document.Zoom));
  }

  private void Invalidate(BaseElement el, bool force = false)
  {
    if (el == null || !force && !el.IsInvalidated)
      return;
    var rc = this.Goc2Gsc(el.InvalidateRec);
    rc.Inflate(10, 10);
    this.Invalidate(rc);
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    var graphics = e.Graphics;
    graphics.PageUnit = GraphicsUnit.Pixel;
    var autoScrollPosition = this.AutoScrollPosition;
    graphics.TranslateTransform((float) autoScrollPosition.X, (float) autoScrollPosition.Y);
    var transform = graphics.Transform;
    var container = graphics.BeginContainer();
    graphics.SmoothingMode = this._document.SmoothingMode;
    graphics.PixelOffsetMode = this._document.PixelOffsetMode;
    graphics.CompositingQuality = this._document.CompositingQuality;
    graphics.ScaleTransform(this._document.Zoom, this._document.Zoom);
    var clippingRegion = this.Gsc2Goc(e.ClipRectangle);
    this._document.DrawElements(graphics, clippingRegion);
    if (this._resizeAction == null || !this._resizeAction.IsResizing)
      this._document.DrawSelections(graphics, e.ClipRectangle);
    if (this._isMultiSelection || this._isAddSelection)
      this.DrawSelectionRectangle(graphics);
    if (this._isAddLink)
    {
      this._linkLine.CalcLink();
      this._linkLine.Draw(graphics);
    }
    if (this._resizeAction != null && (this._moveAction == null || !this._moveAction.IsMoving))
      this._resizeAction.DrawResizeCorner(graphics);
    if (this._mousePointerElement != null && this._mousePointerElement is IControllable)
      ((IControllable) this._mousePointerElement).GetController().DrawSelection(graphics);
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
    var clippingRegion = this.Gsc2Goc(e.ClipRectangle);
    this._document.DrawGrid(graphics, clippingRegion, this._document.GridSize, 1f, this.Size.Width, this.Size.Height);
    graphics.EndContainer(container);
    graphics.Transform = transform;
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    this._document.WindowSize = this.Size;
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    switch (this._document.Action)
    {
      case DesignerAction.Select:
      case DesignerAction.Connect:
        if (e.Button == MouseButtons.Left)
        {
          var point1 = this.Gsc2Goc(new Point(e.X, e.Y));
          this.StartResizeElement(point1);
          if (this._resizeAction == null || !this._resizeAction.IsResizing)
          {
            if (this._isEditLabel)
              this.EndEditLabel();
            this.SelectedElement = this._document.FindElement(point1);
            if (this.SelectedElement != null)
            {
              this.OnElementMouseDown(new ElementMouseEventArgs(this.SelectedElement, e.X, e.Y));
              if (e.Clicks != 2 || !(this.SelectedElement is ILabelElement))
              {
                if (this.SelectedElement is ConnectorElement)
                {
                  this.StartAddLink((ConnectorElement) this.SelectedElement, point1);
                  this.SelectedElement = (BaseElement) null;
                }
                else
                  this.StartSelectElements(this.SelectedElement, point1);
              }
              else
                break;
            }
            else
            {
              this._document.ClearSelection();
              var point2 = this.Gsc2Goc(new Point(e.X, e.Y));
              this._isMultiSelection = true;
              this._selectionArea.Visible = true;
              this._selectionArea.Location = point2;
              this._selectionArea.Size = new Size(0, 0);
              if (this._resizeAction != null)
                this._resizeAction.ShowResizeCorner(false);
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
          this.StartAddElement(this.Gsc2Goc(new Point(e.X, e.Y)));
          break;
        }
        break;
      case DesignerAction.Delete:
        if (e.Button == MouseButtons.Left)
        {
          this.DeleteElement(this.Gsc2Goc(new Point(e.X, e.Y)));
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
      this.Cursor = Cursors.Arrow;
      var point = this.Gsc2Goc(new Point(e.X, e.Y));
      if (this._resizeAction != null && (this._document.Action == DesignerAction.Select || this._document.Action == DesignerAction.Connect && this._resizeAction.IsResizingLink))
        this.Cursor = this._resizeAction.UpdateResizeCornerCursor(point);
      if (this._document.Action == DesignerAction.Connect)
      {
        var element = this._document.FindElement(point);
        if (this._mousePointerElement != element)
        {
          if (element is ConnectorElement)
          {
            this._mousePointerElement = element;
            this._mousePointerElement.Invalidate();
            this.Invalidate(this._mousePointerElement, true);
          }
          else if (this._mousePointerElement != null)
          {
            this._mousePointerElement.Invalidate();
            this.Invalidate(this._mousePointerElement, true);
            this._mousePointerElement = (BaseElement) null;
          }
        }
      }
      else
      {
        this.Invalidate(this._mousePointerElement, true);
        this._mousePointerElement = (BaseElement) null;
      }
    }
    if (e.Button == MouseButtons.Left)
    {
      var point1 = this.Gsc2Goc(new Point(e.X, e.Y));
      if (this._resizeAction != null && this._resizeAction.IsResizing)
      {
        this._resizeAction.Resize(point1);
        this.Invalidate();
      }
      if (this._moveAction != null && this._moveAction.IsMoving)
      {
        this._moveAction.Move(point1);
        this.Invalidate();
      }
      if (this._isMultiSelection || this._isAddSelection)
      {
        var point2 = this.Gsc2Goc(new Point(e.X, e.Y));
        this._selectionArea.Size = new Size(point2.X - this._selectionArea.Location.X, point2.Y - this._selectionArea.Location.Y);
        this._selectionArea.Invalidate();
        this.Invalidate((BaseElement) this._selectionArea, true);
      }
      if (this._isAddLink)
      {
        this.SelectedElement = this._document.FindElement(point1);
        this._linkLine.Connector2 = !(this.SelectedElement is ConnectorElement) || !this._document.CanAddLink(this._connStart, (ConnectorElement) this.SelectedElement) ? this._connEnd : (ConnectorElement) this.SelectedElement;
        ((IMoveController) ((IControllable) this._connEnd).GetController()).Move(point1);
        base.Invalidate();
      }
    }
    base.OnMouseMove(e);
  }

  protected override void OnMouseDoubleClick(MouseEventArgs e)
  {
    if (this.SelectedElement == null)
      return;
    this.OnElementDoubleClick(new ElementEventArgs(this.SelectedElement));
    if (this._moveAction == null)
      return;
    this._moveAction.End();
    this._moveAction = (MoveAction) null;
    this.OnElementMouseUp(new ElementMouseEventArgs(this.SelectedElement, e.X, e.Y));
    if (this.Changed)
      this.AddUndo();
    this.RestartInitValues();
    base.Invalidate();
    base.OnMouseUp(e);
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    var unsignedRectangle = this._selectionArea.GetUnsignedRectangle();
    if (this._moveAction != null && this._moveAction.IsMoving)
    {
      this.OnElementClick(new ElementEventArgs(this.SelectedElement, this.PreviousSelectedElement));
      this._moveAction.End();
      this._moveAction = (MoveAction) null;
      this.OnElementMouseUp(new ElementMouseEventArgs(this.SelectedElement, e.X, e.Y));
      if (this.Changed)
        this.AddUndo();
      this.CheckControlClick();
    }
    if (this._isMultiSelection)
      this.EndSelectElements(unsignedRectangle);
    else if (this._isAddSelection)
      this.EndAddElement(unsignedRectangle);
    else if (this._isAddLink)
    {
      this.EndAddLink();
      this.AddUndo();
    }
    if (this._resizeAction != null)
    {
      if (this._resizeAction.IsResizing)
      {
        this._resizeAction.End(this.Gsc2Goc(new Point(e.X, e.Y)));
        this.AddUndo();
      }
      this._resizeAction.UpdateResizeCorner();
    }
    this.RestartInitValues();
    base.Invalidate();
    this.Invalidate();
    base.OnMouseUp(e);
  }

  private void CheckControlClick()
  {
    if ((Control.ModifierKeys & Keys.Control) != Keys.Control || this.SelectedElement == null || this.PreviousSelectedElement == null || !(this.SelectedElement is NodeElement) || !(this.PreviousSelectedElement is NodeElement))
      return;
    var connStart = ((IEnumerable<ConnectorElement>) ((NodeElement) this.PreviousSelectedElement).Connectors).OrderBy<ConnectorElement, int>((Func<ConnectorElement, int>) (c => c.Links.Count)).FirstOrDefault<ConnectorElement>((Func<ConnectorElement, bool>) (c => !c.IsStart));
    var connEnd = ((IEnumerable<ConnectorElement>) ((NodeElement) this.SelectedElement).Connectors).FirstOrDefault<ConnectorElement>((Func<ConnectorElement, bool>) (c => c.IsStart && c.Links.Count == 0));
    if (connStart == null || connEnd == null)
      return;
    this.Document.AddLink(connStart, connEnd);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementClick;

  protected virtual void OnElementClick(ElementEventArgs e)
  {
    if (this.ElementClick == null)
      return;
    this.ElementClick((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementDoubleClick;

  protected virtual void OnElementDoubleClick(ElementEventArgs e)
  {
    if (this.ElementDoubleClick == null)
      return;
    this.ElementDoubleClick((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementMouseEventHandler ElementMouseDown;

  protected virtual void OnElementMouseDown(ElementMouseEventArgs e)
  {
    if (this.ElementMouseDown == null)
      return;
    this.ElementMouseDown((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementMouseEventHandler ElementMouseUp;

  protected virtual void OnElementMouseUp(ElementMouseEventArgs e)
  {
    if (this.ElementMouseUp == null)
      return;
    this.ElementMouseUp((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementMoving;

  protected virtual void OnElementMoving(ElementEventArgs e)
  {
    if (this.ElementMoving == null)
      return;
    this.ElementMoving((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementMoved;

  protected virtual void OnElementMoved(ElementEventArgs e)
  {
    if (this.ElementMoved == null)
      return;
    this.ElementMoved((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementResizing;

  protected virtual void OnElementResizing(ElementEventArgs e)
  {
    if (this.ElementResizing == null)
      return;
    this.ElementResizing((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler ElementResized;

  protected virtual void OnElementResized(ElementEventArgs e)
  {
    if (this.ElementResized == null)
      return;
    this.ElementResized((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementConnectEventHandler ElementConnecting;

  protected virtual void OnElementConnecting(ElementConnectEventArgs e)
  {
    if (this.ElementConnecting == null)
      return;
    this.ElementConnecting((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementConnectEventHandler ElementConnected;

  protected virtual void OnElementConnected(ElementConnectEventArgs e)
  {
    if (this.ElementConnected == null)
      return;
    this.ElementConnected((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementSelectionEventHandler ElementSelection;

  protected virtual void OnElementSelection(ElementSelectionEventArgs e)
  {
    if (this.ElementSelection == null)
      return;
    this.ElementSelection((object) this, e);
  }

  [Category("Element")]
  public event Designer.ElementEventHandler LinkRemoved;

  protected virtual void OnLinkRemoved(ElementEventArgs e)
  {
    if (this.LinkRemoved == null)
      return;
    this.LinkRemoved((object) this, e);
  }

  private void DocumentPropertyChanged(object sender, EventArgs e)
  {
    if (this.IsChanging())
      return;
    base.Invalidate();
  }

  private void DocumentAppearancePropertyChanged(object sender, EventArgs e)
  {
    if (this.IsChanging())
      return;
    this.AddUndo();
    base.Invalidate();
  }

  private void DocumentElementPropertyChanged(object sender, EventArgs e)
  {
    this.Changed = true;
    if (this.IsChanging())
      return;
    this.AddUndo();
    base.Invalidate();
  }

  private void DocumentElementSelection(object sender, ElementSelectionEventArgs e) => this.OnElementSelection(e);

  private void DocumentLinkRemoved(object sender, ElementEventArgs e) => this.OnLinkRemoved(e);

  public Document Document => this._document;

  public bool CanUndo => this._undo.CanUndo;

  public bool CanRedo => this._undo.CanRedo;

  private bool IsChanging()
  {
    if (this._moveAction != null && this._moveAction.IsMoving || this._isAddLink || this._isMultiSelection)
      return true;
    return this._resizeAction != null && this._resizeAction.IsResizing;
  }

  public Point Gsc2Goc(Point gsp)
  {
    var zoom = this._document.Zoom;
    gsp.X = (int) ((double) (gsp.X - this.AutoScrollPosition.X) / (double) zoom);
    gsp.Y = (int) ((double) (gsp.Y - this.AutoScrollPosition.Y) / (double) zoom);
    return gsp;
  }

  public Rectangle Gsc2Goc(Rectangle gsr)
  {
    var zoom = this._document.Zoom;
    gsr.X = (int) ((double) (gsr.X - this.AutoScrollPosition.X) / (double) zoom);
    gsr.Y = (int) ((double) (gsr.Y - this.AutoScrollPosition.Y) / (double) zoom);
    gsr.Width = (int) ((double) gsr.Width / (double) zoom);
    gsr.Height = (int) ((double) gsr.Height / (double) zoom);
    return gsr;
  }

  public Rectangle Goc2Gsc(Rectangle gsr)
  {
    var zoom = this._document.Zoom;
    gsr.X = (int) ((double) (gsr.X + this.AutoScrollPosition.X) * (double) zoom);
    gsr.Y = (int) ((double) (gsr.Y + this.AutoScrollPosition.Y) * (double) zoom);
    gsr.Width = (int) ((double) gsr.Width * (double) zoom);
    gsr.Height = (int) ((double) gsr.Height * (double) zoom);
    return gsr;
  }

  internal void DrawSelectionRectangle(Graphics g) => this._selectionArea.Draw(g);

  public void SaveBinary(string fileName)
  {
    var formatter = (IFormatter) new BinaryFormatter();
    var serializationStream = (Stream) new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
    formatter.Serialize(serializationStream, (object) this._document);
    serializationStream.Close();
  }

  public Image GetThumbnail()
  {
    var image = this.GetImage(false, true);
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
    this._document = (Document) formatter.Deserialize(serializationStream);
    serializationStream.Close();
    this.RecreateEventsHandlers();
  }

  public void SetDocument(Document document)
  {
    this._document = document;
    this.RecreateEventsHandlers();
  }

  public void Copy()
  {
    if (this._document.SelectedElements.Count == 0)
      return;
    var formatter = (IFormatter) new BinaryFormatter();
    var stream = (Stream) new MemoryStream();
    var arrayClone = this._document.SelectedElements.GetArrayClone();
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
    this._undo.Enabled = false;
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
      this._document.AddElements(els);
      this._document.ClearSelection();
      this._document.SelectElements(els);
    }
    this._undo.Enabled = true;
    this.AddUndo();
    this.EndGeneralAction();
  }

  public void Cut()
  {
    this.Copy();
    this.DeleteSelectedElements();
    this.EndGeneralAction();
  }

  private void EndGeneralAction()
  {
    this.RestartInitValues();
    if (this._resizeAction == null)
      return;
    this._resizeAction.ShowResizeCorner(false);
  }

  private void RestartInitValues()
  {
    this._moveAction = (MoveAction) null;
    this._isMultiSelection = false;
    this._isAddSelection = false;
    this._isAddLink = false;
    this.Changed = false;
    this._connStart = (ConnectorElement) null;
    this._selectionArea.FillColor1 = SystemColors.Control;
    this._selectionArea.BorderColor = SystemColors.Control;
    this._selectionArea.Visible = false;
    this._document.CalcWindow(true);
  }

  private void StartSelectElements(BaseElement selectedElem, Point mousePoint)
  {
    if (!this._document.SelectedElements.Contains(selectedElem))
    {
      if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift)
        this._document.ClearSelection();
      this._document.SelectElement(selectedElem);
    }
    else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
      this._document.SelectedElements.Remove(selectedElem);
    this.Changed = false;
    this._moveAction = new MoveAction();
    var onElementMovingDelegate = new MoveAction.OnElementMovingDelegate(this.OnElementMoving);
    this._moveAction.Start(mousePoint, this._document, onElementMovingDelegate);
    this._controllers = new IController[this._document.SelectedElements.Count];
    for (var index = this._document.SelectedElements.Count - 1; index >= 0; --index)
      this._controllers[index] = !(this._document.SelectedElements[index] is IControllable) ? (IController) null : ((IControllable) this._document.SelectedElements[index]).GetController();
    this._resizeAction = new ResizeAction();
    this._resizeAction.Select(this._document);
  }

  private void EndSelectElements(Rectangle selectionRectangle) => this._document.SelectElements(selectionRectangle);

  private void StartResizeElement(Point mousePoint)
  {
    if (this._resizeAction == null || this._document.Action != DesignerAction.Select && (this._document.Action != DesignerAction.Connect || !this._resizeAction.IsResizingLink))
      return;
    var onElementResizingDelegate = new ResizeAction.OnElementResizingDelegate(this.OnElementResizing);
    this._resizeAction.Start(mousePoint, onElementResizingDelegate);
    if (this._resizeAction.IsResizing)
      return;
    this._resizeAction = (ResizeAction) null;
  }

  private void StartAddLink(ConnectorElement connectorStart, Point mousePoint)
  {
    if (this._document.Action != DesignerAction.Connect)
      return;
    this._connStart = connectorStart;
    var connectorElement = new ConnectorElement(connectorStart.ParentElement);
    connectorElement.Location = connectorStart.Location;
    this._connEnd = connectorElement;
    ((IMoveController) ((IControllable) this._connEnd).GetController()).Start(mousePoint);
    this._isAddLink = true;
    switch (this._document.LinkType)
    {
      case LinkType.Straight:
        this._linkLine = (BaseLinkElement) new StraightLinkElement(connectorStart, this._connEnd);
        break;
      case LinkType.RightAngle:
        this._linkLine = (BaseLinkElement) new RightAngleLinkElement(connectorStart, this._connEnd);
        break;
    }
    this._linkLine.Visible = true;
    this._linkLine.BorderColor = Color.FromArgb(150, Color.Black);
    this._linkLine.BorderWidth = 1;
    this.Invalidate((BaseElement) this._linkLine, true);
    this.OnElementConnecting(new ElementConnectEventArgs(connectorStart.ParentElement, (NodeElement) null, this._linkLine));
  }

  private void EndAddLink()
  {
    if (this._connEnd != this._linkLine.Connector2)
    {
      this._linkLine.Connector1.RemoveLink(this._linkLine);
      this._linkLine = this._document.AddLink(this._linkLine.Connector1, this._linkLine.Connector2);
      var e = new ElementConnectEventArgs(this._linkLine.Connector1.ParentElement, this._linkLine.Connector2.ParentElement, this._linkLine);
      var flag = true;
      if (this._linkLine.Connector1.ParentElement is DiagramBlock)
        flag = (this._linkLine.Connector1.ParentElement as DiagramBlock).OnElementConnected(this, e);
      if (flag)
        this.OnElementConnected(e);
    }
    this._connStart = (ConnectorElement) null;
    this._connEnd = (ConnectorElement) null;
    this._linkLine = (BaseLinkElement) null;
  }

  private void StartAddElement(Point mousePoint)
  {
    this._document.ClearSelection();
    this._selectionArea.FillColor1 = Color.LightSteelBlue;
    this._selectionArea.BorderColor = Color.WhiteSmoke;
    this._isAddSelection = true;
    this._selectionArea.Visible = true;
    this._selectionArea.Location = mousePoint;
    this._selectionArea.Size = new Size(0, 0);
  }

  private void EndAddElement(Rectangle selectionRectangle)
  {
    BaseElement el;
    switch (this._document.ElementType)
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
    this._document.AddElement(el);
    this._document.Action = DesignerAction.Select;
  }

  private void EndEditLabel()
  {
    if (this._editLabelAction != null)
    {
      this._editLabelAction.EndEdit();
      this._editLabelAction = (EditLabelAction) null;
    }
    this._isEditLabel = false;
  }

  private void DeleteElement(Point mousePoint)
  {
    this._document.DeleteElement(mousePoint);
    this.SelectedElement = (BaseElement) null;
    this._document.Action = DesignerAction.Select;
  }

  private void DeleteSelectedElements() => this._document.DeleteSelectedElements();

  public void Undo()
  {
    this._document = (Document) this._undo.Undo();
    this.RecreateEventsHandlers();
    if (this._resizeAction != null)
      this._resizeAction.UpdateResizeCorner();
    base.Invalidate();
  }

  public void Redo()
  {
    this._document = (Document) this._undo.Redo();
    this.RecreateEventsHandlers();
    if (this._resizeAction != null)
      this._resizeAction.UpdateResizeCorner();
    base.Invalidate();
  }

  private void AddUndo() => this._undo.AddUndo((object) this._document);

  private void RecreateEventsHandlers()
  {
    this._document.PropertyChanged += new EventHandler(this.DocumentPropertyChanged);
    this._document.AppearancePropertyChanged += new EventHandler(this.DocumentAppearancePropertyChanged);
    this._document.ElementPropertyChanged += new EventHandler(this.DocumentElementPropertyChanged);
    this._document.ElementSelection += new Document.ElementSelectionEventHandler(this.DocumentElementSelection);
    this._document.LinkRemoved += new Document.ElementEventHandler(this.DocumentLinkRemoved);
  }

  private void MoveElement(Keys key)
  {
    var num = (Control.ModifierKeys & Keys.Shift) == Keys.Shift ? 10 : 1;
    foreach (BaseElement selectedElement in this.Document.SelectedElements)
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
    this.Refresh();
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (e.KeyCode == Keys.Delete)
    {
      this.DeleteSelectedElements();
      this.EndGeneralAction();
      base.Invalidate();
    }
    if (e.Control && e.KeyCode == Keys.Z && this._undo.CanUndo)
      this.Undo();
    if (e.Control && e.KeyCode == Keys.C)
      this.Copy();
    if (e.Control && e.KeyCode == Keys.V)
      this.Paste();
    if (e.Control && e.KeyCode == Keys.X)
      this.Cut();
    if (e.Control && e.KeyCode == Keys.A)
    {
      this.Document.SelectAllElements();
      this.Refresh();
    }
    base.OnKeyDown(e);
  }

  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {
    if (msg.Msg == 256 || msg.Msg == 260)
    {
      if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
      {
        if ((keyData & Keys.Up) == Keys.Up || (keyData & Keys.Down) == Keys.Down || (keyData & Keys.Right) == Keys.Right || (keyData & Keys.Left) == Keys.Left)
          this.MoveElement(keyData);
      }
      else
      {
        if ((keyData & Keys.Down) == Keys.Down)
        {
          foreach (BaseElement selectedElement in this.Document.SelectedElements)
            this.Document.SendToBackElement(selectedElement);
        }
        if ((keyData & Keys.Up) == Keys.Up)
        {
          foreach (BaseElement selectedElement in this.Document.SelectedElements)
            this.Document.BringToFrontElement(selectedElement);
        }
      }
    }
    return base.ProcessCmdKey(ref msg, keyData);
  }

  public Bitmap GetImage(bool drawGrid, bool whitebackground)
  {
    var area = this.Document.GetArea();
    var image = new Bitmap(area.Width - area.X, area.Height - area.Y);
    var g = Graphics.FromImage((Image) image);
    if (whitebackground || drawGrid)
      g.FillRectangle((Brush) new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
    if (drawGrid)
      this.Document.DrawGrid(g, new Rectangle(0, 0, this.Document.Size.Width + this.Document.Location.X, this.Document.Size.Height + this.Document.Location.Y));
    g.TranslateTransform((float) (area.X * -1), (float) (area.Y * -1));
    this.Document.DrawElementsToGraphics(g, new Rectangle?());
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
    var area = this.Document.GetArea();
    graphics.FillRectangle((Brush) new SolidBrush(Color.White), 0, 0, width, height);
    if (scaleToFitPaper && area.Width > 0 && area.Height > 0)
    {
      num = Math.Min((float) width * 1f / (float) (area.Width - area.X), (float) height * 1f / (float) (area.Height - area.Y));
      if (!allowStretch)
        num = Math.Min(num, 1f);
    }
    if (drawGrid)
      this.Document.DrawGrid(graphics, new Rectangle(), new Size(Convert.ToInt32(10f * num), Convert.ToInt32(10f * num)), num, width, height);
    if (scaleToFitPaper && area.Width > 0 && area.Height > 0)
      graphics.ScaleTransform(num, num);
    if (pageNumber > 0)
      graphics.TranslateTransform((float) ((area.X + width * pageNumber) * -1), (float) ((area.Y + height * pageNumber) * -1));
    else
      graphics.TranslateTransform((float) (area.X * -1), (float) (area.Y * -1));
    this.Document.DrawElementsToGraphics(graphics, new Rectangle?());
    return !scaleToFitPaper && (area.X + width * (pageNumber + 1) <= area.Width + area.X || area.Y + height * (pageNumber + 1) <= area.Height + area.Y);
  }

  public delegate void ElementEventHandler(object sender, ElementEventArgs e);

  public delegate void ElementMouseEventHandler(object sender, ElementMouseEventArgs e);

  public delegate void ElementConnectEventHandler(object sender, ElementConnectEventArgs e);

  public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
}