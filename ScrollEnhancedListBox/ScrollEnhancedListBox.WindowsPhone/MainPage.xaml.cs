using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace ScrollEnhancedListBox
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            StatusBar statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundColor = Color.FromArgb(255, 0, 174, 216);
            statusBar.BackgroundOpacity = 1;
        }

        /// <summary>
        /// 在此頁面即將顯示在框架中時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。
        /// 這個參數通常用來設定頁面。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<String> testData = new List<String> { "Top item" };

            for(int i = 0;i < 100;i++)
            {
                testData.Add(i.ToString());
            }

            this.testListBox.ItemsSource = testData;
            this.testListBox.ScrollBarDirecteUpChanged += testListBox_ScrollBarDirecteUpChanged;
        }

        private void testListBox_ScrollBarDirecteUpChanged(object sender, bool isDirectUp)
        {
            Debug.WriteLine("event handler, result: " + isDirectUp.ToString());
            this.BottomAppBar.ClosedDisplayMode = isDirectUp ? AppBarClosedDisplayMode.Compact : AppBarClosedDisplayMode.Minimal;
        }
    }
}
