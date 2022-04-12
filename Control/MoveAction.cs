// Decompiled with JetBrains decompiler
// Type: DiagramNet.MoveAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using DiagramNet.Elements.Controllers;
using DiagramNet.Events;
using System.Collections;

internal class MoveAction
{
  private MoveAction.OnElementMovingDelegate _onElementMovingDelegate;
  private IMoveController[] _moveCtrl;
  private Point _upperSelPoint = Point.Empty;
  private Point _upperSelPointDragOffset = Point.Empty;
  private Document _document;

  public bool IsMoving { get; private set; }

  public void Start(
    Point mousePoint,
    Document document,
    MoveAction.OnElementMovingDelegate onElementMovingDelegate)
  {
    this._document = document;
    this._onElementMovingDelegate = onElementMovingDelegate;
    this._moveCtrl = new IMoveController[document.SelectedElements.Count];
    var arr2 = new IMoveController[document.SelectedElements.Count];
    for (var index = document.SelectedElements.Count - 1; index >= 0; --index)
    {
      this._moveCtrl[index] = ControllerHelper.GetMoveController(document.SelectedElements[index]);
      if (this._moveCtrl[index] != null && this._moveCtrl[index].CanMove)
      {
        onElementMovingDelegate(new ElementEventArgs(document.SelectedElements[index]));
        this._moveCtrl[index].Start(mousePoint);
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
        this._moveCtrl[index] = (IMoveController) null;
    }
    this._moveCtrl = (IMoveController[]) DiagramUtil.ArrayHelper.Append((Array) this._moveCtrl, (Array) arr2);
    this._moveCtrl = (IMoveController[]) DiagramUtil.ArrayHelper.Shrink((Array) this._moveCtrl, (object) null);
    var flag = true;
    foreach (var moveController in this._moveCtrl)
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
      foreach (var moveController in this._moveCtrl)
        moveController?.End();
      this._moveCtrl = new IMoveController[1];
    }
    this.UpdateUpperSelectionPoint();
    this._upperSelPointDragOffset.X = this._upperSelPoint.X - mousePoint.X;
    this._upperSelPointDragOffset.Y = this._upperSelPoint.Y - mousePoint.Y;
    this.IsMoving = true;
  }

  public void Move(Point dragPoint)
  {
    var point = dragPoint;
    point.Offset(this._upperSelPointDragOffset.X, this._upperSelPointDragOffset.Y);
    this._upperSelPoint = point;
    if (point.X < 0)
      point.X = 0;
    if (point.Y < 0)
      point.Y = 0;
    if (point.X == 0)
      dragPoint.X -= this._upperSelPoint.X;
    if (point.Y == 0)
      dragPoint.Y -= this._upperSelPoint.Y;
    foreach (var moveController in this._moveCtrl)
    {
      if (moveController != null && moveController.WillMove(dragPoint))
      {
        moveController.OwnerElement.Invalidate();
        this._onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));
        moveController.Move(dragPoint);
        if (moveController.OwnerElement is NodeElement)
          this.UpdateLinkPosition((NodeElement) moveController.OwnerElement);
        ControllerHelper.GetLabelController(moveController.OwnerElement)?.SetLabelPosition();
      }
    }
  }

  public void End()
  {
    this._upperSelPoint = Point.Empty;
    this._upperSelPointDragOffset = Point.Empty;
    foreach (var moveController in this._moveCtrl)
    {
      if (moveController != null)
      {
        if (moveController.OwnerElement is NodeElement)
          this.UpdateLinkPosition((NodeElement) moveController.OwnerElement);
        moveController.End();
        this._onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));
      }
    }
    this.IsMoving = false;
  }

  private void UpdateUpperSelectionPoint()
  {
    var points = new Point[this._document.SelectedElements.Count];
    var index = 0;
    foreach (BaseElement selectedElement in this._document.SelectedElements)
    {
      points[index] = selectedElement.Location;
      ++index;
    }
    this._upperSelPoint = DiagramUtil.GetUpperPoint(points);
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