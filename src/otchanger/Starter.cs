using System;
using System.Windows.Forms;

namespace otchanger
{
    class Starter : ApplicationContext
    {
        public Starter(Action action)
        {
            action();
        }
    }
}
