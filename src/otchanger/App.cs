using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLua;
using ShawLib;

namespace otchanger
{
    static class App
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        static Lua lua;

        [STAThread]
        static void Main()
        {
            new Thread(() =>
            {
                Application.ThreadException += Application_ThreadException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(start());
            })
            {
                IsBackground = true
            }.Start();

            while (true)
                Thread.Sleep(1);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            showConsole();
            showException(e.Exception);
            exit(true);
        }

        static Starter start()
        {
            return new Starter(() =>
            {
                lua = new Lua();
                lua.LoadCLRPackage();
                registerLuaClass(lua, typeof(App), true);
                registerLuaClass(lua, typeof(NativeMethods));

                foreach (var file in new[] { "data/init.lua", "../data/init.lua", "init.lua" })
                    if (File.Exists(file))
                    {
                        var fileInfo = new FileInfo(file);
                        Directory.SetCurrentDirectory(fileInfo.Directory.FullName);
                        dofile(fileInfo.FullName);
                        return;
                    }

                print("cannot find init.lua!");
                exit(true);
            });
        }

        static void registerLuaClass(Lua lua, Type type, bool extractFromClass = false)
        {
            if (!extractFromClass)
                lua.DoString(type.Name + "={}");

            foreach (var method in type.GetMethods())
                if (method.IsPublic && !method.IsVirtual && !method.IsSecuritySafeCritical)
                    lua.RegisterFunction(!extractFromClass ? (type.Name + "." + method.Name) : method.Name, lua, method);
        }

        public static void write(Memory mem, IntPtr address, int val)
        {
            var size = new UIntPtr((uint)val.MemSize());
            var protection = mem.RemoveProtection(address, size);
            mem.Write(address, val);
            mem.AddProtection(address, size, protection);
        }

        public static int readInt(Memory mem, IntPtr address)
        {
            var size = new UIntPtr(4);
            var protection = mem.RemoveProtection(address, size);
            var ret = mem.Read<int>(address);
            mem.AddProtection(address, size, protection);
            return ret;
        }

        public static void dump(object o)
        {
            print(o.ToString() + " " + o.GetType().ToString());
        }

        public static IntPtr hex(string val)
        {
            return new IntPtr(Int64.Parse(val, NumberStyles.HexNumber));
        }

        public static void exit(bool wait = false)
        {
            if (wait)
            {
                print("press any key to exit..");
                Console.ReadKey(true);
            }

            try
            {
                Application.Exit();
            }
            catch
            { }  // don't handle any lua script errors during shutdown

            Environment.Exit(0);
        }

        public static void print(string str)
        {
            Debug.WriteLine(str);
            Console.WriteLine(str);
        }

        public static void dofile(string file)
        {
            try
            {
                lua.DoFile(file);
            }
            catch (Exception ex)
            {
                showConsole();
                showException(ex);
            }
        }

        static void showException(Exception ex)
        {
            do
            {
                Console.WriteLine(ex);
                ex = ex.InnerException;
            } while (ex != null);
        }

        public static string exportAssemblies()
        {
            var sb = new StringBuilder();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                sb.AppendLine(">> " + ass.FullName);
                foreach (var name in ass.GetReferencedAssemblies())
                    sb.AppendLine("> " + name.FullName);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        static bool shown;

        public static void showConsole()
        {
            if (shown)
                return;

            shown = true;
            NativeMethods.AllocConsole();
        }

        public static void hideConsole()
        {
            if (!shown)
                return;

            shown = false;
            NativeMethods.FreeConsole();
        }
    }
}
