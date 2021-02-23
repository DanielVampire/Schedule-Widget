using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ScheduleWidget.Models
{
    public class ModifyTickBar : TickBar, IDisposable
    {
        public bool isNumber { get; set; } = false;
        public ModifyTickBar()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected override void OnRender(DrawingContext dc)
        {
            Size size = new Size(base.ActualWidth, base.ActualHeight);
            int tickCount = (int)((this.Maximum - this.Minimum) / this.TickFrequency)+1 ;
            Double tickFrequencySize;
            // Calculate tick's setting
            tickFrequencySize = (size.Width * this.TickFrequency / (this.Maximum - this.Minimum));
            string text = "";
            FormattedText formattedText = null;
            // Draw each tick text
            double lastLine = 0;
            for (int i = 0; i <= tickCount; i++)
            {
                Pen pen = new Pen();
                pen.Thickness = 1;
                pen.Brush = new SolidColorBrush(Colors.Black);
                pen.Brush.Opacity = 1;
                if (i == 0)
                {
                    lastLine += 5;
                    dc.DrawLine(pen, new Point(5, 0), new Point(5, size.Height));

                }
                else
                {
                    lastLine += tickFrequencySize;
                    dc.DrawLine(pen, new Point(lastLine, 0), new Point(lastLine, size.Height));

                }
                if (isNumber)
                {
                    text = Convert.ToString(Convert.ToInt32(this.Minimum + this.TickFrequency * i), 10);
                    formattedText = new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
                    if(i==0)
                        dc.DrawText(formattedText, new Point(10, 0));
                    else
                        dc.DrawText(formattedText, new Point(lastLine + 5, 0));
                }
            }
        }
    }
}
