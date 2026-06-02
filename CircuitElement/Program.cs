using System;
using System.Windows.Forms;

namespace CircuitEditor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CircuitEditorForm());
        }
    }
}