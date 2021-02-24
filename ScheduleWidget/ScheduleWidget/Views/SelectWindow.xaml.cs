using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace ScheduleWidget.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для SelectWindow.xaml
    /// </summary>
    public partial class SelectWindow : Window
    {
        #region public properties
        public int countElements
        {
            get { return int.Parse(elements.Text); }
        }
        public int countRows
        {
            get { return int.Parse(rows.Text); }
        }
        #endregion

        #region public constructor
        public SelectWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region private events

        /*
         * ивент нажатия на кнопку "Accept"
         */
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /*
         * ивент при печати текста в поле rows
         */
        private void rows_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //регулярное выражение для проверки ввода цифр
            //то есть в данное поле будут записываться только цифры
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /*
         * ивент при печати текста в поле elements
         */
        private void elements_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //регулярное выражение для проверки ввода цифр
            //то есть в данное поле будут записываться только цифры
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}
