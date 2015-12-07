using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace _4Puzzle.Generators {
    static class size4Easy {

        class square {
            public int areaId;
            public List<SolidColorBrush> list;

            public square(int id, List<SolidColorBrush> l) {
                areaId = id;
                list = l;
            }
        }

        public static void Generate(ref Rectangle[,] puzzle, ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, int gameSize, SolidColorBrush[] receivedColors, int initialSetupNumber) {
            Random rand = new Random();

            SolidColorBrush[] colors = GetColors(rand, receivedColors);

            InitialSetup(initialSetupNumber,puzzle, colors);

            int randomNumber = rand.Next(1, 4);
            for (int i = 0; i < randomNumber; i++)
                RotateClockWise(ref puzzle, gameSize);

            RecalculateWhiteTilePositions(ref whiteTilePositions, puzzle, gameSize);

            Shamble(ref whiteTilePositions, puzzle, gameSize, rand);
        }

        static void RotateClockWise(ref Rectangle[,] puzzle, int gameSize) {
            Brush[,] puzzleCopy = new Brush[gameSize, gameSize];
            for (int i = 0; i < gameSize; i++)
                for (int j = 0; j < gameSize; j++)
                    puzzleCopy[i, j] = puzzle[i, j].Fill;
            //1 over 2
            for (int i = 0; i < gameSize / 2; i++)
                for (int j = 0; j < gameSize / 2; j++)
                    puzzle[i, j + gameSize / 2].Fill = puzzleCopy[i, j];
            //4 over 3
            for (int i = gameSize / 2; i < gameSize; i++)
                for (int j = 0; j < gameSize / 2; j++)
                    puzzle[i, j].Fill = puzzleCopy[i, j + gameSize / 2];
            //3 over 1
            for (int i = 0; i < gameSize / 2; i++)
                for (int j = 0; j < gameSize / 2; j++)
                    puzzle[i, j].Fill = puzzleCopy[i + gameSize / 2, j];
            //2 over 4
            for (int i = 0; i < gameSize / 2; i++)
                for (int j = gameSize / 2; j < gameSize; j++)
                    puzzle[i + gameSize / 2, j].Fill = puzzleCopy[i, j];
        }
        static void RecalculateWhiteTilePositions(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize) {

            SolidColorBrush solidColorBrushBlank = new SolidColorBrush(Color.FromArgb(255, 101, 67, 33)); ;

            for (int i = 0; i < gameSize / 2; i++)
                for (int j = 0; j < gameSize / 2; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color == solidColorBrushBlank.Color) {
                        whiteTilePositions[0].i = i;
                        whiteTilePositions[0].j = j;
                        break;
                    }

            for (int i = 0; i < gameSize / 2; i++)
                for (int j = gameSize / 2; j < gameSize; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color == solidColorBrushBlank.Color) {
                        whiteTilePositions[1].i = i;
                        whiteTilePositions[1].j = j;
                        break;
                    }

            for (int i = gameSize / 2; i < gameSize; i++)
                for (int j = 0; j < gameSize / 2; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color == solidColorBrushBlank.Color) {
                        whiteTilePositions[2].i = i;
                        whiteTilePositions[2].j = j;
                        break;
                    }

            for (int i = gameSize / 2; i < gameSize; i++)
                for (int j = gameSize / 2; j < gameSize; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color == solidColorBrushBlank.Color) {
                        whiteTilePositions[3].i = i;
                        whiteTilePositions[3].j = j;
                        break;
                    }
        }
        public static void Shuffle<T>(this IList<T> list, Random rnd) {
            int n = list.Count;
            while (n > 1) {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        static void Shamble(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize, Random rand) {
            int direction, whiteI, whiteJ;
            for (int areaNumber = 1; areaNumber <= 4; areaNumber++)
                for (int i = 0; i <= gameSize * 2; i++) {
                    while (true) {
                        direction = rand.Next(1, 5);
                        whiteI = whiteTilePositions[areaNumber - 1].i;
                        whiteJ = whiteTilePositions[areaNumber - 1].j;
                        if (direction == 1 && swapUp(ref whiteTilePositions, puzzle, gameSize, areaNumber, whiteI, whiteJ)) break;
                        if (direction == 2 && swapDown(ref whiteTilePositions, puzzle, gameSize, areaNumber, whiteI, whiteJ)) break;
                        if (direction == 3 && swapLeft(ref whiteTilePositions, puzzle, gameSize, areaNumber, whiteI, whiteJ)) break;
                        if (direction == 4 && swapRight(ref whiteTilePositions, puzzle, gameSize, areaNumber, whiteI, whiteJ)) break;
                    }
                }
        }
        public static bool swapUp(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize, int areaNumber, int whiteI, int whiteJ) {
            if (!isInSpecifiedArea(areaNumber, gameSize, whiteI, whiteJ) || !canSwapUp(areaNumber, gameSize, whiteI, whiteJ))
                return false;

            Brush fill = puzzle[whiteI, whiteJ].Fill;
            puzzle[whiteI, whiteJ].Fill = puzzle[whiteI - 1, whiteJ].Fill;
            puzzle[whiteI - 1, whiteJ].Fill = fill;

            whiteTilePositions[areaNumber - 1].i = whiteTilePositions[areaNumber - 1].i - 1;

            return true;
        }
        public static bool swapDown(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize, int areaNumber, int whiteI, int whiteJ) {
            if (!isInSpecifiedArea(areaNumber, gameSize, whiteI, whiteJ) || !canSwapDown(areaNumber, gameSize, whiteI, whiteJ))
                return false;

            Brush fill = puzzle[whiteI, whiteJ].Fill;
            puzzle[whiteI, whiteJ].Fill = puzzle[whiteI + 1, whiteJ].Fill;
            puzzle[whiteI + 1, whiteJ].Fill = fill;

            whiteTilePositions[areaNumber - 1].i = whiteTilePositions[areaNumber - 1].i + 1;

            return true;
        }
        public static bool swapLeft(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize, int areaNumber, int whiteI, int whiteJ) {
            if (!isInSpecifiedArea(areaNumber, gameSize, whiteI, whiteJ) || !canSwapLeft(areaNumber, gameSize, whiteI, whiteJ))
                return false;

            Brush fill = puzzle[whiteI, whiteJ].Fill;
            puzzle[whiteI, whiteJ].Fill = puzzle[whiteI, whiteJ - 1].Fill;
            puzzle[whiteI, whiteJ - 1].Fill = fill;

            whiteTilePositions[areaNumber - 1].j = whiteTilePositions[areaNumber - 1].j - 1;

            return true;
        }
        public static bool swapRight(ref _4Puzzle.SinglePlayerEasy.Tile[] whiteTilePositions, Rectangle[,] puzzle, int gameSize, int areaNumber, int whiteI, int whiteJ) {
            if (!isInSpecifiedArea(areaNumber, gameSize, whiteI, whiteJ) || !canSwapRight(areaNumber, gameSize, whiteI, whiteJ))
                return false;

            Brush fill = puzzle[whiteI, whiteJ].Fill;
            puzzle[whiteI, whiteJ].Fill = puzzle[whiteI, whiteJ + 1].Fill;
            puzzle[whiteI, whiteJ + 1].Fill = fill;

            whiteTilePositions[areaNumber - 1].j = whiteTilePositions[areaNumber - 1].j + 1;

            return true;
        }
        public static bool isInSpecifiedArea(int areaNumber, int gameSize, int i, int j) {
            if (areaNumber == 1)
                return i < gameSize / 2 && j < gameSize / 2;
            if (areaNumber == 2)
                return i < gameSize / 2 && j >= gameSize / 2;
            if (areaNumber == 3)
                return i >= gameSize / 2 && j < gameSize / 2;
            if (areaNumber == 4)
                return i >= gameSize / 2 && j >= gameSize / 2;
            return false;
        }
        public static bool canSwapUp(int areaNumber, int gameSize, int i, int j) {
            if (areaNumber < 3 && i > 0)
                return true;
            if (areaNumber >= 3 && i > gameSize / 2)
                return true;
            return false;
        }
        public static bool canSwapDown(int areaNumber, int gameSize, int i, int j) {
            if (areaNumber < 3 && i < gameSize / 2 - 1)
                return true;
            if (areaNumber >= 3 && i < gameSize - 1)
                return true;
            return false;
        }
        public static bool canSwapLeft(int areaNumber, int gameSize, int i, int j) {
            if (areaNumber % 2 == 1 && j > 0)
                return true;
            if (areaNumber % 2 != 1 && j > gameSize / 2)
                return true;
            return false;
        }
        public static bool canSwapRight(int areaNumber, int gameSize, int i, int j) {
            if (areaNumber % 2 == 1 && j < gameSize / 2 - 1)
                return true;
            if (areaNumber % 2 != 1 && j < gameSize - 1)
                return true;
            return false;
        }

        public static SolidColorBrush[] GetColors(Random rand, SolidColorBrush[] receivedColors) {
            //get a random areaNumber
            receivedColors.Shuffle(rand);
            SolidColorBrush[] colors = new SolidColorBrush[receivedColors.Count() + 1];
            for (int i = 0; i < receivedColors.Count(); i++)
                colors[i] = receivedColors[i];
            colors[receivedColors.Count()] = new SolidColorBrush(Color.FromArgb(255, 101, 67, 33));
            return colors;
        }

        public static void InitialSetup(int initialSetupNumber, Rectangle[,] puzzle, SolidColorBrush[] colors) {
            switch (initialSetupNumber) {
                case 1:
                    InitialSetupEasy1(puzzle, colors);
                    break;
                case 2:
                    InitialSetupMedium1(puzzle, colors);
                    break;
                default:
                    break;
            }
        }
        public static void InitialSetupEasy1(Rectangle[,] puzzle, SolidColorBrush[] colors) {
            puzzle[0, 0].Fill = colors[4];
            puzzle[0, 1].Fill = colors[1];
            puzzle[0, 2].Fill = colors[2];
            puzzle[0, 3].Fill = colors[1];
            puzzle[1, 0].Fill = colors[0];
            puzzle[1, 1].Fill = colors[3];
            puzzle[1, 2].Fill = colors[2];
            puzzle[1, 3].Fill = colors[4];
            puzzle[2, 0].Fill = colors[1];
            puzzle[2, 1].Fill = colors[0];
            puzzle[2, 2].Fill = colors[0];
            puzzle[2, 3].Fill = colors[3];
            puzzle[3, 0].Fill = colors[4];
            puzzle[3, 1].Fill = colors[2];
            puzzle[3, 2].Fill = colors[4];
            puzzle[3, 3].Fill = colors[2];
        }
        public static void InitialSetupMedium1(Rectangle[,] puzzle, SolidColorBrush[] colors) {
            puzzle[0, 0].Fill = colors[0];
            puzzle[0, 1].Fill = colors[1];
            puzzle[0, 2].Fill = colors[2];
            puzzle[0, 3].Fill = colors[3];
            puzzle[0, 4].Fill = colors[4];
            puzzle[0, 5].Fill = colors[5];
            puzzle[1, 0].Fill = colors[5];
            puzzle[1, 1].Fill = colors[0];
            puzzle[1, 2].Fill = colors[1];
            puzzle[1, 3].Fill = colors[2];
            puzzle[1, 4].Fill = colors[3];
            puzzle[1, 5].Fill = colors[4];
            puzzle[2, 0].Fill = colors[4];
            puzzle[2, 1].Fill = colors[5];
            puzzle[2, 2].Fill = colors[6];
            puzzle[2, 2].StrokeThickness = 0;
            puzzle[2, 3].Fill = colors[2];
            puzzle[2, 4].Fill = colors[6];
            puzzle[2, 4].StrokeThickness = 0;
            puzzle[2, 5].Fill = colors[3];
            puzzle[3, 0].Fill = colors[3];
            puzzle[3, 1].Fill = colors[4];
            puzzle[3, 2].Fill = colors[6];
            puzzle[3, 2].StrokeThickness = 0;
            puzzle[3, 3].Fill = colors[6];
            puzzle[3, 3].StrokeThickness = 0;
            puzzle[3, 4].Fill = colors[1];
            puzzle[3, 5].Fill = colors[2];
            puzzle[4, 0].Fill = colors[2];
            puzzle[4, 1].Fill = colors[3];
            puzzle[4, 2].Fill = colors[4];
            puzzle[4, 3].Fill = colors[5];
            puzzle[4, 4].Fill = colors[0];
            puzzle[4, 5].Fill = colors[1];
            puzzle[5, 0].Fill = colors[1];
            puzzle[5, 1].Fill = colors[2];
            puzzle[5, 2].Fill = colors[3];
            puzzle[5, 3].Fill = colors[4];
            puzzle[5, 4].Fill = colors[5];
            puzzle[5, 5].Fill = colors[0];
        }
    }
}
