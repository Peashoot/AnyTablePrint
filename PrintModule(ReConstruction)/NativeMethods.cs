using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PrintModule_ReConstruction_
{
    public class NativeMethods
    {
        [DllImport("User32.dll")]
        internal static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        internal static extern bool SetForegroundWindow(System.IntPtr hWnd);
    }
}
