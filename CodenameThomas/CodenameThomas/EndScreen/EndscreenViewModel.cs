using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace CodenameThomas.EndScreen
{
    public class EndscreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private bool _hasWon;
        private string _correctWord;
        #endregion

        #region Properties

        public bool HasWon
        {
            get => this._hasWon;
            set
            {
                if(this._hasWon != value)
                {
                    this._hasWon = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasWon"));
                }
            }
        }

        public string CorrectWord
        {
            get => this._correctWord;
            set
            {
                if(this._correctWord != value)
                {
                    this._correctWord = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CorrectWord"));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand PlayAnimationCommand { get; }
        public ICommand CancelAnimationCommand { get; }
        public ICommand ShareCommand { get; }
        #endregion

        #region Constructor

        public EndscreenViewModel(bool haswo)
        {

        }
        public EndscreenViewModel(bool hasWon, string correctWord)
        {
            this.HasWon = hasWon;
            this.CorrectWord = correctWord;
        }

        #endregion


    }
}
