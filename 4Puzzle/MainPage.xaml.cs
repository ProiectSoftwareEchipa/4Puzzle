﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace _4Puzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Members

        private SolidColorBrush solidColorBrushYellow;

        private SolidColorBrush solidColorBrushRed;

        private SolidColorBrush solidColorBrushBlue;

        private SolidColorBrush solidColorBrushPurple;

        private SolidColorBrush solidColorBrushWhite;

        private Rectangle[,] rectangleMatrix;
        
        private struct Tile {
            public int i;
            public int j;
        }

        private int GameSize;

        private Tile[] WhiteTilePositions = new Tile[4];

        #endregion Private Members

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.rectangleMatrix = new Rectangle[4, 4];

            this.solidColorBrushYellow = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

            this.solidColorBrushRed = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));

            this.solidColorBrushBlue = new SolidColorBrush(Color.FromArgb(120, 0, 0, 255));

            this.solidColorBrushPurple = new SolidColorBrush(Color.FromArgb(255, 125, 0, 255));

            this.solidColorBrushWhite = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

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
        }

        #endregion Overrides

        #region Event Handlers

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

            if(CheckEndGame)
            {
                validationBlock.Text = "Victory!";
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
            rectangleMatrix[0, 0].Fill = solidColorBrushWhite;
            rectangleMatrix[0, 1].Fill = solidColorBrushRed;
            rectangleMatrix[0, 2].Fill = solidColorBrushBlue;
            rectangleMatrix[0, 3].Fill = solidColorBrushRed;
            rectangleMatrix[1, 0].Fill = solidColorBrushYellow;
            rectangleMatrix[1, 1].Fill = solidColorBrushPurple;
            rectangleMatrix[1, 2].Fill = solidColorBrushBlue;
            rectangleMatrix[1, 3].Fill = solidColorBrushWhite;
            rectangleMatrix[2, 0].Fill = solidColorBrushRed;
            rectangleMatrix[2, 1].Fill = solidColorBrushYellow;
            rectangleMatrix[2, 2].Fill = solidColorBrushYellow;
            rectangleMatrix[2, 3].Fill = solidColorBrushPurple;
            rectangleMatrix[3, 0].Fill = solidColorBrushWhite;
            rectangleMatrix[3, 1].Fill = solidColorBrushBlue;
            rectangleMatrix[3, 2].Fill = solidColorBrushWhite;
            rectangleMatrix[3, 3].Fill = solidColorBrushBlue;
            WhiteTilePositions[0].i = 0;
            WhiteTilePositions[0].j = 0;
            WhiteTilePositions[1].i = 1;
            WhiteTilePositions[1].j = 3;
            WhiteTilePositions[2].i = 3;
            WhiteTilePositions[2].j = 0;
            WhiteTilePositions[3].i = 3;
            WhiteTilePositions[3].j = 2;
            GameSize = 3;
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
                for(int j = 0; j <= 3; j++)
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
        private void CheckNeighbours(Tuple<int, int> rectangleIndex) {
            int i = rectangleIndex.Item1;
            int j = rectangleIndex.Item2;

            if (!IsNearWhiteTile(i, j))
                return;

            int areaNumber = GetSelectedAreaNumber(i, j);

            SwapRectanglesColors(rectangleMatrix[i, j], rectangleMatrix[WhiteTilePositions[areaNumber].i, WhiteTilePositions[areaNumber].j]);
            WhiteTilePositions[areaNumber].i = i;
            WhiteTilePositions[areaNumber].j = j;

        }

        /// <summary>
        /// Proprietatea ce verifica daca sa ajuns in situatia de sfarsit a jocului
        /// </summary>
        private bool CheckEndGame
        {
            get
            {
                for (int i = 0; i <= 2; i++)
                {
                    for (int j = i + 1; j <= 3; j++)
                    {
                        if (rectangleMatrix[0, i].Fill == solidColorBrushWhite || rectangleMatrix[0, j].Fill == solidColorBrushWhite
                           || rectangleMatrix[3, i].Fill == solidColorBrushWhite || rectangleMatrix[3, j].Fill == solidColorBrushWhite
                           || rectangleMatrix[i, 0].Fill == solidColorBrushWhite || rectangleMatrix[j, 0].Fill == solidColorBrushWhite
                           || rectangleMatrix[i, 3].Fill == solidColorBrushWhite || rectangleMatrix[j, 3].Fill == solidColorBrushWhite)
                            return false;

                        if ((rectangleMatrix[0, i].Fill == rectangleMatrix[0, j].Fill) || (rectangleMatrix[3, i].Fill == rectangleMatrix[3, j].Fill)
                            || (rectangleMatrix[i, 0].Fill == rectangleMatrix[j, 0].Fill) || (rectangleMatrix[i, 3].Fill == rectangleMatrix[j, 3].Fill))
                            return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Metoda ce inverseaza culorile intre 2 rectangle-uri
        /// </summary>
        /// <param name="colorRectangle">Rectangle-ul colorat</param>
        /// <param name="whiteRectangle">Rectangle-ul alb</param>
        private void SwapRectanglesColors(Rectangle colorRectangle, Rectangle whiteRectangle)
        {
            whiteRectangle.Fill = colorRectangle.Fill;
            colorRectangle.Fill = solidColorBrushWhite;
        }

        private int GetSelectedAreaNumber(int i, int j) {

            if (i <= GameSize / 2 && j <= GameSize / 2)
                return 0;

            if (i <= GameSize / 2 && j > GameSize / 2)
                return 1;

            if (i > GameSize / 2 && j <= GameSize / 2)
                return 2;

            return 3;
        }

        private bool IsNearWhiteTile(int i, int j) {

            int areaNumber = GetSelectedAreaNumber(i,j);

            if (Math.Abs(WhiteTilePositions[areaNumber].i - i) + Math.Abs(WhiteTilePositions[areaNumber].j - j) == 1)
                return true;

            return false;
        }

        #endregion Private Methods
    }
}
