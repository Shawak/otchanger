using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace otchanger
{
    class Memory
    {
        // Global Variable
        public static IntPtr ProcessHandle = IntPtr.Zero;

        // Native Functions
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(int DesiredAccess, bool InheritHandle, int ProcessID);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr Handle);

        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadByte(IntPtr Handle, int Address, ref byte Value, int Size = 1);
        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadShort(IntPtr Handle, int Address, ref short Value, int Size = 2);
        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadInteger(IntPtr Handle, int Address, ref int Value, int Size = 4);
        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadLong(IntPtr Handle, int Address, ref long Value, int Size = 8);
        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadSingle(IntPtr Handle, int Address, ref float Value, int Size = 4);
        [DllImport("KERNEL32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool ReadDouble(IntPtr Handle, int Address, ref double Value, int Size = 8);

        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteByte(IntPtr Handle, int Address, ref byte Value, int Size = 1);
        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteShort(IntPtr Handle, int Address, ref short Value, int Size = 2);
        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteInteger(IntPtr Handle, int Address, ref int Value, int Size = 4);
        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteLong(IntPtr Handle, int Address, ref long Value, int Size = 8);
        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteSingle(IntPtr Handle, int Address, ref float Value, int Size = 4);
        [DllImport("KERNEL32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool WriteDouble(IntPtr Handle, int Address, ref double Value, int Size = 8);

        public static uint DELETE = 0x00010000;
        public static uint READ_CONTROL = 0x00020000;
        public static uint WRITE_DAC = 0x00040000;
        public static uint WRITE_OWNER = 0x00080000;
        public static uint SYNCHRONIZE = 0x00100000;
        public static uint END = 0xFFF; //if you have Windows XP or Windows Server 2003 you must change this to 0xFFFF
        public static uint PROCESS_ALL_ACCESS = (DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER | SYNCHRONIZE | END);

        // API-Implentation
        public static bool OpenProcess(int PID)
        {
            ProcessHandle = OpenProcess((int)PROCESS_ALL_ACCESS, false, PID);
            return (ProcessHandle == IntPtr.Zero ? false : true);
        }

        public static bool CloseHandle()
        {
            if (ProcessHandle == IntPtr.Zero)
                return false;
            return CloseHandle(ProcessHandle);
        }

        // Read Functions
        public static byte ReadByte(int Address, int Size = 1)
        {
            byte value = 0;
            ReadByte(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static short ReadShort(int Address, int Size = 2)
        {
            short value = 0;
            ReadShort(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static int ReadInteger(int Address, int Size = 4)
        {
            int value = 0;
            ReadInteger(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static long ReadLong(int Address, int Size = 8)
        {
            long value = 0;
            ReadLong(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static float ReadSingle(int Address, int Size = 8)
        {
            float value = 0;
            ReadSingle(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static double ReadDouble(int Address, int Size = 8)
        {
            double value = 0;
            ReadDouble(ProcessHandle, Address, ref value, Size);
            return value;
        }
        public static string ReadString(int Address)
        {
            byte[] b = new byte[] { 0 };
            string str = "";
            bool ret = false;
            do
            {
                ret = ReadByte(ProcessHandle, Address, ref b[0]);
                Address += 0x1;
                if (ret == true)
                    if (b[0] != 0)
                        str += System.Text.Encoding.UTF8.GetString(b);
            } while (b[0] != 0);
            return str;
        }

        // Write Functions
        public static bool WriteByte(int Address, byte Value, int Size = 1)
        {
            return WriteByte(ProcessHandle, Address, ref Value, Size);
        }
        public static bool WriteShort(int Address, short Value, int Size = 2)
        {
            return WriteShort(ProcessHandle, Address, ref Value, Size);
        }
        public static bool WriteInteger(int Address, int Value, int Size = 4)
        {
            return WriteInteger(ProcessHandle, Address, ref Value, Size);
        }
        public static bool WriteLong(int Address, long Value, int Size = 8)
        {
            return WriteLong(ProcessHandle, Address, ref Value, Size);
        }
        public static bool WriteSingle(int Address, float Value, int Size = 8)
        {
            return WriteSingle(ProcessHandle, Address, ref Value, Size);
        }
        public static bool WriteDouble(int Address, double Value, int Size = 8)
        {
            return WriteDouble(ProcessHandle, Address, ref Value, Size);
        }

        // Protection
        [DllImport("KERNEL32", EntryPoint = "VirtualProtectEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualProtectEx(IntPtr Handle, int Address, int Size, uint NewProtection, ref uint OldProtection);

        public static uint RemoveProtection(int Address, int Size)
        {
            uint OldProtection = 0;
            VirtualProtectEx(ProcessHandle, Address, Size, 0x40, ref OldProtection);
            return OldProtection;
        }

        public static void AddProtection(int Address, int Size, uint Protection)
        {
            VirtualProtectEx(ProcessHandle, Address, Size, Protection, ref Protection);
        }

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, ref uint lpflOldProtect);

        public static uint RemoveProtect(Process Proc, int Address, int BytesLength)
        {
            uint oldProtection = 0;
            VirtualProtectEx(ProcessHandle, new IntPtr(Address), new IntPtr(BytesLength), 0x40, ref oldProtection);
            return oldProtection;
        }

        public static bool AddProtect(Process Proc, int Address, int BytesLength, uint oldProtection)
        {
            var gameLookUp = Process.GetProcessesByName(Proc.ProcessName);
            if (gameLookUp.Length == 0)
                return false;

            VirtualProtectEx(ProcessHandle, new IntPtr(Address), new IntPtr(BytesLength), oldProtection, ref oldProtection);
            return true;
        }
    }
}
