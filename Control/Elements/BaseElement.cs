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
    LocationValue = new Point(top, left);
    SizeValue = new Size(width, height);
  }

  public virtual Point Location
  {
    get => LocationValue;
    set
    {
      LocationValue = value;
      OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual Size Size
  {
    get => SizeValue;
    set
    {
      SizeValue = value;
      OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual bool Visible
  {
    get => VisibleValue;
    set
    {
      VisibleValue = value;
      OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual Color BorderColor
  {
    get => BorderColorValue;
    set
    {
      BorderColorValue = value;
      OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual int BorderWidth
  {
    get => BorderWidthValue;
    set
    {
      BorderWidthValue = value;
      OnAppearanceChanged(new EventArgs());
    }
  }

  public virtual int Opacity
  {
    get => OpacityValue;
    set
    {
      OpacityValue = value >= 0 || value <= 100 ? value : throw new Exception("'" + (object) value + "' is not a valid value for 'Opacity'. 'Opacity' should be between 0 and 100.");
      OnAppearanceChanged(new EventArgs());
    }
  }

  internal virtual void Draw(Graphics g) => IsInvalidated = false;

  public virtual void Invalidate()
  {
    InvalidateRec = IsInvalidated ? Rectangle.Union(InvalidateRec, GetUnsignedRectangle()) : GetUnsignedRectangle();
    IsInvalidated = true;
  }

  public virtual Rectangle GetRectangle() => new(Location, Size);

  public virtual Rectangle GetUnsignedRectangle() => GetUnsignedRectangle(GetRectangle());

  internal static Rectangle GetUnsignedRectangle(Rectangle rec)
  {
    var unsignedRectangle = rec;
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
    if (AppearanceChanged == null)
      return;
    AppearanceChanged((object) this, e);
  }

  public BaseElement Clone() => (BaseElement) MemberwiseClone();
}