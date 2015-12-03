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

        private SolidColorBrush solidColorBrushYellow;

        private SolidColorBrush solidColorBrushRed;

        private SolidColorBrush solidColorBrushBlue;

        private SolidColorBrush solidColorBrushPurple;

        private SolidColorBrush solidColorBrushBlank;

        private Rectangle[,] rectangleMatrix;

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

            this.solidColorBrushYellow = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

            this.solidColorBrushRed = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));

            this.solidColorBrushBlue = new SolidColorBrush(Color.FromArgb(120, 0, 0, 255));

            this.solidColorBrushPurple = new SolidColorBrush(Color.FromArgb(255, 125, 0, 255));

            this.solidColorBrushBlank = new SolidColorBrush(Color.FromArgb(255, 101, 67, 33));

            this.blankTilePositions = new Tile[4];

            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            InitializeMatrix();

            InitializeTutorialColors();
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

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
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
                validationBlock.Text = "Victory!";
                StopGame();
            }
            else
            {
                validationBlock.Text = String.Empty;
            }
        }

        #endregion Event Handlers

        #region Private Methods

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
        private void InitializeTutorialColors()
        {
           /* rectangleMatrix[0, 0].Fill = solidColorBrushBlank;
            rectangleMatrix[0, 1].Fill = solidColorBrushRed;
            rectangleMatrix[0, 2].Fill = solidColorBrushBlue;
            rectangleMatrix[0, 3].Fill = solidColorBrushRed;
            rectangleMatrix[1, 0].Fill = solidColorBrushYellow;
            rectangleMatrix[1, 1].Fill = solidColorBrushPurple;
            rectangleMatrix[1, 2].Fill = solidColorBrushBlue;
            rectangleMatrix[1, 3].Fill = solidColorBrushBlank;
            rectangleMatrix[2, 0].Fill = solidColorBrushRed;
            rectangleMatrix[2, 1].Fill = solidColorBrushYellow;
            rectangleMatrix[2, 2].Fill = solidColorBrushYellow;
            rectangleMatrix[2, 3].Fill = solidColorBrushPurple;
            rectangleMatrix[3, 0].Fill = solidColorBrushBlank;
            rectangleMatrix[3, 1].Fill = solidColorBrushBlue;
            rectangleMatrix[3, 2].Fill = solidColorBrushBlank;
            rectangleMatrix[3, 3].Fill = solidColorBrushBlue;
            blankTilePositions[0].i = 0;
            blankTilePositions[0].j = 0;
            blankTilePositions[1].i = 1;
            blankTilePositions[1].j = 3;
            blankTilePositions[2].i = 3;
            blankTilePositions[2].j = 0;
            blankTilePositions[3].i = 3;
            blankTilePositions[3].j = 2;*/
            size4Easy.Generate(ref rectangleMatrix, ref blankTilePositions);
            for(int i = 0; i < 4; i++)
            {
                rectangleMatrix[blankTilePositions[i].i, blankTilePositions[i].j].StrokeThickness = 0;
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

            SwapRectanglesColors(rectangleMatrix[i, j], rectangleMatrix[blankTilePositions[areaNumber].i, blankTilePositions[areaNumber].j]);
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
        /// <param name="colorRectangle">Rectangle-ul colorat</param>
        /// <param name="whiteRectangle">Rectangle-ul alb</param>
        private void SwapRectanglesColors(Rectangle colorRectangle, Rectangle blankRectangle)
        {
            blankRectangle.Fill = colorRectangle.Fill;
            blankRectangle.StrokeThickness = 2;
            colorRectangle.Fill = solidColorBrushBlank;
            colorRectangle.StrokeThickness = 0;
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
