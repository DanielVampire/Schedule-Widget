using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScheduleWidget.Models
{
    public class BlockButton : Button, IDisposable, INotifyPropertyChanged
    {
        private int startTime { get; set; }
        private int endTime { get; set; }
        private int row { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BlockButton()
        {
        }
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
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
