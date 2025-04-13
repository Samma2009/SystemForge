using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using testDocking;
using TestPlugin;
public class PluginClass
{
    public void Init(Register register)
    {
        ToolStripDropDownButton tbn = new ToolStripDropDownButton("AI assistant");

        ToolStripButton OpenAI = new ToolStripButton("Open AI assistant");
        OpenAI.Click += (a, b) => register.ShowDocument(new AITool());
        tbn.DropDownItems.Add(OpenAI);

        ToolStripButton Login = new ToolStripButton("Login");
        tbn.DropDownItems.Add(Login);

        register.RegisterCategories(tbn);
    }
}