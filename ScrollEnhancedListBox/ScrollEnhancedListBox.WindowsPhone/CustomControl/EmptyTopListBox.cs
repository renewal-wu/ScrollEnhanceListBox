using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CustomControl
{
    public class EmptyTopListBox : ListBox
    {
        public static readonly DependencyProperty EmptyTopHeightProperty =
            DependencyProperty.Register("EmptyTopHeight", typeof(Double), typeof(EmptyTopListBox), new PropertyMetadata(0));

        public object EmptyTopHeight
        {
            get { return (Double)GetValue(EmptyTopHeightProperty); }
            set { SetValue(EmptyTopHeightProperty, value); }
        }

        public EmptyTopListBox()
        {
            this.DefaultStyleKey = typeof(EmptyTopListBox);
        }
    }
}
