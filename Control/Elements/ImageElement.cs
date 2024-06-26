﻿// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ImageElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;

[Serializable]
public class ImageElement : BaseElement, IControllable
{
  [NonSerialized]
  private RectangleController _controller;
  private readonly Image _image;
  public int Top;
  public int Left;
  public int Width;
  public int Height;

  public ImageElement(Image image, int top, int left, int width, int height)
  {
    _image = image;
    Top = top;
    Left = left;
    Width = width;
    Height = height;
  }

  public ImageElement(Image image, BaseElement rectangle)
  {
    _image = image;
    Left = rectangle.Location.X + rectangle.Size.Width / 2 - image.Width / 2;
    Top = rectangle.Location.Y + rectangle.Size.Height / 2 - image.Height / 2;
    Width = image.Width;
    Height = image.Height;
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    g.DrawImage(_image, Left, Top, Width, Height);
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RectangleController(this));
}