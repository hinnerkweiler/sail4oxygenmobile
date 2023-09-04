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
#if DEBUG
                Console.Write("Converting from: " + stringValue);
#endif
                string localNumberString = stringValue.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

#if DEBUG
                Console.Write(" replaced local grouping seperator:  "
                    + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator +
                    " with decimal seperator: "+ CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + " to string: " + localNumberString );
#endif

                if (double.TryParse(localNumberString, NumberStyles.Float, CultureInfo.CurrentCulture, out double result))
                {
#if DEBUG
                    Console.WriteLine(" parsed to double: " + result);
#endif
                    return result;
                }
            }
#if DEBUG
            Console.WriteLine("Converter Failed!");
#endif

            return Binding.DoNothing;
         
        }
    }
}

