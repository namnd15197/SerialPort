using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort
{
    public class ONOFFConverter : BaseValueConverter<ONOFFConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null)
            {
                if ((bool)value) return "ON";
                else return "OFF";
            }
            else
            {
                if (parameter.ToString() == "Progressbar")
                {
                    if ((bool)value) return 100;
                    else return 0;
                }
                else return value;
                
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
