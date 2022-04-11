// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.LineElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System.Drawing.Drawing2D;

namespace DiagramNet.Elements;

[Serializable]
public class LineElement : BaseElement, IControllable
{
  private Point _point1;
  private Point _point2;
  private LineCap _startCap = LineCap.Round;
  private LineCap _endCap = LineCap.Round;
  private bool _needCalcLine;
  [NonSerialized]
  private LineController _controller;

  internal LineElement(int x1, int y1, int x2, int y2)
    : this(new Point(x1, y1), new Point(x2, y2))
  {
  }

  internal LineElement(Point p1, Point p2)
  {
    this._point1 = p1;
    this._point2 = p2;
    this.BorderWidthValue = 1;
    this.BorderColorValue = Color.Black;
  }

  public virtual Point Point1
  {
    get
    {
      this.CalcLine();
      return this._point1;
    }
    set
    {
      this._point1 = value;
      this._needCalcLine = true;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual Point Point2
  {
    get
    {
      this.CalcLine();
      return this._point2;
    }
    set
    {
      this._point2 = value;
      this._needCalcLine = true;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual LineCap StartCap
  {
    get => this._startCap;
    set
    {
      this._startCap = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual LineCap EndCap
  {
    get => this._endCap;
    set
    {
      this._endCap = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  internal override void Draw(Graphics g)
  {
    this.IsInvalidated = false;
    var pen = new Pen(this.OpacityValue == 100 ? this.BorderColorValue : Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this.BorderColorValue), (float) this.BorderWidthValue)
    {
      StartCap = this._startCap,
      EndCap = this._endCap
    };
    g.DrawLine(pen, this._point1, this._point2);
    pen.Dispose();
  }

  internal void CalcLine()
  {
    if (!this._needCalcLine)
      return;
    if (this._point1.X < this._point2.X)
    {
      this.LocationValue.X = this._point1.X;
      this.SizeValue.Width = this._point2.X - this._point1.X;
    }
    else
    {
      this.LocationValue.X = this._point2.X;
      this.SizeValue.Width = this._point1.X - this._point2.X;
    }
    if (this._point1.Y < this._point2.Y)
    {
      this.LocationValue.Y = this._point1.Y;
      this.SizeValue.Height = this._point2.Y - this._point1.Y;
    }
    else
    {
      this.LocationValue.Y = this._point2.Y;
      this.SizeValue.Height = this._point1.Y - this._point2.Y;
    }
    this._needCalcLine = false;
  }

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new LineController(this));
}