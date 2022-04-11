// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RectangleElement
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
  public class RectangleElement : BaseElement, IControllable, ILabelElement
  {
    protected Color FillColor1Value = Color.White;
    protected Color FillColor2Value = Color.DodgerBlue;
    protected LabelElement LabelValue = new LabelElement();
    [NonSerialized]
    private RectangleController _controller;

    public RectangleElement()
      : this(0, 0, 100, 100)
    {
    }

    public RectangleElement(Rectangle rec)
      : this(rec.Location, rec.Size)
    {
    }

    public RectangleElement(Point l, Size s)
      : this(l.X, l.Y, s.Width, s.Height)
    {
    }

    public RectangleElement(int top, int left, int width, int height)
    {
      this.LocationValue = new Point(top, left);
      this.SizeValue = new Size(width, height);
    }

    public override Point Location
    {
      get => base.Location;
      set => base.Location = value;
    }

    public override Size Size
    {
      get => base.Size;
      set => base.Size = value;
    }

    public virtual Color FillColor1
    {
      get => this.FillColor1Value;
      set
      {
        this.FillColor1Value = value;
        this.OnAppearanceChanged(new EventArgs());
      }
    }

    public virtual Color FillColor2
    {
      get => this.FillColor2Value;
      set
      {
        this.FillColor2Value = value;
        this.OnAppearanceChanged(new EventArgs());
      }
    }

    public virtual LabelElement Label
    {
      get => this.LabelValue;
      set
      {
        this.LabelValue = value;
        this.OnAppearanceChanged(new EventArgs());
      }
    }

    protected virtual Brush GetBrush(Rectangle r)
    {
      Color color;
      Color color2;
      if (this.OpacityValue == 100)
      {
        color = this.FillColor1Value;
        color2 = this.FillColor2Value;
      }
      else
      {
        color = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this.FillColor1Value);
        color2 = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this.FillColor2Value);
      }
      return !(this.FillColor2Value == Color.Empty) ? (Brush) new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : (Brush) new SolidBrush(color);
    }

    protected virtual void DrawBorder(Graphics g, Rectangle r)
    {
      Pen pen = new Pen(this.BorderColorValue, (float) this.BorderWidthValue);
      g.DrawRectangle(pen, r);
      pen.Dispose();
    }

    internal override void Draw(Graphics g)
    {
      this.IsInvalidated = false;
      Rectangle unsignedRectangle = this.GetUnsignedRectangle();
      Brush brush = this.GetBrush(unsignedRectangle);
      g.FillRectangle(brush, unsignedRectangle);
      this.DrawBorder(g, unsignedRectangle);
      brush.Dispose();
    }

    IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RectangleController((BaseElement) this));
  }
}
