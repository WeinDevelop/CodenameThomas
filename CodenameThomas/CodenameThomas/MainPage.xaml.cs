using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodenameThomas
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel ViewModel => this.BindingContext as MainPageViewModel;
        public MainPage()
        {
            BindingContext = new MainPageViewModel(Navigation);

            InitializeComponent();
        }

        public void Save()
        {

        }
    }
}
