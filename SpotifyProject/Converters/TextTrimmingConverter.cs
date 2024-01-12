using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SpotifyProject.Converters
{
    public class TextTrimmingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                int maxLines = parameter as int? ?? 2; // Số dòng tối đa, mặc định là 2
                string[] lines = text.Split('\n');
                return string.Join(Environment.NewLine, lines.Take(maxLines)) + (lines.Length > maxLines ? "..." : "");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
