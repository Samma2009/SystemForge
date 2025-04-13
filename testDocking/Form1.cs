using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using LibGit2Sharp;
using Microsoft.Web.WebView2.WinForms;
using testDocking;
using WeifenLuo.WinFormsUI.Docking;

namespace testDocking
{
    internal partial class Form1 : Form
    {

        // Constants for window messages
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public static Form1 Current { get; private set; }

        public Form1()
        {
            InitializeComponent();

            Current = this;

            dockPanel1.Theme = vS2015DarkTheme1;

            new ProjectExplorer(this).Show(dockPanel1);
            new BuildConsole().Show(dockPanel1,DockState.DockBottom);
            visualStudioToolStripExtender1.SetStyle(toolStrip1,VisualStudioToolStripExtender.VsVersion.Vs2015,vS2015DarkTheme1);
            BlocklyDoc.GenCache();
            PluginLoader.LoadPlugins(this);
        }

        public void OpenDocumentText(string name,string file)
        {
            BaseDoc doc = CreateNewDocumentText(name,file);
            doc.Show(dockPanel1);
        }
        public void OpenDocumentBlockly(string name, string file)
        {
            var doc = CreateNewDocumentBlockly(name, file);
            doc.Show(dockPanel1);
        }

        private BaseDoc CreateNewDocumentText(string text,string file)
        {
            BaseDoc dummyDoc = new BaseDoc(file);
            dummyDoc.Text = text;
            return dummyDoc;
        }

        private BlocklyDoc CreateNewDocumentBlockly(string text, string file)
        {
            BlocklyDoc dummyDoc = new BlocklyDoc();
            dummyDoc.Text = text;
            dummyDoc.FileName = file;
            return dummyDoc;
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveDocument != null)
            {
                (dockPanel1.ActiveDocument as SaveableDocument).Save();
                Console.WriteLine("saved");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point doffset;

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            doffset = MousePosition;
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (fullscreen)
                {
                    Location = new Point(MousePosition.X-(prevbuffer.Size.Width /2), MousePosition.Y);
                    Size = prevbuffer.Size;
                    toolStripButton2.Text = "\uE922";
                    fullscreen = false;
                }
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        bool fullscreen;
        Rectangle prevbuffer = new Rectangle();

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTBOTTOMRIGHT, 0);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Location != Screen.FromControl(this).WorkingArea.Location)
                fullscreen = false;

            if (!fullscreen)
            {
                prevbuffer = new Rectangle(Location,Size);
                Location = Screen.FromControl(this).WorkingArea.Location;
                Size = Screen.FromControl(this).WorkingArea.Size;
                toolStripButton2.Text = "\uE923";
            }
            else
            {
                Location = prevbuffer.Location;
                Size = prevbuffer.Size;
                toolStripButton2.Text = "\uE922";
            }
            fullscreen = !fullscreen;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new ProjectExplorer(this).Show(dockPanel1);
        }

        WebView2 BlocklyBuilderView;

        void buildBlockly()
        {
            async void builditerator()
            {
                try
                {
                    BuildConsole.tw.Clear();
                    Console.WriteLine("Saving unsaved documents");
                    foreach (SaveableDocument item in dockPanel1.Documents)
                    {
                        item.Save();
                    }

                    if (!Directory.Exists($"{Program.ProjectDirectory}/Compiled"))
                    {
                        Directory.CreateDirectory($@"{Program.ProjectDirectory}/Compiled");
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync("https://github.com/Samma2009/SystemForge/releases/download/template/template.zip");
                            response.EnsureSuccessStatusCode();

                            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                            File.WriteAllBytes($"{Program.ProjectDirectory}/Compiled/template.zip", fileBytes);

                            ZipFile.ExtractToDirectory($"{Program.ProjectDirectory}/Compiled/template.zip", $"{Program.ProjectDirectory}/Compiled");
                        }
                    }

                    try
                    {
                        Directory.Delete($"{Program.ProjectDirectory}/compiled/kernel/bin", true);
                        Directory.Delete($"{Program.ProjectDirectory}/compiled/kernel/obj", true);
                        Directory.Delete($"{Program.ProjectDirectory}/compiled/kernel/src/blockly", true);
                    }
                    catch (Exception)
                    {
                    }
                    Directory.CreateDirectory($"{Program.ProjectDirectory}/compiled/kernel/src/blockly");

                    string CompiledFile = "#include \"utils.h\"\n";

                    foreach (var item in Directory.GetFiles($"{Program.ProjectDirectory}", "*.blk", SearchOption.AllDirectories))
                    {
                        Console.WriteLine("Building " + Path.GetFileName(item));
                        var f = File.ReadAllText(item);
                        if (f != "")
                        {
                            await BlocklyBuilderView.ExecuteScriptAsync($"loadWorkspace({f})");
                            var bcache = await BlocklyBuilderView.ExecuteScriptAsync($"buildWorkspace()");
                            CompiledFile += bcache.Substring(1, bcache.Length - 2)+"\n";

                            if (bcache.Contains("void entry(struct limine_framebuffer *fb)"))
                            {
                                CompiledFile =  CompiledFile.Insert("#include \"utils.h\"\n".Length, "struct limine_framebuffer *framebuffer;\nvolatile uint32_t *fb_ptr;\n");
                            }

                            BuildConsole.tw.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Done!");
                            BuildConsole.tw.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            BuildConsole.tw.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Skipped!");
                            BuildConsole.tw.ForegroundColor = ConsoleColor.White;
                        }
                    }

                    File.WriteAllText($"{Program.ProjectDirectory}/compiled/kernel/src/blockly/Blockly.c", CompiledFile.Replace(@"\n", Environment.NewLine).Replace(@"\t", "\t").Replace(@"\u003C","<"));

                    var Cfiles = Directory.GetFiles($"{Program.ProjectDirectory}", "*.c");
                    var Hfiles = Directory.GetFiles($"{Program.ProjectDirectory}", "*.h");

                    foreach (var item in Cfiles.Concat(Hfiles))
                    {
                        File.WriteAllText($"{Program.ProjectDirectory}/compiled/kernel/src/blockly/" + Path.GetFileName(item), File.ReadAllText(item));
                    }

                    Console.WriteLine("running make");

                    Directory.SetCurrentDirectory($"{Program.ProjectDirectory}/compiled");

                    // Run the make all command in WSL bash
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"-e bash -c make all",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = new Process())
                    {
                        process.StartInfo = processInfo;
                        process.Start();
                        process.WaitForExit();
                    }

                    Directory.SetCurrentDirectory("../..");

                    Console.WriteLine("Build completed");
                }
                catch (Exception ex)
                {
                    BuildConsole.tw.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Build failed! Exception: " + ex.Message);
                    BuildConsole.tw.ForegroundColor = ConsoleColor.White;
                }
            }

            if (BlocklyBuilderView == null)
            {
                BlocklyBuilderView = new WebView2();
                BlocklyBuilderView.Source = new Uri(Path.Combine(System.Windows.Forms.Application.StartupPath, @"blockly.ui.cache.html"));
                BlocklyBuilderView.NavigationCompleted += async (a, b) =>
                {
                    foreach (var item in BlocklyDoc.BlockDefinitionCache)
                    {
                        await BlocklyBuilderView.ExecuteScriptAsync($"addBlockToBlockly({item.Value.json},{item.Value.function})");
                    }
                    builditerator();
                };
            }
            else
            {
                builditerator();
            }
        }

        public static Dictionary<Keys, ToolStripMenuItem> GetMenuShortcuts(ToolStripItemCollection items)
        {
            var shortcuts = new Dictionary<Keys, ToolStripMenuItem>();

            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    if (menuItem.ShortcutKeys != Keys.None)
                    {
                        shortcuts[menuItem.ShortcutKeys] = menuItem;
                    }
                    if (menuItem.DropDownItems.Count > 0)
                    {
                        foreach (var item1 in GetMenuShortcuts(menuItem.DropDownItems))
                        {
                            shortcuts[item1.Key] = item1.Value;
                        }
                    }
                }
                else if (item is ToolStripDropDownItem dropdownItem)
                {
                    foreach (var item1 in GetMenuShortcuts(dropdownItem.DropDownItems))
                    {
                        shortcuts[item1.Key] = item1.Value;
                    }
                }
            }
            return shortcuts;
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BuildConsole().Show(dockPanel1, DockState.DockBottom);
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            BlocklyDoc.BlocklyCached = false;
            BlocklyDoc.GenCache();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            buildBlockly();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            buildBlockly();
        }
    }
}