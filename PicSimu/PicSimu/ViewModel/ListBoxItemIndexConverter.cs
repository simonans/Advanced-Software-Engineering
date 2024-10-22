using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PicSimu.ViewModel
{
    public class ListBoxItemIndexConverter : IValueConverter
    {
        private static ListBoxItemIndexConverter _instance;

        public static ListBoxItemIndexConverter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ListBoxItemIndexConverter();
                }
                return _instance;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = value as ListBoxItem;
            if (item != null)
            {
                ListBox listBox = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
                if (listBox != null)
                {
                    return listBox.ItemContainerGenerator.IndexFromContainer(item);
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
