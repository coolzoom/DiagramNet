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
    Enabled = true;
    List = new MemoryStream[capacity];
    Capacity = capacity;
  }

  public bool CanUndo => CurrPos != -1;

  public bool CanRedo => CurrPos != LastPos;

  public bool Enabled { get; set; }

  public void AddUndo(object o)
  {
    if (!Enabled)
      return;
    ++CurrPos;
    if (CurrPos >= Capacity)
      --CurrPos;
    ClearList(CurrPos);
    PushList();
    List[CurrPos] = SerializeObject(o);
    LastPos = CurrPos;
  }

  public object Undo()
  {
    if (!CanUndo)
      throw new ApplicationException("Can't Undo.");
    var obj = DeserializeObject(List[CurrPos]);
    --CurrPos;
    return obj;
  }

  public object Redo()
  {
    if (!CanRedo)
      throw new ApplicationException("Can't Undo.");
    ++CurrPos;
    return DeserializeObject(List[CurrPos]);
  }

  private MemoryStream SerializeObject(object o)
  {
    var formatter = (IFormatter) new BinaryFormatter();
    var serializationStream = new MemoryStream();
    formatter.Serialize(serializationStream, o);
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
    if (CurrPos >= Capacity - 1)
      return;
    for (var index = p; index < Capacity; ++index)
    {
      if (List[index] != null)
        List[index].Close();
      List[index] = null;
    }
  }

  private void PushList()
  {
    if (CurrPos < Capacity - 1 || List[CurrPos] == null)
      return;
    List[0].Close();
    for (var index = 1; index <= CurrPos; ++index)
      List[index - 1] = List[index];
  }
}