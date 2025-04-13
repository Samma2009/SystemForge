using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace testDocking
{
    internal class PluginLoader
    {
        internal static void LoadPlugins(Form1 form)
        {
            var rg = new Register()
            {
                dockpanel = form.dockPanel1,
                toolstrip = form.toolStrip1,
            };

            foreach (var item in Directory.GetDirectories("Plugins"))
            {
                if (!File.Exists(Path.Combine(item, "plugin.dll"))) continue;
                var assembly = Assembly.LoadFile(Path.GetFullPath(Path.Combine(item, "plugin.dll")));
                var myType = assembly.GetType("PluginClass");
                MethodInfo myMethod = myType.GetMethod("Init");
                var instance = Activator.CreateInstance(myType);
                myMethod.Invoke(instance, new object[] {rg});
            }
        }
    }
    public class Register
    {
        internal ToolStrip toolstrip;
        internal DockPanel dockpanel;
        public void RegisterCategories(params ToolStripItem[] items)
        {
            foreach (var item in items)
            {
                toolstrip.Items.Add(item);
            }
        }
        public void ShowDocument(DockContent document)
        {
            document.Show(dockpanel);
        }
        public void RegisterFileType(string filetype,Func<string,string,bool> registerfunction)
        {
            ProjectExplorer.ExtDict[filetype] = registerfunction;
        }
    }
}
