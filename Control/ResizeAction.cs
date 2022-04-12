// Decompiled with JetBrains decompiler
// Type: DiagramNet.ResizeAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using Elements.Controllers;
using Events;

internal class ResizeAction
{
  private OnElementResizingDelegate _onElementResizingDelegate;
  private IResizeController _resizeCtrl;
  private Document _document;

  public bool IsResizing { get; private set; }

  public bool IsResizingLink => _resizeCtrl != null && _resizeCtrl.OwnerElement is BaseLinkElement;

  public void Select(Document document)
  {
    _document = document;
    if (document.SelectedElements.Count == 1 && document.SelectedElements[0] is IControllable)
    {
      var controller = ((IControllable) document.SelectedElements[0]).GetController();
      if (!(controller is IResizeController))
        return;
      controller.OwnerElement.Invalidate();
      _resizeCtrl = (IResizeController) controller;
      ShowResizeCorner(true);
    }
    else
      _resizeCtrl = (IResizeController) null;
  }

  public void Start(
    Point mousePoint,
    OnElementResizingDelegate onElementResizingDelegate)
  {
    IsResizing = false;
    if (_resizeCtrl == null)
      return;
    _onElementResizingDelegate = onElementResizingDelegate;
    _resizeCtrl.OwnerElement.Invalidate();
    var corner = _resizeCtrl.HitTestCorner(mousePoint);
    if (corner == CornerPosition.Nothing)
      return;
    var e = new ElementEventArgs(_resizeCtrl.OwnerElement);
    onElementResizingDelegate(e);
    _resizeCtrl.Start(mousePoint, corner);
    UpdateResizeCorner();
    IsResizing = true;
  }

  public void Resize(Point dragPoint)
  {
    if (_resizeCtrl == null || !_resizeCtrl.CanResize)
      return;
    _onElementResizingDelegate(new ElementEventArgs(_resizeCtrl.OwnerElement));
    _resizeCtrl.OwnerElement.Invalidate();
    _resizeCtrl.Resize(dragPoint);
    var labelController = ControllerHelper.GetLabelController(_resizeCtrl.OwnerElement);
    if (labelController != null)
      labelController.SetLabelPosition();
    else if (_resizeCtrl.OwnerElement is ILabelElement)
      ((ILabelElement) _resizeCtrl.OwnerElement).Label.PositionBySite(_resizeCtrl.OwnerElement);
    UpdateResizeCorner();
  }

  public void End(Point posEnd)
  {
    if (_resizeCtrl == null)
      return;
    _resizeCtrl.OwnerElement.Invalidate();
    _resizeCtrl.End(posEnd);
    _onElementResizingDelegate(new ElementEventArgs(_resizeCtrl.OwnerElement));
    IsResizing = false;
  }

  public void DrawResizeCorner(Graphics g)
  {
    if (_resizeCtrl == null)
      return;
    foreach (var corner in _resizeCtrl.Corners)
    {
      if (_document.Action == DesignerAction.Select)
      {
        if (corner.Visible)
          corner.Draw(g);
      }
      else if (_document.Action == DesignerAction.Connect && _resizeCtrl.OwnerElement is BaseLinkElement && corner.Visible)
        corner.Draw(g);
    }
  }

  public void UpdateResizeCorner()
  {
    if (_resizeCtrl == null)
      return;
    _resizeCtrl.UpdateCornersPos();
  }

  public Cursor UpdateResizeCornerCursor(Point mousePoint)
  {
    if (_resizeCtrl == null || !_resizeCtrl.CanResize)
      return Cursors.Default;
    switch (_resizeCtrl.HitTestCorner(mousePoint))
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
    if (_resizeCtrl == null)
      return;
    var canResize = _resizeCtrl.CanResize;
    foreach (BaseElement corner in _resizeCtrl.Corners)
      corner.Visible = canResize && show;
    if (_resizeCtrl.Corners.Length < 3)
      return;
    _resizeCtrl.Corners[3].Visible = false;
  }

  public delegate void OnElementResizingDelegate(ElementEventArgs e);
}