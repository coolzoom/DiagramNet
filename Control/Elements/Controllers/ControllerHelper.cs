// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.ControllerHelper
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

internal class ControllerHelper
{
  private ControllerHelper()
  {
  }

  public static IMoveController GetMoveController(BaseElement el)
  {
    if (el is IControllable)
    {
      var controller = ((IControllable) el).GetController();
      if (controller is IMoveController)
        return (IMoveController) controller;
    }
    return (IMoveController) null;
  }

  public static IResizeController GetResizeController(BaseElement el) => el is IControllable ? ControllerHelper.GetResizeController(((IControllable) el).GetController()) : (IResizeController) null;

  public static IResizeController GetResizeController(IController ctrl) => ctrl as IResizeController;

  public static ILabelController GetLabelController(BaseElement el) => el is IControllable && el is ILabelElement ? ControllerHelper.GetLabelController(((IControllable) el).GetController()) : (ILabelController) null;

  public static ILabelController GetLabelController(IController ctrl) => ctrl as ILabelController;
}