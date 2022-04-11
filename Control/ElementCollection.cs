// Decompiled with JetBrains decompiler
// Type: DiagramNet.ElementCollection
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

using DiagramNet.Elements;
using System.Collections;

namespace DiagramNet;

[Serializable]
public class ElementCollection : ReadOnlyCollectionBase
{
  public const int MaxIntSize = 100;
  private Point _location = new Point(100, 100);
  private Size _size = new Size(0, 0);
  private bool _enabledCalc = true;
  private bool _needCalc = true;

  internal ElementCollection()
  {
  }

  public BaseElement this[int item] => (BaseElement) this.InnerList[item];

  public virtual int Add(BaseElement element)
  {
    this._needCalc = true;
    return this.InnerList.Add((object) element);
  }

  public bool Contains(BaseElement element) => this.InnerList.Contains((object) element);

  public int IndexOf(BaseElement element) => this.InnerList.IndexOf((object) element);

  internal void Insert(int index, BaseElement element)
  {
    this._needCalc = true;
    this.InnerList.Insert(index, (object) element);
  }

  internal void Remove(BaseElement element)
  {
    this.InnerList.Remove((object) element);
    this._needCalc = true;
  }

  internal void Clear()
  {
    this.InnerList.Clear();
    this._needCalc = true;
  }

  internal void ChangeIndex(int i, int y)
  {
    object inner = this.InnerList[y];
    this.InnerList[y] = this.InnerList[i];
    this.InnerList[i] = inner;
  }

  public BaseElement[] GetArray()
  {
    BaseElement[] array = new BaseElement[this.InnerList.Count];
    for (int index = 0; index <= this.InnerList.Count - 1; ++index)
      array[index] = (BaseElement) this.InnerList[index];
    return array;
  }

  public BaseElement[] GetArrayClone()
  {
    BaseElement[] arrayClone = new BaseElement[this.InnerList.Count];
    for (int index = 0; index <= this.InnerList.Count - 1; ++index)
      arrayClone[index] = ((BaseElement) this.InnerList[index]).Clone();
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
    foreach (BaseElement element in (ReadOnlyCollectionBase) this)
      this.CalcWindowLocation(element);
    foreach (BaseElement element in (ReadOnlyCollectionBase) this)
      this.CalcWindowSize(element);
    this._needCalc = false;
  }

  internal void CalcWindowLocation(BaseElement element)
  {
    if (!this._enabledCalc)
      return;
    Point location = element.Location;
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
    Point location = element.Location;
    Size size = element.Size;
    int num1 = location.X + size.Width - this._location.X;
    if (num1 > this._size.Width)
      this._size.Width = num1;
    int num2 = location.Y + size.Height - this._location.Y;
    if (num2 <= this._size.Height)
      return;
    this._size.Height = num2;
  }

  public class BaseElementEnumarator : IEnumerator
  {
    private readonly IEnumerator _baseEnumarator;
    private readonly IEnumerable _tmp;

    private BaseElementEnumarator(IEnumerable mapping)
    {
      this._tmp = mapping;
      this._baseEnumarator = this._tmp.GetEnumerator();
    }

    void IEnumerator.Reset() => this._baseEnumarator.Reset();

    bool IEnumerator.MoveNext() => this._baseEnumarator.MoveNext();

    object IEnumerator.Current => this._baseEnumarator.Current;

    public void Reset() => this._baseEnumarator.Reset();

    public bool MoveNext() => this._baseEnumarator.MoveNext();

    public BaseElement Current => (BaseElement) this._baseEnumarator.Current;
  }
}