using System;
using System.Threading;
using System.Windows.Forms;
using NLua;

namespace otchanger
{
    public static class App
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
                    dofile("data/init.lua");
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
    }
}
