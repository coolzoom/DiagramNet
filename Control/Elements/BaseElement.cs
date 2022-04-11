// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.BaseElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

[Serializable]
public abstract class BaseElement
{
  protected Point LocationValue;
  protected Size SizeValue;
  protected bool VisibleValue = true;
  protected Color BorderColorValue = Color.Black;
  protected int BorderWidthValue = 1;
  protected int OpacityValue = 100;
  protected internal Rectangle InvalidateRec = Rectangle.Empty;
  protected internal bool IsInvalidated = true;

  protected BaseElement()
  {
  }

  protected BaseElement(int top, int left, int width, int height)
  {
    this.LocationValue = new Point(top, left);
    this.SizeValue = new Size(width, height);
  }

  public virtual Point Location
  {
    get => this.LocationValue;
    set
    {
      this.LocationValue = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual Size Size
  {
    get => this.SizeValue;
    set
    {
      this.SizeValue = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual bool Visible
  {
    get => this.VisibleValue;
    set
    {
      this.VisibleValue = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual Color BorderColor
  {
    get => this.BorderColorValue;
    set
    {
      this.BorderColorValue = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual int BorderWidth
  {
    get => this.BorderWidthValue;
    set
    {
      this.BorderWidthValue = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual int Opacity
  {
    get => this.OpacityValue;
    set
    {
      this.OpacityValue = value >= 0 || value <= 100 ? value : throw new Exception("'" + (object) value + "' is not a valid value for 'Opacity'. 'Opacity' should be between 0 and 100.");
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  internal virtual void Draw(Graphics g) => this.IsInvalidated = false;

  public virtual void Invalidate()
  {
    this.InvalidateRec = this.IsInvalidated ? Rectangle.Union(this.InvalidateRec, this.GetUnsignedRectangle()) : this.GetUnsignedRectangle();
    this.IsInvalidated = true;
  }

  public virtual Rectangle GetRectangle() => new Rectangle(this.Location, this.Size);

  public virtual Rectangle GetUnsignedRectangle() => BaseElement.GetUnsignedRectangle(this.GetRectangle());

  internal static Rectangle GetUnsignedRectangle(Rectangle rec)
  {
    Rectangle unsignedRectangle = rec;
    if (rec.Width < 0)
    {
      unsignedRectangle.X = rec.X + rec.Width;
      unsignedRectangle.Width = -rec.Width;
    }
    if (rec.Height < 0)
    {
      unsignedRectangle.Y = rec.Y + rec.Height;
      unsignedRectangle.Height = -rec.Height;
    }
    return unsignedRectangle;
  }

  [field: NonSerialized]
  public event EventHandler AppearanceChanged;

  protected virtual void OnAppearanceChanged(EventArgs e)
  {
    if (this.AppearanceChanged == null)
      return;
    this.AppearanceChanged((object) this, e);
  }

  public BaseElement Clone() => (BaseElement) this.MemberwiseClone();
}