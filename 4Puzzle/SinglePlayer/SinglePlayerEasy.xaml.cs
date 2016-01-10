using _4Puzzle.Generators;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SinglePlayerEasy : Page
    {
        #region Const

        private const int gameSize = 4;

        #endregion Const

        #region Private Members

        private ImageBrush imageBrushFig1;

        private ImageBrush imageBrushFig2;

        private ImageBrush imageBrushFig3;

        private ImageBrush imageBrushFig4;

        private ImageBrush imageBrushBlank;

        private Rectangle[,] rectangleMatrix;

        private int singlePlayerEasyWins;

        private int singlePlayerEasyTimer;

        private int singlePlayerEasyBestTime;

        private DispatcherTimer dispatcherTimer;

        private bool extraCheckPopupOk;

        private bool extraCheckPopupCancel;

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public struct Tile
        {
            public int i;
            public int j;
        }

        private Tile[] blankTilePositions;

        #endregion Private Members

        #region Constructors

        public SinglePlayerEasy()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.rectangleMatrix = new Rectangle[gameSize, gameSize];

            this.imageBrushFig1 = new ImageBrush();
            imageBrushFig1.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig1.png"));

            this.imageBrushFig2 = new ImageBrush();
            imageBrushFig2.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig2.png"));

            this.imageBrushFig3 = new ImageBrush();
            imageBrushFig3.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig3.png"));

            this.imageBrushFig4 = new ImageBrush();
            imageBrushFig4.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig4.png"));

            this.imageBrushBlank = new ImageBrush();

            this.blankTilePositions = new Tile[4];

            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            this.extraCheckPopupOk = true;

            this.extraCheckPopupCancel = true;

            //Popup elemets
            PopupButtonCancel.Visibility = Visibility.Collapsed;
            PopupButtonOk.Visibility = Visibility.Collapsed;
            PopupRectangle.Visibility = Visibility.Collapsed;
            PopupTextBlockMessage.Visibility = Visibility.Collapsed;
            PopupTextBoxUsername.Visibility = Visibility.Collapsed;
            PopupTextBlockMessagePlayAgain.Visibility = Visibility.Collapsed;

            LoadStoredData();

            InitializeDispatcherTimer();

            InitializeMatrix();

            InitializeImages();

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
        }

        #endregion Overrides

        #region Event Handlers

        private void buttonPopupCancel_Click(object sender, RoutedEventArgs e)
        {
            if (singlePlayerEasyTimer <= singlePlayerEasyBestTime && singlePlayerEasyWins > 3)
            {
                if (extraCheckPopupCancel)
                {
                    PopupTextBlockMessage.Text = "Are you sure?";
                    extraCheckPopupCancel = false;
                    return;
                }
            }

                PopupButtonCancel.Visibility = Visibility.Collapsed;
            PopupButtonOk.Visibility = Visibility.Collapsed;
            PopupRectangle.Visibility = Visibility.Collapsed;
            PopupTextBlockMessage.Visibility = Visibility.Collapsed;
            PopupTextBoxUsername.Visibility = Visibility.Collapsed;
            PopupTextBlockMessagePlayAgain.Visibility = Visibility.Collapsed;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void buttonPopupOk_Click(object sender, RoutedEventArgs e)
        {
            if (singlePlayerEasyTimer <= singlePlayerEasyBestTime && singlePlayerEasyWins > 3)
            {
                if(extraCheckPopupOk && (PopupTextBoxUsername.Text == "Register Name" || PopupTextBoxUsername.Text == String.Empty))
                {
                    PopupTextBlockMessage.Text = "No name inputed!";
                    extraCheckPopupOk = false;
                    return;
                }
                if (PopupTextBoxUsername.Text != "Register Name" && PopupTextBoxUsername.Text != String.Empty)
                {
                    _4puzzleUtils.SaveScoreOffline(PopupTextBoxUsername.Text, "SinglePlayerEasy", singlePlayerEasyTimer.ToString());
                }
            }
            this.Frame.Navigate(typeof(SinglePlayerEasy), null);
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.Navigate(typeof(SinglePlayerMenu), null);
            }
        }

        /// <summary>
        /// Metoda declansata cand se selecteaza un rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            Tuple<int, int> rectangleIndex = GetRectangleIndex(rectangle);
            CheckNeighbours(rectangleIndex);

            if (CheckEndGame)
            {
                dispatcherTimer.Stop();

                singlePlayerEasyWins++;
                if(singlePlayerEasyTimer < singlePlayerEasyBestTime && singlePlayerEasyWins > 3)
                {
                    PopupButtonCancel.Visibility = Visibility.Visible;
                    PopupButtonOk.Visibility = Visibility.Visible;
                    PopupRectangle.Visibility = Visibility.Visible;
                    PopupTextBlockMessage.Visibility = Visibility.Visible;
                    PopupTextBoxUsername.Visibility = Visibility.Visible;
                    singlePlayerEasyBestTime = singlePlayerEasyTimer;
                }
                else
                {
                    PopupButtonCancel.Visibility = Visibility.Visible;
                    PopupButtonOk.Visibility = Visibility.Visible;
                    PopupRectangle.Visibility = Visibility.Visible;
                    PopupTextBlockMessagePlayAgain.Visibility = Visibility.Visible;
                }

                SaveStoredData();
                StopGame();

                _4puzzleUtils.SaveScoreOffline(null, "SinglePlayerEasy", singlePlayerEasyTimer.ToString());
            }
        }

        /// <summary>
        /// Metoda ce incrementeaza timpul trecut cu 1 secunda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, object e)
        {
            singlePlayerEasyTimer++;

            singlePlayerEasyTimeText.Text = String.Format("{0}:{1}", (singlePlayerEasyTimer / 60).ToString("00"), (singlePlayerEasyTimer % 60).ToString("00"));
        }

        private void PopupTextBoxUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = String.Empty;
            textBox.GotFocus -= PopupTextBoxUsername_GotFocus;
        }

        #endregion Event Handlers

        #region Private Methods

        /// <summary>
        /// Incarca datele stocate local
        /// </summary>
        private void LoadStoredData()
        {
            object wins = localSettings.Values["SinglePlayerEasyWins"];

            object bestTime = localSettings.Values["SinglePlayerEasyBestTime"];

            if (wins != null)
            {
                this.singlePlayerEasyWins = (int)wins;
            }
            else
            {
                this.singlePlayerEasyWins = 0;
            }

            if (bestTime != null)
            {
                this.singlePlayerEasyBestTime = (int)bestTime;
            }
            else
            {
                this.singlePlayerEasyBestTime = int.MaxValue;
            }

            this.singlePlayerEasyTimer = 0;

            singlePlayerEasyWinsText.Text = singlePlayerEasyWins.ToString();

            singlePlayerEasyTimeText.Text = String.Format("{0}:{1}", (singlePlayerEasyTimer / 60).ToString("00"), (singlePlayerEasyTimer % 60).ToString("00"));

            if (singlePlayerEasyBestTime != int.MaxValue)
            {
                singlePlayerEasyBestTimeText.Text = String.Format("{0}:{1}", (singlePlayerEasyBestTime / 60).ToString("00"), (singlePlayerEasyBestTime % 60).ToString("00"));
            }
            else
            {
                singlePlayerEasyBestTimeText.Text = "N/A";
            }
        }

        /// <summary>
        /// Salveaza datele local
        /// </summary>
        private void SaveStoredData()   
        {
            localSettings.Values["SinglePlayerEasyWins"] = singlePlayerEasyWins;

            localSettings.Values["SinglePlayerEasyBestTime"] = singlePlayerEasyBestTime;
        }

        /// <summary>
        /// Initializeaza timer-ul
        /// </summary>
        private void InitializeDispatcherTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        
        /// <summary>
        /// Initializarea matricii
        /// </summary>
        private void InitializeMatrix()
        {
            rectangleMatrix[0, 0] = Rectangle11;
            rectangleMatrix[0, 1] = Rectangle12;
            rectangleMatrix[0, 2] = Rectangle13;
            rectangleMatrix[0, 3] = Rectangle14;
            rectangleMatrix[1, 0] = Rectangle21;
            rectangleMatrix[1, 1] = Rectangle22;
            rectangleMatrix[1, 2] = Rectangle23;
            rectangleMatrix[1, 3] = Rectangle24;
            rectangleMatrix[2, 0] = Rectangle31;
            rectangleMatrix[2, 1] = Rectangle32;
            rectangleMatrix[2, 2] = Rectangle33;
            rectangleMatrix[2, 3] = Rectangle34;
            rectangleMatrix[3, 0] = Rectangle41;
            rectangleMatrix[3, 1] = Rectangle42;
            rectangleMatrix[3, 2] = Rectangle43;
            rectangleMatrix[3, 3] = Rectangle44;
        }

        /// <summary>
        /// Initializarea culorilor pentru versiunea de tutorial
        /// </summary>
        private void InitializeImages()
        {
            ImageBrush[] images = new ImageBrush[] { imageBrushFig1, imageBrushFig2, imageBrushFig3, imageBrushFig4 };

            size4Easy.Generate(ref rectangleMatrix, ref blankTilePositions, gameSize, images, 1);
        }

        /// <summary>
        /// Opreste jocul prin eliminarea eventului de tapped
        /// </summary>
        private void StopGame()
        {
            for (int i = 0; i < gameSize; i++)
                for (int j = 0; j < gameSize; j++)
                {
                    rectangleMatrix[i, j].Tapped -= Rectangle_Tapped;
                }
        }

        /// <summary>
        /// Metoda ce intoarce indecsi rectangle-ului curent
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns>Tuplul de indecsi</returns>
        private Tuple<int, int> GetRectangleIndex(Rectangle rectangle)
        {
            Tuple<int, int> rectangleIndex;

            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (rectangleMatrix[i, j] == rectangle)
                    {
                        rectangleIndex = new Tuple<int, int>(i, j);
                        return rectangleIndex;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Metoda ce verifica vecinii si daca este cazul face swap de culori
        /// </summary>
        /// <param name="rectangleIndex">Indecsi rectangle-ului curent</param>
        private void CheckNeighbours(Tuple<int, int> rectangleIndex)
        {
            int i = rectangleIndex.Item1;
            int j = rectangleIndex.Item2;

            if (!IsNearWhiteTile(i, j))
                return;

            int areaNumber = GetSelectedAreaNumber(i, j);

            SwapRectanglesImages(rectangleMatrix[i, j], rectangleMatrix[blankTilePositions[areaNumber].i, blankTilePositions[areaNumber].j]);
            blankTilePositions[areaNumber].i = i;
            blankTilePositions[areaNumber].j = j;

        }

        /// <summary>
        /// Proprietatea ce verifica daca sa ajuns in situatia de sfarsit a jocului
        /// </summary>
        private bool CheckEndGame
        {
            get
            {
                //white tiles are not in the center
                foreach (Tile tile in blankTilePositions)
                    if (!IsPositionInTheCenter(tile.i) || !IsPositionInTheCenter(tile.j))
                        return false;

                List<Brush> currentList;

                //check per line
                for (int i = 0; i < gameSize; i++)
                {
                    if (IsPositionInTheCenter(i))
                        continue;
                    currentList = new List<Brush>();
                    for (int j = 0; j < gameSize; j++)
                        if (!currentList.Contains(rectangleMatrix[i, j].Fill))
                            currentList.Add(rectangleMatrix[i, j].Fill);
                    if (currentList.Count < gameSize)
                        return false;
                }

                //check per column
                for (int j = 0; j < gameSize; j++)
                {
                    if (IsPositionInTheCenter(j))
                        continue;
                    currentList = new List<Brush>();
                    for (int i = 0; i < gameSize; i++)
                        if (!currentList.Contains(rectangleMatrix[i, j].Fill))
                            currentList.Add(rectangleMatrix[i, j].Fill);
                    if (currentList.Count < gameSize)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Metoda ce inverseaza culorile intre 2 rectangle-uri
        /// </summary>
        /// <param name="imageRectangle">Rectangle-ul colorat</param>
        /// <param name="whiteRectangle">Rectangle-ul alb</param>
        private void SwapRectanglesImages(Rectangle imageRectangle, Rectangle blankRectangle)
        {
            blankRectangle.Fill = imageRectangle.Fill;
            imageRectangle.Fill = imageBrushBlank;
            //swapSound.Play();
        }

        private bool IsPositionInTheCenter(int position)
        {

            if (position == gameSize / 2 || position == gameSize / 2 - 1)
                return true;

            return false;
        }

        private int GetSelectedAreaNumber(int i, int j)
        {

            if (i < gameSize / 2 && j < gameSize / 2)
                return 0;

            if (i < gameSize / 2 && j >= gameSize / 2)
                return 1;

            if (i >= gameSize / 2 && j < gameSize / 2)
                return 2;

            return 3;
        }

        private bool IsNearWhiteTile(int i, int j)
        {

            int areaNumber = GetSelectedAreaNumber(i, j);

            if (Math.Abs(blankTilePositions[areaNumber].i - i) + Math.Abs(blankTilePositions[areaNumber].j - j) == 1)
                return true;

            return false;
        }

        #endregion Private Methods
    }
}
