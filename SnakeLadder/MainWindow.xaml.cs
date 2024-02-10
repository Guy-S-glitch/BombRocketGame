using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SnakeLadder.MainWindow;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace SnakeLadder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<int> Bomb = new List<int>() { 15, 72, 81, 94, 98 };
        public List<int> boost = new List<int>() { 4, 23, 29, 44,63, 71 };
        public int players;
        public List<player> amount = new List<player>();
        public int turn=1;
        public int boot;
        public bool falg=false;
        public int total;
        public bool bom;
        public bool boos;
        public MainWindow()
        {
            
            
            InitializeComponent();
            ColorTable();
            How_much.SelectedIndex = 0;


        }
        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int rolled= random.Next(1, 7);
            dikk(rolled);
            amount[turn - 1].Place += rolled;

            if (amount[turn - 1].Place > 100)
            {
                amount[turn - 1].Place = 100 - (amount[turn - 1].Place - 100);
            }
            else if (amount[turn - 1].Place == 100)
            {
                System.Windows.MessageBox.Show($"player {turn} won");
                Window2 window2 = new Window2();
                this.Visibility = Visibility.Hidden;
                window2.Show();
            }
            total = 0;
            bom = false;
            boos = false;
            do
            {
                falg = false;
                
                foreach (int placeBoost in boost) if (placeBoost == amount[turn - 1].Place)
                    {
                        boot = random.Next(1, 7);
                        total += boot;
                        amount[turn - 1].Place += boot;
                        falg = true;
                        //falg2 = true;
                        bom = true;
                    }

                foreach (int placeBomb in Bomb) if (placeBomb == amount[turn - 1].Place)
                    {
                        boot = random.Next(1, 13);
                        total -= boot;
                        amount[turn - 1].Place -= boot;
                        falg = true;
                        boos = true;
                    }
            } while (falg);

            if (total > 0)
message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. in total\ngain {total} steps" :
                              $"Player {turn} hit boosts.\n in total gain {total} steps";
            else if (total < 0)
message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. in total\nlose {Math.Abs(total)} steps" :
                              $"Player {turn} hit bombs.\n in total lose {Math.Abs(total)} steps";
            else
message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. but stayed\nin his space" :
                               "";
            //MessageBox.Show($"player {turn} is at {amount[turn - 1].Place.ToString()}");


            int Row = amount[turn - 1].Place % 10 != 0 ? 1 + amount[turn - 1].Place / 10 : amount[turn - 1].Place / 10;
            int Colom = Row % 2 != 0 && amount[turn - 1].Place % 10 == 0 ? 10 : Row % 2 == 0 && amount[turn - 1].Place % 10 == 0 ? 1 : Row % 2 == 0 && amount[turn - 1].Place % 10 != 0 ? 11 - (amount[turn - 1].Place % 10) : amount[turn - 1].Place % 10;
            //MessageBox.Show($"player {turn} rolled {rolled} now in row {Row} Colom {Colom} placed {amount[turn-1].Place}");
            GridPP.Children.Remove(amount[turn - 1].TextBlock);
            Grid.SetRow(amount[turn-1].TextBlock, Row);
            Grid.SetColumn(amount[turn - 1].TextBlock, Colom);
            GridPP.Children.Add(amount[turn - 1].TextBlock);
            turn = turn == players ? 1 : turn+1;
            
        }

        public void dikk(int roll)
        {
            switch (roll)
            {
                case 1:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_1.jpg")); break;
                case 2:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_2.jpg")); break;
                case 3:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_3.jpg")); break;
                case 4:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_4.jpg")); break;
                case 5:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_5.jpg")); break;
                default:
                    Imagin.Source = new BitmapImage(new Uri(@"C:\Users\guyso\Downloads\Dice_6.jpg")); break;
            }
        }
        public class player
        {
            public int Id { get; set; }
            public int Place { get; set; }
            public TextBlock TextBlock { get; set; }
            public player(int id, int place , TextBlock text)
            {
                Id =id;
                Place = place;
                TextBlock = text;
                TextBlock.FontSize = 20;
                System.Windows.Controls.Panel.SetZIndex(TextBlock,2);
                TextBlock.Text = Id.ToString();
                TextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                TextBlock.FontWeight =FontWeights.Bold;
            }
            
        }

        private void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (How_much.SelectedIndex!=0)
            {
                players =How_much.SelectedIndex;
                Dice.IsEnabled = true;
                How_much.IsEnabled = false;
                lable.Content = $"{players} players chosen";

                for (int i = 0; i < players; i++) amount.Add(new player(i+1, 0,new TextBlock()));
                int left = 0;
                int bottom = 0;
                for (int padd=0;padd<players; padd++)
                {
                    amount[padd].TextBlock.Padding=new Thickness(left,0,0,bottom);
                    left += 15;
                    left = padd == 2 ? 0:left;
                    bottom = padd == 2 ? bottom + 15 : bottom;
                }
            }
        }
        public int start = 0;
        public byte som;
        public byte som2;
        private void ColorTable()
        {
            while (start < 100)
            {
                som = (byte)(start*2);
                som2 = (byte)(255 - som);
                TextBlock block = new TextBlock();
                block.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, som2, som));
                //block.Background =new SolidColorBrush(Color.FromRgb(128, som2,som));
                start += 1;
                block.FontSize = 8;
                block.TextAlignment = TextAlignment.Left;
                block.Text = start.ToString();

                int Row = start % 10 != 0 ? 1 + start / 10 : start / 10;
                int Colom = Row % 2 != 0 && start % 10 == 0 ? 10 : Row % 2 == 0 && start % 10 == 0 ? 1 : Row % 2 == 0 && start % 10 != 0 ? 11 - (start % 10) : start % 10;

                Grid.SetRow(block, Row);
                Grid.SetColumn(block, Colom);
                GridPP.Children.Add(block);
            }

        }
    }
}
