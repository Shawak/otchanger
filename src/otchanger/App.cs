using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLua;

namespace otchanger
{
    static class App
    {
        static bool stop;
        static Lua lua;

        [STAThread]
        static void Main()
        {
            new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Starter(() =>
                {
                    lua = new Lua();
                    lua.LoadCLRPackage();
                    LuaRegister.RegisterClass(lua, typeof(App), true);
                    LuaRegister.RegisterClass(lua, typeof(NativeMethods));

                    foreach (var file in new[] { "data/init.lua", "../data/init.lua", "init.lua" })
                        if (File.Exists(file))
                        {
                            var fileInfo = new FileInfo(file);
                            Directory.SetCurrentDirectory(fileInfo.Directory.FullName);
                            dofile(fileInfo.FullName);
                            return;
                        }

                    NativeMethods.AllocConsole();
                    print("cannot find init.lua!");
                    print("press any key to exit..");
                    Console.ReadKey(true);
                    exit();
                }));
            })
            {
                IsBackground = true
            }.Start();

            while (!stop)
                Thread.Sleep(1);
        }

        public static void exit()
        {
            stop = true;
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
                Console.WriteLine(ex);
            }
        }

        public static void dostring(string str)
        {
            try
            {
                lua.DoString(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
    }
}
