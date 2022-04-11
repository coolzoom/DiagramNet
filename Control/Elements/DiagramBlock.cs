// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.DiagramBlock
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements.Controllers;
using DiagramNet.Events;
using System.Reflection;

namespace DiagramNet.Elements;

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
    : base(useNextPosition ? DiagramBlock._nextPosition : 0, useNextPosition ? DiagramBlock._nextPosition : 0, 80, 80)
  {
    this.Overrided = true;
    this.Rectangle = new RectangleElement(DiagramBlock._nextPosition, DiagramBlock._nextPosition, 80, 80);
    this.FillColor1 = Color.White;
    this.FillColor2 = Color.White;
    if (!autoRefresh)
      return;
    this.Refresh(image, labelText, blockState, inputStates, outputStates, connectionTextProperty);
  }

  public DiagramBlock(
    Image image,
    string labelText,
    object blockState,
    object[] inputStates,
    object[] outputStates,
    PropertyInfo connectionTextProperty)
    : base(DiagramBlock._nextPosition, DiagramBlock._nextPosition, 80, 80)
  {
    this.Overrided = true;
    this.Rectangle = new RectangleElement(DiagramBlock._nextPosition, DiagramBlock._nextPosition, 80, 80);
    this.FillColor1 = Color.White;
    this.FillColor2 = Color.White;
    this.Refresh(image, labelText, blockState, inputStates, outputStates, connectionTextProperty);
    DiagramBlock._nextPosition += 20;
    if (DiagramBlock._nextPosition <= 200)
      return;
    DiagramBlock._nextPosition = 50;
  }

  public void Refresh(
    Image image,
    string labelText,
    object blockState,
    object[] inputStates,
    object[] outputStates,
    PropertyInfo connectionTextProperty)
  {
    this._image = image;
    this._labelText = labelText;
    this._connectionTextProperty = connectionTextProperty;
    this.State = blockState;
    this.SyncContructors();
    this._inputStates = inputStates;
    this._outputStates = outputStates;
    if (this.Connects == null || this.Connects.Length != inputStates.Length + outputStates.Length)
    {
      this.Connects = new ConnectorElement[inputStates.Length + outputStates.Length];
      for (int index = 0; index < inputStates.Length; ++index)
        this.Connects[index] = new ConnectorElement((NodeElement) this)
        {
          State = inputStates[index]
        };
      for (int index = 0; index < outputStates.Length; ++index)
        this.Connects[inputStates.Length + index] = new ConnectorElement((NodeElement) this)
        {
          State = outputStates[index]
        };
    }
    else
    {
      for (int index = 0; index < inputStates.Length; ++index)
        this.Connects[index].State = inputStates[index];
      for (int index = 0; index < outputStates.Length; ++index)
        this.Connects[inputStates.Length + index].State = outputStates[index];
    }
    this.UpdateConnectorsPosition();
    this.SyncContructors();
  }

  protected new void UpdateConnectorsPosition()
  {
    for (int index = 0; index < this._inputStates.Length; ++index)
    {
      int num = 0;
      if (this._inputStates.Length > 1 && this._inputStates.Length < 5)
        num = 20;
      Point point = new Point(this.LocationValue.X, this.LocationValue.Y + ((this.SizeValue.Height - num) / (this._inputStates.Length + 1) * (index + 1) - 1 - 2 - num / 4));
      ConnectorElement connect = this.Connects[index];
      connect.Location = new Point(point.X - 3, point.Y);
      connect.Size = new Size(6, 6);
      connect.IsStart = true;
      connect.State = this._inputStates[index];
    }
    for (int index = 0; index < this._outputStates.Length; ++index)
    {
      int num = 0;
      if (this._outputStates.Length > 1 && this._outputStates.Length < 5)
        num = 20;
      Point point = new Point(this.LocationValue.X + this.SizeValue.Width, this.LocationValue.Y + ((this.SizeValue.Height - num) / (this._outputStates.Length + 1) * (index + 1) - 1 - 2 - num / 4));
      ConnectorElement connect = this.Connects[this._inputStates.Length + index];
      connect.Location = new Point(point.X - 3, point.Y);
      connect.Size = new Size(6, 6);
      connect.IsStart = false;
      connect.State = this._outputStates[index];
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

  public Image GetImage()
  {
    Bitmap image = new Bitmap(87, 81);
    Graphics g = Graphics.FromImage((Image) image);
    g.TranslateTransform(-47f, -50f);
    this.Draw(g);
    foreach (BaseElement connect in this.Connects)
      connect.Draw(g);
    return (Image) image;
  }

  internal override void Draw(Graphics g)
  {
    this.IsInvalidated = false;
    ImageElement imageElement = new ImageElement(this._image, (BaseElement) this.Rectangle);
    LabelElement labelElement = new LabelElement(this.Rectangle.Location.X, imageElement.Top + imageElement.Height + 2, this.Rectangle.Size.Width, 12)
    {
      Text = this._labelText,
      Font = new Font(FontFamily.GenericSansSerif, 8f)
    };
    this.Rectangle.Draw(g);
    imageElement.Draw(g);
    labelElement.Draw(g);
    foreach (ConnectorElement connect in this.Connects)
    {
      string str = this._connectionTextProperty.GetValue(connect.State, (object[]) null).ToString();
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

  IController IControllable.GetController() => (IController) this._controller ?? (IController) (this._controller = new RectangleController((BaseElement) this));

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
      this.UpdateConnectorsPosition();
      this.OnAppearanceChanged(new EventArgs());
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

  public bool OnElementConnected(Designer designer, ElementConnectEventArgs e)
  {
    if (e.Link.Connector1.IsStart == e.Link.Connector2.IsStart)
    {
      designer.Document.DeleteLink(e.Link, false);
      return false;
    }
    if (e.Link.Connector1.IsStart)
    {
      ConnectorElement connector1 = e.Link.Connector1;
      e.Link.Connector1 = e.Link.Connector2;
      e.Link.Connector2 = connector1;
      e.Link.Invalidate();
    }
    for (int index = designer.Document.Elements.Count - 1; index >= 0; --index)
    {
      if (e.Link != designer.Document.Elements[index] && designer.Document.Elements[index] is BaseLinkElement)
      {
        BaseLinkElement element = (BaseLinkElement) designer.Document.Elements[index];
        if (element.Connector2 == e.Link.Connector2)
          designer.Document.DeleteLink(element, false);
      }
    }
    return true;
  }
}