using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PicSimu.ViewModel
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Überprüfe, ob der Wert vom richtigen Typ ist
            if (value is double)
            {
                // Konvertiere den double-Wert in einen string
                double doubleValue = (double)value;
                return doubleValue.ToString(culture); // Hier könntest du auch ein bestimmtes Format angeben, z.B. "N2" für zwei Dezimalstellen
            }

            // Wenn der Wert nicht vom richtigen Typ ist, gib einfach den Wert zurück
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                // Konvertiere den double-Wert in einen string
                return doubleValue.ToString(culture);
            }

            // Wenn der Wert nicht vom richtigen Typ ist, gib einfach den Wert zurück
            return value;
        }
    }

}
