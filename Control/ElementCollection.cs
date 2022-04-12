// Decompiled with JetBrains decompiler
// Type: DiagramNet.ElementCollection
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using System.Collections.ObjectModel;

[Serializable]
public class ElementCollection : Collection<BaseElement>
{
  public const int MaxIntSize = 100;
  private Point _location = new Point(100, 100);
  private Size _size = new Size(0, 0);
  private bool _enabledCalc = true;
  private bool _needCalc = true;

  public ElementCollection()
  {
  }

  public new virtual void Add(BaseElement element)
  {
    this._needCalc = true;
    base.Add(element);
  }

  public new void Insert(int index, BaseElement element)
  {
    this._needCalc = true;
    base.Insert(index, element);
  }

  public new void Remove(BaseElement element)
  {
    base.Remove(element);
    this._needCalc = true;
  }

  public new void Clear()
  {
    base.Clear();
    this._needCalc = true;
  }

  public void ChangeIndex(int i, int y)
  {
    var inner = this[y];
    // TODO   this[y] = this[i];
    // TODO   this[i] = inner;
  }

  public BaseElement[] GetArray()
  {
    var array = new BaseElement[this.Count];
    for (var index = 0; index <= this.Count - 1; ++index)
      array[index] = (BaseElement) this[index];
    return array;
  }

  public BaseElement[] GetArrayClone()
  {
    var arrayClone = new BaseElement[this.Count];
    for (var index = 0; index <= this.Count - 1; ++index)
      arrayClone[index] = ((BaseElement) this[index]).Clone();
    return arrayClone;
  }

  internal bool EnabledCalc
  {
    get => this._enabledCalc;
    set
    {
      this._enabledCalc = value;
      if (!this._enabledCalc)
        return;
      this._needCalc = true;
    }
  }

  internal Point WindowLocation
  {
    get
    {
      this.CalcWindow();
      return this._location;
    }
  }

  internal Size WindowSize
  {
    get
    {
      this.CalcWindow();
      return this._size;
    }
  }

  internal void CalcWindow(bool forceCalc)
  {
    if (forceCalc)
      this._needCalc = true;
    this.CalcWindow();
  }

  internal void CalcWindow()
  {
    if (!this._enabledCalc || !this._needCalc)
      return;
    this._location.X = 100;
    this._location.Y = 100;
    this._size.Width = 0;
    this._size.Height = 0;
    foreach (BaseElement element in this)
      this.CalcWindowLocation(element);
    foreach (BaseElement element in this)
      this.CalcWindowSize(element);
    this._needCalc = false;
  }

  internal void CalcWindowLocation(BaseElement element)
  {
    if (!this._enabledCalc)
      return;
    var location = element.Location;
    if (location.X < this._location.X)
      this._location.X = location.X;
    if (location.Y >= this._location.Y)
      return;
    this._location.Y = location.Y;
  }

  internal void CalcWindowSize(BaseElement element)
  {
    if (!this._enabledCalc)
      return;
    var location = element.Location;
    var size = element.Size;
    var num1 = location.X + size.Width - this._location.X;
    if (num1 > this._size.Width)
      this._size.Width = num1;
    var num2 = location.Y + size.Height - this._location.Y;
    if (num2 <= this._size.Height)
      return;
    this._size.Height = num2;
  }
}
