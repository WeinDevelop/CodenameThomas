using System;   
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CodenameThomas.Field
{
    public class FieldViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PlayFieldState _fieldState;


        private string _usedLetter;
        private int _position;

        public PlayFieldState FieldState
        {
            get => this._fieldState;
            set
            {
                if (this._fieldState != value)
                {
                    this._fieldState = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FieldState"));
                }
            }
        }

        public string UsedLetter
        {
            get => this._usedLetter;
            set
            {
                if (this._usedLetter != value)
                {
                    this._usedLetter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsedLetter"));
                }
            }
        }
      
        public bool HasBeenChecked { get; set; }

        public FieldViewModel()
        {
            this.FieldState = PlayFieldState.NotChecked;
        }

        public void SetLetter(string letter, int postition)
        {
            this.UsedLetter = letter;
            this._position = postition;
        }

        public void RemoveLetter()
        {
            this.UsedLetter = string.Empty;
            this._position = -1;
        }
        
        public bool Check(string correctWord)
        {
            this.HasBeenChecked = true;
            if (correctWord[_position].ToString().ToLower() == this.UsedLetter.ToLower())
            {
                this.FieldState = PlayFieldState.RightLetterRightPlace;
                return true;
            }
            else if (correctWord.ToLower().Contains(this.UsedLetter.ToLower()))
            {
                this.FieldState = PlayFieldState.RightLetterWrongPlace;
                return false;
            }
            else
            {
                this.FieldState = PlayFieldState.WrongLetter;
                return false;
            }
        }
    }
}
