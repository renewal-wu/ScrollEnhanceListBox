using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace CustomControl
{
    [TemplatePartAttribute(Name = "Header", Type = typeof(ContentPresenter))]
    [TemplatePartAttribute(Name = "listBox", Type = typeof(ListBox))]
    [TemplateVisualState(Name = "SwipeUp", GroupName = "SwipeState")]
    [TemplateVisualState(Name = "SwipeDown", GroupName = "SwipeState")]
    public sealed class ScrollEnhancedListBox : ContentControl
    {
        ScrollBar scrollBar;
        Double headerHeight = 0;
        Double changePosition;
        public readonly Double miniMoveLimit = 15;
        public readonly Double totalMoveLimit = 150;

        private Boolean _isScrollBarDirecteUp;
        /// <summary>
        /// 目前是否為向上捲動
        /// </summary>
        Boolean isScrollBarDirecteUp
        {
            get
            {
                return _isScrollBarDirecteUp;
            }
            set
            {
                if (value != _isScrollBarDirecteUp)
                {
                    _isScrollBarDirecteUp = value;
                    isScrollBarDirecteUpChanged(_isScrollBarDirecteUp);
                }
            }
        }

        #region UI event
        public delegate void ScrollBarDirecteUpChangeHandler(object sender, Boolean isDirectUp);
        public event ScrollBarDirecteUpChangeHandler ScrollBarDirecteUpChanged;
        #endregion

        #region Header related properties
        #endregion

        #region ListBox related properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ScrollEnhancedListBox), new PropertyMetadata(null));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ScrollEnhancedListBox), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemsPanelTemplateProperty =
            DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(ScrollEnhancedListBox), new PropertyMetadata(null));

        public ItemsPanelTemplate ItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ScrollEnhancedListBox), new PropertyMetadata(null));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        #endregion

        public ScrollEnhancedListBox()
        {
            this.DefaultStyleKey = typeof(ScrollEnhancedListBox);
            this.Loaded += ListBoxWithScrollBar_Loaded;
        }

        private void ListBoxWithScrollBar_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox listbox = base.GetTemplateChild("listBox") as ListBox;

            if (listbox != null)
            {
                List<ScrollViewer> controlScrollViewerList = GetVisualChildCollection<ScrollViewer>(sender);

                if (controlScrollViewerList != null && controlScrollViewerList.Count > 0)
                {
                    List<ScrollBar> controlScrollBarList = GetVisualChildCollection<ScrollBar>(controlScrollViewerList.First(o =>
                    {
                        return o.Name == "ScrollViewer";
                    }));

                    scrollBar = controlScrollBarList.First(o => "VerticalScrollBar" == o.Name);

                    if (scrollBar!= null)
                    {
                        scrollBar.ValueChanged += scrollBar_ValueChanged;
                        isScrollBarDirecteUp = true;
                        changePosition = 0;
                    }
                }
            }
            else
            {
                throw new MemberAccessException();
            }

            ContentPresenter Header = base.GetTemplateChild("Header") as ContentPresenter;

            if (Header!= null)
            {
                headerHeight = Header.ActualHeight;
            }
            else
            {
                throw new MemberAccessException();
            }
        }

        private void scrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //if (e.NewValue > headerHeight)

            var diff = (Math.Abs(e.NewValue - e.OldValue));
            Debug.WriteLine("diff: " + diff.ToString());
            //避免因微動而頻繁更動
            if (e.NewValue > headerHeight && (Math.Abs(e.NewValue - e.OldValue) > miniMoveLimit || Math.Abs(e.NewValue - changePosition) > totalMoveLimit))
            {
                isScrollBarDirecteUp = e.NewValue < e.OldValue;
                changePosition = e.NewValue;
            }
        }

        private void isScrollBarDirecteUpChanged(Boolean isDirectUp)
        {
            Debug.WriteLine("isScrollBarDirecteUpChanged to: " + (isDirectUp ? "up" : "down"));
            VisualStateManager.GoToState(this, isDirectUp ? "SwipeUp" : "SwipeDown", true);
            NotifyScrollBarDirecteUpChanged(this, isDirectUp);
        }

        private void NotifyScrollBarDirecteUpChanged(ScrollEnhancedListBox scrollEnhancedListBox, bool isDirectUp)
        {
            if (ScrollBarDirecteUpChanged!= null)
            {
                ScrollBarDirecteUpChanged(scrollEnhancedListBox, isDirectUp);
            }
        }

        #region visual tree helper
        private static List<T> GetVisualChildCollection<T>(object parent) where T : UIElement
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : UIElement
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                    visualCollection.Add(child as T);
                else if (child != null)
                    GetVisualChildCollection(child, visualCollection);
            }
        }
        #endregion
    }
}
