// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.StraightLinkElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DiagramNet.Elements
{
  [Serializable]
  public class StraightLinkElement : BaseLinkElement, IControllable, ILabelElement
  {
    protected LineElement Line1 = new LineElement(0, 0, 0, 0);
    private LabelElement _label = new LabelElement();
    [NonSerialized]
    private LineController _controller;

    internal StraightLinkElement(ConnectorElement conn1, ConnectorElement conn2)
      : base(conn1, conn2)
    {
      this._label.PositionBySite((BaseElement) this.Line1);
    }

    [Browsable(false)]
    public override Point Point1 => this.Line1.Point1;

    [Browsable(false)]
    public override Point Point2 => this.Line1.Point2;

    public override Color BorderColor
    {
      get => this.Line1.BorderColor;
      set => this.Line1.BorderColor = value;
    }

    public override int BorderWidth
    {
      get => this.Line1.BorderWidth;
      set => this.Line1.BorderWidth = value;
    }

    public override Point Location
    {
      get
      {
        this.CalcLink();
        return this.Line1.Location;
      }
    }

    public override Size Size
    {
      get
      {
        this.CalcLink();
        return this.Line1.Size;
      }
    }

    public override int Opacity
    {
      get => this.Line1.Opacity;
      set => this.Line1.Opacity = value;
    }

    public override LineCap StartCap
    {
      get => this.Line1.StartCap;
      set => this.Line1.StartCap = value;
    }

    public override LineCap EndCap
    {
      get => this.Line1.EndCap;
      set => this.Line1.EndCap = value;
    }

    public override LineElement[] Lines => new LineElement[1]
    {
      this.Line1
    };

    internal override void Draw(Graphics g)
    {
      this.IsInvalidated = false;
      this.Line1.Draw(g);
    }

    internal override void CalcLink()
    {
      if (!this.NeedCalcLinkValue)
        return;
      if (this.Line1 != null)
      {
        Point location1 = this.Connector1Value.Location;
        Point location2 = this.Connector2Value.Location;
        Size size1 = this.Connector1Value.Size;
        Size size2 = this.Connector2Value.Size;
        this.Line1.Point1 = new Point(location1.X + size1.Width / 2, location1.Y + size1.Height / 2);
        this.Line1.Point2 = new Point(location2.X + size2.Width / 2, location2.Y + size2.Height / 2);
        this.Line1.CalcLine();
      }
      this.NeedCalcLinkValue = false;
    }

    IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new LineController(this.Line1));

    public virtual LabelElement Label
    {
      get => this._label;
      set
      {
        this._label = value;
        this.OnAppearanceChanged(new EventArgs());
      }
    }
  }
}
