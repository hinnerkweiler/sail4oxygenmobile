using System;
using System.Globalization;

namespace sail4oxygen.Models
{
	public class ConverterHelper : IValueConverter
	{
        /// <summary>
        /// return whatever value is currently stored
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// take the string value and parse it to a double.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (double.TryParse(stringValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }
            }

            return Binding.DoNothing;
        }
    }
}

