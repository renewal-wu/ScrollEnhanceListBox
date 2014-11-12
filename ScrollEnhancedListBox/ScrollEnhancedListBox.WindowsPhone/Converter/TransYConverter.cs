using System;
using Windows.UI.Xaml.Data;

namespace ScrollEnhancedListBox.Converter
{
    public class TransYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Double source = (Double)value;
            return -1 * source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
