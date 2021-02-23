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
using System.Windows.Shapes;
using ScheduleWidget.ViewModels;
using System.Text.RegularExpressions;

namespace ScheduleWidget.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для SelectWindow.xaml
    /// </summary>
    public partial class SelectWindow : Window
    {
        public SelectWindow()
        {
            InitializeComponent();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
                this.DialogResult = true;
        }
        public int countElements
        {
            get { return int.Parse(elements.Text); }
        }
        public int countRows
        {
            get { return int.Parse(rows.Text); }
        }

        private void rows_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void elements_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
