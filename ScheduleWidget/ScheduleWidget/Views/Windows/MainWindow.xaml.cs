using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Bitlush;
using ScheduleWidget.Models;
using ScheduleWidget.ViewModels;

namespace ScheduleWidget
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AvlTree<int, BlockButton> tree;
        private int CountRows { get; set; }
        private int CountElements { get; set; }
        List<Tuple<int, int, int>> NumberForButtons;
        public MainWindow()
        {
            InitializeComponent();
            Data.Content = DateTime.Now.ToString("MMM dd ddd");
            CountRows = Convert.ToInt32(Table.ActualHeight) / 40;
            CountElements = 0;
        }

        private void slide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double xValRatio = slide.Value / (slide.Maximum - slide.Minimum); //gets % the thumb is across the slider
            double thumbPos = xValRatio * (slide.ActualWidth); //gets the absolute position
            line.Visibility = Visibility.Visible;
            triangle.Visibility = Visibility.Visible;
            PointCollection points = new PointCollection();
            points.Add(new Point(thumbPos - 2, 11));
            points.Add(new Point(thumbPos - 2, Scroll.ViewportHeight-1));
            points.Add(new Point(thumbPos + 2, Scroll.ViewportHeight-1));
            points.Add(new Point(thumbPos + 2, 11));
            line.Points = points;
            points = new PointCollection();
            points.Add(new Point(thumbPos - 10, 1));
            points.Add(new Point(thumbPos, 13));
            points.Add(new Point(thumbPos + 10, 1));
            triangle.Points = points;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sliderPanel.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            panel.Margin = new Thickness(e.HorizontalOffset, e.VerticalOffset, 0, -e.VerticalOffset);
            line.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            triangle.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
        }

        async private void GenerateBut_Click(object sender, RoutedEventArgs e)
        {
            SelectWindow select = new SelectWindow();
            select.Owner = this;
            if (select.ShowDialog() == true)
            {
                CountElements = select.countElements;
                CountRows = select.countRows;
                Table.Height = Convert.ToDouble(CountRows * 40);
            }
            else
                return;
            await Task.Run(() =>
            {
                GeneratorNumbers(Table.ActualWidth);
            });
            CreateButtons();
        }
        private void GeneratorNumbers(double Width)
        {
            int count = CountElements;
            Console.WriteLine("Count need: " + count);
            List<Tuple<int, int, int>> numbers = new List<Tuple<int, int, int>>();
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            while (count != 0)
            {
                
                bool isNew = true;
                int startTime = rand.Next(0, Convert.ToInt32(Width - 10));
                int endTime = rand.Next(10, 400)+startTime;
                if (endTime > Width)
                    endTime = Convert.ToInt32(Width) - 2;
                int row = rand.Next(0, CountRows) * 40;
                if (numbers.Count == 0)
                    numbers.Add(new Tuple<int, int, int>(startTime, endTime, row));
                else
                {
                    foreach (var tuple in numbers)
                    {
                        if (startTime <= tuple.Item1 && tuple.Item3 == row)
                        {
                            for (int i = startTime; i <= endTime; i++)
                            {
                                if (i == tuple.Item1)
                                {
                                    isNew = false;
                                    break;
                                }
                            }
                        }
                        else if (tuple.Item1 <= startTime && tuple.Item3 == row)
                        {
                            for (int i = tuple.Item1; i <= tuple.Item2; i++)
                            {
                                if (i == startTime)
                                {
                                    isNew = false;
                                    break;
                                }
                            }
                        }

                    }
                }
                if (isNew)
                    numbers.Add(new Tuple<int, int, int>(startTime, endTime, row));
                count--;
            }
            Console.WriteLine("Count real: " + numbers.Count);
            NumberForButtons = numbers;
        }

        private void CreateButtons()
        {
            //Сделать крч проверку что если объектов нет, то создаем их новые, если есть, то ходим и заменяем значения
            //если сгенерированных больше, то добавляем новые элементы
            //если сгенерированных меньше - удаляем лишние
            //на словах вроде просто
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            if (Table.Children.Count == 0)
            {
                int treeIndex = 0;
                tree = new AvlTree<int, BlockButton>();
                foreach (var data in NumberForButtons)
                {
                    BlockButton button = new BlockButton()
                    {
                        Content = rand.Next(0, 5000),
                        Height = 40,
                        Opacity = 0.7d,
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        StartTime = data.Item1,
                        Width = data.Item2 - data.Item1,
                        Background = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256))),
                        EndTime = data.Item2,
                        Row = data.Item3
                    };
                    button.Click += Button_Click1;
                    tree.Insert(treeIndex, button);
                    Canvas.SetTop(tree.Last().Value, data.Item3);
                    Canvas.SetLeft(tree.Last().Value, data.Item1);
                    Table.Children.Add(tree.Last().Value);
                    treeIndex++;
                }
            }
            else
            {
                Table.Children.Clear();
                int countExistElem = tree.Count();
                int countNewElem = NumberForButtons.Count;
                int difference = countNewElem - countExistElem <=0 ? 0 : countNewElem - countExistElem;
                if (countExistElem < countNewElem)
                {
                    while(countExistElem != countNewElem)
                    { 
                        tree.Insert(countExistElem, new BlockButton());
                        countExistElem++;
                    }
                }
                else if(countExistElem > countNewElem)
                {
                    while(countExistElem != countNewElem)
                    {
                        countExistElem--;
                        tree.Delete(countExistElem);
                    }
                }
                    for (int i = 0; i < tree.Count(); i++)
                    {
                        tree.ElementAt(i).Value.Content = rand.Next(0, 5000);
                        tree.ElementAt(i).Value.StartTime = NumberForButtons[i].Item1;
                        tree.ElementAt(i).Value.Width = NumberForButtons[i].Item2 - NumberForButtons[i].Item1;
                        tree.ElementAt(i).Value.EndTime = NumberForButtons[i].Item2;
                        tree.ElementAt(i).Value.Row = NumberForButtons[i].Item3;
                        tree.ElementAt(i).Value.Height = 40;
                        tree.ElementAt(i).Value.Opacity = 0.7d;
                        tree.ElementAt(i).Value.BorderThickness = new Thickness(1, 1, 1, 1);
                        tree.ElementAt(i).Value.EndTime = NumberForButtons[i].Item2;
                        tree.ElementAt(i).Value.Row = NumberForButtons[i].Item3;
                        if (tree.Count() - difference == i && difference != 0)
                        {
                            tree.ElementAt(i).Value.Click += Button_Click1;
                            difference--;
                        }
                        tree.ElementAt(i).Value.Background = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
                        Canvas.SetTop(tree.ElementAt(i).Value, NumberForButtons[i].Item3);
                        Canvas.SetLeft(tree.ElementAt(i).Value, NumberForButtons[i].Item1);
                        Table.Children.Add(tree.ElementAt(i).Value);
                    }
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var but = (BlockButton)sender;
            MessageBox.Show(String.Format("StartTime: {0}\nEndTime: {1}\nDescription: {2}",but.StartTime,but.EndTime,but.Content));
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
