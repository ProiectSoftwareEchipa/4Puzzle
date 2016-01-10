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
    public sealed partial class Tutorial_information : Page
    {
        #region Private Members

        private int pageNumber;

        #endregion Private Members

        #region Constructors

        public Tutorial_information()
        {
            this.InitializeComponent();

            this.pageNumber = 1;
        }

        #endregion Constructors

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        #region Event Handlers

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.Navigate(typeof(MainPage), null);
            }
        }

        #endregion Event Handlers

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            if(pageNumber == 1)
            {
                pageNumber = 2;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The board";
                textBlockPageNumber.Text = "-2-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage2.png"));
            }
            else if (pageNumber == 2)
            {
                pageNumber = 3;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The goal";
                textBlockPageNumber.Text = "-3-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage3.png"));
            }
            else if (pageNumber == 3)
            {
                pageNumber = 4;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The goal";
                textBlockPageNumber.Text = "-4-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage4.png"));
            }
            else if (pageNumber == 4)
            {
                pageNumber = 5;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The moves";
                textBlockPageNumber.Text = "-5-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage5.png"));
            }
            else if (pageNumber == 5)
            {
                pageNumber = 6;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The moves";
                textBlockPageNumber.Text = "-6-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage6.png"));
            }
            else if (pageNumber == 6)
            {
                pageNumber = 7;
                textBlockText.Text = "After all these infos I am sure you are ready to face the real challenge. Press the right button and try to the finish level. After you have won, Easy mode will be unlocked.";
                textBlockTitle.Text = "The conclusions";
                textBlockPageNumber.Text = "-7-";
                imageCurrentPage.Source = null;
            }
            else if (pageNumber == 7)
            {
                this.Frame.Navigate(typeof(Tutorial), null);
            }
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 2)
            {
                pageNumber = 1;
                textBlockText.Text = "The next few images will teach you the basics about playing the game. After that you will get to try what you have learned. Please press the right button.";
                textBlockTitle.Text = "Welcome to 4Puzzle!";
                textBlockPageNumber.Text = "-1-";
                imageCurrentPage.Source = null;
            }
            else if (pageNumber == 3)
            {
                pageNumber = 2;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The board";
                textBlockPageNumber.Text = "-2-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage2.png"));
            }
            else if (pageNumber == 4)
            {
                pageNumber = 3;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The goal";
                textBlockPageNumber.Text = "-3-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage3.png"));
            }
            else if (pageNumber == 5)
            {
                pageNumber = 4;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The goal";
                textBlockPageNumber.Text = "-4-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage4.png"));
            }
            else if (pageNumber == 6)
            {
                pageNumber = 5;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The moves";
                textBlockPageNumber.Text = "-5-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage5.png"));
            }
            else if (pageNumber == 7)
            {
                pageNumber = 6;
                textBlockText.Text = String.Empty;
                textBlockTitle.Text = "The moves";
                textBlockPageNumber.Text = "-6-";
                imageCurrentPage.Source = new BitmapImage(new Uri("ms-appx:///Images/tutorialPage6.png"));
            }
        }
    }
}
