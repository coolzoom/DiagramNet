﻿// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.Controllers.IController
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements.Controllers;

internal interface IController
{
  BaseElement OwnerElement { get; }

  bool HitTest(Point p);

  bool HitTest(Rectangle r);

  void DrawSelection(Graphics g);
}