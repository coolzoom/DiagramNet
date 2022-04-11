// Decompiled with JetBrains decompiler
// Type: DiagramNet.UndoManager
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

internal class UndoManager
{
  protected MemoryStream[] List;
  protected int CurrPos = -1;
  protected int LastPos = -1;
  protected int Capacity;

  public UndoManager(int capacity)
  {
    this.Enabled = true;
    this.List = new MemoryStream[capacity];
    this.Capacity = capacity;
  }

  public bool CanUndo => this.CurrPos != -1;

  public bool CanRedo => this.CurrPos != this.LastPos;

  public bool Enabled { get; set; }

  public void AddUndo(object o)
  {
    if (!this.Enabled)
      return;
    ++this.CurrPos;
    if (this.CurrPos >= this.Capacity)
      --this.CurrPos;
    this.ClearList(this.CurrPos);
    this.PushList();
    this.List[this.CurrPos] = this.SerializeObject(o);
    this.LastPos = this.CurrPos;
  }

  public object Undo()
  {
    if (!this.CanUndo)
      throw new ApplicationException("Can't Undo.");
    var obj = this.DeserializeObject((Stream) this.List[this.CurrPos]);
    --this.CurrPos;
    return obj;
  }

  public object Redo()
  {
    if (!this.CanRedo)
      throw new ApplicationException("Can't Undo.");
    ++this.CurrPos;
    return this.DeserializeObject((Stream) this.List[this.CurrPos]);
  }

  private MemoryStream SerializeObject(object o)
  {
    var formatter = (IFormatter) new BinaryFormatter();
    var serializationStream = new MemoryStream();
    formatter.Serialize((Stream) serializationStream, o);
    serializationStream.Position = 0L;
    return serializationStream;
  }

  private object DeserializeObject(Stream mem)
  {
    mem.Position = 0L;
    return new BinaryFormatter().Deserialize(mem);
  }

  private void ClearList(int p = 0)
  {
    if (this.CurrPos >= this.Capacity - 1)
      return;
    for (var index = p; index < this.Capacity; ++index)
    {
      if (this.List[index] != null)
        this.List[index].Close();
      this.List[index] = (MemoryStream) null;
    }
  }

  private void PushList()
  {
    if (this.CurrPos < this.Capacity - 1 || this.List[this.CurrPos] == null)
      return;
    this.List[0].Close();
    for (var index = 1; index <= this.CurrPos; ++index)
      this.List[index - 1] = this.List[index];
  }
}