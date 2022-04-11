// Decompiled with JetBrains decompiler
// Type: DiagramNet.ResizeAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements;
using DiagramNet.Elements.Controllers;
using DiagramNet.Events;

namespace DiagramNet;

internal class ResizeAction
{
  private ResizeAction.OnElementResizingDelegate _onElementResizingDelegate;
  private IResizeController _resizeCtrl;
  private Document _document;

  public bool IsResizing { get; private set; }

  public bool IsResizingLink => this._resizeCtrl != null && this._resizeCtrl.OwnerElement is BaseLinkElement;

  public void Select(Document document)
  {
    this._document = document;
    if (document.SelectedElements.Count == 1 && document.SelectedElements[0] is IControllable)
    {
      var controller = ((IControllable) document.SelectedElements[0]).GetController();
      if (!(controller is IResizeController))
        return;
      controller.OwnerElement.Invalidate();
      this._resizeCtrl = (IResizeController) controller;
      this.ShowResizeCorner(true);
    }
    else
      this._resizeCtrl = (IResizeController) null;
  }

  public void Start(
    Point mousePoint,
    ResizeAction.OnElementResizingDelegate onElementResizingDelegate)
  {
    this.IsResizing = false;
    if (this._resizeCtrl == null)
      return;
    this._onElementResizingDelegate = onElementResizingDelegate;
    this._resizeCtrl.OwnerElement.Invalidate();
    var corner = this._resizeCtrl.HitTestCorner(mousePoint);
    if (corner == CornerPosition.Nothing)
      return;
    var e = new ElementEventArgs(this._resizeCtrl.OwnerElement);
    onElementResizingDelegate(e);
    this._resizeCtrl.Start(mousePoint, corner);
    this.UpdateResizeCorner();
    this.IsResizing = true;
  }

  public void Resize(Point dragPoint)
  {
    if (this._resizeCtrl == null || !this._resizeCtrl.CanResize)
      return;
    this._onElementResizingDelegate(new ElementEventArgs(this._resizeCtrl.OwnerElement));
    this._resizeCtrl.OwnerElement.Invalidate();
    this._resizeCtrl.Resize(dragPoint);
    var labelController = ControllerHelper.GetLabelController(this._resizeCtrl.OwnerElement);
    if (labelController != null)
      labelController.SetLabelPosition();
    else if (this._resizeCtrl.OwnerElement is ILabelElement)
      ((ILabelElement) this._resizeCtrl.OwnerElement).Label.PositionBySite(this._resizeCtrl.OwnerElement);
    this.UpdateResizeCorner();
  }

  public void End(Point posEnd)
  {
    if (this._resizeCtrl == null)
      return;
    this._resizeCtrl.OwnerElement.Invalidate();
    this._resizeCtrl.End(posEnd);
    this._onElementResizingDelegate(new ElementEventArgs(this._resizeCtrl.OwnerElement));
    this.IsResizing = false;
  }

  public void DrawResizeCorner(Graphics g)
  {
    if (this._resizeCtrl == null)
      return;
    foreach (var corner in this._resizeCtrl.Corners)
    {
      if (this._document.Action == DesignerAction.Select)
      {
        if (corner.Visible)
          corner.Draw(g);
      }
      else if (this._document.Action == DesignerAction.Connect && this._resizeCtrl.OwnerElement is BaseLinkElement && corner.Visible)
        corner.Draw(g);
    }
  }

  public void UpdateResizeCorner()
  {
    if (this._resizeCtrl == null)
      return;
    this._resizeCtrl.UpdateCornersPos();
  }

  public Cursor UpdateResizeCornerCursor(Point mousePoint)
  {
    if (this._resizeCtrl == null || !this._resizeCtrl.CanResize)
      return Cursors.Default;
    switch (this._resizeCtrl.HitTestCorner(mousePoint))
    {
      case CornerPosition.BottomCenter:
        return Cursors.SizeNS;
      case CornerPosition.BottomLeft:
        return Cursors.SizeNESW;
      case CornerPosition.BottomRight:
        return Cursors.SizeNWSE;
      case CornerPosition.MiddleLeft:
      case CornerPosition.MiddleRight:
        return Cursors.SizeWE;
      case CornerPosition.TopCenter:
        return Cursors.SizeNS;
      case CornerPosition.TopLeft:
        return Cursors.SizeNWSE;
      case CornerPosition.TopRight:
        return Cursors.SizeNESW;
      default:
        return Cursors.Default;
    }
  }

  public void ShowResizeCorner(bool show)
  {
    if (this._resizeCtrl == null)
      return;
    var canResize = this._resizeCtrl.CanResize;
    foreach (BaseElement corner in this._resizeCtrl.Corners)
      corner.Visible = canResize && show;
    if (this._resizeCtrl.Corners.Length < 3)
      return;
    this._resizeCtrl.Corners[3].Visible = false;
  }

  public delegate void OnElementResizingDelegate(ElementEventArgs e);
}