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
    public sealed partial class SinglePlayerMedium : Page
    {
        #region Const

        private const int gameSize = 6;

        #endregion Const

        #region Private Members

        private ImageBrush imageBrushFig1;

        private ImageBrush imageBrushFig2;

        private ImageBrush imageBrushFig3;

        private ImageBrush imageBrushFig4;

        private ImageBrush imageBrushFig5;

        private ImageBrush imageBrushFig6;

        private ImageBrush imageBrushFig1_happy;

        private ImageBrush imageBrushFig2_happy;

        private ImageBrush imageBrushFig3_happy;

        private ImageBrush imageBrushFig4_happy;

        private ImageBrush imageBrushFig5_happy;

        private ImageBrush imageBrushFig6_happy;

        private ImageBrush imageBrushBlank;

        private List<int> happyLines;

        private List<int> happyColumns;

        private Rectangle[,] rectangleMatrix;

        private int[,] rectanglePositionMatrix;

        private int singlePlayerMediumWins;

        private int singlePlayerMediumTimer;

        private int singlePlayerMediumBestTime;

        private DispatcherTimer dispatcherTimer;

        private bool extraCheckPopupOk;

        private bool extraCheckPopupCancel;

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private struct Tile
        {
            public int i;
            public int j;
        }

        //private Tile[] blankTilePositions;

        private _4Puzzle.SinglePlayerEasy.Tile[] blankTilePositions;

        #endregion Private Members

        #region Constructors

        public SinglePlayerMedium()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.rectangleMatrix = new Rectangle[gameSize, gameSize];

            this.rectanglePositionMatrix = new int[gameSize, gameSize];

            this.imageBrushFig1 = new ImageBrush();
            imageBrushFig1.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig1.png"));

            this.imageBrushFig2 = new ImageBrush();
            imageBrushFig2.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig2.png"));

            this.imageBrushFig3 = new ImageBrush();
            imageBrushFig3.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig3.png"));

            this.imageBrushFig4 = new ImageBrush();
            imageBrushFig4.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig4.png"));

            this.imageBrushFig5 = new ImageBrush();
            imageBrushFig5.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig5.png"));

            this.imageBrushFig6 = new ImageBrush();
            imageBrushFig6.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig6.png"));

            this.imageBrushFig1_happy = new ImageBrush();
            imageBrushFig1_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig1_happy.png"));

            this.imageBrushFig2_happy = new ImageBrush();
            imageBrushFig2_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig2_happy.png"));

            this.imageBrushFig3_happy = new ImageBrush();
            imageBrushFig3_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig3_happy.png"));

            this.imageBrushFig4_happy = new ImageBrush();
            imageBrushFig4_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig4_happy.png"));

            this.imageBrushFig5_happy = new ImageBrush();
            imageBrushFig5_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig5_happy.png"));

            this.imageBrushFig6_happy = new ImageBrush();
            imageBrushFig6_happy.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/fig6_happy.png"));

            this.imageBrushBlank = new ImageBrush();

            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            this.blankTilePositions = new _4Puzzle.SinglePlayerEasy.Tile[4];

            this.extraCheckPopupOk = true;

            this.extraCheckPopupCancel = true;

            this.happyLines = new List<int>();

            this.happyColumns = new List<int>();

            //Popup elemets
            PopupButtonCancel.Visibility = Visibility.Collapsed;
            PopupButtonOk.Visibility = Visibility.Collapsed;
            PopupImage.Visibility = Visibility.Collapsed;
            PopupTextBoxUsername.Visibility = Visibility.Collapsed;

            LoadStoredData();

            InitializeDispatcherTimer();

            InitializeMatrix();

            InitializeImages();

            InitializePositionMatrix();

            InitializeHappyFaces();

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

        #region Event Handlers

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

        private void buttonPopupCancel_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            if (singlePlayerMediumTimer <= singlePlayerMediumBestTime && singlePlayerMediumWins > 3)
            {
                if (extraCheckPopupCancel)
                {
                    PopupImage.Source = new BitmapImage(new Uri("ms-appx:///Images/popupWinHighscoreCancel.png"));

                    extraCheckPopupCancel = false;
                    return;
                }
            }

            PopupButtonCancel.Visibility = Visibility.Collapsed;
            PopupButtonOk.Visibility = Visibility.Collapsed;
            PopupImage.Visibility = Visibility.Collapsed;
            PopupTextBoxUsername.Visibility = Visibility.Collapsed;
            if (Frame.CanGoBack)
            {
                this.Frame.Navigate(typeof(SinglePlayerMenu), null);
            }
        }

        private void buttonPopupOk_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Sound)
            {
                buttonSound.Play();
            }

            if (singlePlayerMediumTimer < singlePlayerMediumBestTime && singlePlayerMediumWins > 3)
            {
                if (extraCheckPopupOk && (PopupTextBoxUsername.Text == "Register Name" || PopupTextBoxUsername.Text == String.Empty))
                {
                    PopupImage.Source = new BitmapImage(new Uri("ms-appx:///Images/popupWinHighscoreNoName.png"));

                    extraCheckPopupOk = false;
                    return;
                }
                if (PopupTextBoxUsername.Text != "Register Name" && PopupTextBoxUsername.Text != String.Empty)
                {
                    _4puzzleUtils.SaveScoreOffline(PopupTextBoxUsername.Text, "SinglePlayerMedium", singlePlayerMediumTimer.ToString());
                }
            }
            this.Frame.Navigate(typeof(SinglePlayerMedium), null);
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
                if (AppSettings.Sound)
                {
                    winSound.Play();
                }
                singlePlayerMediumWins++;
                if(singlePlayerMediumTimer < singlePlayerMediumBestTime && singlePlayerMediumWins > 3)
                {
                    PopupButtonCancel.Visibility = Visibility.Visible;
                    PopupButtonOk.Visibility = Visibility.Visible;
                    PopupImage.Visibility = Visibility.Visible;
                    PopupTextBoxUsername.Visibility = Visibility.Visible;
                    singlePlayerMediumBestTime = singlePlayerMediumTimer;
                }
                else
                {
                    if (singlePlayerMediumWins == 3)
                    {
                        PopupImage.Source = new BitmapImage(new Uri("ms-appx:///Images/popup3WinsMedium.png"));
                    }
                    else
                    {
                        PopupImage.Source = new BitmapImage(new Uri("ms-appx:///Images/popupWinSimple.png"));
                    }
                    PopupButtonCancel.Visibility = Visibility.Visible;
                    PopupButtonOk.Visibility = Visibility.Visible;
                    PopupImage.Visibility = Visibility.Visible;
                }

                SaveStoredData();
                StopGame();

                _4puzzleUtils.SaveScoreOffline(null , "SinglePlayerMedium", singlePlayerMediumTimer.ToString());
            }
        }

        /// <summary>
        /// Metoda ce incrementeaza timpul trecut cu 1 secunda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, object e)
        {
            singlePlayerMediumTimer++;

            singlePlayerMediumTimeText.Text = String.Format("{0}:{1}", (singlePlayerMediumTimer / 60).ToString("00"), (singlePlayerMediumTimer % 60).ToString("00"));
        }

        private void PopupTextBoxUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = String.Empty;
            textBox.GotFocus -= PopupTextBoxUsername_GotFocus;
        }

        #endregion Event Handlers

        #region Private Methods

        private void CheckCurrentHappyLinesAndColumns()
        {
            foreach (int i in happyLines.ToList())
            {
                CheckAndRemoveHappyLine(i);
            }
            foreach (int j in happyColumns.ToList())
            {
                CheckAndRemoveHappyColumn(j);
            }
            RemoveHappyFaces();
        }

        private void CheckAndRemoveHappyLine(int i)
        {
            List<int> currentList;

            currentList = new List<int>();
            for (int j = 0; j < gameSize; j++)
                if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                    currentList.Add(rectanglePositionMatrix[i, j]);
            if (currentList.Count != gameSize || currentList.Contains(0))
            {
                happyLines.Remove(i);
            }
        }

        private void CheckAndRemoveHappyColumn(int j)
        {
            List<int> currentList;

            currentList = new List<int>();
            for (int i = 0; i < gameSize; i++)
                if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                    currentList.Add(rectanglePositionMatrix[i, j]);
            if (currentList.Count != gameSize || currentList.Contains(0))
            {
                happyColumns.Remove(j);
            }
        }

        private void CheckNewHappyLine(int i)
        {
            List<int> currentList;

            currentList = new List<int>();
            for (int j = 0; j < gameSize; j++)
                if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                    currentList.Add(rectanglePositionMatrix[i, j]);
            if (currentList.Count == gameSize && !currentList.Contains(0))
            {
                happyLines.Add(i);

                for (int j = 0; j < gameSize; j++)
                {
                    if (rectangleMatrix[i, j].Fill == imageBrushFig1)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig1_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig2)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig2_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig3)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig3_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig4)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig4_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig5)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig5_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig6)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig6_happy;
                    }
                }
            }
        }

        private void CheckNewHappyColumn(int j)
        {
            List<int> currentList;

            currentList = new List<int>();
            for (int i = 0; i < gameSize; i++)
                if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                    currentList.Add(rectanglePositionMatrix[i, j]);
            if (currentList.Count == gameSize && !currentList.Contains(0))
            {
                happyColumns.Add(j);

                for (int i = 0; i < gameSize; i++)
                {
                    if (rectangleMatrix[i, j].Fill == imageBrushFig1)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig1_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig2)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig2_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig3)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig3_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig4)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig4_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig5)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig5_happy;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig6)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig6_happy;
                    }
                }
            }
        }

        private void RemoveHappyFaces()
        {
            for (int i = 0; i < gameSize; i++)
            {
                if (happyLines.Contains(i))
                    continue;

                for (int j = 0; j < gameSize; j++)
                {
                    if (happyColumns.Contains(j))
                        continue;

                    if (rectangleMatrix[i, j].Fill == imageBrushFig1_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig1;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig2_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig2;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig3_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig3;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig4_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig4;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig5_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig5;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig6_happy)
                    {
                        rectangleMatrix[i, j].Fill = imageBrushFig6;
                    }
                }
            }
        }

        /// <summary>
        /// Incarca datele stocate local
        /// </summary>
        private void LoadStoredData()
        {
            object wins = localSettings.Values["SinglePlayerMediumWins"];

            object bestTime = localSettings.Values["SinglePlayerMediumBestTime"];

            if (wins != null)
            {
                this.singlePlayerMediumWins = (int)wins;
            }
            else
            {
                this.singlePlayerMediumWins = 0;
            }

            if (bestTime != null)
            {
                this.singlePlayerMediumBestTime = (int)bestTime;
            }
            else
            {
                this.singlePlayerMediumBestTime = int.MaxValue;
            }

            this.singlePlayerMediumTimer = 0;

            singlePlayerMediumWinsText.Text = singlePlayerMediumWins.ToString();

            singlePlayerMediumTimeText.Text = String.Format("{0}:{1}", (singlePlayerMediumTimer / 60).ToString("00"), (singlePlayerMediumTimer % 60).ToString("00"));

            if (singlePlayerMediumBestTime != int.MaxValue)
            {
                singlePlayerMediumBestTimeText.Text = String.Format("{0}:{1}", (singlePlayerMediumBestTime / 60).ToString("00"), (singlePlayerMediumBestTime % 60).ToString("00"));
            }
            else
            {
                singlePlayerMediumBestTimeText.Text = "N/A";
            }
        }

        /// <summary>
        /// Salveaza datele local
        /// </summary>
        private void SaveStoredData()
        {
            localSettings.Values["SinglePlayerMediumWins"] = singlePlayerMediumWins;

            localSettings.Values["SinglePlayerMediumBestTime"] = singlePlayerMediumBestTime;
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
            rectangleMatrix[0, 4] = Rectangle15;
            rectangleMatrix[0, 5] = Rectangle16;
            rectangleMatrix[1, 0] = Rectangle21;
            rectangleMatrix[1, 1] = Rectangle22;
            rectangleMatrix[1, 2] = Rectangle23;
            rectangleMatrix[1, 3] = Rectangle24;
            rectangleMatrix[1, 4] = Rectangle25;
            rectangleMatrix[1, 5] = Rectangle26;
            rectangleMatrix[2, 0] = Rectangle31;
            rectangleMatrix[2, 1] = Rectangle32;
            rectangleMatrix[2, 2] = Rectangle33;
            rectangleMatrix[2, 3] = Rectangle34;
            rectangleMatrix[2, 4] = Rectangle35;
            rectangleMatrix[2, 5] = Rectangle36;
            rectangleMatrix[3, 0] = Rectangle41;
            rectangleMatrix[3, 1] = Rectangle42;
            rectangleMatrix[3, 2] = Rectangle43;
            rectangleMatrix[3, 3] = Rectangle44;
            rectangleMatrix[3, 4] = Rectangle45;
            rectangleMatrix[3, 5] = Rectangle46;
            rectangleMatrix[4, 0] = Rectangle51;
            rectangleMatrix[4, 1] = Rectangle52;
            rectangleMatrix[4, 2] = Rectangle53;
            rectangleMatrix[4, 3] = Rectangle54;
            rectangleMatrix[4, 4] = Rectangle55;
            rectangleMatrix[4, 5] = Rectangle56;
            rectangleMatrix[5, 0] = Rectangle61;
            rectangleMatrix[5, 1] = Rectangle62;
            rectangleMatrix[5, 2] = Rectangle63;
            rectangleMatrix[5, 3] = Rectangle64;
            rectangleMatrix[5, 4] = Rectangle65;
            rectangleMatrix[5, 5] = Rectangle66;
        }

        /// <summary>
        /// Initializarea culorilor pentru versiunea de tutorial
        /// </summary>
        private void InitializeImages()
        {
            ImageBrush[] images = new ImageBrush[] { imageBrushFig1, imageBrushFig2, imageBrushFig3, imageBrushFig4, imageBrushFig5, imageBrushFig6 };

            size4Easy.Generate(ref rectangleMatrix, ref blankTilePositions, gameSize, images, 2);
        }

        private void InitializeHappyFaces()
        {
            for (int i = 0; i < gameSize; i++)
            {
                CheckNewHappyLine(i);
                CheckNewHappyColumn(i);
            }
        }

        private void InitializePositionMatrix()
        {
            for (int i = 0; i < gameSize; i++)
            {
                for (int j = 0; j < gameSize; j++)
                {
                    if (rectangleMatrix[i, j].Fill == imageBrushBlank)
                    {
                        rectanglePositionMatrix[i, j] = 0;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig1)
                    {
                        rectanglePositionMatrix[i, j] = 1;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig2)
                    {
                        rectanglePositionMatrix[i, j] = 2;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig3)
                    {
                        rectanglePositionMatrix[i, j] = 3;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig4)
                    {
                        rectanglePositionMatrix[i, j] = 4;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig5)
                    {
                        rectanglePositionMatrix[i, j] = 5;
                    }
                    if (rectangleMatrix[i, j].Fill == imageBrushFig6)
                    {
                        rectanglePositionMatrix[i, j] = 6;
                    }
                }
            }
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

            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
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
            SwapRectanglesIntPositions(i, j, blankTilePositions[areaNumber].i, blankTilePositions[areaNumber].j);
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
                foreach (_4Puzzle.SinglePlayerEasy.Tile tile in blankTilePositions)
                    if (!IsPositionInTheCenter(tile.i) || !IsPositionInTheCenter(tile.j))
                        return false;

                List<int> currentList;

                //check per line
                for (int i = 0; i < gameSize; i++)
                {
                    if (IsPositionInTheCenter(i))
                        continue;
                    currentList = new List<int>();
                    for (int j = 0; j < gameSize; j++)
                        if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                            currentList.Add(rectanglePositionMatrix[i, j]);
                    if (currentList.Count < gameSize)
                        return false;
                }

                //check per column
                for (int j = 0; j < gameSize; j++)
                {
                    if (IsPositionInTheCenter(j))
                        continue;
                    currentList = new List<int>();
                    for (int i = 0; i < gameSize; i++)
                        if (!currentList.Contains(rectanglePositionMatrix[i, j]))
                            currentList.Add(rectanglePositionMatrix[i, j]);
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
            //if (AppSettings.Sound)
            //{
            //    swapSound.Play();
            //}
        }

        private void SwapRectanglesIntPositions(int iFrom, int jFrom, int iTo, int jTo)
        {
            int aux = rectanglePositionMatrix[iFrom, jFrom];
            rectanglePositionMatrix[iFrom, jFrom] = rectanglePositionMatrix[iTo, jTo];
            rectanglePositionMatrix[iTo, jTo] = aux;
            CheckCurrentHappyLinesAndColumns();
            CheckNewHappyLine(iTo);
            CheckNewHappyColumn(jTo);
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
