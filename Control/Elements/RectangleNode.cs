// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RectangleNode
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using DiagramNet.Elements.Controllers;

[Serializable]
public class RectangleNode : NodeElement, IControllable, ILabelElement
{
  protected RectangleElement Rectangle;
  protected LabelElement LabelElement = new LabelElement();
  [NonSerialized]
  private RectangleController _controller;

  public RectangleNode()
    : this(0, 0, 100, 100)
  {
  }

  public RectangleNode(System.Drawing.Rectangle rec)
    : this(rec.Location, rec.Size)
  {
  }

  public RectangleNode(Point l, Size s)
    : this(l.X, l.Y, s.Width, s.Height)
  {
  }

  public RectangleNode(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
    this.Rectangle = new RectangleElement(top, left, width, height);
    this.SyncContructors();
  }

  public override Color BorderColor
  {
    get => base.BorderColor;
    set
    {
      this.Rectangle.BorderColor = value;
      base.BorderColor = value;
    }
  }

  public Color FillColor1
  {
    get => this.Rectangle.FillColor1;
    set => this.Rectangle.FillColor1 = value;
  }

  public Color FillColor2
  {
    get => this.Rectangle.FillColor2;
    set => this.Rectangle.FillColor2 = value;
  }

  public override int Opacity
  {
    get => base.Opacity;
    set
    {
      this.Rectangle.Opacity = value;
      base.Opacity = value;
    }
  }

  public override bool Visible
  {
    get => base.Visible;
    set
    {
      this.Rectangle.Visible = value;
      base.Visible = value;
    }
  }

  public override Point Location
  {
    get => base.Location;
    set
    {
      this.Rectangle.Location = value;
      base.Location = value;
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      this.Rectangle.Size = value;
      base.Size = value;
    }
  }

  public override int BorderWidth
  {
    get => base.BorderWidth;
    set
    {
      this.Rectangle.BorderWidth = value;
      base.BorderWidth = value;
    }
  }

  public virtual LabelElement Label
  {
    get => this.LabelElement;
    set
    {
      this.LabelElement = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  private void SyncContructors()
  {
    this.LocationValue = this.Rectangle.Location;
    this.SizeValue = this.Rectangle.Size;
    this.BorderColorValue = this.Rectangle.BorderColor;
    this.BorderWidthValue = this.Rectangle.BorderWidth;
    this.OpacityValue = this.Rectangle.Opacity;
    this.VisibleValue = this.Rectangle.Visible;
  }

  internal override void Draw(Graphics g)
  {
    this.IsInvalidated = false;
    this.Rectangle.Draw(g);
  }

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RectangleController((BaseElement) this));
}