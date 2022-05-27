using System;
using System.Collections.Generic;
using System.Text;

namespace CodenameThomas
{
    public class AlphabetLetter
    {
        public char Letter;
        public PlayFieldState State;
        public AlphabetLetter(char letter, PlayFieldState state)
        {
            this.Letter = letter;
            this.State = state;
        }
    }
}
