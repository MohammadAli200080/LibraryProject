using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Library_Project.Resources.Classes
{
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        private static T _converter = null;

        public ConverterMarkupExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
    [ValueConversion(typeof(int), typeof(DataGridRow))]
    public class RowToIndexConv : ConverterMarkupExtension<RowToIndexConv>
    {
        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DataGridRow)
            {
                DataGridRow row = value as DataGridRow;
                return row.GetIndex() + 1;
            }
            return 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}