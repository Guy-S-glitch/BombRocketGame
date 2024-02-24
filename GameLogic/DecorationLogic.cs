using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Reflection.Emit;
using System.Data.Common;

namespace GameLogic
{
    public class Decoration
    {
        public void DiceOutput(ref short roll,ref Image Imagin)   //showing suitable picture according to the player's roll
        {
            switch (roll)
            {
                case 1:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_1.jpg", UriKind.Relative)); break;
                case 2:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_2.jpg", UriKind.Relative)); break;
                case 3:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_3.jpg", UriKind.Relative)); break;
                case 4:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_4.jpg", UriKind.Relative)); break;
                case 5:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_5.jpg", UriKind.Relative)); break;
                default:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_6.jpg", UriKind.Relative)); break;
            }
        }
        public void ColorTable(Grid GridPP)   //since we have the movement path of the game, it's possible to set the movement to 1 space and fill each space with decoration
        {
            for (short start=1;start<=100;start++)
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();   //make rainbow color
                populateLinearBrush(ref myLinearGradientBrush);
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                populateLabel(ref label, start, myLinearGradientBrush);
                sendToGrid(start, label, ref GridPP);
            }
        }
       private void populateLinearBrush(ref LinearGradientBrush myLinearGradientBrush)
        {
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(1, 1);
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.Yellow, 0.0));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.Red, 0.25));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.Blue, 0.75));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.LimeGreen, 1.0));
        }

        private void populateLabel(ref System.Windows.Controls.Label label,short start, LinearGradientBrush myLinearGradientBrush)
        {
            label.Background = start % 2 == 0 ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(175, 191, 29)) : new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 153, 81));   //we'll have 2 colors fill the table, every space the color will change
            label.BorderThickness = new Thickness(1);
            label.BorderBrush = myLinearGradientBrush;   //paint the border with the colors we set
            label.Content = start;   //every space will have it's own number
            label.FontSize = 18;
            label.HorizontalContentAlignment = HorizontalAlignment.Left;
            label.VerticalContentAlignment = VerticalAlignment.Top;
            label.Foreground = new SolidColorBrush(Colors.GhostWhite);
        }
        private void sendToGrid(short start, System.Windows.Controls.Label label,ref Grid GridPP)
        {

            short _row = (short)(start % 10 != 0 ? 1 + start / 10 : start / 10);
            short _column = (short)(_row % 2 != 0 && start % 10 == 0 ? 10 : _row % 2 == 0 && start % 10 == 0 ? 1 : _row % 2 == 0 && start % 10 != 0 ? 11 - (start % 10) : start % 10);
            Grid.SetRow(label, _row);
            Grid.SetColumn(label, _column);
            GridPP.Children.Add(label);
        }
    }
}
