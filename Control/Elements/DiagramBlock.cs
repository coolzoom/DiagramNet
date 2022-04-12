// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.DiagramBlock
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;
using Events;
using System.Reflection;

[Serializable]
public class DiagramBlock : NodeElement, IControllable
{
  private const int BlockSize = 80;
  [NonSerialized]
  private RectangleController _controller;
  protected RectangleElement Rectangle;
  private Image _image;
  private string _labelText;
  private object[] _inputStates;
  private object[] _outputStates;
  [NonSerialized]
  private PropertyInfo _connectionTextProperty;
  private static int _nextPosition = 50;

  public object State { get; set; }

  public DiagramBlock()
  {
  }

  public DiagramBlock(
    Image image,
    string labelText,
    object blockState,
    object[] inputStates,
    object[] outputStates,
    PropertyInfo connectionTextProperty,
    bool autoRefresh,
    bool useNextPosition = false)
    : base(useNextPosition ? _nextPosition : 0, useNextPosition ? _nextPosition : 0, 80, 80)
  {
    Overrided = true;
    Rectangle = new RectangleElement(_nextPosition, _nextPosition, 80, 80);
    FillColor1 = Color.White;
    FillColor2 = Color.White;
    if (!autoRefresh)
      return;
    Refresh(image, labelText, blockState, inputStates, outputStates, connectionTextProperty);
  }

  public DiagramBlock(
    Image image,
    string labelText,
    object blockState,
    object[] inputStates,
    object[] outputStates,
    PropertyInfo connectionTextProperty)
    : base(_nextPosition, _nextPosition, 80, 80)
  {
    Overrided = true;
    Rectangle = new RectangleElement(_nextPosition, _nextPosition, 80, 80);
    FillColor1 = Color.White;
    FillColor2 = Color.White;
    Refresh(image, labelText, blockState, inputStates, outputStates, connectionTextProperty);
    _nextPosition += 20;
    if (_nextPosition <= 200)
      return;
    _nextPosition = 50;
  }

  public void Refresh(
    Image image,
    string labelText,
    object blockState,
    object[] inputStates,
    object[] outputStates,
    PropertyInfo connectionTextProperty)
  {
    _image = image;
    _labelText = labelText;
    _connectionTextProperty = connectionTextProperty;
    State = blockState;
    SyncContructors();
    _inputStates = inputStates;
    _outputStates = outputStates;
    if (Connects == null || Connects.Length != inputStates.Length + outputStates.Length)
    {
      Connects = new ConnectorElement[inputStates.Length + outputStates.Length];
      for (var index = 0; index < inputStates.Length; ++index)
        Connects[index] = new ConnectorElement(this)
        {
          State = inputStates[index]
        };
      for (var index = 0; index < outputStates.Length; ++index)
        Connects[inputStates.Length + index] = new ConnectorElement(this)
        {
          State = outputStates[index]
        };
    }
    else
    {
      for (var index = 0; index < inputStates.Length; ++index)
        Connects[index].State = inputStates[index];
      for (var index = 0; index < outputStates.Length; ++index)
        Connects[inputStates.Length + index].State = outputStates[index];
    }
    UpdateConnectorsPosition();
    SyncContructors();
  }

  protected new void UpdateConnectorsPosition()
  {
    for (var index = 0; index < _inputStates.Length; ++index)
    {
      var num = 0;
      if (_inputStates.Length > 1 && _inputStates.Length < 5)
        num = 20;
      var point = new Point(LocationValue.X, LocationValue.Y + ((SizeValue.Height - num) / (_inputStates.Length + 1) * (index + 1) - 1 - 2 - num / 4));
      var connect = Connects[index];
      connect.Location = new Point(point.X - 3, point.Y);
      connect.Size = new Size(6, 6);
      connect.IsStart = true;
      connect.State = _inputStates[index];
    }
    for (var index = 0; index < _outputStates.Length; ++index)
    {
      var num = 0;
      if (_outputStates.Length > 1 && _outputStates.Length < 5)
        num = 20;
      var point = new Point(LocationValue.X + SizeValue.Width, LocationValue.Y + ((SizeValue.Height - num) / (_outputStates.Length + 1) * (index + 1) - 1 - 2 - num / 4));
      var connect = Connects[_inputStates.Length + index];
      connect.Location = new Point(point.X - 3, point.Y);
      connect.Size = new Size(6, 6);
      connect.IsStart = false;
      connect.State = _outputStates[index];
    }
  }

  private void SyncContructors()
  {
    LocationValue = Rectangle.Location;
    SizeValue = Rectangle.Size;
    BorderColorValue = Rectangle.BorderColor;
    BorderWidthValue = Rectangle.BorderWidth;
    OpacityValue = Rectangle.Opacity;
    VisibleValue = Rectangle.Visible;
  }

  public Image GetImage()
  {
    var image = new Bitmap(87, 81);
    var g = Graphics.FromImage(image);
    g.TranslateTransform(-47f, -50f);
    Draw(g);
    foreach (BaseElement connect in Connects)
      connect.Draw(g);
    return image;
  }

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    var imageElement = new ImageElement(_image, Rectangle);
    var labelElement = new LabelElement(Rectangle.Location.X, imageElement.Top + imageElement.Height + 2, Rectangle.Size.Width, 12)
    {
      Text = _labelText,
      Font = new Font(FontFamily.GenericSansSerif, 8f)
    };
    Rectangle.Draw(g);
    imageElement.Draw(g);
    labelElement.Draw(g);
    foreach (var connect in Connects)
    {
      var str = _connectionTextProperty.GetValue(connect.State, null).ToString();
      int top;
      StringAlignment stringAlignment;
      if (connect.IsStart)
      {
        top = connect.Location.X + connect.Size.Width + 2;
        stringAlignment = StringAlignment.Near;
      }
      else
      {
        top = connect.Location.X - 42;
        stringAlignment = StringAlignment.Far;
      }
      new LabelElement(top, connect.Location.Y - connect.Size.Height / 2, 40, 12)
      {
        Text = str,
        Alignment = stringAlignment,
        Font = new Font(FontFamily.GenericSansSerif, 7f)
      }.Draw(g);
    }
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RectangleController(this));

  public override Color BorderColor
  {
    get => base.BorderColor;
    set
    {
      Rectangle.BorderColor = value;
      base.BorderColor = value;
    }
  }

  public Color FillColor1
  {
    get => Rectangle.FillColor1;
    set => Rectangle.FillColor1 = value;
  }

  public Color FillColor2
  {
    get => Rectangle.FillColor2;
    set => Rectangle.FillColor2 = value;
  }

  public override int Opacity
  {
    get => base.Opacity;
    set
    {
      Rectangle.Opacity = value;
      base.Opacity = value;
    }
  }

  public override bool Visible
  {
    get => base.Visible;
    set
    {
      Rectangle.Visible = value;
      base.Visible = value;
    }
  }

  public override Point Location
  {
    get => base.Location;
    set
    {
      Rectangle.Location = value;
      base.Location = value;
      UpdateConnectorsPosition();
      OnAppearanceChanged(EventArgs.Empty);
    }
  }

  public override Size Size
  {
    get => base.Size;
    set
    {
      Rectangle.Size = value;
      base.Size = value;
    }
  }

  public override int BorderWidth
  {
    get => base.BorderWidth;
    set
    {
      Rectangle.BorderWidth = value;
      base.BorderWidth = value;
    }
  }

  public bool OnElementConnected(Designer designer, ElementConnectEventArgs e)
  {
    if (e.Link.Connector1.IsStart == e.Link.Connector2.IsStart)
    {
      designer.Document.DeleteLink(e.Link, false);
      return false;
    }
    if (e.Link.Connector1.IsStart)
    {
      var connector1 = e.Link.Connector1;
      e.Link.Connector1 = e.Link.Connector2;
      e.Link.Connector2 = connector1;
      e.Link.Invalidate();
    }
    for (var index = designer.Document.Elements.Count - 1; index >= 0; --index)
    {
      if (e.Link != designer.Document.Elements[index] && designer.Document.Elements[index] is BaseLinkElement)
      {
        var element = (BaseLinkElement) designer.Document.Elements[index];
        if (element.Connector2 == e.Link.Connector2)
          designer.Document.DeleteLink(element, false);
      }
    }
    return true;
  }
}