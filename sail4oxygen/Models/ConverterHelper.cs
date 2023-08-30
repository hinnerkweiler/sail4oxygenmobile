using System;
using System.Globalization;

namespace sail4oxygen.Models
{
	public class ConverterHelper : IValueConverter
	{
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string stringValue)
                {
                    if (double.TryParse(stringValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("en-US"), out double result))
                    {
                        return result;
                    }
                }

                return Binding.DoNothing;
            }
        }
}

