using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using Connect4;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;

namespace Connect4Game.Game_Resources.Graphics_Manager
{
    public static class GraphicsManager
    {
        //Arreglo de imagenes que se usan como background.
        public static ImageBrush[] myBrushes = new ImageBrush[6];


        public static void Paint(Button button)
        {
            button.Background = myBrushes[0];
        }
        public static void LoadImages(MainWindow window)
        {

            for (int i = 0; i < myBrushes.Length; i++)
            {
                var brush = new ImageBrush();
                FileInfo f = new FileInfo($@"Resources\Images\Cell_{i}.jpg");
                //brush.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(f.FullName);
                myBrushes[i] = new ImageBrush() { ImageSource = new BitmapImage(new Uri($"{f.FullName}", UriKind.RelativeOrAbsolute)) };
            }

            window.GameWindow.Background = myBrushes[3];
        }

        public static void SwitchVisibility(MainWindow window)
        {
            //Cambia la visibilidad de cada elemento en "MainGrid" (Lo muestra|Lo esconde).
            foreach (UIElement mainGridChild in window.MainGrid.Children)
            {
                mainGridChild.Visibility = (mainGridChild.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            }
            window.Width = (window.Width == 400) ? 800 : 400;
        }

        public static void InitButtons(MainWindow window)
        {
            //Se asigna una imagen de boton.
            window.SinglePlayer.Background = myBrushes[5];
            window.MultiPlayer.Background = myBrushes[5];
            window.SmallGrid.Background = myBrushes[5];
            window.MediumGrid.Background = myBrushes[5];
            window.LargeGrid.Background = myBrushes[5];
            window.Close.Background = myBrushes[5];
        }

        public static void PaintButtonOrange(Button button)
        {
            button.Background = Brushes.DarkOrange;
        }

        public static void PaintButtonGold(Button button)
        {
            button.Background = myBrushes[4];
        }

        public static void PaintCellRed(Button button)
        {
            button.Background = myBrushes[1];
        }

        public static void PaintCellBlue(Button button)
        {
            button.Background = myBrushes[2];
        }

        public static void BackgroundRed(Button button)
        {
            button.Background = Brushes.Red;
        }

        public static void BackgroundBlue(Button button)
        {
            button.Background = Brushes.Blue;
        }

        public static void BackgroundGold(Button button)
        {
            button.Background = Brushes.Goldenrod;
        }

        public static class Animations
        {
            public static async Task FlashLabel(Button button, bool playerTurn)
            {
                Brush temp = button.Background;
                button.Background = playerTurn ? myBrushes[1] : myBrushes[2];
                await Task.Delay(300);
                
                button.Background = temp;
            }
        }

    }
}
