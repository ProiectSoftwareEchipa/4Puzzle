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
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Members

        private object tutorialWins;

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        #endregion Private Members

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            imageError.Source = null;

            //localSettings.Values["TutorialWins"] = 1;

            localSettings.Values["SinglePlayerEasyWins"] = 3;

            localSettings.Values["SinglePlayerEasyBestTime"] = int.MaxValue;

            localSettings.Values["SinglePlayerMediumWins"] = 3;

            localSettings.Values["SinglePlayerMediumBestTime"] = int.MaxValue;

            localSettings.Values["SinglePlayerHardWins"] = 3;

            localSettings.Values["SinglePlayerHardBestTime"] = int.MaxValue;

            _4puzzleUtils.TrySendOfflineScore();
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

            if (AppSettings.Sound)
            {
                imageSound.Source = new BitmapImage(new Uri("ms-appx:///Images/soundon-icon.png"));
            }
            else
            {
                imageSound.Source = new BitmapImage(new Uri("ms-appx:///Images/soundoff-icon.png"));
            }
        }

        #endregion Overrides

        #region Private Event Handlers

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //TO DO Try make app exit on back here
        }

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            this.Frame.Navigate(typeof(Tutorial_information), null);
            imageError.Source = null;
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            tutorialWins = localSettings.Values["TutorialWins"];

            int tutorialWinsInt = int.MinValue;
            if(tutorialWins != null)
            {
                tutorialWinsInt = (int)tutorialWins;
            }

            if (tutorialWins == null || tutorialWinsInt == 0)
            {
                this.Frame.Navigate(typeof(Tutorial_information), null);
            }
            else
            {
                this.Frame.Navigate(typeof(SinglePlayerMenu), null);
            }
            imageError.Source = null;
        }

        private void Rankings_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            if (!(NetworkInterface.GetIsNetworkAvailable()))
            {
                imageError.Source = new BitmapImage(new Uri("ms-appx:///Images/errorMsjNoInternet.png"));
                return;
            }
            this.Frame.Navigate(typeof(Rankings), null);
            imageError.Source = null;
        }

        private void imageSound_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                AppSettings.Sound = false;
                imageSound.Source = new BitmapImage(new Uri("ms-appx:///Images/soundoff-icon.png"));
            }
            else
            {
                AppSettings.Sound = true;
                imageSound.Source = new BitmapImage(new Uri("ms-appx:///Images/soundon-icon.png"));
            }
        }

        #endregion Private Event Handlers
    }
}
