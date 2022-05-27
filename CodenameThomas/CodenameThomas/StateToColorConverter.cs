using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CodenameThomas
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is PlayFieldState playFieldState)
            {
                if (playFieldState == PlayFieldState.RightLetterRightPlace)
                    return Color.Green;
                else if (playFieldState == PlayFieldState.RightLetterWrongPlace)
                    return Color.Orange;
                else if(playFieldState == PlayFieldState.WrongLetter)
                    return Color.Gray;
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
