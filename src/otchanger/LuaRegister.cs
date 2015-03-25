using System;
using NLua;
using NLua.Exceptions;

namespace otchanger
{
    static class LuaRegister
    {
        public static void RegisterClass(Lua lua, Type type, bool extractFromClass = false)
        {
            if (!extractFromClass)
                lua.DoString(type.Name + "={}");

            foreach (var method in type.GetMethods())
                if (!method.IsVirtual && !method.IsSecuritySafeCritical)
                    lua.RegisterFunction(!extractFromClass ? (type.Name + "." + method.Name) :  method.Name, lua, method);
        }
    }
}
