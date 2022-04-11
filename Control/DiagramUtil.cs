// Decompiled with JetBrains decompiler
// Type: DiagramNet.DiagramUtil
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using System.Collections;

internal class DiagramUtil
{
  private DiagramUtil()
  {
  }

  public static Point DisplayToCartesianCoord(Point p, Rectangle referenceRec)
  {
    var num1 = referenceRec.Width / 2;
    var num2 = referenceRec.Height / 2;
    return new Point(p.X - num1, p.Y - num2);
  }

  public static double PointToAngle(Point cartPoint)
  {
    var num = Math.Atan2((double) cartPoint.Y, (double) cartPoint.X) * (180.0 / Math.PI);
    if (num > 0.0 && num < 180.0)
      num = 360.0 - num;
    return Math.Abs(num);
  }

  public static CardinalDirection GetDirection(Rectangle rec, Point point)
  {
    var angle = DiagramUtil.PointToAngle(DiagramUtil.DisplayToCartesianCoord(point, rec));
    if (angle >= 0.0 && angle < 45.0 || angle >= 315.0)
      return CardinalDirection.East;
    if (angle >= 45.0 && angle < 135.0)
      return CardinalDirection.North;
    if (angle >= 135.0 && angle < 225.0)
      return CardinalDirection.West;
    return angle >= 225.0 && angle < 315.0 ? CardinalDirection.South : CardinalDirection.Nothing;
  }

  public static Point GetUpperPoint(Point[] points)
  {
    var empty = Point.Empty with
    {
      X = int.MaxValue,
      Y = int.MaxValue
    };
    foreach (var point in points)
    {
      if (point.X < empty.X)
        empty.X = point.X;
      if (point.Y < empty.Y)
        empty.Y = point.Y;
    }
    return empty;
  }

  public static Point GetLowerPoint(Point[] points)
  {
    var empty = Point.Empty with
    {
      X = int.MinValue,
      Y = int.MinValue
    };
    foreach (var point in points)
    {
      if (point.X > empty.X)
        empty.X = point.X;
      if (point.Y > empty.Y)
        empty.Y = point.Y;
    }
    return empty;
  }

  public static Point GetRelativePoint(Point location1, Point location2) => Point.Empty with
  {
    X = location2.X - location1.X,
    Y = location2.Y - location1.Y
  };

  public static Size MeasureString(string text, Font font)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(string text, Font font, SizeF layoutArea)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, layoutArea);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(string text, Font font, int width)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, width);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(
    string text,
    Font font,
    PointF origin,
    StringFormat stringFormat)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, origin, stringFormat);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(
    string text,
    Font font,
    SizeF layoutArea,
    StringFormat stringFormat)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, layoutArea, stringFormat);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(string text, Font font, int width, StringFormat format)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, width, format);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static Size MeasureString(
    string text,
    Font font,
    SizeF layoutArea,
    StringFormat stringFormat,
    out int charactersFitted,
    out int linesFilled)
  {
    var bitmap = new Bitmap(1, 1);
    var graphics = Graphics.FromImage((Image) bitmap);
    var sizeF = graphics.MeasureString(text, font, layoutArea, stringFormat, out charactersFitted, out linesFilled);
    bitmap.Dispose();
    graphics.Dispose();
    return Size.Round(sizeF);
  }

  public static int GetInnerElementsCount(BaseElement el)
  {
    var innerElementsCount = 0;
    if (el is ILabelElement)
      ++innerElementsCount;
    if (el is NodeElement)
    {
      var nodeElement = (NodeElement) el;
      innerElementsCount += nodeElement.Connectors.Length;
    }
    return innerElementsCount;
  }

  public static BaseElement[] GetInnerElements(BaseElement el)
  {
    var destinationArray = new BaseElement[DiagramUtil.GetInnerElementsCount(el)];
    var destinationIndex = 0;
    if (el is ILabelElement)
    {
      destinationArray[destinationIndex] = (BaseElement) ((ILabelElement) el).Label;
      ++destinationIndex;
    }
    if (el is NodeElement)
    {
      var connectors = ((NodeElement) el).Connectors;
      Array.Copy((Array) connectors, 0, (Array) destinationArray, destinationIndex, connectors.Length);
    }
    return destinationArray;
  }

  public class ArrayHelper
  {
    private ArrayHelper()
    {
    }

    public static Array Append(Array arr1, Array arr2)
    {
      var elementType1 = arr1.GetType().GetElementType();
      var elementType2 = arr1.GetType().GetElementType();
      if (elementType1 != elementType2)
        throw new Exception("Arrays isn't the same type");
      var arrayList = new ArrayList(arr1.Length + arr2.Length - 1);
      arrayList.AddRange((ICollection) arr1);
      arrayList.AddRange((ICollection) arr2);
      return arrayList.ToArray(elementType1);
    }

    public static Array Shrink(Array arr, object removeValue)
    {
      var arrayList = new ArrayList(arr.Length - 1);
      foreach (var obj in arr)
      {
        if (obj != removeValue)
          arrayList.Add(obj);
      }
      arrayList.TrimToSize();
      return arrayList.ToArray(arr.GetType().GetElementType());
    }
  }
}