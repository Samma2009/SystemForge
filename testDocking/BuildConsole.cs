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
    internal partial class BuildConsole : ToolWindow
    {
        public static CustomTextWriter tw;
        internal BuildConsole()
        {
            InitializeComponent();
            this.TabText = "Console";
            tw = new CustomTextWriter(richTextBox1);
            Console.SetOut(tw);

        }

        private void BuildConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.SetOut(TextWriter.Null);
        }
    }

    public class CustomTextWriter : TextWriter
    {
        RichTextBox tb;
        public ConsoleColor ForegroundColor = ConsoleColor.White;
        public CustomTextWriter(RichTextBox tb) : base() 
        {
            this.tb = tb;
        }
        public override Encoding Encoding => Console.OutputEncoding;
        public override void WriteLine(string value)
        {
            tb.SelectionColor = ConsoleColorConverter.ConvertToColor(ForegroundColor);
            tb.AppendText(value +NewLine);
        }
        public override void Write(string value)
        {
            tb.SelectionColor = ConsoleColorConverter.ConvertToColor(ForegroundColor);
            tb.AppendText(value);
        }
        public void Clear()
        {
            tb.Clear();
        }
    }

    public static class ConsoleColorConverter
    {
        private static readonly Dictionary<ConsoleColor, Color> ConsoleColorMap = new Dictionary<ConsoleColor, Color>
    {
        { ConsoleColor.Black, Color.Black },
        { ConsoleColor.DarkBlue, Color.DarkBlue },
        { ConsoleColor.DarkGreen, Color.DarkGreen },
        { ConsoleColor.DarkCyan, Color.DarkCyan },
        { ConsoleColor.DarkRed, Color.DarkRed },
        { ConsoleColor.DarkMagenta, Color.DarkMagenta },
        { ConsoleColor.DarkYellow, Color.FromArgb(255, 128, 0) }, // There's no direct equivalent, so we approximate
        { ConsoleColor.Gray, Color.Gray },
        { ConsoleColor.DarkGray, Color.DarkGray },
        { ConsoleColor.Blue, Color.DodgerBlue},
        { ConsoleColor.Green, Color.Green },
        { ConsoleColor.Cyan, Color.Cyan },
        { ConsoleColor.Red, Color.Red },
        { ConsoleColor.Magenta, Color.Magenta },
        { ConsoleColor.Yellow, Color.Yellow },
        { ConsoleColor.White, Color.AntiqueWhite }
    };

        public static Color ConvertToColor(ConsoleColor consoleColor)
        {
            if (ConsoleColorMap.TryGetValue(consoleColor, out Color color))
            {
                return color;
            }
            else
            {
                throw new ArgumentException("Invalid console color.");
            }
        }
    }
}
