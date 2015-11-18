using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SinglePlayerMenu : Page
    {
        #region Constructors

        public SinglePlayerMenu()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        #endregion Overrides

        #region Private Event Handlers

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void SinglePlayerEasy_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SinglePlayerEasy), null);
        }

        private void SinglePlayerMedium_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SinglePlayerMedium), null);
        }

        private void SinglePlayerHard_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SinglePlayerHard), null);
        }

        #endregion Private Event Handlers
    }
}
