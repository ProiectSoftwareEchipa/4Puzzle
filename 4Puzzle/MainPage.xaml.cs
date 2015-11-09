using System;
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
        private SolidColorBrush solidColorBrushYellow = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

        private SolidColorBrush solidColorBrushRed = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));

        private SolidColorBrush solidColorBrushBlue = new SolidColorBrush(Color.FromArgb(120, 0, 0, 255));

        private SolidColorBrush solidColorBrushPurple = new SolidColorBrush(Color.FromArgb(255, 125, 0, 255));

        private SolidColorBrush solidColorBrushWhite = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        private Rectangle[,] rectangleMatrix = new Rectangle[4, 4];

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            InitializeMatrix();

            InitializeTutorialColors();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            Tuple<int, int> rectangleIndex = GetRectangleIndex(rectangle);
            Random random = new Random();

            if(rectangle.Fill == solidColorBrushWhite)
            {
                return;
            }

            rectangleMatrix[rectangleIndex.Item1, rectangleIndex.Item2].Fill = new SolidColorBrush(Color.FromArgb((byte)random.Next(0,255), (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
        }

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
            rectangleMatrix[2, 2].Fill = solidColorBrushBlue;
            rectangleMatrix[2, 3].Fill = solidColorBrushPurple;
            rectangleMatrix[3, 0].Fill = solidColorBrushWhite;
            rectangleMatrix[3, 1].Fill = solidColorBrushBlue;
            rectangleMatrix[3, 2].Fill = solidColorBrushYellow;
            rectangleMatrix[3, 3].Fill = solidColorBrushWhite;
        }

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
    }
}
