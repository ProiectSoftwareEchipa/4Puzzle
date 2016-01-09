using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Graphics.Display;
using _4Puzzle.Generators;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            _4puzzleUtils.TrySendOfflineScore();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["SinglePlayerEasyWins"] = 0;

            localSettings.Values["SinglePlayerEasyBestTime"] = int.MaxValue;

            localSettings.Values["SinglePlayerMediumWins"] = 0;

            localSettings.Values["SinglePlayerMediumBestTime"] = int.MaxValue;

            localSettings.Values["SinglePlayerHardWins"] = 0;

            localSettings.Values["SinglePlayerHardBestTime"] = int.MaxValue;
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

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Tutorial), null);
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SinglePlayerMenu), null);
        }

        private void Rankings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Rankings), null);
        }

        #endregion Private Event Handlers
    }
}
