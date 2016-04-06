using NLua;
using ShawLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace otchanger
{
    static class App
    {
        static Lua lua;
        static bool console;

        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(start());
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

                foreach (var file in new[] { "data/otchanger.lua", "../data/otchanger.lua", "otchanger.lua" })
                    if (File.Exists(file))
                    {
                        var fileInfo = new FileInfo(file);
                        Directory.SetCurrentDirectory(fileInfo.Directory.FullName);
                        dofile(fileInfo.FullName);
                        return;
                    }

                showConsole();
                print("can not find otchanger.lua!");
                exit(true);
            });
        }

        public static void exit(bool wait = false)
        {
            if (wait)
            {
                print("press any key to exit..");
                if (console)
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

        public static void print(object obj)
        {
            Debug.WriteLine(obj);
            if (console)
                Console.WriteLine(obj);
        }

        public static void dump(object o)
        {
            print(o.ToString() + " " + o.GetType().ToString());
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
                print(ex);
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

        public static void showConsole()
        {
            if (console)
                return;

            NativeMethods.AllocConsole();
            console = true;
        }

        public static void hideConsole()
        {
            if (!console)
                return;

            console = false;
            NativeMethods.FreeConsole();
        }

        static void registerLuaClass(Lua lua, Type type, bool extractFromClass = false)
        {
            if (!extractFromClass)
                lua.DoString(type.Name + "={}");

            foreach (var method in type.GetMethods())
                if (method.IsPublic && !method.IsVirtual && !method.IsSecuritySafeCritical)
                    lua.RegisterFunction(!extractFromClass ? (type.Name + "." + method.Name) : method.Name, lua, method);
        }

        public static Array toArray(LuaTable table, Type t)
        {
            var arr = Array.CreateInstance(t, table.Values.Count);
            int i = 0;
            foreach (var val in table.Values)
                arr.SetValue(Convert.ChangeType(val, t), i++);
            return arr;
        }

        public static byte[] getBytes(object val)
        {
            if (val.GetType() == typeof(double))
                val = Convert.ToInt64(val);
            var ret = ((IConvertible)val).GetBytes();
            return ret;
        }

        static void write(Memory mem, IntPtr addr, IConvertible val)
        {
            var size = val.GetType() == typeof(string) ? ((string)val).Length : val.MemSize();
            var protection = mem.RemoveProtection(addr, size);
            mem.Write(addr, (string)val);
            mem.AddProtection(addr, size, protection);
        }

        static T read<T>(Memory mem, IntPtr addr) where T : IConvertible
        {
            var size = typeof(T) == typeof(string) ? 309 : typeof(T).MemSize();
            var protection = mem.RemoveProtection(addr, size);
            var ret = mem.Read<T>(addr);
            mem.AddProtection(addr, size, protection);
            return ret;
        }

        public static void writeInt(Memory mem, IntPtr addr, IConvertible val) { write(mem, addr, val); }
        public static void writeString(Memory mem, IntPtr addr, string val) { write(mem, addr, val); }

        public static int readInt(Memory mem, IntPtr addr) { return read<int>(mem, addr); }
        public static string readString(Memory mem, IntPtr addr) { return read<string>(mem, addr); }
    }
}
