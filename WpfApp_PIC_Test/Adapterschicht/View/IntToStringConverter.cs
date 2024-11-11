using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp_PIC.Adapterschicht.View;
public class IntToStringConverter : IValueConverter
{
    // Convert: Konvertiert den int-Wert in einen string
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue.ToString();  // Konvertiere den int-Wert in einen string
        }
        return string.Empty;  // Rückgabe eines leeren Strings, wenn der Wert kein int ist
    }

    // ConvertBack: Wird hier nicht benötigt, da wir nur den Wert von int nach string umwandeln.
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && int.TryParse(stringValue, out int result))
        {
            return result;  // Versuche, den string-Wert in einen int-Wert zu konvertieren
        }
        return 0;  // Rückgabe des Standardwerts (0) bei fehlerhafter Eingabe
    }
}

