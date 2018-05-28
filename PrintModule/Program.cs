using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PrintModule
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = RunningInstance();
            if (instance != null)
            {
                HandleRunningInstance(instance);
                return;
            }
            LoadResourceDll.RegistDLL();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrintForm());
        }

        #region 确保程序只运行一个实例
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表   
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程   
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径  
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程  
                        return process;
                    }
                }
            }
            return null;
        }
        //3.已经有了就把它激活，并将其窗口放置最前端  
        private static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, 1); //调用api函数，正常显示窗口  
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端  
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
        #endregion  
    }
}
