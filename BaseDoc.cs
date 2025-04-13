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
using System.Net;

namespace testDocking
{
    internal partial class BaseDoc : SaveableDocument
    {
        WebView2 vw2;
        public BaseDoc(string filename)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;

            vw2 = new WebView2();
            vw2.Dock = DockStyle.Fill;
            this.Controls.Add(vw2);
            vw2.Source = new Uri(Path.Combine(System.Windows.Forms.Application.StartupPath, @"monaco\index.html"));
            vw2.CoreWebView2InitializationCompleted += (s, e) =>
            {
                vw2.CoreWebView2.Settings.AreDevToolsEnabled = false;
                vw2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                vw2.CoreWebView2.Settings.IsZoomControlEnabled = false;
            };
            vw2.NavigationCompleted += async (a, b) => 
            {
                await vw2.ExecuteScriptAsync($"setEditorText(`" +File.ReadAllText(filename)+ "`)");
                m_fileName = filename;
            };

            vw2.KeyDown += (s, e) =>
            {
                var sh = Form1.GetMenuShortcuts(Form1.Current.toolStrip1.Items);
                if (sh.ContainsKey(e.KeyData)) sh[e.KeyData].PerformClick();
            };
        }

        public override async void Save()
        {
            var c = WebUtility.HtmlDecode(await vw2.ExecuteScriptAsync("getEditorText()"))
                .Trim('"')
                .Replace("\\n", Environment.NewLine)
                .Replace("\\u003C", "<")
                .Replace("\\u003E", ">")
                .Replace("\\t", "\t")
                .Replace("\\\"", "\"");
            File.WriteAllText(m_fileName,c);
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
    }
}
