using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    class PopUpWinObject : BBeBObject
    {
        public PopUpWinObject(ushort id)
            : base(id)
        {
            Type = ObjectType.PopUpWin;
        }
    }
}
