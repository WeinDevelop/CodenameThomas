using System;
using System.Collections.Generic;
using System.Text;

namespace CodenameThomas
{
    public enum CheckResult
    {
        Default = 0,
        Success = 1,
        CorrectWord = 2,
        LetterLengthIsNotOne = 3,
        PostitionIsOutOfRange = 4,
        PositionIsNotFive = 5,
        WrongWord = 6,
        WordIsNotInList = 7,
    }
}
