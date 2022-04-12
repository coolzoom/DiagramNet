// Decompiled with JetBrains decompiler
// Type: DiagramNet.ElementCollection
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using System.Collections.ObjectModel;

[Serializable]
public class ElementCollection : Collection<BaseElement>
{
  public const int MaxIntSize = 100;
  private Point _location = new(100, 100);
  private Size _size = new(0, 0);
  private bool _enabledCalc = true;
  private bool _needCalc = true;

  public new virtual void Add(BaseElement element)
  {
    _needCalc = true;
    base.Add(element);
  }

  public new void Insert(int index, BaseElement element)
  {
    _needCalc = true;
    base.Insert(index, element);
  }

  public new void Remove(BaseElement element)
  {
    base.Remove(element);
    _needCalc = true;
  }

  public new void Clear()
  {
    base.Clear();
    _needCalc = true;
  }

  public void ChangeIndex(int i, int y)
  {
    (this[y], this[i]) = (this[i], this[y]);
  }

  public BaseElement[] GetArray()
  {
    var array = new BaseElement[Count];
    for (var index = 0; index <= Count - 1; ++index)
      array[index] = this[index];
    return array;
  }

  public BaseElement[] GetArrayClone()
  {
    var arrayClone = new BaseElement[Count];
    for (var index = 0; index <= Count - 1; ++index)
      arrayClone[index] = this[index].Clone();
    return arrayClone;
  }

  internal bool EnabledCalc
  {
    get => _enabledCalc;
    set
    {
      _enabledCalc = value;
      if (!_enabledCalc)
        return;
      _needCalc = true;
    }
  }

  internal Point WindowLocation
  {
    get
    {
      CalcWindow();
      return _location;
    }
  }

  internal Size WindowSize
  {
    get
    {
      CalcWindow();
      return _size;
    }
  }

  internal void CalcWindow(bool forceCalc)
  {
    if (forceCalc)
      _needCalc = true;
    CalcWindow();
  }

  internal void CalcWindow()
  {
    if (!_enabledCalc || !_needCalc)
      return;
    _location.X = 100;
    _location.Y = 100;
    _size.Width = 0;
    _size.Height = 0;
    foreach (BaseElement element in this)
      CalcWindowLocation(element);
    foreach (BaseElement element in this)
      CalcWindowSize(element);
    _needCalc = false;
  }

  internal void CalcWindowLocation(BaseElement element)
  {
    if (!_enabledCalc)
      return;
    var location = element.Location;
    if (location.X < _location.X)
      _location.X = location.X;
    if (location.Y >= _location.Y)
      return;
    _location.Y = location.Y;
  }

  internal void CalcWindowSize(BaseElement element)
  {
    if (!_enabledCalc)
      return;
    var location = element.Location;
    var size = element.Size;
    var num1 = location.X + size.Width - _location.X;
    if (num1 > _size.Width)
      _size.Width = num1;
    var num2 = location.Y + size.Height - _location.Y;
    if (num2 <= _size.Height)
      return;
    _size.Height = num2;
  }
}
