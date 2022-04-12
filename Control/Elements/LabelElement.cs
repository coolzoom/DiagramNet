// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.LabelElement
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

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
  private Font _font = new(FontFamily.GenericSansSerif, 8f);
  [NonSerialized]
  private readonly StringFormat _format = new(StringFormatFlags.NoWrap);
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
    Alignment = StringAlignment.Center;
    LineAlignment = StringAlignment.Center;
    Trimming = StringTrimming.Character;
    Vertical = false;
    Wrap = true;
    BorderColorValue = Color.Transparent;
  }

  public string Text
  {
    get => _text;
    set
    {
      _text = value;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public Font Font
  {
    get => _font;
    set
    {
      _font = value;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public StringAlignment Alignment
  {
    get => _alignment;
    set
    {
      _alignment = value;
      _format.Alignment = _alignment;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public StringAlignment LineAlignment
  {
    get => _lineAlignment;
    set
    {
      _lineAlignment = value;
      _format.LineAlignment = _lineAlignment;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public StringTrimming Trimming
  {
    get => _trimming;
    set
    {
      _trimming = value;
      _format.Trimming = _trimming;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public bool Wrap
  {
    get => _wrap;
    set
    {
      _wrap = value;
      if (_wrap)
        _format.FormatFlags &= ~StringFormatFlags.NoWrap;
      else
        _format.FormatFlags |= StringFormatFlags.NoWrap;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public bool Vertical
  {
    get => _vertical;
    set
    {
      _vertical = value;
      if (_vertical)
        _format.FormatFlags |= StringFormatFlags.DirectionVertical;
      else
        _format.FormatFlags &= ~StringFormatFlags.DirectionVertical;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public bool ReadOnly
  {
    get => _readOnly;
    set
    {
      _readOnly = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public Color ForeColor1
  {
    get => _foreColor1;
    set
    {
      _foreColor1 = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public Color ForeColor2
  {
    get => _foreColor2;
    set
    {
      _foreColor2 = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public Color BackColor1
  {
    get => _backColor1;
    set
    {
      _backColor1 = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public Color BackColor2
  {
    get => _backColor2;
    set
    {
      _backColor2 = value;
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public bool AutoSize
  {
    get => _autoSize;
    set
    {
      _autoSize = value;
      if (_autoSize)
        DoAutoSize();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      SizeValue = value;
      if (_autoSize)
        DoAutoSize();
      base.Size = SizeValue;
    }
  }

  internal StringFormat Format => _format;

  public void DoAutoSize()
  {
    if (_text.Length == 0)
      return;
    var size = Size.Round(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(_text, _font, SizeValue.Width, _format));
    if (SizeValue.Height >= size.Height)
      return;
    SizeValue.Height = size.Height;
  }

  private Brush GetBrushBackColor(Rectangle r)
  {
    Color color;
    Color color2;
    if (OpacityValue == 100)
    {
      color = _backColor1;
      color2 = _backColor2;
    }
    else
    {
      color = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), _backColor1);
      color2 = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), _backColor2);
    }
    return !(_backColor2 == Color.Empty) ? new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : new SolidBrush(color);
  }

  private Brush GetBrushForeColor(Rectangle r)
  {
    Color color;
    Color color2;
    if (OpacityValue == 100)
    {
      color = _foreColor1;
      color2 = _foreColor2;
    }
    else
    {
      color = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), _foreColor1);
      color2 = Color.FromArgb((int) (byte.MaxValue * (OpacityValue / 100.0)), _foreColor2);
    }
    return !(_foreColor2 == Color.Empty) ? new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1), color, color2, LinearGradientMode.Horizontal) : new SolidBrush(color);
  }

  internal override void Draw(Graphics g)
  {
    var unsignedRectangle = GetUnsignedRectangle();
    g.FillRectangle(GetBrushBackColor(unsignedRectangle), unsignedRectangle);
    var brushForeColor = GetBrushForeColor(unsignedRectangle);
    g.DrawString(_text, _font, brushForeColor, unsignedRectangle, _format);
    DrawBorder(g, unsignedRectangle);
    brushForeColor.Dispose();
  }

  private void DrawBorder(Graphics g, Rectangle r)
  {
    var pen = new Pen(BorderColorValue, BorderWidthValue);
    g.DrawRectangle(pen, r);
    pen.Dispose();
  }

  private LabelElement(SerializationInfo info, StreamingContext context)
  {
    var type = typeof (LabelElement);
    foreach (var serializableMember in FormatterServices.GetSerializableMembers(type, context))
    {
      if (!(serializableMember.DeclaringType == type))
      {
        var fieldInfo = (FieldInfo) serializableMember;
        fieldInfo.SetValue(this, info.GetValue(fieldInfo.Name, fieldInfo.FieldType));
      }
    }
    ForeColor1 = (Color) info.GetValue("foreColor1", typeof (Color));
    ForeColor2 = (Color) info.GetValue("foreColor2", typeof (Color));
    BackColor1 = (Color) info.GetValue("backColor1", typeof (Color));
    BackColor2 = (Color) info.GetValue("backColor2", typeof (Color));
    Text = info.GetString("text");
    Alignment = (StringAlignment) info.GetValue("alignment", typeof (StringAlignment));
    LineAlignment = (StringAlignment) info.GetValue("lineAlignment", typeof (StringAlignment));
    Trimming = (StringTrimming) info.GetValue("trimming", typeof (StringTrimming));
    Wrap = info.GetBoolean("wrap");
    Vertical = info.GetBoolean("vertical");
    ReadOnly = info.GetBoolean("readOnly");
    AutoSize = info.GetBoolean("autoSize");
    Font = (Font) new FontConverter().ConvertFromString(info.GetString("font"));
  }

  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("foreColor1", _foreColor1);
    info.AddValue("foreColor2", _foreColor2);
    info.AddValue("backColor1", _backColor1);
    info.AddValue("backColor2", _backColor2);
    info.AddValue("text", _text);
    info.AddValue("alignment", _alignment);
    info.AddValue("lineAlignment", _lineAlignment);
    info.AddValue("trimming", _trimming);
    info.AddValue("wrap", _wrap);
    info.AddValue("vertical", _vertical);
    info.AddValue("readOnly", _readOnly);
    info.AddValue("autoSize", _autoSize);
    var fontConverter = new FontConverter();
    info.AddValue("font", fontConverter.ConvertToString(_font));
    var type = typeof (LabelElement);
    foreach (var serializableMember in FormatterServices.GetSerializableMembers(type, context))
    {
      if (!(serializableMember.DeclaringType == type))
        info.AddValue(serializableMember.Name, ((FieldInfo) serializableMember).GetValue(this));
    }
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RectangleController(this));

  internal void PositionBySite(BaseElement site)
  {
    var empty = Point.Empty;
    var location = site.Location;
    var size1 = site.Size;
    var size2 = Size;
    empty.X = location.X + size1.Width / 2 - size2.Width / 2;
    empty.Y = location.Y + size1.Height / 2 - size2.Height / 2;
    Location = empty;
  }
}