// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.ElipseNode
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System;
using System.Drawing;

namespace DiagramNet.Elements
{
  [Serializable]
  public class ElipseNode : NodeElement, IControllable, ILabelElement
  {
    private readonly ElipseElement _elipse;
    private LabelElement _label = new LabelElement();
    [NonSerialized]
    private ElipseController _controller;

    public ElipseNode()
      : this(0, 0, 100, 100)
    {
    }

    public ElipseNode(Rectangle rec)
      : this(rec.Location, rec.Size)
    {
    }

    public ElipseNode(Point l, Size s)
      : this(l.X, l.Y, s.Width, s.Height)
    {
    }

    public ElipseNode(int top, int left, int width, int height)
      : base(top, left, width, height)
    {
      this._elipse = new ElipseElement(top, left, width, height);
      this.SyncContructors();
    }

    public override Color BorderColor
    {
      get => base.BorderColor;
      set
      {
        this._elipse.BorderColor = value;
        base.BorderColor = value;
      }
    }

    public Color FillColor1
    {
      get => this._elipse.FillColor1;
      set => this._elipse.FillColor1 = value;
    }

    public Color FillColor2
    {
      get => this._elipse.FillColor2;
      set => this._elipse.FillColor2 = value;
    }

    public override int Opacity
    {
      get => base.Opacity;
      set
      {
        this._elipse.Opacity = value;
        base.Opacity = value;
      }
    }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        this._elipse.Visible = value;
        base.Visible = value;
      }
    }

    public override Point Location
    {
      get => base.Location;
      set
      {
        this._elipse.Location = value;
        base.Location = value;
      }
    }

    public override Size Size
    {
      get => base.Size;
      set
      {
        this._elipse.Size = value;
        base.Size = value;
      }
    }

    public override int BorderWidth
    {
      get => base.BorderWidth;
      set
      {
        this._elipse.BorderWidth = value;
        base.BorderWidth = value;
      }
    }

    public virtual LabelElement Label
    {
      get => this._label;
      set
      {
        this._label = value;
        this.OnAppearanceChanged(new EventArgs());
      }
    }

    private void SyncContructors()
    {
      this.LocationValue = this._elipse.Location;
      this.SizeValue = this._elipse.Size;
      this.BorderColorValue = this._elipse.BorderColor;
      this.BorderWidthValue = this._elipse.BorderWidth;
      this.OpacityValue = this._elipse.Opacity;
      this.VisibleValue = this._elipse.Visible;
    }

    internal override void Draw(Graphics g)
    {
      this.IsInvalidated = false;
      this._elipse.Draw(g);
    }

    IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new ElipseController((BaseElement) this));
  }
}
