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
using static System.Net.Mime.MediaTypeNames;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace testDocking
{
    internal partial class BlocklyDoc : SaveableDocument
    {
        internal static bool BlocklyCached = false;
        internal static Dictionary<string, (string json, string function)> BlockDefinitionCache = new Dictionary<string, (string json, string function)>();
        WebView2 vw2;
        public BlocklyDoc()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;

            vw2 = new WebView2();
            vw2.Dock = DockStyle.Fill;
            this.Controls.Add(vw2);
            vw2.Source = new Uri(Path.Combine(System.Windows.Forms.Application.StartupPath, @"blockly.ui.cache.html"));
            vw2.CoreWebView2InitializationCompleted += (s, e) =>
            {
                vw2.CoreWebView2.Settings.AreDevToolsEnabled = true;
                vw2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                vw2.CoreWebView2.Settings.IsZoomControlEnabled = false;
            };
            vw2.NavigationCompleted += async (a, b) => 
            {
                
                foreach (var item in BlockDefinitionCache)
                {
                    await vw2.ExecuteScriptAsync($"addBlockToBlockly({item.Value.json},{item.Value.function})");
                }

                if (File.ReadAllText(m_fileName) != "")
                    await vw2.ExecuteScriptAsync($"loadWorkspace({File.ReadAllText(m_fileName)})");
            };

            vw2.KeyDown += (s, e) =>
            {
                var sh = Form1.GetMenuShortcuts(Form1.Current.toolStrip1.Items);
                if (sh.ContainsKey(e.KeyData))
                {
                    sh[e.KeyData].PerformClick();
                };
            };
        }

        public static void GenCache()
        {
            if (!BlocklyCached)
            {
                var BlocklyCache = File.ReadAllText(Path.Combine(System.Windows.Forms.Application.StartupPath, @"blockly\UI.html"));

                BlockDefinitionCache.Clear();

                string toolboxCache = "";

                foreach (var item in Directory.GetDirectories("Plugins"))
                {
                    if (!Directory.Exists(Path.Combine(item, "blockly"))) continue;

                    foreach (var item1 in Directory.GetDirectories(Path.Combine(item, "blockly")))
                    {
                        bool dospawn = Path.GetExtension(item1) != ".NOSPAWN";
                        toolboxCache += dospawn?$"<category name=\"{Path.GetFileNameWithoutExtension(item1)}\" colour=\"{GenerateSeededHexColor(Path.GetFileNameWithoutExtension(item1))}\">\n":"";
                        foreach (var item2 in Directory.GetFiles(item1, "*.json"))
                        {
                            var json = File.ReadAllText(item2);
                            var id = JsonConvert.DeserializeObject<BlocklyBlock>(json).type;
                            var func = File.ReadAllText(item2.Replace(".json", ".js"));

                            BlockDefinitionCache.Add(id, (json, func));
                            toolboxCache += dospawn?$"<block type=\"{id}\"></block>\n":"";
                        }
                        toolboxCache += dospawn?$"</category>\n":"";
                    }
                }
                BlocklyCache = BlocklyCache.Replace("$(Category_PlaceHolder)$", toolboxCache);
                File.WriteAllText(Path.Combine(System.Windows.Forms.Application.StartupPath, @"blockly.ui.cache.html"), BlocklyCache);
                BlocklyCached = true;
            }
        }

        public override async void Save()
        {
            File.WriteAllText(m_fileName,await vw2.ExecuteScriptAsync($"saveWorkspace()"));
        }

        private string m_fileName = string.Empty;
        public string FileName
        {
            get { return m_fileName; }
            set
            {
                m_fileName = value;
            }
        }
        public static string GenerateSeededHexColor(string seed)
        {
            int seedHash = seed.GetHashCode();
            Random random = new Random(seedHash);
            int red = random.Next(0, 256);
            int green = random.Next(0, 256);
            int blue = random.Next(0, 256);
            string hexColor = $"#{red:X2}{green:X2}{blue:X2}";

            return hexColor;
        }

    }

    internal class BlocklyBlock
    {
        public string type;
    }

}
