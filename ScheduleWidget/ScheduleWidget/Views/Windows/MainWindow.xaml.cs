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

namespace ScheduleWidget
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double Vert { get; set; } = 0;
        double Horz { get; set; } = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void slide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double xValRatio = slide.Value / (slide.Maximum - slide.Minimum); //gets % the thumb is across the slider
            double thumbPos = xValRatio * (slide.ActualWidth); //gets the absolute position
            line.Visibility = Visibility.Visible;
            triangle.Visibility = Visibility.Visible;
            PointCollection points = new PointCollection();
            points.Add(new Point(thumbPos - 2, 11));
            points.Add(new Point(thumbPos - 2, Scroll.ViewportHeight));
            points.Add(new Point(thumbPos + 2, Scroll.ViewportHeight));
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
            slide.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            panel.Margin = new Thickness(e.HorizontalOffset, e.VerticalOffset, 0, -e.VerticalOffset);
            line.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
            triangle.Margin = new Thickness(0, e.VerticalOffset, 0, -e.VerticalOffset);
        }

        private void GenerateBut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ты нажал на кнопку!\n Пока что генерации нет");
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
