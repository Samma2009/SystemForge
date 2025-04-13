using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;

namespace testDocking
{
    internal partial class ProjectExplorer : ToolWindow
    {
        Form1 form;
        internal static Dictionary<string, Func<string, string,bool>> ExtDict = new Dictionary<string, Func<string, string,bool>>();
        internal ProjectExplorer(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            this.TabText = "Project explorer";
            TreeNode RootNode = new TreeNode();
            RootNode.Name = Path.GetFileName(Program.ProjectDirectory);
            RootNode.Text = "Project";
            RootNode.ImageIndex = 0;

            foreach (var item in Directory.GetFileSystemEntries(Program.ProjectDirectory))
            {
                if (Directory.Exists(item))
                {
                    RootNode.Nodes.Add(SpawnDirectory(item));
                }
                else
                {
                    RootNode.Nodes.Add(SpawnFile(item));
                }
            }

            treeView1.Nodes.Add(RootNode);
        }

        TreeNode SpawnDirectory(string dir)
        {
            var n = new TreeNode();
            n.Text = Path.GetFileName(dir);
            n.Name = dir;
            if (Directory.GetFileSystemEntries(dir).Length > 0) n.Nodes.Add("tmp");
            return n;
        }

        TreeNode SpawnFile(string dir)
        {
            var n = new TreeNode();
            n.Text = Path.GetFileName(dir);
            n.Name = dir;
            switch (Path.GetExtension(dir))
            {
                case ".blk": n.ImageIndex = 1; break;
                case ".c": n.ImageIndex = 1; break;
                case ".h": n.ImageIndex = 1; break;
                default: n.ImageIndex = 1; break;
            }
            n.SelectedImageIndex = n.ImageIndex;

            return n;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            foreach (var item in Directory.GetFileSystemEntries(e.Node.Name))
            {
                if (Directory.Exists(item))
                {
                    e.Node.Nodes.Add(SpawnDirectory(item));
                }
                else
                {
                    e.Node.Nodes.Add(SpawnFile(item));
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (File.Exists(e.Node.Name))
            {
                switch (Path.GetExtension(e.Node.Name))
                {
                    case ".c":
                    case ".h":
                    case ".txt":
                        form.OpenDocumentText(e.Node.Text,e.Node.Name);
                        break;
                    case ".blk":
                        form.OpenDocumentBlockly(e.Node.Text, e.Node.Name);
                        break;
                    default:
                        if (ExtDict.ContainsKey(Path.GetExtension(e.Node.Name)))
                        {
                            ExtDict[Path.GetExtension(e.Node.Name)].Invoke(e.Node.Text, e.Node.Name);
                        }
                        break;
                }
            }
        }

        string nodedir = "";

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button== MouseButtons.Right)
            {
                var n = treeView1.GetNodeAt(e.Location);
                treeView1.SelectedNode = n;

                if (n == null) n = treeView1.Nodes[0];
                if (string.IsNullOrEmpty(n.Name)) return;

                if (Directory.Exists(n.Name))
                {
                    nodedir = n.Name;
                    contextMenuStrip1.Show(treeView1,e.Location);
                }
            }
        }

        private void newCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fn = Path.Combine(nodedir, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".c");
            File.WriteAllText(fn, "");

            var n = treeView1.SelectedNode;
            if (n == null) n = treeView1.Nodes[0];

            n.Nodes.Add(SpawnFile(fn));
        }

        private void newBlocklyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fn = Path.Combine(nodedir, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".blk");
            File.WriteAllText(fn, "");

            var n = treeView1.SelectedNode;
            if (n == null) n = treeView1.Nodes[0];

            n.Nodes.Add(SpawnFile(fn));
        }
    }
}
