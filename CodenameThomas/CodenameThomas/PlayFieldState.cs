using System;
using System.Collections.Generic;
using System.Text;

namespace CodenameThomas
{
    public enum PlayFieldState
    {
        NotChecked = 0,
        RightLetterRightPlace = 1,
        RightLetterWrongPlace = 2,
        WrongLetter = 3,
    }
}