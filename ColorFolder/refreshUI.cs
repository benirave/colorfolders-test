using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ColorFolder
{
    public class refreshUI
    {
        [DllImport("user32")]
        private static extern int PostMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32")]
        private static extern IntPtr FindWindow(string className, string caption);
        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr startChild, string className, string caption);
        public static void Refresh()
        {
            IntPtr d = FindWindow("Progman", "Program Manager");
            d = FindWindowEx(d, IntPtr.Zero, "SHELLDLL_DefView", null);
            d = FindWindowEx(d, IntPtr.Zero, "SysListView32", null);
            PostMessage(d, 0x100, new IntPtr(0x74), IntPtr.Zero);//WM_KEYDOWN = 0x100  VK_F5 = 0x74
            PostMessage(d, 0x101, new IntPtr(0x74), new IntPtr(1 << 31));//WM_KEYUP = 0x101
        }
    }
}
