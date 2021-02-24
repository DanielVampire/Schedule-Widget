using Bitlush;
using ScheduleWidget.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ScheduleWidget.ViewModels
{
    public class MainWindowModel : INotifyPropertyChanged, IDisposable
    {
        #region private properties
        private AvlTree<int, BlockButton> tree;
        private int countRows { get; set; } //количество строк в таблице
        private int countElements { get; set; } //количество элементов в таблице
        private List<Tuple<int, int, int>> numberForButtons;
        #endregion

        #region public properties
        public event PropertyChangedEventHandler PropertyChanged;

        public AvlTree<int, BlockButton> Tree
        {
            get { return tree; }
            set
            {
                tree = value;
                OnPropertyChanged("Tree");
            }
        }

        public int CountRows
        {
            get { return countRows; }
            set
            {
                countRows = value;
                OnPropertyChanged("CountRows");
            }
        }

        public int CountElements
        {
            get { return countElements; }
            set
            {
                countElements = value;
                OnPropertyChanged("CountRows");
            }
        }

        public List<Tuple<int, int, int>> NumberForButtons
        {
            get { return numberForButtons; }
            set
            {
                numberForButtons = value;
                OnPropertyChanged("NumberForButtons");
            }
        }
        #endregion

        #region public constructor
        public MainWindowModel()
        {
            countRows = 0;
            countElements = 0;
            tree = new AvlTree<int, BlockButton>();
        }
        #endregion

        #region public methods
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
         * Метод для генерации случайных чисел
         *  Width - ширина таблицы
         */
        public void GeneratorNumbers(double Width)
        {
            int count = CountElements;

            //список для сгенерированных чисел
            List<Tuple<int, int, int>> numbers = new List<Tuple<int, int, int>>();

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            while (count != 0)
            {

                bool isNew = true;

                //Генерируем случайные числа для точки начала элемента, точки конца элемента и 
                //числа строки, на которую его необходимо расположить
                int startTime = rand.Next(0, Convert.ToInt32(Width - 12));

                int endTime = rand.Next(startTime + 10, Convert.ToInt32(Width) - 2);

                int row = rand.Next(0, CountRows) * 40;


                if (numbers.Count == 0)
                    numbers.Add(new Tuple<int, int, int>(startTime, endTime, row));

                else
                {
                    //основная проверка случайных чисел. Все элементы должны занимать разные позиции
                    //и не накладываться друг на друга
                    //Для этого делаю следующую проверку. Если начальная точка нового объекта меньше, 
                    //чем точка уже сгенерированного объекта, при этом новый и существующий объект должны находится в одной строке,
                    //то запускаем цикл, берем начальную точку нового объекта и двигаемся к конечной точки этого объекта.
                    //Если на пути было пересечение с другим объектом, отметаем данные, так как они не подходят.
                    //Аналогично для ситуаций, когда новый объект сгенерирует начальную точку в середине уже существующего элемента.
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
            NumberForButtons = numbers;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
