using CodenameThomas.EndScreen;
using CodenameThomas.Field;
using CodenameThomas.Row;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.IO;
using CodenameThomas.WordLists;

namespace CodenameThomas
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }

        #region Field

        private int _counter;

        //TODO Countdown ? für was ?
        // benutzte Wörter abspeichern
        //TODO resette benutzte Wörter
        // Wörterbücher https://github.com/LibreOffice/dictionaries
        //TODO benutzte Buchstaben in der Tastatur einfärben
        //TODO Result Object for Errors
        //TODO Effekte für RückgabeWert

        private int _lastPlayedGameState;
        private int _lastPlayedIndex;
        private int _currentLineNumber;
        private int _postition;

        private string _correctWordOfTheDay;

        private RowViewModel _row0;
        private RowViewModel _row1;
        private RowViewModel _row2;
        private RowViewModel _row3;
        private RowViewModel _row4;
        private RowViewModel _row5;

        //TODO gleich eine Liste von Buttons ? wie die Anordnung in der UI festlegen
        private ObservableCollection<AlphabetLetter> _alphabet;
        private Image _testImage;

        private string _resultMessage;

        #endregion

        #region Properties

        public int Counter
        {
            get => this._counter;
            set
            {
                if (this._counter != value)
                {
                    this._counter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Counter"));
                }
            }
        }


        public int Position
        {
            get => this._postition;
            set
            {
                if (this._postition != value)
                {
                    this._postition = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Position"));
                }
            }
        }

        public RowViewModel Row0
        {
            get => this._row0;
            set
            {
                if (this._row0 != value)
                {
                    this._row0 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row0"));
                    this.ResultMessage = string.Empty;
                }
            }
        }
        public RowViewModel Row1
        {
            get => this._row1;
            set
            {
                if (this._row1 != value)
                {
                    this._row1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row1"));
                    this.ResultMessage = string.Empty;
                }
            }
        }

        public RowViewModel Row2
        {
            get => this._row2;
            set
            {
                if (this._row2 != value)
                {
                    this._row2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row2"));
                    this.ResultMessage = string.Empty;
                }
            }
        }

        public RowViewModel Row3
        {
            get => this._row3;
            set
            {
                if (this._row3 != value)
                {
                    this._row3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row3"));
                    this.ResultMessage = string.Empty;
                }
            }
        }
        public RowViewModel Row4
        {
            get => this._row4;
            set
            {
                if (this._row4 != value)
                {
                    this._row4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row4"));
                    this.ResultMessage = string.Empty;
                }
            }
        }

        public RowViewModel Row5
        {
            get => this._row5;
            set
            {
                if (this._row5 != value)
                {
                    this._row5 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row5"));
                    this.ResultMessage = string.Empty;
                }
            }
        }

        public Image TestImage
        {
            get => this._testImage;
            set
            {
                if (this._testImage != value)
                {
                    this._testImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TestImage"));
                }
            }
        }

        public ObservableCollection<AlphabetLetter> Alphabet
        {
            get => this._alphabet;
            set
            {
                if (this._alphabet != value)
                {
                    this._alphabet = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Alphabet"));
                }
            }
        }

        public string ResultMessage
        {
            get => this._resultMessage;
            set
            {
                if (this._resultMessage != value)
                {
                    this._resultMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResultMessage"));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand AddLetterCommand { get; }
        public ICommand RemoveLetterCommand { get; }
        public ICommand CheckWordCommand { get; }
        public ICommand ClearResultMessageCommand { get; }
        #endregion

        #region Constructor

        public MainPageViewModel(INavigation navigation)
        {

            this.Navigation = navigation;

            if (Application.Current.Properties.ContainsKey("LastPlayedIndex") && Application.Current.Properties["LastPlayedIndex"] is int lastPlayedIndex)
            {
                this._lastPlayedIndex = lastPlayedIndex;
            }

            //0 -> Ingame/not finished, 1 finished and won, 2 finished and loose
            if (Application.Current.Properties.ContainsKey("LastPlayedGameState") && Application.Current.Properties["LastPlayedGameState"] is int lastPlayedGameState)
            {
                this._lastPlayedGameState = lastPlayedGameState;
            }

            var currentIndex = (int)DateTime.Now.Subtract(new DateTime(2022, 04, 01)).TotalDays;

            //6 Versuche -> 6Felder tief
            //5 Buchstaben -> 5Felder breit

            this._correctWordOfTheDay = WordList_German.GetWordOfTheDay(currentIndex);

            this.AddLetterCommand = new Command<string>(this.AddLetter);
            this.CheckWordCommand = new Command(this.CheckWord);
            this.RemoveLetterCommand = new Command(this.RemoveLetter);
            this.ClearResultMessageCommand = new Command(() => this.ResultMessage = string.Empty);


            this.Alphabet = new ObservableCollection<AlphabetLetter>();

            this.LoadAlphabet();
            this.LoadSave();

            this.LoadRows(this._lastPlayedIndex != currentIndex);

            this._lastPlayedIndex = currentIndex;
        }

        #endregion

        #region Methods

        #region Load
        private void InitRows()
        {
            this.Row0 = new RowViewModel();
            this.Row1 = new RowViewModel();
            this.Row2 = new RowViewModel();
            this.Row3 = new RowViewModel();
            this.Row4 = new RowViewModel();
            this.Row5 = new RowViewModel();
        }

        private void LoadSave()
        {
            this._lastPlayedIndex = Preferences.Get("LastPlayedIndex", -1);

            //0 -> Ingame/not finished, 1 finished and won, 2 finished and loose
            this._lastPlayedGameState = Preferences.Get("LastPlayedGameState", 0);
        }

        private void LoadRows(bool newGame)
        {
            if (Preferences.ContainsKey("row0") && !newGame)
                this.Row0 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row0", null));
            else
                this.Row0 = new RowViewModel();

            if (Preferences.ContainsKey("row1") && !newGame)
                this.Row1 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row1", null));
            else
                this.Row1 = new RowViewModel();

            if (Preferences.ContainsKey("row2") && !newGame)
                this.Row2 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row2", null));
            else
                this.Row2 = new RowViewModel();

            if (Preferences.ContainsKey("row3") && !newGame)
                this.Row3 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row3", null));
            else
                this.Row3 = new RowViewModel();

            if (Preferences.ContainsKey("row4") && !newGame)
                this.Row4 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row4", null));
            else
                this.Row4 = new RowViewModel();

            if (Preferences.ContainsKey("row5") && !newGame)
                this.Row5 = JsonConvert.DeserializeObject<RowViewModel>(Preferences.Get("row5", null));
            else
                this.Row5 = new RowViewModel();
        }

        #endregion

        #region Save

        public void SaveAll()
        {
            Preferences.Set("row0", this.Row0.ToString());
            Preferences.Set("row1", this.Row1.ToString());
            Preferences.Set("row2", this.Row2.ToString());
            Preferences.Set("row3", this.Row3.ToString());
            Preferences.Set("row4", this.Row4.ToString());
            Preferences.Set("row5", this.Row5.ToString());

            Preferences.Set("LastPlayedIndex", this._lastPlayedIndex);
            Preferences.Set("LastPlayedGameState", this._lastPlayedGameState);
        }

        #endregion
        public void AddLetter(string letter)
        {
            if (letter.Length > 1 || this.Counter > 5)
                return;//Fehlermeldung, Exception oder gar nichts ?

            switch (this.Counter)
            {
                case 0:
                    this.Row0.AddLetter(letter);
                    this.SaveAll();
                    break;
                case 1:
                    this.Row1.AddLetter(letter);
                    this.SaveAll();
                    break;
                case 2:
                    this.Row2.AddLetter(letter);
                    this.SaveAll();
                    break;
                case 3:
                    this.Row3.AddLetter(letter);
                    this.SaveAll();
                    break;
                case 4:
                    this.Row4.AddLetter(letter);
                    this.SaveAll();
                    break;
                case 5:
                    this.Row5.AddLetter(letter);
                    this.SaveAll();
                    break;
                default:
                    break;
            }

        }

        public void CheckWord()
        {
            //TODO Effekte für RückgabeWert
            CheckResult result = CheckResult.Default;
            switch (this.Counter)
            {
                case 0:
                    result = this.Row0.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                case 1:
                    result = this.Row1.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                case 2:
                    result = this.Row2.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                case 3:
                    result = this.Row3.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                case 4:
                    result = this.Row4.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                case 5:
                    result = this.Row5.CheckWord(this._correctWordOfTheDay);
                    this.SaveAll();
                    break;
                default:
                    break;
            }

            this.SetResultMessage(result);
            
            if (result == CheckResult.WrongWord)
                this.Counter++;

            if (result == CheckResult.Success)
            {
                this.ShowEndScreen(true, this._correctWordOfTheDay);
                this._lastPlayedGameState = 1;
                this.ResultMessage = "Win";
                //TODO Fenster anzeigen
            }
            else if (result == CheckResult.WrongWord && this.Counter == 6)
            {
                this.ShowEndScreen(false, this._correctWordOfTheDay);
                this._lastPlayedGameState = 2;
            }
            else
            {
                this._lastPlayedGameState = 0;
                //this.PaintTheAlphabet();
            }
        }

        private void SetResultMessage(CheckResult checkResult)
        {
            switch (checkResult)
            {
                case CheckResult.Default:
                    this.ResultMessage = string.Empty;
                    break;
                case CheckResult.LetterLengthIsNotOne: // shouldn't happen
                    this.ResultMessage = "How did you do this ?";
                    break;
                case CheckResult.PositionIsNotFive:
                    this.ResultMessage = "Bitte fülle alle Felder.";
                    break;
                case CheckResult.PostitionIsOutOfRange: // shouldn't happen
                    this.ResultMessage = "Please don't do that.";
                    break;
                case CheckResult.WordIsNotInList:
                    this.ResultMessage = "Invalides Wort";
                    break;
                case CheckResult.WrongWord: // no message, bc the rows jump to the next one
                    this.ResultMessage = string.Empty;
                    break;
                default:
                    this.ResultMessage = string.Empty;
                    break;
            }
        }

        private void ShowEndScreen(bool hasWon, string score)
        {
            var endScreenView = new EndscreenView();
            endScreenView.BindingContext = new EndscreenViewModel(hasWon, score);

            if (hasWon)
                Application.Current.MainPage.DisplayAlert("THE END", "You won", "Good luck tomorrow");
            else
                Application.Current.MainPage.DisplayAlert("The End", "You loose", "Good luck tomorrow");

            //this.Navigation.PushModalAsync(endScreenView);

        }

        public void RemoveLetter()
        {

            switch (this.Counter)
            {
                case 0:
                    this.Row0.RemoveLetter();
                    this.SaveAll();
                    break;
                case 1:
                    this.Row1.RemoveLetter();
                    this.SaveAll();
                    break;
                case 2:
                    this.Row2.RemoveLetter();
                    this.SaveAll();
                    break;
                case 3:
                    this.Row3.RemoveLetter();
                    this.SaveAll();
                    break;
                case 4:
                    this.Row4.RemoveLetter();
                    this.SaveAll();
                    break;
                case 5:
                    this.Row5.RemoveLetter();
                    this.SaveAll();
                    break;
                default:
                    break;
            }
        }

        private void LoadAlphabet()
        {
            for (int i = 65; i <= 90; i++)
            {
                this.Alphabet.Add(new AlphabetLetter((char)i, PlayFieldState.NotChecked));
            }
        }

        #endregion
    }
}
