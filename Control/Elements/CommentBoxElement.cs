// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.CommentBoxElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.Drawing.Drawing2D;

[Serializable]
public class CommentBoxElement : RectangleElement, IControllable
{
  [NonSerialized]
  private RectangleController _controller;
  protected Size FoldSize = new(10, 15);

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
    FillColor1Value = Color.LemonChiffon;
    FillColor2Value = Color.FromArgb(byte.MaxValue, byte.MaxValue, 128);
    LabelValue.Opacity = 100;
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    var unsignedRectangle = GetUnsignedRectangle(new Rectangle(LocationValue, SizeValue));
    var points = new Point[5]
    {
      new(unsignedRectangle.X, unsignedRectangle.Y),
      new(unsignedRectangle.X, unsignedRectangle.Y + unsignedRectangle.Height),
      new(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + unsignedRectangle.Height),
      new(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + FoldSize.Height),
      new(unsignedRectangle.X + unsignedRectangle.Width - FoldSize.Width, unsignedRectangle.Y)
    };
    g.FillPolygon(GetBrush(unsignedRectangle), points, FillMode.Alternate);
    g.DrawPolygon(new Pen(BorderColorValue, BorderWidthValue), points);
    g.DrawLine(new Pen(BorderColorValue, BorderWidthValue), new Point(unsignedRectangle.X + unsignedRectangle.Width - FoldSize.Width, unsignedRectangle.Y + FoldSize.Height), new Point(unsignedRectangle.X + unsignedRectangle.Width, unsignedRectangle.Y + FoldSize.Height));
    g.DrawLine(new Pen(BorderColorValue, BorderWidthValue), new Point(unsignedRectangle.X + unsignedRectangle.Width - FoldSize.Width, unsignedRectangle.Y), new Point(unsignedRectangle.X + unsignedRectangle.Width - FoldSize.Width, unsignedRectangle.Y + FoldSize.Height));
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new CommentBoxController(this));
}