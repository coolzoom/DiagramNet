// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.CommentBoxElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DiagramNet.Elements
{
  [Serializable]
  public class CommentBoxElement : RectangleElement, IControllable
  {
    [NonSerialized]
    private RectangleController _controller;
    protected Size FoldSize = new Size(10, 15);

    public CommentBoxElement()
      : this(0, 0, 100, 100)
    {
    }

    public CommentBoxElement(Rectangle rec)
      : this(rec.Location, rec.Size)
    {
    }

    public CommentBoxElement(Point l, Size s)
      : this(l.X, l.Y, s.Width, s.Height)
    {
    }

    public CommentBoxElement(int top, int left, int width, int height)
      : base(top, left, width, height)
    {
      this.FillColor1Value = Color.LemonChiffon;
      this.FillColor2Value = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 128);
      this.LabelValue.Opacity = 100;
    }

    internal override void Draw(Graphics g)
    {
      this.IsInvalidated = false;
      Rectangle unsignedRectangle = BaseElement.GetUnsignedRectangle(new Rectangle(this.LocationValue, this.SizeValue));
      Point[] points = new Point[5]
      {
        new Point(unsignedRectangle.X, unsignedRectangle.Y),
        new Point(unsignedRectangle.X, unsignedRectangle.Y + unsignedRectangle.Height),
        new Point(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + unsignedRectangle.Height),
        new Point(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + this.FoldSize.Height),
        new Point(unsignedRectangle.X + unsignedRectangle.Width - this.FoldSize.Width, unsignedRectangle.Y)
      };
      g.FillPolygon(this.GetBrush(unsignedRectangle), points, FillMode.Alternate);
      g.DrawPolygon(new Pen(this.BorderColorValue, (float) this.BorderWidthValue), points);
      g.DrawLine(new Pen(this.BorderColorValue, (float) this.BorderWidthValue), new Point(unsignedRectangle.X + unsignedRectangle.Width - this.FoldSize.Width, unsignedRectangle.Y + this.FoldSize.Height), new Point(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + this.FoldSize.Height));
      g.DrawLine(new Pen(this.BorderColorValue, (float) this.BorderWidthValue), new Point(unsignedRectangle.X + unsignedRectangle.Width - this.FoldSize.Width, unsignedRectangle.Y), new Point(unsignedRectangle.X + unsignedRectangle.Width - this.FoldSize.Width, unsignedRectangle.Y + this.FoldSize.Height));
    }

    IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = (RectangleController) new CommentBoxController((BaseElement) this));
  }
}
