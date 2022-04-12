// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.EllipseNode
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;

[Serializable]
public class EllipseNode : NodeElement, IControllable, ILabelElement
{
  private readonly EllipseElement _ellipse;
  private LabelElement _label = new();
  [NonSerialized]
  private EllipseController _controller;

  public EllipseNode()
    : this(0, 0, 100, 100)
  {
  }

  public EllipseNode(Rectangle rec)
    : this(rec.Location, rec.Size)
  {
  }

  public EllipseNode(Point l, Size s)
    : this(l.X, l.Y, s.Width, s.Height)
  {
  }

  public EllipseNode(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
    _ellipse = new EllipseElement(top, left, width, height);
    SyncContructors();
  }

  public override Color BorderColor
  {
    get => base.BorderColor;
    set
    {
      _ellipse.BorderColor = value;
      base.BorderColor = value;
    }
  }

  public Color FillColor1
  {
    get => _ellipse.FillColor1;
    set => _ellipse.FillColor1 = value;
  }

  public Color FillColor2
  {
    get => _ellipse.FillColor2;
    set => _ellipse.FillColor2 = value;
  }

  public override int Opacity
  {
    get => base.Opacity;
    set
    {
      _ellipse.Opacity = value;
      base.Opacity = value;
    }
  }

  public override bool Visible
  {
    get => base.Visible;
    set
    {
      _ellipse.Visible = value;
      base.Visible = value;
    }
  }

  public override Point Location
  {
    get => base.Location;
    set
    {
      _ellipse.Location = value;
      base.Location = value;
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      _ellipse.Size = value;
      base.Size = value;
    }
  }

  public override int BorderWidth
  {
    get => base.BorderWidth;
    set
    {
      _ellipse.BorderWidth = value;
      base.BorderWidth = value;
    }
  }

  public virtual LabelElement Label
  {
    get => _label;
    set
    {
      _label = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  private void SyncContructors()
  {
    LocationValue = _ellipse.Location;
    SizeValue = _ellipse.Size;
    BorderColorValue = _ellipse.BorderColor;
    BorderWidthValue = _ellipse.BorderWidth;
    OpacityValue = _ellipse.Opacity;
    VisibleValue = _ellipse.Visible;
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    _ellipse.Draw(g);
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new EllipseController(this));
}