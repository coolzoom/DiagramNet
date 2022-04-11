// Decompiled with JetBrains decompiler
// Type: DiagramNet.EditLabelAction
// Assembly: DiagramNet, Version=0.5.0.31105, Culture=neutral, PublicKeyToken=null
// MVID: B9D60695-31B2-4147-A7EE-DFCE5218CFFE
// Assembly location: C:\dev\trevorde\WaveletStudio\trunk\res\libs\Diagram.net\DiagramNet.dll

namespace DiagramNet;

using DiagramNet.Elements;
using DiagramNet.Elements.Controllers;

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
    this._siteLabelElement = el;
    this._labelElement = ((ILabelElement) this._siteLabelElement).Label;
    this._labelTextBox = textBox;
    this._direction = !(this._siteLabelElement is BaseLinkElement) ? LabelEditDirection.UpDown : LabelEditDirection.Both;
    EditLabelAction.SetTextBoxLocation(this._siteLabelElement, this._labelTextBox);
    this._labelTextBox.AutoSize = true;
    this._labelTextBox.Show();
    this._labelTextBox.Text = this._labelElement.Text;
    this._labelTextBox.Font = this._labelElement.Font;
    this._labelTextBox.WordWrap = this._labelElement.Wrap;
    this._labelElement.Invalidate();
    switch (this._labelElement.Alignment)
    {
      case StringAlignment.Near:
        this._labelTextBox.TextAlign = HorizontalAlignment.Left;
        break;
      case StringAlignment.Center:
        this._labelTextBox.TextAlign = HorizontalAlignment.Center;
        break;
      case StringAlignment.Far:
        this._labelTextBox.TextAlign = HorizontalAlignment.Right;
        break;
    }
    this._labelTextBox.KeyPress += new KeyPressEventHandler(this.LabelTextBoxKeyPress);
    this._labelTextBox.Focus();
    this._center.X = textBox.Location.X + textBox.Size.Width / 2;
    this._center.Y = textBox.Location.Y + textBox.Size.Height / 2;
  }

  public void EndEdit()
  {
    if (this._siteLabelElement == null)
      return;
    this._labelTextBox.KeyPress -= new KeyPressEventHandler(this.LabelTextBoxKeyPress);
    var labelController = ControllerHelper.GetLabelController(this._siteLabelElement);
    this._labelElement.Size = this.MeasureTextSize();
    this._labelElement.Text = this._labelTextBox.Text;
    this._labelTextBox.Hide();
    if (labelController != null)
      labelController.SetLabelPosition();
    else
      this._labelElement.PositionBySite(this._siteLabelElement);
    this._labelElement.Invalidate();
    this._siteLabelElement = (BaseElement) null;
    this._labelElement = (LabelElement) null;
    this._labelTextBox = (TextBox) null;
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
    EditLabelAction.SetTextBoxBorder((Control) tb);
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
    var text = this._labelTextBox.Text;
    var size = Size.Empty;
    if (this._direction == LabelEditDirection.UpDown)
      size = DiagramUtil.MeasureString(text, this._labelElement.Font, this._labelTextBox.Size.Width, this._labelElement.Format);
    else if (this._direction == LabelEditDirection.Both)
      size = DiagramUtil.MeasureString(text, this._labelElement.Font);
    size.Height += 30;
    return size;
  }

  private void LabelTextBoxKeyPress(object sender, KeyPressEventArgs e)
  {
    if (this._labelTextBox.Text.Length == 0)
      return;
    var size1 = this._labelTextBox.Size;
    var size2 = this.MeasureTextSize();
    if (this._direction == LabelEditDirection.UpDown)
      size1.Height = size2.Height;
    else if (this._direction == LabelEditDirection.Both)
      size1 = size2;
    this._labelTextBox.Size = size1;
    this._labelTextBox.Location = new Point(this._center.X - size1.Width / 2, this._center.Y - size1.Height / 2);
  }
}