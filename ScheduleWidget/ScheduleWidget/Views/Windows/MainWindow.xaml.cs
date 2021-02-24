using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        #region private properties
        private MainWindowModel model;
        #endregion

        #region public constructor
        public MainWindow()
        {
            InitializeComponent();
            Date.Content = DateTime.Now.ToString("MMM dd ddd");
            model = new MainWindowModel();
        }
        #endregion

        #region private events
        /*
         * Ивент при изменении значения слайдера.
         * В месте нажатия мышки по области слайдера - рисует полоску
         */
        private void slide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double xValRatio = (slide.Value+0.45) / (slide.Maximum - slide.Minimum); // % поля, который прошел ползунок
            double thumbPos = xValRatio * (slide.ActualWidth); // абсолютная позиция ползунка

            line.Visibility = Visibility.Visible;
            triangle.Visibility = Visibility.Visible;

            //Создание точек для линии
            PointCollection points = new PointCollection();
            points.Add(new Point(thumbPos - 2, 11));
            points.Add(new Point(thumbPos - 2, Scroll.ViewportHeight-1));
            points.Add(new Point(thumbPos + 2, Scroll.ViewportHeight-1));
            points.Add(new Point(thumbPos + 2, 11));
            line.Points = points;

            //Создание точек для треугольника
            points = new PointCollection();
            points.Add(new Point(thumbPos - 10, 1));
            points.Add(new Point(thumbPos, 13));
            points.Add(new Point(thumbPos + 10, 1));
            triangle.Points = points;
        }

        /*
         * Ивент изменения значения у прокрутки
         * Используется для того, чтобы поле с линейкой и 
         * информацией находилось сверху, когда происходит прокрутика
         */
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sliderPanel.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            panel.Margin = new Thickness(e.HorizontalOffset, e.VerticalOffset, 0, -e.VerticalOffset);
            line.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            triangle.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
        }

        /*
         * ивент нажатия на кнопку генерации
         */
        async private void GenerateBut_Click(object sender, RoutedEventArgs e)
        {
            //вызываю диалоговое окно
            SelectWindow select = new SelectWindow();
            select.Owner = this;
            if (select.ShowDialog() == true)
            {
                //записываю введенные данные
                model.CountElements = select.countElements;
                model.CountRows = select.countRows;
                Table.Height = Convert.ToDouble(model.CountRows * 40);
            }
            else
                return;
            await Task.Run(() =>
            {
                //запускается метод генерации чисел в отдельном потоке. Основной поток ждет
                model.GeneratorNumbers(Table.ActualWidth);
            });
            //Метод создания объектов в таблице, используя сгенерированные числа
            CreateButtons();
        }

        /*
         * ивент нажатия на объект в таблице
         */
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var but = (BlockButton)sender;
            MessageBox.Show(String.Format("StartTime: {0}\nEndTime: {1}\nDescription: {2}", but.StartTime, but.EndTime, but.Content));
        }

        /*
         * ивент перетаскивания главного окна (верхняя панель)
         */
        private void frame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /*
         * ивент кнопки, закрывающей приложение
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region private methods

        /*
         * Метод для добавления объектов в таблице
         */
        private void CreateButtons()
        {
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            //проверка на существующие объекты
            if (Table.Children.Count == 0)
            {
                int treeIndex = 0;

                //цикл, по которому сгенерированные данные добавляются в кнопку и размещаются в таблице
                foreach (var data in model.NumberForButtons)
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
                    model.Tree.Insert(treeIndex, button);

                    //задаем верхнюю левую точку объекта на полотне
                    Canvas.SetTop(model.Tree.Last().Value, data.Item3);
                    Canvas.SetLeft(model.Tree.Last().Value, data.Item1);

                    Table.Children.Add(model.Tree.Last().Value);
                    treeIndex++;
                }
            }
            else
            {
                //Если объекты уже существуют
                Table.Children.Clear();

                //количетсво существующих объектов
                int countExistElem = model.Tree.Count();
                //количество новых объектов
                int countNewElem = model.NumberForButtons.Count;
                //разница между количеством существующих и новых элементов
                int difference = countNewElem - countExistElem <= 0 ? 0 : countNewElem - countExistElem;

                //если новых объектов больше
                if (countExistElem < countNewElem)
                {
                    //добавляем недостающее количество в дерево
                    while(countExistElem != countNewElem)
                    {
                        model.Tree.Insert(countExistElem, new BlockButton());
                        countExistElem++;
                    }
                }
                //иначе
                else if(countExistElem > countNewElem)
                {
                    //удаляем из дерева лишнии
                    while(countExistElem != countNewElem)
                    {
                        countExistElem--;
                        model.Tree.Delete(countExistElem);
                    }
                }
                //цикл, по которому идет переопределение данных и вывод объектов в таблицу
                for (int i = 0; i < model.Tree.Count(); i++)
                {
                    model.Tree.ElementAt(i).Value.Content = rand.Next(0, 5000);
                    model.Tree.ElementAt(i).Value.StartTime = model.NumberForButtons[i].Item1;
                    model.Tree.ElementAt(i).Value.Width = model.NumberForButtons[i].Item2 - model.NumberForButtons[i].Item1;
                    model.Tree.ElementAt(i).Value.EndTime = model.NumberForButtons[i].Item2;
                    model.Tree.ElementAt(i).Value.Row = model.NumberForButtons[i].Item3;
                    model.Tree.ElementAt(i).Value.Height = 40;
                    model.Tree.ElementAt(i).Value.Opacity = 0.7d;
                    model.Tree.ElementAt(i).Value.BorderThickness = new Thickness(1, 1, 1, 1);
                    model.Tree.ElementAt(i).Value.EndTime = model.NumberForButtons[i].Item2;
                    model.Tree.ElementAt(i).Value.Row = model.NumberForButtons[i].Item3;

                    //проверка на новые объекты. Если в дерево нужно было добавить новые объекты, 
                    //то только для них нужно определить метод для ивента click
                    if (model.Tree.Count() - difference == i && difference != 0)
                    {
                        model.Tree.ElementAt(i).Value.Click += Button_Click1;
                        difference--;
                    }

                    model.Tree.ElementAt(i).Value.Background = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
                    //задаем верхнюю левую точку объекта на полотне
                    Canvas.SetTop(model.Tree.ElementAt(i).Value, model.NumberForButtons[i].Item3);
                    Canvas.SetLeft(model.Tree.ElementAt(i).Value, model.NumberForButtons[i].Item1);

                    Table.Children.Add(model.Tree.ElementAt(i).Value);
                    }
            }
        }
        #endregion
    }
}
