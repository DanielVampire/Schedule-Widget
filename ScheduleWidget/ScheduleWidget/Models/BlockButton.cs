using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ScheduleWidget.Models
{
    //Наследуемый класс от Button для добавления нужных параметров
    public class BlockButton : Button, IDisposable, INotifyPropertyChanged
    {
        #region private properties
        private int startTime { get; set; }
        private int endTime { get; set; }
        private int row { get; set; }
        #endregion

        #region public properties
        public int Row
        {
            get { return row; }
            set
            {
                row = value;
                OnPropertyChanged("Row");
            }
        }
        public int StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        public int EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region public constructor
        public BlockButton()
        {

        }
        #endregion

        #region public methods
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
