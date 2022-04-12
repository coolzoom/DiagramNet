// Decompiled with JetBrains decompiler
// Type: DiagramNet.MoveAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using Elements.Controllers;
using Events;
using System.Collections;

internal class MoveAction
{
  private OnElementMovingDelegate _onElementMovingDelegate;
  private IMoveController[] _moveCtrl;
  private Point _upperSelPoint = Point.Empty;
  private Point _upperSelPointDragOffset = Point.Empty;
  private Document _document;

  public bool IsMoving { get; private set; }

  public void Start(
    Point mousePoint,
    Document document,
    OnElementMovingDelegate onElementMovingDelegate)
  {
    _document = document;
    _onElementMovingDelegate = onElementMovingDelegate;
    _moveCtrl = new IMoveController[document.SelectedElements.Count];
    var arr2 = new IMoveController[document.SelectedElements.Count];
    for (var index = document.SelectedElements.Count - 1; index >= 0; --index)
    {
      _moveCtrl[index] = ControllerHelper.GetMoveController(document.SelectedElements[index]);
      if (_moveCtrl[index] != null && _moveCtrl[index].CanMove)
      {
        onElementMovingDelegate(new ElementEventArgs(document.SelectedElements[index]));
        _moveCtrl[index].Start(mousePoint);
        if (document.SelectedElements[index] is ILabelElement && ControllerHelper.GetLabelController(document.SelectedElements[index]) == null)
        {
          var label = ((ILabelElement) document.SelectedElements[index]).Label;
          arr2[index] = ControllerHelper.GetMoveController((BaseElement) label);
          if (arr2[index] != null && arr2[index].CanMove)
            arr2[index].Start(mousePoint);
          else
            arr2[index] = (IMoveController) null;
        }
      }
      else
        _moveCtrl[index] = (IMoveController) null;
    }
    _moveCtrl = (IMoveController[]) DiagramUtil.ArrayHelper.Append((Array) _moveCtrl, (Array) arr2);
    _moveCtrl = (IMoveController[]) DiagramUtil.ArrayHelper.Shrink((Array) _moveCtrl, (object) null);
    var flag = true;
    foreach (var moveController in _moveCtrl)
    {
      if (moveController != null)
      {
        moveController.OwnerElement.Invalidate();
        if (!(moveController.OwnerElement is BaseLinkElement) && !(moveController.OwnerElement is LabelElement))
        {
          flag = false;
          break;
        }
      }
    }
    if (flag)
    {
      foreach (var moveController in _moveCtrl)
        moveController?.End();
      _moveCtrl = new IMoveController[1];
    }
    UpdateUpperSelectionPoint();
    _upperSelPointDragOffset.X = _upperSelPoint.X - mousePoint.X;
    _upperSelPointDragOffset.Y = _upperSelPoint.Y - mousePoint.Y;
    IsMoving = true;
  }

  public void Move(Point dragPoint)
  {
    var point = dragPoint;
    point.Offset(_upperSelPointDragOffset.X, _upperSelPointDragOffset.Y);
    _upperSelPoint = point;
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    if (point.X == 0)
      dragPoint.X -= _upperSelPoint.X;
    if (point.Y == 0)
      dragPoint.Y -= _upperSelPoint.Y;
    foreach (var moveController in _moveCtrl)
    {
      if (moveController != null && moveController.WillMove(dragPoint))
      {
        moveController.OwnerElement.Invalidate();
        _onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));
        moveController.Move(dragPoint);
        if (moveController.OwnerElement is NodeElement)
          UpdateLinkPosition((NodeElement) moveController.OwnerElement);
        ControllerHelper.GetLabelController(moveController.OwnerElement)?.SetLabelPosition();
      }
    }
  }

  public void End()
  {
    _upperSelPoint = Point.Empty;
    _upperSelPointDragOffset = Point.Empty;
    foreach (var moveController in _moveCtrl)
    {
      if (moveController != null)
      {
        if (moveController.OwnerElement is NodeElement)
          UpdateLinkPosition((NodeElement) moveController.OwnerElement);
        moveController.End();
        _onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));
      }
    }
    IsMoving = false;
  }

  private void UpdateUpperSelectionPoint()
  {
    var points = new Point[_document.SelectedElements.Count];
    var index = 0;
    foreach (BaseElement selectedElement in _document.SelectedElements)
    {
      points[index] = selectedElement.Location;
      ++index;
    }
    _upperSelPoint = DiagramUtil.GetUpperPoint(points);
  }

  private void UpdateLinkPosition(NodeElement node)
  {
    foreach (var connector in node.Connectors)
    {
      foreach (BaseLinkElement link in connector.Links)
      {
        var controller = ((IControllable) link).GetController();
        if (controller is IMoveController)
        {
          if (!((IMoveController) controller).IsMoving)
            link.NeedCalcLink = true;
        }
        else
          link.NeedCalcLink = true;
        if (link is ILabelElement)
        {
          var label = ((ILabelElement) link).Label;
          var labelController = ControllerHelper.GetLabelController((BaseElement) link);
          if (labelController != null)
            labelController.SetLabelPosition();
          else
            label.PositionBySite((BaseElement) link);
          label.Invalidate();
        }
      }
    }
  }

  public delegate void OnElementMovingDelegate(ElementEventArgs e);
}