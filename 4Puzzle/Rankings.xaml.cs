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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Rankings : Page
    {
        #region Private Members

        private string apiData;

        private List<string> listPlayers;

        private List<string> listScores;

        #endregion Private Members

        #region Constructors

        public Rankings()
        {
            this.InitializeComponent();

            this.listPlayers = new List<string>();

            this.listScores = new List<string>();
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

        #endregion Overrides

        #region Private Event Handlers

        private void buttonSinglePlayerEasy_Click(object sender, RoutedEventArgs e)
        {
            apiData = HttpRequestUtils.Select("SinglePlayerEasy");

            textBlockRankings.Text = "Scores Easy";

            PopulateListViews(apiData);
        }

        private void buttonbuttonSinglePlayerMedium_Click(object sender, RoutedEventArgs e)
        {
            apiData = HttpRequestUtils.Select("SinglePlayerMedium");

            textBlockRankings.Text = "Scores Medium";

            PopulateListViews(apiData);
        }

        private void buttonSinglePlayerHard_Click(object sender, RoutedEventArgs e)
        {
            apiData = HttpRequestUtils.Select("SinglePlayerHard");

            textBlockRankings.Text = "Scores Hard";

            PopulateListViews(apiData);
        }

        #endregion Private Event Handlers

        #region Private Methods

        private void PopulateListViews(string apiData)
        {
            string[] playersAndScores;
            playersAndScores = apiData.Split(';');
            listPlayers.Clear();
            listScores.Clear();
            listViewPlayerNames.ItemsSource = null;
            listViewPlayerScores.ItemsSource = null;

            if (apiData == String.Empty)
                return;

            foreach (string playerAndScore in playersAndScores)
            {
                string[] splitString;
                splitString = playerAndScore.Split(',');
                if (splitString.Count() < 2)
                    continue;

                listPlayers.Add(splitString[0]);
                listScores.Add(String.Format("{0}:{1}", (Convert.ToInt32(splitString[1]) / 60).ToString("00"), (Convert.ToInt32(splitString[1]) % 60).ToString("00")));
            }

            listViewPlayerNames.ItemsSource = listPlayers;
            listViewPlayerScores.ItemsSource = listScores;
        }

        #endregion Private Methods
    }
}
