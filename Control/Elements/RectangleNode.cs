﻿// Decompiled with JetBrains decompiler
// Type: DiagramNet.Elements.RectangleNode
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet.Elements;

using Controllers;

[Serializable]
public class RectangleNode : NodeElement, IControllable, ILabelElement
{
  protected RectangleElement Rectangle;
  protected LabelElement LabelElement = new();
  [NonSerialized]
  private RectangleController _controller;

  public RectangleNode()
    : this(0, 0, 100, 100)
  {
  }

  public RectangleNode(Rectangle rec)
    : this(rec.Location, rec.Size)
  {
  }

  public RectangleNode(Point l, Size s)
    : this(l.X, l.Y, s.Width, s.Height)
  {
  }

  public RectangleNode(int top, int left, int width, int height)
    : base(top, left, width, height)
  {
    Rectangle = new RectangleElement(top, left, width, height);
    SyncContructors();
  }

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

  public virtual LabelElement Label
  {
    get => LabelElement;
    set
    {
      LabelElement = value;
      OnAppearanceChanged(EventArgs.Empty);
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

  internal override void Draw(Graphics g)
  {
    IsInvalidated = false;
    Rectangle.Draw(g);
  }

  IController IControllable.GetController() => (IController) _controller ?? (_controller = new RectangleController(this));
}