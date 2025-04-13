using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace testDocking
{
    public abstract class SaveableDocument : DockContent
    {
        public abstract void Save();
    }
}
