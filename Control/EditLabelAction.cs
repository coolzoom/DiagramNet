// Decompiled with JetBrains decompiler
// Type: DiagramNet.EditLabelAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using Elements;
using Elements.Controllers;

internal class EditLabelAction
{
  private const int TextBoxBorder = 3;
  private BaseElement _siteLabelElement;
  private LabelElement _labelElement;
  private TextBox _labelTextBox;
  private LabelEditDirection _direction;
  private Point _center;

  public void StartEdit(BaseElement el, TextBox textBox)
  {
    if (!(el is ILabelElement) || ((ILabelElement) el).Label.ReadOnly)
      return;
    _siteLabelElement = el;
    _labelElement = ((ILabelElement) _siteLabelElement).Label;
    _labelTextBox = textBox;
    _direction = !(_siteLabelElement is BaseLinkElement) ? LabelEditDirection.UpDown : LabelEditDirection.Both;
    SetTextBoxLocation(_siteLabelElement, _labelTextBox);
    _labelTextBox.AutoSize = true;
    _labelTextBox.Show();
    _labelTextBox.Text = _labelElement.Text;
    _labelTextBox.Font = _labelElement.Font;
    _labelTextBox.WordWrap = _labelElement.Wrap;
    _labelElement.Invalidate();
    switch (_labelElement.Alignment)
    {
      case StringAlignment.Near:
        _labelTextBox.TextAlign = HorizontalAlignment.Left;
        break;
      case StringAlignment.Center:
        _labelTextBox.TextAlign = HorizontalAlignment.Center;
        break;
      case StringAlignment.Far:
        _labelTextBox.TextAlign = HorizontalAlignment.Right;
        break;
    }
    _labelTextBox.KeyPress += new KeyPressEventHandler(LabelTextBoxKeyPress);
    _labelTextBox.Focus();
    _center.X = textBox.Location.X + textBox.Size.Width / 2;
    _center.Y = textBox.Location.Y + textBox.Size.Height / 2;
  }

  public void EndEdit()
  {
    if (_siteLabelElement == null)
      return;
    _labelTextBox.KeyPress -= new KeyPressEventHandler(LabelTextBoxKeyPress);
    var labelController = ControllerHelper.GetLabelController(_siteLabelElement);
    _labelElement.Size = MeasureTextSize();
    _labelElement.Text = _labelTextBox.Text;
    _labelTextBox.Hide();
    if (labelController != null)
      labelController.SetLabelPosition();
    else
      _labelElement.PositionBySite(_siteLabelElement);
    _labelElement.Invalidate();
    _siteLabelElement = (BaseElement) null;
    _labelElement = (LabelElement) null;
    _labelTextBox = (TextBox) null;
  }

  public static void SetTextBoxLocation(BaseElement el, TextBox tb)
  {
    if (!(el is ILabelElement))
      return;
    var label = ((ILabelElement) el).Label;
    el.Invalidate();
    label.Invalidate();
    if (label.Text.Length > 0)
    {
      tb.Location = label.Location;
      tb.Size = label.Size;
    }
    else
    {
      var size = DiagramUtil.MeasureString("XXXXXXX", label.Font, label.Size.Width, label.Format);
      if (el is BaseLinkElement)
      {
        tb.Size = size;
        tb.Location = new Point(el.Location.X + el.Size.Width / 2 - size.Width / 2, el.Location.Y + el.Size.Height / 2 - size.Height / 2);
      }
      else
      {
        size.Width = el.Size.Width;
        tb.Size = size;
        tb.Location = new Point(el.Location.X, el.Location.Y + el.Size.Height / 2 - size.Height / 2);
      }
    }
    SetTextBoxBorder((Control) tb);
  }

  private static void SetTextBoxBorder(Control tb)
  {
    var rectangle = new Rectangle(tb.Location, tb.Size);
    rectangle.Inflate(3, 3);
    tb.Location = rectangle.Location;
    tb.Size = rectangle.Size;
  }

  private Size MeasureTextSize()
  {
    var text = _labelTextBox.Text;
    var size = Size.Empty;
    if (_direction == LabelEditDirection.UpDown)
      size = DiagramUtil.MeasureString(text, _labelElement.Font, _labelTextBox.Size.Width, _labelElement.Format);
    else if (_direction == LabelEditDirection.Both)
      size = DiagramUtil.MeasureString(text, _labelElement.Font);
    size.Height += 30;
    return size;
  }

  private void LabelTextBoxKeyPress(object sender, KeyPressEventArgs e)
  {
    if (_labelTextBox.Text.Length == 0)
      return;
    var size1 = _labelTextBox.Size;
    var size2 = MeasureTextSize();
    if (_direction == LabelEditDirection.UpDown)
      size1.Height = size2.Height;
    else if (_direction == LabelEditDirection.Both)
      size1 = size2;
    _labelTextBox.Size = size1;
    _labelTextBox.Location = new Point(_center.X - size1.Width / 2, _center.Y - size1.Height / 2);
  }
}