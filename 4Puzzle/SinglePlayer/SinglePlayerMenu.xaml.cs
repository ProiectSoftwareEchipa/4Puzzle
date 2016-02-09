using _4Puzzle.Generators;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SinglePlayerMenu : Page
    {
        #region Private Members

        private int winsEasy;

        private int winsMedium;

        private int winsHard;

        #endregion Private Members

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        #region Constructors

        public SinglePlayerMenu()
        {
            this.InitializeComponent();

            LoadStoredData();

            imageError.Source = null;

            ImageBrush buttonMediumActive = new ImageBrush();
            buttonMediumActive.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/button_Medium.png"));

            ImageBrush buttonMediumInactive = new ImageBrush();
            buttonMediumInactive.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/button_Medium_disabled.png"));

            ImageBrush buttonHardActive = new ImageBrush();
            buttonHardActive.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/button_Hard.png"));

            ImageBrush buttonHardInactive = new ImageBrush();
            buttonHardInactive.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/button_Hard_disabled.png"));

            if (winsEasy < 3)
            {
                SinglePlayerMedium.Background = buttonMediumInactive;
            }
            else
            {
                SinglePlayerMedium.Background = buttonMediumActive;
            }

            if(winsMedium < 3)
            {
                SinglePlayerHard.Background = buttonHardInactive;
            }
            else
            {
                SinglePlayerHard.Background = buttonHardActive;
            }
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
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.Navigate(typeof(MainPage), null);
            }
        }

        private void SinglePlayerEasy_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            imageError.Source = null;
            this.Frame.Navigate(typeof(SinglePlayerEasy), null);
        }

        private void SinglePlayerMedium_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            if (winsEasy < 3)
            {
                imageError.Source = new BitmapImage(new Uri("ms-appx:///Images/errorMsjMediumMode.png"));
                return;
            }
            imageError.Source = null;
            this.Frame.Navigate(typeof(SinglePlayerMedium), null);
        }

        private void SinglePlayerHard_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            if (winsMedium < 3)
            {
                imageError.Source = new BitmapImage(new Uri("ms-appx:///Images/errorMsjHardMode.png"));
                return;
            }
            imageError.Source = null;
            this.Frame.Navigate(typeof(SinglePlayerHard), null);
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

        #region Private Methods

        private void LoadStoredData()
        {
            object winsEasy = localSettings.Values["SinglePlayerEasyWins"];

            if (winsEasy != null)
            {
                this.winsEasy = (int)winsEasy;
            }
            else
            {
                this.winsEasy = int.MinValue;
            }

            object winsMedium = localSettings.Values["SinglePlayerMediumWins"];

            if (winsMedium != null)
            {
                this.winsMedium = (int)winsMedium;
            }
            else
            {
                this.winsMedium = int.MinValue;
            }

            object winsHard = localSettings.Values["SinglePlayerHardWins"];

            if (winsHard != null)
            {
                this.winsHard = (int)winsHard;
            }
            else
            {
                this.winsHard = int.MinValue;
            }
        }

        #endregion Private Methods
    }
}
