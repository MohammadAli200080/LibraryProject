using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Library_Project.Resources.Classes
{
    public class CellColorConvertor : IValueConverter
    {
        /// <summary>
        /// a method for converting background color of a cell in datagrid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>a brush if string contains گذشته then it returns Red color else green</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value.ToString();
            if (input.Contains("گذشته"))
            {
                var bc = new BrushConverter();
                return (Brush)bc.ConvertFrom("#FFD94800");
            }
            else
            {
                var bc = new BrushConverter();
                return (Brush)bc.ConvertFrom("#FF43AD03");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
