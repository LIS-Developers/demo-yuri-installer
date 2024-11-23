using net_yuri_installer.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace net_yuri_installer
{
    internal static class Program
    {
        private static readonly string[] dllPaths = new string[4]
        {
            Application.StartupPath + @"\AxInterop.WMPLib.dll",
            Application.StartupPath + @"\Interop.WMPLib.dll",
            Application.StartupPath + @"\7z.dll",
            Application.StartupPath + @"\SevenZipSharp.dll"
        };

        // 定义互斥体对象
        private static Mutex mutex;

        [STAThread]
        static void Main()
        {
            ReleaseDLL();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new _StartWindow();
            Application.Run(form);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = $"/C timeout /t 1 & del \"{dllPaths[0]}\" & del \"{dllPaths[1]}\" & del \"{dllPaths[2]}\" & del \"{dllPaths[3]}\"";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
            Environment.Exit(0);
        }

        static void ReleaseDLL()
        {
            byte[][] dlls = new byte[4][]
            {
                Resources.AxInterop_WMPLib,
                Resources.Interop_WMPLib,
                Resources._7z,
                Resources.SevenZipSharp
            };

            if (dlls.Length != dllPaths.Length)
            {
                throw new LRXDontLoveMeException("DLL不匹配。");
            }

            for (int i = 0; i < dlls.Length; i++)
            {
                using (FileStream fs = new FileStream(dllPaths[i], FileMode.Create))
                {
                    fs.Write(dlls[i], 0, dlls[i].Length);
                }
            }
        }
    }
}