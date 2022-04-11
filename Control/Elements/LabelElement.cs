// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.LabelElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace DiagramNet.Elements;

[TypeConverter(typeof (ExpandableObjectConverter))]
[Serializable]
public sealed class LabelElement : BaseElement, ISerializable, IControllable
{
  private Color _foreColor1 = Color.Black;
  private Color _foreColor2 = Color.Empty;
  private Color _backColor1 = Color.Empty;
  private Color _backColor2 = Color.Empty;
  [NonSerialized]
  private RectangleController _controller;
  private string _text = "";
  private bool _autoSize;
  [NonSerialized]
  private Font _font = new Font(FontFamily.GenericSansSerif, 8f);
  [NonSerialized]
  private readonly StringFormat _format = new StringFormat(StringFormatFlags.NoWrap);
  private StringAlignment _alignment;
  private StringAlignment _lineAlignment;
  private StringTrimming _trimming;
  private bool _wrap;
  private bool _vertical;
  private bool _readOnly;

  public LabelElement()
    : this(0, 0, 100, 100)
  {
  }

  public LabelElement(Rectangle rec)
    : this(rec.Location, rec.Size)
  {
  }

  public LabelElement(Point l, Size s)
    : this(l.X, l.Y, s.Width, s.Height)
  {
  }

  public LabelElement(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
    this.Alignment = StringAlignment.Center;
    this.LineAlignment = StringAlignment.Center;
    this.Trimming = StringTrimming.Character;
    this.Vertical = false;
    this.Wrap = true;
    this.BorderColorValue = Color.Transparent;
  }

  public string Text
  {
    get => this._text;
    set
    {
      this._text = value;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public Font Font
  {
    get => this._font;
    set
    {
      this._font = value;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public StringAlignment Alignment
  {
    get => this._alignment;
    set
    {
      this._alignment = value;
      this._format.Alignment = this._alignment;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public StringAlignment LineAlignment
  {
    get => this._lineAlignment;
    set
    {
      this._lineAlignment = value;
      this._format.LineAlignment = this._lineAlignment;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public StringTrimming Trimming
  {
    get => this._trimming;
    set
    {
      this._trimming = value;
      this._format.Trimming = this._trimming;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public bool Wrap
  {
    get => this._wrap;
    set
    {
      this._wrap = value;
      if (this._wrap)
        this._format.FormatFlags &= ~StringFormatFlags.NoWrap;
      else
        this._format.FormatFlags |= StringFormatFlags.NoWrap;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public bool Vertical
  {
    get => this._vertical;
    set
    {
      this._vertical = value;
      if (this._vertical)
        this._format.FormatFlags |= StringFormatFlags.DirectionVertical;
      else
        this._format.FormatFlags &= ~StringFormatFlags.DirectionVertical;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public bool ReadOnly
  {
    get => this._readOnly;
    set
    {
      this._readOnly = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public Color ForeColor1
  {
    get => this._foreColor1;
    set
    {
      this._foreColor1 = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public Color ForeColor2
  {
    get => this._foreColor2;
    set
    {
      this._foreColor2 = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public Color BackColor1
  {
    get => this._backColor1;
    set
    {
      this._backColor1 = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public Color BackColor2
  {
    get => this._backColor2;
    set
    {
      this._backColor2 = value;
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public bool AutoSize
  {
    get => this._autoSize;
    set
    {
      this._autoSize = value;
      if (this._autoSize)
        this.DoAutoSize();
      this.OnAppearanceChanged(new EventArgs());
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      this.SizeValue = value;
      if (this._autoSize)
        this.DoAutoSize();
      base.Size = this.SizeValue;
    }
  }

  internal StringFormat Format => this._format;

  public void DoAutoSize()
  {
    if (this._text.Length == 0)
      return;
    Size size = Size.Round(Graphics.FromImage((Image) new Bitmap(1, 1)).MeasureString(this._text, this._font, this.SizeValue.Width, this._format));
    if (this.SizeValue.Height >= size.Height)
      return;
    this.SizeValue.Height = size.Height;
  }

  private Brush GetBrushBackColor(Rectangle r)
  {
    Color color;
    Color color2;
    if (this.OpacityValue == 100)
    {
      color = this._backColor1;
      color2 = this._backColor2;
    }
    else
    {
      color = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this._backColor1);
      color2 = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this._backColor2);
    }
    return !(this._backColor2 == Color.Empty) ? (Brush) new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : (Brush) new SolidBrush(color);
  }

  private Brush GetBrushForeColor(Rectangle r)
  {
    Color color;
    Color color2;
    if (this.OpacityValue == 100)
    {
      color = this._foreColor1;
      color2 = this._foreColor2;
    }
    else
    {
      color = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this._foreColor1);
      color2 = Color.FromArgb((int) ((double) byte.MaxValue * ((double) this.OpacityValue / 100.0)), this._foreColor2);
    }
    return !(this._foreColor2 == Color.Empty) ? (Brush) new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : (Brush) new SolidBrush(color);
  }

  internal override void Draw(Graphics g)
  {
    Rectangle unsignedRectangle = this.GetUnsignedRectangle();
    g.FillRectangle(this.GetBrushBackColor(unsignedRectangle), unsignedRectangle);
    Brush brushForeColor = this.GetBrushForeColor(unsignedRectangle);
    g.DrawString(this._text, this._font, brushForeColor, (RectangleF) unsignedRectangle, this._format);
    this.DrawBorder(g, unsignedRectangle);
    brushForeColor.Dispose();
  }

  private void DrawBorder(Graphics g, Rectangle r)
  {
    Pen pen = new Pen(this.BorderColorValue, (float) this.BorderWidthValue);
    g.DrawRectangle(pen, r);
    pen.Dispose();
  }

  private LabelElement(SerializationInfo info, StreamingContext context)
  {
    Type type = typeof (LabelElement);
    foreach (MemberInfo serializableMember in FormatterServices.GetSerializableMembers(type, context))
    {
      if (!(serializableMember.DeclaringType == type))
      {
        FieldInfo fieldInfo = (FieldInfo) serializableMember;
        fieldInfo.SetValue((object) this, info.GetValue(fieldInfo.Name, fieldInfo.FieldType));
      }
    }
    this.ForeColor1 = (Color) info.GetValue("foreColor1", typeof (Color));
    this.ForeColor2 = (Color) info.GetValue("foreColor2", typeof (Color));
    this.BackColor1 = (Color) info.GetValue("backColor1", typeof (Color));
    this.BackColor2 = (Color) info.GetValue("backColor2", typeof (Color));
    this.Text = info.GetString("text");
    this.Alignment = (StringAlignment) info.GetValue("alignment", typeof (StringAlignment));
    this.LineAlignment = (StringAlignment) info.GetValue("lineAlignment", typeof (StringAlignment));
    this.Trimming = (StringTrimming) info.GetValue("trimming", typeof (StringTrimming));
    this.Wrap = info.GetBoolean("wrap");
    this.Vertical = info.GetBoolean("vertical");
    this.ReadOnly = info.GetBoolean("readOnly");
    this.AutoSize = info.GetBoolean("autoSize");
    this.Font = (Font) new FontConverter().ConvertFromString(info.GetString("font"));
  }

  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("foreColor1", (object) this._foreColor1);
    info.AddValue("foreColor2", (object) this._foreColor2);
    info.AddValue("backColor1", (object) this._backColor1);
    info.AddValue("backColor2", (object) this._backColor2);
    info.AddValue("text", (object) this._text);
    info.AddValue("alignment", (object) this._alignment);
    info.AddValue("lineAlignment", (object) this._lineAlignment);
    info.AddValue("trimming", (object) this._trimming);
    info.AddValue("wrap", this._wrap);
    info.AddValue("vertical", this._vertical);
    info.AddValue("readOnly", this._readOnly);
    info.AddValue("autoSize", this._autoSize);
    FontConverter fontConverter = new FontConverter();
    info.AddValue("font", (object) fontConverter.ConvertToString((object) this._font));
    Type type = typeof (LabelElement);
    foreach (MemberInfo serializableMember in FormatterServices.GetSerializableMembers(type, context))
    {
      if (!(serializableMember.DeclaringType == type))
        info.AddValue(serializableMember.Name, ((FieldInfo) serializableMember).GetValue((object) this));
    }
  }

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RectangleController((BaseElement) this));

  internal void PositionBySite(BaseElement site)
  {
    Point empty = Point.Empty;
    Point location = site.Location;
    Size size1 = site.Size;
    Size size2 = this.Size;
    empty.X = location.X + size1.Width / 2 - size2.Width / 2;
    empty.Y = location.Y + size1.Height / 2 - size2.Height / 2;
    this.Location = empty;
  }
}