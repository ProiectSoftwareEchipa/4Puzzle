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

        public static void Generate(ref Rectangle[,] puzzle, ref _4Puzzle.Tutorial.Tile[] whiteTilePositions) {
            SolidColorBrush[] colorsWithoutWhite = new SolidColorBrush[] { new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)), 
                new SolidColorBrush(Color.FromArgb(120, 255, 0, 0)), 
                new SolidColorBrush(Color.FromArgb(120, 0, 0, 255)), 
                new SolidColorBrush(Color.FromArgb(255, 125, 0, 255)) 
            };

            //get a random areaNumber
            var rand = new Random();
            int randomNumber = rand.Next(1, 4);
            colorsWithoutWhite.Shuffle(rand);
            SolidColorBrush[] colors = new SolidColorBrush[] { colorsWithoutWhite[0], 
                colorsWithoutWhite[1], 
                colorsWithoutWhite[2], 
                colorsWithoutWhite[3], 
            new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
            };

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

            for (int i = 0; i < randomNumber; i++)
                RotateClockWise(ref puzzle);

            RecalculateWhiteTilePositions(ref whiteTilePositions, puzzle);
        }

        static void RotateClockWise(ref Rectangle[,] puzzle) {
            Brush[,] puzzleCopy = new Brush[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    puzzleCopy[i, j] = puzzle[i, j].Fill;
            //1 over 2
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    puzzle[i, j + 2].Fill = puzzleCopy[i, j];
            //4 over 3
            for (int i = 2; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    puzzle[i, j].Fill = puzzleCopy[i, j + 2];
            //3 over 1
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    puzzle[i, j].Fill = puzzleCopy[i + 2, j];
            //2 over 4
            for (int i = 0; i < 2; i++)
                for (int j = 2; j < 4; j++)
                    puzzle[i + 2, j].Fill = puzzleCopy[i, j];
        }
        static void RecalculateWhiteTilePositions(ref _4Puzzle.Tutorial.Tile[] whiteTilePositions, Rectangle[,] puzzle) {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color.ToString() == "#FFFFFFFF") {
                        whiteTilePositions[0].i = i;
                        whiteTilePositions[0].j = j;
                        break;
                    }

            for (int i = 0; i < 2; i++)
                for (int j = 2; j < 4; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color.ToString() == "#FFFFFFFF") {
                        whiteTilePositions[1].i = i;
                        whiteTilePositions[1].j = j;
                        break;
                    }

            for (int i = 2; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color.ToString() == "#FFFFFFFF") {
                        whiteTilePositions[2].i = i;
                        whiteTilePositions[2].j = j;
                        break;
                    }

            for (int i = 2; i < 4; i++)
                for (int j = 2; j < 4; j++)
                    if (((SolidColorBrush)puzzle[i, j].Fill).Color.ToString() == "#FFFFFFFF") {
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
    }
}
