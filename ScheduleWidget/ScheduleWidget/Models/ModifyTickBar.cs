using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ScheduleWidget.Models
{
    //наследуемый класс от TickBar для переопределения метода отрисовки тиков
    public class ModifyTickBar : TickBar, IDisposable
    {
        #region public properties
        public bool isNumber { get; set; } = false;
        #endregion

        #region public constructor
        public ModifyTickBar()
        {

        }
        #endregion

        #region protected methods
        protected override void OnRender(DrawingContext dc)
        {
            //Размер слайдера
            Size size = new Size(base.ActualWidth, base.ActualHeight);
            //количество делений
            int tickCount = (int)((this.Maximum - this.Minimum) / this.TickFrequency) + 1;
            //расстояние между делениями
            Double tickFrequencySize = (size.Width * this.TickFrequency / (this.Maximum - this.Minimum));
            FormattedText formattedText = null;
            //позиция последнего деления
            double lastLine = 0;
            for (int i = 0; i <= tickCount; i++)
            {
                //настройка карандаша
                Pen pen = new Pen();
                pen.Thickness = 1;
                pen.Brush = new SolidColorBrush(Colors.Black);
                pen.Brush.Opacity = 1;
                //если первое деление
                if (i == 0)
                {
                    lastLine += 5;
                    dc.DrawLine(pen, new Point(5, 0), new Point(5, size.Height));

                }
                //иначе
                else
                {
                    lastLine += tickFrequencySize;
                    dc.DrawLine(pen, new Point(lastLine, 0), new Point(lastLine, size.Height));

                }
                //Если нужно нарисовать цифры
                if (isNumber)
                {
                    string text = Convert.ToString(Convert.ToInt32(this.Minimum + this.TickFrequency * i), 10);
                    //настраиваем текст
                    formattedText = new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
                    //если текст для первого деления
                    if (i == 0)
                        dc.DrawText(formattedText, new Point(10, 0));
                    //иначе
                    else
                        dc.DrawText(formattedText, new Point(lastLine + 5, 0));
                }
            }
        }
        #endregion

        #region public methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
