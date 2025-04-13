namespace testDocking
{
    partial class ProjectExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorer));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newBlocklyFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(472, 481);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Frame 7.png");
            this.imageList1.Images.SetKeyName(1, "Frame 8.png");
            this.imageList1.Images.SetKeyName(2, "Frame 9.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newCFileToolStripMenuItem,
            this.newBlocklyFileToolStripMenuItem,
            this.newTextFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 70);
            // 
            // newCFileToolStripMenuItem
            // 
            this.newCFileToolStripMenuItem.Name = "newCFileToolStripMenuItem";
            this.newCFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newCFileToolStripMenuItem.Text = "New C file";
            this.newCFileToolStripMenuItem.Click += new System.EventHandler(this.newCFileToolStripMenuItem_Click);
            // 
            // newBlocklyFileToolStripMenuItem
            // 
            this.newBlocklyFileToolStripMenuItem.Name = "newBlocklyFileToolStripMenuItem";
            this.newBlocklyFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newBlocklyFileToolStripMenuItem.Text = "New Blockly file";
            this.newBlocklyFileToolStripMenuItem.Click += new System.EventHandler(this.newBlocklyFileToolStripMenuItem_Click);
            // 
            // newTextFileToolStripMenuItem
            // 
            this.newTextFileToolStripMenuItem.Name = "newTextFileToolStripMenuItem";
            this.newTextFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newTextFileToolStripMenuItem.Text = "New Text file";
            // 
            // ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 481);
            this.Controls.Add(this.treeView1);
            this.Name = "ProjectExplorer";
            this.Text = "Project explorer";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newCFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newBlocklyFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTextFileToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}