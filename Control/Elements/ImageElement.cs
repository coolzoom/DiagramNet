// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ImageElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System;
using System.Drawing;

namespace DiagramNet.Elements
{
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
      this._image = image;
      this.Top = top;
      this.Left = left;
      this.Width = width;
      this.Height = height;
    }

    public ImageElement(Image image, BaseElement rectangle)
    {
      this._image = image;
      this.Left = rectangle.Location.X + rectangle.Size.Width / 2 - image.Width / 2;
      this.Top = rectangle.Location.Y + rectangle.Size.Height / 2 - image.Height / 2;
      this.Width = image.Width;
      this.Height = image.Height;
    }

    internal override void Draw(Graphics g)
    {
      this.IsInvalidated = false;
      g.DrawImage(this._image, this.Left, this.Top, this.Width, this.Height);
    }

    IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RectangleController((BaseElement) this));
  }
}
