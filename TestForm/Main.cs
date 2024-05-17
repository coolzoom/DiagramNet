namespace TestForm;

using Dalssoft.TestForm;
using DiagramNet;

/// <summary>
/// Summary description for Form1.
/// </summary>
public class Form1 : System.Windows.Forms.Form
{
  private bool changeDocumentProp = true;

  private System.Windows.Forms.ToolStrip toolBar1;
  private System.Windows.Forms.ToolStripButton btnSize;
  private System.Windows.Forms.ToolStripDropDownButton btnAdd;
  private System.Windows.Forms.ToolStripButton btnDelete;
  private System.Windows.Forms.ToolStripDropDownButton btnConnect;
  private System.Windows.Forms.ImageList imageList1;
  private System.Windows.Forms.ContextMenuStrip contextMenu1;
  private System.Windows.Forms.ToolStripSeparator sep1;
  private System.Windows.Forms.ToolStripButton btnSave;
  private System.Windows.Forms.ToolStripButton btnOpen;
  private System.Windows.Forms.ToolStripSeparator sep2;
  private System.Windows.Forms.ToolStripButton btnUndo;
  private System.Windows.Forms.ToolStripButton btnRedo;
  private System.Windows.Forms.ToolStripSeparator sep3;
  private System.Windows.Forms.ToolStripButton btnFront;
  private System.Windows.Forms.ToolStripButton btnBack;
  private System.Windows.Forms.ToolStripButton btnMoveUp;
  private System.Windows.Forms.ToolStripButton btnMoveDown;
  private System.Windows.Forms.MenuStrip mainMenu1;
  private System.Windows.Forms.ToolStripMenuItem menuItem11;
  private System.Windows.Forms.ToolStripMenuItem menuItem20;
  private System.Windows.Forms.ToolStripMenuItem menuItem26;
  private System.Windows.Forms.Panel panel1;
  private System.Windows.Forms.PropertyGrid propertyGrid1;
  private System.Windows.Forms.Splitter splitter1;
  private System.Windows.Forms.ToolStripMenuItem mnuFile;
  private System.Windows.Forms.ToolStripMenuItem mnuOpen;
  private System.Windows.Forms.ToolStripMenuItem mnuSave;
  private System.Windows.Forms.ToolStripMenuItem mnuExit;
  private System.Windows.Forms.ToolStripMenuItem mnuEdit;
  private System.Windows.Forms.ToolStripMenuItem mnuRedo;
  private System.Windows.Forms.ToolStripMenuItem mnuAdd;
  private System.Windows.Forms.ToolStripMenuItem mnuRectangle;
  private System.Windows.Forms.ToolStripMenuItem mnuEllipse;
  private System.Windows.Forms.ToolStripMenuItem mnuRectangleNode;
  private System.Windows.Forms.ToolStripMenuItem mnuEllipseNode;
  private System.Windows.Forms.ToolStripMenuItem mnuDelete;
  private System.Windows.Forms.ToolStripMenuItem mnuConnect;
  private System.Windows.Forms.ToolStripMenuItem mnuOrder;
  private System.Windows.Forms.ToolStripMenuItem mnuBringToFront;
  private System.Windows.Forms.ToolStripMenuItem mnuSendToBack;
  private System.Windows.Forms.ToolStripMenuItem mnuMoveUp;
  private System.Windows.Forms.ToolStripMenuItem mnuMoveDown;
  private System.Windows.Forms.ToolStripMenuItem mnuHelp;
  private System.Windows.Forms.ToolStripMenuItem mnuAbout;
  private System.Windows.Forms.ToolStripMenuItem mnuSize;
  private System.Windows.Forms.ToolStripMenuItem mnuTbRectangle;
  private System.Windows.Forms.ToolStripMenuItem mnuTbEllipse;
  private System.Windows.Forms.ToolStripMenuItem mnuTbRectangleNode;
  private System.Windows.Forms.ToolStripMenuItem mnuTbEllipseNode;
  private System.Windows.Forms.OpenFileDialog openFileDialog1;
  private System.Windows.Forms.SaveFileDialog saveFileDialog1;
  private System.Windows.Forms.ToolStripMenuItem mnuUndo;
  private System.Windows.Forms.ContextMenuStrip contextMenu2;
  private System.Windows.Forms.ToolStripMenuItem mnuTbStraightLink;
  private System.Windows.Forms.ToolStripMenuItem mnuTbRightAngleLink;
  private System.Windows.Forms.ToolStripMenuItem menuItem3;
  private System.Windows.Forms.ToolStripMenuItem mnuCut;
  private System.Windows.Forms.ToolStripMenuItem mnuPaste;
  private System.Windows.Forms.ToolStripMenuItem mnuCopy;
  private System.Windows.Forms.ToolStripSeparator sep4;
  private System.Windows.Forms.ToolStripButton btnCut;
  private System.Windows.Forms.ToolStripButton btnCopy;
  private System.Windows.Forms.ToolStripButton btnPaste;
  private System.Windows.Forms.ToolStripSeparator sep5;
  private System.Windows.Forms.ToolStripDropDownButton btnZoom;
  private System.Windows.Forms.ContextMenuStrip contextMenu_Zoom;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_10;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_25;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_50;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_75;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_100;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_150;
  private System.Windows.Forms.ToolStripMenuItem mnuZoom_200;
  private System.Windows.Forms.Splitter splitter2;
  private System.Windows.Forms.TextBox txtLog;
  private System.Windows.Forms.ToolStripMenuItem mnuShowDebugLog;
  private System.Windows.Forms.ToolStripMenuItem menuItem1;
  private DiagramNet.Designer designer1;
  private System.Windows.Forms.ToolStripMenuItem TbCommentBox;
  private ToolStripMenuItem menuSaveas;
  private ToolStripMenuItem TbCommentBoxNode;
  private ToolStripMenuItem TbDiagramBlockNode;
  private System.ComponentModel.IContainer components;

  public Form1()
  {

    //
    // Required for Windows Form Designer support
    //
    InitializeComponent();

    //
    // TODO: Add any constructor code after InitializeComponent call
    //
  }

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (components != null)
      {
        components.Dispose();
      }
    }
    base.Dispose(disposing);
  }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        toolBar1 = new ToolStrip();
        btnOpen = new ToolStripButton();
        btnSave = new ToolStripButton();
        sep1 = new ToolStripSeparator();
        btnCut = new ToolStripButton();
        btnCopy = new ToolStripButton();
        btnPaste = new ToolStripButton();
        btnDelete = new ToolStripButton();
        sep4 = new ToolStripSeparator();
        btnSize = new ToolStripButton();
        btnAdd = new ToolStripDropDownButton();
        contextMenu1 = new ContextMenuStrip(components);
        mnuTbRectangle = new ToolStripMenuItem();
        mnuTbEllipse = new ToolStripMenuItem();
        mnuTbRectangleNode = new ToolStripMenuItem();
        mnuTbEllipseNode = new ToolStripMenuItem();
        TbCommentBox = new ToolStripMenuItem();
        TbCommentBoxNode = new ToolStripMenuItem();
        TbDiagramBlockNode = new ToolStripMenuItem();
        btnConnect = new ToolStripDropDownButton();
        contextMenu2 = new ContextMenuStrip(components);
        mnuTbStraightLink = new ToolStripMenuItem();
        mnuTbRightAngleLink = new ToolStripMenuItem();
        sep2 = new ToolStripSeparator();
        btnUndo = new ToolStripButton();
        btnRedo = new ToolStripButton();
        sep3 = new ToolStripSeparator();
        btnZoom = new ToolStripDropDownButton();
        contextMenu_Zoom = new ContextMenuStrip(components);
        mnuZoom_10 = new ToolStripMenuItem();
        mnuZoom_25 = new ToolStripMenuItem();
        mnuZoom_50 = new ToolStripMenuItem();
        mnuZoom_75 = new ToolStripMenuItem();
        mnuZoom_100 = new ToolStripMenuItem();
        mnuZoom_150 = new ToolStripMenuItem();
        mnuZoom_200 = new ToolStripMenuItem();
        sep5 = new ToolStripSeparator();
        btnFront = new ToolStripButton();
        btnBack = new ToolStripButton();
        btnMoveUp = new ToolStripButton();
        btnMoveDown = new ToolStripButton();
        imageList1 = new ImageList(components);
        mainMenu1 = new MenuStrip();
        mnuFile = new ToolStripMenuItem();
        mnuOpen = new ToolStripMenuItem();
        mnuSave = new ToolStripMenuItem();
        menuSaveas = new ToolStripMenuItem();
        menuItem26 = new ToolStripMenuItem();
        mnuExit = new ToolStripMenuItem();
        mnuEdit = new ToolStripMenuItem();
        mnuUndo = new ToolStripMenuItem();
        mnuRedo = new ToolStripMenuItem();
        menuItem3 = new ToolStripMenuItem();
        mnuCut = new ToolStripMenuItem();
        mnuCopy = new ToolStripMenuItem();
        mnuPaste = new ToolStripMenuItem();
        mnuDelete = new ToolStripMenuItem();
        menuItem11 = new ToolStripMenuItem();
        mnuSize = new ToolStripMenuItem();
        mnuAdd = new ToolStripMenuItem();
        mnuRectangle = new ToolStripMenuItem();
        mnuEllipse = new ToolStripMenuItem();
        mnuRectangleNode = new ToolStripMenuItem();
        mnuEllipseNode = new ToolStripMenuItem();
        mnuConnect = new ToolStripMenuItem();
        menuItem20 = new ToolStripMenuItem();
        mnuOrder = new ToolStripMenuItem();
        mnuBringToFront = new ToolStripMenuItem();
        mnuSendToBack = new ToolStripMenuItem();
        mnuMoveUp = new ToolStripMenuItem();
        mnuMoveDown = new ToolStripMenuItem();
        mnuHelp = new ToolStripMenuItem();
        mnuShowDebugLog = new ToolStripMenuItem();
        menuItem1 = new ToolStripMenuItem();
        mnuAbout = new ToolStripMenuItem();
        panel1 = new Panel();
        designer1 = new DiagramNet.Designer(this.components);
        splitter2 = new Splitter();
        txtLog = new TextBox();
        splitter1 = new Splitter();
        propertyGrid1 = new PropertyGrid();
        openFileDialog1 = new OpenFileDialog();
        saveFileDialog1 = new SaveFileDialog();
        toolBar1.SuspendLayout();
        contextMenu1.SuspendLayout();
        contextMenu2.SuspendLayout();
        contextMenu_Zoom.SuspendLayout();
        mainMenu1.SuspendLayout();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // toolBar1
        // 
        toolBar1.AllowDrop = true;
        toolBar1.ImageList = imageList1;
        toolBar1.ImageScalingSize = new Size(32, 32);
        toolBar1.Items.AddRange(new ToolStripItem[] { btnOpen, btnSave, sep1, btnCut, btnCopy, btnPaste, btnDelete, sep4, btnSize, btnAdd, btnConnect, sep2, btnUndo, btnRedo, sep3, btnZoom, sep5, btnFront, btnBack, btnMoveUp, btnMoveDown });
        toolBar1.Location = new Point(0, 39);
        toolBar1.Name = "toolBar1";
        toolBar1.Size = new Size(1180, 42);
        toolBar1.TabIndex = 1;
        toolBar1.ItemClicked += toolBar1_ButtonClick;
        // 
        // btnOpen
        // 
        btnOpen.ImageIndex = 6;
        btnOpen.Name = "btnOpen";
        btnOpen.Size = new Size(46, 36);
        btnOpen.Tag = "Open";
        btnOpen.ToolTipText = "Open";
        // 
        // btnSave
        // 
        btnSave.ImageIndex = 5;
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(46, 36);
        btnSave.Tag = "Save";
        btnSave.ToolTipText = "Save";
        // 
        // sep1
        // 
        sep1.Name = "sep1";
        sep1.Size = new Size(6, 42);
        // 
        // btnCut
        // 
        btnCut.ImageIndex = 13;
        btnCut.Name = "btnCut";
        btnCut.Size = new Size(46, 36);
        btnCut.Tag = "Cut";
        btnCut.ToolTipText = "Cut";
        // 
        // btnCopy
        // 
        btnCopy.ImageIndex = 14;
        btnCopy.Name = "btnCopy";
        btnCopy.Size = new Size(46, 36);
        btnCopy.Tag = "Copy";
        btnCopy.ToolTipText = "Copy";
        // 
        // btnPaste
        // 
        btnPaste.ImageIndex = 15;
        btnPaste.Name = "btnPaste";
        btnPaste.Size = new Size(46, 36);
        btnPaste.Tag = "Paste";
        btnPaste.ToolTipText = "Paste";
        // 
        // btnDelete
        // 
        btnDelete.ImageIndex = 2;
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(46, 36);
        btnDelete.Tag = "Delete";
        btnDelete.ToolTipText = "Delete";
        // 
        // sep4
        // 
        sep4.Name = "sep4";
        sep4.Size = new Size(6, 42);
        // 
        // btnSize
        // 
        btnSize.ImageIndex = 0;
        btnSize.Name = "btnSize";
        btnSize.Size = new Size(46, 36);
        btnSize.Tag = "Size";
        btnSize.ToolTipText = "Size";
        // 
        // btnAdd
        // 
        btnAdd.DropDown = contextMenu1;
        btnAdd.ImageIndex = 1;
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(54, 36);
        btnAdd.Tag = "Add";
        btnAdd.ToolTipText = "Add";
        // 
        // contextMenu1
        // 
        contextMenu1.ImageScalingSize = new Size(32, 32);
        contextMenu1.Items.AddRange(new ToolStripItem[] { mnuTbRectangle, mnuTbEllipse, mnuTbRectangleNode, mnuTbEllipseNode, TbCommentBox, TbCommentBoxNode, TbDiagramBlockNode });
        contextMenu1.Name = "contextMenu1";
        contextMenu1.OwnerItem = btnAdd;
        contextMenu1.Size = new Size(324, 270);
        // 
        // mnuTbRectangle
        // 
        mnuTbRectangle.Name = "mnuTbRectangle";
        mnuTbRectangle.Size = new Size(323, 38);
        mnuTbRectangle.Text = "&Rectangle";
        mnuTbRectangle.Click += mnuTbRectangle_Click;
        // 
        // mnuTbEllipse
        // 
        mnuTbEllipse.Name = "mnuTbEllipse";
        mnuTbEllipse.Size = new Size(323, 38);
        mnuTbEllipse.Text = "&Ellipse";
        mnuTbEllipse.Click += mnuTbEllipse_Click;
        // 
        // mnuTbRectangleNode
        // 
        mnuTbRectangleNode.Name = "mnuTbRectangleNode";
        mnuTbRectangleNode.Size = new Size(323, 38);
        mnuTbRectangleNode.Text = "&Node Rectangle";
        mnuTbRectangleNode.Click += mnuTbRectangleNode_Click;
        // 
        // mnuTbEllipseNode
        // 
        mnuTbEllipseNode.Name = "mnuTbEllipseNode";
        mnuTbEllipseNode.Size = new Size(323, 38);
        mnuTbEllipseNode.Text = "N&ode Ellipse";
        mnuTbEllipseNode.Click += mnuTbEllipseNode_Click;
        // 
        // TbCommentBox
        // 
        TbCommentBox.Name = "TbCommentBox";
        TbCommentBox.Size = new Size(323, 38);
        TbCommentBox.Text = "Comment Box";
        TbCommentBox.Click += TbCommentBox_Click;
        // 
        // TbCommentBoxNode
        // 
        TbCommentBoxNode.Name = "TbCommentBoxNode";
        TbCommentBoxNode.Size = new Size(323, 38);
        TbCommentBoxNode.Text = "Comment Box Node";
        TbCommentBoxNode.Click += TbCommentBoxNode_Click;
        // 
        // TbDiagramBlockNode
        // 
        TbDiagramBlockNode.Name = "TbDiagramBlockNode";
        TbDiagramBlockNode.Size = new Size(323, 38);
        TbDiagramBlockNode.Text = "Diagram Block Node";
        TbDiagramBlockNode.Click += TbDiagramBlockNode_Click;
        // 
        // btnConnect
        // 
        btnConnect.DropDown = contextMenu2;
        btnConnect.ImageIndex = 3;
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(54, 36);
        btnConnect.Tag = "Connect";
        btnConnect.ToolTipText = "Connect";
        // 
        // contextMenu2
        // 
        contextMenu2.ImageScalingSize = new Size(32, 32);
        contextMenu2.Items.AddRange(new ToolStripItem[] { mnuTbStraightLink, mnuTbRightAngleLink });
        contextMenu2.Name = "contextMenu2";
        contextMenu2.OwnerItem = btnConnect;
        contextMenu2.Size = new Size(277, 80);
        // 
        // mnuTbStraightLink
        // 
        mnuTbStraightLink.Name = "mnuTbStraightLink";
        mnuTbStraightLink.Size = new Size(276, 38);
        mnuTbStraightLink.Text = "Straight Link";
        mnuTbStraightLink.Click += mnuTbStraightLink_Click;
        // 
        // mnuTbRightAngleLink
        // 
        mnuTbRightAngleLink.Name = "mnuTbRightAngleLink";
        mnuTbRightAngleLink.Size = new Size(276, 38);
        mnuTbRightAngleLink.Text = "Right Angle Link";
        mnuTbRightAngleLink.Click += mnuTbRightAngleLink_Click;
        // 
        // sep2
        // 
        sep2.Name = "sep2";
        sep2.Size = new Size(6, 42);
        // 
        // btnUndo
        // 
        btnUndo.ImageIndex = 7;
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(46, 36);
        btnUndo.Tag = "Undo";
        btnUndo.ToolTipText = "Undo";
        // 
        // btnRedo
        // 
        btnRedo.ImageIndex = 8;
        btnRedo.Name = "btnRedo";
        btnRedo.Size = new Size(46, 36);
        btnRedo.Tag = "Redo";
        btnRedo.ToolTipText = "Redo";
        // 
        // sep3
        // 
        sep3.Name = "sep3";
        sep3.Size = new Size(6, 42);
        // 
        // btnZoom
        // 
        btnZoom.DropDown = contextMenu_Zoom;
        btnZoom.ImageIndex = 16;
        btnZoom.Name = "btnZoom";
        btnZoom.Size = new Size(54, 36);
        btnZoom.Tag = "Zoom";
        // 
        // contextMenu_Zoom
        // 
        contextMenu_Zoom.ImageScalingSize = new Size(32, 32);
        contextMenu_Zoom.Items.AddRange(new ToolStripItem[] { mnuZoom_10, mnuZoom_25, mnuZoom_50, mnuZoom_75, mnuZoom_100, mnuZoom_150, mnuZoom_200 });
        contextMenu_Zoom.Name = "contextMenu_Zoom";
        contextMenu_Zoom.OwnerItem = btnZoom;
        contextMenu_Zoom.Size = new Size(152, 270);
        // 
        // mnuZoom_10
        // 
        mnuZoom_10.Name = "mnuZoom_10";
        mnuZoom_10.Size = new Size(151, 38);
        mnuZoom_10.Text = "10%";
        mnuZoom_10.Click += mnuZoom_10_Click;
        // 
        // mnuZoom_25
        // 
        mnuZoom_25.Name = "mnuZoom_25";
        mnuZoom_25.Size = new Size(151, 38);
        mnuZoom_25.Text = "25%";
        mnuZoom_25.Click += mnuZoom_25_Click;
        // 
        // mnuZoom_50
        // 
        mnuZoom_50.Name = "mnuZoom_50";
        mnuZoom_50.Size = new Size(151, 38);
        mnuZoom_50.Text = "50%";
        mnuZoom_50.Click += mnuZoom_50_Click;
        // 
        // mnuZoom_75
        // 
        mnuZoom_75.Name = "mnuZoom_75";
        mnuZoom_75.Size = new Size(151, 38);
        mnuZoom_75.Text = "75%";
        mnuZoom_75.Click += mnuZoom_75_Click;
        // 
        // mnuZoom_100
        // 
        mnuZoom_100.Name = "mnuZoom_100";
        mnuZoom_100.Size = new Size(151, 38);
        mnuZoom_100.Text = "100%";
        mnuZoom_100.Click += mnuZoom_100_Click;
        // 
        // mnuZoom_150
        // 
        mnuZoom_150.Name = "mnuZoom_150";
        mnuZoom_150.Size = new Size(151, 38);
        mnuZoom_150.Text = "150%";
        mnuZoom_150.Click += mnuZoom_150_Click;
        // 
        // mnuZoom_200
        // 
        mnuZoom_200.Name = "mnuZoom_200";
        mnuZoom_200.Size = new Size(151, 38);
        mnuZoom_200.Text = "200%";
        mnuZoom_200.Click += mnuZoom_200_Click;
        // 
        // sep5
        // 
        sep5.Name = "sep5";
        sep5.Size = new Size(6, 42);
        // 
        // btnFront
        // 
        btnFront.ImageIndex = 9;
        btnFront.Name = "btnFront";
        btnFront.Size = new Size(46, 36);
        btnFront.Tag = "Front";
        btnFront.ToolTipText = "Bring to Front";
        // 
        // btnBack
        // 
        btnBack.ImageIndex = 10;
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(46, 36);
        btnBack.Tag = "Back";
        btnBack.ToolTipText = "Send to Back";
        // 
        // btnMoveUp
        // 
        btnMoveUp.ImageIndex = 11;
        btnMoveUp.Name = "btnMoveUp";
        btnMoveUp.Size = new Size(46, 36);
        btnMoveUp.Tag = "MoveUp";
        btnMoveUp.ToolTipText = "Move Up";
        // 
        // btnMoveDown
        // 
        btnMoveDown.ImageIndex = 12;
        btnMoveDown.Name = "btnMoveDown";
        btnMoveDown.Size = new Size(46, 36);
        btnMoveDown.Tag = "MoveDown";
        btnMoveDown.ToolTipText = "Move Down";
        // 
        // imageList1
        // 
        imageList1.ColorDepth = ColorDepth.Depth8Bit;
        imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
        imageList1.TransparentColor = Color.Silver;
        imageList1.Images.SetKeyName(0, "");
        imageList1.Images.SetKeyName(1, "");
        imageList1.Images.SetKeyName(2, "");
        imageList1.Images.SetKeyName(3, "");
        imageList1.Images.SetKeyName(4, "");
        imageList1.Images.SetKeyName(5, "");
        imageList1.Images.SetKeyName(6, "");
        imageList1.Images.SetKeyName(7, "");
        imageList1.Images.SetKeyName(8, "");
        imageList1.Images.SetKeyName(9, "");
        imageList1.Images.SetKeyName(10, "");
        imageList1.Images.SetKeyName(11, "");
        imageList1.Images.SetKeyName(12, "");
        imageList1.Images.SetKeyName(13, "");
        imageList1.Images.SetKeyName(14, "");
        imageList1.Images.SetKeyName(15, "");
        imageList1.Images.SetKeyName(16, "");
        // 
        // mainMenu1
        // 
        mainMenu1.ImageScalingSize = new Size(32, 32);
        mainMenu1.Items.AddRange(new ToolStripItem[] { mnuFile, mnuEdit, mnuHelp });
        mainMenu1.Location = new Point(0, 0);
        mainMenu1.Name = "mainMenu1";
        mainMenu1.Size = new Size(1180, 39);
        mainMenu1.TabIndex = 3;
        // 
        // mnuFile
        // 
        mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuOpen, mnuSave, menuSaveas, menuItem26, mnuExit });
        mnuFile.Name = "mnuFile";
        mnuFile.Size = new Size(73, 35);
        mnuFile.Text = "&File";
        // 
        // mnuOpen
        // 
        mnuOpen.Name = "mnuOpen";
        mnuOpen.Size = new Size(239, 44);
        mnuOpen.Text = "&Open";
        mnuOpen.Click += mnuOpen_Click;
        // 
        // mnuSave
        // 
        mnuSave.Name = "mnuSave";
        mnuSave.Size = new Size(239, 44);
        mnuSave.Text = "&Save";
        mnuSave.Click += mnuSave_Click;
        // 
        // menuSaveas
        // 
        menuSaveas.Name = "menuSaveas";
        menuSaveas.Size = new Size(239, 44);
        menuSaveas.Text = "Save &as ";
        menuSaveas.Click += menuSaveas_Click;
        // 
        // menuItem26
        // 
        menuItem26.Name = "menuItem26";
        menuItem26.Size = new Size(239, 44);
        menuItem26.Text = "-";
        // 
        // mnuExit
        // 
        mnuExit.Name = "mnuExit";
        mnuExit.Size = new Size(239, 44);
        mnuExit.Text = "&Exit";
        mnuExit.Click += mnuExit_Click;
        // 
        // mnuEdit
        // 
        mnuEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuUndo, mnuRedo, menuItem3, mnuCut, mnuCopy, mnuPaste, mnuDelete, menuItem11, mnuSize, mnuAdd, mnuConnect, menuItem20, mnuOrder });
        mnuEdit.Name = "mnuEdit";
        mnuEdit.Size = new Size(77, 35);
        mnuEdit.Text = "&Edit";
        // 
        // mnuUndo
        // 
        mnuUndo.Name = "mnuUndo";
        mnuUndo.Size = new Size(243, 44);
        mnuUndo.Text = "&Undo";
        mnuUndo.Click += mnuUndo_Click;
        // 
        // mnuRedo
        // 
        mnuRedo.Name = "mnuRedo";
        mnuRedo.Size = new Size(243, 44);
        mnuRedo.Text = "&Redo";
        mnuRedo.Click += mnuRedo_Click;
        // 
        // menuItem3
        // 
        menuItem3.Name = "menuItem3";
        menuItem3.Size = new Size(243, 44);
        menuItem3.Text = "-";
        // 
        // mnuCut
        // 
        mnuCut.Name = "mnuCut";
        mnuCut.Size = new Size(243, 44);
        mnuCut.Text = "Cu&t";
        mnuCut.Click += mnuCut_Click;
        // 
        // mnuCopy
        // 
        mnuCopy.Name = "mnuCopy";
        mnuCopy.Size = new Size(243, 44);
        mnuCopy.Text = "Cop&y";
        mnuCopy.Click += mnuCopy_Click;
        // 
        // mnuPaste
        // 
        mnuPaste.Name = "mnuPaste";
        mnuPaste.Size = new Size(243, 44);
        mnuPaste.Text = "Paste";
        mnuPaste.Click += mnuPaste_Click;
        // 
        // mnuDelete
        // 
        mnuDelete.Name = "mnuDelete";
        mnuDelete.Size = new Size(243, 44);
        mnuDelete.Text = "&Delete";
        mnuDelete.Click += mnuDelete_Click;
        // 
        // menuItem11
        // 
        menuItem11.Name = "menuItem11";
        menuItem11.Size = new Size(243, 44);
        menuItem11.Text = "-";
        // 
        // mnuSize
        // 
        mnuSize.Name = "mnuSize";
        mnuSize.Size = new Size(243, 44);
        mnuSize.Text = "&Size";
        mnuSize.Click += mnuSize_Click;
        // 
        // mnuAdd
        // 
        mnuAdd.DropDownItems.AddRange(new ToolStripItem[] { mnuRectangle, mnuEllipse, mnuRectangleNode, mnuEllipseNode });
        mnuAdd.Name = "mnuAdd";
        mnuAdd.Size = new Size(243, 44);
        mnuAdd.Text = "&Add";
        // 
        // mnuRectangle
        // 
        mnuRectangle.Name = "mnuRectangle";
        mnuRectangle.Size = new Size(332, 44);
        mnuRectangle.Text = "&Rectangle";
        mnuRectangle.Click += mnuRectangle_Click;
        // 
        // mnuEllipse
        // 
        mnuEllipse.Name = "mnuEllipse";
        mnuEllipse.Size = new Size(332, 44);
        mnuEllipse.Text = "&Ellipse";
        mnuEllipse.Click += mnuEllipse_Click;
        // 
        // mnuRectangleNode
        // 
        mnuRectangleNode.Name = "mnuRectangleNode";
        mnuRectangleNode.Size = new Size(332, 44);
        mnuRectangleNode.Text = "&Node Rectangle";
        mnuRectangleNode.Click += mnuRectangleNode_Click;
        // 
        // mnuEllipseNode
        // 
        mnuEllipseNode.Name = "mnuEllipseNode";
        mnuEllipseNode.Size = new Size(332, 44);
        mnuEllipseNode.Text = "N&ode Ellipse";
        mnuEllipseNode.Click += mnuEllipseNode_Click;
        // 
        // mnuConnect
        // 
        mnuConnect.Name = "mnuConnect";
        mnuConnect.Size = new Size(243, 44);
        mnuConnect.Text = "&Connect";
        mnuConnect.Click += mnuConnect_Click;
        // 
        // menuItem20
        // 
        menuItem20.Name = "menuItem20";
        menuItem20.Size = new Size(243, 44);
        menuItem20.Text = "-";
        // 
        // mnuOrder
        // 
        mnuOrder.DropDownItems.AddRange(new ToolStripItem[] { mnuBringToFront, mnuSendToBack, mnuMoveUp, mnuMoveDown });
        mnuOrder.Name = "mnuOrder";
        mnuOrder.Size = new Size(243, 44);
        mnuOrder.Text = "&Order";
        // 
        // mnuBringToFront
        // 
        mnuBringToFront.Name = "mnuBringToFront";
        mnuBringToFront.Size = new Size(306, 44);
        mnuBringToFront.Text = "&Bring to Front";
        mnuBringToFront.Click += mnuBringToFront_Click;
        // 
        // mnuSendToBack
        // 
        mnuSendToBack.Name = "mnuSendToBack";
        mnuSendToBack.Size = new Size(306, 44);
        mnuSendToBack.Text = "Send to &Back";
        mnuSendToBack.Click += mnuSendToBack_Click;
        // 
        // mnuMoveUp
        // 
        mnuMoveUp.Name = "mnuMoveUp";
        mnuMoveUp.Size = new Size(306, 44);
        mnuMoveUp.Text = "Move &Up";
        mnuMoveUp.Click += mnuMoveUp_Click;
        // 
        // mnuMoveDown
        // 
        mnuMoveDown.Name = "mnuMoveDown";
        mnuMoveDown.Size = new Size(306, 44);
        mnuMoveDown.Text = "Move &Down";
        mnuMoveDown.Click += mnuMoveDown_Click;
        // 
        // mnuHelp
        // 
        mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuShowDebugLog, menuItem1, mnuAbout });
        mnuHelp.Name = "mnuHelp";
        mnuHelp.Size = new Size(88, 35);
        mnuHelp.Text = "&Help";
        // 
        // mnuShowDebugLog
        // 
        mnuShowDebugLog.Name = "mnuShowDebugLog";
        mnuShowDebugLog.Size = new Size(361, 44);
        mnuShowDebugLog.Text = "&Show Debug Log...";
        mnuShowDebugLog.Click += mnuShowDebugLog_Click;
        // 
        // menuItem1
        // 
        menuItem1.Name = "menuItem1";
        menuItem1.Size = new Size(361, 44);
        menuItem1.Text = "-";
        // 
        // mnuAbout
        // 
        mnuAbout.Name = "mnuAbout";
        mnuAbout.Size = new Size(361, 44);
        mnuAbout.Text = "&About...";
        mnuAbout.Click += mnuAbout_Click;
        // 
        // panel1
        // 
        panel1.Controls.Add(this.designer1);
        panel1.Controls.Add(splitter2);
        panel1.Controls.Add(txtLog);
        panel1.Controls.Add(splitter1);
        panel1.Controls.Add(propertyGrid1);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(0, 81);
        panel1.Name = "panel1";
        panel1.Size = new Size(1180, 748);
        panel1.TabIndex = 2;
        // 
	    // designer1
	    // 
	    this.designer1.AutoScroll = true;
	    this.designer1.AutoScrollMinSize = new System.Drawing.Size(100, 100);
	    this.designer1.BackColor = System.Drawing.SystemColors.Window;
	    this.designer1.Dock = System.Windows.Forms.DockStyle.Fill;
	    this.designer1.Location = new System.Drawing.Point(0, 0);
	    this.designer1.Name = "designer1";
	    this.designer1.Size = new System.Drawing.Size(423, 243);
	    this.designer1.TabIndex = 6;
	    this.designer1.ElementClick += new DiagramNet.Designer.ElementEventHandler(this.designer1_ElementClick);
	    this.designer1.ElementMouseDown += new DiagramNet.Designer.ElementMouseEventHandler(this.designer1_ElementMouseDown);
	    this.designer1.ElementMouseUp += new DiagramNet.Designer.ElementMouseEventHandler(this.designer1_ElementMouseUp);
	    this.designer1.ElementMoving += new DiagramNet.Designer.ElementEventHandler(this.designer1_ElementMoving);
	    this.designer1.ElementMoved += new DiagramNet.Designer.ElementEventHandler(this.designer1_ElementMoved);
	    this.designer1.ElementResizing += new DiagramNet.Designer.ElementEventHandler(this.designer1_ElementResizing);
	    this.designer1.ElementResized += new DiagramNet.Designer.ElementEventHandler(this.designer1_ElementResized);
	    this.designer1.ElementConnecting += new DiagramNet.Designer.ElementConnectEventHandler(this.designer1_ElementConnecting);
	    this.designer1.ElementConnected += new DiagramNet.Designer.ElementConnectEventHandler(this.designer1_ElementConnected);
	    this.designer1.ElementSelection += new DiagramNet.Designer.ElementSelectionEventHandler(this.designer1_ElementSelection);
	    this.designer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.designer1_MouseUp);
        //
        // splitter2
        // 
        splitter2.Dock = DockStyle.Bottom;
        splitter2.Location = new Point(0, 491);
        splitter2.Name = "splitter2";
        splitter2.Size = new Size(589, 9);
        splitter2.TabIndex = 5;
        splitter2.TabStop = false;
        // 
        // txtLog
        // 
        txtLog.BorderStyle = BorderStyle.None;
        txtLog.Dock = DockStyle.Bottom;
        txtLog.Location = new Point(0, 500);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Both;
        txtLog.Size = new Size(589, 248);
        txtLog.TabIndex = 4;
        txtLog.Text = "Log:";
        txtLog.Visible = false;
        // 
        // splitter1
        // 
        splitter1.Dock = DockStyle.Right;
        splitter1.Location = new Point(589, 0);
        splitter1.Name = "splitter1";
        splitter1.Size = new Size(8, 748);
        splitter1.TabIndex = 1;
        splitter1.TabStop = false;
        // 
        // propertyGrid1
        // 
        propertyGrid1.Dock = DockStyle.Right;
        propertyGrid1.LineColor = SystemColors.ScrollBar;
        propertyGrid1.Location = new Point(597, 0);
        propertyGrid1.Name = "propertyGrid1";
        propertyGrid1.Size = new Size(583, 748);
        propertyGrid1.TabIndex = 0;
        // 
        // openFileDialog1
        // 
        openFileDialog1.DefaultExt = "*.dgn";
        openFileDialog1.RestoreDirectory = true;
        // 
        // Form1
        // 
        AutoScaleBaseSize = new Size(13, 31);
        ClientSize = new Size(1180, 829);
        Controls.Add(panel1);
        Controls.Add(toolBar1);
        Controls.Add(mainMenu1);
        MainMenuStrip = mainMenu1;
        Name = "Form1";
        Text = "Diagram.NET Test Form";
        WindowState = FormWindowState.Maximized;
        Load += Form1_Load;
        toolBar1.ResumeLayout(false);
        toolBar1.PerformLayout();
        contextMenu1.ResumeLayout(false);
        contextMenu2.ResumeLayout(false);
        contextMenu_Zoom.ResumeLayout(false);
        mainMenu1.ResumeLayout(false);
        mainMenu1.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
  static void Main()
  {
    Application.EnableVisualStyles();
    Application.DoEvents();

    Application.Run(new Form1());
  }

  #region Functions
  private void Edit_UpdateUndoRedoEnable()
  {
    mnuUndo.Enabled = designer1.CanUndo;
    btnUndo.Enabled = designer1.CanUndo;
    mnuRedo.Enabled = designer1.CanRedo;
    btnRedo.Enabled = designer1.CanRedo;
  }

  private void Edit_Undo()
  {
    if (designer1.CanUndo)
      designer1.Undo();

    Edit_UpdateUndoRedoEnable();
  }

  private void Edit_Redo()
  {
    if (designer1.CanRedo)
      designer1.Redo();

    Edit_UpdateUndoRedoEnable();
  }

  private void Action_None()
  {
    mnuSize.Checked = false;
    mnuAdd.Checked = false;
    mnuDelete.Checked = false;
    mnuConnect.Checked = false;

    btnSize.CheckOnClick = false;
    // TODO   btnAdd.Pushed = false;
    btnDelete.CheckOnClick = false;
    // TODO   btnConnect.Pushed = false;

    mnuRectangle.Checked = false;
    mnuTbRectangle.Checked = false;
    mnuEllipse.Checked = false;
    mnuTbEllipse.Checked = false;
    mnuRectangleNode.Checked = false;
    mnuTbRectangleNode.Checked = false;
    mnuEllipseNode.Checked = false;
    mnuTbEllipseNode.Checked = false;
  }

  private void Action_Size()
  {
    Action_None();
    mnuSize.Checked = true;
    btnSize.CheckOnClick = true;
    if (changeDocumentProp)
      designer1.Document.Action = DesignerAction.Select;
  }

  private void Action_Add(ElementType e)
  {
    Action_None();
    // TODO   btnAdd.Pushed = true;
    switch (e)
    {
      case ElementType.Rectangle:
        mnuRectangle.Checked = true;
        mnuTbRectangle.Checked = true;
        break;
      case ElementType.Ellipse:
        mnuEllipse.Checked = true;
        mnuTbEllipse.Checked = true;
        break;
      case ElementType.RectangleNode:
        mnuRectangleNode.Checked = true;
        mnuTbRectangleNode.Checked = true;
        break;
      case ElementType.EllipseNode:
        mnuEllipseNode.Checked = true;
        mnuTbEllipseNode.Checked = true;
        break;
      case ElementType.DiagramBlock:
        break;
    }

    if (changeDocumentProp)
    {
      designer1.Document.Action = DesignerAction.Add;
      designer1.Document.ElementType = e;
    }
  }

  private void Action_Delete()
  {
    Action_None();
    mnuDelete.Checked = true;
    btnDelete.CheckOnClick = true;
    if (changeDocumentProp)
      designer1.Document.DeleteSelectedElements();
    Action_None();
  }

  private void Action_Connect()
  {
    Action_None();
    mnuConnect.Checked = true;
    // TODO   btnConnect.Pushed = true;
    if (changeDocumentProp)
      designer1.Document.Action = DesignerAction.Connect;
  }

  private void Action_Connector(LinkType lt)
  {
    Action_None();
    switch (lt)
    {
      case LinkType.Straight:
        mnuTbStraightLink.Checked = true;
        mnuTbRightAngleLink.Checked = false;
        break;
      case LinkType.RightAngle:
        mnuTbStraightLink.Checked = false;
        mnuTbRightAngleLink.Checked = true;
        break;
    }
    designer1.Document.LinkType = lt;
    Action_Connect();
  }

  private void Action_Zoom(float zoom)
  {
    designer1.Document.Zoom = zoom;
  }
  public string FileName { get; set; }
  private void File_Open()
  {
    openFileDialog1.FileName = FileName;
    openFileDialog1.Filter = "Diagram.Net files (*.dgn)|*.dgn|All files (*.*)|*.*";
    openFileDialog1.DefaultExt = ".dgn";
    if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
    {
      FileName = openFileDialog1.FileName;

      designer1.OpenBinary(openFileDialog1.FileName);
    }
  }

  private void File_Save()
  {
    if (File.Exists(FileName))
    {
      designer1.SaveBinary(FileName);
      Bitmap bmp = new Bitmap(designer1.Width, designer1.Height);
      designer1.DrawToBitmap(bmp, new Rectangle(0, 0, designer1.Width, designer1.Height));
      bmp.Save(FileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
    }
    else
    {
      File_SaveAs();
    }
  }

  private void File_SaveAs()
  {
    saveFileDialog1.FileName = FileName;
    saveFileDialog1.Filter = "Diagram.Net files (*.dgn)|*.dgn|All files (*.*)|*.*";
    saveFileDialog1.DefaultExt = ".dgn";
    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
    {
      FileName = saveFileDialog1.FileName;
      designer1.SaveBinary(saveFileDialog1.FileName);
      Bitmap bmp = new Bitmap(designer1.Width, designer1.Height);
      designer1.DrawToBitmap(bmp, new Rectangle(0, 0, designer1.Width, designer1.Height));
      bmp.Save(saveFileDialog1.FileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
    }
  }

  private void Order_BringToFront()
  {
    if (designer1.Document.SelectedElements.Count == 1)
    {
      designer1.Document.BringToFrontElement(designer1.Document.SelectedElements[0]);
      designer1.Refresh();
    }
  }

  private void Order_SendToBack()
  {
    if (designer1.Document.SelectedElements.Count == 1)
    {
      designer1.Document.SendToBackElement(designer1.Document.SelectedElements[0]);
      designer1.Refresh();
    }
  }

  private void Order_MoveUp()
  {
    if (designer1.Document.SelectedElements.Count == 1)
    {
      designer1.Document.MoveUpElement(designer1.Document.SelectedElements[0]);
      designer1.Refresh();
    }
  }

  private void Order_MoveDown()
  {
    if (designer1.Document.SelectedElements.Count == 1)
    {
      designer1.Document.MoveDownElement(designer1.Document.SelectedElements[0]);
      designer1.Refresh();
    }
  }

  private void Clipboard_Cut()
  {
    designer1.Cut();
  }

  private void Clipboard_Copy()
  {
    designer1.Copy();
  }

  private void Clipboard_Paste()
  {
    designer1.Paste();
  }

  #endregion

  #region Menu Events
  private void mnuUndo_Click(object sender, System.EventArgs e)
  {
    Edit_Undo();
  }

  private void mnuRedo_Click(object sender, System.EventArgs e)
  {
    Edit_Redo();
  }

  private void mnuSize_Click(object sender, System.EventArgs e)
  {
    Action_Size();
  }

  private void mnuRectangle_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.Rectangle);
  }

  private void mnuEllipse_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.Ellipse);
  }

  private void mnuRectangleNode_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.RectangleNode);
  }

  private void mnuEllipseNode_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.EllipseNode);
  }

  private void mnuDelete_Click(object sender, System.EventArgs e)
  {
    Action_Delete();
  }

  private void mnuCut_Click(object sender, System.EventArgs e)
  {
    Clipboard_Cut();
  }

  private void mnuCopy_Click(object sender, System.EventArgs e)
  {
    Clipboard_Copy();
  }

  private void mnuPaste_Click(object sender, System.EventArgs e)
  {
    Clipboard_Paste();
  }

  private void mnuConnect_Click(object sender, System.EventArgs e)
  {
    Action_Connect();
  }

  private void mnuBringToFront_Click(object sender, System.EventArgs e)
  {
    Order_BringToFront();
  }

  private void mnuSendToBack_Click(object sender, System.EventArgs e)
  {
    Order_SendToBack();
  }

  private void mnuMoveUp_Click(object sender, System.EventArgs e)
  {
    Order_MoveUp();
  }

  private void mnuMoveDown_Click(object sender, System.EventArgs e)
  {
    Order_MoveDown();
  }

  private void mnuOpen_Click(object sender, System.EventArgs e)
  {
    File_Open();
  }

  private void mnuSave_Click(object sender, System.EventArgs e)
  {
    File_Save();
  }

  private void mnuExit_Click(object sender, System.EventArgs e)
  {
    this.Close();
  }

  private void mnuAbout_Click(object sender, System.EventArgs e)
  {
    About about = new About();
    about.ShowDialog(this);
  }
  #endregion

  #region Toolbar Events
  private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
  {

    string btn = (string)e.ClickedItem.Tag;

    if (btn == "Open") File_Open();
    if (btn == "Save") File_Save();

    if (btn == "Size") Action_Size();
    //if (btn == "Add")
    if (btn == "Delete") Action_Delete();
    if (btn == "Connect") Action_Connect();

    if (btn == "Undo") Edit_Undo();
    if (btn == "Redo") Edit_Redo();

    if (btn == "Front") Order_BringToFront();
    if (btn == "Back") Order_SendToBack();
    if (btn == "MoveUp") Order_MoveUp();
    if (btn == "MoveDown") Order_MoveDown();

    if (btn == "Cut") Clipboard_Cut();
    if (btn == "Copy") Clipboard_Copy();
    if (btn == "Paste") Clipboard_Paste();
  }

  private void mnuTbRectangle_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.Rectangle);
  }

  private void mnuTbEllipse_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.Ellipse);
  }

  private void mnuTbRectangleNode_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.RectangleNode);
  }

  private void mnuTbEllipseNode_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.EllipseNode);
  }

  private void TbCommentBox_Click(object sender, System.EventArgs e)
  {
    Action_Add(ElementType.CommentBox);
  }

  private void mnuTbStraightLink_Click(object sender, System.EventArgs e)
  {
    Action_Connector(LinkType.Straight);
  }

  private void mnuTbRightAngleLink_Click(object sender, System.EventArgs e)
  {
    Action_Connector(LinkType.RightAngle);
  }

  #endregion

  private void Form1_Load(object sender, System.EventArgs e)
  {
    Edit_UpdateUndoRedoEnable();

    //Events
    designer1.Document.PropertyChanged += new EventHandler(Document_PropertyChanged);
  }

  private void Document_PropertyChanged(object sender, EventArgs e)
  {
    changeDocumentProp = false;

    Action_None();

    switch (designer1.Document.Action)
    {
      case DesignerAction.Select:
        Action_Size();
        break;
      case DesignerAction.Delete:
        Action_Delete();
        break;
      case DesignerAction.Connect:
        Action_Connect();
        break;
      case DesignerAction.Add:
        Action_Add(designer1.Document.ElementType);
        break;
    }

    Edit_UpdateUndoRedoEnable();

    changeDocumentProp = true;
  }

  private void mnuZoom_10_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(0.1f);
  }

  private void mnuZoom_25_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(0.25f);
  }

  private void mnuZoom_50_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(0.5f);
  }

  private void mnuZoom_75_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(0.75f);
  }

  private void mnuZoom_100_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(1f);
  }

  private void mnuZoom_150_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(1.5f);
  }

  private void mnuZoom_200_Click(object sender, System.EventArgs e)
  {
    Action_Zoom(2.0f);
  }

  private void mnuShowDebugLog_Click(object sender, System.EventArgs e)
  {
    mnuShowDebugLog.Checked = !mnuShowDebugLog.Checked;
    txtLog.Visible = mnuShowDebugLog.Checked;

  }

  #region Events Handling 
  private void designer1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
  {
    AppendLog("designer1_MouseUp: {0}", e.ToString());

    propertyGrid1.SelectedObject = null;

    if (designer1.Document.SelectedElements.Count == 1)
    {
      propertyGrid1.SelectedObject = designer1.Document.SelectedElements[0];
    }
    else if (designer1.Document.SelectedElements.Count > 1)
    {
      propertyGrid1.SelectedObjects = designer1.Document.SelectedElements.GetArray();
    }
    else if (designer1.Document.SelectedElements.Count == 0)
    {
      propertyGrid1.SelectedObject = designer1.Document;
    }
  }

  private void designer1_ElementClick(object sender, DiagramNet.Events.ElementEventArgs e)
  {
    AppendLog("designer1_ElementClick: {0}", e.ToString());
  }

  private void designer1_ElementMouseDown(object sender, DiagramNet.Events.ElementMouseEventArgs e)
  {
    AppendLog("designer1_ElementMouseDown: {0}", e.ToString());
  }

  private void designer1_ElementMouseUp(object sender, DiagramNet.Events.ElementMouseEventArgs e)
  {
    AppendLog("designer1_ElementMouseUp: {0}", e.ToString());
  }

  private void designer1_ElementMoved(object sender, DiagramNet.Events.ElementEventArgs e)
  {
    AppendLog("designer1_ElementMoved: {0}", e.ToString());
  }

  private void designer1_ElementMoving(object sender, DiagramNet.Events.ElementEventArgs e)
  {
    AppendLog("designer1_ElementMoving: {0}", e.ToString());
  }

  private void designer1_ElementResized(object sender, DiagramNet.Events.ElementEventArgs e)
  {
    AppendLog("designer1_ElementResized: {0}", e.ToString());
  }

  private void designer1_ElementResizing(object sender, DiagramNet.Events.ElementEventArgs e)
  {
    AppendLog("designer1_ElementResizing: {0}", e.ToString());
  }

  private void designer1_ElementConnected(object sender, DiagramNet.Events.ElementConnectEventArgs e)
  {
    AppendLog("designer1_ElementConnected: {0}", e.ToString());
  }

  private void designer1_ElementConnecting(object sender, DiagramNet.Events.ElementConnectEventArgs e)
  {
    AppendLog("designer1_ElementConnecting: {0}", e.ToString());
  }

  private void designer1_ElementSelection(object sender, DiagramNet.Events.ElementSelectionEventArgs e)
  {
    AppendLog("designer1_ElementSelection: {0}", e.ToString());
  }

  #endregion

  private void AppendLog(string log)
  {
    AppendLog(log, "");
  }

  private void AppendLog(string log, params object[] args)
  {
    if (mnuShowDebugLog.Checked)
      txtLog.AppendText(String.Format(log, args) + "\r\n");
  }

  private void menuSaveas_Click(object sender, EventArgs e)
  {
    File_SaveAs();
  }

  private void TbCommentBoxNode_Click(object sender, EventArgs e)
  {
    Action_Add(ElementType.CommentBox);
  }

  private void TbDiagramBlockNode_Click(object sender, EventArgs e)
  {
    Action_Add(ElementType.DiagramBlock);
  }
}