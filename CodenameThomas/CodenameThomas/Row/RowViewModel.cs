using CodenameThomas.Field;
using CodenameThomas.WordLists;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CodenameThomas.Row
{
    public class RowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private FieldViewModel _field1;
        private FieldViewModel _field2;
        private FieldViewModel _field3;
        private FieldViewModel _field4;
        private FieldViewModel _field5;

        private int _counter;
        #endregion

        #region Properties
        public FieldViewModel Field0
        {
            get => this._field1;
            set
            {
                if (this._field1 != value)
                {
                    this._field1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field1"));
                }
            }
        }

        public FieldViewModel Field1
        {
            get => this._field2;
            set
            {
                if (this._field2 != value)
                {
                    this._field2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field2"));
                }
            }
        }

        public FieldViewModel Field2
        {
            get => this._field3;
            set
            {
                if (this._field3 != value)
                {
                    this._field3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field3"));
                }
            }
        }

        public FieldViewModel Field3
        {
            get => this._field4;
            set
            {
                if (this._field4 != value)
                {
                    this._field4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field4"));
                }
            }
        }

        public FieldViewModel Field4
        {
            get => this._field5;
            set
            {
                if (this._field5 != value)
                {
                    this._field5 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field5"));
                }
            }
        }

        #endregion

        #region Constructor

        public RowViewModel()
        {
            this._counter = 0;
            this.Field0 = new FieldViewModel();
            this.Field1 = new FieldViewModel();
            this.Field2 = new FieldViewModel();
            this.Field3 = new FieldViewModel();
            this.Field4 = new FieldViewModel();

        }

        #endregion

        //return value bool-> true if added, false if limit is reached
        public CheckResult AddLetter(string letter)
        {
            if (letter.Length != 1)
                return CheckResult.LetterLengthIsNotOne;


            switch (this._counter)
            {
                case 0:
                    this.Field0.SetLetter(letter,this._counter);
                    break;

                case 1:
                    this.Field1.SetLetter(letter,this._counter);
                    break;

                case 2:
                    this.Field2.SetLetter(letter, this._counter);
                    break;

                case 3:
                    this.Field3.SetLetter(letter, this._counter);
                    break;

                case 4:
                    this.Field4.SetLetter(letter, this._counter);
                    break;

                default:
                    return CheckResult.PostitionIsOutOfRange;
            }
            this._counter++;

            return CheckResult.Success;

        }

        public CheckResult RemoveLetter()
        {
            switch (this._counter)
            {
                case 5:
                    this.Field4.RemoveLetter();
                    this._counter--;
                    break;
                case 4:
                    this.Field3.RemoveLetter();
                    this._counter--;
                    break;
                case 3:
                    this.Field2.RemoveLetter();
                    this._counter--;
                    break;
                case 2:
                    this.Field1.RemoveLetter();
                    this._counter--;
                    break;
                case 1:
                    this.Field0.RemoveLetter();
                    this._counter--;
                    break;
                default:
                    return CheckResult.PostitionIsOutOfRange;
            }

            return CheckResult.Success;
        }

        public CheckResult CheckWord(string correctWord)
        {
            if (this._counter != 5)
                return CheckResult.PositionIsNotFive;

            if (WordList_German.IsWordInList(this.GetWordFromFields()) == false)
                return CheckResult.WordIsNotInList;

            var correct0 = this.Field0.Check(correctWord);
            var correct1 = this.Field1.Check(correctWord);
            var correct2 = this.Field2.Check(correctWord);
            var correct3 = this.Field3.Check(correctWord);
            var correct4 = this.Field4.Check(correctWord);

            if (correct0 && correct1 && correct2 && correct3 && correct4)
                return CheckResult.Success;

            return CheckResult.WrongWord;
        }

        private string GetWordFromFields()
        {
            return Field0.UsedLetter
                + Field1.UsedLetter
                + Field2.UsedLetter
                + Field3.UsedLetter
                + Field4.UsedLetter;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
